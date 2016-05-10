using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace IBE.EDDN
{
    public class MessageHeader
    {
        [JsonProperty("softwareVersion")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("gatewayTimestamp")]
        public string GatewayTimestamp { get; set; }

        [JsonProperty("softwareName")]
        public string SoftwareName { get; set; }

        [JsonProperty("uploaderID")]
        public string UploaderID { get; set; }
    }
}
