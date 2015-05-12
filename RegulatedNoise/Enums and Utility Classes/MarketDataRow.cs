using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using Newtonsoft.Json;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    public class MarketDataRow
    {
        private static readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        [JsonProperty(PropertyName = "systemName")]
        public string SystemName { get; set; }
        
        public string StationID { get { return SystemName + " [" + StationName + "]"; } }

        [JsonProperty(PropertyName = "stationName")]
        public string StationName { get; set; }

        [JsonProperty(PropertyName = "itemName")]
        public string CommodityName { get; set; }

        [JsonProperty(PropertyName = "sellPrice")]
        public int SellPrice { get; set; }

        [JsonProperty(PropertyName = "buyPrice")]
        public int BuyPrice { get; set; }

        [JsonProperty(PropertyName = "stationStock")]
        public int Stock { get; set; }

        [JsonProperty(PropertyName = "demand")]
        public int Demand { get; set; }

        [JsonProperty(PropertyName = "demandLevel")]
        public ProposalLevel? DemandLevel { get; set; }

        [JsonProperty(PropertyName = "supply")]
        public int Supply { get; set; }

        [JsonProperty(PropertyName = "supplyLevel")]
        public ProposalLevel? SupplyLevel { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public DateTime SampleDate { get; set; }

        public string Source { get; set; }

        public string MarketDataId
        {
            get
            {
                return CommodityName + "@" + StationID;
            }
        }

        public override string ToString()
        {
            return ToCsv(true);
        }

        public static MarketDataRow ReadCsv(string csv)
        {
            if (String.IsNullOrWhiteSpace(csv)) throw new ArgumentException("invalid csv", "csv");
            //System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;
            var fields = csv.Split(';');
            if (fields.Length < 10) throw new ArgumentException("invalid csv", "csv");
            var marketData = new MarketDataRow()
            {
                SystemName = fields[0].Trim()
                ,StationName = Format(fields[1])
                ,CommodityName = Format(fields[2])
                ,SellPrice = String.IsNullOrWhiteSpace(fields[3]) ? -1 : Int32.Parse(fields[3].Trim())
                ,BuyPrice = String.IsNullOrWhiteSpace(fields[4]) ? -1 : Int32.Parse(fields[4].Trim())
                ,Demand = String.IsNullOrWhiteSpace(fields[5]) ? -1 : Int32.Parse(fields[5].Trim())
                ,DemandLevel = fields[6].ToProposalLevel()
                ,Supply = String.IsNullOrWhiteSpace(fields[7]) ? -1 : Int32.Parse(fields[7].Trim())
                ,SupplyLevel = fields[8].ToProposalLevel()
                ,SampleDate = String.IsNullOrWhiteSpace(fields[9]) ? DateTime.MinValue : ReadCsvDate(fields[9])
            };
            if (fields.Length > 10)
            {
                marketData.Source = fields[8].Trim();
            }
            return marketData;
        }

        private static DateTime ReadCsvDate(string datefield)
        {
            DateTime date;
            try
            {
                date = XmlConvert.ToDateTime(datefield.Trim(), XmlDateTimeSerializationMode.Local);
            }
            catch (Exception ex)
            {
                if (!DateTime.TryParse(datefield, out date))
                {
                    Trace.TraceWarning("unable to parse date from '" + datefield + "' " + ex);
                }
            }
            return date;
        }

        private static string Format(string source)
        {
            return _textInfo.ToTitleCase(source.Trim().ToLower());
        }

        public static MarketDataRow ReadJson(string json)
        {
            return JsonConvert.DeserializeObject<MarketDataRow>(json);
        }

        public string ToCsv(bool useExtended)
        {
            return SystemName + ";" +
                        StationName + ";" +
                        CommodityName + ";" +
                        (SellPrice != 0 ? SellPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        (BuyPrice != 0 ? BuyPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        (Demand != 0 ? Demand.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        DemandLevel.Display() + ";" +
                        (Supply != 0 ? Supply.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        SupplyLevel.Display() + ";" +
                        XmlConvert.ToString(SampleDate, XmlDateTimeSerializationMode.Local) +
                        (useExtended ? ";" + Source : String.Empty);
        }

        public static bool AreEqual(MarketDataRow lhs, MarketDataRow rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (lhs == null || rhs == null) return false;
            return String.Compare(lhs.MarketDataId, rhs.MarketDataId, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public override bool Equals(object obj)
        {
            return AreEqual(this, obj as MarketDataRow);
        }

        public override int GetHashCode()
        {
            return MarketDataId.GetHashCode();
        }
    }
}
