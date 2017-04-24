using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace IBE.Enums_and_Utility_Classes
{
    public class TextHelper
    {
        private Dictionary<Font, Int32> spaceWidthCache = new Dictionary<Font, int>();
        

        public string FixedLength(String shortString, Font font, int fullLength, StringAlignment alignment = StringAlignment.Near)
        {
            Int32 spaceWidth = 0;
            // Declare a proposed size with dimensions set to the maximum integer value.
            Size proposedSize = new Size(int.MaxValue, int.MaxValue);
            Int32 textWidth = TextRenderer.MeasureText(shortString, font, proposedSize, TextFormatFlags.NoPadding).Width;
            String returnString = "";

            if(!spaceWidthCache.TryGetValue(font, out spaceWidth))
            {
                spaceWidth = TextRenderer.MeasureText(" ", font, proposedSize, TextFormatFlags.NoPadding).Width;
                spaceWidthCache.Add(font, spaceWidth);
            }

            switch (alignment)
            {
                case StringAlignment.Near:
                    returnString = shortString.PadRight((Int32)Math.Round(((Double)fullLength - (Double)textWidth) / (Double)spaceWidth, 0, MidpointRounding.AwayFromZero));
                    break;
                case StringAlignment.Far:
                    returnString = shortString.PadLeft((Int32)Math.Round(((Double)fullLength - (Double)textWidth) / (Double)spaceWidth, 0, MidpointRounding.AwayFromZero));
                    break;
                case StringAlignment.Center:
                    Int32 leftValue  = (Int32)Math.Round((((Double)fullLength - (Double)textWidth) / (Double)spaceWidth) / 2, 0, MidpointRounding.AwayFromZero);
                    Int32 rightValue = (Int32)Math.Round( ((Double)fullLength - (Double)textWidth) / (Double)spaceWidth     , 0, MidpointRounding.AwayFromZero) - leftValue;
                    returnString = shortString.PadLeft(leftValue);
                    returnString = returnString.PadRight(rightValue);
                    break;
            }
           
            return returnString;
        }
    }
}
