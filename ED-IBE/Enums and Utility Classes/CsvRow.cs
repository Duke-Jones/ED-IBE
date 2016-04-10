using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.Enums_and_Utility_Classes
{
    public class CsvRow
    {
        public String       SystemName;
        public String       StationID;
        public String       StationName;
        public String       CommodityName;
        public Decimal      SellPrice;
        public Decimal      BuyPrice;
        public Decimal      Cargo;
        public Decimal      Demand;
        public String       DemandLevel;
        public Decimal      Supply;
        public String       SupplyLevel;
        public DateTime     SampleDate;
        public String       SourceFileName;
        public String       DataSource;

        /// <summary>
        /// default constructor
        /// </summary>
        public CsvRow()
        { }
        
        /// <summary>
        /// constructor with initial values from csv row
        /// </summary>
        /// <param name="CsvString"></param>
        public CsvRow(String CsvString)
        {
            try
            {
                FromString(CsvString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating CsvRow object", ex);
            }
        }

        public override String ToString()
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
                        SourceFileName + ";" +
                        DataSource;
        }

        /// <summary>
        /// converts a csv row to classobject CsvRow
        /// </summary>
        /// <param name="CsvString"></param>
        /// <returns></returns>
        public void FromString(String CsvString)
        {
            try
            {
                String[] Parts = CsvString.Split(';');

                if(Parts.Count() >= 10)
                {
                    SystemName          = Parts[0].Trim();
                    StationName         = Parts[1].Trim();
                    StationID           = String.Format("{0}[{1}]", StationName.Trim(), SystemName.Trim());
                    CommodityName       = Parts[2];
                    SellPrice           = Parts[3] == "" ? 0 : Decimal.Parse(Parts[3]);
                    BuyPrice            = Parts[4] == "" ? 0 : Decimal.Parse(Parts[4]);
                    Demand              = Parts[5] == "" ? 0 : Decimal.Parse(Parts[5]);
                    DemandLevel         = Parts[6];
                    Supply              = Parts[7] == "" ? 0 : Decimal.Parse(Parts[7]);
                    SupplyLevel         = Parts[8];
                    SampleDate          = DateTime.Parse(Parts[9], CultureInfo.CurrentUICulture, DateTimeStyles.AssumeUniversal);

                    if(Parts.Count() > 10)
                        SourceFileName  = Parts[10];
                    else
                        SourceFileName  = "";

                    if(Parts.Count() > 11)
                        DataSource  = Parts[11];
                    else
                        DataSource  = "";
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting csv row to class CsvRow", ex);
            }
        }
    }
}
