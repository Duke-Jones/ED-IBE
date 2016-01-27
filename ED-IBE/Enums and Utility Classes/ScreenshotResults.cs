using System.Drawing;

namespace RegulatedNoise
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
