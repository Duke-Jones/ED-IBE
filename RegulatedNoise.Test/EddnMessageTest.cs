using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise.Test
{
    [TestClass]
    public class EddnMessageTest
    {
        [TestMethod]
        public void i_can_parse_software_version()
        {
            const string rawText = @"{""header"": {""softwareVersion"": ""0.6.0.2"", ""gatewayTimestamp"": ""2015-05-09T17:21:42.354527"", ""softwareName"": ""EliteOCR"", ""uploaderID"": ""EO4d8dca19""}, ""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1"", ""message"": {""buyPrice"": 3747, ""timestamp"": ""2015-05-09T17:18:30+00:00"", ""stationStock"": 2582658, ""systemName"": ""Cupis"", ""stationName"": ""Boodt Port"", ""demand"": 0, ""sellPrice"": 3669, ""itemName"": ""Marine Equipment"", ""supplyLevel"": ""High""}}";
            var eddnMessage = JsonConvert.DeserializeObject<EddnMessage>(rawText);
            Assert.AreEqual("0.6.0.2", eddnMessage.header.softwareVersion, "unexpected software version");
        }

        [TestMethod]
        public void i_can_parse_demand_level()
        {
            const string rawText = @"{""header"": {""softwareVersion"": ""0.6.0.2"", ""gatewayTimestamp"": ""2015-05-09T17:21:42.354527"", ""softwareName"": ""EliteOCR"", ""uploaderID"": ""EO4d8dca19""}, ""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1"", ""message"": {""buyPrice"": 3747, ""timestamp"": ""2015-05-09T17:18:30+00:00"", ""stationStock"": 2582658, ""systemName"": ""Cupis"", ""stationName"": ""Boodt Port"", ""demand"": 0, ""sellPrice"": 3669, ""itemName"": ""Marine Equipment"", ""supplyLevel"": ""High""}}";
            var eddnMessage = JsonConvert.DeserializeObject<EddnMessage>(rawText);
            Assert.AreEqual(ProposalLevel.High, eddnMessage.message.supplyLevel, "unexpected supply level");
        }
    }
}