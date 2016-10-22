using System;
using System.Linq;
using System.Text;
using IBE.EDDB_Data;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using IBE.Enums_and_Utility_Classes;
using System.Diagnostics;
using System.Globalization;
using IBE.SQL.Datasets;
using System.Collections.Generic;

namespace IBE.SQL
{
    public class EliteDBIO
    {

#region "enums"

        private enum enCommodityTypes
        {
            import,
            export,
            prohibited
        }

        public enum enVisitType 
        {
            Stations = 1,
            Systems  = 2
        }

        public enum enImportBehaviour
        {
            OnlyNewer    = 0,
            NewerOrEqual = 1,
            All          = 2
        }

        public enum enLocalizationType
        {
            Commodity       = 0,
            Category        = 1,
            Economylevel    = 2
        }

        public enum enLocalisationImportType
        {
            onlyNew             = 0,
            overwriteNonBase    = 1,
            overWriteAll        = 2
        }

        public enum enDataSource
        {
            fromIBE_OCR         = -2,
            fromRN              = -1,
            undefined           =  0,
            fromIBE             =  1,
            fromEDDN            =  2,
            fromFILE            =  3,
            fromEDDN_T          =  4
        }

#endregion

        /// <summary>
        /// constructor
        /// </summary>
        public EliteDBIO()
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

        private const Int32 m_TimeSlice_ms      = 500;
        private const Int32 m_PercentSlice      = 10;
        private PerformanceTimer m_EventTimer   = new PerformanceTimer();
        private long m_lastProgress             = 0;

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

        public Boolean InitImportDone
        {
            get
            {
                return Program.DBCon.getIniValue<Boolean>("Database", "InitImportDone", "false", false, false);
            }
            set
            {
                Program.DBCon.setIniValue("Database", "InitImportDone", value.ToString());
            }
        }

        //public event EventHandler<ProgressEventArgs_OLD> Progress;

        //protected virtual void OnProgress(ProgressEventArgs_OLD e)
        //{
        //    EventHandler<ProgressEventArgs_OLD> myEvent = Progress;
        //    if (myEvent != null)
        //    {
        //        myEvent(this, e);
        //    }
        //}

        //public class ProgressEventArgs_OLD : EventArgs
        //{
        //    public String Tablename { get; set; }
        //    public Int32  Index { get; set; }
        //    public Int32  Total { get; set; }
        //    public String Unit { get; set; }
        //    public ProgressView progressObject;
        //    public Boolean Single_NoReuse{ get; set; }
        //    public Boolean Clear { get; set; }
            
        //}

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
            public String Info { get; set; }
            public Int64  CurrentValue { get; set; }
            public Int64  TotalValue { get; set; }
            public String Unit { get; set; }
            public Boolean NewLine{ get; set; }
            public Boolean Clear { get; set; }
            public Boolean AddSeparator { get; set; }
            public Boolean ForceRefresh { get; set; }
            public Boolean Cancelled { get; set; }

         
            public ProgressEventArgs()
            {
                Info            = null;
                CurrentValue    = -1;
                TotalValue      = -1;
                Unit            = null;
                NewLine         = false;
                Clear           = false;
                AddSeparator    = false;
                ForceRefresh    = false;
                Cancelled       = false;
            }
               
        }

