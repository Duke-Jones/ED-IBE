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

        private const String table = "tbLog";

        /// <summary>
        /// main selection string for the data from the database
        /// </summary>
        private const String _sqlString = "select L.time, S.systemname, St.stationname, E.event As eevent, C.action," + 
                                          "       Co.loccommodity, L.cargovolume, L.credits_transaction, L.credits_total, L.notes" +
                                         " from tbLog L left join tbEventType E   on L.event_id       = E.id" + 
                                         "              left join tbCargoAction C on L.cargoaction_id = C.id" +
                                         "              left join tbSystems S     on L.system_id      = S.id" +
                                         "              left join tbStations St   on L.station_id     = St.id" +
                                         "              left join tbCommodity Co  on L.commodity_id   = Co.id";

        private SQL.Datasets.dsCommandersLog      m_BaseData;
        public tabCommandersLog     m_GUI;
        private BindingSource       m_BindingSource;
        private DataTable           m_Datatable;
        private DataRetriever       retriever;

        /// <summary>
        /// constructor
        /// </summary>
        public CommandersLog()
        {
            m_BindingSource             = new BindingSource();
            m_Datatable                 = new DataTable();

            m_BindingSource.DataSource  = m_Datatable;
        }

        /// <summary>
        /// initialization of the dataretriever object (for DGV virtual mode)
        /// </summary>
        /// <returns></returns>
        internal int InitRetriever()
        {
            try
            {
                retriever = new DataRetriever(Program.DBCon, table, _sqlString, "time", DataRetriever.SQLSortOrder.desc);

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
        public SQL.Datasets.dsCommandersLog BaseData
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

        public string CreateEvent()
        {
            String newEventID = Guid.NewGuid().ToString();

            //LogEvents.Add(new CommandersLogEvent 
            //{
            //    EventType =_callingForm.cbLogEventType.Text,
            //    Station =_callingForm.cbLogStationName.Text,
            //    System =_callingForm.cbLogSystemName.Text,
            //    Cargo =_callingForm.cbLogCargoName.Text,
            //    CargoAction =_callingForm.cbLogCargoAction.Text,
            //    CargoVolume =int.Parse(_callingForm.cbLogQuantity.Text),
            //    Notes =_callingForm.tbLogNotes.Text,
            //    EventDate = _callingForm.dtpLogEventDate.Value,
            //    EventID = newEventID
            //});

            return newEventID;
        }

        public String CreateEvent(string eventType, string station, string system, string cargo, string cargoAction, int cargoVolume, string notes, DateTime eventDate)
        {
            String newEventID = Guid.NewGuid().ToString();

            //LogEvents.Add(new CommandersLogEvent
            //{
            //    EventType =               eventType                  ,
            //    Station =                 station                    ,
            //    System =                  system                     ,
            //    Cargo =                   cargo                      ,
            //    CargoAction =             cargoAction                ,
            //    CargoVolume =             cargoVolume                ,
            //    Notes =                   notes                      ,
            //    EventDate        =        eventDate                  ,
            //    EventID = newEventID  
            //});

            //UpdateCommandersLogListView();

            return newEventID;
        }

        public void CreateNewEvent() // Clears the fields ready for input
        {
            // set it to UTC everywhere or nowhere -> pay attention to the different timezones
            // if you wan't to concatenate ED-time and local pc time
            //var now =DateTime.UtcNow;
            //var now = DateTime.Now;
            //ClearLogEventFields();
            //_callingForm.dtpLogEventDate.Value = now;
            //_callingForm.tbLogEventID.Text ="";
            //_callingForm.btCreateAddEntry.Text = "Save As New Entry";

        }

        public void CreateEvent(CommandersLogEvent partiallyCompleteCommandersLogEventEvent) // when we create from the webserver
        {
            // set it to UTC everywhere or nowhere -> pay attention to the different timezones
            // if you wan't to concatenate ED-time and local pc time
            ////var now =DateTime.UtcNow;
            //var now = DateTime.Now;
            //var newGuid = Guid.NewGuid().ToString();
            //ClearLogEventFields();
            //_callingForm.dtpLogEventDate.Value = now;
            //_callingForm.tbLogEventID.Text = newGuid;
            //partiallyCompleteCommandersLogEventEvent.EventID = Guid.NewGuid().ToString();
            //partiallyCompleteCommandersLogEventEvent.EventDate = now;
            
            //LogEvents.Add(partiallyCompleteCommandersLogEventEvent);

        }

        private void ClearLogEventFields()
        {
            //_callingForm.cbLogEventType.Text = "";
            //_callingForm.cbLogStationName.Text = "";
            //_callingForm.cbLogSystemName.Text = "";
            //_callingForm.cbLogCargoName.Text = "";
            //_callingForm.cbLogCargoAction.Text = "";
            //_callingForm.cbLogQuantity.Text = "0";
            //_callingForm.tbLogNotes.Text = "";
            //_callingForm.nbTransactionAmount.Text = "0";

            //_callingForm.UpdateSystemNameFromLogFile();

            //if (_callingForm.Program.actualCondition.System != "")
            //    _callingForm.cbLogSystemName.Text = _callingForm.Program.actualCondition.System;

            //if (_callingForm.Program.actualCondition.Station != "")
            //    _callingForm.cbLogStationName.Text = _callingForm.Program.actualCondition.Station;
        }

        /// <summary>
        /// saves new entrys if the timestamp is not existing, otherwise existing data will be changed
        /// </summary>
        /// <param name="dataGridViewExt"></param>
        /// <param name="RowIndex"></param>
        internal void SaveData(Enums_and_Utility_Classes.DataGridViewExt dataGridViewExt, int RowIndex)
        {
            String           sqlString;

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
                                            DBConnector.SQLDateTime(DateTime.Parse((String)dataGridViewExt["time", RowIndex].Value, CultureInfo.CurrentUICulture , DateTimeStyles.None)), 
                                            DBConnector.SQLAString(DBConnector.SQLEscape((String)dataGridViewExt["systemname", RowIndex].Value)),
                                            DBConnector.SQLAString(DBConnector.SQLEscape((String)dataGridViewExt["stationname", RowIndex].Value)), 
                                            DBConnector.SQLAString((String)dataGridViewExt["eevent", RowIndex].Value),
                                            DBConnector.SQLAString((String)dataGridViewExt["loccommodity", RowIndex].Value),
                                            DBConnector.SQLAString((String)dataGridViewExt["action", RowIndex].Value),
                                            dataGridViewExt["cargovolume", RowIndex].Value,
                                            dataGridViewExt["credits_transaction", RowIndex].Value,
                                            dataGridViewExt["credits_total", RowIndex].Value,
                                            dataGridViewExt["notes", RowIndex].Value.ToString().Trim() == String.Empty ? "null" : String.Format("'{0}'", DBConnector.SQLEscape(dataGridViewExt["notes", RowIndex].Value.ToString())));

                if(Program.DBCon.Execute(sqlString) != 0)
                    throw new Exception("Nothing saved to database !!!");
     
            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving Commanders Log to DB", ex);    
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
                if(theCombobox.Name.Equals(m_GUI.cbLogEventType.Name))
                {
                    theCombobox.DataSource       = m_BaseData.Tables["tbEventType"];
                    theCombobox.DisplayMember    = "event";
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogSystemName.Name))
                {
                    theCombobox.DataSource       = m_BaseData.Tables["tbSystems"];
                    theCombobox.DisplayMember    = "systemname";
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogStationName.Name))
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
                else if(theCombobox.Name.Equals(m_GUI.cbLogCargoName.Name))
                {
                    theCombobox.DataSource       = m_BaseData.Tables["tbCommodity"];
                    theCombobox.DisplayMember    = "loccommodity";
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogCargoAction.Name))
                {
                    theCombobox.DataSource       = m_BaseData.Tables["tbCargoAction"];
                    theCombobox.DisplayMember    = "action";
                }

                theCombobox.ValueMember      = "id";

            }
            catch (Exception ex)
            {
                throw new Exception("Error in <prepareCmb_EventTypes> while preparing '" + theCombobox.Name + "'", ex);
            }
        }
    }

#region DataRetriever


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

