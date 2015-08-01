using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegulatedNoise.EDDB_Data;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using RegulatedNoise.Enums_and_Utility_Classes;
using System.Diagnostics;
using System.Globalization;

namespace RegulatedNoise.SQL
{
    class DBPorter
    {
        private enum enCommodityTypes
        {
            import,
            export,
            prohibited
        }

        private String[] BaseTables_Systems = new String[] {"tbGovernment", 
                                                            "tbAllegiance", 
                                                            "tbState", 
                                                            "tbSecurity", 
                                                            "tbEconomy", 
                                                            "tbStationtype",
                                                            "tbCommodity"};

        DataSet Data = null;

#region handling of commodities

        /// <summary>
        /// imports the data from the file into the database
        /// (only newer data will be imported)
        /// </summary>
        /// <param name="Filename"></param>
        internal void ImportCommodities(String Filename)
        {
            String sqlString;
            List<EDCommodities> Commodities;

            try
            {

                Data = new DataSet();

                Commodities = JsonConvert.DeserializeObject<List<EDCommodities>>(File.ReadAllText(Filename));

                // gettin' some freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Program.DBCon.TransBegin();

                // insert or update all commodities
                foreach (EDCommodities Commodity in Commodities)
                {
                    sqlString = "insert into tbCategory(id, category, loccategory) values ("  +
                                Commodity.Category.Id.ToString() + "," +
                                SQL.DBConnector.SQLAString(Commodity.Category.Name.ToString()) + "," +
                                SQL.DBConnector.SQLAString(Commodity.Category.Name.ToString()) +  
                                ") ON DUPLICATE KEY UPDATE " +
                                "category         = " + SQL.DBConnector.SQLAString(Commodity.Category.Name.ToString()) + "," +
                                "loccategory      = " + SQL.DBConnector.SQLAString(Commodity.Category.Name.ToString());

                    Program.DBCon.Execute(sqlString);

                    sqlString = "insert into tbCommodity(id, commodity, loccommodity, category_id, average_price) values ("  +
                                Commodity.Id.ToString() + "," + 
                                SQL.DBConnector.SQLAString(Commodity.Name.ToString()) + "," + 
                                SQL.DBConnector.SQLAString(Commodity.Name.ToString()) + "," + 
                                Commodity.CategoryId.ToString() + "," + 
                                DBConvert.ToString(Commodity.AveragePrice) + 
                                ") ON DUPLICATE KEY UPDATE " +
                                "commodity         = " + SQL.DBConnector.SQLAString(Commodity.Name.ToString()) + "," + 
                                "loccommodity      = " + SQL.DBConnector.SQLAString(Commodity.Name.ToString()) + "," + 
                                "category_id       = " + Commodity.CategoryId.ToString() + "," + 
                                "average_price     = " + DBConvert.ToString(Commodity.AveragePrice);

                    Program.DBCon.Execute(sqlString);
                }

                Program.DBCon.TransCommit();

                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky perfomance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");
                }
                catch (Exception) { }

                throw new Exception("Error while importing system data", ex);
            }
        }

