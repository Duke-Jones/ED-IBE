using System.Drawing;

namespace IBE
{
    public class ScreeenshotResults
    {
        public string[,] s;
        public Bitmap[,] originalBitmaps;
        public float[,] originalBitmapConfidences;
        public string screenshotName;
        public string[] rowIds;
    }
}
