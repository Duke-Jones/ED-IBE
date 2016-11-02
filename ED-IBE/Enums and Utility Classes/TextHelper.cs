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
        

        public string FixedLength(String shortString, Font font, int fullLength)
        {
            Int32 spaceWidth = 0;

            if(!spaceWidthCache.TryGetValue(font, out spaceWidth))
            {
                spaceWidth = TextRenderer.MeasureText(" ", font).Width;
                spaceWidthCache.Add(font, spaceWidth);
            }

            return shortString.PadRight((Int32)Math.Round(((Double)fullLength - (Double)TextRenderer.MeasureText(shortString, font).Width) / (Double)spaceWidth, 0));
        }
    }
}