        /// <summary>
        /// loads the localized commodity names and check if 
        /// the self added names now included in the official dictionary
        /// </summary>
        internal void ImportCommodityLocalizations(String Filename)
        {
            DataSet                   Data;
            Dictionary<String, Int32> foundLanguagesFromFile     = new Dictionary<String, Int32>();
            String                    sqlString;
            Int32                     currentSelfCreatedIndex;

            try
            {
                Data = new DataSet();
                Data.ReadXml(Filename);

                sqlString = "select min(id) As min_id from tbCommodity";
                Program.DBCon.Execute(sqlString, "minID", ref Data);

                if(Convert.IsDBNull(Data.Tables["minID"].Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)Data.Tables["minID"].Rows[0]["min_id"]) - 1;
                    if(currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                Program.DBCon.TableRead("select * from tbLanguage", "tbLanguage", ref Data);
                Program.DBCon.TableRead("select * from tbCommodityLocalization", "tbCommodityLocalization", ref Data);
                Program.DBCon.TableRead("select * from tbCommodity", "tbCommodity", ref Data);

                // first check if there's a new language
                foreach (DataColumn LanguageFromFile in Data.Tables["Names"].Columns)
                {
                    DataRow[] LanguageName  = Data.Tables["tbLanguage"].Select("language  = " + DBConnector.SQLAString(LanguageFromFile.ColumnName));

                    if(LanguageName.Count() == 0)
                    {
                        // add a non existing language
                        DataRow newRow  = Data.Tables["tbLanguage"].NewRow();
                        int?    Wert    = DBConvert.To<int?>(Data.Tables["tbLanguage"].Compute("max(id)", ""));

                        if(Wert == null)
                            Wert = 0;

                        newRow["id"]        = Wert;
                        newRow["language"]  = LanguageFromFile.ColumnName;

                        Data.Tables["tbLanguage"].Rows.Add(newRow);

                        foundLanguagesFromFile.Add(LanguageFromFile.ColumnName, (Int32)Wert);
                    }
                    else
                        foundLanguagesFromFile.Add((String)LanguageName[0]["language"], (Int32)LanguageName[0]["id"]);
                    
                }
                
                // submit changes (tbLanguage)
                Program.DBCon.TableUpdate("tbLanguage", ref Data);

                // compare and add the localized names
                foreach (DataRow LocalizationFromFile in Data.Tables["Names"].AsEnumerable())
                {
                    String    BaseName              = (String)LocalizationFromFile[Program.BASE_LANGUAGE];
                    DataRow[] Commodity             = Data.Tables["tbCommodity"].Select("commodity = " + DBConnector.SQLAString(BaseName));

                    if (Commodity.Count() == 0)
                    { 
                        // completely unknown commodity - add first new entry to "tbCommodities"
                        DataRow newRow = Data.Tables["tbCommodity"].NewRow();

                        newRow["id"]            = currentSelfCreatedIndex;
                        newRow["commodity"]     = BaseName;
                        newRow["is_rare"]       = 0;

                        Data.Tables["tbCommodity"].Rows.Add(newRow);

                        currentSelfCreatedIndex -= 1;

                        // submit changes (tbCommodity)
                        Program.DBCon.TableUpdate("tbCommodity", ref Data);

                        Commodity             = Data.Tables["tbCommodity"].Select("commodity = " + DBConnector.SQLAString(BaseName));
                    }

                    foreach (KeyValuePair<String, Int32> LanguageFormFile in foundLanguagesFromFile)
	                {
                        DataRow[] currentLocalizations  = Data.Tables["tbCommodityLocalization"].Select("     commodity_id  = " + Commodity[0]["id"] + 
                                                                                                        " and language_id   = " + LanguageFormFile.Value);

                        if(currentLocalizations.Count() == 0)
                        {
                            // add a new localization
                            DataRow newRow = Data.Tables["tbCommodityLocalization"].NewRow();

                            newRow["commodity_id"]  = Commodity[0]["id"];
                            newRow["language_id"]   = LanguageFormFile.Value;
                            newRow["locname"]       = (String)LocalizationFromFile[LanguageFormFile.Key];

                            Data.Tables["tbCommodityLocalization"].Rows.Add(newRow);
                        }
	                }
                }

                // submit changes
                Program.DBCon.TableUpdate("tbCommodityLocalization", ref Data);

                Program.DBCon.TableReadRemove("tbLanguage");
                Program.DBCon.TableReadRemove("tbCommodityLocalization");
                Program.DBCon.TableReadRemove("tbCommodity");

            }
            catch (Exception ex)
            {
                Program.DBCon.TableReadRemove("tbLanguage");
                Program.DBCon.TableReadRemove("tbCommodityLocalization");
                Program.DBCon.TableReadRemove("tbCommodity");

                throw new Exception("Error while loading commodity names", ex);
            }

        }

        /// <summary>
        /// loads the existing price-warnlevel data
        /// </summary>
        internal void ImportCommodityPriceWarnLevels(String Filename)
        {
            DataSet                           Data;
            List<EDCommoditiesWarningLevels>  WarnLevels;

            WarnLevels = JsonConvert.DeserializeObject<List<EDCommoditiesWarningLevels>>(File.ReadAllText(Filename));

            try
            {
                Data = new DataSet();

                Program.DBCon.TableRead("select * from tbCommodity", "tbCommodity", ref Data);

                // first check if there's a new language
                foreach (EDCommoditiesWarningLevels Warnlevel in WarnLevels)
                {
                    DataRow[] Commodity = Data.Tables["tbCommodity"].Select("commodity = " + DBConnector.SQLAString(Warnlevel.Name));

                    if(Commodity.Count() > 0)
                    {
                        Commodity[0]["pwl_demand_buy_low"]    = Warnlevel.PriceWarningLevel_Demand_Buy_Low;
                        Commodity[0]["pwl_demand_buy_high"]   = Warnlevel.PriceWarningLevel_Demand_Buy_High;
                        Commodity[0]["pwl_supply_buy_low"]    = Warnlevel.PriceWarningLevel_Supply_Buy_Low;
                        Commodity[0]["pwl_supply_buy_high"]   = Warnlevel.PriceWarningLevel_Supply_Buy_High;
                        Commodity[0]["pwl_demand_sell_low"]   = Warnlevel.PriceWarningLevel_Demand_Sell_Low;
                        Commodity[0]["pwl_demand_sell_high"]  = Warnlevel.PriceWarningLevel_Demand_Sell_High;
                        Commodity[0]["pwl_supply_sell_low"]   = Warnlevel.PriceWarningLevel_Supply_Sell_Low;
                        Commodity[0]["pwl_supply_sell_high"]  = Warnlevel.PriceWarningLevel_Supply_Sell_High;
                    }
                }
                
                // submit changes (tbLanguage)
                Program.DBCon.TableUpdate("tbCommodity", ref Data);

                Program.DBCon.TableReadRemove("tbCommodity");

            }
            catch (Exception ex)
            {
                Program.DBCon.TableReadRemove("tbCommodity");

                throw new Exception("Error while loading commodity names", ex);
            }

        }

#endregion

#region handling of systems

        /// <summary>
        /// imports the data from the file into the database
        /// (only newer data will be imported)
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportSystems(String Filename)
        {
            String sqlString;
            List<EDSystem> Systems;
            DataRow[] FoundRows, FoundRows_org;
            DateTime Timestamp_new, Timestamp_old;
            Int32 Counter = 0;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<Int32, Int32>();

            try
            {
                Data = new DataSet();

                PrepareBaseTables(Data);

                // gettin' some freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Systems = JsonConvert.DeserializeObject<List<EDSystem>>(File.ReadAllText(Filename));

                Program.DBCon.TransBegin();

                sqlString = "select * from tbSystems lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbSystems", ref Data);

                sqlString = "select * from tbSystems_org lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbSystems_org", ref Data);

                foreach (EDSystem System in Systems)
                {
                    FoundRows = Data.Tables["tbSystems"].Select("id=" + System.Id.ToString());

                    if ((FoundRows != null) && (FoundRows.Count() > 0))
                    {
                        // system is existing

                        if ((bool)(FoundRows[0]["is_changed"]))
                        {
                            // data is changed by user - hold it ...

                            // ...and check table "tbSystems_org" for the original data
                            FoundRows_org = Data.Tables["tbSystems_org"].Select("id=" + System.Id.ToString());

                            if ((FoundRows_org != null) && (FoundRows_org.Count() > 0))
                            {
                                // system is in "tbSystems_org" existing - keep the newer version 
                                Timestamp_old = (DateTime)(FoundRows_org[0]["updated_at"]);
                                Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(System.UpdatedAt);

                                if (Timestamp_new > Timestamp_old)
                                {
                                    // data from file is newer
                                    CopyEDSystemToDataRow(System, ref FoundRows_org[0]);
                                    Counter += 1;
                                }
                            }
                        }
                        else
                        {
                            // system is existing - keep the newer version 
                            Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                            Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(System.UpdatedAt);

                            if (Timestamp_new > Timestamp_old)
                            {
                                // data from file is newer
                                CopyEDSystemToDataRow(System, ref FoundRows[0]);
                                Counter += 1;
                            }
                        }
                    }
                    else
                    {
                        // check if theres a user generated system
                        // self-created systems don't have the correct id so it must be identified by name    
                        FoundRows = Data.Tables["tbSystems"].Select("     systemname = " + DBConnector.SQLAString(System.Name.ToString()) +
                                                                    " and id         < 0");

                        if (FoundRows.Count() > 0)
                        {
                            // self created systems is existing -> correct id and get new data from EDDB
                            // (changed system_id in tbStations are automatically internal updated by the database itself)
                            CopyEDSystemToDataRow(System, ref FoundRows[0]);
                        }
                        else
                        {
                            // add a new system
                            var newRow = Data.Tables["tbSystems"].NewRow();
                            CopyEDSystemToDataRow(System, ref newRow);
                            Data.Tables["tbSystems"].Rows.Add(newRow);
                        }

                        Counter += 1;
                    }

                    if ((Counter > 0) && ((Counter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Systems : " + Counter.ToString());

                        Program.DBCon.TableUpdate("tbSystems", ref Data);
                        Program.DBCon.TableUpdate("tbSystems_org", ref Data);
                    }

                }

                // save changes
                Program.DBCon.TableUpdate("tbSystems", ref Data, true);
                Program.DBCon.TableUpdate("tbSystems_org", ref Data, true);

                Program.DBCon.TransCommit();

                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky perfomance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove("tbSystems");
                    Program.DBCon.TableReadRemove("tbSystems_org");

                }
                catch (Exception) { }

                throw new Exception("Error while importing system data", ex);
            }
        }

