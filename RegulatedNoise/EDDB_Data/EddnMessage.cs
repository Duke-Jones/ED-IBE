using System;
using Newtonsoft.Json;
using RegulatedNoise.Enums_and_Utility_Classes;

// ReSharper disable InconsistentNaming

namespace RegulatedNoise.EDDB_Data
{
    public class EddnMessage
    {
        public static EddnMessage ReadJson(string json)
        {
            return JsonConvert.DeserializeObject<EddnMessage>(json);
        }

        public Header header { get; set; }
        [JsonProperty(PropertyName = "$schemaRef")]
        public string schemaRef { get; set; }
        public MarketDataRow message { get; set; }
        public string RawText { get; set; }

        public bool IsTest
        {
            get { return schemaRef != null && schemaRef.Contains("Test"); }
        }
    }

    public class Header
    {
        public string softwareVersion { get; set; }
        public DateTime gatewayTimestamp { get; set; }
        public string softwareName { get; set; }
        public string uploaderID { get; set; }
    }
}