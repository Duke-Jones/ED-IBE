using System;
using System.Collections.Generic;
using System.Linq;
using IBE.Enums_and_Utility_Classes;
using System.Diagnostics;
using IBE.SQL.Datasets;
using System.Data;

namespace IBE.IBECompanion
{
    class CompanionConverter
    {
        private List<String> ignoreStuff = new List<String>() {"bobble", "decal", "paintjob"};

        public EDDN.OutfittingObject GetOutfittingFromFDevIDs(SQL.Datasets.dsEliteDB.tboutfittingbaseDataTable basedata,  Newtonsoft.Json.Linq.JToken outfittingItem, bool param1)
        {
            EDDN.OutfittingObject outfitting = null;
            List<String> nameParts;
            String nameFull;

            try
            {
               
                if(outfittingItem != null)
                { 
                    if(outfittingItem.SelectToken("id", false) == null)
                        throw new NotSupportedException(String.Format("Missing id : {0}", outfittingItem.ToString()));

                    nameFull  = outfittingItem.SelectToken("name", false).ToString().ToLower();
                    nameParts = nameFull.Split(new char[] {'_'}).ToList();

                    if (!ignoreStuff.Contains(nameParts[0]))
                    {
                        SQL.Datasets.dsEliteDB.tboutfittingbaseRow itemData = basedata.FindByid((Int32)outfittingItem.SelectToken("id", false));

                        if(itemData != null)
                        {
                            outfitting = new EDDN.OutfittingObject();

                            outfitting.Id               = itemData.id;
                            outfitting.Category         = itemData.category;
                            outfitting.Name             = itemData.name;
                            outfitting.Mount            = itemData.mount;
                            outfitting.Guidance         = itemData.guidance.ToNString();
                            outfitting.Ship             = itemData.ship.ToNString();
                            outfitting.Class            = itemData._class;
                            outfitting.Rating           = itemData.rating;
                            outfitting.Entitlement      = itemData.entitlement;
                        }
                        else
                            throw new NotSupportedException(String.Format("Unknown id : {0}", outfittingItem.ToString()));
                    }
                }

            }
            catch (NotSupportedException ex)
            {
                Program.MainLog.Log(String.Format("Converting error: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting companion data to shipyard object", ex);
            }

            return outfitting;

        }

        public EDDN.ShipyardObject GetShipFromFDevIDs(SQL.Datasets.dsEliteDB.tbshipyardbaseDataTable basedata, Newtonsoft.Json.Linq.JToken shipyardItem, bool param1)
        {
            EDDN.ShipyardObject shipyardObject = null;  
            String skuString = null;

            try
            {
               
                if(shipyardItem != null)
                { 
                    if (shipyardItem.SelectToken("id", false) == null)
                        throw new NotSupportedException(String.Format("Missing id : {0}", shipyardItem));

                    SQL.Datasets.dsEliteDB.tbshipyardbaseRow itemData = basedata.FindByid((Int32)shipyardItem.SelectToken("id", false));

                    if(itemData != null)
                    {
                        shipyardObject = new EDDN.ShipyardObject();

                        shipyardObject.Id         = itemData.id;
                        shipyardObject.Name       = itemData.name;
                        shipyardObject.BaseValue  = shipyardItem.SelectToken("basevalue").ToString();
                        shipyardObject.Sku        = skuString = (shipyardItem.SelectToken("sku", false) ?? "").ToString();

                        if(shipyardItem.SelectToken("unavailableReason", false) != null)
                            shipyardObject.UnavailableReason = shipyardItem.SelectToken("unavailableReason", false).ToString();

                        if(shipyardItem.SelectToken("factionId", false) != null)
                            shipyardObject.FactionID = shipyardItem.SelectToken("factionId", false).ToString();

                        if(shipyardItem.SelectToken("requiredRank", false) != null)
                            shipyardObject.RequiredRank = shipyardItem.SelectToken("requiredRank", false).ToString();
                    }
                    else
                        throw new NotSupportedException(String.Format("{0}: Unknown ship", shipyardItem));

                }
                else
                    Debug.Print("!");

                return shipyardObject;

            }
            catch (NotSupportedException ex)
            {
                Program.MainLog.Log(String.Format("Converting error: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting companion data to shipyard object", ex);
            }

            return null;
        }

        public EDDN.CommodityObject GetCommodityFromFDevIDs(IBE.SQL.Datasets.dsEliteDB.tbcommoditybaseDataTable basedata, Newtonsoft.Json.Linq.JToken commodityItem, bool param1)
        {
            EDDN.CommodityObject commodityObject = null;  
            String skuString = null;

            try
            {
               
                if(commodityItem != null)
                { 
                    if (commodityItem.SelectToken("id", false) == null)
                        throw new NotSupportedException(String.Format("Missing id : {0}", commodityItem));

                    SQL.Datasets.dsEliteDB.tbcommoditybaseRow itemData = basedata.FindByid((Int32)commodityItem.SelectToken("id", false));

                    if(itemData != null)
                    {
                        commodityObject = new EDDN.CommodityObject();

                        commodityObject.Id         = itemData.id;
                        commodityObject.Name       = itemData.name;
                        commodityObject.Category   = itemData.category;
                        commodityObject.Average    = itemData.average;
                    }
                    else
                        throw new NotSupportedException(String.Format("Unknown commodity : {0}", commodityItem));

                }
                else
                    Debug.Print("!");

                return commodityObject;

            }
            catch (NotSupportedException ex)
            {
                Program.MainLog.Log(String.Format("Converting error: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting companion data to commodity object", ex);
            }

            return null;
        }

        /// <summary>
        /// returns the name of a ship from it's symbolname
        /// </summary>
        /// <param name="tbshipyardbase"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        internal String GetShipNameFromSymbol(dsEliteDB.tbshipyardbaseDataTable tbshipyardbase, String symbolName)
        {   
            DataRow[] found = tbshipyardbase.Select("symbol = " + SQL.DBConnector.SQLAString(symbolName));

            if(found.Count() > 0)
            {
                return (String)(found[0]["name"]);
            }
            else
            {
                return symbolName;
            }
        }
    }
}