        /// <summary>
        /// imports the own data from the file into the database (only initially once needed)
        /// </summary>
        /// <param name="Filename"></param>
        public Dictionary<Int32, Int32> ImportSystems_Own(String Filename)
        {
            String sqlString;
            List<EDSystem> Systems;
            DataRow[] FoundRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 Counter = 0;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<Int32, Int32>();
            Int32 currentSelfCreatedIndex = -1;

            try
            {

                Data = new DataSet();

                PrepareBaseTables(Data);

                // gettin' some freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Systems = JsonConvert.DeserializeObject<List<EDSystem>>(File.ReadAllText(Filename));

                Program.DBCon.TransBegin();

                sqlString = "select * from tbSystems lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbSystems", ref Data);

                sqlString = "select * from tbSystems_org lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbSystems_org", ref Data);

                sqlString = "select min(id) As min_id from tbSystems";
                Program.DBCon.Execute(sqlString, "minID", ref Data);

                if(Convert.IsDBNull(Data.Tables["minID"].Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)Data.Tables["minID"].Rows[0]["min_id"]) - 1;
                    if(currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                foreach (EDSystem System in Systems)
                {
                    // self-created systems don't have the correct id so it must be identified by name    
                    FoundRows = Data.Tables["tbSystems"].Select("systemname=" + DBConnector.SQLAString(System.Name.ToString()));

                    if ((FoundRows != null) && (FoundRows.Count() > 0))
                    {
                        // system is existing
                        // memorize the changed system ids for importing user changed stations in the (recommend) second step
                        changedSystemIDs.Add(System.Id, (Int32)FoundRows[0]["id"]);
                        System.Id = (Int32)FoundRows[0]["id"];

                        if ((bool)(FoundRows[0]["is_changed"]))
                        {
                            // old data is changed by user and the new data is also a user changed data
                            // keep the newer version in the main table 
                            Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                            Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(System.UpdatedAt);

                            if (Timestamp_new > Timestamp_old)
                            {
                                // data from file is newer -> take it but hold the old id
                                CopyEDSystemToDataRow(System, ref FoundRows[0], true);

                                Counter += 1;
                            }
                        }
                        else
                        {
                            // new data is user changed data, old data is original data
                            // copy the original data ("tbSystems") to the saving data table ("tbSystems_org")
                            // and get the correct system ID
                            Data.Tables["tbSystems_org"].LoadDataRow(FoundRows[0].ItemArray, false);
                            CopyEDSystemToDataRow(System, ref FoundRows[0], true);

                            Counter += 1;
                        }
                    }
                    else
                    {
                        // add a new system
                        // memorize the changed system ids for importing user changed stations in the (recommend) second step
                        changedSystemIDs.Add(System.Id, currentSelfCreatedIndex);
                        System.Id = currentSelfCreatedIndex;

                        var newRow = Data.Tables["tbSystems"].NewRow();
                        CopyEDSystemToDataRow(System, ref newRow, true);
                        Data.Tables["tbSystems"].Rows.Add(newRow);


                        currentSelfCreatedIndex -= 1;
                        Counter += 1;
                    }

                    if ((Counter > 0) && ((Counter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Systems : " + Counter.ToString());

                        Program.DBCon.TableUpdate("tbSystems", ref Data);
                        Program.DBCon.TableUpdate("tbSystems_org", ref Data);
                    }

                }

                // save changes
                Program.DBCon.TableUpdate("tbSystems", ref Data, true);
                Program.DBCon.TableUpdate("tbSystems_org", ref Data, true);

                Program.DBCon.TransCommit();

                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                // return all changed ids
                return changedSystemIDs;
            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky perfomance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove("tbSystems");
                    Program.DBCon.TableReadRemove("tbSystems_org");

                }
                catch (Exception) { }

                throw new Exception("Error while importing system data", ex);
            }
        }

