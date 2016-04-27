using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using IBE.Enums_and_Utility_Classes;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using IBE.SQL;
using IBE.SQL.Datasets;
using System.Collections.Generic;
using IBE.ExtData;


namespace IBE.MTPriceAnalysis
{
    public class PriceAnalysis
    {
        private const string tbn_BestMarketPrices = "tbBestMarketPrices";

        public enum enGUIEditElements
        {
            cbLogEventType,
            cbLogSystemName,
            cbLogStationName,
            cbLogCargoName,
            cbCargoAction
        }

#region event handler

        public event EventHandler<DataChangedEventArgs> DataChanged;

        protected virtual void OnDataChanged(DataChangedEventArgs e)
        {
            EventHandler<DataChangedEventArgs> myEvent = DataChanged;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class DataChangedEventArgs : EventArgs
        {
            public Int32    DataRow { get; set; }
            public DateTime DataKey { get; set; }
        }

#endregion

        private dsEliteDB                           m_BaseData;
        private tabPriceAnalysis                    m_GUI;
        private BindingSource                       m_BindingSource;
        private DataTable                           m_Datatable;
        private DataRetriever                       retriever;
        private Boolean                             m_NoGuiNotifyAfterSave;
        private FileScanner.EDLogfileScanner        m_LogfileScanner;
        private ExternalDataInterface               m_ExternalDataInterface;
        private IBE.IBESettingsView                 m_Settings;
        private DBConnector                         m_lDBCon;
        
        /// <summary>
        /// constructor
        /// </summary>
        public PriceAnalysis(DBConnector ownDBConnector)
        {
            try
            {
                m_lDBCon                    = ownDBConnector;
                m_BindingSource             = new BindingSource();
                m_Datatable                 = new DataTable();

                m_BindingSource.DataSource  = m_Datatable;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating the object", ex);
            }
        }

        /// <summary>
        /// initialization of the dataretriever object (for DGV virtual mode)
        /// </summary>
        /// <returns></returns>
        internal int InitRetriever()
        {
            try
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in InitRetriever", ex);
            }
        }

        /// <summary>
        /// access to the dataretriever object (for DGV virtual mode)
        /// </summary>
        public DataRetriever Retriever
        {
            get
            {
                return retriever;
            }
        }

        /// <summary>
        /// gets or sets the belonging base dataset
        /// </summary>
        public dsEliteDB BaseData
        {
            get
            {
                return m_BaseData;
            }
            set
            {
                m_BaseData = value;
            }
        }

        /// <summary>
        /// access to the belonging gui object
        /// </summary>
        public tabPriceAnalysis GUI
        {
            get
            {
                return m_GUI;
            }
            set
            {
                m_GUI = value;
                if((m_GUI != null) && (m_GUI.DataSource != this))
                    m_GUI.DataSource = this;
            }
        }

        /// <summary>
        /// register the LogfileScanner in the CommandersLog for the DataEvent
        /// </summary>
        /// <param name="LogfileScanner"></param>
        public void registerLogFileScanner(FileScanner.EDLogfileScanner LogfileScanner)
        {
            try
            {
                if(m_LogfileScanner == null)
                { 
                    m_LogfileScanner = LogfileScanner;
                    m_LogfileScanner.LocationChanged += LogfileScanner_LocationChanged;
                }
                else 
                    throw new Exception("LogfileScanner already registered");

            }
            catch (Exception ex)
            {
                throw new Exception("Error while registering the LogfileScanner", ex);
            }
        }

        /// <summary>
        /// register the external tool in the CommandersLog for the DataEvent
        /// </summary>
        /// <param name="LogfileScanner"></param>
        public void registerExternalTool(ExternalDataInterface ExternalDataInterface)
        {
            try
            {
                if(m_ExternalDataInterface == null)
                { 
                    m_ExternalDataInterface                    = ExternalDataInterface;
                    m_ExternalDataInterface.ExternalDataEvent += m_ExternalDataInterface_ExternalDataEvent;
                }
                else 
                    throw new Exception("LogfileScanner already registered");

            }
            catch (Exception ex)
            {
                throw new Exception("Error while registering the LogfileScanner", ex);
            }
        }

        /// <summary>
        /// id of a fixed target station if not null
        /// </summary>
        public Int32 FixedStation
        {
            get
            {
                return Program.DBCon.getIniValue<Int32>(tabPriceAnalysis.DB_GROUPNAME, "FixedStationValue", "0", false);
            }
            set
            {
                Program.DBCon.setIniValue(tabPriceAnalysis.DB_GROUPNAME, "FixedStationValue", value.ToString());
            }
        }

        /// <summary>
        /// filter for commodities to buy from station 1
        /// </summary>
        public List<Int32> CommoditiesSend
        {
            get
            {
                return (Program.DBCon.getIniValue(tabPriceAnalysis.DB_GROUPNAME, "BuyCommoditiesSend", "").Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries)).ToList().ConvertAll(s => Int32.Parse(s));
            }
            set
            {
                Program.DBCon.setIniValue(tabPriceAnalysis.DB_GROUPNAME, "BuyCommoditiesSend", String.Join(";", value));
            }
        }

