using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.Core.DomainModel;

namespace RegulatedNoise.Test
{
    [TestClass]
    public class GalacticMarketTest
    {
        [TestMethod]
        public void i_can_instantiate()
        {
            GalacticMarket galacticMarket = NewCommodities();
        }

        [TestMethod]
        public void added_event_raised_on_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            Assert.IsTrue(events.Any(e => e.IsAdded && e.Actual == marketData), "no event raised");
        }

        [TestMethod]
        public void added_event_raised_on_bulk_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1,10).Select(i =>NewMarketData(DateTime.Now, i)).ToArray();
            galacticMarket.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.IsTrue(events.Any(e => e.IsAdded && e.Actual == marketData), "no event raised");                
            }
        }

        [TestMethod]
        public void removed_event_raised_on_remove()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            galacticMarket.Remove(marketData);
            Assert.IsTrue(events.Any(e => e.IsRemoved), "no event raised");
        }

        [TestMethod]
        public void replaced_event_raised_on_replace()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.Update(NewMarketData(DateTime.Now));
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            galacticMarket.Update(NewMarketData(DateTime.Now.AddMinutes(2)));
            Assert.IsTrue(events.Any(e => e.IsReplaced), "no event raised");
        }

        [TestMethod]
        public void data_is_added_on_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            Assert.AreEqual(marketData, galacticMarket[marketData.MarketDataId], "data has not been added");
        }

        [TestMethod]
        public void data_is_added_on_station_index_on_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            Assert.IsTrue(galacticMarket.StationMarket(marketData.StationID).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through station index");
        }

        [TestMethod]
        public void data_is_added_on_commodities_index_on_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            Assert.IsTrue(galacticMarket.CommodityMarket(marketData.CommodityName).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through comodities index");
        }

        [TestMethod]
        public void data_are_added_on_bulk_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1, 10).Select(i => NewMarketData(DateTime.Now, i)).ToArray();
            galacticMarket.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.AreEqual(marketData, galacticMarket[marketData.MarketDataId], "data has not been added");
            }
        }

        [TestMethod]
        public void data_are_added_in_station_index_on_bulk_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1, 10).Select(i => NewMarketData(DateTime.Now, i)).ToArray();
            galacticMarket.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.IsTrue(galacticMarket.StationMarket(marketData.StationID).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through station index");
            }
        }

        [TestMethod]
        public void data_are_added_in_commodities_index_on_bulk_add()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            var marketDatas = Enumerable.Range(1, 10).Select(i => NewMarketData(DateTime.Now, i)).ToArray();
            galacticMarket.UpdateRange(marketDatas);
            foreach (MarketDataRow marketData in marketDatas)
            {
                Assert.IsTrue(galacticMarket.CommodityMarket(marketData.CommodityName).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through comodities index");
            }
        }

        [TestMethod]
        public void data_is_removed_on_remove()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            galacticMarket.Remove(marketData);
            Assert.IsFalse(galacticMarket.Contains(marketData), "marketdata has not been removed");
        }

        [TestMethod]
        public void data_is_removed_from_station_index_on_remove()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            galacticMarket.Remove(marketData);
            Assert.IsFalse(galacticMarket.StationMarket(marketData.StationID).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through station index");
        }

        [TestMethod]
        public void data_is_removed_from_commodities_index_on_remove()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            galacticMarket.Remove(marketData);
            Assert.IsFalse(galacticMarket.CommodityMarket(marketData.CommodityName).Any(m => m.MarketDataId == marketData.MarketDataId), "data is not available through comodities index");
        }

        [TestMethod]
        public void obsolete_data_is_silently_discarded()
        {
            GalacticMarket galacticMarket = NewCommodities();
            var events = new List<MarketDataEventArgs>();
            var marketData = NewMarketData(DateTime.Now);
            galacticMarket.Update(marketData);
            galacticMarket.OnMarketDataUpdate += (sender, args) => events.Add(args);
            galacticMarket.Update(NewMarketData(DateTime.Now.AddHours(-2)));
            Assert.IsFalse(events.Any(), "no event should have been raised");
            Assert.AreEqual(marketData.SampleDate, galacticMarket[marketData.MarketDataId].SampleDate, "marketdata should not have been replaced by obsolete data");
        }

        private static MarketDataRow NewMarketData(DateTime timestamp, int? seed = null)
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

        private static GalacticMarket NewCommodities()
        {
            return new GalacticMarket();
        }
    }
}