        /// <summary>
        /// copies the data from a "EDSystem"-object to a Datarow from table "tbSystems"
        /// </summary>
        /// <param name="SystemObject"></param>
        /// <param name="SystemRow"></param>
        private void CopyEDSystemToDataRow(EDSystem SystemObject, ref System.Data.DataRow SystemRow, Boolean OwnData = false, int? SystemID = null)
        {
            try
            {

                SystemRow["id"]                     = SystemID == null ? DBConvert.From(SystemObject.Id) : SystemID;
                SystemRow["systemname"]             = DBConvert.From(SystemObject.Name);
                SystemRow["x"]                      = DBConvert.From(SystemObject.X);
                SystemRow["y"]                      = DBConvert.From(SystemObject.Y);
                SystemRow["z"]                      = DBConvert.From(SystemObject.Z);
                SystemRow["faction"]                = DBConvert.From(SystemObject.Faction);
                SystemRow["population"]             = DBConvert.From(SystemObject.Population);
                SystemRow["government_id"]          = DBConvert.From(BaseTableNameToID("government", SystemObject.Government));
                SystemRow["allegiance_id"]          = DBConvert.From(BaseTableNameToID("allegiance", SystemObject.Allegiance));
                SystemRow["state_id"]               = DBConvert.From(BaseTableNameToID("state", SystemObject.State));
                SystemRow["security_id"]            = DBConvert.From(BaseTableNameToID("security", SystemObject.Security));
                SystemRow["primary_economy_id"]     = DBConvert.From(BaseTableNameToID("economy", SystemObject.PrimaryEconomy));
                SystemRow["needs_permit"]           = DBConvert.From(SystemObject.NeedsPermit);
                SystemRow["updated_at"]             = DBConvert.From(UnixTimeStamp.UnixTimeStampToDateTime(SystemObject.UpdatedAt));
                SystemRow["is_changed"]             = OwnData ? DBConvert.From(1) : DBConvert.From(0);
                SystemRow["visited"]                = DBConvert.From(0);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while copying system data", ex);
            }
        }

#endregion

#region handling of stations

