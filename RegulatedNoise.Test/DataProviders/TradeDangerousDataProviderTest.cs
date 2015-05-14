using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

    internal class TradeDangerousDataProvider
    {
        private const string PRICES_3H_URL = "http://www.davek.com.au/td/prices-3h.asp";
        private const string SYSTEMS_URL = "http://www.davek.com.au/td/System.csv";
        private const string STATIONS_URL = "http://www.davek.com.au/td/station.asp";
        private const string SHIP_VENDORS_URL = "http://www.davek.com.au/td/shipvendor.asp";
        private const string CATEGORIES_URL = "http://www.davek.com.au/td/Category.csv";
        private const string ITEMS_URL = "http://www.davek.com.au/td/Item.csv";

        public async Task RetrieveSystems()
        {
            await RetrieveData(SYSTEMS_URL, s => Debug.WriteLine("[4]: " + Thread.CurrentThread.ManagedThreadId + " -> " + s));
        }

        public async Task RetrieveItems()
        {
            await RetrieveData(ITEMS_URL, s => Debug.WriteLine("[4]: " + Thread.CurrentThread.ManagedThreadId + " -> " + s));
        }

        public async Task<IEnumerable<MarketDataRow>> RetrievePrices()
        {
            PriceParser parser = new PriceParser();
            await RetrieveData(PRICES_3H_URL, line => parser.Parse(line));
            return parser.MarketDatas;
        }

        public async Task RetrieveShipVendors()
        {
            await RetrieveData(SHIP_VENDORS_URL, s => Debug.WriteLine("[4]: " + Thread.CurrentThread.ManagedThreadId + " -> " + s));
        }

        private async Task RetrieveData(string uri, Action<string> onLineRead)
        {
            HttpResponseMessage httpResponse;

            using (var client = new HttpClient())
            {
                Debug.WriteLine("[1]: " + Thread.CurrentThread.ManagedThreadId);
                httpResponse = await client.GetAsync(new Uri(uri));
                Debug.WriteLine("[2]: " + Thread.CurrentThread.ManagedThreadId);
            }
            try
            {
                using (var reader = new StreamReader(await httpResponse.Content.ReadAsStreamAsync()))
                {
                    while(!reader.EndOfStream)
                    {
                        onLineRead(await reader.ReadLineAsync());
                        Debug.WriteLine("[3]: " + Thread.CurrentThread.ManagedThreadId);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("edsc parsing failure: " + ex);
            }
        }

    }

    internal class PriceParser
    {
        //# <item name> <sell> <buy> <demand units><level> <stock units><level> <timestamp>
//@ HILLA/Siemens Vision
//   + Chemicals
//      Explosives                    0     223          ?    49505M  2015-05-13 20:43:06  # EO0cf14a2b_EliteOCR_0.6.0.9
//      Hydrogen Fuel                 0      94          ?   116027H  2015-05-13 20:43:06  # EO0cf14a2b_EliteOCR_0.6.0.9
//      Mineral Oil                 327       0   1165717H         -  2015-05-13 20:43:06  # EO0cf14a2b_EliteOCR_0.6.0.9

        private readonly List<MarketDataRow> _marketDatas;

        private readonly Regex _rowRegex = new Regex("\\s*(?<commodity>([\\w]+|[\\w]+ )+)\\s+(?<sell>\\d+)\\s+(?<buy>\\d+)\\s+(?<demand>(\\d+|))(?<demandlevel>[LMH\\?])\\s+(?<stock>(\\d+|))(?<supplylevel>([LMH\\?-]))\\s+(?<timestamp>[^#]+)", RegexOptions.Compiled);
        private string _currentSystemName;
        private string _currentStationName;

        private int _lineCount;

        public PriceParser()
        {
            _marketDatas = new List<MarketDataRow>();
        }

        public void Parse(string line)
        {
            ++_lineCount;
            try
            {
                if (String.IsNullOrWhiteSpace(line)) return;
                line = line.Trim();
                if (line.StartsWith("#")) return;
                if (line.StartsWith("+")) return;
                if (line.StartsWith("@"))
                {
                    string[] fields = line.Substring(1).Split('/');
                    _currentSystemName = fields[0].Trim();
                    _currentStationName = fields[1].Trim();
                }
                else // commodity row
                {
                    MarketDataRow currentRow = new MarketDataRow() { SystemName = _currentSystemName, StationName = _currentStationName };
                    Match match = _rowRegex.Match(line);
                    Group commodity = match.Groups["commodity"];
                    if (commodity.Success)
                        currentRow.CommodityName = commodity.Value.Trim();
                    Group sell = match.Groups["sell"];
                    if (sell.Success)
                        currentRow.SellPrice = Int32.Parse(sell.Value);
                    Group buy = match.Groups["buy"];
                    if (buy.Success)
                        currentRow.BuyPrice = Int32.Parse(buy.Value);
                    Group demand = match.Groups["demand"];
                    if (demand.Success && !String.IsNullOrWhiteSpace(demand.Value))
                        currentRow.Demand = Int32.Parse(demand.Value);
                    Group demandLevel = match.Groups["demandlevel"];
                    if (demandLevel.Success)
                        currentRow.DemandLevel = ParseProposalLevel(demandLevel.Value);
                    Group stock = match.Groups["stock"];
                    if (stock.Success && !String.IsNullOrWhiteSpace(stock.Value))
                        currentRow.Stock = Int32.Parse(stock.Value);
                    Group supplyLevel = match.Groups["supplylevel"];
                    if (supplyLevel.Success)
                        currentRow.SupplyLevel = ParseProposalLevel(supplyLevel.Value);
                    Group timestamp = match.Groups["timestamp"];
                    if (timestamp.Success)
                        currentRow.SampleDate = DateTime.SpecifyKind(DateTime.Parse(timestamp.Value.Trim()), DateTimeKind.Utc).ToLocalTime();
                    _marketDatas.Add(currentRow);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("error encountered line #" + _lineCount + ": " + line + Environment.NewLine + ex);
            }
        }

        private static ProposalLevel? ParseProposalLevel(string value)
        {
            if (value == "L")
            {
                return ProposalLevel.Low;
            }
            else if (value == "M")
            {
                return ProposalLevel.Med;
            }
            else if (value == "H")
            {
                return ProposalLevel.High;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<MarketDataRow> MarketDatas
        {
            get { return _marketDatas; }
        }
    }
}