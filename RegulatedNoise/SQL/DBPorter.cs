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
        private String[] BaseTables = new String[] {"tbGovernment", 
                                                    "tbAllegiance", 
                                                    "tbState", 
                                                    "tbSecurity", 
                                                    "tbEconomy", 
                                                    "tbStationtype"};

        DataSet Data = null;

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

            try{

                Data        = new DataSet();

                PrepareBaseTables(Data);

                // gettin' some freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=0");

                Systems     = JsonConvert.DeserializeObject<List<EDSystem>>(File.ReadAllText(Filename));

                sqlString   = "select * from tbSystems";
                Program.DBCon.TableRead(sqlString, "tbSystems", ref Data);

                sqlString   = "select * from tbSystems_org";
                Program.DBCon.TableRead(sqlString, "tbSystems_org", ref Data);

                foreach (EDSystem System in Systems){

                    FoundRows = Data.Tables["tbSystems"].Select("id=" + System.Id.ToString());

                    if ((FoundRows != null) && (FoundRows.Count() > 0)){
                        // system is existing

                        if ((bool)(FoundRows[0]["is_changed"])){
                            // data is changed by user - hold it ...

                            // ...and check table "tbSystems_org" for the original data
                            FoundRows_org = Data.Tables["tbSystems_org"].Select("id=" + System.Id.ToString());

                            if ((FoundRows_org != null) && (FoundRows_org.Count() > 0)){
                                // system is in "tbSystems_org" existing - keep the newer version 
                                Timestamp_old = (DateTime)(FoundRows_org[0]["updated_at"]);
                                Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(System.UpdatedAt);

                                if (Timestamp_new > Timestamp_old){ 
                                    // data from file is newer
                                    CopyEDSystemToDataRow(System, ref FoundRows_org[0]);
                                    Counter += 1;
                                }
                            }

                        }else{
                            // system is existing - keep the newer version 
                            Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                            Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(System.UpdatedAt);

                            if (Timestamp_new > Timestamp_old){ 
                                // data from file is newer
                                CopyEDSystemToDataRow(System, ref FoundRows[0]);
                                Counter += 1;
                            }
                        }
                    }else{
                        // add a new system
                        var newRow = Data.Tables["tbSystems"].NewRow();
                        CopyEDSystemToDataRow(System, ref newRow);
                        Data.Tables["tbSystems"].Rows.Add(newRow);

                        Counter += 1;
                    }
                    
                    if((Counter > 0) && ((Counter % 100) == 0)){
                        // save changes
                        Debug.Print(Counter.ToString());

                        Program.DBCon.TableUpdate("tbSystems", ref Data);
                        Program.DBCon.TableUpdate("tbSystems_org", ref Data);
                    }

                }

                // save changes
                Program.DBCon.TableUpdate("tbSystems", ref Data, true);
                Program.DBCon.TableUpdate("tbSystems_org", ref Data, true);

                // reset freaky perfomance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }catch (Exception ex){
                try{
                    // reset freaky perfomance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");
                }catch (Exception) { }

                throw new Exception("Error while importing system data", ex);   
            }
        }

        /// <summary>
        /// copies the data from a "EDSystem"-object to a Datarow from table "tbSystems"
        /// </summary>
        /// <param name="SystemObject"></param>
        /// <param name="SystemRow"></param>
        private void CopyEDSystemToDataRow(EDSystem SystemObject, ref System.Data.DataRow SystemRow){
            try{
                
                SystemRow["id"]                     = DBConvert.From(SystemObject.Id);
                SystemRow["systemname"]             = DBConvert.From(SystemObject.Name);
                SystemRow["x"]                      = DBConvert.From(SystemObject.X);
                SystemRow["y"]                      = DBConvert.From(SystemObject.Y);
                SystemRow["z"]                      = DBConvert.From(SystemObject.Z);
                SystemRow["faction"]                = DBConvert.From(SystemObject.Faction);
                SystemRow["population"]             = DBConvert.From(SystemObject.Population);
                SystemRow["government_id"]          = DBConvert.From(BaseTableNameToID("government", SystemObject.Government));
                SystemRow["allegiance_id"]          = DBConvert.From(BaseTableNameToID("allegiance", SystemObject.Allegiance));
                SystemRow["state_id"]               = DBConvert.From(BaseTableNameToID("state"     , SystemObject.State));
                SystemRow["security_id"]            = DBConvert.From(BaseTableNameToID("security"  , SystemObject.Security));
                SystemRow["primary_economy_id"]     = DBConvert.From(BaseTableNameToID("economy"   , SystemObject.PrimaryEconomy));
                SystemRow["needs_permit"]           = DBConvert.From(SystemObject.NeedsPermit);
                SystemRow["updated_at"]             = DBConvert.From(UnixTimeStamp.UnixTimeStampToDateTime(SystemObject.UpdatedAt));
                SystemRow["is_changed"]              = DBConvert.From(0);

            }catch (Exception ex){
                throw new Exception("Error while copying system data", ex);
            }    
        }

        private void PrepareBaseTables(DataSet Data){
            try{

                foreach (String BaseTable in BaseTables){
                    // preload all tables with base data
                    Program.DBCon.Execute(String.Format("select * from {0}", BaseTable), BaseTable, ref Data);
                }

            }catch (Exception ex){
                throw new Exception("Error while preparing base tables", ex);
            }
        }

        /// <summary>
        /// looks for the id of a name from a base table
        /// </summary>
        /// <param name="Tablename">name of the basetable WITHOUT leading 'tb'</param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private object BaseTableNameToID(String Tablename, String Name){
            try{

                if(Name == null)
                    return null;
                else
                    return (Int32)(Data.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("{0} = '{1}'", Tablename, Name))[0]["id"]);

            }catch (Exception ex){
                throw new Exception(String.Format("Error while searching for the id of <{0}> in table <tb{1}>", Name, Tablename), ex);
            }
        }

        /// <summary>
        /// looks for the name of a id from a base table
        /// </summary>
        /// <param name="Tablename">name of the basetable WITHOUT leading 'tb'</param>
        /// <param name="id"></param>
        /// <returns></returns>
        private object BaseTableIDToName(String Tablename, int? id){
            try{

                if(id == null)
                    return null;
                else
                    return (String)(Data.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("id = {0}", id))[0][Tablename]);

            }catch (Exception ex){
                throw new Exception(String.Format("Error while searching for the name of <{0}> in table <tb{1}>", id.ToNString(), Tablename), ex);
            }
        }


        ///// <summary>
        ///// looks for the id of a government name
        ///// </summary>
        ///// <param name="government"></param>
        ///// <returns></returns>
        //private Int32 GovernmentNameToID(String government){
        //    try{
        //        return (Int32)(Data.Tables["tbGovernment"].Select("government = " + DBConnector.SQLAString(government))[0]["id"]);
        //    }catch (Exception ex){
        //        throw new Exception("Error while searching for the government id", ex);
        //    }
        //}

        ///// <summary>
        ///// looks for the id of a allegiance name
        ///// </summary>
        ///// <param name="allegiance"></param>
        ///// <returns></returns>
        //private Int32 AllegianceNameToID(String allegiance){
        //    try{
        //        return (Int32)(Data.Tables["tbAllegiance"].Select("allegiance = " + DBConnector.SQLAString(allegiance))[0]["id"]);
        //    }catch (Exception ex){
        //        throw new Exception("Error while searching for the allegiance id", ex);
        //    }
        //}

        ///// <summary>
        ///// looks for the id of a state name
        ///// </summary>
        ///// <param name="state"></param>
        ///// <returns></returns>
        //private Int32 StateNameToID(String state){
        //    try{
        //        return (Int32)(Data.Tables["tbState"].Select("state = " + DBConnector.SQLAString(state))[0]["id"]);
        //    }catch (Exception ex){
        //        throw new Exception("Error while searching for the state id", ex);
        //    }
        //}

        ///// <summary>
        ///// looks for the id of a security name
        ///// </summary>
        ///// <param name="security"></param>
        ///// <returns></returns>
        //private Int32 securityNameToID(String security){
        //    try{
        //        return (Int32)(Data.Tables["tbSecurity"].Select("security = " + DBConnector.SQLAString(security))[0]["id"]);
        //    }catch (Exception ex){
        //        throw new Exception("Error while searching for the security id", ex);
        //    }
        //}

        ///// <summary>
        ///// looks for the id of a economy name
        ///// </summary>
        ///// <param name="economy"></param>
        ///// <returns></returns>
        //private Int32 economyNameToID(String economy){
        //    try{
        //        return (Int32)(Data.Tables["tbEconomy"].Select("economy = " + DBConnector.SQLAString(economy))[0]["id"]);
        //    }catch (Exception ex){
        //        throw new Exception("Error while searching for the economy id", ex);
        //    }
        //}

    }
}