        /// <summary>
        /// imports the data from the file into the database
        /// (only newer data will be imported)
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportStations(String Filename)
        {
            String sqlString;
            List<EDStation> Stations;
            DataRow[] FoundRows, FoundRows_org;
            DateTime Timestamp_new, Timestamp_old;
            Int32 Counter = 0;

            try
            {
                Data = new DataSet();

                PrepareBaseTables(Data);

                // gettin' some freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Stations = JsonConvert.DeserializeObject<List<EDStation>>(File.ReadAllText(Filename));

                Program.DBCon.TransBegin();

                sqlString = "select * from tbStations lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbStations", ref Data);
                sqlString = "select * from tbStations_org lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbStations_org", ref Data);
                sqlString = "select * from tbStationEconomy lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbStationEconomy", ref Data);
                sqlString = "select * from tbImportCommodity lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbImportCommodity", ref Data);
                sqlString = "select * from tbExportCommodity lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbExportCommodity", ref Data);
                sqlString = "select * from tbProhibitedCommodity lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbProhibitedCommodity", ref Data);

                foreach (EDStation Station in Stations)
                {

                    FoundRows = Data.Tables["tbStations"].Select("id=" + Station.Id.ToString());

                    if (FoundRows.Count() > 0)
                    {
                        // Station is existing

                        if ((bool)(FoundRows[0]["is_changed"]))
                        {
                            // data is changed by user - hold it ...

                            // ...and check table "tbStations_org" for the original data
                            FoundRows_org = Data.Tables["tbStations_org"].Select("id=" + Station.Id.ToString());

                            if ((FoundRows_org != null) && (FoundRows_org.Count() > 0))
                            {
                                // Station is in "tbStations_org" existing - keep the newer version 
                                Timestamp_old = (DateTime)(FoundRows_org[0]["updated_at"]);
                                Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(Station.UpdatedAt);

                                if (Timestamp_new > Timestamp_old)
                                {
                                    // data from file is newer
                                    CopyEDStationToDataRow(Station, ref FoundRows_org[0]);

                                    CopyEDStationEconomiesToDataRows(Station, Data.Tables["tbStationEconomy"]);
                                    CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbImportCommodity"]);
                                    CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbExportCommodity"]);
                                    CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbProhibitedCommodity"]);

                                    Counter += 1;

                                }
                            }

                        }
                        else
                        {
                            // Station is existing - keep the newer version 
                            Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                            Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(Station.UpdatedAt);

                            if (Timestamp_new > Timestamp_old)
                            {
                                // data from file is newer
                                CopyEDStationToDataRow(Station, ref FoundRows[0]);

                                CopyEDStationEconomiesToDataRows(Station, Data.Tables["tbStationEconomy"]);
                                CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbImportCommodity"]);
                                CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbExportCommodity"]);
                                CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbProhibitedCommodity"]);

                                Counter += 1;
                            }
                        }
                    }
                    else
                    {

                        // self-created stations don't have the correct id so they must be identified by name    
                        FoundRows = Data.Tables["tbStations"].Select("stationname = " + DBConnector.SQLAString(Station.Name.ToString()) + " and " + 
                                                                     "  system_id = " + Station.SystemId + " and " + 
                                                                     "  id        < 0");

                        if (FoundRows.Count() > 0)
                        {
                            // self created station is existing -> correct id and get new data from EDDB
                            CopyEDStationToDataRow(Station, ref FoundRows[0]); 

                            // update immediately because otherwise the references are wrong after changing a id
                            Program.DBCon.TableUpdate("tbStations", ref Data);
                            Program.DBCon.TableUpdate("tbStations_org", ref Data);
                            Program.DBCon.TableUpdate("tbStationEconomy", ref Data);
                            Program.DBCon.TableUpdate("tbImportCommodity", ref Data);
                            Program.DBCon.TableUpdate("tbExportCommodity", ref Data);
                            Program.DBCon.TableUpdate("tbProhibitedCommodity", ref Data);

                            Program.DBCon.TableRefresh("tbStationEconomy", ref Data);
                            Program.DBCon.TableRefresh("tbImportCommodity", ref Data);
                            Program.DBCon.TableRefresh("tbExportCommodity", ref Data);
                            Program.DBCon.TableRefresh("tbProhibitedCommodity", ref Data);
                        }
                        else
                        {
                            // add a new Station
                            DataRow   newStationRow           = Data.Tables["tbStations"].NewRow();
                        
                            CopyEDStationToDataRow(Station, ref newStationRow);
                            Data.Tables["tbStations"].Rows.Add(newStationRow);
                        }

                        CopyEDStationEconomiesToDataRows(Station, Data.Tables["tbStationEconomy"]);
                        CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbImportCommodity"]);
                        CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbExportCommodity"]);
                        CopyEDStationCommodityToDataRow(Station,  Data.Tables["tbProhibitedCommodity"]);

                        Counter += 1;
                    }

                    if ((Counter > 0) && ((Counter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Stations : " + Counter.ToString());

                        Program.DBCon.TableUpdate("tbStations", ref Data);
                        Program.DBCon.TableUpdate("tbStations_org", ref Data);
                        Program.DBCon.TableUpdate("tbStationEconomy", ref Data);
                        Program.DBCon.TableUpdate("tbImportCommodity", ref Data);
                        Program.DBCon.TableUpdate("tbExportCommodity", ref Data);
                        Program.DBCon.TableUpdate("tbProhibitedCommodity", ref Data);
                    }

                }

                // save changes
                Program.DBCon.TableUpdate("tbStations", ref Data, true);
                Program.DBCon.TableUpdate("tbStations_org", ref Data, true);
                Program.DBCon.TableUpdate("tbStationEconomy", ref Data, true);
                Program.DBCon.TableUpdate("tbImportCommodity", ref Data, true);
                Program.DBCon.TableUpdate("tbExportCommodity", ref Data, true);
                Program.DBCon.TableUpdate("tbProhibitedCommodity", ref Data, true);

                Program.DBCon.TransCommit();

                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky perfomance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove("tbStations");
                    Program.DBCon.TableReadRemove("tbStations_org");
                    Program.DBCon.TableReadRemove("tbStationEconomy");
                    Program.DBCon.TableReadRemove("tbImportCommodity");
                    Program.DBCon.TableReadRemove("tbExportCommodity");
                    Program.DBCon.TableReadRemove("tbProhibitedCommodity");

                }
                catch (Exception) { }

                throw new Exception("Error while importing Station data", ex);
            }
        }

        /// <summary>
        /// imports the own data from the file into the database (only initially once needed)
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportStations_Own(String Filename, Dictionary<Int32, Int32> changedSystemIDs)
        {
            String sqlString;
            List<EDStation> Stations;
            DataRow[] FoundRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 Counter = 0;
            Int32 currentSelfCreatedIndex = -1;

            try
            {
                Data = new DataSet();

                PrepareBaseTables(Data);

                // gettin' some freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Stations = JsonConvert.DeserializeObject<List<EDStation>>(File.ReadAllText(Filename));

                Program.DBCon.TransBegin();

                sqlString = "select * from tbStations lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbStations", ref Data);
                sqlString = "select * from tbStations_org lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbStations_org", ref Data);
                sqlString = "select * from tbStationEconomy lock in share mode";
                Program.DBCon.TableRead(sqlString, "tbStationEconomy", ref Data);

                // get the smallest ID for self added stations
                sqlString = "select min(id) As min_id from tbStations";
                Program.DBCon.Execute(sqlString, "minID", ref Data);

                if(Convert.IsDBNull(Data.Tables["minID"].Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)Data.Tables["minID"].Rows[0]["min_id"]) - 1;
                    if(currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                foreach (EDStation Station in Stations)
                {
                    Int32 SystemID;

                    // is the system id changed ? --> get the new system id, otherwise the original
                    if( changedSystemIDs.TryGetValue(Station.SystemId, out SystemID) )
                        Station.SystemId = SystemID;

                    // self-created stations don't have the correct id so they must be identified by name    
                    FoundRows = Data.Tables["tbStations"].Select("stationname=" + DBConnector.SQLAString(Station.Name.ToString()) + " and " + 
                                                                 "system_id =  " + Station.SystemId);

                    if ((FoundRows != null) && (FoundRows.Count() > 0))
                    {
                        // Station is existing, get the same Id
                        Station.Id = (Int32)FoundRows[0]["id"];

                        if ((bool)(FoundRows[0]["is_changed"]))
                        {
                            // existing data data is also changed by user - keep the newer version 
                            Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                            Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(Station.UpdatedAt);

                            if (Timestamp_new > Timestamp_old)
                            {
                                // data from file is newer
                                CopyEDStationToDataRow(Station, ref FoundRows[0], true);

                                CopyEDStationEconomiesToDataRows(Station, Data.Tables["tbStationEconomy"]);
                                // commodities are not considered because there was no possibility for input in the old RN

                                Counter += 1;
                            }
                        }
                        else
                        {
                            // new data is user changed data, old data is original data
                            // copy the original data ("tbSystems") to the saving data table ("tbSystems_org")
                            // and get the correct system ID
                            Data.Tables["tbStations_org"].LoadDataRow(FoundRows[0].ItemArray, false);
                            CopyEDStationToDataRow(Station, ref FoundRows[0], true);
                            
                            CopyEDStationEconomiesToDataRows(Station, Data.Tables["tbStationEconomy"]);
                            // commodities are not considered because there was no possibility for input in the old RN

                            Counter += 1;
                        }
                    }
                    else
                    {
                        // add a new system
                        Station.Id = currentSelfCreatedIndex;

                        var newRow = Data.Tables["tbStations"].NewRow();
                        
                        CopyEDStationToDataRow(Station, ref newRow, true);
                        Data.Tables["tbStations"].Rows.Add(newRow);

                        currentSelfCreatedIndex -= 1;
                        Counter += 1;

                        CopyEDStationEconomiesToDataRows(Station, Data.Tables["tbStationEconomy"]);
                        // commodities are not considered because there was no possibility for input in the old RN

                        Counter += 1;
                    }

                    if ((Counter > 0) && ((Counter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Stations : " + Counter.ToString());

                        Program.DBCon.TableUpdate("tbStations", ref Data);
                        Program.DBCon.TableUpdate("tbStations_org", ref Data);
                        Program.DBCon.TableUpdate("tbStationEconomy", ref Data);
                    }

                }

                // save changes
                Program.DBCon.TableUpdate("tbStations", ref Data, true);
                Program.DBCon.TableUpdate("tbStations_org", ref Data, true);
                Program.DBCon.TableUpdate("tbStationEconomy", ref Data, true);

                Program.DBCon.TransCommit();

                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky perfomance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove("tbStations");
                    Program.DBCon.TableReadRemove("tbStations_org");
                    Program.DBCon.TableReadRemove("tbStationEconomy");
                }
                catch (Exception) { }

                throw new Exception("Error while importing Station data", ex);
            }
        }

        /// <summary>
        /// copies the data from a "EDStation"-object to a Datarow from table "tbStations"
        /// </summary>
        /// <param name="SystemObject"></param>
        /// <param name="SystemRow"></param>
        private void CopyEDStationToDataRow(EDStation StationObject, ref System.Data.DataRow StationRow, Boolean OwnData = false, int? StationID = null)
        {
            try
            {

                StationRow["id"]                    = StationID == null ? DBConvert.From(StationObject.Id) : StationID;
                StationRow["stationname"]           = DBConvert.From(StationObject.Name);
                StationRow["system_id"]             = DBConvert.From(StationObject.SystemId);
                StationRow["max_landing_pad_size"]  = DBConvert.From(StationObject.MaxLandingPadSize);
                StationRow["distance_to_star"]      = DBConvert.From(StationObject.DistanceToStar);
                StationRow["faction"]               = DBConvert.From(StationObject.Faction);
                StationRow["government_id"]         = DBConvert.From(BaseTableNameToID("government", StationObject.Government));
                StationRow["allegiance_id"]         = DBConvert.From(BaseTableNameToID("allegiance", StationObject.Allegiance));
                StationRow["state_id"]              = DBConvert.From(BaseTableNameToID("state", StationObject.State));
                StationRow["stationtype_id"]        = DBConvert.From(BaseTableNameToID("stationtype", StationObject.Type));
                StationRow["has_blackmarket"]       = DBConvert.From(StationObject.HasBlackmarket);
                StationRow["has_commodities"]       = DBConvert.From(StationObject.HasCommodities);
                StationRow["has_refuel"]            = DBConvert.From(StationObject.HasRefuel);
                StationRow["has_repair"]            = DBConvert.From(StationObject.HasRepair);
                StationRow["has_rearm"]             = DBConvert.From(StationObject.HasRearm);
                StationRow["has_outfitting"]        = DBConvert.From(StationObject.HasOutfitting);
                StationRow["updated_at"]            = DBConvert.From(UnixTimeStamp.UnixTimeStampToDateTime(StationObject.UpdatedAt));
                StationRow["is_changed"]            = OwnData ? DBConvert.From(1) : DBConvert.From(0);
                StationRow["visited"]               = DBConvert.From(0);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while copying station data", ex);
            }
        }

        /// <summary>
        /// copies the economies data from a "EDStation"-object to "tbStationEconomy"-table
        /// </summary>
        /// <param name="StationObject"></param>
        /// <param name="EconomyRow"></param>
        /// <param name=""></param>
        private void CopyEDStationEconomiesToDataRows(EDStation StationObject, System.Data.DataTable EconomyTable)
        {
            try
            {

                // get all existing economies from the database (memory list)
                List<DataRow> Existing = EconomyTable.Select("station_id = " + StationObject.Id).ToList();

                foreach (String Economy in StationObject.Economies)
                {
                    // get the current new economy id
                    Int32 EconomyID = (Int32)DBConvert.From(BaseTableNameToID("economy", Economy));

                    // and check, if it is already existing
                    var Found = from DataRow relevantEconomy in Existing
                                where relevantEconomy.Field<Int32>("economy_id") == EconomyID
                                select relevantEconomy;
 
                    // if it's not existing, insert it
                    if(Found.Count() == 0)
                    {
                        DataRow newRow = EconomyTable.NewRow();

                        newRow["station_id"]        = StationObject.Id;
                        newRow["economy_id"]        = EconomyID;

                        EconomyTable.Rows.Add(newRow);

                    }
                    else
                    {
                        Existing.Remove(Found.First());
                    }
                }

                // remove all old, not more existing data
                foreach (DataRow RemovedRow in Existing)
                    EconomyTable.Rows.Remove(RemovedRow);    

            }
            catch (Exception ex)
            {
                throw new Exception("Error while copying station economy data", ex);
            }
        }

        /// <summary>
        /// copies the commodities data from a "EDStation"-object to "tb_______Commodity"-table
        /// </summary>
        /// <param name="StationObject"></param>
        /// <param name="EconomyRow"></param>
        private void CopyEDStationCommodityToDataRow(EDStation StationObject, DataTable CommodityTable)
        {
            try
            {
                string[] Commodities    = null;

                // get all existing economies from the database (memory list)
                List<DataRow> Existing      = CommodityTable.Select("station_id = " + StationObject.Id).ToList();

                switch (CommodityTable.TableName)
                {
                    case "tbImportCommodity":
                        Commodities = StationObject.ImportCommodities;
                    break;
                    case "tbExportCommodity":
                        Commodities = StationObject.ExportCommodities;
                    break;
                    case "tbProhibitedCommodity":
                        Commodities = StationObject.ProhibitedCommodities;
                    break;
                }

                foreach (String Commodity in Commodities)
                {
                    // get the current new  id
                    Int32 CommodityID = (Int32)DBConvert.From(BaseTableNameToID("commodity", Commodity));

                    // and check, if it is already existing
                    var Found = from DataRow relevantCommodity in Existing
                                where relevantCommodity.Field<Int32>("commodity_id") == CommodityID
                                select relevantCommodity;

                    // if it's not existing, insert it
                    if(Found.Count() == 0)
                    {
                        DataRow newRow = CommodityTable.NewRow();

                        newRow["station_id"]    = DBConvert.From(StationObject.Id);
                        newRow["commodity_id"]  = CommodityID;

                        CommodityTable.Rows.Add(newRow);
                    }
                    else
                    {
                        Existing.Remove(Found.First());
                    }
                }

                // remove all old, not more existing data
                foreach (DataRow RemovedRow in Existing)
                    CommodityTable.Rows.Remove(RemovedRow);    

            }
            catch (Exception ex)
            {
                throw new Exception("Error while copying station commodity data", ex);
            }
        }


