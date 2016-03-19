using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using IBE.Enums_and_Utility_Classes;
using IBE.SQL;
using System.Globalization;

namespace IBE
{
    public class PlausibiltyChecker
    {
        public TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

        public bool CheckPricePlausibility(string[] dataRows, bool simpleEDDNCheck = false)
        {
            bool implausible = false;
            SQL.Datasets.dsEliteDB.tbcommodityRow[] commodityData;
            

            foreach (string currentPriceData in dataRows)
            {
                if (currentPriceData.Contains(";"))
                {
                    string[] values = currentPriceData.Split(';');
                    CsvRow currentRow = new CsvRow();

                    currentRow.SellPrice    = -1;
                    currentRow.BuyPrice     = -1;
                    currentRow.Demand       = -1;
                    currentRow.Supply       = -1;

                    currentRow.SystemName       = values[0];
                    currentRow.StationName      = _textInfo.ToTitleCase(values[1].ToLower());
                    currentRow.StationID        = _textInfo.ToTitleCase(values[1].ToLower()) + " [" + currentRow.SystemName + "]";
                    currentRow.CommodityName    = _textInfo.ToTitleCase(values[2].ToLower());

                    if (!String.IsNullOrEmpty(values[3]))
                        Decimal.TryParse(values[3], out currentRow.SellPrice);
                    if (!String.IsNullOrEmpty(values[4]))
                        Decimal.TryParse(values[4], out currentRow.BuyPrice);
                    if (!String.IsNullOrEmpty(values[5]))
                        Decimal.TryParse(values[5], out currentRow.Demand);
                    if (!String.IsNullOrEmpty(values[7]))
                        Decimal.TryParse(values[7], out currentRow.Supply);

                    currentRow.DemandLevel      = _textInfo.ToTitleCase(values[6].ToLower());
                    currentRow.SupplyLevel      = _textInfo.ToTitleCase(values[8].ToLower());

                    DateTime.TryParse(values[9], out currentRow.SampleDate);

                    commodityData = (SQL.Datasets.dsEliteDB.tbcommodityRow[])
                                    Program.Data.BaseData.tbcommodity.Select("commodity    = " + DBConnector.SQLAString(currentRow.CommodityName) + 
                                                                             " or " +
                                                                             "loccommodity = " + DBConnector.SQLAString(currentRow.CommodityName));
                    

                    if (currentRow.CommodityName == "Panik")
                        Debug.Print("STOP");
                            
                    if ((commodityData != null) && (commodityData.GetUpperBound(0) >= 0))
                    { 
                        if ((!String.IsNullOrEmpty(currentRow.SupplyLevel)) && (!String.IsNullOrEmpty(currentRow.DemandLevel)))
                        {
                            // demand AND supply !?
                            implausible = true;
                        }
                        else if ((!String.IsNullOrEmpty(currentRow.SupplyLevel)) || (simpleEDDNCheck && (currentRow.Supply > 0)))
                        { 
                            // check supply data             

                            if ((currentRow.SellPrice <= 0) || (currentRow.BuyPrice <= 0))
                            { 
                                // both on 0 is not plausible
                                implausible = true;
                            }

                            if (((commodityData[0].pwl_supply_sell_low  >= 0) && (currentRow.SellPrice < commodityData[0].pwl_supply_sell_low)) ||
                                ((commodityData[0].pwl_supply_sell_high >= 0) && (currentRow.SellPrice > commodityData[0].pwl_supply_sell_high)))
                            {
                                // sell price is out of range
                                implausible = true;
                            }

                            if (((commodityData[0].pwl_supply_buy_low  >= 0) && (currentRow.BuyPrice  < commodityData[0].pwl_supply_buy_low)) ||
                                ((commodityData[0].pwl_supply_buy_high >= 0) && (currentRow.SellPrice > commodityData[0].pwl_supply_buy_high)))
                            {
                                // buy price is out of range
                                implausible = true;
                            }

                            if (currentRow.Supply.Equals(-1))
                            {   
                                // no supply quantity
                                implausible = true;
                            }

                        }
                        else if ((!String.IsNullOrEmpty(currentRow.DemandLevel)) || (simpleEDDNCheck && (currentRow.Demand > 0)))
                        { 
                            // check demand data

                            if (currentRow.SellPrice <= 0)
                            {
                                // at least the sell price must be present
                                implausible = true;
                            }

                            if (((commodityData[0].pwl_demand_sell_low  >= 0) && (currentRow.SellPrice < commodityData[0].pwl_demand_sell_low)) ||
                                ((commodityData[0].pwl_demand_sell_high >= 0) && (currentRow.SellPrice > commodityData[0].pwl_demand_sell_high)))
                            {
                                // buy price is out of range
                                implausible = true;
                            }

                            if (currentRow.BuyPrice >= 0) 
                                if (((commodityData[0].pwl_demand_buy_low  >= 0) && (currentRow.BuyPrice < commodityData[0].pwl_demand_buy_low)) ||
                                    ((commodityData[0].pwl_demand_buy_high >= 0) && (currentRow.BuyPrice > commodityData[0].pwl_demand_buy_high)))
                                {
                                    // buy price is out of range
                                    implausible = true;
                                }

                            if (currentRow.Demand.Equals(-1))
                            {
                                // no supply quantity
                                implausible = true;
                            }
                        }
                        else
                        { 
                            // nothing ?!
                            implausible = true;
                        }
                    }
                }

                if (implausible)
                    break;
            }

            return implausible;
        }

    }
}
