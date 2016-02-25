using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IBE.EDDB_Data;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using IBE.Enums_and_Utility_Classes;
using System.Diagnostics;
using System.Globalization;
using IBE.SQL.Datasets;
using System.Data.Common;

namespace IBE.SQL
{
    public class EliteDBIO
    {
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

        private const Int32 m_TimeSlice_ms    = 500;
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
        private void sendProgressEvent(String Tablename, Int32 Index, Int32 Total, Boolean noSuppress = false)
        {
            Int32   ProgressSendLevel;
            try 
	        {	        
               
		        if((m_EventTimer.currentMeasuring() > m_TimeSlice_ms) || noSuppress)
                {
                    // time is reason
                    Progress.Raise(this, new ProgressEventArgs() { Tablename = Tablename, Index = Index, Total = Total});

                    if(Total > 0)
                        ProgressSendLevel =  ((100 * Index/Total) / 10) * 10;
                    else
                        ProgressSendLevel =  -1;

                    m_EventTimer.startMeasuring();
                    m_lastProgress = ProgressSendLevel;

                    Debug.Print("Progress (t):" + ProgressSendLevel.ToString());
                }
                else
                { 
                    if(Index == 2 && Total == 8)
                        Debug.Print("!");

                    // progress is reason
                    if(Total > 0)
                        ProgressSendLevel =  ((100 * Index/Total) / 10) * 10;
                    else
                        ProgressSendLevel =  -1;
                    
                    if(((Total != 0) && (((100 * Index/Total) % m_PercentSlice) == 0) && (ProgressSendLevel != m_lastProgress)) || 
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
                                                            "tblanguage"};

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
                            // changes (MySQL doesn't support MARS "Multiple Active Result Sets")

                            currentDBCon = new DBConnector(Program.DBCon.ConfigData);
                            m_BaseData_Connector.Add(m_BaseData.Tables[BaseTable], currentDBCon);

                            currentDBCon.Connect();
                        }

                        Runtime.startMeasuring();
                        m_BaseData.Tables[BaseTable].Clear();

                        // preload all tables with base data
                        currentDBCon.TableRead(String.Format("select * from {0}", BaseTable), BaseTable, m_BaseData);
                        
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
                    throw new Exception("Attempt to load an unknown basetable");
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

            try
            {
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
        internal void ImportCommodityLocalizations(DataSet DataNames)
        {
            dsEliteDB                 Data;
            Dictionary<String, Int32> foundLanguagesFromFile     = new Dictionary<String, Int32>();
            String                    sqlString;
            Int32                     currentSelfCreatedIndex;
            Int32 Counter = 0;

            Data      = new dsEliteDB();

            try
            {
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
                    sendProgressEvent("import economy level localization", Counter, DataNames.Tables["Levels"].Rows.Count);

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
                        sendProgressEvent("import economy level localization", Counter, DataNames.Tables["Levels"].Rows.Count);
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
                                Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(System.UpdatedAt).DateTime;

                                if (Timestamp_new > Timestamp_old)
                                {
                                    // data from file is newer
                                    CopyEDSystemToDataRow(System, (DataRow)FoundRows_org[0], false, null, true);
                                    ImportCounter += 1;
                                }
                            }
                        }
                        else
                        {
                            // system is existing - keep the newer version 
                            Timestamp_old = (DateTime)(FoundRows[0]["updated_at"]);
                            Timestamp_new = DateTimeOffset.FromUnixTimeSeconds(System.UpdatedAt).DateTime;

                            if (Timestamp_new > Timestamp_old)
                            {
                                // data from file is newer
                                CopyEDSystemToDataRow(System, FoundRows[0], false, null, true);
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
                            CopyEDSystemToDataRow(System, (DataRow)FoundRows[0], false, null, true);
                        }
                        else
                        {
                            // add a new system
                            dsEliteDB.tbsystemsRow newRow = (dsEliteDB.tbsystemsRow)Data.tbsystems.NewRow();
                            CopyEDSystemToDataRow(System, (DataRow)newRow, false, null, true);
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
                sqlString = "select * from tbsource";
                Program.DBCon.Execute(sqlString, Data.tbsource);
                sqlString = "select * from tbCommodityClassification lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbcommodityclassification);
                sqlString = "select * from tbcommodity_has_attribute lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbcommodity_has_attribute);
                sqlString = "select * from tbattribute lock in share mode";
                Program.DBCon.TableRead(sqlString, Data.tbattribute);

                currentComodityClassificationID = getFreeIndex("tbCommodityClassification");

                Program.DBCon.Execute(sqlString, Data.tbsource);

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
                            Program.DBCon.TableUpdate(Data.tbstations);
                            Program.DBCon.TableUpdate(Data.tbstations_org);
                            Program.DBCon.TableUpdate(Data.tbstationeconomy);
                            Program.DBCon.TableUpdate(Data.tbcommodityclassification);
                            Program.DBCon.TableUpdate(Data.tbcommodity_has_attribute);
                            Program.DBCon.TableUpdate(Data.tbattribute);

                            Program.DBCon.TableRefresh(Data.tbstationeconomy);
                            Program.DBCon.TableRefresh(Data.tbcommodityclassification);
                            Program.DBCon.TableRefresh(Data.tbcommodity_has_attribute);
                            Program.DBCon.TableRefresh(Data.tbattribute);
                        }
                        else
                        {
                            // add a new Location
                            dsEliteDB.tbstationsRow newStationRow = (dsEliteDB.tbstationsRow)Data.tbstations.NewRow();

                            CopyEDStationToDataRow(Station, (DataRow)newStationRow, false, null, true);
                            Data.tbstations.Rows.Add(newStationRow);
                        }

                        CopyEDStationEconomiesToDataRows(Station, Data.tbstationeconomy);
                        CopyEDStationCommodityToDataRow(Station, Data, ref currentComodityClassificationID);

                        ImportCounter += 1;
                    }

                    if ((ImportCounter > 0) && ((ImportCounter % 100) == 0))
                    {
                        // save changes
                        Debug.Print("added Stations : " + ImportCounter.ToString());

                        Program.DBCon.TableUpdate(Data.tbstations);
                        Program.DBCon.TableUpdate(Data.tbstations_org);
                        Program.DBCon.TableUpdate(Data.tbstationeconomy);
                        Program.DBCon.TableUpdate(Data.tbcommodityclassification);
                        Program.DBCon.TableUpdate(Data.tbcommodity_has_attribute);
                        Program.DBCon.TableUpdate(Data.tbattribute);
                    }

                    Counter++;
                    sendProgressEvent("import stations", Counter, Stations.Count);

                }

                // save changes
                Program.DBCon.TableUpdate(Data.tbstations, true);
                Program.DBCon.TableUpdate(Data.tbstations_org, true);
                Program.DBCon.TableUpdate(Data.tbstationeconomy, true);
                Program.DBCon.TableUpdate(Data.tbcommodityclassification, true);
                Program.DBCon.TableUpdate(Data.tbcommodity_has_attribute, true);
                Program.DBCon.TableUpdate(Data.tbattribute, true);

                Program.DBCon.TransCommit();

                // now add the prices if wanted
                if (addPrices)
                {
                    ImportPrices(Stations);
                }

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
                    Program.DBCon.TableReadRemove(Data.tbcommodityclassification);
                    Program.DBCon.TableReadRemove(Data.tbcommodity_has_attribute);
                    Program.DBCon.TableReadRemove(Data.tbattribute);

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

                            Program.DBCon.TableUpdate(Data.tbstations);
                            Program.DBCon.TableUpdate(Data.tbstations_org);
                            Program.DBCon.TableUpdate(Data.tbstationeconomy);
                        }
                    }
                    else
                        Debug.Print("why");


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