#endregion

#region Commander's Log

        /// <summary>
        /// imports the "Commander's Log" into the database
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportCommandersLog(String Filename)
        {
            DataSet Data;
            String  sqlString;
            Int32   added = 0;

            try
            {
                Data = new DataSet();

                Data.ReadXml(Filename);

                // gettin' some freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                //Program.DBCon.TableRead("select * from tbLog for update", "tbLog", ref Data);
                if(Data.Tables.Contains("CommandersLogEvent"))
                    foreach(DataRow Event in Data.Tables["CommandersLogEvent"].AsEnumerable())
                    {
                        DateTime EventTime = DateTime.Parse((String)Event["EventDate"], CultureInfo.CurrentUICulture , DateTimeStyles.None);
                        String   Station   = StructureHelper.CombinedNameToStationName((String)Event["Station"]);
                        String   EventType = Event["EventType"].ToString().Trim().Length == 0 ? "Other" : Event["EventType"].ToString().Trim();

                        sqlString = String.Format("INSERT INTO tbLog(time, system_id, station_id, event_id, commodity_id," +
                                                  "                  cargoaction_id, cargovolume, credits_transaction, credits_total, notes)" +
                                                  " SELECT d.* FROM (SELECT" +
                                                  "          {0} AS time," +
                                                  "          (select id from tbSystems  where systemname  = {1}" +
                                                  "          ) AS system_id," +
                                                  "          (select id from tbStations where stationname = {2} " + 
                                                  "                                     and   system_id   = (select id from tbSystems" + 
                                                  "                                                           where systemname = {1})" +
                                                  "          ) AS station_id," +
                                                  "          (select id from tbEventType   where event     = {3}) As event_id," +
                                                  "          (select id from tbCommodity   where commodity = {4} or loccommodity = {4} limit 1) As commodity_id," +
                                                  "          (select id from tbCargoAction where action    = {5}) AS cargoaction_id," +
                                                  "          {6} AS cargovolume," +
                                                  "          {7} AS credits_transaction," +
                                                  "          {8} AS credits_total," +
                                                  "          {9} AS notes) AS d" +
                                                  " WHERE 0 IN (SELECT COUNT(*)" +
                                                  "                     FROM tbLog" +
                                                  "                     WHERE time     = {0})",
                                                  DBConnector.SQLDateTime(EventTime), 
                                                  DBConnector.SQLAString(Event["System"].ToString()),
                                                  DBConnector.SQLAString(Station), 
                                                  DBConnector.SQLAString(EventType),
                                                  DBConnector.SQLAString(Event["Cargo"].ToString()),
                                                  DBConnector.SQLAString(Event["CargoAction"].ToString()),
                                                  Event["CargoVolume"],
                                                  Event["TransactionAmount"],
                                                  Event["Credits"],
                                                  Event["Notes"].ToString().Trim() == String.Empty ? "null" : String.Format("'{0}'", DBConnector.SQLEscape(Event["Notes"].ToString())));

                        added += Program.DBCon.Execute(sqlString);

                        if ((added > 0) && ((added % 10) == 0))
                            Debug.Print(added.ToString());
                    }

                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                throw new Exception("Error when importing the Commander's Log ", ex);
            }
        }

