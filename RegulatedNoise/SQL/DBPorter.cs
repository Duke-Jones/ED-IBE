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

        private String[] BaseTables = new String[] {"tbGovernment", 
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
            Int32 Counter = 0;

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
                        // add a new system
                        var newRow = Data.Tables["tbSystems"].NewRow();
                        CopyEDSystemToDataRow(System, ref newRow);
                        Data.Tables["tbSystems"].Rows.Add(newRow);

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
        /// copies the data from a "EDSystem"-object to a Datarow from table "tbSystems"
        /// </summary>
        /// <param name="SystemObject"></param>
        /// <param name="SystemRow"></param>
        private void CopyEDSystemToDataRow(EDSystem SystemObject, ref System.Data.DataRow SystemRow)
        {
            try
            {

                SystemRow["id"]                     = DBConvert.From(SystemObject.Id);
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
                SystemRow["is_changed"]             = DBConvert.From(0);
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

                    if ((FoundRows != null) && (FoundRows.Count() > 0))
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
                        // add a new Station
                        DataRow   newStationRow           = Data.Tables["tbStations"].NewRow();
                        
                        CopyEDStationToDataRow(Station, ref newStationRow);
                        Data.Tables["tbStations"].Rows.Add(newStationRow);

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
        /// copies the data from a "EDStation"-object to a Datarow from table "tbStations"
        /// </summary>
        /// <param name="SystemObject"></param>
        /// <param name="SystemRow"></param>
        private void CopyEDStationToDataRow(EDStation StationObject, ref System.Data.DataRow StationRow)
        {
            try
            {

                StationRow["id"]                    = DBConvert.From(StationObject.Id);
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
                StationRow["is_changed"]            = DBConvert.From(0);
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
                DataRow[] Existing = EconomyTable.Select("station_id = " + StationObject.Id);

                foreach (String Economy in StationObject.Economies)
                {
                    // get the current new economy id
                    Int32 EconomyID = (Int32)DBConvert.From(BaseTableNameToID("economy", Economy));

                    // and check, if it is already existing
                    object[] Found = Existing.Select(x => x[EconomyID]).ToArray();

                    // if it's not existing, insert it
                    if((Found == null) || (Found.GetUpperBound(0) == -1))
                    {
                        DataRow newRow = EconomyTable.NewRow();

                        newRow["station_id"]        = StationObject.Id;
                        newRow["economy_id"]        = EconomyID;

                        EconomyTable.Rows.Add(newRow);
                    }
                    // otherwise remove it from the memory list
                    else
                    {
                        //EconomyTable.Rows.Remove(Found[0]);
                    }
                }

                // remove all old, deleted data
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
                DataRow[] Existing      = CommodityTable.Select("station_id = " + StationObject.Id);

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
                    object[] Found = Existing.Select(x => x[CommodityID]).ToArray();

                    // if it's not existing, insert it
                    if((Found == null) || (Found.GetUpperBound(0) == -1))
                    {
                        DataRow newRow = CommodityTable.NewRow();

                        newRow["station_id"]    = DBConvert.From(StationObject.Id);
                        newRow["commodity_id"]  = CommodityID;

                        CommodityTable.Rows.Add(newRow);
                    }
                    // otherwise remove it from the memory list
                    else
                    {
                        //EconomyTable.Rows.Remove(Found[0]);
                    }
                }

                // remove all old, deleted data
                foreach (DataRow RemovedRow in Existing)
                    CommodityTable.Rows.Remove(RemovedRow);    

            }
            catch (Exception ex)
            {
                throw new Exception("Error while copying station commodity data", ex);
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

                foreach (String BaseTable in BaseTables)
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
