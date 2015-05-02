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
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Enums_and_Utility_Classes;

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
        private EBPixeltest PixelTest;

        Bitmap _bTrimmedHeader, _bTrimmed_4_OCR, _bOriginal, _bOriginalClone, _bTrimmed_4_View;

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
            if (_bTrimmed_4_View != null) _bTrimmed_4_View.Dispose();
            if (_bTrimmed_4_OCR != null) _bTrimmed_4_OCR.Dispose();
            if (_bTrimmedHeader != null) _bTrimmedHeader.Dispose();

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

            // get the area of the commodity data
            var trim_4_CommodityArea = new Rectangle(_calibrationPoints[2].X ,  _calibrationPoints[2].Y,
                                                     _calibrationPoints[10].X - _calibrationPoints[2].X, 
                                                     _calibrationPoints[11].Y - _calibrationPoints[2].Y);

            // RNGraphics.Crop image to the commodity area
            _bTrimmed_4_OCR = RNGraphics.Crop(_bOriginalClone, trim_4_CommodityArea);

            // save the still colored area for viewing reasons
            _bTrimmed_4_View = (Bitmap)(_bTrimmed_4_OCR.Clone());

            // set all dark colors to black - this removes all crap
            _bTrimmed_4_OCR = RNGraphics.changeColour(_bTrimmed_4_OCR, Color.Black, Color.Black, Form1.RegulatedNoiseSettings.GUIColorCutoffLevel , RNGraphics.enPixelCompare.pc_RGB_all);

            // find automatically the textlines in the commodity area 
            var textRowLocations = new List<Tuple<int, int>>();
            var nextLine = 0;
            while (nextLine < _bTrimmed_4_OCR.Height - 1)
            {
                int startLine = -1, endLine = -1;
                for (int i = nextLine; i < _bTrimmed_4_OCR.Height - 1; i++)
                {
                    nextLine = i + 1;
                    bool hasOrangePixel;
                    using (Bitmap singlePixelRow = RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(0, i, _bTrimmed_4_OCR.Width, 1)))
                    {
                        hasOrangePixel = RNGraphics.hasGUIColoredPixel(singlePixelRow);
                    }
                    //Debug.WriteLine(i + " of " + bTrimmed.Height + ": " + hasOrangePixel);
                    if (endLine == -1 && !hasOrangePixel)
                        startLine = i;
                    else
                        if (hasOrangePixel)
                            endLine = i + 1;
                        else
                            break;

                }
                if (startLine != -1 && endLine != -1)
                    textRowLocations.Add(new Tuple<int, int>(startLine, endLine));
            }

            // get the area of the header
            var trim_4_Header = new Rectangle(_calibrationPoints[0].X,   _calibrationPoints[0].Y,
                                              _calibrationPoints[10].X - _calibrationPoints[0].X, 
                                              _calibrationPoints[1].Y  - _calibrationPoints[0].Y);

            // RNGraphics.Crop image to the header area and preprocess for OCR
            _bTrimmedHeader = RNGraphics.PreprocessScreenshot(RNGraphics.Crop(_bOriginalClone, trim_4_Header),1, Form1.RegulatedNoiseSettings.GUIColorCutoffLevel);

            // now process screenshot for OCR and Elitebrainerous 
            _bTrimmed_4_OCR  = RNGraphics.PreprocessScreenshot(_bTrimmed_4_OCR,1, Form1.RegulatedNoiseSettings.GUIColorCutoffLevel);

            // show preprocessed parts on the GUI
            _callingForm.UpdateTrimmedImage(_bTrimmed_4_OCR, _bTrimmedHeader);

            int min=100, max=0;

            if (textRowLocations.Count > 0)
            {
                // check if the last line is complete or RNGraphics.Cropped -> if it's RNGraphics.Cropped we delete it
                var finalRowLocation = textRowLocations[textRowLocations.Count - 1];

                foreach (var x in textRowLocations)
                    if (x.Item1 != finalRowLocation.Item1)
                    {
                        if (min > x.Item2 - x.Item1) 
                            min = x.Item2 - x.Item1;

                        if (max < x.Item2 - x.Item1) 
                            max = x.Item2 - x.Item1;
                    }

                if (finalRowLocation.Item2 - finalRowLocation.Item1 < (min - 2))
                    textRowLocations.RemoveAt(textRowLocations.Count - 1);
 
            }

            Debug.Print("process screenshot " + screenshot);
            PerformOcr(textRowLocations);
        }

        public void PerformOcr(List<Tuple<int, int>> textRowLocations)
        {
            int DarkPixels;
            var conv = new BitmapToPixConverter();

            Pix p = conv.Convert(_bTrimmedHeader);

            string headerResult;

            // delete the old brainerous images - otherwise Brainerous will process older but not relevant images too
            if (Directory.Exists(@".\Brainerous\images"))
                foreach (string file in Directory.GetFiles(@".\\Brainerous\\images", "*.*"))
                    File.Delete(file);
            else
                Directory.CreateDirectory("./Brainerous/images");

            using (var engine = new TesseractEngine(@"./tessdata", Form1.RegulatedNoiseSettings.TraineddataFile, EngineMode.Default))
            {
                using (var page = engine.Process(p))
                {
                    var text = page.GetText();
                    headerResult = StripPunctuationFromScannedText(text);// (text + " {" + page.GetMeanConfidence() + "}\r\n");
                }
            }

            string[] StationsInSystem = _callingForm.myMilkyway.getStationNames(SystemAtTimeOfScreenshot);
            string headerResult_temp = StationsInSystem.FirstOrDefault(x => x.Equals(_callingForm.tbCurrentStationinfoFromLogs.Text, StringComparison.InvariantCultureIgnoreCase));

            if(headerResult_temp == null)
            { 
                // station not found in database
                var matchesInStationReferenceList = StationsInSystem.OrderBy(x => _levenshtein.LD2(headerResult, x)).ToList();

                if(matchesInStationReferenceList.Count > 0)
                {
                    var ld = _levenshtein.LD2(headerResult, matchesInStationReferenceList[0].ToUpper());
                
                    // this depends on the length of the word - this factor works really good
                    double LevenshteinLimit = Math.Round((matchesInStationReferenceList[0].Length * 0.7), 0);

                    if (ld <= LevenshteinLimit)
                        headerResult = matchesInStationReferenceList[0].ToUpper();
                }
            }
            else
            {
                headerResult = headerResult_temp;
            }

            // show station on GUI

            _callingForm.DisplayResults(headerResult);
            
            var commodityColumnText         = new string[textRowLocations.Count(), 8]; 
            var originalBitmaps             = new Bitmap[textRowLocations.Count(),8];
            var originalBitmapConfidences   = new float[textRowLocations.Count(), 8];
            var rowIds                      = new string[textRowLocations.Count()];
            var rowCtr                      = 0;
			
            var bitmapCtr = 0;

            foreach (var row in textRowLocations)
            {
                int startRow = row.Item1 - 3;
                int heightRow = row.Item2 - row.Item1 + 6;

                if (startRow < 0) 
                    startRow = 0;

                if (heightRow + startRow > _bTrimmed_4_OCR.Height) 
                    heightRow = _bTrimmed_4_OCR.Height - startRow;

                // We'll use this later to identify the right correction image
                rowIds[rowCtr] = Guid.NewGuid().ToString();
                using (Bitmap b = RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(0, startRow, _bTrimmed_4_OCR.Width, heightRow)))
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
                            // commodity
                            left = 0; width = _calibrationPoints[3].X - _calibrationPoints[2].X;
                            break;
                        case 1:
                            // sell
                            left = _calibrationPoints[3].X - _calibrationPoints[2].X; width = _calibrationPoints[4].X - _calibrationPoints[3].X;
                            break;
                        case 2:
                            //buy
                            left = _calibrationPoints[4].X - _calibrationPoints[2].X; width = _calibrationPoints[5].X - _calibrationPoints[4].X;
                            break;
                        case 3:
                            // freight
                            left = _calibrationPoints[5].X - _calibrationPoints[2].X; width = _calibrationPoints[6].X - _calibrationPoints[5].X;
                            break;
                        case 4:
                            // demand
                            left = _calibrationPoints[6].X - _calibrationPoints[2].X; width = _calibrationPoints[7].X - _calibrationPoints[6].X;
                            break;
                        case 5:
                            // demand level
                            left = _calibrationPoints[7].X - _calibrationPoints[2].X; width = _calibrationPoints[8].X - _calibrationPoints[7].X;
                            break;
                        case 6:
                            // supply 
                            left = _calibrationPoints[8].X - _calibrationPoints[2].X; width = _calibrationPoints[9].X - _calibrationPoints[8].X;
                            break;
                        case 7:
                            // supply level
                            left = _calibrationPoints[9].X - _calibrationPoints[2].X; width = _calibrationPoints[10].X - _calibrationPoints[9].X;
                            break;
                        default:
                            left = 0; width = _calibrationPoints[3].X - _calibrationPoints[2].X;
                            break;
                    }
                    var fudgeFactor = 0;// _bOriginal.Height * 6 / 1440;
                    left = left + fudgeFactor;
                    width = width - fudgeFactor;

                    DarkPixels = 0;

                    if (_callingForm.cbCheckAOne.Checked)
                    {
                        if (PixelTest == null)
                            PixelTest = new EBPixeltest();

                        if (columnCounter == 3)
                        {
                            var brainerousOut = RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left, startRow, width, heightRow));

                            // check how much dark pixels are on the bitmap
                            for (int i = 0; i < brainerousOut.Height; i++)
                                for (int j = 0; j < brainerousOut.Width; j++)
                                    if (brainerousOut.GetPixel(j, i).GetBrightness() < Form1.RegulatedNoiseSettings.EBPixelThreshold)
                                        DarkPixels++;

                            PixelTest.addPicture(brainerousOut, DarkPixels);
                        }
                    }
                    else
                    {
                        //  RNGraphics.Crop a little bit more form the left border because sometimes if theres 
                        // the line of the table it was recognized as "1" or "7"
                        left += 10;
                        width -= 10;

                        if (columnCounter != 0 && columnCounter != 5 && columnCounter != 7)
                        {   //If it's a numeric column write it out for Brainerous to process later
                            var brainerousOut = RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left, startRow, width, heightRow));

                            if (Form1.RegulatedNoiseSettings.EBPixelAmount > 0)
                            {
                                // check how much dark pixels are on the bitmap -> we process only bitmaps 
                                // with something on it (minimum one digit supposed, a "1" hat about 25 pixels in default 1920x1200)
                                for (int i = 0; i < brainerousOut.Height; i++)
                                    for (int j = 0; j < brainerousOut.Width; j++)
                                        if (brainerousOut.GetPixel(j, i).GetBrightness() < Form1.RegulatedNoiseSettings.EBPixelThreshold)
                                            DarkPixels++;
                            }

                            if (DarkPixels >= Form1.RegulatedNoiseSettings.EBPixelAmount)
                                brainerousOut.Save("./Brainerous/images/" + bitmapCtr + ".png");

                            bitmapCtr++;
                        }
                        else
                        {   // It's a text column, we'll use Tesseract

                            // Prepare some different versions of the bitmap, we will take the best result
                            var c = new Bitmap[7];
                            c[0] = (RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left, startRow, width, heightRow)));
                            c[1] = (RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left + 1, startRow, width, heightRow)));
                            c[2] = (RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left - 1, startRow, width, heightRow)));
                            c[3] = (RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left, startRow - 1, width, heightRow)));
                            c[4] = (RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left + 1, startRow - 1, width, heightRow)));
                            c[5] = (RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left - 1, startRow - 1, width, heightRow)));
                            c[6] = (RNGraphics.Crop(_bTrimmed_4_OCR, new Rectangle(left, startRow + 2, width, heightRow - 2)));

                            var t = new string[c.Length];
                            var cf = new float[c.Length];

                            using (var engine = new TesseractEngine(@"./tessdata", Form1.RegulatedNoiseSettings.TraineddataFile, EngineMode.Default))
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
                    }

                    columnCounter++;
                }
                rowCtr++;
            }

            if (_callingForm.cbCheckAOne.Checked)
            {
                PixelTest.StartModal(_callingForm);

            }
            else
            {
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
                        var o4 = outputFromBrainerous.Substring(o2).IndexOf("./images", StringComparison.InvariantCultureIgnoreCase);

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

                    List<string> splitOutput = ((string[])outputFromBrainerous.Replace("\r", "").Split('\n')).ToList();

                    for (var i = 0; i < (textRowLocations.Count * 10); i += 2)
                    {
                        string Filename = (i / 2).ToString() + ".png";
                        if ((splitOutput.Count <= i) || (splitOutput[i].Length < 14) || (splitOutput[i].Substring(9) != Filename))
                        {
                            splitOutput.Insert(i, "./images/" + Filename);
                            splitOutput.Insert(i + 1, "");
                        }
                    }


                    // Load the result from Brainerous into the OCR output
                    for (var i = 0; i < textRowLocations.Count; i++)
                    {
                        commodityColumnText[i, 1] = splitOutput[i * 10 + 1];
                        originalBitmaps[i, 1] = null;
                        originalBitmapConfidences[i, 1] = 1;
                        commodityColumnText[i, 2] = splitOutput[i * 10 + 3];
                        originalBitmaps[i, 2] = null;
                        originalBitmapConfidences[i, 2] = 1;
                        commodityColumnText[i, 3] = splitOutput[i * 10 + 5];
                        originalBitmaps[i, 3] = null;
                        originalBitmapConfidences[i, 3] = 1;
                        commodityColumnText[i, 4] = splitOutput[i * 10 + 7];
                        originalBitmaps[i, 4] = null;
                        originalBitmapConfidences[i, 4] = 1;
                        commodityColumnText[i, 6] = splitOutput[i * 10 + 9];
                        originalBitmaps[i, 6] = null;
                        originalBitmapConfidences[i, 6] = 1;

                    }
                }
            }

            _bOriginal.Dispose();
            _bOriginalClone.Dispose();

            if (_callingForm.cbCheckAOne.Checked)
            {
                _callingForm.setCheckbox(_callingForm.cbCheckAOne, false);
                Form1.InstanceObject.clearOcrOutput();
            }
            else
            {
                // Send the results for this screenshot back to the Form
                _callingForm.DisplayCommodityResults(commodityColumnText, originalBitmaps, originalBitmapConfidences, rowIds, CurrentScreenshot);
            }

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

    }
}