#endregion

        #region basetable handling

        /// <summary>
        /// loads the data from the basetables into memory
        /// </summary>
        /// <param name="Data"></param>
        private void PrepareBaseTables(DataSet Data)
        {
            try
            {

                foreach (String BaseTable in BaseTables_Systems)
                {
                    // preload all tables with base data
                    Program.DBCon.Execute(String.Format("select * from {0}", BaseTable), BaseTable, ref Data);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while preparing base tables", ex);
            }
        }

        /// <summary>
        /// looks for the id of a name from a base table
        /// </summary>
        /// <param name="Tablename">name of the basetable WITHOUT leading 'tb'</param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private object BaseTableNameToID(String Tablename, String Name)
        {
            try
            {

                if (Name == null)
                    return null;
                else
                    return (Int32)(Data.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("{0} = '{1}'", Tablename, Name))[0]["id"]);

            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error while searching for the id of <{0}> in table <tb{1}>", Name, Tablename), ex);
            }
        }

        /// <summary>
        /// looks for the name of a id from a base table
        /// </summary>
        /// <param name="Tablename">name of the basetable WITHOUT leading 'tb'</param>
        /// <param name="id"></param>
        /// <returns></returns>
        private object BaseTableIDToName(String Tablename, int? id)
        {
            try
            {

                if (id == null)
                    return null;
                else
                    return (String)(Data.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("id = {0}", id))[0][Tablename]);

            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error while searching for the name of <{0}> in table <tb{1}>", id.ToNString(), Tablename), ex);
            }
        }

    }

#endregion

}
