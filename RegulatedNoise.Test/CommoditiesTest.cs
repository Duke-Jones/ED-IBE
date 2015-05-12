using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.Test
{
    [TestClass]
    public class CommoditiesTest
    {
        [TestMethod]
        public void i_can_instantiate()
        {
            Commodities commodities = NewCommodities();
        }

        [TestMethod]
        public void added_event_raised_on_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            Assert.IsTrue(events.Any(e => e.IsAdded && e.Actual == marketData), "no event raised");
        }

        [TestMethod]
        public void added_event_raised_on_bulk_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1,10).Select(i =>NewMarketData(DateTime.Now, i)).ToArray();
            commodities.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.IsTrue(events.Any(e => e.IsAdded && e.Actual == marketData), "no event raised");                
            }
        }

        [TestMethod]
        public void removed_event_raised_on_remove()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            commodities.Remove(marketData);
            Assert.IsTrue(events.Any(e => e.IsRemoved), "no event raised");
        }

        [TestMethod]
        public void replaced_event_raised_on_replace()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.Update(NewMarketData(DateTime.Now));
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            commodities.Update(NewMarketData(DateTime.Now.AddMinutes(2)));
            Assert.IsTrue(events.Any(e => e.IsReplaced), "no event raised");
        }

        [TestMethod]
        public void data_is_added_on_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            Assert.AreEqual(marketData, commodities[marketData.MarketDataId], "data has not been added");
        }

        [TestMethod]
        public void data_is_added_on_station_index_on_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            Assert.IsTrue(commodities.StationMarket(marketData.StationID).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through station index");
        }

        [TestMethod]
        public void data_is_added_on_commodities_index_on_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            Assert.IsTrue(commodities.CommodityMarket(marketData.CommodityName).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through comodities index");
        }

        [TestMethod]
        public void data_are_added_on_bulk_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1, 10).Select(i => NewMarketData(DateTime.Now, i)).ToArray();
            commodities.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.AreEqual(marketData, commodities[marketData.MarketDataId], "data has not been added");
            }
        }

        [TestMethod]
        public void data_are_added_in_station_index_on_bulk_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1, 10).Select(i => NewMarketData(DateTime.Now, i)).ToArray();
            commodities.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.IsTrue(commodities.StationMarket(marketData.StationID).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through station index");
            }
        }

        [TestMethod]
        public void data_are_added_in_commodities_index_on_bulk_add()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1, 10).Select(i => NewMarketData(DateTime.Now, i)).ToArray();
            commodities.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.IsTrue(commodities.CommodityMarket(marketData.CommodityName).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through comodities index");
            }
        }

        [TestMethod]
        public void data_is_removed_on_remove()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            commodities.Remove(marketData);
            Assert.IsFalse(commodities.Contains(marketData), "marketdata has not been removed");
        }

        [TestMethod]
        public void data_is_removed_from_station_index_on_remove()
        {
            Commodities commodities = NewCommodities();
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            commodities.Remove(marketData);
            Assert.IsFalse(commodities.StationMarket(marketData.StationID).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through station index");
        }

        [TestMethod]
        public void data_is_removed_from_commodities_index_on_remove()
        {
            Commodities commodities = NewCommodities();
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            commodities.Remove(marketData);
            Assert.IsFalse(commodities.CommodityMarket(marketData.CommodityName).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through comodities index");
        }

        [TestMethod]
        public void obsolete_data_is_silently_discarded()
        {
            Commodities commodities = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            var marketData = NewMarketData(DateTime.Now);
            commodities.Update(marketData);
            commodities.OnMarketDataUpdate += (sender, args) => events.Add(args);
            commodities.Update(NewMarketData(DateTime.Now.AddHours(-2)));
            Assert.IsFalse(events.Any(), "no event should have been raised");
            Assert.AreEqual(marketData.SampleDate, commodities[marketData.MarketDataId].SampleDate, "marketdata should not have been replaced by obsolete data");
        }

        private MarketDataRow NewMarketData(DateTime timestamp, int? seed = null)
        {
            string index = (!seed.HasValue ? String.Empty : "_" + seed.Value.ToString("00"));
            return new MarketDataRow()
            {
                SystemName = "mySytem" + index
                ,StationName = "myStation" + index
                ,CommodityName = "myCommodity" + index
                ,SampleDate = timestamp
            };
        }

        private static Commodities NewCommodities()
        {
            return new Commodities();
        }
    }
}