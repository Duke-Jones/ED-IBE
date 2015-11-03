using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using RegulatedNoise.Enums_and_Utility_Classes;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using RegulatedNoise.SQL;
using RegulatedNoise.SQL.Datasets;
using System.Collections.Generic;

namespace RegulatedNoise.MTPriceAnalysis
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

        private const String table = "tbLog";

        /// <summary>
        /// main selection string for the data from the database
        /// </summary>
        // private const String _sqlString = "select L.time, S.systemname, St.stationname, E.event As eevent, C.action," + 
        //                                   "       Co.loccommodity, L.cargovolume, L.credits_transaction, L.credits_total, L.notes" +
        //                                   " from tbLog L left join tbEventType E   on L.event_id       = E.id" + 
        //                                   "              left join tbCargoAction C on L.cargoaction_id = C.id" +
        //                                   "              left join tbSystems S     on L.system_id      = S.id" +
        //                                   "              left join tbStations St   on L.station_id     = St.id" +
        //                                   "              left join tbCommodity Co  on L.commodity_id   = Co.id";
        //
        // ^^^^^^^^^^ replaced by view "viLog" vvvvvvvvvvvvvvv
        private const String _sqlString = "select * from viLog";

        private dsEliteDB           m_BaseData;
        public tabPriceAnalysis     m_GUI;
        private BindingSource       m_BindingSource;
        private DataTable           m_Datatable;
        private DataRetriever       retriever;
        private Boolean             m_NoGuiNotifyAfterSave;

        /// <summary>
        /// constructor
        /// </summary>
        public PriceAnalysis()
        {
            try
            {
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
                retriever = new DataRetriever(Program.DBCon, table, _sqlString, "time", DBConnector.SQLSortOrder.desc, new dsEliteDB.vilogDataTable());

                return retriever.RowCount;
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
        public void createFilteredTable(Int32 SystemID, Object Distance, Object DistanceToStar, Object minLandingPadSize, Program.enVisitedFilter VisitedFilter)
        {
            String sqlString;
            DataTable currentSystem = new DataTable();

            try
            {
                // get info of basesystem
                sqlString = "select * from tbSystems where ID = " + SystemID.ToString();
                Program.DBCon.Execute(sqlString, currentSystem);

                sqlString = "delete from tmFilteredStations;";
                Program.DBCon.Execute(sqlString);

                sqlString = String.Format(
                            "insert into tmFilteredStations(System_id, Station_id, Distance, x, y, z) " +
                            "   select Sy.ID As System_id, St.ID As Station_id, SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)) As Distance, Sy.x, Sy.x, Sy.z" + 
                            "   from tbSystems Sy, tbStations St" +
                            "   where Sy.ID = St.System_id",
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["x"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["y"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["z"])); 

                if(true)                            
                {
                    // filter only stations with commodities
                    sqlString = sqlString + 
                            "   and exists (select CD.Station_ID from tbCommodityData CD" +
			                "                 where St.ID = CD.Station_ID" +
			                "                 group by STation_ID)";
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
                            "   and ((Sy.x <> 0.0 AND Sy.y <> 0.0 AND Sy.Z <> 0.0) Or (Sy.Systemname = 'Sol'))" +
                            "   and sqrt(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)) <=  {3}",
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["x"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["y"]), 
                            DBConnector.SQLDecimal((Double)currentSystem.Rows[0]["z"]), 
                            ((Int32)Distance).ToString());
                }
                            
                if(DistanceToStar != null)                            
                {
                    sqlString = sqlString + String.Format(
                            "   and St.Distance_To_Star <= {0}",
                            ((Int32)DistanceToStar).ToString());

                }

                if(minLandingPadSize != null)                            
                {
                    sqlString = sqlString + String.Format(
                            "   and St.max_landing_pad_size = '{0}'",
                            (String)minLandingPadSize);
                }

                sqlString += ";";

                Program.DBCon.Execute(sqlString);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating the base view", ex);
            }
        }

        public DataTable getPriceExtremum(Boolean OnlyTradedCommodities)
        {
            String sqlString;
            DataTable Data      = new DataTable();
            var Result          = new dsEliteDB.tmpa_allcommoditiesDataTable();
            DataRow BuyMin;
            DataRow SellMax;
            DataRow lastCommodity;

            try
            {
                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                if(OnlyTradedCommodities)
                {
                    // only inquired/offered commodities
                    sqlString = "select Sy.ID As SystemID, Sy.Systemname, ST.id As StationID, ST.Stationname," +
                                "       FS.Distance," +
                                "       C.ID as CommodityID, C.Commodity, C.LocCommodity," +
                                "       nullif(CD.Buy, 0) As Buy, nullif(CD.Sell, 0) As Sell, CD.timestamp, " +
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
                                "       nullif(CD.Buy, 0) As Buy, nullif(CD.Sell, 0) As Sell, CD.timestamp, " +
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
  
                Program.DBCon.Execute(sqlString, Data);

                lastCommodity   = null;
                BuyMin          = null;
                SellMax         = null;
                Result.Clear();

                foreach (DataRow currentRow in Data.AsEnumerable())
                {
                    // Debug.Print((String)lastCommodity["CommodityID"] + " " + (String)currentRow["CommodityID"]);
                    // Debug.Print((String)currentRow["Stationname"]);

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

                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                return Result;
            }
            catch (Exception ex)
            {
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                throw new Exception("Error while getting the best market prices", ex);
            }
        }

        public DataTable calculateTradingRoutes()
        {
            String sqlBaseString;
            String sqlString;
            DataSet Data           = new DataSet();
            var tmNeighbourstations  = new dsEliteDB.tmneighbourstationsDataTable();
            var tmFilteredStations  = new dsEliteDB.tmfilteredstationsDataTable();
            DataRow BuyMin;
            DataRow SellMax;
            DataRow lastCommodity;
            HashSet<String> Calculated = new HashSet<String>();
            //Dictionary<Int32, List<DataRow>> CollectedData;
            SortedList<Int32, DataRow> CollectedData;
            Int32 StationCount;
            Int32 SystemCount;
            Int32 Current = 0;
            ProgressView PV;
            Boolean Cancelled = false;
            Int32 DataFound = 0;
            Int32 maxTradingDistance;
            dsEliteDB.tmpa_s2s_besttripsDataTable Result;

            try
            {

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                getFilteredSystemAndStationCount(out StationCount, out SystemCount);

                // get the results for a cancellable loop
                sqlString = "select * from tmfilteredstations" +
                            " order by Station_ID";
                Program.DBCon.Execute(sqlString, tmFilteredStations);

                maxTradingDistance = -1;
                if(Program.DBCon.getIniValue<Boolean>(tabPriceAnalysis.DB_GROUPNAME, "MaxTripDistance"))
                    maxTradingDistance = Program.DBCon.getIniValue<Int32>(tabPriceAnalysis.DB_GROUPNAME, "MaxTripDistanceValue");

                // delete old content
                sqlString = "delete from tmNeighbourstations;";
                Program.DBCon.Execute(sqlString);
                    

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

                PV = new ProgressView();
                Current = 0;

                if(maxTradingDistance >= 0)
                    PV.progressStart("determine neigbour-systems in range of "+ maxTradingDistance + " ly of " + SystemCount + " selected systems...");
                else
                    PV.progressStart("determine neigbour-systems without range limit of " + SystemCount + " selected systems...");

                foreach (dsEliteDB.tmfilteredstationsRow CurrentStation in tmFilteredStations)
                {
                    // preparing a table with the stations from "tmFilteredStations" and all 
                    // their neighbour stations who are not further away than the max trading distance

                    if(maxTradingDistance >= 0)
                        sqlString = String.Format(sqlBaseString, CurrentStation.Station_id, maxTradingDistance);
                    else
                        sqlString = String.Format(sqlBaseString, CurrentStation.Station_id);

                    Program.DBCon.Execute(sqlString);
                        
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
                sqlBaseString = "insert into tmBestProfits(Station_ID_From, Station_ID_To, Max_Profit)" +
                                " select Station_ID_From, Station_ID_To, max(Profit) As Max_Profit from " +
                                " (select PR1.Station_ID_From, PR1.Station_ID_To, Pr1.Forward, Pr2.Back, (ifnull(Pr1.Forward, 0) + ifnull(Pr2.Back,0)) As Profit  from  " +
                                " 	(select L1.*, if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, (nullif(L2.Sell,0) - nullif(L1.Buy,0)), null) As Forward,  " +
                                " 			if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, (nullif(L1.Sell,0) - nullif(L2.Buy,0)), null) As Back  " +
                                " 	from (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell  " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on  N.Station_ID_From = CD.Station_ID " +
                                " 																		  and N.Station_ID_From = {0} " +
                                " 				   ) L1  " +
                                " 	 join " +
                                " 				  (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on N.Station_ID_To = CD.Station_ID " +
                                " 				   ) L2 " +
                                " 	on  L1.Station_ID_From = L2.Station_ID_From " +
                                " 	and L1.Station_ID_To   = L2.Station_ID_To " +
                                " 	and L1.Commodity_ID    = L2.Commodity_ID) Pr1 " +
                                "      " +
                                "     join  " +
                                "  " +
                                " 	(select L1.*, if((nullif(L2.Sell,0) - nullif(L1.Buy,0)) > 0, (nullif(L2.Sell,0) - nullif(L1.Buy,0)), null) As Forward,  " +
                                " 			if((nullif(L1.Sell,0) - nullif(L2.Buy,0)) > 0, (nullif(L1.Sell,0) - nullif(L2.Buy,0)), null) As Back  " +
                                " 	from (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell  " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on  N.Station_ID_From = CD.Station_ID " +
                                " 																		  and N.Station_ID_From = {0} " +
                                " 				   ) L1  " +
                                " 	 join " +
                                " 				  (select N.Station_ID_From, N.Station_ID_To, CD.Commodity_ID, CD.Buy, CD.Sell " +
                                " 					   from tmNeighbourstations N join tbCommodityData CD on N.Station_ID_To = CD.Station_ID " +
                                " 				   ) L2 " +
                                " 	on  L1.Station_ID_From = L2.Station_ID_From " +
                                " 	and L1.Station_ID_To   = L2.Station_ID_To " +
                                " 	and L1.Commodity_ID    = L2.Commodity_ID) Pr2 " +
                                "      " +
                                " on  Pr1.Station_ID_From = Pr2.Station_ID_From " +
                                " and Pr1.Station_ID_To   = Pr2.Station_ID_To) ALL_RESULTS " +
                                " group by Station_ID_From, Station_ID_To";

                if(!Cancelled)
                {
                    DataFound = 0;
                    Current = 0;
                    Calculated.Clear();

                    Program.DBCon.Execute("delete from tmBestProfits");

                    // get the start stations for a cancellable loop
                    sqlString = "select Station_ID_From, count(*) As Neighbours from tmNeighbourstations" +
                                " group by Station_ID_From" +
                                " order by Station_ID_From";
                    Program.DBCon.Execute(sqlString, "StartStations", Data);

                    PV = new ProgressView();

                    if(maxTradingDistance >= 0)
                        PV.progressStart("Processing market data of " + StationCount + " stations from " + SystemCount + " systems\n" +
                                            "(max. trading distance = " + maxTradingDistance + " ly)...");
                    else
                        PV.progressStart("Processing market data of " + StationCount + " stations from " + SystemCount + " systems\n" +
                                            "(no trading distance limit)...");

                    foreach(DataRow StartStation in Data.Tables["StartStations"].Rows)
                    {
                        // get the trading data 
                        sqlString = String.Format(sqlBaseString, StartStation["Station_ID_From"]);

                        Program.DBCon.Execute(sqlString);

                        Current += 1;

                        PV.progressUpdate(Current,  Data.Tables["StartStations"].Rows.Count);

                        if(PV.Cancelled)
                        {
                            Cancelled = true;
                            break;
                        }
                    }


                    Program.DBCon.Execute("delete from tmPA_S2S_BestTrips");

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

                    Program.DBCon.Execute(sqlString);

                    Result = new dsEliteDB.tmpa_s2s_besttripsDataTable();



                    PV.progressStop();

                }


                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

                sqlString = "select distinct St1.*, St2.*, Bp.Max_Profit As Profit, NS.Distance_Between As Distance from tmBestProfits Bp" +
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
                Program.DBCon.Execute(sqlString, Result);

                return Result;

	        }
	        catch (Exception ex)
	        {
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

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
                Program.DBCon.Execute(sqlString, Data);

                SystemCount  = (Int32)(Int64)Data.Rows[0]["Systems"];
                StationCount = (Int32)(Int64)Data.Rows[0]["Stations"];
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the number of filtered systems and stations", ex);
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

