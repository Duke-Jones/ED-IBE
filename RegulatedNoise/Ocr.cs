using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RegulatedNoise
{
    class Ocr
    {
        public string FolderPath { get; set; }
        public bool Working = false;
        public bool IsMonitoring { get; set; }
        public List<string> ScreenshotBuffer = new List<string>(); 
        public string CurrentScreenshot;
        public DateTime CurrentScreenshotDateTime;
        public string SystemAtTimeOfScreenshot;
        
        private readonly Form1 _callingForm;
        private Point[] _calibrationPoints;
        private readonly SingleThreadLogger _logger;
        private Levenshtein _levenshtein = new Levenshtein();
        private TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        Bitmap _bTrimmedHeader, _bTrimmed, _bOriginal, _bOriginalClone;

        public Ocr(Form1 callingForm)
        {
            _callingForm = callingForm;
            
            _logger = new SingleThreadLogger(ThreadLoggerType.Ocr);
        }

        public void ScreenshotCreated(string filePath, string systemAtTimeOfScreenshot)
        {
            SystemAtTimeOfScreenshot = systemAtTimeOfScreenshot;

            Debug.WriteLine("Screenshot created: OCR is "+Working);
            if (Working)
            {
                ScreenshotBuffer.Add(filePath);
                return;
            }
            Working = true;
            ProcessNewScreenshot(filePath);

        }

        private void ProcessNewScreenshot(string screenshot)
        {
            CurrentScreenshot = screenshot;
            CurrentScreenshotDateTime = File.GetCreationTime(CurrentScreenshot);

            if (_bOriginal != null) _bOriginal.Dispose();
            if (_bOriginalClone != null) _bOriginalClone.Dispose();

            // Well, we can get the bitmap without locking its file, like this... maybe it will help 
            using (Stream s = File.OpenRead(CurrentScreenshot))
                _bOriginal = (Bitmap)Bitmap.FromStream(s);

            using (Stream s = File.OpenRead(CurrentScreenshot))
                _bOriginalClone = (Bitmap)Bitmap.FromStream(s);

            Bitmap _bAnotherClone = null;
            // I apologise for what comes next.
            try
            {
                if(_bOriginal != null)
                    _bAnotherClone = (Bitmap) (_bOriginal.Clone());
            }
            catch (Exception ex)
            {
                _logger.Log("Ignoring _bOriginal.Clone() error...\r\n"+ex);
            }
            _calibrationPoints = _callingForm.UpdateOriginalImage(_bAnotherClone);

            var trim = new Rectangle(_calibrationPoints[2].X, _calibrationPoints[2].Y,
                _calibrationPoints[10].X - _calibrationPoints[2].X, _calibrationPoints[11].Y - _calibrationPoints[2].Y);

            if (_bTrimmed != null) _bTrimmed.Dispose();
            _bTrimmed = Crop(_bOriginalClone, trim);

            var textRowLocations = new List<Tuple<int, int>>();

            var nextLine = 0;

            while (nextLine < _bTrimmed.Height - 1)
            {
                int startLine = -1, endLine = -1;
                for (int i = nextLine; i < _bTrimmed.Height - 1; i++)
                {
                    nextLine = i + 1;


                    bool hasOrangePixel;

                    using (Bitmap singlePixelRow = Crop(_bTrimmed, new Rectangle(0, i, _bTrimmed.Width, 1)))
                    {
                        hasOrangePixel = HasOrangePixel(singlePixelRow);
                    }
                    //Debug.WriteLine(i + " of " + bTrimmed.Height + ": " + hasOrangePixel);
                    if (endLine == -1 && !hasOrangePixel)
                        startLine = i;
                    else
                    {
                        if (hasOrangePixel)
                            endLine = i + 1;
                        else
                            break;
                    }


                }
                if (startLine != -1 && endLine != -1)
                    textRowLocations.Add(new Tuple<int, int>(startLine, endLine));
            }




            trim = new Rectangle(_calibrationPoints[0].X, _calibrationPoints[0].Y,
                _calibrationPoints[10].X - _calibrationPoints[0].X, _calibrationPoints[1].Y - _calibrationPoints[0].Y);

            if(_bTrimmedHeader != null) _bTrimmedHeader.Dispose();

            _bTrimmedHeader = PreprocessScreenshot(Crop(_bOriginalClone, trim));

            _bTrimmed = PreprocessScreenshot(_bTrimmed);

            _callingForm.UpdateTrimmedImage(_bTrimmed, _bTrimmedHeader);

            int min=100, max=0;

            if (textRowLocations.Count < 1)
            {
                _logger.Log("No text row locations found...");
                _callingForm.GenericSingleParameterMessage(null, AppDelegateType.MaximizeWindow);
                MessageBox.Show(
                    "Couldn't find any text row locations to process.  Have you changed the UI somehow?  You might like to investigate the \"I've changed the UI colour\" button on the OCR Calibration tab...");
            }

            else
            {

                var finalRowLocation = textRowLocations[textRowLocations.Count - 1];

                foreach (var x in textRowLocations)
                {
                    if (x.Item1 != finalRowLocation.Item1)
                    {
                        if (min > x.Item2 - x.Item1) min = x.Item2 - x.Item1;
                        if (max < x.Item2 - x.Item1) max = x.Item2 - x.Item1;
                    }
                }

                if (finalRowLocation.Item2 - finalRowLocation.Item1 < (min - 2))
                {
                    textRowLocations.RemoveAt(textRowLocations.Count - 1);
                }
                //if(textRowLocations.ElementAt(textRowLocations.Count))
                //textRowLocations[textRow].
            }
            PerformOcr(textRowLocations);
        }

        public Bitmap PreprocessScreenshot(Bitmap b)
        {
			// tested with default ED gui setting
            // it's much!! better than v1.82 and I think better than v1.84, too

            b = Invert(b);
            b = MakeGrayscale(b);
            b = MakeBrighter(b, .20f);
            b = Contrast(b, 100);

            return b;
        }

        public void PerformOcr(List<Tuple<int, int>> textRowLocations)
        {
            var conv = new BitmapToPixConverter();

            Pix p = conv.Convert(_bTrimmedHeader);

            string headerResult;
			
            // using the "big.traineddata" from EliteOCR results in clearly better ocr
            using (var engine = new TesseractEngine(@"./tessdata", "big", EngineMode.Default))
            {
                using (var page = engine.Process(p))
                {
                    var text = page.GetText();
                    headerResult = StripPunctuationFromScannedText(text);// (text + " {" + page.GetMeanConfidence() + "}\r\n");
                }
            }

            var matchesInStationReferenceList =
                _callingForm.StationReferenceList.Where(x => x.System == SystemAtTimeOfScreenshot.ToUpper()).OrderBy(x => _levenshtein.LD2(headerResult, x.Name)).ToList();

            var q = _callingForm.StationReferenceList.Where(x => x.Name.Contains("'"));

            if(matchesInStationReferenceList.Count > 0)
            {
                var ld = _levenshtein.LD2(headerResult, matchesInStationReferenceList[0].Name.ToUpper());
                
                // this depends on the length of the word - this factor works really good
                double LevenshteinLimit = Math.Round((matchesInStationReferenceList[0].Name.Length * 0.7), 0);

                if (ld < LevenshteinLimit)
                    headerResult = matchesInStationReferenceList[0].Name.ToUpper();
            }

            _callingForm.DisplayResults(headerResult);
            
            var commodityColumnText = new string[textRowLocations.Count(), 8]; ;
            var originalBitmaps = new Bitmap[textRowLocations.Count(),8];
            var originalBitmapConfidences = new float[textRowLocations.Count(), 8];
            var rowIds = new string[textRowLocations.Count()];

            var rowCtr = 0;
			
			// don't do more "optimization" - it's the best I think. don't cchange it here again
            //var bTrimmedContrast = Contrast(MakeGrayscale(MakeBrighter((Bitmap)(_bTrimmed.Clone()),.25f)),60);
            var bTrimmedContrast = (Bitmap)_bTrimmed.Clone();

            var bitmapCtr = 0;
            
            foreach (var row in textRowLocations)
            {
                int startRow = row.Item1 - 3;
                int heightRow = row.Item2 - row.Item1 + 6;

                if (startRow < 0) startRow = 0;
                if (heightRow + startRow > bTrimmedContrast.Height) heightRow = bTrimmedContrast.Height - startRow;

                // We'll use this later to identify the right correction image
                rowIds[rowCtr] = Guid.NewGuid().ToString();
                using (Bitmap b = Crop(bTrimmedContrast, new Rectangle(0, startRow, bTrimmedContrast.Width, heightRow)))
                {
                    b.Save(".//OCR Correction Images//" + rowIds[rowCtr] + ".png");
                }

                int columnCounter = 0;
                while (columnCounter < 8)
                {
                    int left, width;
                    switch(columnCounter)
                    {
                        case 0:
                            left = 0; width = _calibrationPoints[3].X - _calibrationPoints[2].X;
                            break;
                        case 1:
                            left = _calibrationPoints[3].X - _calibrationPoints[2].X; width = _calibrationPoints[4].X - _calibrationPoints[3].X;
                            break;
                        case 2:
                            left = _calibrationPoints[4].X - _calibrationPoints[2].X; width = _calibrationPoints[5].X - _calibrationPoints[4].X;
                            break;
                        case 3:
                            left = _calibrationPoints[5].X - _calibrationPoints[2].X; width = _calibrationPoints[6].X - _calibrationPoints[5].X;
                            break;
                        case 4:
                            left = _calibrationPoints[6].X - _calibrationPoints[2].X; width = _calibrationPoints[7].X - _calibrationPoints[6].X;
                            break;
                        case 5:
                            left = _calibrationPoints[7].X - _calibrationPoints[2].X; width = _calibrationPoints[8].X - _calibrationPoints[7].X;
                            break;
                        case 6:
                            left = _calibrationPoints[8].X - _calibrationPoints[2].X; width = _calibrationPoints[9].X - _calibrationPoints[8].X;
                            break;
                        case 7:
                            left = _calibrationPoints[9].X - _calibrationPoints[2].X; width = _calibrationPoints[10].X - _calibrationPoints[9].X;
                            break;
                        default:
                            left = 0; width = _calibrationPoints[3].X - _calibrationPoints[2].X;
                            break;
                    }
                    var fudgeFactor = 0;// _bOriginal.Height * 6 / 1440;
                    left = left + fudgeFactor;
                    width = width - fudgeFactor;

                    if (columnCounter != 0 && columnCounter != 5 && columnCounter != 7)
                    {   //If it's a numeric column write it out for Brainerous to process later
                        var brainerousOut = Crop(bTrimmedContrast,
                            new Rectangle(left, startRow, width, heightRow));

                        if (!Directory.Exists("./Brainerous/images"))
                            Directory.CreateDirectory("./Brainerous/images");

                        brainerousOut.Save("./Brainerous/images/" + bitmapCtr + ".png");
                        bitmapCtr++;
                    }
                    else
                    {   // It's a text column, we'll use Tesseract

                        // Prepare some different versions of the bitmap, we will take the best result
                        var c = new Bitmap[7];
                        c[0] = (Crop(bTrimmedContrast, new Rectangle(left, startRow, width, heightRow)));
                        c[1] = (Crop(bTrimmedContrast, new Rectangle(left + 1, startRow, width, heightRow)));
                        c[2] = (Crop(bTrimmedContrast, new Rectangle(left - 1, startRow, width, heightRow)));
                        c[3] = (Crop(bTrimmedContrast, new Rectangle(left, startRow - 1, width, heightRow)));
                        c[4] = (Crop(bTrimmedContrast, new Rectangle(left + 1, startRow - 1, width, heightRow)));
                        c[5] = (Crop(bTrimmedContrast, new Rectangle(left - 1, startRow - 1, width, heightRow)));
                        c[6] = (Crop(bTrimmedContrast, new Rectangle(left, startRow + 2, width, heightRow - 2)));

                        var t = new string[c.Length];
                        var cf = new float[c.Length];

                        using (var engine = new TesseractEngine(@"./tessdata", "big", EngineMode.Default))
                        {
                            for (int i = 0; i < c.Length; i++)
                            {
                                t[i] = AnalyseFrameUsingTesseract((Bitmap)(c[i].Clone()), engine, out cf[i]);
                            }
                        }

                        int result = 0;
                        float confidence = cf[0];

                        for (int i = 1; i < c.Length; i++)
                        {
                            if (confidence < cf[i])
                            { result = i; confidence = cf[i]; }
                        }

                        originalBitmaps[rowCtr, columnCounter] = (Bitmap)(c[result].Clone());

                        switch (columnCounter)
                        {
                            //bodges for number columns
                            case 1:
                            case 2:
                            case 3:
                                t[result] = t[result].Replace(" ", "").Replace("O", "0").Replace("I", "1").Replace("'", "");
                                t[result] = System.Text.RegularExpressions.Regex.Replace(t[result], @"[a-zA-Z\s]+", string.Empty); // remove any alphas that remain
                                break;
                            case 5:
                            case 7:
                                t[result] = t[result].Replace(" ", "").Replace("-", "");
                                if (t[result] == "HIGH" || t[result] == "MED" || t[result] == "LOW")
                                {
                                    cf[result] = 1;
                                }
                                break;
                        }
                        if ((columnCounter == 5 && t[result].Contains("ENTER")) ||
                            (columnCounter == 6 && (t[result].Contains("NGAR") || t[result].Contains("SURFACE"))))
                        {
                            t[result] = "";
                            cf[result] = 1;
                        }
                        commodityColumnText[rowCtr, columnCounter] += t[result];
                        originalBitmapConfidences[rowCtr, columnCounter] = cf[result];
                    }

                    columnCounter++;
                }
                rowCtr++;
            }

            if (textRowLocations.Count > 0)
            {
                // Call out to Brainerous to process the numeric bitmaps we saved earlier
                var outputFromBrainerous = "";
                var pr = new Process();
                pr.StartInfo.UseShellExecute = false;
                pr.StartInfo.CreateNoWindow = true;
                pr.StartInfo.RedirectStandardOutput = true;
                pr.StartInfo.FileName = "./Brainerous/nn_training.exe";
                pr.StartInfo.WorkingDirectory = "./Brainerous/";
                pr.Start();
                outputFromBrainerous = pr.StandardOutput.ReadToEnd();
                while (outputFromBrainerous.Contains("Failed to pad successfully"))
                {
                    var o2 = outputFromBrainerous.IndexOf("Failed to ");
                    var o3 = outputFromBrainerous.Substring(0, o2);
                    var o4 = outputFromBrainerous.Substring(o2).IndexOf("./images");

					// I had a string with "Failed to pad successfully" and only some trash behind but no "./images"
                    // so "o4" was "-1" and this results in strange behaviour
                    if (o4 > 0)
                    {
                    var o5 = outputFromBrainerous.Substring(o2 + o4);
                    outputFromBrainerous = o3 + "\r\n" + o5;
                }
                    else
                    {
                        outputFromBrainerous = o3; 
                    }
                }

                pr.WaitForExit();

                var splitOutput = outputFromBrainerous.Replace("\r", "").Split('\n');

                // Load the result from Brainerous into the OCR output
                for (var i = 0; i < textRowLocations.Count; i++)
                {
                    commodityColumnText[i, 1] = splitOutput[i*10 + 1];
                    originalBitmaps[i, 1] = null;
                    originalBitmapConfidences[i, 1] = 1;
                    commodityColumnText[i, 2] = splitOutput[i*10 + 3];
                    originalBitmaps[i, 2] = null;
                    originalBitmapConfidences[i, 2] = 1;
                    commodityColumnText[i, 3] = splitOutput[i*10 + 5];
                    originalBitmaps[i, 3] = null;
                    originalBitmapConfidences[i, 3] = 1;
                    commodityColumnText[i, 4] = splitOutput[i*10 + 7];
                    originalBitmaps[i, 4] = null;
                    originalBitmapConfidences[i, 4] = 1;
                    commodityColumnText[i, 6] = splitOutput[i*10 + 9];
                    originalBitmaps[i, 6] = null;
                    originalBitmapConfidences[i, 6] = 1;
                }
            }
            _bOriginal.Dispose();
            _bOriginalClone.Dispose();
            // Send the results for this screenshot back to the Form
            _callingForm.DisplayCommodityResults(commodityColumnText, originalBitmaps, originalBitmapConfidences, rowIds, CurrentScreenshot);

            // ...and if we've got any buffered screenshots waiting to be processed, process the next one
            if (ScreenshotBuffer.Count > 0)
            {
                var screenshot = ScreenshotBuffer[0];
                ScreenshotBuffer.Remove(screenshot);
                ProcessNewScreenshot(screenshot);
            }

            Working = false;

            Debug.WriteLine("set to " + Working);
        }

        private string StripPunctuationFromScannedText(string input)
        {
            return _textInfo.ToUpper(input.Replace("\n\n", "").Replace("-", "").Replace(".", "").Replace(",", ""));
        }

        private static string AnalyseFrameUsingTesseract(Bitmap c1, TesseractEngine engine, out float cf1)
        {
            var conv = new BitmapToPixConverter();
            var p = conv.Convert(c1);
            string t1;
            using (var page = engine.Process(p))
            {
                t1 = page.GetText().Replace("\n\n", "").Replace(".", "").Replace(",", "");
                cf1 = page.GetMeanConfidence();
            }

            return t1;
        }

        #region Image-Processing Utilities
        public Bitmap Crop(Bitmap b, Rectangle r)
        {
         // From http://stackoverflow.com/questions/734930/how-to-crop-an-image-using-c
            if (r.Width < 1 || r.Height < 1)
                Debug.WriteLine("Yikes!");
            var nb = new Bitmap(r.Width, r.Height);
            var g = Graphics.FromImage(nb);
            g.DrawImage(b, -r.X, -r.Y);
            return nb;
        }

        #region Deleted but might be useful
        //private unsafe int GetAverageBitmapColor(Bitmap bm)
        //{
        //    var srcData = bm.LockBits(
        //    new Rectangle(0, 0, bm.Width, bm.Height),
        //    ImageLockMode.ReadOnly,
        //    PixelFormat.Format32bppArgb);
        //
        //    int stride = srcData.Stride;
        //
        //    IntPtr Scan0 = srcData.Scan0;
        //
        //    long[] totals = new long[] { 0, 0, 0 };
        //
        //    long width = bm.Width;
        //    long height = bm.Height;
        //
        //    unsafe
        //    {
        //        byte* p = (byte*)(void*)Scan0;
        //
        //        for (int y = 0; y < height; y++)
        //        {
        //            for (int x = 0; x < width; x++)
        //            {
        //                for (int color = 0; color < 3; color++)
        //                {
        //                    int idx = (y * stride) + x * 4 + color;
        //
        //                    totals[color] += p[idx];
        //                }
        //            }
        //        }
        //    }
        //
        //    int avgB = (int)(totals[0] / (width * height));
        //    int avgG = (int)(totals[1] / (width * height));
        //    int avgR = (int)(totals[2] / (width * height));
        //    return avgR + avgG + avgB / 3;
        //}
        #endregion

        // well, i guess it might not be orange any more...
        private unsafe bool HasOrangePixel(Bitmap bm)
        {
            BitmapData srcData = bm.LockBits(
            new Rectangle(0, 0, bm.Width, bm.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

            var stride = srcData.Stride;

            var scan0 = srcData.Scan0;

            long width = bm.Width;
            long height = bm.Height;

            var p = (byte*)(void*)scan0;

            var uiColour = Form1.RegulatedNoiseSettings.UiColour;
            int red = int.Parse(uiColour.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(uiColour.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(uiColour.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            int lowRed = red - 64 < 0 ? 0 : red - 64;
            int lowGreen = green - 64 < 0 ? 0 : green - 64;
            int lowBlue = blue - 64 < 0 ? 0 : blue - 64;
            int hiRed = red + 64 > 255 ? 255 : red + 64;
            int hiGreen = green + 64 > 255 ? 255 : green + 64;
            int hiBlue = blue + 64 > 255 ? 255 : blue + 64;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idxB = 0, idxG = 0, idxR = 0;


                    for (int colorIdx = 0; colorIdx < 3; colorIdx++)
                    {
                        int idx = (y * stride) + x * 4 + colorIdx;
                        switch (colorIdx)
                        {
                            case 0:
                                idxB = p[idx];
                                break;
                            case 1:
                                idxG = p[idx];
                                break;
                            case 2:
                                idxR = p[idx];
                                break;
                        }
                    }



                    // 0 = Blue, 1 = Green, 2= Red
                    //if (idxB <= idxR / 2.5 && idxG >= idxR / 3.5 && idxG <= idxR / 1.8 && idxR >= 120)
                    if(idxB >= lowBlue && idxB <= hiBlue && idxG >= lowGreen && idxG <= hiGreen && idxR >= lowRed && idxR <= hiRed)
                    {
                        //System.Diagnostics.Debug.WriteLine("Pixel "+ x);
                        return true;
                    }
                }
            }

            return false;
        }

        public static Bitmap MakeGrayscale(Bitmap original)
        {
        // From http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
            //create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            var g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new [] 
      {
         new [] {.3f, .3f, .3f, 0, 0},
         new [] {.59f, .59f, .59f, 0, 0},
         new [] {.11f, .11f, .11f, 0, 0},
         new [] {0, 0, 0, 1f, 0},
         new [] {0, 0, 0, 0, 1f}
      });
            //create some image attributes
            var attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static Bitmap MakeBrighter(Bitmap original, float amount) // .25, -.45
        {
            //create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            var g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            var colorMatrix = new ColorMatrix(
               new float[][] 
      {
         new float[] {1, 0, 0, 0, 0},
         new float[] {0, 1, 0, 0, 0},
         new float[] {0, 0, 1, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {-amount, -amount, -amount, 0, 1}
      });

            //create some image attributes
            var attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static Bitmap Invert(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
      {
         new float[] {-1, 0, 0, 0, 0},
         new float[] {0, -1, 0, 0, 0},
         new float[] {0, 0, -1, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {1, 1, 1, 0, 1}
      });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        // From http://softwarebydefault.com/2013/04/20/image-contrast/
        public Bitmap Contrast(Bitmap sourceBitmap, int threshold)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);
            double blue = 0;
            double green = 0;
            double red = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;

                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;

                red = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;

                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }
                
                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }

                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }

            var resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        } 
        #endregion
    }
}
