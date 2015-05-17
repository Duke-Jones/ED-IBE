using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RegulatedNoise.Core.DomainModel;

namespace RegulatedNoise.Test
{
    [TestClass]
    public class MarketDataRowTest
    {
        [TestMethod]
        public void i_can_retrieve_station_name_from_stationId()
        {
            var marketData = new MarketDataRow() { SystemName = "aSystem", StationName = "aStation"};
            Assert.AreEqual(marketData.StationName, MarketDataRow.StationIdToStationName(marketData.StationID), "unexpected station name extracted from stationId");
        }

		  [TestMethod]
		  public void i_can_serialize_proposal_level()
		  {
			  var marketData = new MarketDataRow() { SystemName = "aSystem", StationName = "aStation", DemandLevel = ProposalLevel.High };
			  string json = JsonConvert.SerializeObject(marketData, Formatting.Indented);
			  Debug.WriteLine(json);
			  Assert.IsTrue(json.Contains(@"""demandLevel"": ""High"""), "demand level not serialized correctly");
			  Assert.IsFalse(json.Contains(@"""supplyLevel"""), "supply level not serialized correctly");
		  }

		  [TestMethod]
		  public void i_can_deserialize_proposal_level()
		  {
			  var marketData = new MarketDataRow() { SystemName = "aSystem", StationName = "aStation", DemandLevel = ProposalLevel.High };
			  string json = JsonConvert.SerializeObject(marketData, Formatting.Indented);
			  MarketDataRow deserialized = JsonConvert.DeserializeObject<MarketDataRow>(json);
			  Assert.AreEqual(marketData.DemandLevel, deserialized.DemandLevel, "unexpected demand level deserialized");
			  Assert.AreEqual(marketData.SupplyLevel, deserialized.SupplyLevel, "unexpected supply level deserialized");
		  }

        [TestMethod]
        public void i_can_retrieve_system_name_from_stationId()
        {
            var marketData = new MarketDataRow() { SystemName = "aSystem", StationName = "aStation" };
            Assert.AreEqual(marketData.SystemName, MarketDataRow.StationIdToSystemName(marketData.StationID), "unexpected station name extracted from stationId");
        }
    }
}