                    //Debug.Print(System + "," + Location);

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
                        //Debug.Print("Error while importing station in history :" + Location);
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

                        added += Program.DBCon.Execute(sqlString);

                        if ((added > 0) && ((added % 10) == 0))
                            Debug.Print(added.ToString());

                        Counter++;
                        sendProgressEvent("import log", Counter, Data.Tables["CommandersLogEvent"].Rows.Count);
                    }

                }

                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                return added;
            }
            catch (Exception ex)
            {
                // reset freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

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
        private void ImportPrices(List<EDStation> Stations, enImportBehaviour importBehaviour = enImportBehaviour.OnlyNewer)
        {
            try
            { 
                StringBuilder sqlStringB = new StringBuilder();
                String timeFilter = "";
                Int32 Counter;

                // for the prices is no transaction necessary, because we're changing
                // only a single table

                Counter = 0;
                sendProgressEvent("updating prices", Counter, Stations.Count);

                //Int32 SourceID = ((dsEliteDB.tbsourceRow[])Data.tbsource.Select("source = " + DBConnector.SQLAString("EDDN")))[0].id;
                Int32 SourceID = (Int32)BaseTableNameToID("source", "EDDN");

                Boolean AddComma = false;
                int?  DemandLevel = null;
                int?  SupplyLevel = null;

                Dictionary<String, int?> Levels = new Dictionary<String, int?>();

                // now add the commodities and prices
                foreach (EDStation Station in Stations)
                {
                    if ((Station.Id != 0) && (Station.Listings.Count() > 0))
                    {
                        sqlStringB.Clear();
                        sqlStringB.Append("insert into tbCommodityData(id, station_id, commodity_id, Sell, Buy," +
                                          "Demand, DemandLevel, Supply, SupplyLevel, Sources_id, timestamp) ");

                        foreach (Listing StationListing in Station.Listings)
                        {
                            if (AddComma)
                                sqlStringB.Append(" union all ");

                            // cache level-ids
                            getLevels(ref DemandLevel, ref SupplyLevel, Levels, StationListing);

                            // add only, if not existing or newer !!
                                
                            

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

                        Program.DBCon.Execute(sqlStringB.ToString());
                    }

                    AddComma = false;
                    Counter++;
                    sendProgressEvent("updating prices", Counter, Stations.Count);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing prices", ex);
            }
        }

        /// <summary>
        /// Imports the prices from a file with csv-strings (e.g. the old autosave-file)
        /// </summary>
        /// <returns></returns>
        public Int32 ImportPricesFromCSVFile(String filename, enImportBehaviour importBehaviour = enImportBehaviour.OnlyNewer)
        {
            try
            {
                String[] CSV_Strings    = new String[0];
                var reader              = new StreamReader(File.OpenRead(filename));


                string header = reader.ReadLine();

                if(header.StartsWith("System;Station"))
                {
                    CSV_Strings = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                }
                reader.Close();


                ImportPricesFromCSVStrings(CSV_Strings, importBehaviour);

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
        /// <param name="Stations"></param>
        public void ImportPricesFromCSVStrings(String[] CSV_Strings, enImportBehaviour importBehaviour = enImportBehaviour.OnlyNewer)
        {
            Boolean MissingSystem   = false;
            Boolean MissingStation  = false;
            String currentLanguage;
            DataTable newData;
            List<EDStation> StationData;
            List<EDSystem> SystemData = null;


            try
            {
                // *****************************************************************
                // START :section for automatically add unknown commodities

                currentLanguage     = Program.DBCon.getIniValue(MTSettings.tabSettings.DB_GROUPNAME, "Language", Program.BASE_LANGUAGE, false);
                newData             = new DataTable();
                newData.TableName   = "Names";
                newData.Columns.Add(Program.BASE_LANGUAGE, typeof(String));
                if(currentLanguage != Program.BASE_LANGUAGE)
                    newData.Columns.Add(currentLanguage, typeof(String));

                foreach (String DataLine in CSV_Strings)
	            {
                    String currentName;
                    List<dsEliteDB.tbcommoditylocalizationRow> currentCommodity;

                    if(DataLine.Trim().Length > 0)
                    {
                        currentName         = new CsvRow(DataLine).CommodityName;
                        currentCommodity    = Program.Data.BaseData.tbcommoditylocalization.Where(x => x.locname.Equals(currentName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                        if((currentCommodity.Count == 0) && (!String.IsNullOrEmpty(currentName)))
                        {
                            if(currentLanguage == Program.BASE_LANGUAGE)
                                newData.Rows.Add(currentName);
                            else
                                newData.Rows.Add(currentName, currentName);
                        }
                    }
	            }

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
                StationData = fromCSV(CSV_Strings, ref SystemData);

                // check if we've unknown systems or stations
                foreach (EDStation Station in StationData)
                {
                    if (Station.SystemId == 0)
                        MissingSystem = true;
                    else if(Station.Id == 0)
                        MissingStation = true;
                }

                if (MissingSystem)
                {
                    // add unknown systems
                    ImportSystems_Own(ref SystemData, true);
                }

                if (MissingSystem || MissingStation)
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
                ImportPrices(StationData, importBehaviour);

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
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing self collected price data", ex);
            }
        }

        /// <summary>
        /// creates a list of "EDStations" with price listings from csv-array
        /// </summary>
        /// <param name="CSV_Strings"></param>
        /// <returns></returns>
        public List<EDStation> fromCSV(String[] CSV_Strings, ref List<EDSystem> foundSystems)
        {
            List<EDStation> foundValues                     = new List<EDStation>();
            Dictionary<String, Int32> foundIndex            = new Dictionary<String, Int32>();
            Dictionary<String, Int32> foundSystemIndex      = new Dictionary<String, Int32>();
            String LastID                                   = "";
            EDSystem LastSystem                             = null;
            String currentID                                = "";
            EDStation currentStation                        = null;
            Int32 Index                                     = 0;

            try
            {
                if(foundSystems != null)
                    foundSystems.Clear();
                else
                    foundSystems = new List<EDSystem>();


                foreach (String CSV_String in CSV_Strings)
	            {

                    if(!String.IsNullOrEmpty(CSV_String.Trim()))
                    {
		                CsvRow currentRow           = new CsvRow(CSV_String);

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

                        currentStation.addListing(currentRow);
                    }
	            }

                if(currentStation != null)
                    currentStation.ListingExtendMode = false;

                return foundValues;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting station values from CSV-String", ex);
            }
        }

        /// <summary>
        /// function deletes all exept the newest price for a commodity
        /// on a station, if there exists more than one
        /// </summary>
        public void DeleteMultiplePrices()
        {
            String sqlString;
            DataTable data = new DataTable();

            try 
	        {	        
		        sqlString = "select id from tbCommodityData C1" +
                            "	where exists(" +
                            "		select C2.station_id, C2.commodity_id, count(*) as cnt" +
                            "		from tbCommodityData C2" +
                            "		where C2.station_id   = C1.station_id" +
                            "		 and  C2.commodity_id = C1.commodity_id" +
                            "		group by C2.station_id, C2.commodity_id" +
                            "		having cnt > 1)" +
                            "	order by timestamp asc" +
                            "	limit 1";

                if (Program.DBCon.Execute(sqlString, data) > 0)
                {
                    sqlString = "";
                    foreach (DataRow deleteID in data.Rows)
                    {
                        if(sqlString.Length > 0)
                            sqlString += " or ";

                        sqlString += String.Format("id = {0}", deleteID[0].ToString());
                    }

                    sqlString = "delete from tbCommodityData where " + sqlString;

                    Program.DBCon.Execute(sqlString, data);
                }
	        }
	        catch (Exception ex)
	        {
		        throw new Exception("Error while deleting mulitple prices", ex);
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
        /// updates the localization of all commodities for the current language
        /// </summary>
        public void updateTranslation()
        {
            try
            {

                String currentLanguage = Program.DBCon.getIniValue(MTSettings.tabSettings.DB_GROUPNAME, "Language", Program.BASE_LANGUAGE, false);

                switchLanguage(currentLanguage);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating the current translation", ex);
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

           
                sendProgressEvent("clearing database...", 0, 0);

                foreach (String Table in Tables)       
	            {
                    Debug.Print("deleting " + Table + "...");
                    sendProgressEvent("deleting " + Table + "...", Counter + 1, Tables.Count, true);

		            Program.DBCon.Execute("delete from " + Table);

                    Counter++;
        	    }
                

                sendProgressEvent("clearing database...<done>", 1, 1);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while clearing the database", ex);
            }
        }

        public void checkPotentiallyNewSystemOrStation(String System, String Station, Boolean setVisitedFlag = true)
        {
            String sqlString;
            Int32 SystemID;
            Int32 LocationID;
            Boolean systemFirstTimeVisited     = false;
            Boolean stationFirstTimeVisited    = false;
            Boolean isNewSystem                = false;
            Boolean isNewStation               = false;
            DataTable Data          = new DataTable();
            Boolean Visited;

            try
            {
                System  = System.Trim();
                Station = Station.Trim();

                if(!String.IsNullOrEmpty(System))
                { 
                    sqlString       = "select id, visited from tbSystems where Systemname = " + DBConnector.SQLAEscape(System);
                    if(Program.DBCon.Execute(sqlString, Data) > 0)
                    { 
                        // check or update the visited-flag
                        SystemID = (Int32)(Data.Rows[0]["ID"]);
                        Visited  = (Boolean)(Data.Rows[0]["visited"]);

                        if(!Visited)
                        { 
                            sqlString = String.Format("insert ignore into tbVisitedSystems(system_id, time) values" +
                                                      " ({0},{1})", SystemID.ToString(),  DBConnector.SQLDateTime(DateTime.UtcNow));
                            Program.DBCon.Execute(sqlString);
                            systemFirstTimeVisited = true;
                        }
                    }
                    else
                    {
                        // add a new system
                        EDSystem newSystem      = new EDSystem();
                        newSystem.Name          = System;

                        var systemIDs           = ImportSystems_Own(newSystem, true, setVisitedFlag);

                        SystemID                = newSystem.Id;

                        isNewSystem             = true;
                        systemFirstTimeVisited  = true;
                    }
                
                

                    if(!String.IsNullOrEmpty(Station))
                    { 
                        Data.Clear();

                        sqlString    = "select St.ID, St.visited from tbSystems Sy, tbStations St" +
                                       " where Sy.ID = St. System_ID" +
                                       " and   Sy.ID          = " + SystemID +
                                       " and   St.Stationname = " + DBConnector.SQLAEscape(Station);

                        if(Program.DBCon.Execute(sqlString, Data) > 0)
                        { 
                            // check or update the visited-flag
                            LocationID = (Int32)(Data.Rows[0]["ID"]);
                            Visited    = (Boolean)(Data.Rows[0]["visited"]);

                            if(!Visited)
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
                            EDStation newStation    = new EDStation();
                            newStation.Name         = Station;
                            newStation.SystemId     = SystemID;

                            ImportStations_Own(newStation, true, setVisitedFlag);
                                         
                            isNewStation              = true;
                            stationFirstTimeVisited = true;
                        }
                    }


                    if(systemFirstTimeVisited || stationFirstTimeVisited)
                    {
                        // if there's a new visitedflag set in the visited-tables
                        // then update the maintables
                        Program.Data.updateVisitedFlagsFromBase(systemFirstTimeVisited, stationFirstTimeVisited);

                        // last but not least reload the BaseTables with the new visited-information
                        if(systemFirstTimeVisited)
                            Program.Data.PrepareBaseTables(Program.Data.BaseData.tbsystems.TableName);

                        if(stationFirstTimeVisited)
                            Program.Data.PrepareBaseTables(Program.Data.BaseData.tbstations.TableName);
                    }

                    if(isNewSystem || isNewStation)
                    {
                        Program.Data.PrepareBaseTables(Program.Data.BaseData.visystemsandstations.TableName);
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
        /// adds missing distances in the tbLog-table
        /// </summary>
        /// <param name="maxAge">considers only entrys, if they are younger or equal than ..</param>
        internal void addMissingDistancesInLog(DateTime maxAge)
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
                             " order by L1.time desc) c" +
                             " set Lg.distance = c.Distance_Between" +
                             " where Lg.time = c.T1", 
                             DBConnector.SQLDateTime(maxAge));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting dictionary of all economylevels", ex);
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
#endregion

        /// <summary>
        /// exports the market data to a file
        /// </summary>
        /// <param name="fileName"></param>
        public void ExportToCSV(string fileName, Boolean inCurrentLanguage)
        {
            String sqlString;
            DataTable data;
            Int32 Counter;

            try
            {

                if (inCurrentLanguage)
                {
                    // export names in user language
                    sqlString = "select Sy.systemname, St.stationname, C.loccommodity as commodity, D.sell, D.buy, D.demand, D.demandlevel, D.supply, D.supplylevel, D.timestamp" +
                                " from tbSystems Sy, tbStations St, tbCommodityData D, tbCommodity C, tbeconomylevel E" +
                                " where Sy.id           = St.system_id" + 
                                " and   St.id           = D.station_id" +
                                " and   D.commodity_id  = C.id" +
                                " order by Sy.systemname, St.stationname, C.loccommodity"; 
                }
                else
                {
                    // export names in default language (english)
                    sqlString = "select Sy.systemname, St.stationname, C.commodity, D.sell, D.buy, D.demand, D.demandlevel, D.supply, D.supplylevel, D.timestamp" +
                                " from tbSystems Sy, tbStations St, tbCommodityData D, tbCommodity C" +
                                " where Sy.id           = St.system_id" + 
                                " and   St.id           = D.station_id" +
                                " and   D.commodity_id  = C.id" +
                                " order by Sy.systemname, St.stationname, C.commodity"; 
                }

                if(System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                data        = new DataTable();
                Counter     = 0;

                Program.DBCon.Execute(sqlString, data);

                sendProgressEvent("export prices...", 0, 0);

                var writer = new StreamWriter(File.OpenWrite(fileName));
                writer.WriteLine("System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;");
                

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

                    writer.WriteLine(row["systemname"] + ";" + 
                                     row["stationname"] + ";" + 
                                     row["commodity"] + ";" + 
                                     row["sell"] + ";" + 
                                     row["buy"] + ";" + 
                                     row["demand"] + ";" + 
                                     Demand + ";" + 
                                     row["supply"] + ";" + 
                                     Supply + ";" + 
                                     row["timestamp"]);
                    Counter++;
                    sendProgressEvent("export prices...", Counter, data.Rows.Count);
                }

                sendProgressEvent("export prices...", 1, 1);

                writer.Close();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while exporting to csv file", ex);
            }
        }

    }

}
