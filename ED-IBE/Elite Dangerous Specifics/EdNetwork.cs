using System.Xml.Serialization;

namespace RegulatedNoise
{
    public class EdNetwork
    {
        [XmlAttribute("VerboseLogging")]
        public int VerboseLogging { get; set; }
    }
}