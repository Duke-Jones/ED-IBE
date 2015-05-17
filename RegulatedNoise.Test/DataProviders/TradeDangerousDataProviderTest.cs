using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegulatedNoise.Core.DataProviders;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.Test.DataProviders
{
    [TestClass]
    public class TradeDangerousDataProviderTest
    {
        [TestMethod]
        public void i_can_retrieve_in_parallel()
        {
            Trace.UseGlobalLock = false;
            Trace.AutoFlush = true;
            var dataProvider = new TradeDangerousDataProvider();
            var tasks = new Task[]
            {
                dataProvider.RetrieveSystems()
                , dataProvider.RetrieveItems()
                , dataProvider.RetrieveShipVendors()
                , dataProvider.RetrievePrices()
            };
            Task.WaitAll(tasks, TimeSpan.FromSeconds(30));
        }

        [TestMethod]
        public void i_can_retrieve_systems()
        {
            var dataProvider = new TradeDangerousDataProvider();
            dataProvider.RetrieveSystems().Wait(TimeSpan.FromMinutes(1));
        }

        [TestMethod]
        public void i_can_retrieve_commodities()
        {
            var dataProvider = new TradeDangerousDataProvider();
            dataProvider.RetrieveItems().Wait(TimeSpan.FromMinutes(1));
        }

        [TestMethod]
        public void i_can_retrieve_ship_vendors()
        {
            var dataProvider = new TradeDangerousDataProvider();
            dataProvider.RetrieveShipVendors().Wait(TimeSpan.FromMinutes(1));
        }

        [TestMethod]
        public void i_can_retrieve_prices()
        {
            var dataProvider = new TradeDangerousDataProvider();
            dataProvider.RetrievePrices().Wait(TimeSpan.FromMinutes(1));
        }

        [TestMethod]
        public void i_can_parse_prices()
        {
            const string line =
                "      Mineral Oil                 327       0   1165717H         -  2015-05-13 20:43:06  # EO0cf14a2b_EliteOCR_0.6.0.9";
            var parser = new PriceParser();
            parser.Parse("@ mySystem B2/myStation 01 B2");
            parser.Parse(line);
            Assert.IsTrue(parser.MarketDatas.Any(), "no marketdata parsed");
            var marketData = parser.MarketDatas.First();
            Assert.AreEqual("mySystem B2", marketData.SystemName, "unexpected system name");
            Assert.AreEqual("Mystation 01 B2", marketData.StationName, "unexpected system name");
            Assert.AreEqual("Mineral Oil", marketData.CommodityName, "unexpected commodity name");
            Assert.AreEqual(327, marketData.SellPrice, "unexpected sell price");
            Assert.AreEqual(0, marketData.BuyPrice, "unexpected buy price");
            Assert.AreEqual(1165717, marketData.Demand, "unexpected demand");
            Assert.AreEqual(ProposalLevel.High, marketData.DemandLevel, "unexpected demand level");
            Assert.AreEqual(new DateTime(2015,5,13,20,43,06,DateTimeKind.Utc).ToLocalTime(), marketData.SampleDate, "unexpected sample date");
        }

        [TestMethod]
        public void i_can_parse_all_prices()
        {
            var parser = new PriceParser();
            using (var reader = new StreamReader("playground/TradeDangerous-3h.prices.txt"))
            {
                while(!reader.EndOfStream)
                {
                    parser.Parse(reader.ReadLine());
                }
            }
        }
    }
}