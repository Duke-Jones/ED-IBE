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

namespace RegulatedNoise.Commander_s_Log
{
    public class CommandersLog
    {
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
        public tabCommandersLog     m_GUI;
        private BindingSource       m_BindingSource;
        private DataTable           m_Datatable;
        private DataRetriever       retriever;
        private Boolean             m_NoGuiNotifyAfterSave;

        /// <summary>
        /// constructor
        /// </summary>
        public CommandersLog()
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
        public tabCommandersLog GUI
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
        /// prepares the values of the comoboboxes
        /// </summary>
        /// <param name="theCombobox"></param>
        internal void prepareCmb_EventTypes(ref ComboBox_ro theCombobox, ComboBox_ro theReferenceCombobox = null)
        {
            try
            {
                if(theCombobox.Name.Equals(m_GUI.cbLogEventType.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    theCombobox.DataSource       = m_BaseData.tbeventtype;
                    theCombobox.DisplayMember    = "event";
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogSystemName.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    theCombobox.DataSource       = m_BaseData.tbsystems;
                    theCombobox.DisplayMember    = "systemname";
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogStationName.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if((theReferenceCombobox == null) || (theReferenceCombobox.SelectedItem == null))
                    { 
                        theCombobox.DataSource       = m_BaseData.tbstations;
                        theCombobox.DisplayMember    = "stationname";
                    }
                    else
                    {
                        Int32 SytemID                = (Int32)((System.Data.DataRowView)theReferenceCombobox.SelectedItem).Row["id"];
                        theCombobox.DataSource       = m_BaseData.tbstations.Select("system_id = " + SytemID);
                        theCombobox.DisplayMember    = "stationname";
                    }
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogCargoName.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    theCombobox.DataSource       = m_BaseData.tbcommodity;
                    theCombobox.DisplayMember    = "loccommodity";
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogCargoAction.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    theCombobox.DataSource       = m_BaseData.tbcargoaction;
                    theCombobox.DisplayMember    = "action";
                }

                theCombobox.ValueMember      = "id";

            }
            catch (Exception ex)
            {
                throw new Exception("Error in <prepareCmb_EventTypes> while preparing '" + theCombobox.Name + "'", ex);
            }
        }

        /// <summary>
        /// saving the data of the datarow,
        /// saves new entrys if the timestamp is not existing, otherwise existing data will be changed
        /// </summary>
        /// <param name="ChangedData">row with data to save</param>
        public void SaveEvent(CommandersLogEvent Event)
        {
            try
            {
                dsEliteDB.vilogDataTable TempTable;
                dsEliteDB.vilogRow TempRow;

                TempTable = new dsEliteDB.vilogDataTable();
                TempRow = (dsEliteDB.vilogRow)TempTable.NewRow();

                TempRow.time                = Event.EventDate;
                TempRow.systemname          = Event.System;
                TempRow.stationname         = Event.Station;
                TempRow.loccommodity        = Event.Cargo;
                TempRow.action              = Event.CargoAction;
                TempRow.cargovolume         = (Int32)Event.CargoVolume;
                TempRow.credits_transaction = (Int32)Event.TransactionAmount;
                TempRow.credits_total       = (Int32)Event.Credits;
                TempRow.eevent              = Event.EventType;
                TempRow.notes               = Event.Notes;

                SaveEvent(TempRow);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while preparing save data (Event class)", ex);
            }
        }

        /// <summary>
        /// saving the data of the datarow,
        /// saves new entrys if the timestamp is not existing, otherwise existing data will be changed
        /// </summary>
        /// <param name="ChangedData">row with data to save</param>
        public void SaveEvent(DateTime EventDate, String System, String Station, String Cargo, String CargoAction, int CargoVolume, Int32 CreditsTransAction, Int32 Credits_Total, String EventType, String Notes)
        {
            try
            {
                dsEliteDB.vilogDataTable TempTable;
                dsEliteDB.vilogRow TempRow;

                TempTable = new dsEliteDB.vilogDataTable();
                TempRow = (dsEliteDB.vilogRow)TempTable.NewRow();

                TempRow.time                = EventDate;
                TempRow.systemname          = System;
                TempRow.stationname         = Station;
                TempRow.loccommodity        = Cargo;
                TempRow.action              = CargoAction;
                TempRow.cargovolume         = CargoVolume;
                TempRow.credits_transaction = CreditsTransAction;
                TempRow.credits_total       = Credits_Total;
                TempRow.eevent              = EventType;
                TempRow.notes               = Notes;

                SaveEvent(TempRow);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while preparing save data (single params)", ex);
            }
        }

        /// <summary>
        /// saving the data of the datarow,
        /// saves new entrys if the timestamp is not existing, otherwise existing data will be changed
        /// </summary>
        /// <param name="ChangedData">row with data to save</param>
        internal void SaveEvent(dsEliteDB.vilogRow ChangedData)
        {
            String sqlString;

            try
            {

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
                                            " ON DUPLICATE KEY UPDATE" +
                                            "  system_id            = d.system_id," +
                                            "  station_id           = d.station_id," +
                                            "  event_id             = d.event_id," +
                                            "  commodity_id         = d.commodity_id," +
                                            "  cargoaction_id       = d.cargoaction_id," +
                                            "  cargovolume          = d.cargovolume," +
                                            "  credits_transaction  = d.credits_transaction," +
                                            "  credits_total        = d.credits_total," +
                                            "  notes                = d.notes",
                                            DBConnector.SQLDateTime(ChangedData.time),
                                            DBConnector.SQLAString(DBConnector.SQLEscape(ChangedData.systemname)),
                                            DBConnector.SQLAString(DBConnector.SQLEscape(ChangedData.stationname)),
                                            DBConnector.SQLAString(ChangedData.eevent),
                                            DBConnector.SQLAString(ChangedData.loccommodity),
                                            DBConnector.SQLAString(ChangedData.action),
                                            ChangedData.cargovolume,
                                            ChangedData.credits_transaction,
                                            ChangedData.credits_total,
                                            ChangedData.notes.Trim() == String.Empty ? "null" : String.Format("'{0}'", DBConnector.SQLEscape(ChangedData.notes)));

                Program.DBCon.Execute(sqlString); 

                if(!m_NoGuiNotifyAfterSave)
                {
                    Int32 RowIndex;

                    m_NoGuiNotifyAfterSave  = false;
                    RowIndex                = Program.DBCon.getRowIndex("viLog", "time", DBConnector.SQLSortOrder.desc, "time", 
                                                                        DBConnector.SQLDateTime(ChangedData.time));


                    DataChanged.Raise(this, new DataChangedEventArgs() { DataRow = RowIndex, DataKey = ChangedData.time});                     
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving Commanders Log to DB", ex);
            }
        }


    }

#region outdated 

    /// <summary>
    /// class only for compatibilty reasons at the moment,
    /// will later removed
    /// </summary>

    [Serializable]
    public class CommandersLogEvent
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

