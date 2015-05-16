using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.Enums_and_Utility_Classes;

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
        public void i_can_retrieve_system_name_from_stationId()
        {
            var marketData = new MarketDataRow() { SystemName = "aSystem", StationName = "aStation" };
            Assert.AreEqual(marketData.SystemName, MarketDataRow.StationIdToSystemName(marketData.StationID), "unexpected station name extracted from stationId");
        }
    }
}