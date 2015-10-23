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

namespace RegulatedNoise.Price_Analysis
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
        public void createFilteredTable(Int32 SystemID, Object Distance, Object DistanceToStar, Object minLandingPadSize)
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

                sqlString = String.Format(
                            "delete from tmFilteredStations;" +
                            "insert into tmFilteredStations(System_id, Station_id, Distance) " +
                            "   select Sy.ID As System_id, St.ID As Station_id, SQRT(POW(Sy.x - {0}, 2) + POW(Sy.y - {1}, 2) +  POW(Sy.z - {2}, 2)) As Distance" + 
                            "   from tbSystems Sy, tbStations St" +
                            "   where Sy.ID = St.System_id",
                            currentSystem.Rows[0]["x"].ToString(), 
                            currentSystem.Rows[0]["y"].ToString(), 
                            currentSystem.Rows[0]["z"].ToString());


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

                return Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the best market prices", ex);
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

