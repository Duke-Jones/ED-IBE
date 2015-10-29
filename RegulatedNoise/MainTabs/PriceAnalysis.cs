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

                //sqlString = String.Format(
                //            "drop temporary table if exists tmFilteredStations;" +
                //            "create temporary table tmFilteredStations as" +
                //            "   select Sy.ID As System_id, St.ID As Station_id, SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)) As Distance" + 
                //            "   from tbSystems Sy, tbStations St" +
                //            "   where Sy.ID = St.System_id",
                //            currentSystem.Rows[0]["x"].ToString(), 
                //            currentSystem.Rows[0]["y"].ToString(), 
                //            currentSystem.Rows[0]["z"].ToString());

                sqlString = "delete from tmFilteredStations;";
                Program.DBCon.Execute(sqlString);

                sqlString = String.Format(
                            "insert into tmFilteredStations(System_id, Station_id, Distance, x, y, z) " +
                            "   select Sy.ID As System_id, St.ID As Station_id, SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)) As Distance, Sy.x, Sy.x, Sy.z" + 
                            "   from tbSystems Sy, tbStations St" +
                            "   where Sy.ID = St.System_id",
                            currentSystem.Rows[0]["x"].ToString(), 
                            currentSystem.Rows[0]["y"].ToString(), 
                            currentSystem.Rows[0]["z"].ToString());

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
                            currentSystem.Rows[0]["x"].ToString(), 
                            currentSystem.Rows[0]["y"].ToString(), 
                            currentSystem.Rows[0]["z"].ToString(), 
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

        public DataTable getMinMax(Boolean OnlyTradedCommodities)
        {
            String sqlString;
            DataTable Data      = new DataTable();
            var Result          = new dsEliteDB.tbpa_allcommoditiesDataTable();
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
                        var newRow = (dsEliteDB.tbpa_allcommoditiesRow)Result.NewRow();
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

        public void calculateTradingRoutes(Int32 maxTradingDistance)
        {
            String sqlBaseString;
            String sqlString;
            DataTable Data           = new DataTable();
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

            try
            {

                // gettin' some freaky performance
                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=2");

                getFilteredSystemAndStationCount(out StationCount, out SystemCount);

                // get the results for a cancellable loop
                sqlString = "select * from tmfilteredstations" +
                            " order by Station_ID";
                Program.DBCon.Execute(sqlString, tmFilteredStations);

                // delete old content
                sqlString = "delete from tmNeighbourstations;";
                Program.DBCon.Execute(sqlString);
                    
                sqlBaseString = "insert into tmNeighbourstations(System_ID_From, Station_ID_From, Distance_From," +
                                "                                System_ID_To, Station_ID_To, Distance_To, " +
                                "                                Distance_Between) " +
                                " select BS.System_ID As System_ID_From, BS.Station_ID As Station_ID_From, BS.Distance As Distance_From," +
                                "        FS.System_ID As System_ID_To, FS.Station_ID As Station_ID_To, FS.Distance As Distance_To," + 
                                "        sqrt(POW(FS.x - BS.x, 2) + POW(FS.y - BS.y, 2) +  POW(FS.z - BS.z, 2)) as Distance_Between" +
                                " from (select * from tmFilteredStations  where Station_ID = {0} order by System_ID, Station_ID) BS" + 
                                "                                                                join tmfilteredstations FS on (sqrt(POW(FS.x - BS.x, 2) + POW(FS.y - BS.y, 2) +  POW(FS.z - BS.z, 2)) <=  {1})" +
                                "                                                                join tbStations St on FS.Station_ID = St.ID" +
                                "                                                                join tbSystems Sy on St.System_ID  = Sy.ID" +
                                " having  BS.Station_ID <> FS.Station_ID;";

                PV = new ProgressView();
                Current = 0;

                PV.progressStart("determine neigbour-systems in range of "+ maxTradingDistance + " ly of " + SystemCount + " selected systems...");

                foreach (dsEliteDB.tmfilteredstationsRow CurrentStation in tmFilteredStations)
                {
                    // preparing a table with the stations from "tmFilteredStations" and all 
                    // their neighbour stations who are not further away than the max trading distance

                    sqlString = String.Format(sqlBaseString, CurrentStation.Station_id, maxTradingDistance);
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

                
                if(!Cancelled)
                {
                    // get the results for a cancellable loop
                    sqlString = "select * from tmNeighbourstations" +
                                " order by Station_ID_From";
                    Program.DBCon.Execute(sqlString, tmNeighbourstations);

                    sqlBaseString = "select CD1.station_ID As Station1, CD2.station_ID As Station2, CD1.commodity_id," +
                                    " 	   if((nullif(CD2.Sell,0) - nullif(CD1.Buy,0)) > 0, (nullif(CD2.Sell,0) - nullif(CD1.Buy,0)), null) As Forward," + 
                                    "        if((nullif(CD1.Sell,0) - nullif(CD2.Buy,0)) > 0, (nullif(CD1.Sell,0) - nullif(CD2.Buy,0)), null) As Back" + 
                                    " 	from tbCommodityData CD1 join (" +
                                    "                                    select * from tmNeighbourstations N, tbCommodityData CD3" + 
                                    "                                       where N.Station_ID_To   = CD3.station_id" +
                                    "                                       and   N.Station_ID_From = {0}" +
                                    " 								  ) CD2" +
                                    "             on  (CD1.commodity_id = CD2.commodity_id)" +
                                    "             and (CD1.station_id   = {0})" +
                                    "             join" + 
                                    "        		(select CD1.commodity_id, max((nullif(CD2.Sell,0) - nullif(CD1.Buy,0))) As Forward, max((nullif(CD1.Sell,0) - nullif(CD2.Buy,0))) As Back" +
                                    " 			      from tbCommodityData CD1 join (" +
                                    " 										   select * from tmNeighbourstations N, tbCommodityData CD3" + 
                                    " 											  where N.Station_ID_To   = CD3.station_id" +
                                    " 											  and   N.Station_ID_From = {0}" +
                                    " 										  ) CD2" +
                                    " 					on  (CD1.commodity_id = CD2.commodity_id)" +
                                    " 					and (CD1.station_id   = {0})" +
                                    "          		group by CD1.commodity_id) XP on CD1.Commodity_id  = XP.Commodity_id" +
                                    "                                                  and (   (nullif(CD2.Sell,0) - nullif(CD1.Buy,0))      = XP.Forward" +
                                    "                                                       or (nullif(CD1.Sell,0) - nullif(CD2.Buy,0))      = XP.Back)" +
                                    " having ((Forward Is Not null) or (Back Is Not null))";
 
                    Int32 DataFound = 0;
                    Current = 0;
                    Calculated.Clear();
                    //CollectedData = new Dictionary<Int32, List<DataRow>>();

                    PV = new ProgressView();

                    PV.progressStart("Processing data of " + StationCount + " stations from " + SystemCount + " systems (" + tmNeighbourstations.Count + " routes)...");

                    foreach(dsEliteDB.tmneighbourstationsRow StartStation in tmNeighbourstations)
                    {
                        Boolean isNew = false;

                        // first check if we've already calculated all data between these station
                        if(StartStation.Station_ID_From < StartStation.Station_ID_To)
                            isNew = Calculated.Add(StartStation.Station_ID_From.ToString() + "|" + StartStation.Station_ID_To.ToString());
                        else
                            isNew = Calculated.Add(StartStation.Station_ID_To.ToString() + "|" + StartStation.Station_ID_From.ToString());

                        if(isNew)
                        {
                            // get the trading data 
                            sqlString = String.Format(sqlBaseString, StartStation.Station_ID_From);

                            if(Program.DBCon.Execute(sqlString, Data) > 0)
                            {
                                //Debug.Print("got something");
                                DataFound += 1;
                            }

                            foreach(DataRow Profit in Data.AsEnumerable())
                            {

                                //if(CollectedData)
                                //CollectedData.Last();
                                //List<DataRow> CommodityData;


                                //if(!CollectedData.TryGetValue((Int32)(Profit["commodity_id"]), out CommodityData))
                                //{
                                //    // first data
                                //    CommodityData = new List<DataRow>() {Profit};
                                //}
                                //else
                                //{
                                //    // following data, 
                                    

                                //}

                                //var x = CollectedData.Last();

                            }
                        }

                        Current += 1;

                        PV.progressUpdate(Current,  tmNeighbourstations.Rows.Count);

                        if(PV.Cancelled)
                        {
                            Cancelled = true;
                            break;
                        }
                    }

                    PV.progressStop();

                }

                Program.DBCon.Execute("set global innodb_flush_log_at_trx_commit=1");

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

