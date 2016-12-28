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
using Newtonsoft.Json.Linq;
using System.Drawing;
using IBE.FileScanner;

namespace IBE.MTCommandersLog
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
         private const String _sqlColumnString = "select L.time, S.systemname, St.stationname, E.eventtype, C.cargoaction," + 
                                                 "       Co.loccommodity, L.cargovolume, L.credits_transaction, L.credits_total, L.notes, L.distance";

        private const String _sqlBaseString = " from tbLog L left join tbEventType E   on L.event_id       = E.id" + 
                                              "              left join tbCargoAction C on L.cargoaction_id = C.id" +
                                              "              left join tbSystems S     on L.system_id      = S.id" +
                                              "              left join tbStations St   on L.station_id     = St.id" +
                                              "              left join tbCommodity Co  on L.commodity_id   = Co.id";

        private const String _sqlWhereString = "";
        
        private dsEliteDB                               m_BaseData;
        public tabCommandersLog                         m_GUI;
        private BindingSource                           m_BindingSource;
        private DataTable                               m_Datatable;
        private DataRetriever                           retriever;
        private Boolean                                 m_NoGuiNotifyAfterSave;
        private FileScanner.EDJournalScanner             m_JournalScanner;
        private IBE.IBECompanion.DataEventBase          m_DataEventObject;
        private Dictionary<Object, BindingSource>       m_BindingSources;

        /// <summary>
        /// constructor
        /// </summary>
        public CommandersLog()
        {
            try
            {
                m_BindingSource             = new BindingSource();
                m_Datatable                 = new DataTable();
                m_BindingSources            = new Dictionary<object,BindingSource>();
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
        internal int InitRetriever(Boolean getRowCount = false)
        {
            try
            {
                if(retriever == null)
                    retriever = new DataRetriever(Program.DBCon, table, _sqlColumnString, _sqlBaseString, "time", DBConnector.SQLSortOrder.desc, m_GUI.bindNavCmdrsLog, new dsEliteDB.vilogDataTable());

                if(getRowCount)
                    return retriever.RowCount(true);
                else
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
            Boolean initial = false;
            BindingSource currentBS = null;

            try
            {
                if(!m_BindingSources.TryGetValue(theCombobox, out currentBS))
                {
                    BindingSource newBS     = new BindingSource();
                    theCombobox.DataSource  = newBS;
                    m_BindingSources.Add(theCombobox, newBS);

                    currentBS = newBS;
                    initial = true;
                }

                if(theCombobox.Name.Equals(m_GUI.cbLogEventType.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    DataTable selectedDT = m_BaseData.tbeventtype;

                    if(initial)
                    {
                        currentBS.DataSource = selectedDT.Clone();

                        theCombobox.DisplayMember    = "eventtype";
                        theCombobox.ValueMember      = "id";
                    }

                    currentBS.DataSource = selectedDT.Copy();
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogSystemName.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    DataTable selectedDT = m_BaseData.tbsystems.Clone();

                    if(initial)
                    {
                        currentBS.DataSource = selectedDT;
                        theCombobox.DisplayMember   = "systemname";
                        theCombobox.ValueMember     = "id";
                    }

                    if ((GUI.dgvCommandersLog.RowCount > 0) && (GUI.dgvCommandersLog.CurrentCell != null))
                    {
                        var systemName = GUI.dgvCommandersLog.Rows[GUI.dgvCommandersLog.CurrentCell.RowIndex].Cells["systemname"].Value.ToString();

                        String sqlString = "select * from tbSystems where systemName = " + DBConnector.SQLAString(systemName);
                        Program.DBCon.Execute(sqlString, selectedDT);
                    }

                    currentBS.DataSource = selectedDT;
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogStationName.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if(initial)
                    {
                        currentBS.DataSource         = m_BaseData.tbstations.Clone();
                        theCombobox.DisplayMember    = "stationname";
                        theCombobox.ValueMember      = "id";
                    }

                    String system = theReferenceCombobox.Text;

                    // small performance improvement -> only reload if necessary
                    if((theCombobox.Tag == null) || (((String)theCombobox.Tag) != system))
                    { 
                        var systemdata = m_BaseData.tbsystems.Select("systemname = '" +  system + "'");

                        if(systemdata.Count() > 0)
                        {
                            DataTable selectedDT = m_BaseData.tbstations.Clone();
                            selectedDT.Clear();
                            m_BaseData.tbstations.Select("system_id = " + systemdata[0]["id"]).CopyToDataTable(selectedDT,LoadOption.OverwriteChanges);

                            // add a empty row, if we don't wan't to select a station
                            dsEliteDB.tbstationsRow emptyRow = (dsEliteDB.tbstationsRow)selectedDT.NewRow();
                            emptyRow.stationname = "";
                            emptyRow.id         = 0;
                            emptyRow.updated_at = DateTime.UtcNow;
                            emptyRow.system_id  = 0;

                            selectedDT.Rows.InsertAt(emptyRow, 0);

                            currentBS.DataSource = selectedDT;
                        }
                        else
                        {
                            ((DataTable)currentBS.DataSource).Clear();
                        }

                        theCombobox.Tag = system;
                    }
                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogCargoName.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    DataTable selectedDT = m_BaseData.tbcommodity;

                    if(initial)
                    {
                        currentBS.DataSource         = selectedDT.Clone();
                        theCombobox.DisplayMember    = "loccommodity";
                        theCombobox.ValueMember      = "id";
                    }

                    currentBS.DataSource = selectedDT.Copy();

                }
                else if(theCombobox.Name.Equals(m_GUI.cbLogCargoAction.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    DataTable selectedDT = m_BaseData.tbcargoaction;

                    if(initial)
                    {
                        currentBS.DataSource         = selectedDT.Clone();
                        theCombobox.DisplayMember    = "cargoaction";
                        theCombobox.ValueMember      = "id";
                    }

                    currentBS.DataSource = selectedDT.Copy();

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error in <prepareCmb_EventTypes> while preparing '" + theCombobox.Name + "'", ex);
            }
        }

        /// <summary>
        /// saving the data of the datarow,
        /// saves new entrys if the timestamp is not existingClassification, otherwise existingClassification data will be changed
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
                TempRow.cargoaction         = Event.CargoAction;
                TempRow.cargovolume         = (Int32)Event.CargoVolume;
                TempRow.credits_transaction = (Int32)Event.TransactionAmount;
                TempRow.credits_total       = (Int32)Event.Credits;
                TempRow.eventtype           = Event.EventType;
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
        /// saves new entrys if the timestamp is not existingClassification, otherwise existingClassification data will be changed
        /// </summary>
        /// <param name="ChangedData">row with data to save</param>
        public void SaveEvent(DateTime EventDate, String System, String Station, String Cargo, String CargoAction, int CargoVolume, Int32 CreditsTransAction, Int32 Credits_Total, String EventType, String Notes, double? Distance=null)
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
                TempRow.cargoaction         = CargoAction;
                TempRow.cargovolume         = CargoVolume;
                TempRow.credits_transaction = CreditsTransAction;
                TempRow.credits_total       = Credits_Total;
                TempRow.eventtype           = EventType;
                TempRow.notes               = Notes;

                if(Distance.HasValue)
                    TempRow.distance        = Distance.Value;

                SaveEvent(TempRow);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while preparing save data (single params)", ex);
            }
        }

        /// <summary>
        /// saving the data of the datarow,
        /// saves new entrys if the timestamp is not existingClassification, otherwise existingClassification data will be changed
        /// </summary>
        /// <param name="ChangedData">row with data to save</param>
        internal void SaveEvent(dsEliteDB.vilogRow ChangedData)
        {
            String sqlString;
            double? nDistance = null;

            try
            {
                try
                {
                    nDistance = ChangedData.distance;
                }
                catch (Exception)
                {
                    // typed datasets can't handle nullvalues for non-string columns 
                    // thank you, ms ]-P
                }

                sqlString = String.Format("INSERT INTO tbLog(time, system_id, station_id, event_id, commodity_id," +
                                            "                  cargoaction_id, cargovolume, credits_transaction, credits_total, notes, distance)" +
                                            " SELECT d.* FROM (SELECT" +
                                            "          {0} AS time," +
                                            "          (select id from tbSystems  where systemname      = {1}" +
                                            "          ) AS system_id," +
                                            "          (select id from tbStations where stationname     = {2} " +
                                            "                                     and   system_id       = (select id from tbSystems" +
                                            "                                                               where systemname = {1})" +
                                            "          ) AS station_id," +
                                            "          (select id from tbEventType   where eventtype    = {3}) As event_id," +
                                            "          (select id from tbCommodity   where commodity    = {4} or loccommodity = {4} limit 1) As commodity_id," +
                                            "          (select id from tbCargoAction where cargoaction  = {5}) AS cargoaction_id," +
                                            "          {6} AS cargovolume," +
                                            "          {7} AS credits_transaction," +
                                            "          {8} AS credits_total," +
                                            "          {9} AS notes," +
                                            "          {10} AS distance) AS d" +
                                            " ON DUPLICATE KEY UPDATE" +
                                            "  system_id            = d.system_id," +
                                            "  station_id           = d.station_id," +
                                            "  event_id             = d.event_id," +
                                            "  commodity_id         = d.commodity_id," +
                                            "  cargoaction_id       = d.cargoaction_id," +
                                            "  cargovolume          = d.cargovolume," +
                                            "  credits_transaction  = d.credits_transaction," +
                                            "  credits_total        = d.credits_total," +
                                            "  notes                = d.notes," +
                                            "  distance             = d.distance",
                                            DBConnector.SQLDateTime(ChangedData.time),
                                            DBConnector.SQLAString(DBConnector.SQLEscape(ChangedData.systemname)),
                                            DBConnector.SQLAString(DBConnector.SQLEscape(ChangedData.stationname)),
                                            DBConnector.SQLAString(ChangedData.eventtype),
                                            DBConnector.SQLAString(ChangedData.loccommodity),
                                            DBConnector.SQLAString(ChangedData.cargoaction),
                                            ChangedData.cargovolume,
                                            ChangedData.credits_transaction,
                                            ChangedData.credits_total,
                                            ChangedData.notes.Trim() == String.Empty ? "null" : String.Format("'{0}'", DBConnector.SQLEscape(ChangedData.notes, true)),
                                            nDistance == null ? "null" : DBConnector.SQLDecimal(nDistance.Value));

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

        /// <summary>
        /// register the LogfileScanner in the CommandersLog for the DataEvent
        /// </summary>
        /// <param name="journalScanner"></param>
        public void registerJournalScanner(FileScanner.EDJournalScanner journalScanner)
        {
            try
            {
                if(m_JournalScanner == null)
                { 
                    m_JournalScanner = journalScanner;
                    m_JournalScanner.JournalEventRecieved += JournalEventRecieved;
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
        public void registerExternalTool(IBE.IBECompanion.DataEventBase ExternalDataInterface)
        {
            try
            {
                if(m_DataEventObject == null)
                { 
                    m_DataEventObject                    = ExternalDataInterface;
                    m_DataEventObject.ExternalDataEvent += m_ExternalDataInterface_ExternalDataEvent;
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
        /// unregister the LogfileScanner
        /// </summary>
        /// <param name="LogfileScanner"></param>
        public void UnregisterJournalScanner()
        {
            try
            {
                if(m_JournalScanner != null)
                { 
                    m_JournalScanner.JournalEventRecieved -= JournalEventRecieved;
                    m_JournalScanner = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while unregistering the LogfileScanner", ex);
            }
        }

        /// <summary>
        /// unregister the LogfileScanner
        /// </summary>
        /// <param name="LogfileScanner"></param>
        public void unregisterExternalTool()
        {
            try
            {
                if(m_DataEventObject != null)
                { 
                    m_DataEventObject.ExternalDataEvent -= m_ExternalDataInterface_ExternalDataEvent;
                    m_DataEventObject = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while unregistering the ExternalDataTool", ex);
            }
        }


        /// <summary>
        /// event-worker for JournalEventRecieved-event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void JournalEventRecieved(object sender, FileScanner.EDJournalScanner.JournalEventArgs e)
        {
            try
            {
                //NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                nfi = (NumberFormatInfo) nfi.Clone();
                nfi.CurrencySymbol = "";
                nfi.CurrencyDecimalDigits = 0;

                switch (e.EventType)
                {
                    case FileScanner.EDJournalScanner.JournalEvent.Docked:

                        if((!Program.actualCondition.System.EqualsNullOrEmpty(e.Data.Value<String>("StarSystem"))) || 
                           (!Program.actualCondition.Station.EqualsNullOrEmpty(e.Data.Value<String>("StationName"))))
                        {
                            if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_Visited", true.ToString(), false))
                                SaveEvent(DateTime.UtcNow, e.Data.Value<String>("StarSystem"), e.Data.Value<String>("StationName"), "", "", 0, 0, Program.CompanionIO.SGetCreditsTotal(), "Visited", "");
                        }
                        else if(e.History.Find(x => ((x.EventType == FileScanner.EDJournalScanner.JournalEvent.Resurrect) && (e.History.IndexOf(x) < 2))) != null)
                        {
                            var resurrectEvent = e.History.Find(x => ((x.EventType == FileScanner.EDJournalScanner.JournalEvent.Resurrect) && (e.History.IndexOf(x) < 2)));

                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      e.Data.Value<String>("StarSystem"), 
                                      e.Data.Value<String>("StationName"), 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Resurrect", 
                                      String.Format("option: {0}\ncost: {1} cr.\nbankrupt = {2}", 
                                                    resurrectEvent.Data.Value<String>("Option"), 
                                                    resurrectEvent.Data.Value<Int32>("Cost"), 
                                                    resurrectEvent.Data.Value<Boolean>("Bankrupt") ? "yes" : "no"));

                        }
                        
                        break;
                    case FileScanner.EDJournalScanner.JournalEvent.FSDJump:
                        if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_JumpedTo", true.ToString(), false))
                        {
                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      e.Data.Value<String>("StarSystem"),
                                      "", 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Jumped To", 
                                      "", 
                                      e.Data.Value<Double>("JumpDist"));
                        }
                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.Died:

                        String killInfo= "";
                        IBECompanion.CompanionConverter cmpConverter = new IBECompanion.CompanionConverter();  

                        if(e.Data.Value<String>("KillerName") != null)
                        {
                            
                            killInfo =  String.Format("killed by \t: {0}\n" +
                                                      "ship \t: {1}\n" +
                                                      "rank \t: {2}", 
                                                      (e.Data.Value<String>("KillerName_Localised") != null) ? e.Data.Value<String>("KillerName_Localised") : e.Data.Value<String>("KillerName"), 
                                                      cmpConverter.GetShipNameFromSymbol(Program.Data.BaseData.tbshipyardbase, e.Data.Value<String>("KillerShip")), 
                                                      e.Data.Value<String>("KillerRank")); 


                        }
                        else if(e.Data.Value<Object>("Killers") != null)
                        {
                            killInfo = "killed by wing:\n";
                            Int32 counter = 1;
                            foreach (JToken killerData in e.Data.SelectTokens("Killers.[*]"))
                            {

                                killInfo +=  String.Format("{4}{3}. \t{0}, \t{1}, \t{2}", 
                                                           killerData.Value<String>("Name"), 
                                                           cmpConverter.GetShipNameFromSymbol(Program.Data.BaseData.tbshipyardbase, killerData.Value<String>("Ship")), 
                                                           killerData.Value<String>("Rank"), 
                                                           counter, 
                                                           counter > 1 ? "\n":""); 
                                counter++;
                            }
                        }

                        SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                  Program.actualCondition.System, 
                                  "", 
                                  "", 
                                  "", 
                                  0, 
                                  0, 
                                  Program.CompanionIO.SGetCreditsTotal(), 
                                  "Died", 
                                  killInfo);
                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.Resurrect:
                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.Touchdown:
                        if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_TouchDown", true.ToString(), false))
                        {
                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      "", 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Touchdown", 
                                      String.Format("landed on {0}{1}\n" +
                                                    "long: {3}\n" +
                                                    "lat : {2}",
                                                    Program.actualCondition.Body, 
                                                    String.IsNullOrWhiteSpace(Program.actualCondition.BodyType) ? "" : " (" + Program.actualCondition.BodyType + ")",
                                                    e.Data.Value<Double>("Latitude"), 
                                                    e.Data.Value<Double>("Longitude")));
                        }
                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.Liftoff:
                        if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_Liftoff", true.ToString(), false))
                        {
                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      "", 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Liftoff", 
                                      String.Format("liftoff from {0}{1}\n" +
                                                    "long: {3}\n" +
                                                    "lat : {2}",
                                                    Program.actualCondition.Body, 
                                                    String.IsNullOrWhiteSpace(Program.actualCondition.BodyType) ? "" : " (" + Program.actualCondition.BodyType + ")",
                                                    e.Data.Value<Double>("Latitude"), 
                                                    e.Data.Value<Double>("Longitude")));
                        }
                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.Scan:
                        if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_Scan", true.ToString(), false))
                        {
                            TextHelper txtHelp = new TextHelper();

                            Font usedFont = m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font != null ? m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font : m_GUI.dgvCommandersLog.DefaultCellStyle.Font ; 
                            System.Text.StringBuilder data  = new System.Text.StringBuilder();
                            System.Text.StringBuilder rings = new System.Text.StringBuilder();
                         
                            Int32 fullLength = 170;

                            if(e.Data.Value<Object>("StarType") != null)
                            {
                                data.AppendLine(String.Format("{0} :   {1}   (Star)", txtHelp.FixedLength("Name", usedFont, fullLength),  e.Data.Value<String>("BodyName")));
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Type", usedFont, fullLength),  e.Data.Value<String>("StarType")));
                                data.AppendLine(String.Format("{0} :   {1:N1} ls", txtHelp.FixedLength("Distance", usedFont, fullLength),  e.Data.Value<Double>("DistanceFromArrivalLS")));

                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Radius")))
                                    data.AppendLine(String.Format("{0} :   {1:N1} km", txtHelp.FixedLength("Radius", usedFont, fullLength),  e.Data.Value<Double>("Radius")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("StellarMass")))
                                    data.AppendLine(String.Format("{0} :   {1:N1} stellar masses", txtHelp.FixedLength("Mass", usedFont, fullLength),  e.Data.Value<Double>("StellarMass")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Age_MY")))
                                    data.AppendLine(String.Format("{0} :   {1:N1} my", txtHelp.FixedLength("Age", usedFont, fullLength),  e.Data.Value<Double>("Age_MY")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("SurfaceTemperature")))
                                    data.AppendLine(String.Format("{0} :   {1:N1}°K  ( {2:N1}°C )", txtHelp.FixedLength("Temp.", usedFont, fullLength),  e.Data.Value<Double>("SurfaceTemperature"), e.Data.Value<Double>("SurfaceTemperature") - 273.15));
                            }
                            else
                            {
                                System.Text.StringBuilder materials = new System.Text.StringBuilder();

                                data.AppendLine(String.Format("{0} :   {1}   (Planet/Moon)", txtHelp.FixedLength("Name", usedFont, fullLength),  e.Data.Value<String>("BodyName")));
                                data.AppendLine(String.Format("{0} :   {1:N1} ls", txtHelp.FixedLength("Distance", usedFont, fullLength),  e.Data.Value<Double>("DistanceFromArrivalLS")));

                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("TerraformState")))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Terraform", usedFont, fullLength),  e.Data.Value<String>("TerraformState")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("PlanetClass")))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Class", usedFont, fullLength),  e.Data.Value<String>("PlanetClass")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Atmosphere")))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Atmosphere", usedFont, fullLength),  e.Data.Value<String>("Atmosphere")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Volcanism")))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Volcanism", usedFont, fullLength),  e.Data.Value<String>("Volcanism")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("SurfaceGravity")))
                                    data.AppendLine(String.Format("{0} :   {1:N2} g", txtHelp.FixedLength("Gravity", usedFont, fullLength),  e.Data.Value<Double>("SurfaceGravity")/10.0));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("SurfaceTemperature")))
                                    data.AppendLine(String.Format("{0} :   {1:N1}°K  ( {2:N1}°C )", txtHelp.FixedLength("Temp.", usedFont, fullLength),  e.Data.Value<Double>("SurfaceTemperature"), e.Data.Value<Double>("SurfaceTemperature") - 273.15));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("SurfacePressure")))
                                    data.AppendLine(String.Format("{0} :   {1:N1} atm", txtHelp.FixedLength("Pressure", usedFont, fullLength),  e.Data.Value<Double>("SurfacePressure") / 100000.0));
                                if((!String.IsNullOrWhiteSpace(e.Data.Value<String>("Landable"))) && e.Data.Value<Boolean>("Landable"))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Landable", usedFont, fullLength),  e.Data.Value<Boolean>("Landable") ? "yes" : "no"));

                                if (e.Data.Value<Object>("Materials") != null)
                                {
                                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                                    foreach (JProperty  material in e.Data.SelectToken("Materials"))
                                    {
                                        if(materials.Length == 0)
                                            materials.AppendFormat(String.Format("\n{0} :   ", txtHelp.FixedLength("Materials", usedFont, fullLength)));
                                        else
                                            materials.AppendFormat(", ");
                                        materials.AppendFormat("{0} : {1:N1}%", textInfo.ToTitleCase(material.Name), (Double)material.Value);
                                    }
                                    data.AppendLine(materials.ToString());
                                }
                            }

                            if(e.Data.Value<Object>("Rings") != null)
                            {
                                foreach (JObject ring in e.Data.SelectTokens("Rings.[*]"))
                                {
                                    String classname = ring.Value<String>("RingClass").Replace("eRingClass_","")
                                                                                      .Replace("Rich"," Rich")
                                                                                      .Replace("Metal","Metall");

                                    if(rings.Length == 0)
                                        rings.AppendLine(String.Format("\n{0} • {1} (Type: {2})", txtHelp.FixedLength("Belts", usedFont, 100), ring.Value<String>("Name"), classname));
                                    else
                                        rings.AppendLine(String.Format("{0} • {1} (Type: {2})", txtHelp.FixedLength("", usedFont, 100), ring.Value<String>("Name"), classname));
                                }

                                data.Append(rings);
                            }

                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      "", 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Scan", 
                                      data.ToString());
                        }
                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.MissionAccepted:
                        if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_MissionAccepted", true.ToString(), false))
                        {
                            TextHelper txtHelp = new TextHelper();

                            Font usedFont = m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font != null ? m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font : m_GUI.dgvCommandersLog.DefaultCellStyle.Font ; 
                            System.Text.StringBuilder data  = new System.Text.StringBuilder();
                            System.Text.StringBuilder rings = new System.Text.StringBuilder();
                         
                            Int32 fullLength = 190;

                            data.AppendLine(String.Format("{0} :   {1}   ( id : {2} )", txtHelp.FixedLength("Mission", usedFont, fullLength), System.Text.RegularExpressions.Regex.Replace(e.Data.Value<String>("Name"), "Mission_", "" , System.Text.RegularExpressions.RegexOptions.IgnoreCase).Replace("_name", "").Replace("_", " "), e.Data.Value<String>("MissionID")));
                            data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Faction", usedFont, fullLength),  e.Data.Value<String>("Faction")));
                            
                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Commodity_Localised")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Commodity", usedFont, fullLength),  e.Data.Value<String>("Commodity_Localised")));
                            else if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Commodity")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Commodity", usedFont, fullLength),  e.Data.Value<String>("Commodity")));

                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Count")))
                                data.AppendLine(String.Format("{0} :   {1} t", txtHelp.FixedLength("Count", usedFont, fullLength),  e.Data.Value<String>("Count")));

                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Target")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Target", usedFont, fullLength),  e.Data.Value<String>("Target")));
                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("TargetType")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("TargetType", usedFont, fullLength),  e.Data.Value<String>("TargetType")));
                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("TargetFaction")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("TargetFaction", usedFont, fullLength),  e.Data.Value<String>("TargetFaction")));

                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Expiry")))
                            {
                                DateTime expTime = DateTime.ParseExact(e.Data.Value<String>("Expiry"), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                                data.AppendLine(String.Format("{0} :   {1:d} {1:t} ({2})", txtHelp.FixedLength("Expiry", usedFont, fullLength), expTime, (expTime - DateTime.UtcNow).ToReadableString()));
                            }
                                
                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("DestinationSystem")))
                            {
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("DestinationStation")))
                                    data.AppendLine(String.Format("{0} :   {2} / {1}", txtHelp.FixedLength("Destination", usedFont, fullLength),  
                                                                                       e.Data.Value<String>("DestinationStation"), 
                                                                                       e.Data.Value<String>("DestinationSystem")));
                                else
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Destination", usedFont, fullLength),  e.Data.Value<String>("DestinationSystem")));
                            }
                            else if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("DestinationStation")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Destination", usedFont, fullLength),  e.Data.Value<String>("DestinationStation")));

                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("PassengerCount")))
                            {
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("PassengerCount", usedFont, fullLength),  e.Data.Value<String>("Passengers")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("PassengerVIPs")))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("PassengerVIPs", usedFont, fullLength),  e.Data.Value<String>("Is VIP")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("PassengerWanted")))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("PassengerWanted", usedFont, fullLength),  e.Data.Value<String>("Is wanted")));
                                if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("PassenderType")))
                                    data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("PassenderType", usedFont, fullLength),  e.Data.Value<String>("Type")));
                            }

                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      Program.actualCondition.Station, 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Mission Accepted", 
                                      data.ToString());
                        }

                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.MissionCompleted:
                        if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_MissionCompleted", true.ToString(), false))
                        {
                            TextHelper txtHelp = new TextHelper();

                            Font usedFont = m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font != null ? m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font : m_GUI.dgvCommandersLog.DefaultCellStyle.Font ; 
                            System.Text.StringBuilder data  = new System.Text.StringBuilder();
                         
                            Int32 fullLength = 190;

                            data.AppendLine(String.Format("{0} :   {1}   ( id : {2} )", txtHelp.FixedLength("Mission", usedFont, fullLength), System.Text.RegularExpressions.Regex.Replace(e.Data.Value<String>("Name"), "Mission_", "" , System.Text.RegularExpressions.RegexOptions.IgnoreCase).Replace("_name", "").Replace("_", " "), e.Data.Value<String>("MissionID")));
                            data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Faction", usedFont, fullLength),  e.Data.Value<String>("Faction")));
                            
                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Commodity_Localised")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Commodity", usedFont, fullLength),  e.Data.Value<String>("Commodity_Localised")));
                            else if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Commodity")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Commodity", usedFont, fullLength),  e.Data.Value<String>("Commodity")));

                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Count")))
                                data.AppendLine(String.Format("{0} :   {1} t", txtHelp.FixedLength("Count", usedFont, fullLength),  e.Data.Value<String>("Count")));

                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Target")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Target", usedFont, fullLength),  e.Data.Value<String>("Target")));
                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("TargetType")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("TargetType", usedFont, fullLength),  e.Data.Value<String>("TargetType")));
                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("TargetFaction")))
                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("TargetFaction", usedFont, fullLength),  e.Data.Value<String>("TargetFaction")));

                            if (!String.IsNullOrWhiteSpace(e.Data.Value<String>("Reward")))
                                data.AppendLine(String.Format("{0} :   {1} cr.", txtHelp.FixedLength("Reward", usedFont, fullLength),  e.Data.Value<Int32>("Reward")));


                            if(e.Data.Value<Object>("CommodityReward") != null)
                            {
                                System.Text.StringBuilder commodityRewards = new System.Text.StringBuilder();

                                foreach (JObject ring in e.Data.SelectTokens("CommodityReward.[*]"))
                                {
                                    if(commodityRewards.Length > 0 )
                                        commodityRewards.Append(",");

                                    commodityRewards.Append(String.Format("{1} t {0}", ring.SelectToken("Name"), ring.SelectToken("Count")));
                                }

                                data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Rewards", usedFont, fullLength), commodityRewards));
                            }
                                

                            if (!String.IsNullOrWhiteSpace(e.Data.Value<String>("Donation")))
                                data.AppendLine(String.Format("{0} :   {1} cr.", txtHelp.FixedLength("Donation", usedFont, fullLength),  e.Data.Value<Int32>("Donation")));

                            if (e.Data.Value<Object>("PermitsAwarded") != null)
                            {
                                System.Text.StringBuilder permits = new System.Text.StringBuilder();

                                foreach (JToken permitItem in e.Data.SelectTokens("PermitsAwarded.[*]"))
                                {
                                    if(permits.Length > 0 )
                                        permits.Append(",");

                                    permits.Append(permitItem.Value<String>());
                                }

                                data.AppendLine(String.Format("{0} :   {1} cr.", txtHelp.FixedLength("Permits", usedFont, fullLength), permits));
                            }


                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      Program.actualCondition.Station, 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Mission Completed", 
                                      data.ToString());

                        }

                        break;
                    case FileScanner.EDJournalScanner.JournalEvent.MissionFailed:

                        if(Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_MissionFailed", true.ToString(), false))
                        {
                            TextHelper txtHelp = new TextHelper();

                            Font usedFont = m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font != null ? m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font : m_GUI.dgvCommandersLog.DefaultCellStyle.Font ; 
                            System.Text.StringBuilder data  = new System.Text.StringBuilder();
                         
                            Int32 fullLength = 190;

                            data.AppendLine(String.Format("{0} :   {1}   ( id : {2} )", txtHelp.FixedLength("Mission", usedFont, fullLength), System.Text.RegularExpressions.Regex.Replace(e.Data.Value<String>("Name"), "Mission_", "" , System.Text.RegularExpressions.RegexOptions.IgnoreCase).Replace("_name", "").Replace("_", " "), e.Data.Value<String>("MissionID")));
                            
                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      Program.actualCondition.Station, 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Mission Failed", 
                                      data.ToString());

                        }
                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.MissionAbandoned:

                        if (Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_MissionAbandoned", true.ToString(), false))
                        {
                            TextHelper txtHelp = new TextHelper();

                            Font usedFont = m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font != null ? m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font : m_GUI.dgvCommandersLog.DefaultCellStyle.Font ; 
                            System.Text.StringBuilder data  = new System.Text.StringBuilder();
                         
                            Int32 fullLength = 190;

                            data.AppendLine(String.Format("{0} :   {1}   ( id : {2} )", txtHelp.FixedLength("Mission", usedFont, fullLength), System.Text.RegularExpressions.Regex.Replace(e.Data.Value<String>("Name"), "Mission_", "" , System.Text.RegularExpressions.RegexOptions.IgnoreCase).Replace("_name", "").Replace("_", " "), e.Data.Value<String>("MissionID")));
                            
                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      Program.actualCondition.Station, 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Mission Abandoned", 
                                      data.ToString());

                        }

                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.LoadGame:

                        if (Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_LoadGame", true.ToString(), false))
                        {
                            TextHelper txtHelp = new TextHelper();

                            Font usedFont = m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font != null ? m_GUI.dgvCommandersLog.Columns["notes"].DefaultCellStyle.Font : m_GUI.dgvCommandersLog.DefaultCellStyle.Font ; 
                            System.Text.StringBuilder data  = new System.Text.StringBuilder();
                         
                            Int32 fullLength = 190;

                            data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Cmdr.", usedFont, fullLength), e.Data.Value<String>("Commander")));
                            data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Ship", usedFont, fullLength), Program.Data.GetShipname(e.Data.Value<String>("Ship"))));

                            String group = "";

                            if(!String.IsNullOrWhiteSpace(e.Data.Value<String>("Group")))
                                group = String.Format(" \"{0}\"", e.Data.Value<String>("Group"));

                            data.AppendLine(String.Format("{0} :   {1}{2}", txtHelp.FixedLength("Mode", usedFont, fullLength), e.Data.Value<String>("GameMode"), group));
                            data.AppendLine(String.Format("{0} :   {1}", txtHelp.FixedLength("Credits", usedFont, fullLength), e.Data.Value<Int32>("Credits")));

                            if(e.Data.Value<Int32>("Loan") > 0)
                                data.AppendLine(String.Format("{0} :   {1}{2}", txtHelp.FixedLength("Loan", usedFont, fullLength), e.Data.Value<Int32>("Loan"), group));
                            
                            SaveEvent(e.Data.Value<DateTime>("timestamp"), 
                                      Program.actualCondition.System,
                                      Program.actualCondition.Station, 
                                      "", 
                                      "", 
                                      0, 
                                      0, 
                                      Program.CompanionIO.SGetCreditsTotal(), 
                                      "Load Game", 
                                      data.ToString());

                        }

                        break;
                }

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while processing the JournalEventRecieved-event");
            }
        }

        void m_ExternalDataInterface_ExternalDataEvent(object sender, IBE.IBECompanion.DataEventBase.LocationChangedEventArgs e)
        {
            try
            {
                if((e.Changed & IBE.IBECompanion.DataEventBase.enExternalDataEvents.DataCollected) > 0)
                {
                    createMarketdataCollectedEvent();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the LocationChanged-event", ex);
            }
        }

        /// <summary>
        /// creates a MarketDataCollected-event for the current station
        /// </summary>
        public void createMarketdataCollectedEvent()
        {
            try
            {
                if (Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_Marketdata", true.ToString(), false, true))
                {
                    if (Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoAdd_ReplaceVisited", true.ToString(), false, true))
                    {

                        String sqlString                = "select * from tbLog" +
                                                          " order by time desc limit 1";

                        DataTable Data   = new DataTable();

                        if(Program.DBCon.Execute(sqlString, Data) == 1)
                        {
                            if(((Int32)Data.Rows[0]["system_id"]  == Program.actualCondition.System_ID) && 
                               ((System.Convert.IsDBNull(Data.Rows[0]["station_id"]) ? null : (int?)Data.Rows[0]["station_id"])  == Program.actualCondition.Station_ID) && 
                               ((Int32)Data.Rows[0]["event_id"]   == (Int32)Program.Data.BaseTableNameToID("EventType", "Visited")))
                            {
                                // change existing
                                sqlString = "update tbLog" +
                                            " set event_id = " + (Int32)Program.Data.BaseTableNameToID("EventType", "Market Data Collected") +
                                            " where time   = " + DBConnector.SQLDateTime((DateTime)Data.Rows[0]["time"]);

                                Program.DBCon.Execute(sqlString);

                                if(!m_NoGuiNotifyAfterSave)
                                {
                                    DataChanged.Raise(this, new DataChangedEventArgs() { DataRow = 0, DataKey = (DateTime)Data.Rows[0]["time"]});                     
                                }

                            }
                            else
                            {
                                // add new
                                Program.CommandersLog.SaveEvent(DateTime.UtcNow, Program.actualCondition.System, 
                                                                Program.actualCondition.Station, "", "", 0, 0, 0, 
                                                                "Market Data Collected", "");
                            }
                        }
                    }
                    else
                    {
                        // add new
                        Program.CommandersLog.SaveEvent(DateTime.UtcNow, Program.actualCondition.System, 
                                                        Program.actualCondition.Station, "", "", 0, 0, 0, 
                                                        "Market Data Collected", "");
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating a MarketDataCollected-event", ex);
            }
        }

        /// <summary>
        /// deletes the rows in the database
        /// </summary>
        /// <param name="markedRows"></param>
        internal void DeleteRows(DataGridViewSelectedRowCollection markedRows)
        {
            System.Text.StringBuilder sqlString = new System.Text.StringBuilder();

            try
            {
                sqlString.Append("delete from tbLog where ");

                for (int i = 0; i < markedRows.Count; i++)
                {
                    if(i == 0)
                        sqlString.Append(String.Format(" time = {0}", DBConnector.SQLDateTime((DateTime)markedRows[i].Cells["time"].Value)));   
                    else
                        sqlString.Append(String.Format(" or time = {0} ", DBConnector.SQLDateTime((DateTime)markedRows[i].Cells["time"].Value)));   
                }

                Program.DBCon.Execute(sqlString.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting rows", ex);
            }
        }

        /// <summary>
        /// loads only the systems matching the current input
        /// </summary>
        /// <param name="systemString"></param>
        /// <param name="cBox"></param>
        public void LoadSystemComboBoxData(string systemString, ComboBox cBox)
        {
            BindingSource currentBS = null;
            DataTable currentDt = null;

            try
            {
                currentBS = m_BindingSources[cBox];
                currentDt = (DataTable)currentBS.DataSource;

                if (systemString != "")
                {
                    String sqlString = "select * from tbSystems where systemName like " + DBConnector.SQLAString(DBConnector.SQLEscape(systemString) + "%");
                    Program.DBCon.Execute(sqlString, currentDt);
                }
                else
                {
                    currentDt.Clear();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading specific stationdata", ex);
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

