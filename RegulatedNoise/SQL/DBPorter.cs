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
using RegulatedNoise.SQL.Datasets;
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

        /// <summary>
        /// constructor
        /// </summary>
        public DBPorter()
        {
            try 
	        {	        
                m_EventTimer = new PerformanceTimer();
                m_EventTimer.startMeasuring();
	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error in constructor", ex);
	        }
        }

#region event handler

        private const Int32 m_TimeSlice_ms    = 100;
        private const Int32 m_PercentSlice    = 10;
        private PerformanceTimer m_EventTimer = new PerformanceTimer();
        private Int32 m_lastProgress          = 0;

        /// <summary>
        /// checks if the import of old data is done or sets the value
        /// </summary>
        public Boolean OldDataImportDone
        {
            get
            {
                return Program.DBCon.getIniValue<Boolean>("Database", "OldImportDone", "false", false, false);
            }
            set
            {
                Program.DBCon.setIniValue("Database", "OldImportDone", value.ToString());
            }
        }

        public event EventHandler<ProgressEventArgs> Progress;

        protected virtual void OnProgress(ProgressEventArgs e)
        {
            EventHandler<ProgressEventArgs> myEvent = Progress;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class ProgressEventArgs : EventArgs
        {
            public String Tablename { get; set; }
            public Int32  Index { get; set; }
            public Int32  Total { get; set; }
        }

        /// <summary>
        /// fires the progress event if necessary
        /// </summary>
        /// <param name="Tablename"></param>
        /// <param name="Index"></param>
        /// <param name="Total"></param>
        private void sendProgressEvent(String Tablename, Int32 Index, Int32 Total)
        {
            Boolean send = false;
            Int32   rest;
            Int32   ProgressSendLevel;
            try 
	        {	        
		        if(m_EventTimer.currentMeasuring() > m_TimeSlice_ms)
                {
                    // time is reason
                    Progress.Raise(this, new ProgressEventArgs() { Tablename = Tablename, Index = Index, Total = Total});

                    ProgressSendLevel =  ((100 * Index/Total) / 10) * 10;

                    m_EventTimer.startMeasuring();
                    m_lastProgress = ProgressSendLevel;

                    Debug.Print("Progress (t):" + ProgressSendLevel.ToString());
                }
                else
                { 
                    if(Index == 2 && Total == 8)
                        Debug.Print("!");

                    // progress is reason
                    ProgressSendLevel =  ((100 * Index/Total) / 10) * 10;
                    
                    if(((((100 * Index/Total) % m_PercentSlice) == 0) && (ProgressSendLevel != m_lastProgress)) || 
                        (ProgressSendLevel != m_lastProgress))
                    { 
                        // time is reason
                        Progress.Raise(this, new ProgressEventArgs() { Tablename = Tablename, Index = Index, Total = Total});

                        m_EventTimer.startMeasuring();
                        m_lastProgress = ProgressSendLevel;
                        Debug.Print("Progress (l):" + ProgressSendLevel.ToString());
                    }
                }
	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while checking for progress event needed", ex);
	        }
        }

#endregion

#region "import data"

        

        /// <summary>
        /// checks if the database is already initialized
        /// </summary>
        /// <returns></returns>
        internal Boolean checkDBVersion()
        {
            Boolean retValue = true;
            try
            {
                Decimal DBVersion = Program.DBCon.getIniValue<Decimal>("Database", "Version", Program.DB_VERSION_NONE.ToString(), false);

                if(DBVersion == Program.DB_VERSION_NONE)
                    retValue = false;
                
                return retValue;
                   
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking db-version", ex);
            }
        }

        internal void InitializeData()
        {
            String FileName;
            String RNPath;
            var fbFolderDialog = new System.Windows.Forms.FolderBrowserDialog();

            try
            {
                /// muss auch klappen, wenn der user eine erstinstallation macht

                fbFolderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                fbFolderDialog.Description = "First run. It's neccessary to import the data from your existing (old) installation." + Environment.NewLine +
                                             "Select your RN-Folder with the old data files ....";
                fbFolderDialog.SelectedPath = System.IO.Directory.GetCurrentDirectory();
                fbFolderDialog.ShowDialog();

                RNPath = fbFolderDialog.SelectedPath.Trim();

                if (!String.IsNullOrEmpty(RNPath))
                {
                    // import the commodities from EDDB
                    Program.Data.ImportCommodities(Path.Combine(RNPath, @"Data\commodities.json"));

                    // import the localizations from the old RN files
                    Program.Data.ImportCommodityLocalizations(Path.Combine(RNPath, @"Data\Commodities.xml"));

                    // import the self added localizations from the old RN files
                    Program.Data.ImportCommodityLocalizations(Path.Combine(RNPath, @"Data\Commodities_Own.xml"));

                    // import the pricewarnlevels from the old RN files
                    Program.Data.ImportCommodityPriceWarnLevels(Path.Combine(RNPath, @"Data\Commodities_RN.json"));

                    // import the systems and stations from EDDB
                    Program.Data.ImportSystems(Path.Combine(RNPath, @"Data/systems.json"));
                    Program.Data.ImportStations(Path.Combine(RNPath, @"Data/stations.json"));

                    // import (once) the self-changed or added systems and stations 
                    Dictionary<Int32, Int32> changedSystemIDs;
                    changedSystemIDs = Program.Data.ImportSystems_Own(Path.Combine(RNPath, @"Data/systems_own.json"));
                    Program.Data.ImportStations_Own(Path.Combine(RNPath, @"Data/stations_own.json"), changedSystemIDs);

                    // import the Commander's Log from the old RN files
                    Program.Data.ImportCommandersLog(Path.Combine(RNPath, @"CommandersLogAutoSave.xml"));

                    //import the history of visited stations
                    Program.Data.ImportVisitedStations(Path.Combine(RNPath, @"Data/StationHistory.json"));
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing the whole old data to database", ex);
            }
        }

#endregion

#region basedata

        private String[] BaseTables_Systems = new String[] {"tbgovernment", 
                                                            "tballegiance", 
                                                            "tbstate", 
                                                            "tbsecurity", 
                                                            "tbeconomy", 
                                                            "tbstationtype",
                                                            "tbcommodity", 
                                                            "tbeventType",
                                                            "tbsystems",
                                                            "tbstations"};

        // dataset with base data
        private dsEliteDB m_BaseData = null;

        /// <summary>
        /// access to the dataset with the base data
        /// </summary>
        public dsEliteDB BaseData { get { return m_BaseData; } }


        /// <summary>
        /// loads the data from the basetables into memory
        /// </summary>
        /// <param name="m_BaseData"></param>
        internal void PrepareBaseTables(String TableName = "")
        {
            PerformanceTimer Runtime;

            try
            {
                Runtime = new PerformanceTimer();

                if(String.IsNullOrEmpty(TableName))
                { 
                    if(m_BaseData == null)
                        m_BaseData = new dsEliteDB();

                    foreach (String BaseTable in BaseTables_Systems)
                    {
                        Runtime.startMeasuring();
                        m_BaseData.Tables[BaseTable].Clear();

                        // preload all tables with base data
                        Program.DBCon.Execute(String.Format("select * from {0}", BaseTable), BaseTable, m_BaseData);
                        Runtime.PrintAndReset("loading full table '" + BaseTable + "':");
                    }
                }
                else if(BaseTables_Systems.Contains(TableName))
                {
                    m_BaseData.Tables[TableName].Clear();

                    // reload selected table
                    Program.DBCon.Execute(String.Format("select * from {0}", TableName), TableName, m_BaseData);
                    Runtime.PrintAndReset("loading full table '" + TableName + "':");
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
                    return (Int32)(m_BaseData.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("{0} = '{1}'", Tablename, Name))[0]["id"]);

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
                    return (String)(m_BaseData.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("id = {0}", id))[0][Tablename]);

            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error while searching for the name of <{0}> in table <tb{1}>", id.ToNString(), Tablename), ex);
            }
        }

#endregion

#region handling of commodities

        /// <summary>
        /// imports the data from the file into the database
        /// (only newer data will be imported)
        /// </summary>
        /// <param name="Filename"></param>
        internal void ImportCommodities(String Filename)
        {
            String              sqlString;
            List<EDCommodities> Commodities;
            Int32 Counter = 0;

            try
            {
                Commodities = JsonConvert.DeserializeObject<List<EDCommodities>>(File.ReadAllText(Filename));

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Program.DBCon.TransBegin();
                
                sendProgressEvent("import commodities", 0, Commodities.Count);

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

                    Counter++;
                    sendProgressEvent("import commodities", Counter, Commodities.Count);
                }

                Program.DBCon.TransCommit();

                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");
            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky performance
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
            dsEliteDB                 Data;
            DataSet                   DataNames;
            Dictionary<String, Int32> foundLanguagesFromFile     = new Dictionary<String, Int32>();
            String                    sqlString;
            Int32                     currentSelfCreatedIndex;
            Int32 Counter = 0;

            Data      = new dsEliteDB();
            DataNames = new DataSet();

            try
            {

                DataNames.ReadXml(Filename);

                sqlString = "select min(id) As min_id from tbCommodity";
                Program.DBCon.Execute(sqlString, "minID", DataNames);

                if(Convert.IsDBNull(DataNames.Tables["minID"].Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)DataNames.Tables["minID"].Rows[0]["min_id"]) - 1;
                    if(currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                Program.DBCon.TableRead("select * from tbLanguage", Data.tblanguage);
                Program.DBCon.TableRead("select * from tbCommodityLocalization", Data.tbcommoditylocalization);
                Program.DBCon.TableRead("select * from tbCommodity", Data.tbcommodity);

                if(DataNames.Tables["Names"] != null)
                { 
                    sendProgressEvent("import commodity localization", Counter, DataNames.Tables["Names"].Rows.Count);

                    // first check if there's a new language
                    foreach (DataColumn LanguageFromFile in DataNames.Tables["Names"].Columns)
                    {
                        DataRow[] LanguageName  = Data.tblanguage.Select("language  = " + DBConnector.SQLAString(LanguageFromFile.ColumnName));

                        if(LanguageName.Count() == 0)
                        {
                            // add a non existing language
                            DataRow newRow  = Data.tblanguage.NewRow();
                            int?    Wert    = DBConvert.To<int?>(Data.tblanguage.Compute("max(id)", ""));

                            if(Wert == null)
                                Wert = 0;

                            newRow["id"]        = Wert;
                            newRow["language"]  = LanguageFromFile.ColumnName;

                            Data.tblanguage.Rows.Add(newRow);

                            foundLanguagesFromFile.Add(LanguageFromFile.ColumnName, (Int32)Wert);
                        }
                        else
                            foundLanguagesFromFile.Add((String)LanguageName[0]["language"], (Int32)LanguageName[0]["id"]);
                    
                    }
                
                    // submit changes (tbLanguage)
                    Program.DBCon.TableUpdate(Data.tblanguage);

                    // compare and add the localized names
                    foreach (DataRow LocalizationFromFile in DataNames.Tables["Names"].AsEnumerable())
                    {
                        String    BaseName              = (String)LocalizationFromFile[Program.BASE_LANGUAGE];
                        DataRow[] Commodity             = Data.tbcommodity.Select("commodity = " + DBConnector.SQLAString(DBConnector.DTEscape(BaseName)));

                        if (Commodity.Count() == 0)
                        { 
                            // completely unknown commodity - add first new entry to "tbCommodities"
                            DataRow newRow = Data.tbcommodity.NewRow();

                            newRow["id"]            = currentSelfCreatedIndex;
                            newRow["commodity"]     = BaseName;
                            newRow["is_rare"]       = 0;

                            Data.tbcommodity.Rows.Add(newRow);

                            currentSelfCreatedIndex -= 1;

                            // submit changes (tbCommodity)
                            Program.DBCon.TableUpdate(Data.tbcommodity);

                            Commodity             = Data.tbcommodity.Select("commodity = " + DBConnector.SQLAString(DBConnector.DTEscape(BaseName)));
                        }

                        foreach (KeyValuePair<String, Int32> LanguageFormFile in foundLanguagesFromFile)
	                    {
                            DataRow[] currentLocalizations  = Data.tbcommoditylocalization.Select("     commodity_id  = " + Commodity[0]["id"] + 
                                                                                                  " and language_id   = " + LanguageFormFile.Value);

                            if(currentLocalizations.Count() == 0)
                            {
                                // add a new localization
                                DataRow newRow = Data.tbcommoditylocalization.NewRow();

                                newRow["commodity_id"]  = Commodity[0]["id"];
                                newRow["language_id"]   = LanguageFormFile.Value;
                                newRow["locname"]       = (String)LocalizationFromFile[LanguageFormFile.Key];

                                Data.tbcommoditylocalization.Rows.Add(newRow);
                            }
	                    }

                        Counter++;
                        sendProgressEvent("import commodity localization", Counter, DataNames.Tables["Names"].Rows.Count);
                    }
                }
                // submit changes
                Program.DBCon.TableUpdate(Data.tbcommoditylocalization);

                Program.DBCon.TableReadRemove(Data.tblanguage);
                Program.DBCon.TableReadRemove(Data.tbcommoditylocalization);
                Program.DBCon.TableReadRemove(Data.tbcommodity);

            }
            catch (Exception ex)
            {
                Program.DBCon.TableReadRemove(Data.tblanguage);
                Program.DBCon.TableReadRemove(Data.tbcommoditylocalization);
                Program.DBCon.TableReadRemove(Data.tbcommodity);

                throw new Exception("Error while loading commodity names", ex);
            }

        }

        /// <summary>
        /// loads the existing price-warnlevel data
        /// </summary>
        internal void ImportCommodityPriceWarnLevels(String Filename)
        {
            dsEliteDB                         Data;
            List<EDCommoditiesWarningLevels>  WarnLevels;
            Int32 Counter = 0;

            WarnLevels  = JsonConvert.DeserializeObject<List<EDCommoditiesWarningLevels>>(File.ReadAllText(Filename));
            Data        = new dsEliteDB();

            try
            {
                

                Program.DBCon.TableRead("select * from tbCommodity", Data.tbcommodity);

                sendProgressEvent("import warnlevels", Counter, WarnLevels.Count);

                // first check if there's a new language
                foreach (EDCommoditiesWarningLevels Warnlevel in WarnLevels)
                {
                    DataRow[] Commodity = Data.tbcommodity.Select("commodity = " + DBConnector.SQLAString(DBConnector.DTEscape(Warnlevel.Name)));

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

                    Counter++;
                    sendProgressEvent("import warnlevels", Counter, WarnLevels.Count);
                }
                
                // submit changes (tbLanguage)
                Program.DBCon.TableUpdate(Data.tbcommodity);

                Program.DBCon.TableReadRemove(Data.tbcommodity);

            }
            catch (Exception ex)
            {
                Program.DBCon.TableReadRemove(Data.tbcommodity);

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
            dsEliteDB.tbsystemsRow[] FoundRows;
            dsEliteDB.tbsystems_orgRow[] FoundRows_org;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<Int32, Int32>();
            dsEliteDB Data;
            Int32 Counter = 0;

            Data = new dsEliteDB();

            try
            {
                

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Systems = JsonConvert.DeserializeObject<List<EDSystem>>(File.ReadAllText(Filename));

                Program.DBCon.TransBegin();

                sqlString = "select * from tbSystems lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbsystems);

                sqlString = "select * from tbSystems_org lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbsystems_org);

                sendProgressEvent("import systems", Counter, Systems.Count);

                foreach (EDSystem System in Systems)
                {
                    FoundRows = (dsEliteDB.tbsystemsRow[])Data.tbsystems.Select("id=" + System.Id.ToString());

                    if ((FoundRows != null) && (FoundRows.Count() > 0))
                    {
                        // system is existing

                        if ((bool)(FoundRows[0]["is_changed"]))
                        {
                            // data is changed by user - hold it ...

                            // ...and check table "tbSystems_org" for the original data
                            FoundRows_org = (dsEliteDB.tbsystems_orgRow[])Data.tbsystems_org.Select("id=" + System.Id.ToString());

                            if ((FoundRows_org != null) && (FoundRows_org.Count() > 0))
                            {
                                // system is in "tbSystems_org" existing - keep the newer version 
                                Timestamp_old = (DateTime)(FoundRows_org[0]["updated_at"]);
                                Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(System.UpdatedAt);

                                if (Timestamp_new > Timestamp_old)
                                {
                                    // data from file is newer
                                    CopyEDSystemToDataRow(System, (DataRow)FoundRows_org[0]);
                                    ImportCounter += 1;
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
                                CopyEDSystemToDataRow(System, FoundRows[0]);
                                ImportCounter += 1;
                            }
                        }
                    }
                    else
                    {
                        // check if theres a user generated system
                        // self-created systems don't have the correct id so it must be identified by name    
                        FoundRows = (dsEliteDB.tbsystemsRow[])Data.tbsystems.Select("     systemname = " + DBConnector.SQLAString(DBConnector.DTEscape(System.Name.ToString())) +
                                                                    " and id         < 0");

                        if (FoundRows.Count() > 0)
                        {
                            // self created systems is existing -> correct id and get new data from EDDB
                            // (changed system_id in tbStations are automatically internal updated by the database itself)
                            CopyEDSystemToDataRow(System, (DataRow)FoundRows[0]);
                        }
                        else
                        {
                            // add a new system
                            dsEliteDB.tbsystemsRow newRow = (dsEliteDB.tbsystemsRow)Data.tbsystems.NewRow();
                            CopyEDSystemToDataRow(System, (DataRow)newRow);
                            Data.tbsystems.Rows.Add(newRow);
                        }

                        ImportCounter += 1;
                    }

                    if ((ImportCounter > 0) && ((ImportCounter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Systems : " + ImportCounter.ToString());

                        Program.DBCon.TableUpdate(Data.tbsystems);
                        Program.DBCon.TableUpdate(Data.tbsystems_org);
                    }

                    Counter++;
                    sendProgressEvent("import systems", Counter, Systems.Count);

                }

                // save changes
                Program.DBCon.TableUpdate(Data.tbsystems, true);
                Program.DBCon.TableUpdate(Data.tbsystems_org, true);

                Program.DBCon.TransCommit();

                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                if (Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove(Data.tbsystems);
                    Program.DBCon.TableReadRemove(Data.tbsystems_org);

                }
                catch (Exception) { }

                throw new Exception("Error while importing system data", ex);
            }
        }

        /// <summary>
        /// imports the own data from the file into the database (only initially once needed)
        /// </summary>
        /// <param name="Filename"></param>
        public Dictionary<Int32, Int32> ImportSystems_Own(String Filename, Boolean OnlyAddUnknown = false)
        {
            try
            {
                List<EDSystem> SystemList = JsonConvert.DeserializeObject<List<EDSystem>>(File.ReadAllText(Filename));
                return ImportSystems_Own(ref SystemList, OnlyAddUnknown);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in <ImportSystems_Own> from file", ex);
            }
        }
            
        /// <summary>
        /// imports the data from the list of systems
        /// </summary>
        /// <param name="Filename"></param>
        public Dictionary<Int32, Int32> ImportSystems_Own(EDSystem System, Boolean OnlyAddUnknown = false)
        {
            try
            {
                List<EDSystem> SystemList = new List<EDSystem>() {System};
                return ImportSystems_Own(ref SystemList, OnlyAddUnknown);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in <ImportSystems_Own> from single station", ex);
            }
        }

        /// <summary>
        /// imports the data from the list of systems
        /// </summary>
        /// <param name="Filename"></param>
        public Dictionary<Int32, Int32> ImportSystems_Own(ref List<EDSystem> Systems, Boolean OnlyAddUnknown = false)
        {
            String sqlString;
            dsEliteDB.tbsystemsRow[] FoundRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<Int32, Int32>();
            Int32 currentSelfCreatedIndex = -1;
            dsEliteDB Data;
            Int32 Counter = 0;

            try
            {

                Data = new dsEliteDB();

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Program.DBCon.TransBegin();

                sqlString = "select * from tbSystems lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbsystems);

                sqlString = "select * from tbSystems_org lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbsystems_org);

                currentSelfCreatedIndex = getNextOwnSystemIndex();

                sendProgressEvent("import self-added systems", Counter, Systems.Count);

                foreach (EDSystem System in Systems)
                {
                    if (!String.IsNullOrEmpty(System.Name.ToString().Trim()))
                    {
                        // self-created systems don't have the correct id so it must be identified by name    
                        FoundRows = (dsEliteDB.tbsystemsRow[])Data.tbsystems.Select("systemname=" + DBConnector.SQLAString(DBConnector.DTEscape(System.Name.ToString())));

                        if ((FoundRows != null) && (FoundRows.Count() > 0))
                        {
                            if (!OnlyAddUnknown)
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
                                        CopyEDSystemToDataRow(System, (DataRow)FoundRows[0], true);

                                        ImportCounter += 1;
                                    }
                                }
                                else
                                {
                                    // new data is user changed data, old data is original data
                                    // copy the original data ("tbSystems") to the saving data table ("tbSystems_org")
                                    // and get the correct system ID
                                    Data.tbsystems_org.LoadDataRow(FoundRows[0].ItemArray, false);
                                    CopyEDSystemToDataRow(System, (DataRow)FoundRows[0], true);

                                    ImportCounter += 1;
                                }
                            }
                            else
                            {
                                System.Id = (Int32)FoundRows[0]["id"];
                            }
                        }
                        else
                        {
                            // add a new system
                            // memorize the changed system ids for importing user changed stations in the (recommend) second step
                            if (!OnlyAddUnknown)
                                changedSystemIDs.Add(System.Id, currentSelfCreatedIndex);

                            System.Id = currentSelfCreatedIndex;

                            dsEliteDB.tbsystemsRow newRow = (dsEliteDB.tbsystemsRow)Data.tbsystems.NewRow();
                            CopyEDSystemToDataRow(System, (DataRow)newRow, true);
                            Data.tbsystems.Rows.Add(newRow);


                            currentSelfCreatedIndex -= 1;
                            ImportCounter += 1;

                        }

                        if ((ImportCounter > 0) && ((ImportCounter % 100) == 0))
                        {
                            // save changes
                            Debug.Print("added Systems : " + ImportCounter.ToString());

                            Program.DBCon.TableUpdate(Data.tbsystems);
                            Program.DBCon.TableUpdate(Data.tbsystems_org);
                        }
                    }

                    Counter++;
                    sendProgressEvent("import self-added systems", Counter, Systems.Count);
                }

                // save changes
                Program.DBCon.TableUpdate(Data.tbsystems, true);
                Program.DBCon.TableUpdate(Data.tbsystems_org, true);

                Program.DBCon.TransCommit();

                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                // return all changed ids
                return changedSystemIDs;
            }
            catch (Exception ex)
            {
                if (Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove("tbSystems");
                    Program.DBCon.TableReadRemove("tbSystems_org");

                }
                catch (Exception)
                { }

                throw new Exception("Error while importing system data", ex);
            }
        }

        /// <summary>
        /// gets the next free system-id for user added systems
        /// </summary>
        /// <returns></returns>
        private int getNextOwnSystemIndex()
        {
            String      sqlString;
            Int32       currentSelfCreatedIndex;
            DataTable   Data;

            try
            {
                Data        = new DataTable();
                sqlString   = "select min(id) As min_id from tbSystems";

                Program.DBCon.Execute(sqlString, Data);

                if(Convert.IsDBNull(Data.Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)Data.Rows[0]["min_id"]) - 1;
                    if(currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                return currentSelfCreatedIndex;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting next free <own> system index", ex);
            }

        }

        /// <summary>
        /// copies the data from a "EDSystem"-object to a Datarow from table "tbSystems"
        /// </summary>
        /// <param name="SystemObject"></param>
        /// <param name="SystemRow"></param>
        private void CopyEDSystemToDataRow(EDSystem SystemObject, DataRow SystemRow, Boolean OwnData = false, int? SystemID = null)
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
            dsEliteDB.tbstations_orgRow[] FoundRows_org;
            dsEliteDB.tbstationsRow[] FoundRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            dsEliteDB Data;
            Int32 Counter = 0;

            Data = new dsEliteDB();

            try
            {
                

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Stations = JsonConvert.DeserializeObject<List<EDStation>>(File.ReadAllText(Filename));

                sendProgressEvent("import stations", Counter, Stations.Count);

                Program.DBCon.TransBegin();

                sqlString = "select * from tbStations lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbstations);
                sqlString = "select * from tbStations_org lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbstations_org);
                sqlString = "select * from tbStationEconomy lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbstationeconomy);
                sqlString = "select * from tbImportCommodity lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbimportcommodity);
                sqlString = "select * from tbExportCommodity lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbexportcommodity);
                sqlString = "select * from tbProhibitedCommodity lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbprohibitedcommodity);

                foreach (EDStation Station in Stations)
                {

                    FoundRows = (dsEliteDB.tbstationsRow[])Data.tbstations.Select("id=" + Station.Id.ToString());

                    if (FoundRows.Count() > 0)
                    {
                        // Station is existing

                        if ((bool)(FoundRows[0]["is_changed"]))
                        {
                            // data is changed by user - hold it ...

                            // ...and check table "tbStations_org" for the original data
                            FoundRows_org = (dsEliteDB.tbstations_orgRow[])Data.tbstations_org.Select("id=" + Station.Id.ToString());

                            if ((FoundRows_org != null) && (FoundRows_org.Count() > 0))
                            {
                                // Station is in "tbStations_org" existing - keep the newer version 
                                Timestamp_old = (DateTime)(FoundRows_org[0]["updated_at"]);
                                Timestamp_new = UnixTimeStamp.UnixTimeStampToDateTime(Station.UpdatedAt);

                                if (Timestamp_new > Timestamp_old)
                                {
                                    // data from file is newer
                                    CopyEDStationToDataRow(Station, (DataRow)FoundRows_org[0]);

                                    CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                                    CopyEDStationCommodityToDataRow(Station, Data.tbimportcommodity);
                                    CopyEDStationCommodityToDataRow(Station, Data.tbexportcommodity);
                                    CopyEDStationCommodityToDataRow(Station, Data.tbprohibitedcommodity);

                                    ImportCounter += 1;

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
                                CopyEDStationToDataRow(Station, (DataRow)FoundRows[0]);

                                CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                                CopyEDStationCommodityToDataRow(Station, Data.tbimportcommodity);
                                CopyEDStationCommodityToDataRow(Station, Data.tbexportcommodity);
                                CopyEDStationCommodityToDataRow(Station, Data.tbprohibitedcommodity);

                                ImportCounter += 1;
                            }
                        }
                    }
                    else
                    {

                        // self-created stations don't have the correct id so they must be identified by name    
                        FoundRows = (dsEliteDB.tbstationsRow[])Data.tbstations.Select("stationname = " + DBConnector.SQLAString(DBConnector.DTEscape(Station.Name.ToString())) + " and " +
                                                                     "  system_id = " + Station.SystemId + " and " +
                                                                     "  id        < 0");

                        if (FoundRows.Count() > 0)
                        {
                            // self created station is existing -> correct id and get new data from EDDB
                            CopyEDStationToDataRow(Station, (DataRow)FoundRows[0]);

                            // update immediately because otherwise the references are wrong after changing a id
                            Program.DBCon.TableUpdate(Data.tbstations);
                            Program.DBCon.TableUpdate(Data.tbstations_org);
                            Program.DBCon.TableUpdate(Data.tbstationeconomy);
                            Program.DBCon.TableUpdate(Data.tbimportcommodity);
                            Program.DBCon.TableUpdate(Data.tbexportcommodity);
                            Program.DBCon.TableUpdate(Data.tbprohibitedcommodity);

                            Program.DBCon.TableRefresh(Data.tbstationeconomy);
                            Program.DBCon.TableRefresh(Data.tbimportcommodity);
                            Program.DBCon.TableRefresh(Data.tbexportcommodity);
                            Program.DBCon.TableRefresh(Data.tbprohibitedcommodity);
                        }
                        else
                        {
                            // add a new Station
                            dsEliteDB.tbstationsRow newStationRow = (dsEliteDB.tbstationsRow)Data.tbstations.NewRow();

                            CopyEDStationToDataRow(Station, (DataRow)newStationRow);
                            Data.tbstations.Rows.Add(newStationRow);
                        }

                        CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                        CopyEDStationCommodityToDataRow(Station, Data.tbimportcommodity);
                        CopyEDStationCommodityToDataRow(Station, Data.tbexportcommodity);
                        CopyEDStationCommodityToDataRow(Station, Data.tbprohibitedcommodity);

                        ImportCounter += 1;
                    }

                    if ((ImportCounter > 0) && ((ImportCounter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Stations : " + ImportCounter.ToString());

                        Program.DBCon.TableUpdate(Data.tbstations);
                        Program.DBCon.TableUpdate(Data.tbstations_org);
                        Program.DBCon.TableUpdate(Data.tbstationeconomy);
                        Program.DBCon.TableUpdate(Data.tbimportcommodity);
                        Program.DBCon.TableUpdate(Data.tbexportcommodity);
                        Program.DBCon.TableUpdate(Data.tbprohibitedcommodity);
                    }

                    Counter++;
                    sendProgressEvent("import stations", Counter, Stations.Count);

                }

                // save changes
                Program.DBCon.TableUpdate(Data.tbstations, true);
                Program.DBCon.TableUpdate(Data.tbstations_org, true);
                Program.DBCon.TableUpdate(Data.tbstationeconomy, true);
                Program.DBCon.TableUpdate(Data.tbimportcommodity, true);
                Program.DBCon.TableUpdate(Data.tbexportcommodity, true);
                Program.DBCon.TableUpdate(Data.tbprohibitedcommodity, true);

                Program.DBCon.TransCommit();

                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                if (Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove(Data.tbstations);
                    Program.DBCon.TableReadRemove(Data.tbstations_org);
                    Program.DBCon.TableReadRemove(Data.tbstationeconomy);
                    Program.DBCon.TableReadRemove(Data.tbimportcommodity);
                    Program.DBCon.TableReadRemove(Data.tbexportcommodity);
                    Program.DBCon.TableReadRemove(Data.tbprohibitedcommodity);

                }
                catch (Exception) { }

                throw new Exception("Error while importing Station data", ex);
            }
        }


        /// <summary>
        /// imports the "own" station data from the file into the database (only initially once needed)
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportStations_Own(String Filename, Boolean OnlyAddUnknown = false)
        {
            try
            {
                ImportStations_Own(JsonConvert.DeserializeObject<List<EDStation>>(File.ReadAllText(Filename)), new Dictionary<Int32, Int32>(), OnlyAddUnknown);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in <ImportSystems_Own> from file", ex);
            }
        }
            
        /// <summary>
        /// imports the "own" station data from the list of stations
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportStations_Own(EDStation Station, Boolean OnlyAddUnknown = false)
        {
            try
            {
                ImportStations_Own(new List<EDStation>() {Station}, new Dictionary<Int32, Int32>(), OnlyAddUnknown);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in <ImportSystems_Own> from single station", ex);
            }
        }

        /// <summary>
        /// imports the "own" station data from the list of stations
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportStations_Own(String Filename, Dictionary<Int32, Int32> changedSystemIDs, Boolean OnlyAddUnknown = false)
        {
            try
            {
                ImportStations_Own(JsonConvert.DeserializeObject<List<EDStation>>(File.ReadAllText(Filename)), changedSystemIDs, OnlyAddUnknown);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in <ImportSystems_Own> from single station", ex);
            }
        }

        /// <summary>
        /// imports the "own" station data into the database
        /// </summary>
        /// <param name="Filename"></param>
        public void ImportStations_Own(List<EDStation> Stations, Dictionary<Int32, Int32> changedSystemIDs, Boolean OnlyAddUnknown = false)
        {
            String sqlString;
            dsEliteDB.tbstationsRow[] FoundRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            Int32 currentSelfCreatedIndex = -1;
            dsEliteDB Data = new dsEliteDB();
            Int32 Counter = 0;

            try
            {

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Program.DBCon.TransBegin();

                sqlString = "select * from tbStations lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbstations);
                sqlString = "select * from tbStations_org lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbstations_org);
                sqlString = "select * from tbStationEconomy lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbstationeconomy);

                // get the smallest ID for self added stations
                sqlString = "select min(id) As min_id from tbStations";
                Program.DBCon.Execute(sqlString, "minID", Data);

                if (Convert.IsDBNull(Data.Tables["minID"].Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)Data.Tables["minID"].Rows[0]["min_id"]) - 1;
                    if (currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                sendProgressEvent("import self-added stations", Counter, Stations.Count);

                foreach (EDStation Station in Stations)
                {
                    Int32 SystemID;

                    if (!String.IsNullOrEmpty(Station.Name.Trim()) && (Station.SystemId > 0))
                    {
                        // is the system id changed ? --> get the new system id, otherwise the original
                        if (changedSystemIDs.TryGetValue(Station.SystemId, out SystemID))
                            Station.SystemId = SystemID;

                        // self-created stations don't have the correct id so they must be identified by name    
                        FoundRows = (dsEliteDB.tbstationsRow[])Data.tbstations.Select("stationname=" + DBConnector.SQLAString(DBConnector.DTEscape(Station.Name)) + " and " +
                                                                     "system_id =  " + Station.SystemId);

                        if ((FoundRows != null) && (FoundRows.Count() > 0))
                        {
                            if (!OnlyAddUnknown)
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
                                        CopyEDStationToDataRow(Station, (DataRow)FoundRows[0], true);

                                        CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                                        // commodities are not considered because there was no possibility for input in the old RN

                                        ImportCounter += 1;
                                    }
                                }
                                else
                                {
                                    // new data is user changed data, old data is original data
                                    // copy the original data ("tbStations") to the saving data table ("tbStations_org")
                                    // and get the correct system ID
                                    Data.tbstations_org.LoadDataRow(FoundRows[0].ItemArray, false);
                                    CopyEDStationToDataRow(Station, (DataRow)FoundRows[0], true);

                                    CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                                    // commodities are not considered because there was no possibility for input in the old RN

                                    ImportCounter += 1;
                                }
                            }
                        }
                        else
                        {
                            // add a new station
                            Station.Id = currentSelfCreatedIndex;

                            dsEliteDB.tbstationsRow newRow = (dsEliteDB.tbstationsRow)Data.tbstations.NewRow();

                            CopyEDStationToDataRow(Station, (DataRow)newRow, true);
                            Data.tbstations.Rows.Add(newRow);

                            currentSelfCreatedIndex -= 1;
                            ImportCounter += 1;

                            CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                            // commodities are not considered because there was no possibility for input in the old RN

                            ImportCounter += 1;
                        }

                        if ((ImportCounter > 0) && ((ImportCounter % 100) == 0))
                        {
                            // save changes
                            Debug.Print("added Stations : " + ImportCounter.ToString());

                            Program.DBCon.TableUpdate(Data.tbstations);
                            Program.DBCon.TableUpdate(Data.tbstations_org);
                            Program.DBCon.TableUpdate(Data.tbstationeconomy);
                        }
                    }

                    Counter++;
                    sendProgressEvent("import self-added stations", Counter, Stations.Count);

                }

                // save changes
                Program.DBCon.TableUpdate(Data.tbstations, true);
                Program.DBCon.TableUpdate(Data.tbstations_org, true);
                Program.DBCon.TableUpdate(Data.tbstationeconomy, true);

                Program.DBCon.TransCommit();

                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                if (Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    Program.DBCon.TableReadRemove(Data.tbstations);
                    Program.DBCon.TableReadRemove(Data.tbstations_org);
                    Program.DBCon.TableReadRemove(Data.tbstationeconomy);
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
        private void CopyEDStationToDataRow(EDStation StationObject, DataRow StationRow, Boolean OwnData = false, int? StationID = null)
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
                    case "tbimportcommodity":
                        Commodities = StationObject.ImportCommodities;
                    break;
                    case "tbexportcommodity":
                        Commodities = StationObject.ExportCommodities;
                    break;
                    case "tbprohibitedcommodity":
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

        /// <summary>
        /// imports the old list of visited stations to the database
        /// </summary>
        /// <param name="StationObject"></param>
        /// <param name="EconomyRow"></param>
        public void ImportVisitedStations(string Filename)
        {
            String sqlString;
            Int32 Counter = 0;

            try
            {
                List<StationVisit> History  = JsonConvert.DeserializeObject<List<StationVisit>>(File.ReadAllText(Filename));

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Program.DBCon.TransBegin("ImportVisitedStations");

                sendProgressEvent("import visited stations", Counter, History.Count);

                foreach(StationVisit VisitEvent in History)
                {
                    String System  = StructureHelper.CombinedNameToSystemName(VisitEvent.Station);                    
                    String Station = StructureHelper.CombinedNameToStationName(VisitEvent.Station);

                    //Debug.Print(System + "," + Station);

                    try
                    {
                        sqlString = String.Format("insert ignore into tbVisitedSystems(system_id, time)" +
                                                  " SELECT d.* FROM (SELECT" +
                                                  " (select id from tbSystems where systemname = {0}) as system_id," +
                                                  " {1} as time) as d", 
                                                  DBConnector.SQLAString(DBConnector.SQLEscape(System)), 
                                                  DBConnector.SQLDateTime(VisitEvent.Visited));

                        Program.DBCon.Execute(sqlString);
                    }
                    catch (Exception)
                    {
                        //Debug.Print("Error while importing system in history :" + System);
                    };

                    try
                    {
                        sqlString = String.Format("insert ignore into tbVisitedStations(station_id, time)" +
                                                  " SELECT d.* FROM (SELECT" +
                                                  " (select id from tbStations" + 
                                                  "        where stationname = {0}" + 
                                                  "          and system_id   = (select id from tbSystems where systemname = {1})) as station_id," +
                                                  " {2} as time) as d", 
                                                  DBConnector.SQLAString(DBConnector.SQLEscape(Station)), 
                                                  DBConnector.SQLAString(DBConnector.SQLEscape(System)),
                                                  DBConnector.SQLDateTime(VisitEvent.Visited));

                        Program.DBCon.Execute(sqlString);
                    }
                    catch (Exception)
                    {
                        //Debug.Print("Error while importing station in history :" + Station);
                    };
                    
                    Counter++;
                    sendProgressEvent("import visited stations", Counter, History.Count);
                }

                Program.DBCon.TransCommit();
                
            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                throw new Exception("Error while importing the history of visited stations", ex);
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
            List<EDSystem> Systems   = new List<EDSystem>();
            List<EDStation> Stations = new List<EDStation>();
            Int32 currentIndex = 0;
            Int32 Counter = 0;

            try
            {
                Data = new DataSet();

                Data.ReadXml(Filename);

                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                if(Data.Tables.Contains("CommandersLogEvent"))
                { 
                    sendProgressEvent("import log", Counter, Data.Tables["CommandersLogEvent"].Rows.Count);

                    foreach(DataRow Event in Data.Tables["CommandersLogEvent"].AsEnumerable())
                    {
                        String   System    = Event["System"].ToString().Trim();
                        Systems.Add(new EDSystem{Name = System});
                    }
                    ImportSystems_Own(ref Systems, true);

                    foreach(DataRow Event in Data.Tables["CommandersLogEvent"].AsEnumerable())
                    {
                        String   Station   = StructureHelper.CombinedNameToStationName((String)Event["Station"]).Trim();
                        Stations.Add(new EDStation{Name = Station, SystemId = Systems[currentIndex].Id});
                        currentIndex++;
                    }
                    ImportStations_Own(Stations, new Dictionary<Int32, Int32>(), true);

                    foreach(DataRow Event in Data.Tables["CommandersLogEvent"].AsEnumerable())
                    {
                        DateTime EventTime = DateTime.Parse((String)Event["EventDate"], CultureInfo.CurrentUICulture , DateTimeStyles.None);
                        String   System    = Event["System"].ToString().Trim();
                        String   Station   = StructureHelper.CombinedNameToStationName((String)Event["Station"]).Trim();
                        String   EventType = Event["EventType"].ToString().Trim().Length == 0 ? "Other" : Event["EventType"].ToString().Trim();

                        // add a new log entry
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
                                                  DBConnector.SQLAString(DBConnector.SQLEscape(Event["System"].ToString())),
                                                  DBConnector.SQLAString(DBConnector.SQLEscape(Station)), 
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

                        Counter++;
                        sendProgressEvent("import log", Counter, Data.Tables["CommandersLogEvent"].Rows.Count);
                    }

                }

                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

            }
            catch (Exception ex)
            {
                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                throw new Exception("Error when importing the Commander's Log ", ex);
            }
        }


#endregion




    }

}
