using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RegulatedNoise.Core.Helpers;

namespace RegulatedNoise.Core.DomainModel
{
	public class MarketDataRow: UpdatableEntity
	{
		private string _stationName;
		private string _commodityName;
		private string _systemName;

		[JsonProperty(PropertyName = "systemName")]
		public string SystemName
		{
			get { return _systemName; }
			set { _systemName = value.ToCleanTitleCase(); }
		}

		[JsonIgnore]
		public string StationID { get { return StationName + " [" + SystemName + "]"; } }

		[JsonProperty(PropertyName = "stationName")]
		public string StationName
		{
			get { return _stationName; }
			set { _stationName = value.ToCleanTitleCase(); }
		}

		[JsonProperty(PropertyName = "itemName")]
		public string CommodityName
		{
			get { return _commodityName; }
			set { _commodityName = value.ToCleanTitleCase(); }
		}

		[JsonProperty(PropertyName = "sellPrice")]
		public int SellPrice { get; set; }

		[JsonProperty(PropertyName = "buyPrice")]
		public int BuyPrice { get; set; }

		[JsonProperty(PropertyName = "stationStock")]
		public int Stock { get; set; }

		[JsonProperty(PropertyName = "demand")]
		public int Demand { get; set; }

		[JsonProperty(PropertyName = "demandLevel", NullValueHandling = NullValueHandling.Ignore)]
		[JsonConverter(typeof(StringEnumConverter))]
		public ProposalLevel? DemandLevel { get; set; }

		[JsonProperty(PropertyName = "supplyLevel", NullValueHandling = NullValueHandling.Ignore)]
		[JsonConverter(typeof(StringEnumConverter))]
		public ProposalLevel? SupplyLevel { get; set; }

		[JsonProperty(PropertyName = "timestamp")]
		public DateTime SampleDate { get; set; }

		[JsonIgnore]
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
				SystemName = fields[0].ToLower().ToCleanUpperCase()
				 ,StationName = fields[1].ToLower().ToCleanTitleCase()
				 ,CommodityName = fields[2].ToLower().ToCleanTitleCase()
				 ,SellPrice = String.IsNullOrWhiteSpace(fields[3]) ? -1 : Int32.Parse(fields[3].Trim())
				 ,BuyPrice = String.IsNullOrWhiteSpace(fields[4]) ? -1 : Int32.Parse(fields[4].Trim())
				 ,Demand = String.IsNullOrWhiteSpace(fields[5]) ? -1 : Int32.Parse(fields[5].Trim())
				 ,DemandLevel = fields[6].ToProposalLevel()
				 ,Stock = String.IsNullOrWhiteSpace(fields[7]) ? -1 : Int32.Parse(fields[7].Trim())
				 ,SupplyLevel = fields[8].ToProposalLevel()
				 ,SampleDate = String.IsNullOrWhiteSpace(fields[9]) ? DateTime.MinValue : ReadCsvDate(fields[9])
			};
			if (fields.Length > 10)
			{
				var source = fields[10];
				if (!String.IsNullOrWhiteSpace(source))
				{
					marketData.Source = source.Trim();
				}
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

		public static MarketDataRow ReadJson(string json)
		{
			return JsonConvert.DeserializeObject<MarketDataRow>(json);
		}

		public string ToCsv(bool useExtended)
		{
			return SystemName + ";" +
							StationName + ";" +
							CommodityName + ";" +
							(SellPrice > 0 ? SellPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
							(BuyPrice > 0 ? BuyPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
							(Demand > 0 ? Demand.ToString(CultureInfo.InvariantCulture) : "") + ";" +
							DemandLevel.Display() + ";" +
							(Stock > 0 ? Stock.ToString(CultureInfo.InvariantCulture) : "") + ";" +
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

		public static string StationIdToStationName(string stationId)
		{
			return stationId.Substring(0, stationId.IndexOf("[") - 1);
		}

		public static string StationIdToSystemName(string stationId)
		{
			return stationId.Substring(stationId.IndexOf("[") + 1).TrimEnd(']');
		}
	}
}
