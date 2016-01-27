using System.Xml.Serialization;

namespace IBE
{
    public class EdNetwork
    {
        [XmlAttribute("VerboseLogging")]
        public int VerboseLogging { get; set; }
    }
}