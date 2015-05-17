using System;
using Newtonsoft.Json;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.EDDB_Data
{
	public class EddnMessage
	{
		public static EddnMessage ReadJson(string json)
		{
			var eddnMessage = JsonConvert.DeserializeObject<EddnMessage>(json);
			eddnMessage.RawText = json;
			return eddnMessage;
		}

		[JsonProperty(PropertyName = "header")]
		public Header Header { get; set; }
		[JsonProperty(PropertyName = "$schemaRef")]
		public string SchemaRef { get; set; }
		[JsonProperty(PropertyName = "message")]
		public MarketDataRow Message { get; set; }
		[JsonIgnore]
		public string RawText { get; set; }

		[JsonIgnore]
		public bool IsTest
		{
			get { return SchemaRef != null && SchemaRef.Contains("Test"); }
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}

	public class Header
	{
		[JsonProperty(PropertyName = "softwareVersion")]
		public string SoftwareVersion { get; set; }
		[JsonProperty(PropertyName = "gatewayTimestamp")]
		public DateTime GatewayTimestamp { get; set; }
		[JsonProperty(PropertyName = "softwareName")]
		public string SoftwareName { get; set; }
		[JsonProperty(PropertyName = "uploaderID")]
		public string UploaderId { get; set; }
	}
}