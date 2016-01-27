using System.Drawing;

namespace IBE
{
    [System.Xml.Serialization.XmlRoot("DisplayConfig")]
    public class EdDisplayConfig
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public Point Resolution
        {
            get
            {
                return new Point(ScreenWidth, ScreenHeight);
            }
        }

        public int FullScreen { get; set; }
    }
}