        /// <summary>
        /// fires the progress event if necessary
        /// </summary>
        /// <param name="pEArgs"></param>
        /// <returns>"true", if the process should be cancelled, otherwise "false"</returns>
        private Boolean sendProgressEvent(ProgressEventArgs pEArgs)
        {
            long   ProgressSendLevel;
            Boolean retValue            = false;

            try 
	        {	        
               
		        if((m_EventTimer.currentMeasuring() > m_TimeSlice_ms) || pEArgs.ForceRefresh || pEArgs.NewLine || pEArgs.AddSeparator)
                {
                    // time is reason
                    Progress.Raise(this, pEArgs);

                    if(pEArgs.TotalValue > 0)
                        ProgressSendLevel =  ((100 * pEArgs.CurrentValue/pEArgs.TotalValue) / 10) * 10;
                    else
                        ProgressSendLevel =  -1;

                    m_EventTimer.startMeasuring();
                    m_lastProgress = ProgressSendLevel;

                    Debug.Print("Progress (t):" + ProgressSendLevel.ToString());
                }
                else
                { 
                    // progress is reason
                    if(pEArgs.TotalValue > 0)
                        ProgressSendLevel =  ((100 * pEArgs.CurrentValue/pEArgs.TotalValue) / 10) * 10;
                    else
                        ProgressSendLevel =  -1;
                    
                    if(((pEArgs.TotalValue != 0) && (((100 * pEArgs.CurrentValue/pEArgs.TotalValue) % m_PercentSlice) == 0) && (ProgressSendLevel != m_lastProgress)) || 
                       (ProgressSendLevel != m_lastProgress))
                    { 
                        // time is reason
                        Progress.Raise(this, pEArgs);

                        m_EventTimer.startMeasuring();
                        m_lastProgress = ProgressSendLevel;
                        Debug.Print("Progress (l):" + ProgressSendLevel.ToString());
                    }
                }

                retValue = pEArgs.Cancelled ;

                return retValue;
	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while checking for progress event needed", ex);
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
                                                            "tbeventtype",
                                                            "tbcargoaction",
                                                            "tbsystems",
                                                            "tbstations", 
                                                            "tbattribute", 
                                                            "tbsource", 
                                                            "visystemsandstations", 
                                                            "tbcommoditylocalization", 
                                                            "tblevellocalization", 
                                                            "tbcategorylocalization", 
                                                            "tbcategory", 
                                                            "tbeconomylevel", 
                                                            "tbvisitedsystems", 
                                                            "tbvisitedstations", 
                                                            "tbpower",
                                                            "tbpowerstate", 
                                                            "tblanguage", 
                                                            "tbtrustedsenders",
                                                            "tbdnmap_commodity",
                                                            "tbcommoditybase",
                                                            "tboutfittingbase",
                                                            "tbshipyardbase"};

        // dataset with base data
        private dsEliteDB m_BaseData = null;
        private Dictionary<DataTable, DBConnector> m_BaseData_Connector = null;

        /// <summary>
        /// access to the dataset with the base data
        /// </summary>
        public dsEliteDB                              BaseData { get { return m_BaseData; } }
//        private Dictionary<DataTable, MySql.Data.MySqlClient.MySqlDataAdapter>     m_BaseData_UpdateObjects = new Dictionary<DataTable, MySql.Data.MySqlClient.MySqlDataAdapter>();
        



        /// <summary>
        /// loads the data from the basetables into memory. For correct initialization it is
        /// necessary to call this fuction with a unset tableName
        /// 
        /// </summary>
        /// <param name="m_BaseData"></param>
        internal void PrepareBaseTables(String TableName = "", Boolean saveChanged = false)
        {
            PerformanceTimer Runtime;
            //MySql.Data.MySqlClient.MySqlDataAdapter dataAdapter;
            DBConnector currentDBCon;

            try
            {
                Runtime = new PerformanceTimer();

                if(String.IsNullOrEmpty(TableName))
                { 
                    if(m_BaseData == null)
                    {
                        m_BaseData              = new dsEliteDB();
                        m_BaseData_Connector    = new Dictionary<DataTable, DBConnector>();
                    }

                    foreach (String BaseTable in BaseTables_Systems)
                    {
                        if (!m_BaseData_Connector.TryGetValue(m_BaseData.Tables[BaseTable], out currentDBCon))
                        {
                            // each basetable gets it's own DBConnector, because 
                            // the contained DataReaders will be hold open for possible 
                            // changes (MySQL doesn't support MARS "Multiple Active result Sets")

                            currentDBCon = new DBConnector(Program.DBCon.ConfigData);
                            m_BaseData_Connector.Add(m_BaseData.Tables[BaseTable], currentDBCon);

                            currentDBCon.Connect();
                        }

                        Runtime.startMeasuring();
                        m_BaseData.Tables[BaseTable].Clear();

                        if (!Program.SplashScreen.IsDisposed)
                            Program.SplashScreen.InfoAdd("...loading basetable '" + BaseTable + "'...");                            

                        // preload all tables with base data
                        currentDBCon.TableRead(String.Format("select * from {0}", BaseTable), BaseTable, m_BaseData);
                        
                        if (!Program.SplashScreen.IsDisposed)
                            Program.SplashScreen.InfoAppendLast("<OK>");                            

                        Runtime.PrintAndReset("loading full table '" + BaseTable + "':");
                    }
                }
                else if(BaseTables_Systems.Contains(TableName))
                {
                    currentDBCon = m_BaseData_Connector[m_BaseData.Tables[TableName]];

                    if(saveChanged)
                    {
                        // save all containing changes
                        Runtime.PrintAndReset("saving changes in table '" + TableName + "':");
                        currentDBCon.TableUpdate(TableName, m_BaseData);
                    }
                    else
                    {
                        Runtime.startMeasuring();

                        m_BaseData.Tables[TableName].Clear();

                        // reload selected table
                        currentDBCon.TableRead("", TableName, m_BaseData);

                        Runtime.PrintAndReset("re-loading full table '" + TableName + "':");
                    }
                }
                else
                {
                    throw new Exception(string.Format("Attempt to load an unknown basetable : <{0}>", TableName));
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
        public object BaseTableNameToID(String Tablename, String Name, Boolean insertUnknown = false)
        {
            
            try
            {
                String fullTableName = String.Format("tb{0}", Tablename);

                if (Name == null)
                    return null;
                else
                {
                    DataRow[] data = m_BaseData.Tables[fullTableName].Select(String.Format("{0} = '{1}'", Tablename, Name));

                    if ((data.GetUpperBound(0) == -1) && insertUnknown)
                    { 
                        int maxValue;
                        DataTable Table     = m_BaseData.Tables[fullTableName];
                        if (Table.Rows.Count > 0)
                            maxValue        = Convert.ToInt32(Table.Compute("Max(id)", string.Empty));
                        else
                            maxValue        = 0;

                        DataRow newRow      = Table.NewRow();
                        newRow["id"]        = maxValue + 1;
                        newRow[Tablename]   = DBConnector.DTEscape(Name);

                        Table.Rows.Add(newRow);
                        PrepareBaseTables(fullTableName, true);
                    }

                    return (Int32)(m_BaseData.Tables[fullTableName].Select(String.Format("{0} = '{1}'", Tablename, Name))[0]["id"]);
                }

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
        public object BaseTableIDToName(String Tablename, int? id, String column = "")
        {
            try
            {

                if (id == null)
                    return null;
                else
                {
                    if(String.IsNullOrEmpty(column))
                        return (String)(m_BaseData.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("id = {0}", id))[0][Tablename]);
                    else
                        return (String)(m_BaseData.Tables[String.Format("tb{0}", Tablename)].Select(String.Format("id = {0}", id))[0][column]);
                }
                    

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
        /// <param name="fileName"></param>
        internal void ImportCommoditiesFromFile(String fileName)
        {
            List<EDCommodities> Commodities;

            try
            {
                Commodities = JsonConvert.DeserializeObject<List<EDCommodities>>(File.ReadAllText(fileName));

                ImportCommodities(Commodities);
            }
            catch(Exception ex)
            {
                throw new Exception("Error while importing commodities from file", ex);
            }
        }

        /// <summary>
        /// imports the data from the list
        /// (only newer data will be imported)
        /// </summary>
        /// <param name="fileName"></param>
        internal void ImportCommodities(List<EDCommodities> Commodities)
        {
            String              sqlString;
            Int32 Counter = 0;

            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);					

            try
            {
                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                lDBCon.TransBegin();
                
                sendProgressEvent(new ProgressEventArgs() { Info="import commodities", CurrentValue=0, TotalValue=Commodities.Count });

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

                    lDBCon.Execute(sqlString);

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

                    lDBCon.Execute(sqlString);

                    Counter++;

                    
                    
                    if(sendProgressEvent(new ProgressEventArgs() { Info="import commodities", CurrentValue=Counter, TotalValue=Commodities.Count }))
                        break;
                }

                lDBCon.TransCommit();

                // reset freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();
            }
            catch (Exception ex)
            {
                if(lDBCon.TransActive())
                    lDBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");
                    lDBCon.Dispose();
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
            DataSet                   DataNames;

            DataNames = new DataSet();

            try
            {

                DataNames.ReadXml(Filename);
                ImportCommodityLocalizations(DataNames);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading commodity names", ex);
            }

        }


        /// <summary>
        /// loads the localized commodity names and check if 
        /// the self added names now included in the official dictionary
        /// </summary>
        internal void ImportCommodityLocalizations(DataSet DataNames, enLocalisationImportType importType = enLocalisationImportType.onlyNew)
        {
            dsEliteDB                 Data;
            Dictionary<String, Int32> foundLanguagesFromFile     = new Dictionary<String, Int32>();
            String                    sqlString;
            Int32                     currentSelfCreatedIndex;
            Int32                     Counter = 0;
            Boolean                   idColumnFound = false;
            String                    BaseName;
            DataRow[]                 Commodity;

            Data      = new dsEliteDB();

            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);

            try
            {
                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                sqlString = "select min(id) As min_id from tbCommodity";
                lDBCon.Execute(sqlString, "minID", DataNames);

                if(Convert.IsDBNull(DataNames.Tables["minID"].Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)DataNames.Tables["minID"].Rows[0]["min_id"]) - 1;
                    if(currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                lDBCon.TableRead("select * from tbLanguage", Data.tblanguage);
                lDBCon.TableRead("select * from tbCommodityLocalization", Data.tbcommoditylocalization);
                lDBCon.TableRead("select * from tbCommodity", Data.tbcommodity);

                if(DataNames.Tables["Names"] != null)
                { 
                    sendProgressEvent(new ProgressEventArgs() {Info="import commodity localization", CurrentValue=Counter, TotalValue=DataNames.Tables["Names"].Rows.Count });

                    // first check if there's a new language
                    foreach (DataColumn LanguageFromFile in DataNames.Tables["Names"].Columns)
                    {
                        if(!LanguageFromFile.ColumnName.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                        {
                            DataRow[] LanguageName  = Data.tblanguage.Select("language  = " + DBConnector.SQLAString(LanguageFromFile.ColumnName));

                            if(LanguageName.Count() == 0)
                            {
                                // add a non existing language
                                DataRow newRow  = Data.tblanguage.NewRow();
                                int?    Wert    = DBConvert.To<int?>(Data.tblanguage.Compute("max(id)", ""));

                                if(Wert == null)
                                    Wert = 0;

                                Wert += 1;
                                newRow["id"]        = Wert;
                                newRow["language"]  = LanguageFromFile.ColumnName;

                                Data.tblanguage.Rows.Add(newRow);

                                foundLanguagesFromFile.Add(LanguageFromFile.ColumnName, (Int32)Wert);
                            }
                            else
                                foundLanguagesFromFile.Add((String)LanguageName[0]["language"], (Int32)LanguageName[0]["id"]);
                        }
                        else
                            idColumnFound = true;
                    
                    }
                
                    // submit changes (tbLanguage)
                    lDBCon.TableUpdate(Data.tblanguage);

                    // compare and add the localized names
                    foreach (DataRow LocalizationFromFile in DataNames.Tables["Names"].AsEnumerable())
                    {
                        int? commodityID = null;

                        if (idColumnFound)
                            commodityID  = DBConvert.To<int?>(LocalizationFromFile["id"]);

                        if (commodityID == 1)
                            Debug.Print("Stop");

                        BaseName  = (String)LocalizationFromFile[Program.BASE_LANGUAGE];

                        if ((commodityID == null) || (commodityID < 0))
                        {
                            // no id or selfcreated
                            Commodity = Data.tbcommodity.Select("commodity = " + DBConnector.SQLAString(DBConnector.DTEscape(BaseName)));
                        }
                        else
                        { 
                            // confirmed commodity with id    
                            Commodity = Data.tbcommodity.Select("id = " + commodityID);
                        }
                            

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
                            lDBCon.TableUpdate(Data.tbcommodity);

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
                                newRow["language_id"] = LanguageFormFile.Value;
                                if((String)LocalizationFromFile[LanguageFormFile.Key] == "")
                                    newRow["locname"] = BaseName;
                                else
                                    newRow["locname"] = (String)LocalizationFromFile[LanguageFormFile.Key];

                                Data.tbcommoditylocalization.Rows.Add(newRow);
                            }
                            else if((importType == enLocalisationImportType.overWriteAll) || 
                                   ((importType == enLocalisationImportType.overwriteNonBase) && (LanguageFormFile.Key != Program.BASE_LANGUAGE)))
                            {
                                if((String)LocalizationFromFile[LanguageFormFile.Key] != "")
                                    currentLocalizations[0]["locname"] = (String)LocalizationFromFile[LanguageFormFile.Key];
                            }

                        }

                        Counter++;
                        sendProgressEvent(new ProgressEventArgs() {Info="import commodity localization", CurrentValue=Counter, TotalValue=DataNames.Tables["Names"].Rows.Count });

                        //if((Counter % 50) == 0)
                        //    lDBCon.TableUpdate(Data.tbcommoditylocalization);
                    }
                }
                // submit changes
                lDBCon.TableUpdate(Data.tbcommoditylocalization);

                lDBCon.TableReadRemove(Data.tblanguage);
                lDBCon.TableReadRemove(Data.tbcommoditylocalization);
                lDBCon.TableReadRemove(Data.tbcommodity);

                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();

            }
            catch (Exception ex)
            {
                try
                {
                    // reset freaky performance
                    lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");
                }
                catch (Exception) { }

                lDBCon.TableReadRemove(Data.tblanguage);
                lDBCon.TableReadRemove(Data.tbcommoditylocalization);
                lDBCon.TableReadRemove(Data.tbcommodity);

                lDBCon.Dispose();

                throw new Exception("Error while loading commodity names", ex);
            }

        }

        /// <summary>
        /// loads the localized economy level names and check
        /// </summary>
        internal void ImportEconomyLevelLocalizations(String Filename)
        {
            dsEliteDB                 Data;
            DataSet                   DataNames;
            Dictionary<String, Int32> foundLanguagesFromFile     = new Dictionary<String, Int32>();
            Int32 Counter = 0;

            Data      = new dsEliteDB();
            DataNames = new DataSet();

            try
            {

                DataNames.ReadXml(Filename);

                Program.DBCon.TableRead("select * from tbLanguage", Data.tblanguage);
                Program.DBCon.TableRead("select * from tbLevelLocalization", Data.tblevellocalization);
                Program.DBCon.TableRead("select * from tbEconomyLevel", Data.tbeconomylevel);

                if(DataNames.Tables["Levels"] != null)
                { 
                    sendProgressEvent(new ProgressEventArgs() {Info="import economy level localization", CurrentValue=Counter, TotalValue=DataNames.Tables["Levels"].Rows.Count });

                    // first check if there's a new language
                    foreach (DataColumn LanguageFromFile in DataNames.Tables["Levels"].Columns)
                    {
                        if(!LanguageFromFile.ColumnName.Equals("ID", StringComparison.InvariantCultureIgnoreCase))
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
                    
                    }
                
                    // submit changes (tbLanguage)
                    Program.DBCon.TableUpdate(Data.tblanguage);

                    // compare and add the localized names
                    foreach (DataRow LocalizationFromFile in DataNames.Tables["Levels"].AsEnumerable())
                    {
                        String    BaseName              = (String)LocalizationFromFile[Program.BASE_LANGUAGE];
                        DataRow[] Level                 = Data.tbeconomylevel.Select("level = " + DBConnector.SQLAString(DBConnector.DTEscape(BaseName)));

                        foreach (KeyValuePair<String, Int32> LanguageFormFile in foundLanguagesFromFile)
	                    {
                            DataRow[] currentLocalizations  = Data.tblevellocalization.Select("     economylevel_id  = " + Level[0]["id"] + 
                                                                                                  " and language_id  = " + LanguageFormFile.Value);

                            if(currentLocalizations.Count() == 0)
                            {
                                // add a new localization
                                dsEliteDB.tblevellocalizationRow newRow = (dsEliteDB.tblevellocalizationRow)Data.tblevellocalization.NewRow();

                                newRow.economylevel_id = (Int32)Level[0]["id"];
                                newRow.language_id     = LanguageFormFile.Value;
                                newRow.locname         = LocalizationFromFile[LanguageFormFile.Key].ToString();

                                Data.tblevellocalization.Rows.Add(newRow);
                            }
	                    }

                        Counter++;
                        sendProgressEvent(new ProgressEventArgs() {Info="import economy level localization", CurrentValue=Counter,  TotalValue=DataNames.Tables["Levels"].Rows.Count });
                    }
                }
                // submit changes
                Program.DBCon.TableUpdate(Data.tblevellocalization);

                Program.DBCon.TableReadRemove(Data.tblanguage);
                Program.DBCon.TableReadRemove(Data.tblevellocalization);
                Program.DBCon.TableReadRemove(Data.tbeconomylevel);

            }
            catch (Exception ex)
            {
                Program.DBCon.TableReadRemove(Data.tblanguage);
                Program.DBCon.TableReadRemove(Data.tblevellocalization);
                Program.DBCon.TableReadRemove(Data.tbeconomylevel);

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

                sendProgressEvent(new ProgressEventArgs() {Info="import warnlevels", CurrentValue=Counter, TotalValue=WarnLevels.Count });

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
                    sendProgressEvent(new ProgressEventArgs() { Info="import warnlevels", CurrentValue=Counter, TotalValue=WarnLevels.Count });
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

        /// <summary>
        /// retrieves all known commodity names in the current language
        /// </summary>
        /// <returns></returns>
        internal List<string> getCommodityNames()
        {
            String sqlString;
            DataTable Data;
            List<string> retValue = new  List<string>();

            try
            {
                Data = new DataTable();
                
                sqlString = "select loccommodity from tbCommodity";
                Program.DBCon.Execute(sqlString, Data);

                foreach (DataRow currentRow in Data.Rows)
                    retValue.Add((String)currentRow["loccommodity"]);

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting names of all commodities", ex);
            }
        }

        /// <summary>
        /// adds for all items (commodities, levels, categories) the missing entries in the localization tables
        /// </summary>
        internal void AddMissingLocalizationEntries()
        {
            string sqlString;
            dsEliteDB.tblanguageDataTable data;

            try
            {
                data = new dsEliteDB.tblanguageDataTable();

                sqlString = "select * from tbLanguage order by id";
                
                Program.DBCon.Execute(sqlString, data);

                foreach (dsEliteDB.tblanguageRow dRow in data.Rows)
	            {
                    /* add missing entrys for commodities */
                    sqlString = String.Format("insert into tbcommoditylocalization (commodity_id, language_id, locname)" + 
                                              "   select C.id as commodity_id, {0} as language_id, C.commodity from tbcommodity C" +
                                              "                    left join" +
                                              "                 (select * from tbcommoditylocalization where language_id = {0}) L" +
                                              "                    on C.ID = L.commodity_id" +
                                              "      where L.commodity_id is null;", 
                                              dRow.id);
                    Program.DBCon.Execute(sqlString);  

                    /* add missing entrys for categories */
                    sqlString = String.Format("insert into tbcategorylocalization (category_id, language_id, locname)" + 
                                              "   select C.id as category_id, {0} as language_id, C.category from tbcategory C" +
                                              "                    left join" +
                                              "                 (select * from tbcategorylocalization where language_id = {0}) L" +
                                              "                    on C.ID = L.category_id" +
                                              "      where L.category_id is null;", 
                                              dRow.id);
                    Program.DBCon.Execute(sqlString);  

                    /* add missing entrys for economylevels */
                    sqlString = String.Format("insert into tblevellocalization (economylevel_id, language_id, locname)" + 
                                              "   select C.id as economylevel_id, {0} as language_id, C.level from tbeconomylevel C" +
                                              "                    left join" +
                                              "                 (select * from tblevellocalization where language_id = {0}) L" +
                                              "                    on C.ID = L.economylevel_id" +
                                              "      where L.economylevel_id is null;", 
                                              dRow.id);
                    Program.DBCon.Execute(sqlString);  
	            }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding missing localization entrys", ex);
            }        
        }

#endregion

#region handling of systems

        /// <summary>
        /// imports the data from the file into the database
        /// (only newer data will be imported)
        /// </summary>
        /// <param name="fileName"></param>
        public void ImportSystems(String Filename)
        {
            String sqlString;
            List<EDSystem> Systems;
            EDSystem importSystem;
            dsEliteDB.tbsystemsRow[] FoundRows;
            dsEliteDB.tbsystems_orgRow[] FoundRows_org;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<Int32, Int32>();
            dsEliteDB localDataSet;
            Int32 counter = 0;
            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);
            Boolean dataChanged;
            localDataSet = new dsEliteDB();
            Int32 updated = 0;
            Int32 added = 0;
            MySql.Data.MySqlClient.MySqlDataAdapter dataAdapter_sys = null;
            MySql.Data.MySqlClient.MySqlDataAdapter dataAdapter_sysorg = null;
            Int32 systemsTotal=0;

            try
            {
                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");
                StreamReader rawDataStream;
                JsonTextReader jsonReader;
                JsonSerializer serializer = new JsonSerializer();

                rawDataStream = new StreamReader(Filename);
                jsonReader = new JsonTextReader(rawDataStream);

                sendProgressEvent(new ProgressEventArgs() {  Info="import systems...", NewLine = true } );

                while (jsonReader.Read())
                    if(jsonReader.TokenType == JsonToken.StartObject)
                        systemsTotal++;

                jsonReader.Close();
                rawDataStream.Close();
                rawDataStream.Dispose();

                rawDataStream = new StreamReader(Filename);
                jsonReader = new JsonTextReader(rawDataStream);

                while(jsonReader.Read())
                {
                    if(jsonReader.TokenType == JsonToken.StartObject)
                    {
                        dataChanged = false;

                        importSystem = serializer.Deserialize<EDSystem>(jsonReader);

                        localDataSet.Clear();
                        if(dataAdapter_sys != null)
                        {
                            dataAdapter_sys.Dispose();
                            dataAdapter_sys = null;
                        }

                        if(dataAdapter_sysorg != null)
                        {
                            dataAdapter_sysorg.Dispose();
                            dataAdapter_sysorg = null;
                        }

                        lDBCon.TableRead(String.Format("select * from tbSystems where id = {0} lock in share mode;", importSystem.Id), localDataSet.tbsystems, ref dataAdapter_sys);

                        //sqlString = "select * from tbSystems_org lock in share mode";
                        //lDBCon.TableRead(sqlString, Data.tbsystems_org);

                        if (localDataSet.tbsystems.Rows.Count > 0)
                        {
                            // system is existing

                            if ((bool)(localDataSet.tbsystems.Rows[0]["is_changed"]))
                            {
                                // data is changed by user - hold it ...

                                // ...and check table "tbSystems_org" for the original data
                                lDBCon.TableRead(String.Format("select * from tbSystems_org where id = {0} lock in share mode;", importSystem.Id), localDataSet.tbsystems_org, ref dataAdapter_sysorg);

                                if (localDataSet.tbsystems_org.Rows.Count > 0)
                                {
                                    // system is in "tbSystems_org" existing - keep the newer version 
                                    Timestamp_old = (DateTime)(localDataSet.tbsystems_org.Rows[0]["updated_at"]);
                                    Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(importSystem.UpdatedAt).DateTime;

                                    if (Timestamp_new > Timestamp_old)
                                    {
                                        // data from file is newer
                                        CopyEDSystemToDataRow(importSystem, (DataRow)localDataSet.tbsystems_org.Rows[0], false, null, true);
                                        ImportCounter += 1;
                                        dataChanged = true;
                                    }
                                }
                            }
                            else
                            {
                                // system is existing - keep the newer version 
                                Timestamp_old = (DateTime)(localDataSet.tbsystems.Rows[0]["updated_at"]);
                                Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(importSystem.UpdatedAt).DateTime;

                                if (Timestamp_new > Timestamp_old)
                                {
                                    // data from file is newer
                                    CopyEDSystemToDataRow(importSystem, localDataSet.tbsystems.Rows[0], false, null, true);
                                    ImportCounter += 1;
                                    dataChanged = true;
                                    updated += 1;
                                }
                            }
                        }
                        else
                        {
                            if(dataAdapter_sys != null)
                            {
                                dataAdapter_sys.Dispose();
                                dataAdapter_sys = null;
                            }

                            // check if there's a user generated system
                            // self-created systems don't have the correct id so it must be identified by name    
                            lDBCon.TableRead(String.Format("select * from tbSystems where systemname = {0} and id < 0 lock in share mode;", DBConnector.SQLAEscape(importSystem.Name.ToString()) ), localDataSet.tbsystems, ref dataAdapter_sys);

                            if (localDataSet.tbsystems.Rows.Count > 0)
                            {
                                // self created systems is existing -> correct id and get new data from EDDB
                                // (changed system_id in tbStations are automatically internal updated by the database itself)
                                CopyEDSystemToDataRow(importSystem, (DataRow)localDataSet.tbsystems.Rows[0], false, null, true);
                                dataChanged = true;
                            }
                            else
                            {
                                // add a new system
                                dsEliteDB.tbsystemsRow newRow = (dsEliteDB.tbsystemsRow)localDataSet.tbsystems.NewRow();
                                CopyEDSystemToDataRow(importSystem, (DataRow)newRow, false, null, true);
                                localDataSet.tbsystems.Rows.Add(newRow);
                                dataChanged = true;
                            }

                            added += 1;
                            ImportCounter += 1;
                        }

                        if(dataChanged)
                        {
                            if(localDataSet.tbsystems.Rows.Count > 0)
                                lDBCon.TableUpdate(localDataSet.tbsystems, dataAdapter_sys);

                            if(localDataSet.tbsystems_org.Rows.Count > 0)
                                lDBCon.TableUpdate(localDataSet.tbsystems_org, dataAdapter_sysorg);

                            dataChanged = false;
                        }

                        counter++;
                        
                        if(sendProgressEvent(new ProgressEventArgs() { Info = String.Format("import systems : analysed={0}, updated={1}, added={2}", counter, ImportCounter-added, added), CurrentValue=counter, TotalValue=systemsTotal}))
                            break;
                    }
                }

                // reset freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();

            }
            catch (Exception ex)
            {
                if (lDBCon.TransActive())
                    lDBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    lDBCon.TableReadRemove(Program.Data.BaseData.tbsystems);
                    lDBCon.TableReadRemove(Program.Data.BaseData.tbsystems_org);

                    lDBCon.Dispose();
                }
                catch (Exception) { }

                throw new Exception("Error while importing system data", ex);
            }
        }

        /// <summary>
        /// imports the own data from the file into the database (only initially once needed)
        /// </summary>
        /// <param name="fileName"></param>
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
        /// <param name="fileName"></param>
        public Dictionary<Int32, Int32> ImportSystems_Own(EDSystem System, Boolean OnlyAddUnknown = false, Boolean setVisitedFlag = false)
        {
            try
            {
                List<EDSystem> SystemList = new List<EDSystem>() {System};
                return ImportSystems_Own(ref SystemList, OnlyAddUnknown, setVisitedFlag);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in <ImportSystems_Own> from single station", ex);
            }
        }

        /// <summary>
        /// imports the data from the list of systems
        /// </summary>
        /// <param name="fileName"></param>
        public Dictionary<Int32, Int32> ImportSystems_Own(ref List<EDSystem> Systems, Boolean OnlyAddUnknown = false, Boolean setVisitedFlag = false)
        {
            String sqlString;
            dsEliteDB.tbsystemsRow[] FoundRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            Dictionary<Int32, Int32> changedSystemIDs = new Dictionary<Int32, Int32>();
            Int32 currentSelfCreatedIndex = -1;
            dsEliteDB Data;
            Int32 Counter = 0;

            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);

            try
            {

                Data = new dsEliteDB();

                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                lDBCon.TransBegin();

                sqlString = "select * from tbSystems lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbsystems);

                sqlString = "select * from tbSystems_org lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbsystems_org);

                currentSelfCreatedIndex = getNextOwnSystemIndex();

                sendProgressEvent(new ProgressEventArgs() { Info="import self-added systems", CurrentValue=Counter, TotalValue=Systems.Count });

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
                                    Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(System.UpdatedAt).DateTime;

                                    if (Timestamp_new > Timestamp_old)
                                    {
                                        // data from file is newer -> take it but hold the old id
                                        CopyEDSystemToDataRow(System, (DataRow)FoundRows[0], true, null, true);

                                        ImportCounter += 1;
                                    }
                                }
                                else
                                {
                                    // new data is user changed data, old data is original data
                                    // copy the original data ("tbSystems") to the saving data table ("tbSystems_org")
                                    // and get the correct system ID
                                    Data.tbsystems_org.LoadDataRow(FoundRows[0].ItemArray, false);
                                    CopyEDSystemToDataRow(System, (DataRow)FoundRows[0], true, null, true);

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
                            CopyEDSystemToDataRow(System, (DataRow)newRow, true, null, true);

                            newRow.visited      = setVisitedFlag;
                            newRow.updated_at   = DateTime.UtcNow;

                            Data.tbsystems.Rows.Add(newRow);

                            currentSelfCreatedIndex -= 1;
                            ImportCounter += 1;

                        }

                        if ((ImportCounter > 0) && ((ImportCounter % 100) == 0))
                        {
                            // save changes
                            Debug.Print("added Systems : " + ImportCounter.ToString());

                            lDBCon.TableUpdate(Data.tbsystems);
                            lDBCon.TableUpdate(Data.tbsystems_org);
                        }
                    }

                    Counter++;
                    sendProgressEvent(new ProgressEventArgs() { Info="import self-added systems", CurrentValue=Counter, TotalValue=Systems.Count });
                }

                // save changes
                lDBCon.TableUpdate(Data.tbsystems, true);
                lDBCon.TableUpdate(Data.tbsystems_org, true);

                lDBCon.TransCommit();

                // reset freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();

                // return all changed ids
                return changedSystemIDs;
            }
            catch (Exception ex)
            {
                if (lDBCon.TransActive())
                    lDBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    lDBCon.TableReadRemove("tbSystems");
                    lDBCon.TableReadRemove("tbSystems_org");

                    lDBCon.Dispose();
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
        private void CopyEDSystemToDataRow(EDSystem SystemObject, DataRow SystemRow, Boolean OwnData = false, int? SystemID = null, Boolean insertUnknown = false)
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
                SystemRow["government_id"]          = DBConvert.From(BaseTableNameToID("government", SystemObject.Government, insertUnknown));
                SystemRow["allegiance_id"]          = DBConvert.From(BaseTableNameToID("allegiance", SystemObject.Allegiance, insertUnknown));
                SystemRow["state_id"]               = DBConvert.From(BaseTableNameToID("state", SystemObject.State, insertUnknown));
                SystemRow["security_id"]            = DBConvert.From(BaseTableNameToID("security", SystemObject.Security, insertUnknown));
                SystemRow["primary_economy_id"]     = DBConvert.From(BaseTableNameToID("economy", SystemObject.PrimaryEconomy, insertUnknown));
                SystemRow["power_id"]               = DBConvert.From(BaseTableNameToID("power", SystemObject.Power, insertUnknown));
                SystemRow["powerstate_id"]          = DBConvert.From(BaseTableNameToID("powerstate", SystemObject.PowerState, insertUnknown));
                SystemRow["needs_permit"]           = DBConvert.From(SystemObject.NeedsPermit);
                SystemRow["updated_at"]             = DBConvert.From(DateTimeOffset.FromUnixTimeSeconds(SystemObject.UpdatedAt).DateTime);
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
        /// <param name="fileName"></param>
        public void ImportStations(String Filename, Boolean addPrices)
        {
            String sqlString;
            List<EDStation> Stations;
            dsEliteDB.tbstations_orgRow[] FoundRows_org;
            dsEliteDB.tbstationsRow[] FoundRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            dsEliteDB Data;
            Int32 Counter = 0;
            UInt32 currentComodityClassificationID=0;
            Int32 updated = 0;
            Int32 added = 0;


            Data = new dsEliteDB();

            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);

            try
            {

                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                Stations = JsonConvert.DeserializeObject<List<EDStation>>(File.ReadAllText(Filename));

                sendProgressEvent(new ProgressEventArgs() { Info="import systems", NewLine = true });

                lDBCon.TransBegin();

                sqlString = "select * from tbStations lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbstations);
                sqlString = "select * from tbStations_org lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbstations_org);
                sqlString = "select * from tbStationEconomy lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbstationeconomy);
                sqlString = "select * from tbsource";
                lDBCon.Execute(sqlString, Data.tbsource);
                sqlString = "select * from tbCommodityClassification lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbcommodityclassification);
                sqlString = "select * from tbcommodity_has_attribute lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbcommodity_has_attribute);
                sqlString = "select * from tbattribute lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbattribute);

                currentComodityClassificationID = getFreeIndex("tbCommodityClassification");

                lDBCon.Execute(sqlString, Data.tbsource);

                foreach (EDStation Station in Stations)
                {

                    FoundRows = (dsEliteDB.tbstationsRow[])Data.tbstations.Select("id=" + Station.Id.ToString());

                    if (FoundRows.Count() > 0)
                    {
                        // Location is existing

                        if ((bool)(FoundRows[0]["is_changed"]))
                        {
                            // data is changed by user - hold it ...

                            // ...and check table "tbStations_org" for the original data
                            FoundRows_org = (dsEliteDB.tbstations_orgRow[])Data.tbstations_org.Select("id=" + Station.Id.ToString());

                            if ((FoundRows_org != null) && (FoundRows_org.Count() > 0))
                            {
                                // Location is in "tbStations_org" existing - keep the newer version 
                                Timestamp_old = (DateTime)(FoundRows_org[0]["updated_at"]);
                                Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(Station.UpdatedAt).DateTime;

                                if (Timestamp_new > Timestamp_old)
                                {
                                    // data from file is newer
                                    CopyEDStationToDataRow(Station, (DataRow)FoundRows_org[0], false, null, true);

                                    CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                                    CopyEDStationCommodityToDataRow(Station, Data, ref currentComodityClassificationID);

                                    ImportCounter += 1;

                                }
                            }

                        }
                        else
                        {
                            // Location is existing - keep the newer version 
                            Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                            Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(Station.UpdatedAt).DateTime;

                            if (Timestamp_new > Timestamp_old)
                            {
                                // data from file is newer
                                CopyEDStationToDataRow(Station, (DataRow)FoundRows[0], false, null, true);

                                CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                                CopyEDStationCommodityToDataRow(Station, Data, ref currentComodityClassificationID);

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
                            CopyEDStationToDataRow(Station, (DataRow)FoundRows[0], false, null, true);

                            // update immediately because otherwise the references are wrong after changing a id
                            lDBCon.TableUpdate(Data.tbstations);
                            lDBCon.TableUpdate(Data.tbstations_org);
                            lDBCon.TableUpdate(Data.tbstationeconomy);
                            lDBCon.TableUpdate(Data.tbcommodityclassification);
                            lDBCon.TableUpdate(Data.tbcommodity_has_attribute);
                            lDBCon.TableUpdate(Data.tbattribute);

                            lDBCon.TableRefresh(Data.tbstationeconomy);
                            lDBCon.TableRefresh(Data.tbcommodityclassification);
                            lDBCon.TableRefresh(Data.tbcommodity_has_attribute);
                            lDBCon.TableRefresh(Data.tbattribute);
                        }
                        else
                        {
                            // add a new Location
                            dsEliteDB.tbstationsRow newStationRow = (dsEliteDB.tbstationsRow)Data.tbstations.NewRow();

                            CopyEDStationToDataRow(Station, (DataRow)newStationRow, false, null, true);
                            Data.tbstations.Rows.Add(newStationRow);
                            added++;
                        }

                        CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                        CopyEDStationCommodityToDataRow(Station, Data, ref currentComodityClassificationID);

                        ImportCounter += 1;
                    }

                    if ((ImportCounter > 0) && ((ImportCounter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Stations : " + ImportCounter.ToString());

                        lDBCon.TableUpdate(Data.tbstations);
                        lDBCon.TableUpdate(Data.tbstations_org);
                        lDBCon.TableUpdate(Data.tbstationeconomy);
                        lDBCon.TableUpdate(Data.tbcommodityclassification);
                        lDBCon.TableUpdate(Data.tbcommodity_has_attribute);
                        lDBCon.TableUpdate(Data.tbattribute);
                    }

                    Counter++;

                    if(sendProgressEvent(new ProgressEventArgs() { Info = String.Format("import stations : analysed={0}, updated={1}, added={2}", Counter, ImportCounter-added, added), CurrentValue=Counter, TotalValue=Stations.Count}))
                        break;
                }

                // save changes
                lDBCon.TableUpdate(Data.tbstations, true);
                lDBCon.TableUpdate(Data.tbstations_org, true);
                lDBCon.TableUpdate(Data.tbstationeconomy, true);
                lDBCon.TableUpdate(Data.tbcommodityclassification, true);
                lDBCon.TableUpdate(Data.tbcommodity_has_attribute, true);
                lDBCon.TableUpdate(Data.tbattribute, true);

                lDBCon.TransCommit();

                // now add the prices if wanted
                if (addPrices)
                {
                    ImportPrices(Stations, enImportBehaviour.OnlyNewer, enDataSource.fromEDDN);
                }

                // reset freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();

            }
            catch (Exception ex)
            {
                if (lDBCon.TransActive())
                    lDBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    lDBCon.TableReadRemove(Data.tbstations);
                    lDBCon.TableReadRemove(Data.tbstations_org);
                    lDBCon.TableReadRemove(Data.tbstationeconomy);
                    lDBCon.TableReadRemove(Data.tbcommodityclassification);
                    lDBCon.TableReadRemove(Data.tbcommodity_has_attribute);
                    lDBCon.TableReadRemove(Data.tbattribute);

                    lDBCon.Dispose();
                }
                catch (Exception) { }

                throw new Exception("Error while importing Station data", ex);
            }
        }

        /// <summary>
        /// imports the "own" station data from the file into the database (only initially once needed)
        /// </summary>
        /// <param name="fileName"></param>
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
        /// <param name="fileName"></param>
        public void ImportStations_Own(EDStation Station, Boolean OnlyAddUnknown = false, Boolean setVisitedFlag = false)
        {
            try
            {
                ImportStations_Own(new List<EDStation>() {Station}, new Dictionary<Int32, Int32>(), OnlyAddUnknown, setVisitedFlag);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in <ImportSystems_Own> from single station", ex);
            }
        }

        /// <summary>
        /// imports the "own" station data from the list of stations
        /// </summary>
        /// <param name="fileName"></param>
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
        /// <param name="fileName"></param>
        public void ImportStations_Own(List<EDStation> Stations, Dictionary<Int32, Int32> changedSystemIDs, Boolean OnlyAddUnknown = false, Boolean setVisitedFlag = false)
        {
            String sqlString;
            dsEliteDB.tbstationsRow[] FoundRows;
            dsEliteDB.tbsystemsRow[] FoundSysRows;
            DateTime Timestamp_new, Timestamp_old;
            Int32 ImportCounter = 0;
            Int32 currentSelfCreatedIndex = -1;
            dsEliteDB Data = new dsEliteDB();
            Int32 Counter = 0;

            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);

            try
            {

                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                lDBCon.TransBegin();

                sqlString = "select * from tbStations lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbstations);
                sqlString = "select * from tbStations_org lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbstations_org);
                sqlString = "select * from tbStationEconomy lock in share mode";
                lDBCon.TableRead(sqlString, Data.tbstationeconomy);

                // get the smallest ID for self added stations
                sqlString = "select min(id) As min_id from tbStations";
                lDBCon.Execute(sqlString, "minID", Data);

                if (Convert.IsDBNull(Data.Tables["minID"].Rows[0]["min_id"]))
                    currentSelfCreatedIndex = -1;
                else
                {
                    currentSelfCreatedIndex = ((Int32)Data.Tables["minID"].Rows[0]["min_id"]) - 1;
                    if (currentSelfCreatedIndex >= 0)
                        currentSelfCreatedIndex = -1;
                }

                sendProgressEvent(new ProgressEventArgs() { Info="import self-added stations", CurrentValue=Counter,  TotalValue=Stations.Count });

                foreach (EDStation Station in Stations)
                {
                    Int32 SystemID;

                    if(Station.Name == "Glass City")
                        Debug.Print("stop");

                    // is the system id changed ? --> get the new system id, otherwise the original
                    if (changedSystemIDs.TryGetValue(Station.SystemId, out SystemID))
                        Station.SystemId = SystemID;

                    // if there are missing system ids, try to get them
                    if ((Station.SystemId == 0) && (!String.IsNullOrEmpty(Station.SystemName)))
                    {
                        FoundSysRows = (dsEliteDB.tbsystemsRow[])Data.tbsystems.Select("systemname=" + DBConnector.SQLAString(DBConnector.DTEscape(Station.SystemName)));

                        if((FoundSysRows != null) && (FoundSysRows.Count() > 0))
                        {
                            // got it - set the id
                            Station.SystemId = FoundSysRows[0].id;
                        }
                    }

                    if (!String.IsNullOrEmpty(Station.Name.Trim()) && (Station.SystemId != 0))
                    {
                        // self-created stations don't have the correct id so they must be identified by name    
                        FoundRows = (dsEliteDB.tbstationsRow[])Data.tbstations.Select("stationname=" + DBConnector.SQLAString(DBConnector.DTEscape(Station.Name)) + " and " +
                                                                     "system_id =  " + Station.SystemId);

                        if ((FoundRows != null) && (FoundRows.Count() > 0))
                        {
                            // Location is existing, get the same Id
                            Station.Id = (Int32)FoundRows[0]["id"];

                            if (!OnlyAddUnknown)
                            {

                                if ((bool)(FoundRows[0]["is_changed"]))
                                {
                                    // existing data data is also changed by user - keep the newer version 
                                    Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                                    Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(Station.UpdatedAt).DateTime;

                                    if (Timestamp_new > Timestamp_old)
                                    {
                                        // data from file is newer
                                        CopyEDStationToDataRow(Station, (DataRow)FoundRows[0], true, null, true);

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
                                    CopyEDStationToDataRow(Station, (DataRow)FoundRows[0], true, null, true);

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

                            CopyEDStationToDataRow(Station, (DataRow)newRow, true, null, true);
                            newRow.visited      = setVisitedFlag;
                            newRow.updated_at   = DateTime.UtcNow;

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

                            lDBCon.TableUpdate(Data.tbstations);
                            lDBCon.TableUpdate(Data.tbstations_org);
                            lDBCon.TableUpdate(Data.tbstationeconomy);
                        }
                    }
                    else
                        Debug.Print("why");


                    Counter++;
                    sendProgressEvent(new ProgressEventArgs() { Info="import self-added stations", CurrentValue=Counter, TotalValue=Stations.Count });

                }

                // save changes
                lDBCon.TableUpdate(Data.tbstations, true);
                lDBCon.TableUpdate(Data.tbstations_org, true);
                lDBCon.TableUpdate(Data.tbstationeconomy, true);

                lDBCon.TransCommit();

                // reset freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();
            }
            catch (Exception ex)
            {
                if (lDBCon.TransActive())
                    lDBCon.TransRollback();

                try
                {
                    // reset freaky performance
                    lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                    lDBCon.TableReadRemove(Data.tbstations);
                    lDBCon.TableReadRemove(Data.tbstations_org);
                    lDBCon.TableReadRemove(Data.tbstationeconomy);

                    lDBCon.Dispose();
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
        private void CopyEDStationToDataRow(EDStation StationObject, DataRow StationRow, Boolean OwnData = false, int? StationID = null, Boolean insertUnknown = false)
        {
            try
            {

                StationRow["id"]                    = StationID == null ? DBConvert.From(StationObject.Id) : StationID;
                StationRow["stationname"]           = DBConvert.From(StationObject.Name);
                StationRow["system_id"]             = DBConvert.From(StationObject.SystemId);
                StationRow["max_landing_pad_size"]  = DBConvert.From(StationObject.MaxLandingPadSize);
                StationRow["distance_to_star"]      = DBConvert.From(StationObject.DistanceToStar);
                StationRow["faction"]               = DBConvert.From(StationObject.Faction);
                StationRow["government_id"]         = DBConvert.From(BaseTableNameToID("government", StationObject.Government, insertUnknown));
                StationRow["allegiance_id"]         = DBConvert.From(BaseTableNameToID("allegiance", StationObject.Allegiance, insertUnknown));
                StationRow["state_id"]              = DBConvert.From(BaseTableNameToID("state", StationObject.State, insertUnknown));
                StationRow["stationtype_id"]        = DBConvert.From(BaseTableNameToID("stationtype", StationObject.Type, insertUnknown));
                StationRow["has_blackmarket"]       = DBConvert.From(StationObject.HasBlackmarket);
                StationRow["has_market"]            = DBConvert.From(StationObject.HasMarket);
                StationRow["has_refuel"]            = DBConvert.From(StationObject.HasRefuel);
                StationRow["has_repair"]            = DBConvert.From(StationObject.HasRepair);
                StationRow["has_outfitting"]        = DBConvert.From(StationObject.HasOutfitting);
                StationRow["has_shipyard"]          = DBConvert.From(StationObject.HasShipyard);
                StationRow["updated_at"]            = DBConvert.From(DateTimeOffset.FromUnixTimeSeconds(StationObject.UpdatedAt).DateTime);
                StationRow["is_changed"]            = OwnData ? DBConvert.From(1) : DBConvert.From(0);
                StationRow["visited"]               = DBConvert.From(0);
                StationRow["shipyard_updated_at"]   = DBConvert.From(DateTimeOffset.FromUnixTimeSeconds(StationObject.Shipyard_UpdatedAt.GetValueOrDefault()).DateTime);
                StationRow["outfitting_updated_at"] = DBConvert.From(DateTimeOffset.FromUnixTimeSeconds(StationObject.Outfitting_UpdatedAt.GetValueOrDefault()).DateTime);
                StationRow["market_updated_at"]     = DBConvert.From(DateTimeOffset.FromUnixTimeSeconds(StationObject.Market_UpdatedAt.GetValueOrDefault()).DateTime);
                StationRow["type_id"]               = DBConvert.From(StationObject.TypeID);
                StationRow["has_commodities"]       = DBConvert.From(StationObject.HasCommodities); 
                StationRow["is_planetary"]          = DBConvert.From(StationObject.IsPlanetary);

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
        private void CopyEDStationCommodityToDataRow(EDStation StationObject, dsEliteDB Data, ref UInt32 AutoIndex)
        {
            Int32 i;
            String[] currentCommodityCollection = null;
            String currentCommodityAttribute = null;

            try
            {
               
                var existingClassification      =  ((dsEliteDB.tbcommodityclassificationRow[])Data.tbcommodityclassification.Select("station_id = " + StationObject.Id)).ToList();
                var newCommodityClassification  = new Dictionary<String, List<String>>();

                // collect classification data
                for(i=0 ; i<=2 ; i++)
                {
                    switch (i)
                    {
                    	case 0:
                    		currentCommodityCollection  = StationObject.ImportCommodities;
                            currentCommodityAttribute   = "import";
                    		break;
                    	case 1:
                    		currentCommodityCollection  = StationObject.ExportCommodities;
                            currentCommodityAttribute   = "export";
                    		break;
                    	case 2:
                    		currentCommodityCollection  = StationObject.ProhibitedCommodities;
                            currentCommodityAttribute   = "prohibited";
                    		break;
                    }

                    foreach (var Commodity in currentCommodityCollection)
	                {
                        // cyxle throught the <Attribute>-commodities from the station
                        List<String> currentClassification;
		                if(!newCommodityClassification.TryGetValue(Commodity, out currentClassification))
                        {
                            // this commodity is not registered at all
                            var newCL = new List<String>() {currentCommodityAttribute};
                            newCommodityClassification.Add(Commodity, newCL);
                        }
                        else
                        {
                            // this commodity is already registered
                            if(!currentClassification.Contains(currentCommodityAttribute))
                            {
                                // but not yet for this classification
                                currentClassification.Add(currentCommodityAttribute);
                            }
                        }
	                }
                }

                // process classification data
                foreach (var Classification in newCommodityClassification)
                {
                    // get the current commodity id
                    Int32  CommodityID = (Int32)DBConvert.From(BaseTableNameToID("commodity", Classification.Key));
                    UInt32 CClassifID;

                    // and check, if the commodity is already added to station
                    var Found = from dsEliteDB.tbcommodityclassificationRow relevantCommodity in existingClassification
                                where  ((relevantCommodity.RowState      != DataRowState.Deleted) 
                                     && (relevantCommodity.commodity_id  == CommodityID))
                                select relevantCommodity;

                    // if it's not existing, insert commodity
                    if(Found.Count() == 0)
                    {
                        var newRow = (dsEliteDB.tbcommodityclassificationRow)Data.tbcommodityclassification.NewRow();

                        newRow.id           = AutoIndex;
                        newRow.station_id   = StationObject.Id;
                        newRow.commodity_id = CommodityID;

                        Data.tbcommodityclassification.Rows.Add(newRow);

                        CClassifID          = newRow.id;
                        AutoIndex          += 1;
                    }
                    else
                    {
                        // memorize the id and remove commodity from list to mark it as "found"
                        CClassifID = Found.First().id;
                        existingClassification.Remove(Found.First());
                        //Debug.Print("removed " + Classification.Key);
                    }

                    var existingAttributes    =  ((dsEliteDB.tbcommodity_has_attributeRow[])Data.tbcommodity_has_attribute.Select("tbcommodityclassification_id   = " + CClassifID)).ToList();

                    // now check the attributes for this commodity
                    foreach (var Attribute in Classification.Value)
                    {
                        // get the current attribute id
                        Int32 AttributeID = (Int32)DBConvert.From(BaseTableNameToID("attribute", Attribute));    

                        // and check, if the attribute is already added to classification
                        var FoundCC = from dsEliteDB.tbcommodity_has_attributeRow relevantCommodity in existingAttributes
                                    where   relevantCommodity.RowState        != DataRowState.Deleted
                                         && relevantCommodity.tbAttribute_id  == AttributeID
                                    select relevantCommodity;

                        // if it's not existing, insert attribute
                        if(FoundCC.Count() == 0)
                        {
                            var newRow = (dsEliteDB.tbcommodity_has_attributeRow)Data.tbcommodity_has_attribute.NewRow();

                            newRow.tbAttribute_id               = AttributeID;
                            newRow.tbCommodityClassification_id = CClassifID;

                            Data.tbcommodity_has_attribute.Rows.Add(newRow);
                        }
                        else
                        {
                            // remove attribute from list to mark it as "found"
                            existingAttributes.Remove(FoundCC.First());
                        }
                    }

                    // remove all old, not more existing attributes
                    foreach (DataRow RemovedRow in existingAttributes)
                        Data.tbcommodity_has_attribute.Rows.Remove(RemovedRow);    

                }

                // remove all old, not more existing classification
                foreach (DataRow RemovedRow in existingClassification)
                    Data.tbcommodityclassification.Rows.Remove(RemovedRow);    

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

            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);

            try
            {
                List<StationVisit> History  = JsonConvert.DeserializeObject<List<StationVisit>>(File.ReadAllText(Filename));

                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                lDBCon.TransBegin("ImportVisitedStations");

                sendProgressEvent(new ProgressEventArgs() { Info="import visited stations", CurrentValue=Counter,  TotalValue=History.Count });

                foreach(StationVisit VisitEvent in History)
                {
                    String System  = StructureHelper.CombinedNameToSystemName(VisitEvent.Station);                    
                    String Station = StructureHelper.CombinedNameToStationName(VisitEvent.Station);

                    //Debug.Print(System + "," + Location);

                    try
                    {
                        sqlString = String.Format("insert ignore into tbVisitedSystems(system_id, time)" +
                                                  " SELECT d.* FROM (SELECT" +
                                                  " (select id from tbSystems where systemname = {0}) as system_id," +
                                                  " {1} as time) as d", 
                                                  DBConnector.SQLAString(DBConnector.SQLEscape(System)), 
                                                  DBConnector.SQLDateTime(VisitEvent.Visited));

                        lDBCon.Execute(sqlString);
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

                        lDBCon.Execute(sqlString);
                    }
                    catch (Exception)
                    {
                        //Debug.Print("Error while importing station in history :" + Location);
                    };
                    
                    Counter++;
                    sendProgressEvent(new ProgressEventArgs() { Info="import visited stations", CurrentValue=Counter,  TotalValue=History.Count });
                }

                lDBCon.TransCommit();

                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();
            }
            catch (Exception ex)
            {
                if(lDBCon.TransActive())
                    lDBCon.TransRollback();

                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();

                throw new Exception("Error while importing the history of visited stations", ex);
            }
        }

        /// <summary>
        /// retrieves all stationnames in the system in a array
        /// </summary>
        /// <param name="System"></param>
        /// <returns></returns>
        public string[] getStations(string System)
        {
            String sqlString;
            DataTable Data;
            String[] retValue = new String[0];
            Int32 RowCounter;

            try
            {
                Data = new DataTable();
                
                sqlString = "select St.Stationname from tbSystems Sy, tbStations St" +
                            " where Sy.ID         = St.System_ID" +
                            " and   Sy.Systemname = " + DBConnector.SQLAString(DBConnector.SQLEscape(System));

                Program.DBCon.Execute(sqlString, Data);

                Array.Resize(ref retValue, Data.Rows.Count);
                RowCounter = 0;

                foreach (DataRow currentRow in Data.Rows)
                {
                    retValue[RowCounter] = (String)currentRow["Stationname"];
                    RowCounter++;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting names of stations in a system", ex);
            }
        }

#endregion

#region Commander's Log

        /// <summary>
        /// imports the "Commander's Log" into the database
        /// </summary>
        /// <param name="fileName"></param>
        public Int32 ImportCommandersLog(String Filename)
        {
            DataSet Data;
            String  sqlString;
            Int32   added = 0;
            List<EDSystem> Systems   = new List<EDSystem>();
            List<EDStation> Stations = new List<EDStation>();
            Int32 currentIndex = 0;
            Int32 Counter = 0;
            Dictionary<int, int> changedSystemIDs;
            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);					

            try
            {
                Data = new DataSet();

                Data.ReadXml(Filename);

                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                if(Data.Tables.Contains("CommandersLogEvent"))
                { 
                    sendProgressEvent(new ProgressEventArgs() { Info="import log", CurrentValue=Counter, TotalValue=Data.Tables["CommandersLogEvent"].Rows.Count });

                    foreach(DataRow Event in Data.Tables["CommandersLogEvent"].AsEnumerable())
                    {
                        String   System    = Event["System"].ToString().Trim();
                        Systems.Add(new EDSystem{Name = System});
                    }
                    changedSystemIDs = ImportSystems_Own(ref Systems, true);

                    foreach(DataRow Event in Data.Tables["CommandersLogEvent"].AsEnumerable())
                    {
                        String   Station   = StructureHelper.CombinedNameToStationName((String)Event["Station"]).Trim();
                        Stations.Add(new EDStation{Name = Station, SystemId = Systems[currentIndex].Id});
                        currentIndex++;
                    }
                    ImportStations_Own(Stations, changedSystemIDs, true);

                    foreach(DataRow Event in Data.Tables["CommandersLogEvent"].AsEnumerable())
                    {
                        DateTime EventTime = DateTime.Parse((String)Event["EventDate"], CultureInfo.CurrentUICulture , DateTimeStyles.AssumeUniversal);
                        String   System    = Event["System"].ToString().Trim();
                        String   Station   = StructureHelper.CombinedNameToStationName((String)Event["Station"]).Trim();
                        String   EventType = Event["EventType"].ToString().Trim().Length == 0 ? "Other" : Event["EventType"].ToString().Trim();

                        // add a new log entry
                        sqlString = String.Format("INSERT INTO tbLog(time, system_id, station_id, event_id, commodity_id," +
                                                  "                  cargoaction_id, cargovolume, credits_transaction, credits_total, notes)" +
                                                  " SELECT d.* FROM (SELECT" +
                                                  "          {0} AS time," +
                                                  "          (select id from tbSystems" +
                                                  "              where systemname = {1}" +
                                                  "              order by updated_at limit 1" +
                                                  "          ) AS system_id," +
                                                  "          (select id from tbStations where stationname       = {2} " + 
                                                  "                                     and   system_id         = (select id from tbSystems" + 
                                                  "                                                                 where systemname = {1}" +
                                                  "                                                                 order by updated_at limit 1)" +
                                                  "          ) AS station_id," +
                                                  "          (select id from tbEventType   where eventtype      = {3}) As event_id," +
                                                  "          (select id from tbCommodity   where commodity      = {4} or loccommodity = {4} limit 1) As commodity_id," +
                                                  "          (select id from tbCargoAction where cargoaction    = {5}) AS cargoaction_id," +
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

                        added += lDBCon.Execute(sqlString);

                        if ((added > 0) && ((added % 10) == 0))
                            Debug.Print(added.ToString());

                        Counter++;
                        sendProgressEvent(new ProgressEventArgs() { Info="import log", CurrentValue=Counter, TotalValue=Data.Tables["CommandersLogEvent"].Rows.Count });
                    }

                }

                // reset freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();

                return added;
            }
            catch (Exception ex)
            {
                // reset freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");
                lDBCon.Dispose();

                throw new Exception("Error when importing the Commander's Log ", ex);
            }
        }


#endregion

#region handling of prices


        private static void getLevels(ref int? DemandLevel, ref int? SupplyLevel, Dictionary<String, int?> Levels, Listing StationListing)
        {
            try
            {
                DemandLevel = null;

                if (!String.IsNullOrEmpty(StationListing.DemandLevel))
                    if (!Levels.TryGetValue(StationListing.DemandLevel, out DemandLevel))
                    {
                        dsEliteDB.tblevellocalizationRow[] LocRow = (dsEliteDB.tblevellocalizationRow[])Program.Data.BaseData.tblevellocalization.Select(String.Format("locname = '{0}'", StationListing.DemandLevel));

                        if (LocRow.GetUpperBound(0) >= 0)
                        {
                            DemandLevel = LocRow[0].economylevel_id;
                            Levels.Add(StationListing.DemandLevel, LocRow[0].economylevel_id);
                        }
                    }
                
                    
                SupplyLevel = null;

                if (!String.IsNullOrEmpty(StationListing.SupplyLevel))
                    if (!Levels.TryGetValue(StationListing.SupplyLevel, out SupplyLevel))
                    {
                        dsEliteDB.tblevellocalizationRow[] LocRow = (dsEliteDB.tblevellocalizationRow[])Program.Data.BaseData.tblevellocalization.Select(String.Format("locname = '{0}'", StationListing.SupplyLevel));

                        if (LocRow.GetUpperBound(0) >= 0)
                        {
                            SupplyLevel = LocRow[0].economylevel_id;
                            Levels.Add(StationListing.SupplyLevel, LocRow[0].economylevel_id);
                        }
                    }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while caching levels", ex);
            }
        }

        /// <summary>
        /// Imports the prices from the list of stations. It's clever
        /// firstly to import the stations from the same file.
        /// </summary>
        /// <param name="Stations"></param>
        private void ImportPrices(List<EDStation> Stations, enImportBehaviour importBehaviour, enDataSource dataSource)
        {
            DBConnector lDBCon = new DBConnector(Program.DBCon.ConfigData, true);
            ProgressEventArgs eva;

            try
            { 
                StringBuilder sqlStringB = new StringBuilder();
                String timeFilter = "";
                Int32 Counter;
                Dictionary<Int32, Int32> commodityIDs = new Dictionary<Int32, Int32>();
                List<Listing> missedListings = new List<Listing>();
                Listing[] currentListing;
                Boolean currentListingDone;
                Int32 priceCountTotal = 0;
                Int32 priceCount = 0;
                Int32 SourceID;
                enDataSource initialDataSource = dataSource;
                

                if ((dataSource == enDataSource.fromRN) || (dataSource == enDataSource.fromIBE_OCR))
                    dataSource = enDataSource.fromIBE;

                if(dataSource == enDataSource.fromEDDN_T)
                    dataSource = enDataSource.fromEDDN;

                // for the prices is no transaction necessary, because we're changing
                // only a single table

                // count the prices for messages
                foreach (EDStation Station in Stations)
                    priceCountTotal += Station.Listings.Count();

                Counter = 0;
                sendProgressEvent(new ProgressEventArgs() { Info="updating prices...", AddSeparator=true });

                Boolean AddComma = false;
                int?  DemandLevel = null;
                int?  SupplyLevel = null;

                Dictionary<String, int?> Levels = new Dictionary<String, int?>();

                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                // now add the commodities and prices
                foreach (EDStation Station in Stations)
                {
                    currentListingDone = false;
                    missedListings.Clear();
                    currentListing = Station.Listings;

                    do
                    {
                        commodityIDs.Clear();
                        currentListingDone = true;

                        if ((Station.Id != 0) && (currentListing.Count() > 0))
                        {
                            sqlStringB.Clear();
                            sqlStringB.Append("insert into tbCommodityData(id, station_id, commodity_id, Sell, Buy," +
                                              "Demand, DemandLevel, Supply, SupplyLevel, Sources_id, timestamp) ");

                            foreach (Listing StationListing in currentListing)
                            {
                                // is this commodity already added in this round ? .... 
                                if (!commodityIDs.ContainsKey(StationListing.CommodityId))
                                {
                                    // ... no

                                    if (!String.IsNullOrEmpty(StationListing.DataSource))
                                        SourceID = (Int32)BaseTableNameToID("source", StationListing.DataSource);
                                    else
                                        SourceID = (Int32)dataSource;

                                    if (dataSource <= 0)
                                        throw new Exception("Illegal SourceID for import : " + SourceID);

                                    if (AddComma)
                                        sqlStringB.Append(" union all ");

                                    // cache level-ids
                                    getLevels(ref DemandLevel, ref SupplyLevel, Levels, StationListing);
                            
                                    switch (importBehaviour)
	                                {
                                        case enImportBehaviour.OnlyNewer:
                                            timeFilter = String.Format("SC1.timestamp < {0}) or (SC1.timestamp is null)", DBConnector.SQLDateTime(DateTimeOffset.FromUnixTimeSeconds(StationListing.CollectedAt).DateTime));
                                            break;

                                        case enImportBehaviour.NewerOrEqual:
                                            timeFilter = String.Format("SC1.timestamp <= {0}) or (SC1.timestamp is null)", DBConnector.SQLDateTime(DateTimeOffset.FromUnixTimeSeconds(StationListing.CollectedAt).DateTime));
                                            break;                                                          

                                        case enImportBehaviour.All:
                                            timeFilter = String.Format("SC1.timestamp = SC1.timestamp", DBConnector.SQLDateTime(DateTimeOffset.FromUnixTimeSeconds(StationListing.CollectedAt).DateTime));
                                            break;
	                                }

                                    sqlStringB.Append(String.Format("(select if(SC1.cnt = 0, 0, SC1.id),{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}" +
                                                                    "         from (select ID, station_id, commodity_id, Count(*) As cnt, timestamp from tbCommodityData" +
                                                                    "              where station_id   = {0}" +
                                                                    "              and   commodity_id = {1}) SC1" +
                                                                    "  where ({10})",
                                                                    Station.Id,
                                                                    StationListing.CommodityId,
                                                                    StationListing.SellPrice,
                                                                    StationListing.BuyPrice,
                                                                    StationListing.Demand,
                                                                    DemandLevel.ToNString("null"),
                                                                    StationListing.Supply,
                                                                    SupplyLevel.ToNString("null"),
                                                                    SourceID,
                                                                    DBConnector.SQLDateTime(DateTimeOffset.FromUnixTimeSeconds(StationListing.CollectedAt).DateTime), 
                                                                    timeFilter));

                                    AddComma = true;
                                    commodityIDs.Add(StationListing.CommodityId, 0);
                                }
                                else
                                {
                                    // If we add the same commodity multiple times in one command the database will not recognize
                                    // the doubled price and add both. So we add multiple prices step by step- only the newest 
                                    // price will remain by this way.
                                    currentListingDone = false;
                                    missedListings.Add(StationListing);
                                }

                                priceCount++;
                            }

                            sqlStringB.Append(" on duplicate key update " +
                                              "  Sell        = Values(Sell)" +
                                              ", Buy         = Values(Buy)" +
                                              ", Demand      = Values(Demand)" +
                                              ", DemandLevel = Values(DemandLevel)" +
                                              ", Supply      = Values(Supply)" +
                                              ", SupplyLevel = Values(SupplyLevel)" +
                                              ", Sources_id  = Values(Sources_id)" +
                                              ", timestamp   = Values(timestamp)");

                            lDBCon.Execute(sqlStringB.ToString());
                        }

                        AddComma = false;
                        Counter++;

                        eva = new ProgressEventArgs() { Info="updating prices...", CurrentValue=priceCount, TotalValue=priceCountTotal };
                        if(sendProgressEvent(eva))
                            break;

                        currentListing = missedListings.ToArray();

                        
                    } while (!currentListingDone);

                    if(((initialDataSource == enDataSource.fromIBE) || (initialDataSource == enDataSource.fromEDDN_T)) && 
                        Program.DBCon.getIniValue<Boolean>(frmDataIO.DB_GROUPNAME, "AutoPurgeNotMoreExistingDataDays", true.ToString(), false))
                    {
                        // remove old prices if we got the data from ourself or from trusted eddn senders
                        Program.Data.DeleteNoLongerExistingMarketData(Program.DBCon.getIniValue<Int32>(frmDataIO.DB_GROUPNAME, "PurgeNotMoreExistingDataDays", "30", false), Station.Id);
                    }

                    if(eva.Cancelled)
                        break;
                }

                eva = new ProgressEventArgs() { Info="updating prices...", CurrentValue=priceCount, TotalValue=priceCountTotal, ForceRefresh=true };

                // gettin' some freaky performance
                lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                lDBCon.Dispose();

            }
            catch (Exception ex)
            {
                try{
                    // gettin' some freaky performance
                    lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");
                    lDBCon.Dispose();
                }catch (Exception) { }

                throw new Exception("Error while importing prices", ex);
            }
        }

        /// <summary>
        /// Imports the prices from a file with csv-strings (e.g. the old autosave-file)
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="importBehaviour"></param>
        /// <param name="dataSource"></param>
        /// <param name="importParams">only for control import behaviour from EDDB files, can be null</param>
        /// <returns></returns>
        public Int32 ImportPricesFromCSVFile(String filename, enImportBehaviour importBehaviour, enDataSource dataSource, PriceImportParameters importParams = null)
        {
            try
            {
                List<String> CSV_Strings    = new List<string>();
                ProgressEventArgs eva;

                var reader              = new StreamReader(File.OpenRead(filename));
                Int32 counter           = 0;

                string header = reader.ReadLine();

                sendProgressEvent(new ProgressEventArgs() { Info="reading data from file ...", AddSeparator=true });
              
                if(header.StartsWith("System;Station;Commodity;Sell;Buy;Demand;;Supply"))
                {
                    // old RN format
                    do
                    {
                        CSV_Strings.Add(reader.ReadLine());
                        counter ++;


                        if(sendProgressEvent(new ProgressEventArgs() { Info="reading data from file ...", CurrentValue=counter}))
                            break;

                    } while (!reader.EndOfStream);

                    reader.Close();

                    if(!sendProgressEvent(new ProgressEventArgs() { Info="reading data from file ...", CurrentValue=counter, TotalValue=counter, ForceRefresh = true})) 
                       ImportPricesFromCSVStrings(CSV_Strings.ToArray(), importBehaviour, dataSource);

                }
                else if(header.StartsWith("id,station_id,commodity_id,supply,buy_price,sell_price,demand"))
                {
                    // EDDB format
                    do
                    {
                        CSV_Strings.Add(reader.ReadLine());
                        counter ++;

                        if(sendProgressEvent(new ProgressEventArgs() { Info="reading data from file ...", CurrentValue=counter}))
                            break;

                    } while (!reader.EndOfStream);

                    reader.Close();

                    if(!sendProgressEvent(new ProgressEventArgs() { Info="reading data from file ...", CurrentValue=counter, TotalValue=counter, ForceRefresh = true}))
                        ImportPricesFromEDDBStrings(CSV_Strings.ToArray(), importBehaviour, dataSource, importParams);
                }
                return CSV_Strings.Count();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing self collected price data", ex);
            }
        }

        /// <summary>
        /// Imports the prices from a list of csv-strings
        /// </summary>
        /// <param name="CSV_Strings">data to import</param>
        /// <param name="importBehaviour">filter, which prices to import</param>
        /// <param name="dataSource">if data has no information about the datasource, this setting will count</param>
        /// <returns>a list of converted station data (including correct station ids) </returns>
        public List<EDStation> ImportPricesFromCSVStrings(String[] CSV_Strings, enImportBehaviour importBehaviour, enDataSource dataSource)
        {
            Boolean MissingSystem   = false;
            Boolean MissingStation  = false;
            String currentLanguage;
            DataTable newData;
            List<EDStation> StationData;
            List<EDSystem> SystemData = null;
            List<CsvRow> csvRowList = new List<CsvRow>();
            ProgressEventArgs eva;

            Int32 counter = 0;
            Dictionary<String, String> foundNames = new Dictionary<string,string>();            // quick cache for finding commodity names

            try
            {
                // *****************************************************************
                // START :section for automatically add unknown commodities

                currentLanguage     = Program.DBCon.getIniValue(IBESettingsView.DB_GROUPNAME, "Language", Program.BASE_LANGUAGE, false);
                newData             = new DataTable();
                newData.TableName   = "Names";
                newData.Columns.Add(Program.BASE_LANGUAGE, typeof(String));
                if(currentLanguage != Program.BASE_LANGUAGE)
                    newData.Columns.Add(currentLanguage, typeof(String));

                eva = new ProgressEventArgs() { Info="analysing data...", AddSeparator = true};
                sendProgressEvent(eva);

                for (int i = 0; i < CSV_Strings.Length; i++)
                {
                    String currentName;
                    List<dsEliteDB.tbcommoditylocalizationRow> currentCommodity;
                    if (CSV_Strings[i].Trim().Length > 0)
                    {
                        currentName = new CsvRow(CSV_Strings[i]).CommodityName;
                        if (!String.IsNullOrEmpty(currentName))
                        {
                            // check if we need to remap this name
                            Datasets.dsEliteDB.tbdnmap_commodityRow mappedName = (Datasets.dsEliteDB.tbdnmap_commodityRow)BaseData.tbdnmap_commodity.Rows.Find(new object[] {currentName, ""});
                            if (mappedName != null)
                            {
                                CSV_Strings[i] = CSV_Strings[i].Replace(mappedName.CompanionName, mappedName.GameName);
                                currentName = mappedName.GameName;
                            }

                            if (!foundNames.ContainsKey(currentName))
                            {
                                currentCommodity = Program.Data.BaseData.tbcommoditylocalization.Where(x => x.locname.Equals(currentName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                                if (currentCommodity.Count == 0)
                                {
                                    if (currentLanguage == Program.BASE_LANGUAGE)
                                        newData.Rows.Add(currentName);
                                    else
                                        newData.Rows.Add(currentName, currentName);
                                }
                                foundNames.Add(currentName, "");
                            }
                        }
                    }
                    counter++;

                    eva = new ProgressEventArgs() { Info="analysing data...", CurrentValue=counter, TotalValue=CSV_Strings.GetUpperBound(0) + 1 };
                    sendProgressEvent(eva);
                    if(eva.Cancelled)
                        break;
                }

                eva = new ProgressEventArgs() { Info="analysing data...", CurrentValue=counter, TotalValue=counter, ForceRefresh=true };
                sendProgressEvent(eva);

                if (!eva.Cancelled)
                    if(newData.Rows.Count > 0)
                    {
                        // add found unknown commodities
                        var ds = new DataSet();
                        ds.Tables.Add(newData);
                        ImportCommodityLocalizations(ds);

                        // refresh translation columns
                        Program.Data.updateTranslation();

                        // refresh working tables 
                        Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommoditylocalization.TableName);
                        Program.Data.PrepareBaseTables(Program.Data.BaseData.tbcommodity.TableName);
                    }
                    
                // END : section for automatically add unknown commodities
                // *****************************************************************

                // convert csv-strings to EDStation-objects
                StationData = fromCSV(CSV_Strings, ref SystemData, ref csvRowList);

                // check if we've unknown systems or stations
                if(!eva.Cancelled)
                    foreach (EDStation Station in StationData)
                    {
                        if (Station.SystemId == 0)
                            MissingSystem = true;
                        else if(Station.Id == 0)
                            MissingStation = true;
                    }


                if ((!eva.Cancelled) && MissingSystem)
                {
                    // add unknown systems
                    ImportSystems_Own(ref SystemData, true);
                }

                if (!eva.Cancelled && (MissingSystem || MissingStation))
                {
                    // add unknown stations
                    foreach (EDStation Station in StationData)
                    {
                        // first get all missing system ids
                        if (Station.SystemId == 0)
                        {
                            EDSystem thisSystem = SystemData.FirstOrDefault(x => x.Name == Station.SystemName);

                            if(thisSystem != null)
                            {
                                // got it - set the id
                                Station.SystemId = thisSystem.Id;
                            }
                        }

                    }

                    ImportStations_Own(StationData, new Dictionary<Int32, Int32>(), true);
                }

                // now import the prices
                ImportPrices(StationData, importBehaviour, dataSource);

                if (MissingSystem)
                {
                    // reloading of base tables
                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                }

                if (MissingSystem || MissingStation)
                {
                    // reloading of base tables
                    Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);

                    Program.Data.PrepareBaseTables(Program.Data.BaseData.visystemsandstations.TableName);
                }

                return StationData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing self collected price data", ex);
            }
        }

        /// <summary>
        /// imports prices from a JSON companion data object 
        /// </summary>
        /// <param name="companionData">JSON object with companion data</param>
        /// <returns></returns>
        public Int32 ImportPrices(Newtonsoft.Json.Linq.JObject companionData)
        {
            String system;
            String starPort;
            Int32 commodityCount = 0;
            List<String> csvStrings = new List<string>();
            List<EDStation> stationData = null;

            try
            {
                system   = companionData["lastSystem"]["name"].ToString();
                starPort = companionData["lastStarport"]["name"].ToString();

                foreach (Newtonsoft.Json.Linq.JToken commodity in companionData.SelectTokens("lastStarport.commodities[*]"))
                {                                                  
                    if(!commodity.Value<String>("categoryname").Equals("NonMarketable", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CsvRow csvData = new CsvRow();

                        csvData.SystemName          = system;
                        csvData.StationName         = starPort;
                        csvData.StationID           = String.Format("{0} [{1}]", starPort, system);
                        csvData.CommodityName       = commodity.Value<String>("name");
                        csvData.SellPrice           = commodity.Value<Int32>("sellPrice");
                        csvData.BuyPrice            = commodity.Value<Int32>("buyPrice");
                        csvData.Demand              = commodity.Value<Int32>("demand");
                        csvData.Supply              = commodity.Value<Int32>("stock");
                        csvData.SampleDate          = DateTime.Now;

                        if((!String.IsNullOrEmpty(commodity.Value<String>("demandBracket"))) && (commodity.Value<Int32>("demandBracket") > 0))
                            csvData.DemandLevel         = (String)Program.Data.BaseTableIDToName("economylevel", commodity.Value<Int32>("demandBracket") - 1, "level");
                        else
                            csvData.DemandLevel = null;

                        if((!String.IsNullOrEmpty(commodity.Value<String>("stockBracket"))) && (commodity.Value<Int32>("stockBracket") > 0))
                            csvData.SupplyLevel         = (String)Program.Data.BaseTableIDToName("economylevel", commodity.Value<Int32>("stockBracket") - 1, "level");
                        else
                            csvData.SupplyLevel = null;

                        csvData.SourceFileName      = "";
                        csvData.DataSource          = "";

                        csvStrings.Add(csvData.ToString());

                        commodityCount++;
                    }
                } 

                if(csvStrings.Count > 0)
                    stationData = ImportPricesFromCSVStrings(csvStrings.ToArray(), SQL.EliteDBIO.enImportBehaviour.OnlyNewer, SQL.EliteDBIO.enDataSource.fromIBE);

                return commodityCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing prices from companion interface", ex);
            }
        }

        /// <summary>
        /// Imports the prices from a list of csv-strings in EDDB format
        /// </summary>
        /// <param name="CSV_Strings">data to import</param>
        /// <param name="importBehaviour">filter, which prices to import</param>
        /// <param name="dataSource">if data has no information about the datasource, this setting will count</param>
        public void ImportPricesFromEDDBStrings(String[] CSV_Strings, enImportBehaviour importBehaviour, enDataSource dataSource, PriceImportParameters importParams)
        {
            List<EDStation> StationData;
            Boolean updateTables = false;
            ProgressEventArgs eva=new ProgressEventArgs();
            Int32 initialSize=0;

            try
            {
                StationData = fromCSV_EDDB(CSV_Strings);

                if(importParams != null)
                {
                    DataTable data = Program.Data.GetNeighbourSystems(importParams.SystemID, importParams.Radius);

                    String info = "filter data to the bubble (radius " + importParams.Radius+ " ly) : " + data.Rows.Count +" systems...";
                    eva = new ProgressEventArgs() { Info=info, NewLine=true};      

                    if(!sendProgressEvent(eva))
                    {
                       if(data.Rows.Count > 0)
                       {
                           updateTables = true;

                            initialSize = StationData.Count();

                            for (int i = StationData.Count()-1 ; i >= 0 ; i--)
                           {
                               if(data.Rows.Find(StationData[i].SystemId) == null)    
                               {
                                   // system is not in the bubble
                                   StationData.Remove(StationData[i]);
                               }
                               else
                               { 
                                      // system is in the bubble - set as visited
                                   Program.Data.checkPotentiallyNewSystemOrStation(StationData[i].SystemName, StationData[i].Name, null, true, false);
                               }

                               eva = new ProgressEventArgs() { Info=info, CurrentValue=initialSize-i, TotalValue=initialSize };
                               sendProgressEvent(eva);
                               if(eva.Cancelled)
                                   break;

                           }

                       }
                       else
                           StationData.Clear();
                    }

                    eva = new ProgressEventArgs() { Info=info, CurrentValue=initialSize, TotalValue=initialSize, ForceRefresh=true };
                    sendProgressEvent(eva);
                }

                if((!eva.Cancelled) && (updateTables))
                { 
                    eva = new ProgressEventArgs() { Info = "refreshing basetables in memory...", NewLine=true };
                    sendProgressEvent(eva);

                    if(!eva.Cancelled)
                    {
                        Program.Data.updateVisitedFlagsFromBase();
                        eva = new ProgressEventArgs() { Info = "refreshing basetables in memory...", CurrentValue=25, TotalValue=100, ForceRefresh=true };
                        sendProgressEvent(eva);
                    }
                    if(!eva.Cancelled)
                    {
                        Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);
                        eva = new ProgressEventArgs() { Info = "refreshing basetables in memory...", CurrentValue=50, TotalValue=100, ForceRefresh=true };
                        sendProgressEvent(eva);
                    }
                    if(!eva.Cancelled)
                    {
                        Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                        eva = new ProgressEventArgs() { Info = "refreshing basetables in memory...", CurrentValue=75, TotalValue=100, ForceRefresh=true };
                        sendProgressEvent(eva);
                    }
                    if(!eva.Cancelled)
                    {
                        Program.Data.PrepareBaseTables(Program.Data.BaseData.visystemsandstations.TableName);
                        eva = new ProgressEventArgs() { Info = "refreshing basetables in memory...", CurrentValue=100, TotalValue=100, ForceRefresh=true };
                        sendProgressEvent(eva);
                    }
                }

                // now import the prices
                if(!eva.Cancelled)
                { 
                   ImportPrices(StationData, importBehaviour, dataSource);
                }



            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing self collected price data", ex);
            }
        }


        /// <summary>
        /// creates a list of "EDStations" with price listings from csv-array
        /// </summary>
        /// <param name="CSV_Strings">String to be converted</param>
        /// <param name="foundSystems"></param>
        /// <param name="csvRowList">for optional processing outside: a list of the data converted to CsvRow-objects</param>
        /// <returns></returns>
        public List<EDStation> fromCSV(String[] CSV_Strings, ref List<EDSystem> foundSystems, ref List<CsvRow> csvRowList)
        {
            List<EDStation> foundValues                     = new List<EDStation>();
            Dictionary<String, Int32> foundIndex            = new Dictionary<String, Int32>();
            Dictionary<String, Int32> foundSystemIndex      = new Dictionary<String, Int32>();
            String LastID                                   = "";
            EDSystem LastSystem                             = null;
            String currentID                                = "";
            EDStation currentStation                        = null;
            Int32 Index                                     = 0;
            Dictionary<String, Int32> commodityIDCache      = new Dictionary<string,Int32>();            // quick cache for finding commodity names
            Int32 currentItem                               = 0;
            ProgressEventArgs eva;

            try
            {
                eva = new ProgressEventArgs() { Info="converting data...", CurrentValue=currentItem, TotalValue=CSV_Strings.GetUpperBound(0)+1, AddSeparator = true };
                sendProgressEvent(eva);

                if(foundSystems != null)
                    foundSystems.Clear();
                else
                    foundSystems = new List<EDSystem>();


                foreach (String CSV_String in CSV_Strings)
	            {

                    if(!String.IsNullOrEmpty(CSV_String.Trim()))
                    {
		                CsvRow currentRow           = new CsvRow(CSV_String);

                        if(csvRowList != null)
                            csvRowList.Add(currentRow);

                        currentID = currentRow.StationID;

                        if(!LastID.Equals(currentID, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if(currentStation != null)
                                currentStation.ListingExtendMode = false;

                            if(foundIndex.TryGetValue(currentID, out Index))
                                currentStation = foundValues[Index];
                            else
                            {
                                currentStation  = new EDStation(currentRow);

                                foundValues.Add(currentStation);
                                foundIndex.Add(currentID, foundValues.Count-1);
                            }
                            LastID = currentRow.StationID;

                            currentStation.ListingExtendMode = true;


                            if((LastSystem == null) || (!LastSystem.Name.Equals(currentRow.SystemName, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                if(foundSystemIndex.TryGetValue(currentRow.SystemName, out Index))
                                    LastSystem = foundSystems[Index];
                                else
                                {
                                    LastSystem  = new EDSystem();
                                    LastSystem.Name = currentRow.SystemName;

                                    if(LastSystem.Id == 0)
                                        LastSystem.Id = currentStation.SystemId;


                                    foundSystems.Add(LastSystem);
                                    foundSystemIndex.Add(currentRow.SystemName, foundSystems.Count-1);
                                }
                            }
                        }

                        currentStation.addListing(currentRow, ref commodityIDCache);
                    }

                    eva = new ProgressEventArgs() { Info="converting data...", CurrentValue=currentItem, TotalValue=CSV_Strings.GetUpperBound(0)+1};
                    sendProgressEvent(eva);

                    if(eva.Cancelled)
                        break;

                    currentItem++;

	            }

                if(currentStation != null)
                    currentStation.ListingExtendMode = false;

                eva = new ProgressEventArgs() { Info="converting data...", CurrentValue=currentItem, TotalValue=CSV_Strings.GetUpperBound(0)+1, ForceRefresh=true};
                sendProgressEvent(eva);

                return foundValues;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting station values from CSV-String", ex);
            }
        }

        /// <summary>
        /// creates a list of "EDStations" with price listings from csv-array in EDDB format
        /// </summary>
        /// <param name="CSV_Strings">String to be converted</param>
        /// <param name="foundSystems"></param>
        /// <param name="csvRowList">for optional processing outside: a list of the data converted to CsvRow-objects</param>
        /// <returns></returns>
        public List<EDStation> fromCSV_EDDB(String[] CSV_Strings)
        {
            List<EDStation> foundValues                     = new List<EDStation>();
            Dictionary<Int32, Int32> foundIndex             = new Dictionary<Int32, Int32>();
            Dictionary<String, Int32> foundSystemIndex      = new Dictionary<String, Int32>();
            Int32 LastID                                    = 0;
            EDSystem LastSystem                             = null;
            Int32 currentID                                 = 0;
            EDStation currentStation                        = null;
            Int32 Index                                     = 0;
            Dictionary<String, Int32> commodityIDCache      = new Dictionary<string,Int32>();            // quick cache for finding commodity names
            Int32 currentItem                               = 0;
            ProgressEventArgs eva;

            try
            {
                eva = new ProgressEventArgs() { Info="converting data...", CurrentValue=currentItem, TotalValue=CSV_Strings.GetUpperBound(0), NewLine= true};
                sendProgressEvent(eva);

                foreach (String CSV_String in CSV_Strings)
	            {

                    if(!String.IsNullOrEmpty(CSV_String.Trim()))
                    {
		                Listing currentRow           = new Listing(CSV_String);

                        currentID = currentRow.StationId;

                        if(LastID != currentID)
                        {
                            if(currentStation != null)
                                currentStation.ListingExtendMode = false;

                            if(foundIndex.TryGetValue(currentID, out Index))
                                currentStation = foundValues[Index];
                            else
                            {
                                currentStation  = new EDStation(currentRow);

                                foundValues.Add(currentStation);
                                foundIndex.Add(currentID, foundValues.Count-1);
                            }
                            LastID = currentRow.StationId;

                            currentStation.ListingExtendMode = true;

                        }

                        currentStation.addListing(currentRow);
                    }
                
                    eva = new ProgressEventArgs() { Info="converting data...", CurrentValue=currentItem, TotalValue=CSV_Strings.GetUpperBound(0)};
                    sendProgressEvent(eva);

                    if(eva.Cancelled)
                        break;

                    currentItem++;
	            }

                if(currentStation != null)
                    currentStation.ListingExtendMode = false;

                eva = new ProgressEventArgs() { Info="converting data...", CurrentValue=currentItem, TotalValue=CSV_Strings.GetUpperBound(0), ForceRefresh=true};
                sendProgressEvent(eva);

                return foundValues;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting station values from CSV-String", ex);
            }
        }

        /// <summary>
        /// function deletes all exept the newest price for a commodity
        /// on a station, if there exists more than one.
        /// </summary>
        /// <param name="idList">if not null the function only examines the given commodity-ids
        /// (for improving performance)</param>
        public void DeleteMultiplePrices(List<Int32> idList = null)
        {
            String sqlString;
            DataSet data = new DataSet();
            Int32 deletedPricesByCommodities = 0;
            Int32 deletedPrices = 0;
            String idString = "";

            try 
	        {	        
                if ((idList!= null) && (idList.Count > 0))
                {
                    foreach (Int32 id in idList)
                    {
                        if(idString.Length > 0)    
                            idString += " or ";

                        idString += String.Format("commodity_id = {0}", 
                                                     id.ToString());
                    }

                    idString = " where (" + idString + ")";
                }

		        sqlString = "select C2.station_id, C2.commodity_id, count(*) as cnt" +
		                    " from tbCommodityData C2" +
                            idString +
		                    " group by C2.station_id, C2.commodity_id" +
		                    " having cnt > 1";
                Program.DBCon.Execute(sqlString, "tbFoundData", data);

                if (data.Tables["tbFoundData"].Rows.Count > 0)
                {
                    sqlString = "";
                    foreach (DataRow foundData in data.Tables["tbFoundData"].Rows)
                    {
                        Int32 counter = 0;
                        sqlString = String.Format("select id from tbCommodityData" +
                                                  " where station_id   = {0}" + 
                                                  " and   commodity_id = {1}" + 
                                                  " order by timestamp desc",
                                                  foundData[0].ToString(), foundData[1].ToString());

                        Program.DBCon.Execute(sqlString, "tbDeleteData", data);
                        
                        foreach (DataRow deleteItem in data.Tables["tbDeleteData"].Rows)
	                    {
                            // all but the first (=newest) price
		                    if (counter > 0)
                            {
                                sqlString = String.Format("delete from tbCommodityData" +
                                                          " where id = {0}", 
                                                          deleteItem["id"].ToString());
                        
                                Program.DBCon.Execute(sqlString, "tbDeleteData", data);
                                deletedPrices++;
                            }
                            counter++;
	                    }

                        deletedPricesByCommodities++;
                    }
                }
	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while deleting mulitple prices", ex);
	        }
        }

        /// <summary>
        /// exports the market data to a file
        /// </summary>
        /// <param name="fileName"></param>
        public void ExportMarketDataToCSV(string fileName, Boolean inCurrentLanguage, Boolean extendedFormat)
        {
            String sqlString;
            DataTable data;
            Int32 Counter;
            StringBuilder sBuilder = new StringBuilder();
            Char filterCharacter=(char)(48);
            Int32 totalDataCount = 0;
            ProgressEventArgs eva;

            try
            {

                if(System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                var writer = new StreamWriter(File.OpenWrite(fileName));

                if(extendedFormat)
                    writer.WriteLine("System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;SourceFileName;Source");
                else
                    writer.WriteLine("System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;");

                data        = new DataTable();
                Counter     = 0;
                    
                totalDataCount = Program.DBCon.Execute<Int32>("select count(*) from tbCommodityData;");

                for (int i = 0; i < 36; i++)
                {
                    if(i < 10)
                        filterCharacter = (char)(48 + i);
                    else
                        filterCharacter = (char)(65 + i - 10);

                    if (inCurrentLanguage)
                    {
                        // export names in user language
                        sqlString = String.Format("select Sy.systemname, St.stationname, C.loccommodity as commodity, D.sell, D.buy, D.demand, D.demandlevel, D.supply, D.supplylevel, D.timestamp, S.source" +
                                                  " from tbSystems Sy, tbStations St, tbCommodityData D, tbCommodity C, tbSource S" +
                                                  " where Sy.id           = St.system_id" + 
                                                  " and   St.id           = D.station_id" +
                                                  " and   D.commodity_id  = C.id" +
                                                  " and   D.sources_id    = S.id" +
                                                  " and   Sy.systemname like '{0}%'" +
                                                  " order by Sy.systemname, St.stationname, C.loccommodity",
                                                  filterCharacter); 
                    }
                    else
                    {
                        // export names in default language (english)
                        sqlString = String.Format("select Sy.systemname, St.stationname, C.commodity, D.sell, D.buy, D.demand, D.demandlevel, D.supply, D.supplylevel, D.timestamp, S.source" +
                                                  " from tbSystems Sy, tbStations St, tbCommodityData D, tbCommodity C, tbSource S" +
                                                  " where Sy.id           = St.system_id" + 
                                                  " and   St.id           = D.station_id" +
                                                  " and   D.commodity_id  = C.id" +
                                                  " and   D.sources_id    = S.id" +
                                                  " and   Sy.systemname like '{0}%'" +
                                                  " order by Sy.systemname, St.stationname, C.commodity",
                                                  filterCharacter); 
                    }

                    eva = new ProgressEventArgs() { Info=String.Format("collecting data '{0}'...", filterCharacter), CurrentValue=Counter, TotalValue=totalDataCount, NewLine=true};
                    sendProgressEvent(eva);

                    Program.DBCon.Execute(sqlString, data);

                    foreach (DataRow row in data.Rows)
                    {
                        String Demand = "";
                        String Supply = "";

                        if (inCurrentLanguage)
                        {
                            Demand =  ((String)BaseTableIDToName("economylevel", SQL.DBConvert.To<int?>(row["demandlevel"]), "loclevel")).NToString("");
                            Supply =  ((String)BaseTableIDToName("economylevel", SQL.DBConvert.To<int?>(row["supplylevel"]), "loclevel")).NToString("");
                        }
                        else
                        {
                            Demand =  ((String)BaseTableIDToName("economylevel", SQL.DBConvert.To<int?>(row["demandlevel"]), "level")).NToString("");
                            Supply =  ((String)BaseTableIDToName("economylevel", SQL.DBConvert.To<int?>(row["supplylevel"]), "level")).NToString("");
                        }

                        sBuilder.Append(row["systemname"] + ";" + 
                                        row["stationname"] + ";" + 
                                        row["commodity"] + ";" + 
                                        row["sell"] + ";" + 
                                        row["buy"] + ";" + 
                                        row["demand"] + ";" + 
                                        Demand + ";" + 
                                        row["supply"] + ";" + 
                                        Supply + ";" + 
                                        row["timestamp"]);

                        if(extendedFormat)
                        {

                            sBuilder.Append(";;" +              //sourceFileName, no more in use but for compatibility
                                           row["source"]);      //source
                        }

                        writer.WriteLine(sBuilder.ToString());

                        sBuilder.Clear();

                        Counter++;

                        eva = new ProgressEventArgs() { Info=String.Format("export prices '{0}'...", filterCharacter), CurrentValue=Counter, TotalValue=totalDataCount};
                        sendProgressEvent(eva);
                        if(eva.Cancelled)
                            break;
                    }

                    eva = new ProgressEventArgs() { Info=String.Format("export prices '{0}'...", filterCharacter), CurrentValue=Counter, TotalValue=totalDataCount, ForceRefresh=true};
                    sendProgressEvent(eva);
                    if(eva.Cancelled)
                        break;

                    if(eva.Cancelled)
                        break;

                }

                eva = new ProgressEventArgs() { Info=String.Format("export prices '{0}'...", filterCharacter), CurrentValue=Counter, TotalValue=totalDataCount, ForceRefresh=true};
                sendProgressEvent(eva);

                writer.Close();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while exporting to csv file", ex);
            }
        }

        /// <summary>
        /// delete all market data older than x days
        /// </summary>
        /// <param name="value"></param>
        public void DeleteMarketData(Int32 days)
        {
            String sqlString;
            Int32 deletedRows = 0;
            Int32 deletedRowsSum = 0;
            Int32 rowsToDelete;

            sqlString = String.Format("select count(*) from tbcommoditydata" + 
                                      " where timestamp <= (NOW() - INTERVAL {0} DAY)", 
                                      days);

            rowsToDelete = Program.DBCon.Execute<Int32>(sqlString);    

            sqlString = String.Format("delete from tbcommoditydata" + 
                                      " where timestamp <= (NOW() - INTERVAL {0} DAY)" +
                                      " limit 10000", 
                                      days);
            sendProgressEvent(new ProgressEventArgs() { Info=String.Format("deleting prices older than {0} days from database...", days), CurrentValue=0, TotalValue=rowsToDelete, AddSeparator=true });

            do
            {
                deletedRows = Program.DBCon.Execute(sqlString);    
                deletedRowsSum += deletedRows;

                if(sendProgressEvent(new ProgressEventArgs() { Info=String.Format("deleting prices older than {0} days from database...", days), CurrentValue=deletedRowsSum, TotalValue = rowsToDelete }))
                    break;
                        

            } while (deletedRows > 0);

            sendProgressEvent(new ProgressEventArgs() { Info=String.Format("deleting prices older than {0} days from database...", days), CurrentValue=deletedRowsSum, TotalValue = rowsToDelete, ForceRefresh=true });
        }


        /// <summary>
        /// delete all data for every station, older than "youngest update" - "n days"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void DeleteNoLongerExistingMarketData(int days, int? singleStationID=null)
        {
            String sqlString;
            Int32 deletedRows = 0;
            Int32 deletedRowsSum = 0;
            Int32 stationsToCheck;
            String sqlBaseString_Get;
            String sqlBaseString_Delete;
            DataTable data = new DataTable();
            Int32 firstPosition = 0;
            Int32 quantity      = 500;
            Int32 deleted = 0;
            Int32 checkedStations = 0;

            try
            {
                sqlBaseString_Get = " select * from" + 
                                    " 				(select * from tbStations) L1" + 
                                    " 	left join " +
                                    " 				(select id " +
                                    " 				from " +
                                    " 					(select * from tbStations) L3 " +
                                    " 				order by L3.id asc" +
                                    " 				limit {0}) L2 " +
                                    " on L1.id = L2.id " +
                                    " where L2.id is null " +
                                    " order by L1.id asc" +
                                    " limit {1}";

                sqlBaseString_Delete =  " delete C2 from tbCommodityData C2 " +
                                        "  where C2.station_id = {0}  " +
                                        "  and   C2.timestamp <  " +
                                        "      (select * from " +
                                        "           (select Date_Add(max(timestamp), INTERVAL -{1} DAY)  " +
                                        "             from tbCommodityData where station_id = {0} " +
                                        "           ) D1  " +
                                        " 	   ) ";


                if(!singleStationID.HasValue)
                    stationsToCheck = Program.DBCon.Execute<Int32>("select count(*) from tbstations"); 
                else
                    stationsToCheck = 1;   

                sqlString = String.Format("select id from tbStations",days);
                sendProgressEvent(new ProgressEventArgs() { Info=String.Format("delete prices of no longer existing commodities on stations...", days), CurrentValue=0, TotalValue=stationsToCheck, AddSeparator=true });

                do
                {
                    if(singleStationID.HasValue)
                    {
                        deleted += Program.DBCon.Execute(String.Format(sqlBaseString_Delete, singleStationID.Value, days));
                    }
                    else
                    {
                        Program.DBCon.Execute(String.Format(sqlBaseString_Get, firstPosition, quantity), data);
                
                        foreach (DataRow stationID in data.Rows)
                        {
                            deleted += Program.DBCon.Execute(String.Format(sqlBaseString_Delete, stationID["id"], days));

                            checkedStations++;

                            if(sendProgressEvent(new ProgressEventArgs() { Info=String.Format("delete prices of no longer existing commodities on stations - deleted prices={0}...", deleted), CurrentValue=checkedStations, TotalValue = stationsToCheck, Unit=" stations" }))
                                break;

                        }

                        firstPosition+=quantity;
                    }

                } while ((data.Rows.Count > 0) && (!singleStationID.HasValue));

                sendProgressEvent(new ProgressEventArgs() { Info=String.Format("delete prices of no longer existing commodities on stations - deleted prices={0}...", deleted), CurrentValue=checkedStations, TotalValue = stationsToCheck, Unit=" stations", ForceRefresh=true });

            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting no more existing commodities from stations", ex);
            }
        }

#endregion

#region localization

        /// <summary>
        /// updates the localization of all commodities for the current language
        /// </summary>
        public void updateTranslation()
        {
            try
            {

                String currentLanguage = Program.DBCon.getIniValue(IBESettingsView.DB_GROUPNAME, "Language", Program.BASE_LANGUAGE, false);

                switchLanguage(currentLanguage);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating the current translation", ex);
            }
        }

        /// <summary>
        /// Checks the commodity names with reference to table tbDNMap_Commodity.
        /// Correct misspellings if some found.
        /// </summary>
        public Int32 CorrectMisspelledCommodities()
        {
            String sqlString;
            String sqlBaseString;
            DataTable data = new DataTable();
            Int32 found = 0;


            try
            {
                sqlBaseString = "select c1.id as ID, c2.ID as WrongID" +
                                "  from" +
                                "    (select id from tbcommodity where commodity = {0}) c1" +
                                "  join" +
                                "    (select id from tbcommodity where commodity = {1}) c2;";

                foreach (dsEliteDB.tbdnmap_commodityRow mapping in BaseData.tbdnmap_commodity.Rows)
	            {
                    Program.DBCon.Execute(String.Format(sqlBaseString, DBConnector.SQLAEscape(mapping.GameName), DBConnector.SQLAEscape(mapping.CompanionName)), data);

                    foreach (DataRow wrongSpellings in data.Rows)
	                {
                        if(((Int32)wrongSpellings["ID"]) != ((Int32)wrongSpellings["WrongID"]) && (((Int32)wrongSpellings["WrongID"]) < 0))
                        { 
                            Program.SplashScreen.InfoAdd(String.Format("...alter '{0}' to '{1}'...", mapping.CompanionName, mapping.GameName));
                            Program.DBCon.TransBegin();

                            // change the collected data to the new id
                            sqlString = String.Format("update tbCommodityData" +
                                                        " set   commodity_id = {1}" +
                                                        " where commodity_id = {0}", 
                                                        wrongSpellings["WrongID"], 
                                                        wrongSpellings["ID"]);
                            Program.DBCon.Execute(sqlString);

                            sqlString = String.Format("update tbPriceHistory" +
                                                        " set   commodity_id = {1}" +
                                                        " where commodity_id = {0}", 
                                                        wrongSpellings["WrongID"], 
                                                        wrongSpellings["ID"]);
                            Program.DBCon.Execute(sqlString);

                            // delete entry from tbCommodity, the ForeigenKeys will delete the 
                            // entries from the other affected tables
                            // entries in table "tbCommodityClassification" can be deleted
                            sqlString = String.Format("delete from tbCommodity" +
                                                        " where id = {0}", 
                                                        wrongSpellings["WrongID"]);
                            Program.DBCon.Execute(sqlString);

                            Program.Data.DeleteMultiplePrices(new List<Int32>() {(Int32)wrongSpellings["ID"]});

                            Program.DBCon.TransCommit();

                            Program.SplashScreen.InfoAppendLast("OK");
                            found++;
                        }
	                }
	            }

                Program.Data.AddMissingLocalizationEntries();
                Program.Data.updateTranslation();

                return found;
            }
            catch (Exception ex)
            {
                if(Program.DBCon.TransActive())
                    Program.DBCon.TransRollback();

                throw new Exception("Error while saving data", ex);
            }
        }


#endregion

#region general

        /// <summary>
        /// Specifies all systems/stations from the Commander's Log as "visited".
        /// Necessary/recommended after a import of old "Commander's Log" files.
        /// </summary>
        /// <param name="Refresh"></param>
        public void updateVisitedBaseFromLog(enVisitType Refresh)
        {
            String sqlString;
            
            try
            {
                if((Refresh & enVisitType.Systems) > 0)
                { 
                    sqlString = "insert into tbVisitedSystems(system_id, time)" +
                                "   select L1.system_id, L1.time " +
                                "      from tbLog L1 left join tbVisitedSystems V1 on L1.system_id = V1.system_id " +
                                "      where L1.system_id is not null " +
                                "      and   L1.time      > V1.time" +
                                "      and  not exists (select * from tbLog L2 " +
                                "                        where L1.system_id = L2.system_id" +
                                "                        and   L1.time      < L2.time)" +
                                "on duplicate key update time = L1.time";

                    Program.DBCon.Execute(sqlString);
                }

                if((Refresh & enVisitType.Stations) > 0)
                { 
                    sqlString = "insert into tbVisitedStations(station_id, time)" +
                                "   select L1.station_id, L1.time " +
                                "      from tbLog L1 left join tbVisitedStations V1 on L1.station_id = V1.station_id " +
                                "      where L1.station_id is not null " +
                                "      and   L1.time      > V1.time" +
                                "      and  not exists (select * from tbLog L2 " +
                                "                        where L1.station_id = L2.station_id" +
                                "                        and   L1.time      < L2.time)" +
                                " on duplicate key update time = L1.time";

                    Program.DBCon.Execute(sqlString);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating the visited systems from log", ex);
            }
        }

        /// <summary>
        /// Update the visited-flag in the stations- and systems-tables
        /// from the visited-basetables (tbVisitedStations/tbVistitedSystems).
        /// </summary>
        /// <param name="Refresh"></param>
        public void updateVisitedFlagsFromBase(Boolean newSystem = true, Boolean newStation = true)
        {
            String sqlString;
            
            try
            {
                if(newSystem)
                { 
                    sqlString = "update tbSystems S left join tbVisitedSystems V on S.id = V.system_id" +
                                "   set visited = if(V.system_id is null, 0, 1)";
                    Program.DBCon.Execute(sqlString);
                }

                if(newStation)
                { 
                    sqlString = "update tbStations S left join tbVisitedStations V on S.id = V.station_id" +
                                "   set visited = if(V.station_id is null, 0, 1)";
                    Program.DBCon.Execute(sqlString);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating the visited systems from log", ex);
            }
        }

        /// <summary>
        /// Switch the language, updating the loccommodity-string in tbCommodities.
        /// </summary>
        /// <param name="Language">new language for viewing</param>
        public void switchLanguage(String Language)
        {
            String sqlString;
            
            try
            {
                // commodities
                sqlString = String.Format(
                            "update tbcommodity C, (select C1.ID, if(Loc1.LocName is null, C1.Commodity, Loc1.LocName) As loccommodity" + 
                            "                          from tbCommodity C1 left join  (" +
                            "                               select * from tbCommodityLocalization L2, tbLanguage La2" +
							"				                 where L2.Language_id = La2.id" +
							"				                 and La2.language     = {0}) Loc1" + 
							"	                        on C1.id = loc1.Commodity_id) Loc" +
							" set C.loccommodity = Loc.loccommodity" +
							" where C.id = Loc.ID", DBConnector.SQLAString(Language)); 

                Program.DBCon.Execute(sqlString);

                // categories
                sqlString = String.Format(
                            "update tbCategory C, (select C1.ID, if(Loc1.LocName is null, C1.Category, Loc1.LocName) As locCategory" + 
                            "                          from tbCategory C1 left join  (" +
                            "                               select * from tbCategoryLocalization L2, tbLanguage La2" +
							"				                 where L2.Language_id = La2.id" +
							"				                 and La2.language     = {0}) Loc1" + 
							"	                        on C1.id = loc1.Category_id) Loc" +
							" set C.locCategory = Loc.locCategory" +
							" where C.id = Loc.ID", DBConnector.SQLAString(Language)); 

                Program.DBCon.Execute(sqlString);

                // economy levels
                sqlString = String.Format(
                            "update tbEconomyLevel C, (select C1.ID, if(Loc1.LocName is null, C1.Level, Loc1.LocName) As locEconomyLevel" +
                            "                              from tbEconomyLevel C1 left join  (" +
                            "                                   select * from tbLevelLocalization L2, tbLanguage La2" +
                            "                                    where L2.Language_id = La2.id" +
                            "                                    and   La2.language   = {0}) Loc1" +
                            "                               on C1.id = loc1.EconomyLevel_id) Loc " +
                            " set C.locLevel = Loc.locEconomyLevel " +
                            " where C.id = Loc.ID", DBConnector.SQLAString(Language)); 

                Program.DBCon.Execute(sqlString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while switching the language to <" + Language + ">", ex);
            }
        }

        /// <summary>
        /// get the next free index for the table (positive only)
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private UInt32 getFreeIndex(String TableName)
        {
            String sqlString ;
            UInt32 retValue = 0;
            DataTable data  = new DataTable();

            try
            {
                // get the highest ID from the table
                sqlString = "select max(id) As max_id from " + TableName;
                Program.DBCon.Execute(sqlString, data);

                if (Convert.IsDBNull(data.Rows[0]["max_id"]))
                    retValue = 0;
                else
                {
                    retValue = ((UInt32)data.Rows[0]["max_id"]) + 1;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while determine the next free id", ex);
            }
        }

        internal void ClearAll()
        {
            try
            {
                Int32 Counter = 0;
                List<String> Tables = new List<String>() {"tbVisitedStations", 
                                                          "tbVisitedSystems", 
                                                          "tbLog", 
                                                          "tbCommodityData", 
                                                          "tbPriceHistory", 
                                                          "tbCommodityClassification", 
                                                          "tbCommodityLocalization", 
                                                          "tbCategoryLocalization", 
                                                          "tbLevelLocalization", 
                                                          "tbCommodity", 
                                                          "tbCategory", 
                                                          "tbStations_org", 
                                                          "tbSystems_org", 
                                                          "tbStations", 
                                                          "tbSystems"};

           
                sendProgressEvent(new ProgressEventArgs() { Info="clearing database...", CurrentValue=0, TotalValue=0, AddSeparator=true });

                foreach (String Table in Tables)       
	            {
                    Debug.Print("deleting " + Table + "...");
                    sendProgressEvent(new ProgressEventArgs() { Info="deleting " + Table + "...", CurrentValue=Counter + 1, TotalValue=Tables.Count, ForceRefresh=true });

		            Program.DBCon.Execute("delete from " + Table);

                    Counter++;
        	    }
                

                sendProgressEvent(new ProgressEventArgs() { Info="clearing database...<done>", CurrentValue=1, TotalValue=1, ForceRefresh=true });

            }
            catch (Exception ex)
            {
                throw new Exception("Error while clearing the database", ex);
            }
        }

        /// <summary>
        /// Checks if the system is existing and adds it, if not.
        /// Also sets the visited-flag if not set.
        /// </summary>
        /// <param name="System"></param>
        /// <param name="Station"></param>
        /// <param name="setVisitedFlag"></param>
        /// <param name="name"></param>
        public void checkPotentiallyNewSystemOrStation(String System, String Station, Point3Dbl coordinates = null, Boolean setVisitedFlag = true, Boolean refreshBasetables = true)
        {
            String sqlString;
            Int32 SystemID;
            Int32 LocationID;
            Boolean systemFirstTimeVisited = false;
            Boolean stationFirstTimeVisited = false;
            Boolean isNewSystem = false;
            Boolean isNewStation = false;
            DataTable Data = new DataTable();
            Boolean Visited;

            try
            {
                System = System.Trim();
                Station = Station.Trim();

                if (!String.IsNullOrEmpty(System))
                {
                    sqlString = "select id, visited from tbSystems where Systemname = " + DBConnector.SQLAEscape(System);
                    if (Program.DBCon.Execute(sqlString, Data) > 0)
                    {
                        // check or update the visited-flag
                        SystemID = (Int32)(Data.Rows[0]["ID"]);
                        Visited = (Boolean)(Data.Rows[0]["visited"]);

                        if (!Visited)
                        {
                            sqlString = String.Format("insert ignore into tbVisitedSystems(system_id, time) values" +
                                                      " ({0},{1})", SystemID.ToString(), DBConnector.SQLDateTime(DateTime.UtcNow));
                            Program.DBCon.Execute(sqlString);
                            systemFirstTimeVisited = true;
                        }
                    }
                    else
                    {
                        // add a new system
                        EDSystem newSystem = new EDSystem();
                        newSystem.Name = System;

                        var systemIDs = ImportSystems_Own(newSystem, true, setVisitedFlag);

                        SystemID = newSystem.Id;

                        isNewSystem = true;
                        systemFirstTimeVisited = true;
                    }

                    if((coordinates != null) && (coordinates.Valid))
                    {
                        sqlString = String.Format("update tbSystems set x={0}, y={1}, z={2}, updated_at = now()" +
                                                  " where (x<>{0} or y<>{1} or z<>{2}) and id = {3}", 
                                                  DBConnector.SQLDecimal(coordinates.X.Value), 
                                                  DBConnector.SQLDecimal(coordinates.Y.Value), 
                                                  DBConnector.SQLDecimal(coordinates.Z.Value), 
                                                  SystemID);
                        if(Program.DBCon.Execute(sqlString)>0)
                            Debug.Print("system coordinates updated");
                    }


                    if (!String.IsNullOrEmpty(Station))
                    {
                        Data.Clear();

                        sqlString = "select St.ID, St.visited from tbSystems Sy, tbStations St" +
                                       " where Sy.ID = St. System_ID" +
                                       " and   Sy.ID          = " + SystemID +
                                       " and   St.Stationname = " + DBConnector.SQLAEscape(Station);

                        if (Program.DBCon.Execute(sqlString, Data) > 0)
                        {
                            // check or update the visited-flag
                            LocationID = (Int32)(Data.Rows[0]["ID"]);
                            Visited = (Boolean)(Data.Rows[0]["visited"]);

                            if (!Visited)
                            {
                                sqlString = String.Format("insert ignore into tbVisitedStations(station_id, time) values" +
                                                          " ({0},{1})", LocationID.ToString(), DBConnector.SQLDateTime(DateTime.UtcNow));
                                Program.DBCon.Execute(sqlString);
                                stationFirstTimeVisited = true;
                            }
                        }
                        else
                        {
                            // add a new station
                            EDStation newStation = new EDStation();
                            newStation.Name = Station;
                            newStation.SystemId = SystemID;

                            ImportStations_Own(newStation, true, setVisitedFlag);

                            isNewStation = true;
                            stationFirstTimeVisited = true;
                        }
                    }


                    if (refreshBasetables)
                    {
                        if (systemFirstTimeVisited || stationFirstTimeVisited)
                        {
                            // if there's a new visitedflag set in the visited-tables
                            // then update the maintables
                            Program.Data.updateVisitedFlagsFromBase(systemFirstTimeVisited, stationFirstTimeVisited);

                            // last but not least reload the BaseTables with the new visited-information
                            if (systemFirstTimeVisited)
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);

                            if (stationFirstTimeVisited)
                                Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                        }

                        if (isNewSystem || isNewStation)
                        {
                            Program.Data.PrepareBaseTables(Program.Data.BaseData.visystemsandstations.TableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting a visited flag", ex);
            }
        }

        /// <summary>
        /// retrieves all known economylevels as original string in capitals and in the current language
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> getEconomyLevels()
        {
            String sqlString;
            DataTable Data;
            Dictionary<string, string> retValue = new  Dictionary<string, string>();

            try
            {
                Data = new DataTable();
                
                sqlString = "select Upper(level) as level, loclevel from tbEconomyLevel";
                Program.DBCon.Execute(sqlString, Data);

                foreach (DataRow currentRow in Data.Rows)
                    retValue.Add((String)currentRow["level"], (String)currentRow["loclevel"]);

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting dictionary of all economylevels", ex);
            }
        }

        /// <summary>
        /// recalculates distances in the tbLog-table
        /// </summary>
        /// <param name="startTime">considers only entries, if they are younger or equal than ..</param>
        internal void RecalcJumpDistancesInLog(DateTime maxAge)
        {
            try
            {
                RecalcJumpDistancesInLog(maxAge, DateTime.Now);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while recalcing jump distances with 'maxage'", ex);
            }
        }

        /// <summary>
        /// recalculates distances in the tbLog-table
        /// </summary>
        /// <param name="startTime">considers only entries, if they are younger or equal than ..</param>
        /// <param name="endTime">considers only entries, if they are older or equal than ..</param>
        internal void RecalcJumpDistancesInLog(DateTime startTime, DateTime endTime)
        {
            String sqlString;
            Dictionary<string, string> retValue = new  Dictionary<string, string>();

            try
            {
                sqlString = String.Format(
                              "update tbLog Lg," + 
                             " (select L1.time As T1, L1.system_id As System_1, " +
                             "         L2.time As T2, L2.system_id As System_2," +
                             "         sqrt(POW(L1.x - L2.x, 2) + POW(L1.y - L2.y, 2) +  POW(L1.z - L2.z, 2)) as Distance_Between " +
                             " from (select L.time, L.system_id, Sy.systemname, Sy.x, Sy.y, Sy.z  from tbLog L, tbSystems Sy where L.system_id = sy.id) L1, " +
                             "         (select L.time, L.system_id, Sy.systemname, Sy.x, Sy.y, Sy.z  from tbLog L, tbSystems Sy where L.system_id = sy.id) L2" +
                             " where L1.Time > L2.Time" +
                             "     and L2.Time = (select max(L3.time) from tbLog L3 where L3.time < L1.Time)" +
                             "     and L1.system_id <> L2.system_id" +
                             "     and ((L1.x <> 0.0 AND L1.y <> 0.0 AND L1.Z <> 0.0) Or (L1.Systemname = 'Sol'))" +
                             "     and ((L2.x <> 0.0 AND L2.y <> 0.0 AND L2.Z <> 0.0) Or (L2.Systemname = 'Sol'))" +
                             "     and L1.time >= {0}" +
                             "     and L1.time <= {1}" +
                             " order by L1.time desc) c" +
                             " set Lg.distance = c.Distance_Between" +
                             " where Lg.time = c.T1", 
                             DBConnector.SQLDateTime(startTime), 
                             DBConnector.SQLDateTime(endTime));

                Program.DBCon.Execute(sqlString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while recalcing jump distances", ex);
            }
        }

        /// <summary>
        /// retrun the distance between two systems (given by name)
        /// </summary>
        /// <param name="System_1"></param>
        /// <param name="System_2"></param>
        /// <returns></returns>
        internal double? getDistanceBetween(string System_1, string System_2)
        {
            String sqlString;
            DataTable Data;
            double? retValue = null;

            try
            {
                Data = new DataTable();

                sqlString = String.Format("select sqrt(POW(L1.x - L2.x, 2) + POW(L1.y - L2.y, 2) +  POW(L1.z - L2.z, 2)) as Distance_Between " +
                                          " from  " +
                                          " (select Sy.id, Sy.systemname, Sy.x, Sy.y, Sy.z  from tbSystems Sy where sy.systemname = {0}) L1,  " +
                                          " (select Sy.id, Sy.systemname, Sy.x, Sy.y, Sy.z  from tbSystems Sy where sy.systemname = {1}) L2 " +
                                          "  where ((L1.x <> 0.0 AND L1.y <> 0.0 AND L1.Z <> 0.0) Or (L1.Systemname = 'Sol')) " +
                                          "  and ((L2.x <> 0.0 AND L2.y <> 0.0 AND L2.Z <> 0.0) Or (L2.Systemname = 'Sol'))", 
                                          DBConnector.SQLAEscape(System_1), DBConnector.SQLAEscape(System_2));

                if(Program.DBCon.Execute(sqlString, Data) > 0)
                    retValue = (Double)Data.Rows[0]["Distance_Between"];

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while calculating the distance between two systems", ex);
            }
        }

        /// <summary>
        /// returns the distance between two systems (given by id)
        /// </summary>
        /// <param name="System_1"></param>
        /// <param name="System_2"></param>
        /// <returns></returns>
        internal double? getDistanceBetween(Int32 System_1, Int32 System_2)
        {
            String sqlString;
            DataTable Data;
            double? retValue = null;

            try
            {
                Data = new DataTable();

                sqlString = String.Format("select sqrt(POW(L1.x - L2.x, 2) + POW(L1.y - L2.y, 2) +  POW(L1.z - L2.z, 2)) as Distance_Between " +
                                          " from  " +
                                          " (select Sy.id, Sy.systemname, Sy.x, Sy.y, Sy.z  from tbSystems Sy where sy.id = {0}) L1,  " +
                                          " (select Sy.id, Sy.systemname, Sy.x, Sy.y, Sy.z  from tbSystems Sy where sy.id = {1}) L2 " +
                                          "  where ((L1.x <> 0.0 AND L1.y <> 0.0 AND L1.Z <> 0.0) Or (L1.Systemname = 'Sol')) " +
                                          "  and ((L2.x <> 0.0 AND L2.y <> 0.0 AND L2.Z <> 0.0) Or (L2.Systemname = 'Sol'))", 
                                          System_1, System_2);

                if(Program.DBCon.Execute(sqlString, Data) > 0)
                    retValue = (Double)Data.Rows[0]["Distance_Between"];

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while calculating the distance between two systems", ex);
            }
        }

        /// <summary>
        /// Inserts data for new columns in existing definitions of datagrids
        /// </summary>
        /// <param name="iniGroup"></param>
        /// <param name="iniKey"></param>
        /// <param name="newColumnIndex"></param>
        /// <param name="newViewIndex"></param>
        /// <param name="newColumnDefinition"></param>
        public static void InsertColumnDefinition(string iniGroup, string iniKey, int newColumnIndex,  int newViewIndex, string newColumnDefinition)
        {
            // format from "DBGUIInterface"
            // 
            // SaveString.Append(String.Format("{0}/{1}/{2}/{3}/{4}/{5};", currentColumn.DisplayIndex.ToString(), 
            //                                                             currentColumn.Visible.ToString(), 
            //                                                             currentColumn.AutoSizeMode.ToString(), 
            //                                                             currentColumn.Width.ToString(), 
            //                                                             currentColumn.FillWeight.ToString().Replace(",","."), 
            //                                                             currentColumn.MinimumWidth.ToString()));

            try
            {

                System.Data.DataTable data = new System.Data.DataTable();

                String sqlString = String.Format("select InitValue from tbInitValue" +
                                                 " where InitGroup = '{0}'" +
                                                 " and InitKey     = '{1}'",
                                                 iniGroup, iniKey);

                if(Program.DBCon.Execute(sqlString, data) != 0)
                {
                    String dataString = (String)(data.Rows[0].ItemArray[0]);
                    System.Text.StringBuilder newString = new System.Text.StringBuilder();

                    Int32 currentColumnIndex = 0;

                    foreach (String existingColumnDefinition in dataString.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {

                        if(currentColumnIndex == newColumnIndex)
                        {
                            // insert the new column
                            newString.Append(newViewIndex + "/" + newColumnDefinition);
                            newString.Append(";");
                        }

                        String[] existingParts = existingColumnDefinition.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        if(Int32.Parse(existingParts[0]) >= newViewIndex)
                        { 
                            existingParts[0] = (Int32.Parse(existingParts[0]) + 1).ToString();
                            newString.Append(String.Join("/", existingParts));
                        }
                        else
                        { 
                            // take the leading column unchanged 
                            newString.Append(existingColumnDefinition);
                        }

                        newString.Append(";");
                        currentColumnIndex++;
                    } 

                    sqlString = String.Format("update tbInitValue" +
                                              " set InitValue   = '{2}'" +  
                                              " where InitGroup = '{0}'" +
                                              " and InitKey     = '{1}';",
                                              iniGroup, iniKey, newString.ToString());
                    Program.DBCon.Execute(sqlString);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting new column defnitions", ex);
            }   

        }

        /// <summary>
        /// gets all neighbours in a bubble around a system
        /// </summary>
        /// <param name="stationId">system in the center of the bubble</param>
        /// <param name="maxDistance">radius of the bubble</param>
        /// <returns></returns>
        public DataTable GetNeighbourSystems(Int32 stationId, Double maxDistance)
        {
            String sqlBaseString;
            String sqlString;
            DataTable data            = new DataTable();
            DataColumn[] keys         = new DataColumn[1]; 

            try
            {

                sqlBaseString =  " select FS.Id, FS.Systemname, sqrt(POW(FS.x - BS.x, 2) + POW(FS.y - BS.y, 2) +  POW(FS.z - BS.z, 2)) as Distance" +
                                 " from (select * from tbSystems " + 
                                 "         where id = {0}) BS" +
                                 " join tbSystems FS on (sqrt(POW(FS.x - BS.x, 2) + POW(FS.y - BS.y, 2) +  POW(FS.z - BS.z, 2)) <=  {1});";

                sqlString = String.Format(sqlBaseString, stationId, maxDistance);

                Program.DBCon.Execute(sqlString, data);

                keys[0] = data.Columns["Id"];
                data.PrimaryKey  = keys;

                return data;

	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while getting neighbours of a system", ex);
	        }
        }




        #endregion

#region handling of EDCD data

        public void ImportEDCDData(frmDataIO.enImportTypes enImportTypes, String importFile)
        {
            try
            {
                String sqlBaseString = "";
                String sqlString = "";
                Int32 dataParts = 0;
                List<String> CSV_Strings = new List<String>();
                String headerDefinition = "";
                Int32 changed = 0;
                Int32 errors = 0;
                Int32 counter = 0;
                Int32 counter2 = 0;
                ProgressEventArgs eva;

                switch (enImportTypes)
                {
                    case frmDataIO.enImportTypes.EDCD_Commodity:
                        headerDefinition = "id,category,name,average";
                        sqlBaseString    = "INSERT INTO tbCommodityBase" +
                                           " (id, category, name, average)" +
                                           " VALUES ({0}, {1}, {2}, {3}) " +
                                           " ON DUPLICATE KEY UPDATE " +
                                           " id          = Values(id)," +
                                           " category    = Values(category)," +
                                           " name        = Values(name)," +
                                           " average     = Values(average);";
                        break;

                    case frmDataIO.enImportTypes.EDCD_Outfitting:
                        headerDefinition = "id,symbol,category,name,mount,guidance,ship,class,rating,entitlement";
                        sqlBaseString    = "INSERT INTO tbOutfittingBase" +
                                           " (id, symbol, category, name, mount, guidance, ship, class, rating, entitlement)" +
                                           " VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}) " +
                                           " ON DUPLICATE KEY UPDATE " +
                                           " id          = Values(id)," +
                                           " symbol      = Values(symbol)," +
                                           " category    = Values(category)," +
                                           " name        = Values(name)," +
                                           " mount       = Values(mount)," +
                                           " guidance    = Values(guidance)," +
                                           " ship        = Values(ship)," +
                                           " class       = Values(class)," +
                                           " rating      = Values(rating)," +
                                           " entitlement = Values(entitlement);";
                        break;

                    case frmDataIO.enImportTypes.EDCD_Shipyard:
                        headerDefinition = "id,symbol,name";
                        sqlBaseString    = "INSERT INTO tbShipyardBase" +
                                           " (id, symbol, name)" +
                                           " VALUES ({0}, {1}, {2}) " +
                                           " ON DUPLICATE KEY UPDATE " +
                                           " id          = Values(id)," +
                                           " symbol      = Values(symbol)," +
                                           " name        = Values(name);";
                        break;

                    default:
                        break;
                }

                dataParts      = headerDefinition.Split(new char[] { ',' }).ToList().Count;
                var reader     = new StreamReader(File.OpenRead(importFile));
                string header  = reader.ReadLine();

                sendProgressEvent(new ProgressEventArgs() { Info="reading data from file " + Path.GetFileName(importFile) + " ...", AddSeparator=true });

                if (header.StartsWith(headerDefinition))
                {
                    
                    do
                    {
                        CSV_Strings.Add(reader.ReadLine());
                        counter++;

                        if(sendProgressEvent(new ProgressEventArgs() { Info="reading data from file " + Path.GetFileName(importFile) + " ...",  CurrentValue=counter }))
                            break;

                    } while (!reader.EndOfStream);

                    reader.Close();

                    if(!sendProgressEvent(new ProgressEventArgs() { Info="reading data from file..." + Path.GetFileName(importFile) + " ...",  CurrentValue=counter, TotalValue=counter, ForceRefresh=true }))
                    {
                        // gettin' some freaky performance
                        Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                        sendProgressEvent(new ProgressEventArgs() { Info="importing data from file " + Path.GetFileName(importFile) + " ...",  CurrentValue=0, TotalValue=counter, ForceRefresh=true });

                        foreach (String csvString in CSV_Strings)
                        {
                            List<String> csvParts = csvString.Split(new char[] {','}).ToList();

                            counter2++;

                            if(csvParts.Count == dataParts)
                            {
                                try
                                {
                                    switch (enImportTypes)
                                    {
                                        case frmDataIO.enImportTypes.EDCD_Commodity:
                                            sqlString = String.Format(sqlBaseString, 
                                                                        csvParts[0], 
                                                                        DBConnector.SQLAEscape(csvParts[1]), 
                                                                        DBConnector.SQLAEscape(csvParts[2]), 
                                                                        String.IsNullOrEmpty(csvParts[3]) ? "null" : csvParts[3]);
                                            break;

                                        case frmDataIO.enImportTypes.EDCD_Outfitting:
                                            sqlString = String.Format(sqlBaseString, 
                                                                        csvParts[0], 
                                                                        DBConnector.SQLAEscape(csvParts[1]), 
                                                                        DBConnector.SQLAEscape(csvParts[2]), 
                                                                        DBConnector.SQLAEscape(csvParts[3]), 
                                                                        DBConnector.SQLAEscape(csvParts[4]), 
                                                                        DBConnector.SQLAEscape(csvParts[5]), 
                                                                        DBConnector.SQLAEscape(csvParts[6]), 
                                                                        DBConnector.SQLAEscape(csvParts[7]), 
                                                                        DBConnector.SQLAEscape(csvParts[8]), 
                                                                        DBConnector.SQLAEscape(csvParts[9]));
                                            break;

                                        case frmDataIO.enImportTypes.EDCD_Shipyard:
                                            sqlString = String.Format(sqlBaseString, 
                                                                        csvParts[0], 
                                                                        DBConnector.SQLAEscape(csvParts[1]), 
                                                                        DBConnector.SQLAEscape(csvParts[2]));
                                            break;
                                    }


                                    changed += Program.DBCon.Execute(sqlString);
                                }
                                catch (Exception ex)
                                {
                                    errors++;
                                    sendProgressEvent(new ProgressEventArgs() { Info="error while importing line <" + counter2 + "> : " + csvString, CurrentValue=-1, TotalValue=-1, NewLine=true });
                                }
                            }
                            else
                            {
                                    errors++;
                                    sendProgressEvent(new ProgressEventArgs() { Info="error while importing line <" + counter2 + "> : " + csvString, CurrentValue=-1, TotalValue=-1, NewLine=true });
                            }

                            if(sendProgressEvent(new ProgressEventArgs() {Info="importing data from file..." + Path.GetFileName(importFile) + " ...", CurrentValue=counter2, TotalValue=counter }))
                                break;
                        }

                        // gettin' some freaky performance
                        Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                        sendProgressEvent(new ProgressEventArgs() {Info="importing data from file " + Path.GetFileName(importFile) + " ...", CurrentValue=counter2, TotalValue=counter, ForceRefresh=true});
                        sendProgressEvent(new ProgressEventArgs() {Info="new entries = " + changed + ", errors = " + errors, NewLine = true });
                    }
                }

            }
            catch (Exception ex)
            {
                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                throw new Exception("Error while importing EDCD data", ex);
            }
                    
        }

#endregion

        public void ExportLocalizationDataToCSV(string fileName, enLocalizationType activeSetting)
        {
            String sqlString = "";
            DataTable data;
            Int32 counter;
            String infoString = "";
            String idString = "";

            try
            {
                switch (activeSetting)
                {
                    case enLocalizationType.Commodity:
                        sqlString = "select Lo.commodity_id As id, La.language, Lo.locname" + 
                                    "   from tbCommodityLocalization Lo, tbLanguage La" +
                                    "   where Lo.language_id = La.id" +
                                    " order by Lo.commodity_id, La.id";
                        infoString = "export commodity localization...";
                        idString = "Commodity_ID;Language;Name";
                        break;
                    case enLocalizationType.Category:
                        sqlString = "select Lo.category_id As id, La.language, Lo.locname" + 
                                    "   from tbCategoryLocalization Lo, tbLanguage La" +
                                    "   where Lo.language_id = La.id" +
                                    " order by Lo.category_id, La.id";
                        infoString = "export category localization...";
                        idString = "Category_ID;Language;Name";
                        break;
                    case enLocalizationType.Economylevel:
                        sqlString = "select Lo.economylevel_id As id, La.language, Lo.locname" + 
                                    "   from tbLevelLocalization Lo, tbLanguage La" +
                                    "   where Lo.language_id = La.id" +
                                    " order by Lo.economylevel_id, La.id";
                        infoString = "export economylevel localization...";
                        idString = "EconomyLevel_ID;Language;Name";
                        break;
                    default:
                        throw new Exception("unknown setting :  " + activeSetting);
                }

                if(System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                data        = new DataTable();
                counter     = 0;

                Program.DBCon.Execute(sqlString, data);

                sendProgressEvent(new ProgressEventArgs() { Info="export prices...", NewLine=true });

                var writer = new StreamWriter(File.OpenWrite(fileName));
                writer.WriteLine(idString);
                

                foreach (DataRow row in data.Rows)
                {
                    writer.WriteLine(row["id"] + ";" + 
                                     row["language"] + ";" + 
                                     row["locname"] );
                    counter++;
                    sendProgressEvent(new ProgressEventArgs() { Info="export prices...", CurrentValue=counter, TotalValue=data.Rows.Count });
                }

                sendProgressEvent(new ProgressEventArgs() {Info="export prices...", CurrentValue=1, TotalValue=1, ForceRefresh=true });

                writer.Close();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while exporting localization data to csv", ex);
            }
        }

        public void ImportLocalizationDataFromCSV(string fileName, EliteDBIO.enLocalizationType activeSetting, enLocalisationImportType importType = enLocalisationImportType.onlyNew)
        {
            String sqlString = "";
            Int32 counter = 0;
            String infoString = "";
            String idString = "";
            String dataLine;
            DataSet importData;
            DataTable importTable;

            try
            {
                switch (activeSetting)
                {
                    case enLocalizationType.Commodity:
                        infoString = "import commodity localization...";
                        idString = "Commodity_ID;Language;Name";
                        break;
                    case enLocalizationType.Category:
                        infoString = "import category localization...";
                        idString = "Category_ID;Language;Name";
                        break;
                    case enLocalizationType.Economylevel:
                        infoString = "import economylevel localization...";
                        idString = "EconomyLevel_ID;Language;Name";
                        break;
                    default:
                        throw new Exception("unknown setting :  " + activeSetting);
                }

                List<string> dataLines = File.ReadAllLines(fileName).ToList();
                
                if(dataLines[0].Equals(idString, StringComparison.InvariantCultureIgnoreCase))
                {
                    importData  = new DataSet();
                    importTable = new DataTable("Names");
                    importData.Tables.Add(importTable);
                    DataColumn column;

                    column = new DataColumn("id", Type.GetType("System.Int32"));
                    column.AllowDBNull = false;
                    column.Unique       = true;
                    importTable.Columns.Add(column);

                    for (int i = 1; i < dataLines.Count; i++)
        			{
                        if(!String.IsNullOrEmpty(dataLines[i]))
                        {
                            switch (activeSetting)
                            {
                                case enLocalizationType.Commodity:
                                    List<String> data = dataLines[i].Split(new char[] {';'}).ToList();
                                    Int32 currentID = Int32.Parse(data[0]);

                                    if (!importTable.Columns.Contains(data[1]))
                                    {

                                        Debug.Print(Type.GetType("System.String").ToString());
                                        // add a new language in table
                                        column = new DataColumn(data[1], Type.GetType("System.String"));
                                        column.DefaultValue = "";
                                        importTable.Columns.Add(column);
                                    }

                                    DataRow currentrow;

                                    var rowForID = importTable.Select("id = " + currentID);
                                    
                                    if (rowForID.Count() == 0)
                                    {
                                        // add a new row
                                        currentrow        = importTable.NewRow();
                                        currentrow["id"]  = currentID;
                                        importTable.Rows.Add(currentrow);
                                    }
                                    else
                                        currentrow = rowForID[0];

                                    currentrow[data[1]] = data[2].Trim();

                                    break;
                                case enLocalizationType.Category:
                                    break;
                                case enLocalizationType.Economylevel:
                                    break;
                                default:
                                    throw new Exception("unknown setting :  " + activeSetting);
                            }
                        }

                        counter++;
                    }
                
                    switch (activeSetting)
                    {
                        case enLocalizationType.Commodity:
                            ImportCommodityLocalizations(importData, importType);
                            break;
                        case enLocalizationType.Category:
                            infoString = "import category localization...";
                            idString = "Category_ID;Language;Name";
                            break;
                        case enLocalizationType.Economylevel:
                            infoString = "import economylevel localization...";
                            idString = "EconomyLevel_ID;Language;Name";
                            break;
                        default:
                            throw new Exception("unknown setting :  " + activeSetting);
                    }

                }
                else
                {
                    sendProgressEvent(new ProgressEventArgs() { Info="abort: file has wrong header", CurrentValue=1, TotalValue=1, ForceRefresh=true });
                }

                dataLines.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing localization data from csv", ex);
            }
        }

        internal string GetMapping(string mappingTable, string idString1, Boolean throwException = true)
        {
            string idString2 = String.Empty;
            return GetMapping(mappingTable, idString1, idString2, throwException);
        }

        internal string GetMapping(string mappingTable, string idString1, string idString2, Boolean throwException = true)
        {
            String retValue;

            Tuple<String, String> mappedValue = GetMappingT(mappingTable, idString1, idString2, throwException);

            if(mappedValue == null)
                retValue = null;
            else
                retValue = mappedValue.Item1;

            return  retValue;
        }

        internal Tuple<String, String> GetMappingT(string mappingTable, string idString1, Boolean throwException = true)
        {
            string idString2 = String.Empty;
            return GetMappingT(mappingTable, idString1, idString2, throwException);
        }

        internal Tuple<String,String> GetMappingT(string mappingTable, string idString1, string idString2, Boolean throwException = true)
        {
            Tuple<String,String> retValue;

            try
            {
                DataRow mappingData = m_BaseData.Tables["tbDNMap_" + mappingTable].Rows.Find(new Object[] {idString1, idString2});

                if(mappingData == null)
                    if(throwException)
                        throw new KeyNotFoundException(String.Format("Key '{0}' + '{1}' not found in 'tbDNMap_{2}'", idString1, idString2, mappingTable));
                    else
                        retValue = null;
                else
                {
                    retValue = new Tuple<string,string>(mappingData.Field<String>("GameName"), mappingData.Field<String>("GameAddition"));
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting a mapping", ex);
            }
        }

        /// <summary>
        /// returns the coordinates of the system
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        internal Point3Dbl GetCoordinates(String systemName)
        {
            Point3Dbl retValue = new Point3Dbl();;
            String sqlString;
            DataTable data = new DataTable();

            try
            {
                sqlString = "select x, y, z from tbSystems where SystemName = " + DBConnector.SQLAEscape(systemName);

                Program.DBCon.Execute(sqlString, data);

                if(data.Rows.Count > 0)
                {
                    retValue.X = ((data.Rows[0]["x"] == DBNull.Value) ? null : (double?)data.Rows[0]["x"]);
                    retValue.Y = ((data.Rows[0]["y"] == DBNull.Value) ? null : (double?)data.Rows[0]["y"]);
                    retValue.Z = ((data.Rows[0]["z"] == DBNull.Value) ? null : (double?)data.Rows[0]["z"]);
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving system coordinates from database", ex);
            }
        }

        /// <summary>
        /// sends the log entries with the given timestamps to EDSM
        /// </summary>
        /// <param name="timeStamps"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeRush", "Try statement without catch or finally")]
        public void SendLogToEDSM(List<DateTime> timeStamps)
        {
            String sqlString        = "";
            DataTable data          = new DataTable();
            StringBuilder logData   = new StringBuilder();
            String lastSystemname   = "";

            try
            {
                if(timeStamps.Count > 0)
                {
                    sqlString = String.Format("select L.time, Sy.Systemname" +
                                              " from tbLog L, tbSystems Sy" +
                                              " where L.system_id = Sy.id" +
                                              " and   L.time >= {0}" +
                                              " and   L.time <= {1}" +
                                              " and   event_id = {2}" +
                                              " order by l.time asc;",
                                              DBConnector.SQLDateTime(timeStamps.Min()),
                                              DBConnector.SQLDateTime(timeStamps.Max()), 
                                              BaseTableNameToID("eventtype", "Jumped To"));

                    Program.DBCon.Execute(sqlString, data);

                    foreach (DataRow dRow in data.Rows)
                    {
                        Program.EDSMComm.TransmitVisit((String)dRow["Systemname"], null, null, null, (DateTime)dRow["time"]);
                    }



                    //sqlString = String.Format("select L.time, Sy.Systemname, Sy.id, L.notes" +
                    //                          " from tbLog L, tbSystems Sy" +
                    //                          " where L.system_id = Sy.id" +
                    //                          " and   L.time >= {0}" +
                    //                          " and   L.time <= {1}" +
                    //                          " and   L.notes Is Not null" +
                    //                          " and   Length(Trim(L.notes)) > 0" +
                    //                          " order by Sy.id asc, l.time asc;",
                    //                          DBConnector.SQLDateTime(timeStamps.Min()),
                    //                          DBConnector.SQLDateTime(timeStamps.Max()));

                    //Program.DBCon.Execute(sqlString, data);


                    //for (int i = 0; i <= data.Rows.Count; i++)
                    //{
                    //    if(i < data.Rows.Count)
                    //    {
                    //        DataRow dRow = data.Rows[i];

                    //        if ((((String)dRow["Systemname"]) != lastSystemname) && (!String.IsNullOrEmpty(lastSystemname)))
                    //        {
                    //            // send comment
                    //            Program.EDSMComm.TransmitCommentExtension(lastSystemname, logData.ToString());
                    //            logData.Clear();
                    //        }

                    //        // extend comment
                    //        logData.AppendLine(String.Format("{0:G}:", dRow["time"]));
                    //        logData.AppendLine((String)dRow["notes"]);

                    //        lastSystemname = (String)dRow["Systemname"];
                    //    }
                    //    else if(!String.IsNullOrEmpty(lastSystemname))
                    //    {
                    //        // send comment
                    //        Program.EDSMComm.TransmitCommentExtension(lastSystemname, logData.ToString());
                    //        logData.Clear();
                    //    }
                    //}


                    sqlString = String.Format("select L.time, Sy.Systemname, St.Stationname, Sy.id, L.notes" +
                                              " from tbSystems Sy, tbLog L left join tbStations St on L.station_id = St.id" +
                                              " where L.system_id  = Sy.id" +
                                              " and   L.time >= {0}" +
                                              " and   L.time <= {1}" +
                                              " and   L.notes Is Not null" +
                                              " and   Length(Trim(L.notes)) > 0" +
                                              " order by l.time asc;",
                                              DBConnector.SQLDateTime(timeStamps.Min()),
                                              DBConnector.SQLDateTime(timeStamps.Max()));

                    Program.DBCon.Execute(sqlString, data);


                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        DataRow dRow = data.Rows[i];

                        String stationName = dRow["Stationname"].ToString();

                        if(!String.IsNullOrWhiteSpace(stationName))
                            logData.AppendLine(String.Format("{0:G} - {1} :", dRow["time"], stationName));
                        else
                            logData.AppendLine(String.Format("{0:G} :", dRow["time"]));

                        
                        logData.AppendLine((String)dRow["notes"]);
                        // send comment

                        Program.EDSMComm.TransmitCommentExtension((String)dRow["Systemname"], stationName, 
                                                                  System.Text.RegularExpressions.Regex.Replace(logData.ToString(), "(?<!\r)\n", "\r\n"), 
                                                                  (DateTime)dRow["time"]);
                        logData.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while collecting data for transmission to EDSM", ex);
            }
        }

        /// <summary>
        /// returns the name of a system through a given station-id
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public String GetSystemnameFromStation(int stationID)
        {
            try
            {
                return Program.DBCon.Execute<String>("select systemname from tbSystems Sy, tbStations St" +
                                                     " where Sy.id = St.System_id" +
                                                     " and   St.id = " + stationID).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting a systemname from a station id", ex);
            }
        }

        /// <summary>
        /// returns the name of a system through a given station-id
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public String GetStationnameFromStationID(int stationID)
        {
            try
            {
                return Program.DBCon.Execute<String>("select stationname from tbSystems Sy, tbStations St" +
                                                     " where Sy.id = St.System_id" +
                                                     " and   St.id = " + stationID).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting a stationname from a station id", ex);
            }
        }

        /// <summary>
        /// returns a single log entry identified by its timestamp
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public dsEliteDB.vilogRow GetLogByTimestamp(DateTime timeStamp)
        {
            try
            {
                dsEliteDB.vilogDataTable data = new dsEliteDB.vilogDataTable();

                // the query is the vilog, but with the added 'where' clause to improve the query speed
                String sqlString = "select `l`.`time` AS `time`,`s`.`systemname` AS `systemname`,`st`.`stationname` AS `stationname`,`e`.`eventtype` AS `eevent`,`c`.`cargoaction`" +
                                   " AS `action`,`co`.`loccommodity` AS `loccommodity`,`l`.`cargovolume` AS `cargovolume`,`l`.`credits_transaction` " +
                                   " AS `credits_transaction`,`l`.`credits_total` AS `credits_total`, `l`.`distance` AS `distance`, `l`.`notes` AS `notes` from (((((`tblog` `l` " +
                                   " left join `tbeventtype` `e` on((`l`.`event_id` = `e`.`id`))) left join `tbcargoaction` `c` on((`l`.`cargoaction_id` = `c`.`id`))) " +
                                   " left join `tbsystems` `s` on((`l`.`system_id` = `s`.`id`))) left join `tbstations` `st` on((`l`.`station_id` = `st`.`id`))) " +
                                   " left join `tbcommodity` `co` on((`l`.`commodity_id` = `co`.`id`))) " +
                                   " where time = " + DBConnector.SQLDateTime(timeStamp);

                if(Program.DBCon.Execute(sqlString, data) > 0)
                    return (dsEliteDB.vilogRow)data.Rows[0];
                else
                    return null;


            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting a stationname from a station id", ex);
            }
        }


    }
}
