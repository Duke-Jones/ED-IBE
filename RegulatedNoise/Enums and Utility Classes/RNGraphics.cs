using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    static class RNGraphics
    {
        /// <summary>
        /// change brightness, contrast and/or gamma of a bitmap
        /// (see http://stackoverflow.com/questions/15408607/adjust-brightness-contrast-and-gamma-of-an-image)
        /// </summary>
        /// <param name="originalImage">image to process</param>
        /// <param name="brightness">new brightness (1.0 = no changes, 0.0 to 2.0)</param>
        /// <param name="contrast">new contrast (1.0 = no changes)</param>
        /// <param name="gamma">new gamma (1.0 = no changes)</param>
        /// <returns></returns>
        static internal Bitmap adjustBitmap(Bitmap originalImage, float brightness = 1.0f, float contrast = 1.0f, float gamma = 1.0f)
        {
            Bitmap adjustedImage;
            ImageAttributes imageAttributes;
            Graphics g;
            float adjustedBrightness;

            adjustedBrightness = brightness - 1.0f;

            // create matrix that will brighten and contrast the image
            float[][] ptsArray ={
                    new float[] {contrast, 0, 0, 0, 0},     // scale red
                    new float[] {0, contrast, 0, 0, 0},     // scale green
                    new float[] {0, 0, contrast, 0, 0},     // scale blue
                    new float[] {0, 0, 0, 1.0f, 0},         // don't scale alpha
                    new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

            imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);

            adjustedImage   = new Bitmap(originalImage.Width, originalImage.Height);
            g               = Graphics.FromImage(adjustedImage);

            g.DrawImage(originalImage, new Rectangle(0,0,adjustedImage.Width,adjustedImage.Height),
                        0,0,originalImage.Width,originalImage.Height,
                        GraphicsUnit.Pixel, imageAttributes);

            g.Dispose();

            return adjustedImage;
        }

        /// <summary>
        /// set bitmap to greyscale
        /// (see http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale)
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        internal static Bitmap makeGrayscaleBitmap(Bitmap original)
        {
            //create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            var g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
                new [] {
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

        /// <summary>
        /// inverts a bitmap
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        internal static Bitmap invertBitmap(Bitmap original)
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

    internal enum enPixelCompare
	{
        pc_RGB_all,
        pc_RGB_single,
        pc_Brightness
	}

        /// <summary>
        /// (see http://stackoverflow.com/questions/16346212/can-you-change-one-colour-to-another-in-an-bitmap-image
        /// and https://msdn.microsoft.com/en-GB/library/ms229672%28v=vs.90%29.aspx)
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="inColourR"></param>
        /// <param name="inColourG"></param>
        /// <param name="inColourB"></param>
        /// <param name="outColourR"></param>
        /// <param name="outColourG"></param>
        /// <param name="outColourB"></param>
        internal unsafe static Bitmap changeColour(this Bitmap original, Color oldColor, Color newColor, int Tolerance = 0,enPixelCompare CompareType = enPixelCompare.pc_RGB_all)
        {
            byte inColourR_max, inColourG_max, inColourB_max;
            byte inColourR_min, inColourG_min, inColourB_min;
            byte outColourR, outColourG, outColourB;
            int tempValue;

            //create a clone of the original
            Bitmap newBitmap = (Bitmap)original.Clone();

            tempValue = (int)oldColor.R + Tolerance;
            inColourR_max = (tempValue > 255) ? (byte)255 : (byte)tempValue ; 

            tempValue = (int)oldColor.G + Tolerance;
            inColourG_max = (tempValue > 255) ? (byte)255 : (byte)tempValue ; 

            tempValue = (int)oldColor.B + Tolerance;
            inColourB_max = (tempValue > 255) ? (byte)255 : (byte)tempValue ; 

            tempValue = (int)oldColor.R - Tolerance;
            inColourR_min = (tempValue < 0) ? (byte)0 : (byte)tempValue ; 

            tempValue = (int)oldColor.G - Tolerance;
            inColourG_min = (tempValue < 0) ? (byte)0: (byte)tempValue ; 

            tempValue = (int)oldColor.B - Tolerance;
            inColourB_min = (tempValue < 0) ? (byte)0 : (byte)tempValue ; 

            outColourR = newColor.R;
            outColourG = newColor.G;
            outColourB = newColor.B;

            // Specify a pixel format.
            PixelFormat pxf = original.PixelFormat;

            // Lock the bitmap's bits.
            Rectangle rect      = new Rectangle(0, 0, newBitmap.Width, newBitmap.Height);
            BitmapData bmpData  = newBitmap.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = newBitmap.Width * newBitmap.Height * 3; 
            int numBytes = bmpData.Stride * newBitmap.Height;
            byte[] rgbValues = new byte[numBytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);
            
            double newColorBrightness = (double)(newColor.R+newColor.R+newColor.B+newColor.G+newColor.G+newColor.G)/6.0;

            // Manipulate the bitmap
            for (int line = 0; line < bmpData.Height ; line += 1)
            {
                for (int pixel = 0; (pixel+3) < bmpData.Stride ; pixel += 3)
                {
                    int currentPixel = line * bmpData.Stride + pixel;

                    switch (CompareType)
                    {
                        case enPixelCompare.pc_RGB_all:
                            if (((rgbValues[currentPixel]     >= inColourB_min) && (rgbValues[currentPixel]      <= inColourB_max)) &&
                                ((rgbValues[currentPixel + 1] >= inColourG_min) && (rgbValues[currentPixel + 1]  <= inColourG_max)) &&
                                ((rgbValues[currentPixel + 2] >= inColourR_min) && (rgbValues[currentPixel + 2]  <= inColourR_max)))
                            {
                                rgbValues[currentPixel]      = outColourB;
                                rgbValues[currentPixel + 1]  = outColourG;
                                rgbValues[currentPixel + 2]  = outColourR;
                            }
                            break;
                        case enPixelCompare.pc_RGB_single:
                            if (((rgbValues[currentPixel]     >= inColourB_min) && (rgbValues[currentPixel]      <= inColourB_max)) ||
                                ((rgbValues[currentPixel + 1] >= inColourG_min) && (rgbValues[currentPixel + 1]  <= inColourG_max)) ||
                                ((rgbValues[currentPixel + 2] >= inColourR_min) && (rgbValues[currentPixel + 2]  <= inColourR_max)))
                            {
                                rgbValues[currentPixel]      = outColourB;
                                rgbValues[currentPixel + 1]  = outColourG;
                                rgbValues[currentPixel + 2]  = outColourR;
                            }
                            break;
                        case enPixelCompare.pc_Brightness:
                            double currentBrighness = (double)(rgbValues[currentPixel + 2]+rgbValues[currentPixel + 2]+
                                                             rgbValues[currentPixel]    +
                                                             rgbValues[currentPixel + 1]+rgbValues[currentPixel + 1]+rgbValues[currentPixel + 1])
                                                             /6.0;

                            if (Math.Abs(currentBrighness - newColorBrightness) <= Tolerance)
                            {
                                rgbValues[currentPixel]      = outColourB;
                                rgbValues[currentPixel + 1]  = outColourG;
                                rgbValues[currentPixel + 2]  = outColourR;
                            }
                            break;
                    }
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Unlock the bits.
            newBitmap.UnlockBits(bmpData);

            return newBitmap;
        }

        internal static Bitmap PreprocessScreenshot(Bitmap b, int Preset, int GUIColorCutoffLevel)
        {
			// tested with default ED gui setting
            // it's much!! better than v1.82 and I think better than v1.84, too
            switch (Preset)
            {
            	case 0:
                    b = Invert(b);
                    b = MakeGrayscale(b);
                    b = MakeBrighter(b, .20f);

                    //b= RNGraphics.adjustBitmap(b, 1.0f, 5.0f, 1.0f);
                    b = Contrast(b, 100);
            		
            		break;
	            case 1:

                    Color UIColor = Form1.RegulatedNoiseSettings.getUiColor();
                    Bitmap reserve = (Bitmap)b.Clone();

                    b = (Bitmap)reserve.Clone();

                    // set all dark colors to black
                    b = RNGraphics.changeColour(b, Color.Black, Color.Black, GUIColorCutoffLevel , RNGraphics.enPixelCompare.pc_RGB_all);

                    // set all GUI colors to white
                    b = RNGraphics.changeColour(b, UIColor, Color.White, 250, RNGraphics.enPixelCompare.pc_Brightness);

                    // invert the bitmap
                    b = RNGraphics.invertBitmap(b);

                    break;
            } 

            return b;
        }

        #region Image-Processing Utilities
        internal static Bitmap Crop(Bitmap b, Rectangle r)
        {
         // From http://stackoverflow.com/questions/734930/how-to-crop-an-image-using-c
            if (r.Width < 1 || r.Height < 1)
                Debug.WriteLine("Yikes!");
            var nb = new Bitmap(r.Width, r.Height, b.PixelFormat);
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
        internal static unsafe bool hasGUIColoredPixel(Bitmap bm)
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

            Color UIColor = Form1.RegulatedNoiseSettings.getUiColor();

            int lowRed      = UIColor.R - 64 < 0   ? 0   : UIColor.R - 64;
            int lowGreen    = UIColor.G - 64 < 0   ? 0   : UIColor.G - 64;
            int lowBlue     = UIColor.B - 64 < 0   ? 0   : UIColor.B - 64;
            int hiRed       = UIColor.R + 64 > 255 ? 255 : UIColor.R + 64;
            int hiGreen     = UIColor.G + 64 > 255 ? 255 : UIColor.G + 64;
            int hiBlue      = UIColor.B + 64 > 255 ? 255 : UIColor.B + 64;

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

        internal static Bitmap MakeGrayscale(Bitmap original)
        {
        // From http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
            //create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height, original.PixelFormat);

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

        internal static Bitmap MakeBrighter(Bitmap original, float amount) // .25, -.45
        {
            //create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height, original.PixelFormat);

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

        internal static Bitmap Invert(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height, original.PixelFormat);

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
        internal unsafe static Bitmap Contrast(Bitmap sourceBitmap, int threshold)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);
            double blue = 0;
            double green = 0;
            double red = 0;

            for (int line = 0; line < sourceData.Height ; line += 1)
            {
                for (int pixel = 0; (pixel+3) < sourceData.Stride ; pixel += 3)
                {
                    int currentPixel = line * sourceData.Stride + pixel;

                    blue = ((((pixelBuffer[currentPixel] / 255.0) - 0.5) *
                                contrastLevel) + 0.5) * 255.0;

                    green = ((((pixelBuffer[currentPixel + 1] / 255.0) - 0.5) *
                                contrastLevel) + 0.5) * 255.0;

                    red = ((((pixelBuffer[currentPixel + 2] / 255.0) - 0.5) *
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

                    pixelBuffer[currentPixel] = (byte)blue;
                    pixelBuffer[currentPixel + 1] = (byte)green;
                    pixelBuffer[currentPixel + 2] = (byte)red;
                }
            }

            var resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.PixelFormat);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, sourceBitmap.PixelFormat);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        } 

        #endregion
    }

}
