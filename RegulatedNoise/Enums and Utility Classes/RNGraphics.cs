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
        static public Bitmap adjustBitmap(Bitmap originalImage, float brightness = 1.0f, float contrast = 1.0f, float gamma = 1.0f)
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
        public static Bitmap makeGrayscaleBitmap(Bitmap original)
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
        public static Bitmap invertBitmap(Bitmap original)
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
        public static Bitmap changeColour(this Bitmap original, Color oldColor, Color newColor, int Tolerance = 0)
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
            PixelFormat pxf = PixelFormat.Format24bppRgb;

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

            // Manipulate the bitmap
            for (int counter = 0; counter < rgbValues.Length; counter += 3)
            {
                //Debug.Print(rgbValues[counter] + "," +rgbValues[counter+1] + "," + rgbValues[counter+2]);
                //if (rgbValues[counter] != 255 || rgbValues[counter+1] != 255 || rgbValues[counter+2] != 255)
                //    Debug.Print("1");
                if (((rgbValues[counter]   >= inColourB_min) && (rgbValues[counter]      <= inColourB_max)) &&
                    ((rgbValues[counter+1] >= inColourG_min) && (rgbValues[counter + 1]  <= inColourG_max)) &&
                    ((rgbValues[counter+2] >= inColourR_min) && (rgbValues[counter + 2]  <= inColourR_max)))
                    {
                        rgbValues[counter]      = outColourB;
                        rgbValues[counter + 1]  = outColourG;
                        rgbValues[counter + 2]  = outColourR;
                    }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Unlock the bits.
            newBitmap.UnlockBits(bmpData);

            return newBitmap;
        }
    }
}
