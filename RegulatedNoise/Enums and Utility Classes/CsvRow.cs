using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    public class CsvRow
    {
        public string SystemName;
        public string StationID;
        public string StationName;
        public string CommodityName;
        public decimal SellPrice;
        public decimal BuyPrice;
        public decimal Cargo;
        public decimal Demand;
        public string DemandLevel;
        public decimal Supply;
        public string SupplyLevel;
        public DateTime SampleDate;
        public string SourceFileName;

        public override string ToString()
        {
            return SystemName + ";" +
                        StationID.Replace(" [" + SystemName + "]", "") + ";" +
                        CommodityName + ";" +
                        (SellPrice != 0 ? SellPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        (BuyPrice != 0 ? BuyPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        (Demand != 0 ? Demand.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        DemandLevel + ";" +
                        (Supply != 0 ? Supply.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                        SupplyLevel + ";" +
                        SampleDate.ToString("s", CultureInfo.CurrentCulture).Substring(0, 16) + ";" +
                        SourceFileName;
        }
    }
}
