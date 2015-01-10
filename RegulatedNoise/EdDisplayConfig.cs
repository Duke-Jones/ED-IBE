namespace RegulatedNoise
{
    [System.Xml.Serialization.XmlRoot("DisplayConfig")]
    public class EdDisplayConfig
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int FullScreen { get; set; }
    }
}