        /// <summary>
        /// filter for commodities to buy from station 2 and return to station 1
        /// </summary>
        public List<Int32> CommoditiesReturn
        {
            get
            {
                return (Program.DBCon.getIniValue(tabPriceAnalysis.DB_GROUPNAME, "BuyCommoditiesReturn", "").Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries)).ToList().ConvertAll(s => Int32.Parse(s));
            }
            set
            {
                Program.DBCon.setIniValue(tabPriceAnalysis.DB_GROUPNAME, "BuyCommoditiesReturn", String.Join(";", value));
            }
        }

        /// <summary>
        /// event-worker for LocationChangedEvent-event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LogfileScanner_LocationChanged(object sender, FileScanner.EDLogfileScanner.LocationChangedEventArgs e)
        {
            try
            {
                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.System) > 0)
                {
                    GUI.setFilterHasChanged(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the LocationChanged-event", ex);
            }
        }

        void m_ExternalDataInterface_ExternalDataEvent(object sender, ExternalDataInterface.LocationChangedEventArgs e)
        {
            try
            {
                if((e.Changed & ExternalDataInterface.enExternalDataEvents.Landed) > 0)
                {
                  
                }

                if((e.Changed & ExternalDataInterface.enExternalDataEvents.DataCollected) > 0)
                {
                    GUI.setFilterHasChanged(true);
                    //GUI.RefreshData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the LocationChanged-event", ex);
            }
        }

        /// <summary>
        /// Gets if the GUI must be informed after a data change or sets this value.
        /// Will automatically resetted after the next call of "SaveEvent".
        /// Default is "GUI must be informed" (NoGuiNotifyAfterSave = False)
        /// </summary>
        public Boolean NoGuiNotifyAfterSave
        {
            get
            {
                return m_NoGuiNotifyAfterSave;
            }
            set
            {
                m_NoGuiNotifyAfterSave = value;
            }
        }

        /// <summary>
        /// creates the filtered basetable of systems and stations
        /// </summary>
        /// <param name="SystemID"></param>
        /// <param name="Distance"></param>
        /// <param name="DistanceToStar"></param>
        /// <param name="minLandingPadSize"></param>
        /// <param name="locationType"></param>
        public void createFilteredTable(Int32 SystemID, Object Distance, Object DistanceToStar, Object minLandingPadSize, Program.enVisitedFilter VisitedFilter, Object locationType)
        {
            String sqlString;
            DataTable currentSystem = new DataTable();
            DateTime stopTime;

            try
            {
                stopTime = (DateTime.Now - new TimeSpan(Program.DBCon.getIniValue<Int32>(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "TimeFilterDays", "30", true), 0, 0, 0));

                // get info of basesystem
                sqlString = "select * from tbSystems where ID = " + SystemID.ToString();
                m_lDBCon.Execute(sqlString, currentSystem);

                sqlString = "truncate table tmFilteredStations;";
                m_lDBCon.Execute(sqlString);

                sqlString = String.Format(
                            "insert into tmFilteredStations(System_id, Station_id, Distance, x, y, z) " +
                            "   select Sy.ID As System_id, St.ID As Station_id, SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)) As Distance, Sy.x, Sy.y, Sy.z" + 
                            "   from tbSystems Sy, tbStations St" +
                            "   where Sy.ID = St.System_id",
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["x"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["y"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["z"])); 

                if(true)                            
                {
                    // filter only stations with commodities

                    // timestamp
                    if(Program.DBCon.getIniValue<Boolean>(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "TimeFilter", false.ToString(), true))
                    {
                        sqlString = sqlString + 
                                "   and exists (select CD.Station_ID from tbCommodityData CD" +
			                    "                 where St.ID         = CD.Station_ID" +
                                "                 and   CD.timestamp >= " + DBConnector.SQLDateTime(stopTime) +
			                    "                 group by Station_ID)";
                    }
                    else
                    {
                        sqlString = sqlString + 
                                "   and exists (select CD.Station_ID from tbCommodityData CD" +
			                    "                 where St.ID = CD.Station_ID" +
			                    "                 group by Station_ID)";
                    }


                }

                if(VisitedFilter == Program.enVisitedFilter.showOnlyVistedStations)                            
                {
                    // filter only visited stations
                    sqlString = sqlString + "   and St.visited <> 0";
                }
                else if(VisitedFilter == Program.enVisitedFilter.showOnlyVistedSystems)
                { 
                    // filter only visited systems
                    sqlString = sqlString + "   and Sy.visited <> 0";
                }

                if(Distance != null)                            
                {
                    sqlString = sqlString + String.Format(
                            "   and ((Sy.x <> 0.0 AND Sy.y <> 0.0 AND Sy.z <> 0.0) Or (Sy.Systemname = 'Sol'))" +
                            "   and sqrt(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)) <=  {3}",
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["x"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["y"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["z"]), 
                            ((Int32)Distance).ToString());
                }
                            
                if(DistanceToStar != null)                            
                {
                    if (Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "NoDistanceToStar", "consider").Equals("consider"))
                    {
                        sqlString = sqlString + String.Format(
                                "   and ((St.Distance_To_Star <= {0}) or (St.Distance_To_Star is null))",
                                ((Int32)DistanceToStar).ToString());
                    }
                    else
                    {
                        sqlString = sqlString + String.Format(
                                "   and St.Distance_To_Star <= {0}",
                                ((Int32)DistanceToStar).ToString());
                    }
                }

                if(minLandingPadSize != null)                            
                {
                    String LandingPadString = "";
                    Boolean consider = Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "NoLandingPadSize", "consider").Equals("consider");

                    switch (((String)minLandingPadSize).ToUpper())
                    {
                        case "S":
                            LandingPadString = "(St.max_landing_pad_size  = 'S') or " +
                                                "(St.max_landing_pad_size = 'M') or " +
                                                "(St.max_landing_pad_size = 'L')";
                            break;
                        case "M":
                            LandingPadString = "(St.max_landing_pad_size  = 'M') or " +
                                                "(St.max_landing_pad_size = 'L')";
                            break;
                        case "L":
                            LandingPadString = "(St.max_landing_pad_size  = 'L')";
                            break;
                    }

                    if(consider)
                        LandingPadString += " or  (St.max_landing_pad_size is null)";

                    sqlString = sqlString + String.Format(
                            "   and ({0})",
                            LandingPadString);
                }

                if(locationType != null)                            
                {
                    Boolean consider = Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "NoLocation", "consider").Equals("consider");

                    if(consider)
                    { 
                        if (((String)locationType).Equals("space"))
                        {
                            sqlString = sqlString + " and ((St.is_planetary is null) or (St.is_planetary = 0))";
                        }
                        else
                        {
                            sqlString = sqlString + " and ((St.is_planetary is null) or (St.is_planetary = 1))";
                        }
                    }
                    else
                    {
                        if (((String)locationType).Equals("space"))
                        {
                            sqlString = sqlString + " and St.is_planetary = 0";
                        }
                        else
                        {
                            sqlString = sqlString + " and St.is_planetary = 1";
                        }
                    }
                }

                sqlString += ";";

                m_lDBCon.Execute(sqlString);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating the base view", ex);
            }
        }

        /// <summary>
        /// calculating the best prices of all station for each commoditiy
        /// </summary>
        /// <param name="OnlyTradedCommodities"></param>
        /// <returns></returns>
        public DataTable getPriceExtremum(DataTable Result, Boolean OnlyTradedCommodities)
        {
            String sqlString;
            DataTable Data      = new DataTable();
            DataRow BuyMin;
            DataRow SellMax;
            DataRow lastCommodity;

            try
            {
                // gettin' some freaky performance
                m_lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                if(OnlyTradedCommodities)
                {
                    // only inquired/offered commodities
                    sqlString = "select Sy.ID As SystemID, Sy.Systemname, ST.id As StationID, ST.Stationname," +
                                "       FS.Distance," +
                                "       C.ID as CommodityID, C.Commodity, C.LocCommodity," +
                                "       nullif(CD.Buy, 0) As Buy, nullif(CD.Sell, 0) As Sell, CD.timestamp, CD.Sources_id, " +
                                "       nullif(xp.min_buy, 0) As min_buy, nullif(xp.max_sell, 0) As max_sell" +
                                "  from" +
                                "			tbCommodity C inner join (tbCommodityData CD, tmFilteredStations FS, tbStations St, tbSystems Sy," +
                                "						(select Commodity_id, max(nullif(Sell, 0)) as max_sell," +
                                "										      min(nullif(Buy, 0))  as min_buy" +
                                "						from tbCommodity C, tbCommodityData CD, tmFilteredStations FS" +
                                "						where FS.station_id = CD.Station_id" +
                                "						and   CD.Commodity_id = C.ID" +
                                "						group by Commodity_id) XP) " +
                                "           on (     C.ID             = CD.commodity_id" +
                                "                and FS.station_id    = CD.Station_id" +
                                "                and FS.station_id    = St.ID" +
                                "                and FS.system_id     = Sy.ID" +
                                "                and CD.Commodity_id  = XP.Commodity_id " +
                                "                and (    CD.buy      = XP.min_buy" +
                                "                      or CD.sell     = XP.max_sell))" +
                                " order by C.ID, FS.Distance";
                }
                else
                {
                    // all - also not inquired/offered - commodities
                    sqlString = "select Sy.ID As SystemID, Sy.Systemname, ST.id As StationID, ST.Stationname," +
                                "       FS.Distance," +
                                "       C.ID as CommodityID, C.Commodity, C.LocCommodity," +
                                "       nullif(CD.Buy, 0) As Buy, nullif(CD.Sell, 0) As Sell, CD.timestamp, CD.Sources_id, " +
                                "       nullif(xp.min_buy, 0) As min_buy, nullif(xp.max_sell, 0) As max_sell" +
                                "  from" +
                                "			tbCommodity C left join (tbCommodityData CD, tmFilteredStations FS, tbStations St, tbSystems Sy," +
                                "						(select Commodity_id, max(nullif(Sell, 0)) as max_sell," +
                                "										      min(nullif(Buy, 0))  as min_buy" +
                                "						from tbCommodity C, tbCommodityData CD, tmFilteredStations FS" +
                                "						where FS.station_id = CD.Station_id" +
                                "						and   CD.Commodity_id = C.ID" +
                                "						group by Commodity_id) XP) " +
                                "           on (     C.ID             = CD.commodity_id" +
                                "                and FS.station_id    = CD.Station_id" +
                                "                and FS.station_id    = St.ID" +
                                "                and FS.system_id     = Sy.ID" +
                                "                and CD.Commodity_id  = XP.Commodity_id " +
                                "                and (    CD.buy      = XP.min_buy" +
                                "                      or CD.sell     = XP.max_sell))" +
                                " order by C.ID, FS.Distance";
                }
  
                var oldTimeout = m_lDBCon.Connection.ConnectionTimeout;

                m_lDBCon.Execute(sqlString, Data);

                lastCommodity   = null;
                BuyMin          = null;
                SellMax         = null;
                Result.Clear();

                foreach (DataRow currentRow in Data.AsEnumerable())
                {
                    // Debug.Print((String)lastCommodity["CommodityID"] + " " + (String)currentRow["CommodityID"]);
                    // Debug.Print((String)currentRow["Locationname"]);

                    if((lastCommodity != null) && ((Int32)lastCommodity["CommodityID"] != (Int32)currentRow["CommodityID"]))
                    {
                        // Debug.Print((String)currentRow["LocCommodity"]);

                        // next commodity found, save result 
                        var newRow = (dsEliteDB.tmpa_allcommoditiesRow)Result.NewRow();
                        newRow.CommodityID      = (Int32)lastCommodity["CommodityID"];
                        newRow.Commodity        = (String)lastCommodity["LocCommodity"];

                        if (BuyMin != null)
                        {
                            newRow.Buy_SystemID     = (Int32)BuyMin["SystemID"];
                            newRow.Buy_System       = (String)BuyMin["Systemname"];
                            newRow.Buy_StationID    = (Int32)BuyMin["StationID"];
                            newRow.Buy_Station      = (String)BuyMin["Stationname"];
                            newRow.Buy_Min          = (Int32)(Int64)BuyMin["Buy"];
                            newRow.Buy_Distance     = (Double)BuyMin["Distance"];
                            newRow.Buy_Timestamp    = (DateTime)BuyMin["timestamp"];
                            newRow.Buy_Sources_id   = (Int32)BuyMin["Sources_id"];
                        }

                        if (SellMax != null)
                        {
                            newRow.Buy_SystemID     = (Int32)SellMax["SystemID"];
                            newRow.Sell_System      = (String)SellMax["Systemname"];
                            newRow.Sell_StationID   = (Int32)SellMax["StationID"];
                            newRow.Sell_Station     = (String)SellMax["Stationname"];
                            newRow.Sell_Max         = (Int32)(Int64)SellMax["Sell"];
                            newRow.Sell_Distance    = (Double)SellMax["Distance"];
                            newRow.Sell_Timestamp   = (DateTime)SellMax["timestamp"];
                            newRow.Sell_Sources_id  = (Int32)SellMax["Sources_id"];
                        }

                        if ((BuyMin != null) && (SellMax != null))
                            newRow.Max_Profit = newRow.Sell_Max - newRow.Buy_Min;
                        

                        Result.Rows.Add(newRow);

                        BuyMin  = null;
                        SellMax = null;
                    }

                    lastCommodity = currentRow;

                    if(!Convert.IsDBNull(currentRow["Buy"])  && ((BuyMin == null)  || ((Int64)currentRow["Buy"]  < (Int64)BuyMin["Buy"])))
                    { 
                        //if(BuyMin != null)
                        //    Debug.Print("Buy " + BuyMin["Buy"] + " -> " + currentRow["Buy"]);
                        //else
                        //    Debug.Print("Buy " + "--- -> " + currentRow["Buy"]);

                        BuyMin  = currentRow;
                    }

                    if(!Convert.IsDBNull(currentRow["Sell"]) && ((SellMax == null) || ((Int64)currentRow["Sell"] > (Int64)SellMax["Sell"])))
                    {
                        //if(SellMax != null)
                        //    Debug.Print("Sell " + SellMax["Sell"] + " -> " + currentRow["Sell"]);
                        //else
                        //    Debug.Print("Sell " + "--- -> " + currentRow["Sell"]);
                        
                        SellMax  = currentRow;
                    }
                }

                m_lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                return Result;
            }
            catch (Exception ex)
            {
                m_lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                throw new Exception("Error while getting the best market prices", ex);
            }
        }

        /// <summary>
        /// calculating the best trading routes
        /// </summary>
        /// <returns></returns>
        public DataTable calculateTradingRoutes()
        {
            String sqlBaseString;
            String sqlBaseString2;
            String sqlString;
            String sqlLocationString;
            DataSet Data            = new DataSet();
            var tmNeighbourstations  = new dsEliteDB.tmneighbourstationsDataTable();
            var tmFilteredStations  = new dsEliteDB.tmfilteredstationsDataTable();
            HashSet<String> Calculated = new HashSet<String>();
            Int32 StationCount;
            Int32 SystemCount;
            Int32 Current = 0;
            ProgressView PV;
            Boolean Cancelled = false;
            Int32 maxTradingDistance;
            dsEliteDB.tmpa_s2s_besttripsDataTable Result;
            DateTime stopTime;

            try
            {

                Debug.Print("start :" + DateTime.Now.ToShortTimeString());
                stopTime = (DateTime.Now - new TimeSpan(Program.DBCon.getIniValue<Int32>(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "TimeFilterDays", "30", true), 0, 0, 0));

                // gettin' some freaky performance
                m_lDBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                getFilteredSystemAndStationCount(out StationCount, out SystemCount);


                // get the results for a cancellable loop
                if(FixedStation == 0)
                { 
                    sqlString = "select * from tmfilteredstations" +
                                " order by Station_ID";
                }
                else
                { 
                    sqlString = String.Format("select * from tmfilteredstations" +
                                              " where Station_ID = {0}" +
                                              " order by Station_ID",
                                              FixedStation);
                }

                m_lDBCon.Execute(sqlString, tmFilteredStations);

                maxTradingDistance = -1;
                if(m_lDBCon.getIniValue<Boolean>(tabPriceAnalysis.DB_GROUPNAME, "MaxTripDistance"))
                    maxTradingDistance = m_lDBCon.getIniValue<Int32>(tabPriceAnalysis.DB_GROUPNAME, "MaxTripDistanceValue");

                // delete old content
                sqlString = "truncate table tmNeighbourstations;";
                m_lDBCon.Execute(sqlString);

                sqlBaseString =  "insert into tmNeighbourstations(System_ID_From, Station_ID_From, Distance_From," +
                                 "                                System_ID_To, Station_ID_To, Distance_To, " +
                                 "                                Distance_Between) " +
                                 " select BS.System_ID As System_ID_From, BS.Station_ID As Station_ID_From, BS.Distance As Distance_From," +
                                 "        FS.System_ID As System_ID_To, FS.Station_ID As Station_ID_To, FS.Distance As Distance_To," + 
                                 "        sqrt(POW(FS.x - BS.x, 2) + POW(FS.y - BS.y, 2) +  POW(FS.z - BS.z, 2)) as Distance_Between" +
                                 " from (select * from tmFilteredStations " + 
                                 "         where Station_ID = {0}" + 
                                 "         order by System_ID, Station_ID) BS"; 
                if(maxTradingDistance >= 0)
                    sqlBaseString += " join tmfilteredstations FS on (sqrt(POW(FS.x - BS.x, 2) + POW(FS.y - BS.y, 2) +  POW(FS.z - BS.z, 2)) <=  {1})";
                else
                    sqlBaseString += " join tmfilteredstations FS";

                sqlBaseString += " join tbStations St on FS.Station_ID = St.ID" +
                                 " join tbSystems Sy on St.System_ID  = Sy.ID" +
                                 " having  BS.Station_ID <> FS.Station_ID;";

                PV = new ProgressView(this.GUI);
                Current = 0;

                if(maxTradingDistance >= 0)
                    PV.progressStart("determine neigbour-systems in range of "+ maxTradingDistance + " ly of " + SystemCount + " selected systems...");
                else
                    PV.progressStart("determine neigbour-systems without range limit of " + SystemCount + " selected systems...");

                foreach (dsEliteDB.tmfilteredstationsRow CurrentStation in tmFilteredStations)
                {
                    // preparing a table with the stations from "tmFilteredStations" and all 
                    // their neighbour stations who are not further away than the max trading distance
                    
                    if((CurrentStation.System_id == 12761) || (CurrentStation.System_id == 19737))
                        Debug.Print("Stop");
                    
                    if(maxTradingDistance >= 0)
                        sqlString = String.Format(sqlBaseString, CurrentStation.Station_id, maxTradingDistance);
                    else
                        sqlString = String.Format(sqlBaseString, CurrentStation.Station_id);

                    m_lDBCon.Execute(sqlString);
                        
                    Current += 1;
                    PV.progressUpdate(Current, tmFilteredStations.Rows.Count);

                    if(PV.Cancelled)
                    {
                        Cancelled = true;
                        break;
                    }
                }

                PV.progressStop();

                // get for one station and all of it's neighbours the tradings for all commodity combinations
                // result gives per "station to station" route only the one best profit for all combinations of commodities
                sqlBaseString = "insert ignore into tmBestProfits(Station_ID_From, Station_ID_To, Max_Profit)" +
                                " select Station_ID_From, Station_ID_To, max(Profit) As Max_Profit from " +
                                " (select PR1.Station_ID_From, PR1.Station_ID_To, Pr1.Forward, Pr2.Back, (ifnull(Pr1.Forward, 0) + ifnull(Pr2.Back,0)) As Profit  from  " +
                                " 	(select L1.*, if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, (nullif(L2.Sell,0) - nullif(L1.Buy,0)), null) As Forward,  " +
                                " 			if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, (nullif(L1.Sell,0) - nullif(L2.Buy,0)), null) As Back  " +
                                " 	from          (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell  " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on  N.Station_ID_From = CD.Station_ID " +
                                " 																		  and N.Station_ID_From = {0} {2}" +
                                " 				   ) L1  " +
                                " 	 join " +
                                " 				  (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on N.Station_ID_To = CD.Station_ID " +
                                " 																		  and N.Station_ID_From = {0} {2}" +
                                " 				   ) L2 " +
                                " 	on  L1.Station_ID_From = L2.Station_ID_From " +
                                " 	and L1.Station_ID_To   = L2.Station_ID_To " +
                                " 	and L1.Commodity_ID    = L2.Commodity_ID" +
                                "   {4}) Pr1 " +
                                "      " +
                                "     join  " +
                                "  " +
                                " 	(select L1.*, if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, (nullif(L2.Sell,0) - nullif(L1.Buy,0)), null) As Forward,  " +
                                " 			if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, (nullif(L1.Sell,0) - nullif(L2.Buy,0)), null) As Back  " +
                                " 	from          (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell  " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on  N.Station_ID_From = CD.Station_ID " +
                                " 																		  and N.Station_ID_From = {0} {3}" +
                                " 				   ) L1  " +
                                " 	 join " +
                                " 				  (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on N.Station_ID_To = CD.Station_ID " +
                                " 																		  and N.Station_ID_From = {0} {3}" +
                                " 				   ) L2 " +
                                " 	on  L1.Station_ID_From = L2.Station_ID_From " +
                                " 	and L1.Station_ID_To   = L2.Station_ID_To " +
                                " 	and L1.Commodity_ID    = L2.Commodity_ID" +
                                "   {5}) Pr2 " +
                                "      " +
                                " on  Pr1.Station_ID_From = Pr2.Station_ID_From " +
                                " and Pr1.Station_ID_To   = Pr2.Station_ID_To) ALL_RESULTS " +
                                " where Profit > {1}" +
                                " group by Station_ID_From, Station_ID_To;" + 
                                " " +
                                "delete BP1 from tmBestProfits BP1, (select Max_Profit from tmBestProfits" +
                                "                                     order by Max_Profit desc" +
                                "                                     limit 100,1) BP2" +
                                " where BP1.Max_Profit < BP2.Max_Profit;" +  
                                " " +
                                "select Max_Profit As Min_Profit from tmBestProfits" +
                                " order by Max_Profit desc" +
                                " limit 100,1;";

                String wherePart_Return         = "";
                String wherePart_Send           = "";
                String havingPart_Return        = "";
                String havingPart_Send          = "";


                // time filter         
                if(Program.DBCon.getIniValue<Boolean>(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "TimeFilter", false.ToString(), true))
                {
                    wherePart_Send   = " where CD.timestamp >= " + DBConnector.SQLDateTime(stopTime);
                    wherePart_Return = " where CD.timestamp >= " + DBConnector.SQLDateTime(stopTime);
                }

                // commodity filter
                if(CommoditiesSend.Count > 0)
                {
                    if(String.IsNullOrEmpty(wherePart_Send))
                        wherePart_Send  = " where " + DBConnector.GetString_Or<Int32>("CD.Commodity_ID", CommoditiesSend);
                    else
                        wherePart_Send += " and " + DBConnector.GetString_Or<Int32>("CD.Commodity_ID", CommoditiesSend);

                    havingPart_Send = " having Forward > 0 ";
                }

                if(CommoditiesReturn.Count > 0)
                {
                    if(String.IsNullOrEmpty(wherePart_Return))
                        wherePart_Return  = " where " + DBConnector.GetString_Or<Int32>("CD.Commodity_ID", CommoditiesReturn);
                    else
                        wherePart_Return += " and " + DBConnector.GetString_Or<Int32>("CD.Commodity_ID", CommoditiesReturn);

                    havingPart_Return = " having Back > 0 ";
                }

                if(!Cancelled)
                {
                    Current     = 0;
                    Calculated.Clear();

                    m_lDBCon.Execute("truncate table tmBestProfits");

                    // get the start stations for a cancellable loop
                    sqlString = "select Station_ID_From, count(*) As Neighbours from tmNeighbourstations" +
                                " group by Station_ID_From" +
                                " order by Station_ID_From";
                    m_lDBCon.Execute(sqlString, "StartStations", Data);

                    PV = new ProgressView(this.GUI);

                    if(maxTradingDistance >= 0)
                        PV.progressStart("Processing market data of " + StationCount + " stations from " + SystemCount + " systems\n" +
                                            "(max. trading distance = " + maxTradingDistance + " ly)...");
                    else
                        PV.progressStart("Processing market data of " + StationCount + " stations from " + SystemCount + " systems\n" +
                                            "(no trading distance limit)...");

                    Int32 currentMinValue = 0;

                    foreach(DataRow StartStation in Data.Tables["StartStations"].Rows)
                    {
                        sqlString = String.Format(sqlBaseString, StartStation["Station_ID_From"], currentMinValue, wherePart_Send, wherePart_Return, havingPart_Send, havingPart_Return);


                        m_lDBCon.Execute(sqlString, "MinProfit", Data);

                        if((Data.Tables["minProfit"].Rows.Count > 0) && (!Convert.IsDBNull(Data.Tables["MinProfit"])))
                            currentMinValue = (Int32)Data.Tables["MinProfit"].Rows[0]["Min_Profit"];
                        Clipboard.SetText(sqlString);
                        Current += 1;

                        PV.progressUpdate(Current,  Data.Tables["StartStations"].Rows.Count);

                        Data.Tables["MinProfit"].Clear();

                        if(PV.Cancelled)
                        {
                            Cancelled = true;
                            break;
                        }
                    }


                    m_lDBCon.Execute("truncate table tmPA_S2S_BestTrips");

                    sqlString = "create temporary table tmpForDelete As" +
                                " (select BP1.Station_Id_From, BP1.Station_Id_To from tmBestProfits BP1" +
                                " 	where exists (select * from tmBestProfits BP2 " +
                                " 					where BP1.Station_Id_From = BP2.Station_Id_To " +
                                " 					and BP1.Station_Id_To     = BP2.Station_Id_From " +
                                " 					and BP1.Max_Profit        = BP1.Max_Profit" +
                                " 					and BP1.Station_Id_From   > BP2.Station_Id_From));" +
                                
                                "delete BP1 from tmBestProfits BP1, tmpForDelete T" +
                                " where BP1.Station_Id_From = T.Station_Id_From" +
                                " and   BP1.Station_Id_To   = T.Station_Id_To;" +

                                "drop temporary table tmpForDelete;" ;

                    m_lDBCon.Execute(sqlString);

                    Result = new dsEliteDB.tmpa_s2s_besttripsDataTable();

                    PV.progressStop();

                }


                m_lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                sqlString = "select distinct St1.*, St2.*, Bp.Max_Profit As Profit, NS.Distance_Between As Distance," +
                            "                null As TimeStamp_1, null As TimeStamp_2 from tmBestProfits Bp" +
                            " " +
                            " join    " +
                            " " +
                            " (select Sy.ID As System_ID_1, Sy.SystemName As SystemName_1, St.ID As Station_ID_1, St.StationName As StationName_1, " +
                            "         St.distance_to_star As DistanceToStar_1 from tmBestProfits BP, tbStations St, tbSystems Sy" +
                            "   where BP.Station_ID_From 	    = St.ID " +
                            "   and   St.System_ID 			= Sy.ID) St1" +
                            "   " +
                            " on    St1.Station_ID_1         = BP.Station_ID_From" +
                            "   " +
                            " join    " +
                            " " +
                            " (select Sy.ID As System_ID_2, Sy.SystemName As SystemName_2, St.ID As Station_ID_2, St.StationName As StationName_2, " +
                            "         St.distance_to_star As DistanceToStar_2 from tmBestProfits BP, tbStations St, tbSystems Sy" +
                            "   where BP.Station_ID_To 	    = St.ID " +
                            "   and   St.System_ID 			= Sy.ID) St2" +
                            "   " +
                            " on    St2.Station_ID_2         = BP.Station_ID_To" +
                            " join " +
                            "  tmneighbourstations NS" +
                            " on  St1.Station_ID_1 = NS.Station_Id_From " +
                            " and St2.Station_ID_2 = NS.Station_Id_To" +
                            " " +
                            " order by Max_Profit desc";

                Result = new dsEliteDB.tmpa_s2s_besttripsDataTable();
                m_lDBCon.Execute(sqlString, Result);

                sqlBaseString = "select PR1.Commodity_ID As FWCommodityID, Pr1.LocCommodity As FWCommodity," +
                                "	   PR1.timestamp    As FWTimeStamp," +
                                "       PR2.Commodity_ID As BkCommodityID, Pr2.LocCommodity As BkCommodity,  " +
                                "	   PR2.timestamp    As BkTimeStamp," +
                                "       Pr1.Forward, Pr2.Back, (ifnull(Pr1.Forward, 0) + ifnull(Pr2.Back,0)) As Profit  from " +
                                " (select L1.Commodity_ID, T.LocCommodity, " +
                                "		if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, (nullif(L2.Sell,0) - nullif(L1.Buy,0)), null) As Forward, " +
                                "		if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, (nullif(L1.Sell,0) - nullif(L2.Buy,0)), null) As Back," +
                                "        if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, L1.timestamp, if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, L2.timestamp, null)) As timestamp" +
                                "	from (select CD.Commodity_ID, CD.Buy, CD.Sell, CD.timestamp " +
                                "				 from tbCommodityData CD " +
                                "                 where CD.Station_ID      = {0} {4}" +
                                "		  ) L1 " +
                                "	 join" +
                                "		 (select CD.Commodity_ID, CD.Buy, CD.Sell, CD.timestamp" +
                                "				 from tbCommodityData CD " +
                                "                 where CD.Station_ID      = {1} {4}" +
                                "		  ) L2" +
                                "	on  L1.Commodity_ID    = L2.Commodity_ID" +
                                "    join tbCommodity T" +
                                "    on  L1.Commodity_ID    = T.ID" +
                                "    ) Pr1" +
                                " join " +
                                " (select L1.Commodity_ID, T.LocCommodity," +
                                "		if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, (nullif(L2.Sell,0) - nullif(L1.Buy,0)), null) As Forward, " +
                                "		if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, (nullif(L1.Sell,0) - nullif(L2.Buy,0)), null) As Back," +
                                "        if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, L1.timestamp, if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, L2.timestamp, null)) As timestamp" +
                                "	from (select CD.Commodity_ID, CD.Buy, CD.Sell, CD.timestamp " +
                                "				 from tbCommodityData CD " +
                                "                 where CD.Station_ID      = {0} {3}" +
                                "		  ) L1 " +
                                "	 join" +
                                "		 (select CD.Commodity_ID, CD.Buy, CD.Sell, CD.timestamp" +
                                "				 from tbCommodityData CD " +
                                "                 where CD.Station_ID      = {1} {3}" +
                                "		  ) L2" +
                                "	on  L1.Commodity_ID    = L2.Commodity_ID" +
                                "    join tbCommodity T" +
                                "    on  L1.Commodity_ID    = T.ID" +
                                "    ) Pr2" +
                                "    order by Profit desc {2}";
                
                sqlBaseString2 = "select distance from tmFilteredStations" +
                                 " where (Station_id = {0}" +
                                 "    or  Station_id = {1})" +
                                 " order by distance limit 1";


                sqlLocationString = "select st1.is_planetary As FW_is_planetary, st2.is_planetary As BW_is_planetary from " +
                                    " (select is_planetary from tbstations" +
                                    " where ID = {0}) as st1 ," +
                                    " (select is_planetary from tbstations" +
                                    " where ID = {1}) as st2;";

                wherePart_Send   = "";
                wherePart_Return = "";

                // time filter         
                if(Program.DBCon.getIniValue<Boolean>(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "TimeFilter", false.ToString(), true))
                {
                    wherePart_Send   = " and CD.timestamp >= " + DBConnector.SQLDateTime(stopTime);
                    wherePart_Return = " and CD.timestamp >= " + DBConnector.SQLDateTime(stopTime);
                }

                // commodity filter
                if(CommoditiesSend.Count > 0)
                {
                    wherePart_Send += " and " + DBConnector.GetString_Or<Int32>("CD.Commodity_ID", CommoditiesSend);
                }

                if(CommoditiesReturn.Count > 0)
                {
                    wherePart_Return += " and " + DBConnector.GetString_Or<Int32>("CD.Commodity_ID", CommoditiesReturn);
                }

                // now get the timestamps of the best-profit commodities and get other details
                foreach (dsEliteDB.tmpa_s2s_besttripsRow CurrentRow in Result)
                {
                    sqlString = String.Format(sqlBaseString, CurrentRow.Station_ID_1, CurrentRow.Station_ID_2, "limit 1", wherePart_Send, wherePart_Return);
                    m_lDBCon.Execute(sqlString, "Timestamps", Data);

                    if(!DBNull.Value.Equals(Data.Tables["Timestamps"].Rows[0]["FWTimeStamp"]))
                        CurrentRow.TimeStamp_1 = (DateTime)Data.Tables["Timestamps"].Rows[0]["FWTimeStamp"];

                    if(!DBNull.Value.Equals(Data.Tables["Timestamps"].Rows[0]["BkTimeStamp"]))
                        CurrentRow.TimeStamp_2 = (DateTime)Data.Tables["Timestamps"].Rows[0]["BkTimeStamp"];

                    // distance
                    sqlString = String.Format(sqlBaseString2, CurrentRow.Station_ID_1, CurrentRow.Station_ID_2);
                    m_lDBCon.Execute(sqlString, "Distance", Data);

                    if((Data.Tables["Distance"].Rows.Count > 0) && (!DBNull.Value.Equals(Data.Tables["Distance"].Rows[0]["distance"])))
                        CurrentRow.DistanceToRoute = (Double)Data.Tables["Distance"].Rows[0]["distance"];
                    else
                        CurrentRow.DistanceToRoute = double.NaN;

                    // location
                    sqlString = String.Format(sqlLocationString, CurrentRow.Station_ID_1, CurrentRow.Station_ID_2);
                    m_lDBCon.Execute(sqlString, "Location", Data);

                    if(!DBNull.Value.Equals(Data.Tables["Location"].Rows[0]["FW_is_planetary"]))
                        if(((Boolean)Data.Tables["Location"].Rows[0]["FW_is_planetary"]))
                            CurrentRow.Station_Location_1 = "P";
                        else
                            CurrentRow.Station_Location_1 = "S";

                    if(!DBNull.Value.Equals(Data.Tables["Location"].Rows[0]["BW_is_planetary"]))
                        if(((Boolean)Data.Tables["Location"].Rows[0]["BW_is_planetary"]))
                            CurrentRow.Station_Location_2 = "P";
                        else
                            CurrentRow.Station_Location_2 = "S";

                    Data.Tables["Timestamps"].Clear();
                    Data.Tables["Distance"].Clear();
                    Data.Tables["Location"].Clear();
                }
                
                Debug.Print("Ende :" + DateTime.Now.ToShortTimeString());


                return Result;

	        }
	        catch (Exception ex)
	        {
                m_lDBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

		        throw new Exception("Error while calculating possible trading routes", ex);
	        }
        }

        /// <summary>
        /// gets the number of filtered systems and stations from the table "tmFilteredStations"
        /// </summary>
        /// <param name="StationCount"></param>
        /// <param name="SystemCount"></param>
        public void getFilteredSystemAndStationCount(out Int32 StationCount, out Int32 SystemCount)
        {
            DataTable Data = new DataTable();
            String sqlString;

            try
            {
                // get amount of data for viewing
                sqlString = "select count(distinct(System_id)) As Systems, count(*) As Stations" +
                            " from tmFilteredStations";
                m_lDBCon.Execute(sqlString, Data);

                SystemCount  = (Int32)(Int64)Data.Rows[0]["Systems"];
                StationCount = (Int32)(Int64)Data.Rows[0]["Stations"];
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the number of filtered systems and stations", ex);
            }
        }

        /// <summary>
        /// loads all possible trading data from one to another station (one direction)
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Station_From"></param>
        /// <param name="Station_To"></param>
        public void loadBestProfitStationCommodities(dsEliteDB.tmpa_s2s_stationdataDataTable Data, int? Station_From, int? Station_To, List<Int32> commodityFilter)
        {
            String sqlString;
            String orString = "";

            try
            {
                if ((Station_From == null) || (Station_To == null))
                {
                    // if a id equals null it will result in a exception from mysql:
                    // "There is already an open DataReader associated with this Connection which must be closed first."
                    Data.Clear();
                }
                else
                {
                    if(commodityFilter.Count > 0)
                        orString = " and " + DBConnector.GetString_Or<Int32>("commodity_id", commodityFilter);

                    sqlString = String.Format(
                                "select Sd1.Station_ID, Sd1.Commodity_Id, Cm.LocCommodity As Commodity, " +
                                "       Sd1.Buy, Sd1.Supply, Sd1.SupplyLevel, Sd1.timestamp As Timestamp1, " +
                                "       Sd2.Sell, Sd2.Demand, Sd2.DemandLevel, Sd2.timestamp As Timestamp2, " +
                                "       (nullif(Sd2.Sell, 0) - nullif(Sd1.Buy,0)) As Profit, Sd1.Sources_ID from " +
                                " (select * from tbCommodityData " +
                                "   where Station_ID   = {0} " + 
                                    orString + ") Sd1 " +
                                " join" +
                                " (select * from tbCommodityData " +
                                "   where Station_ID   = {1}) Sd2" +
                                "   on Sd1.Commodity_ID = Sd2.Commodity_ID" +
                                " join" +
                                " tbCommodity Cm" +
                                "   on Sd1.Commodity_ID = Cm.ID" +
                                "   having Profit is not null" +
                                "   order by Profit Desc;", Station_From.ToNString("null"), Station_To.ToNString("null"));

                    m_lDBCon.Execute(sqlString, Data);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading trading data", ex);
            }

        }

        /// <summary>
        /// loads all possible trading data from one to another station (one direction)
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Station_From"></param>
        /// <param name="Station_To"></param>
        public void loadCommoditiesByStation(DataTable Data, int? Station)
        {
            String sqlString;
            try
            {
                sqlString = String.Format("select Cd.Commodity_ID, Co.LocCommodity As Commodity, " +
                                          "        Cd.Buy, Cd.Supply, Cd.SupplyLevel, " +
                                          "        Cd.Sell, Cd.Demand, Cd.DemandLevel, " +
                                          "        Cd.Timestamp, BB.Best_Buy, BS.Best_Sell, " +
                                          "        (BS.Best_Sell - BB.Best_Buy) As MaxProfit, Cd.Sources_id " +
                                          " from tbStations St, tbCommodity Co, tbCommodityData Cd  " +
                                          "   join  " +
                                          " 	   (select Cd.Commodity_ID, Min(nullif(Cd.Buy, 0)) As Best_Buy  " +
                                          "           from tbCommodityData Cd, tmfilteredstations Fi  " +
                                          " 	      where Fi.Station_ID = Cd.station_id " +
                                          " 	     group by Cd.Commodity_ID) BB " +
                                          "   on Cd.Commodity_ID = BB.Commodity_ID " +
                                          "   join  " +
                                          " 	   (select Cd.Commodity_ID, Max(nullif(Cd.Sell, 0)) As Best_Sell " +
                                          "           from tbCommodityData Cd, tmfilteredstations Fi  " +
                                          " 	      where Fi.Station_ID = Cd.station_id " +
                                          " 	     group by Cd.Commodity_ID) BS " +
                                          "   on Cd.Commodity_ID = BS.Commodity_ID " +
                                          " where St.ID 		   = Cd.Station_ID " +
                                          " and   Cd.Commodity_ID 	= Co.ID " +
                                          " and St.ID = {0}", Station);
  
                m_lDBCon.Execute(sqlString, Data);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading station data", ex);
            }
        }

        /// <summary>
        /// load all stationdata for a specific commodity
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Commodity_ID"></param>
        internal void loadStationsByCommodity(DataTable Data, int? Commodity_ID)
        {
            String sqlString;
            try
            {
                sqlString = String.Format("select Sy.ID As System_ID, Sy.Systemname As System, St.ID As Station_ID, St.Stationname As Station, Fi.Distance," +
                                          "        nullif(Cd.Buy,0) As Buy, nullif(Cd.Supply,0) As Supply, Cd.SupplyLevel, " +
                                          "        nullif(Cd.Sell,0) As Sell, nullif(Cd.Demand,0) As Demand, CD.DemandLevel, CD.Timestamp, Cd.Sources_id" +
                                          " from tbCommodityData Cd, tbSystems Sy, tbStations St, tmfilteredstations Fi" +
                                          " where Cd.Station_ID   = St.ID" +
                                          " and   St.System_ID    = Sy.ID" +
                                          " and   St.ID           = Fi.station_id" +
                                          " and   Cd.Commodity_ID = {0}", Commodity_ID);
  
                m_lDBCon.Execute(sqlString, Data);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading commodity data", ex);
            }
        }
        
        /// <summary>
        /// loads the list of all known commodities into the table
        /// </summary>
        /// <param name="Data"></param>
        internal void loadCommodities(DataTable Data)
        {
            String sqlString;
            try
            {
                sqlString = String.Format("select Id, LocCommodity As Commodity" + 
                                          " from tbCommodity");
  
                m_lDBCon.Execute(sqlString, Data);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading list of all commodities", ex);
            }
        }

        ///// <summary>
        ///// loads as list of all systems
        ///// </summary>
        ///// <param name="Data"></param>
        //internal void loadSystems(DataTable Data, Program.enVisitedFilter VisitedFilter)
        //{
        //    String sqlString;

        //    try
        //    {

        //        if(VisitedFilter != Program.enVisitedFilter.showAll)
        //        {
        //            // only visited systems
        //            sqlString = " select 0 As SystemID, '" + tabPriceAnalysis.CURRENT_SYSTEM + "' As SystemName" +
        //                        " union " +
        //                        " (select Sy.ID As SystemID, Sy.SystemName" +
        //                        " from tbSystems Sy" +
        //                        " where Sy.Visited   = 1" +
        //                        " order by SystemName)" ;
        //        }
        //        else
        //        {
        //            // all systems
        //            sqlString = " select 0 As SystemID, '" + tabPriceAnalysis.CURRENT_SYSTEM + "' As SystemName" +
        //                        " union " +
        //                        " (select Sy.ID As SystemID, Sy.SystemName" +
        //                        " from tbSystems Sy" +
        //                        " order by SystemName)" ;
        //        }

        //        m_lDBCon.Execute(sqlString, Data);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while loading systems", ex);
        //    }
        //}

        
        /// <summary>
        /// loads only the systems matching the current input
        /// </summary>
        /// <param name="systemString"></param>
        /// <param name="cBox"></param>
        internal void LoadSystemsForBaseComboBox(string systemString, DataTable currentDt, Program.enVisitedFilter vFilter)
        {
            String sqlString = "";

            try
            {
                if (systemString != "")
                {
                    if(vFilter != Program.enVisitedFilter.showAll)
                    {
                        // only visited systems
                        sqlString = " (select 0 As SystemID, '<current system>' As Systemname)" +
                                    "  union " +
                                    " (select Sy.ID As SystemID, Sy.SystemName" +
                                    "  from tbSystems Sy" +
                                    "  where Sy.Visited   = 1" +
                                    "  and   Sy.SystemName like " + DBConnector.SQLAString(DBConnector.SQLEscape(systemString) + "%") +
                                    "  order by SystemName)" ;
                    }
                    else
                    {
                        // all systems
                        sqlString = "(select 0 As SystemID, '<current system>' As Systemname)" +
                                    " union " +
                                    "(select Sy.ID As SystemID, Sy.SystemName" +
                                    " from tbSystems Sy" +
                                    " where Sy.SystemName like " + DBConnector.SQLAString(DBConnector.SQLEscape(systemString) + "%") +
                                    " order by SystemName)" ;
                    }

                    Program.DBCon.Execute(sqlString, currentDt);
                }
                else if(currentDt.Columns.Count == 0)
                {
                    // init datatable to be able to set ValueMember and DisplayMember in the Combobox
                    sqlString = " select 0 As SystemID, '<current system>' As Systemname";
                    Program.DBCon.Execute(sqlString, currentDt);
                }
                else
                {
                    currentDt.Clear();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading specific stationdata for basetable", ex);
            }
        }
    }

#region outdated 

    /// <summary>
    /// class only for compatibilty reasons at the moment,
    /// will later removed
    /// </summary>

    [Serializable]
    public class PriceAnalysisEvent
    {
        public DateTime EventDate   { get; set; }
        public string   EventType   { get; set; }
        public string   Station     { get; set; }
        public string   System      { get; set; }
        public string   Cargo       { get; set; }
        public string   CargoAction { get; set; }
        public decimal  CargoVolume { get; set; }
        public string   Notes       { get; set; }
        public string   EventID     { get; set; }
        public decimal  TransactionAmount { get; set; }
        public decimal  Credits     { get; set; }
    }



#endregion

}

