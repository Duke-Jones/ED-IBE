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

namespace RegulatedNoise.Commander_s_Log
{
    public class CommandersLog
    {
        public enum enGUIElements
        {
            cbLogEventType,
            cbLogSystemName,
            cbLogStationName,
            cbLogCargoName,
            cbCargoAction
        }

        private const String table = "tbLog";


        private const String _sqlString = "select L.time, S.systemname, St.stationname, E.event As eevent, C.action, Co.loccommodity, L.cargovolume, L.credits_transaction, L.credits_total, L.notes" +
                                         " from tbLog L left join tbEventType E   on L.event_id       = E.id" + 
                                         "              left join tbCargoAction C on L.cargoaction_id = C.id" +
                                         "              left join tbSystems S     on L.system_id      = S.id" +
                                         "              left join tbStations St   on L.station_id     = St.id" +
                                         "              left join tbCommodity Co  on L.commodity_id   = Co.id" +
                                         " order by time desc";

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

        internal int InitRetriever()
        {
            try
            {
                retriever = new DataRetriever(Program.DBCon, table, _sqlString);

                return retriever.RowCount;
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error in InitRetriever", ex);
            }

            
        }

        public DataRetriever Retriever
        {
            get
            {
                return retriever;
            }
        }

        /// <summary>
        /// gets or sets the base dataset
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

        //public void SaveLog(bool force = false)
        //{
        //    string newFile, backupFile, currentFile;

        //    if (force)
        //        currentFile = "CommandersLogAutoSave.xml";
        //    else
        //        currentFile = "Commander's Log Events to " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".xml";

        //    newFile     = String.Format("{0}_new{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));
        //    backupFile  = String.Format("{0}_bak{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));

        //    Stream stream = new FileStream(newFile, FileMode.Create, FileAccess.Write, FileShare.None);
        //    var x =new XmlSerializer(LogEvents.GetType());
        //    x.Serialize(stream, LogEvents);
        //    stream.Close();

        //    // we delete the current file not until the new file is written without errors

        //    if (force)
        //    {
        //        // delete old backup
        //        if (File.Exists(backupFile))
        //            File.Delete(backupFile);

        //        // rename current file to old backup
        //        if (File.Exists(currentFile))
        //            File.Move(currentFile, backupFile);
        //    }
        //    else
        //    {
        //        // delete file if exists
        //        if (File.Exists(currentFile))
        //            File.Delete(currentFile);

        //    }

        //    // rename new file to current file
        //    File.Move(newFile, currentFile);

        //}

        //public void LoadLog(bool force = false)
        //{
        //    try
        //    {
        //        var openFile = new OpenFileDialog
        //        {
        //            DefaultExt = "xml",
        //            Multiselect = false,
        //            Filter = "XML (*.xml)|*.xml",
        //            InitialDirectory = Environment.CurrentDirectory
        //        };

        //        if (!force)
        //            openFile.ShowDialog();

        //        if (force || openFile.FileNames.Length > 0)
        //        {
        //            var serializer = new XmlSerializer(typeof(SortableBindingList<CommandersLogEvent>));

        //            if (force && !File.Exists("CommandersLogAutoSave.xml"))
        //                return;

        //            var fs = new FileStream(force ? "CommandersLogAutoSave.xml" : openFile.FileName, FileMode.Open);
        //            var reader = XmlReader.Create(fs);

        //            var logEvents2 = (SortableBindingList<CommandersLogEvent>)serializer.Deserialize(reader);
        //            LogEvents = logEvents2;
        //            fs.Close();
        //        }

        //        isLoaded = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error loading CommandersLog", ex);
        //    }
        //}

        public void UpdateCommandersLogListView()
        {
            //_callingForm.lvCommandersLog.SuspendLayout();
            //_callingForm.lvCommandersLog.BeginUpdate();

            //_callingForm.lvCommandersLog.Items.Clear();
            //foreach (var x in LogEvents)
            //{
            //    var listViewData = new string[_callingForm.LogEventProperties.Count()];

            //    listViewData[0] = x.EventDate.ToString(CultureInfo.CurrentUICulture);

            //    int ctr = 1;
            //    foreach (var y in _callingForm.LogEventProperties)
            //    {
            //        if (y.Name != "EventDate")
            //        {
            //            listViewData[ctr] = y.GetValue(x).ToString();
            //            ctr++;
            //        }
            //    }

            //    _callingForm.lvCommandersLog.Items.Add(new ListViewItem(listViewData));
            //}
            //_callingForm.lvCommandersLog.EndUpdate();
            //_callingForm.lvCommandersLog.ResumeLayout();
        }

        public class RequestParams
        {
            public Int32 Limit;
        }

        //internal void LoadData(DataGridView dataGridView, RequestParams Parameters)
        //{
        //    try
        //    {
                
        //        dataGridView.SuspendLayout();

        //        _SqlString = "select L.time, S.systemname, St.stationname, E.event As eevent, C.action, Co.loccommodity, L.cargovolume, L.credits_transaction, L.credits_total, L.notes" + " from tbLog L left join tbEventType E   on L.event_id       = E.id" + "              left join tbCargoAction C on L.cargoaction_id = C.id" + "              left join tbSystems S     on L.system_id      = S.id" + "              left join tbStations St   on L.station_id     = St.id" + "              left join tbCommodity Co  on L.commodity_id   = Co.id" + " order by time desc";

        //        if(Parameters.Limit > 0)
        //            _SqlString += " limit 1000";

        //        Program.DBCon.Execute(_SqlString, m_Datatable);

        //        m_BindingSource.DataSource = m_Datatable;

        //        dataGridView.DataSource = null;
        //        dataGridView.DataSource = m_BindingSource;

        //        dataGridView.ResumeLayout();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while loading Commanders Log", ex);    
        //    }
        //}

        internal void SaveData(Enums_and_Utility_Classes.DataGridViewExt dataGridViewExt, int RowIndex)
        {
            String sqlString;

            try
            {
                sqlString = "insert into tbLog(time, system_id, station_id, event_id, commodity_id, cargoaction_id, cargovolume, credits_transaction, credits_total, notes) values" +
                            "";
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
// ReSharper disable once InconsistentNaming
        public string   EventID     { get; set; }
        public decimal  TransactionAmount { get; set; }
        public decimal  Credits     { get; set; }
    }

#region DataRetriever

    public class DataRetriever : DataRetrieverBase
    {

        public DataRetriever(SQL.DBConnector DBCon, string tableName, String DataStatement)
        {
            MySqlConnection connection  = (MySqlConnection)DBCon.Connection;
            command                     = connection.CreateCommand();
            this.tableName              = tableName;
            this.DataStatement          = DataStatement;

            memoryCache                 = new DataGridViewCache(this, 50);

        }

        private int rowCountValue = -1;

        public int RowCount
        {
            get
            {
                // Return the existing value if it has already been determined.
                if (rowCountValue != -1)
                {
                    return rowCountValue;
                }

                Object result = -1;
                // Retrieve the row count from the database.
                command.CommandText = "SELECT COUNT(*) FROM " + tableName;
                result = command.ExecuteScalar();
                if (result != null)
                    rowCountValue = Convert.ToInt32(result);

                return rowCountValue;
            }
        }

        private DataColumnCollection columnsValue;

        public DataColumnCollection Columns
        {
            get
            {
                // Return the existing value if it has already been determined.
                if (columnsValue != null)
                {
                    return columnsValue;
                }

                // Retrieve the column information from the database.
                //command.CommandText = "SELECT * FROM " + tableName;
                command.CommandText         = DataStatement;
                MySqlDataAdapter adapter    = new MySqlDataAdapter();
                adapter.SelectCommand       = command;
                DataTable table             = new DataTable();
                table.Locale                = System.Globalization.CultureInfo.InvariantCulture;

                adapter.FillSchema(table, SchemaType.Source);

                columnsValue                = table.Columns;

                return columnsValue;
            }
        }

        private string commaSeparatedListOfColumnNamesValue = null;
        private String usedPrefix = null;

        private string CommaSeparatedListOfColumnNames(String Prefix = "")
        {
            // Return the existing value if it has already been determined.
            if ((commaSeparatedListOfColumnNamesValue != null) && (Prefix == usedPrefix))
            {
                return commaSeparatedListOfColumnNamesValue;
            }

            // Store a list of column names for use in the
            // SupplyPageOfData method.
            System.Text.StringBuilder commaSeparatedColumnNames =
                new System.Text.StringBuilder();
            bool firstColumn = true;
            foreach (DataColumn column in Columns)
            {
                if (!firstColumn)
                {
                    commaSeparatedColumnNames.Append(", ");
                }
                if (!String.IsNullOrEmpty(Prefix))
                    commaSeparatedColumnNames.Append(Prefix);

                commaSeparatedColumnNames.Append(column.ColumnName);
                firstColumn = false;
            }

            commaSeparatedListOfColumnNamesValue    = commaSeparatedColumnNames.ToString();
            usedPrefix                              = Prefix;

            return commaSeparatedListOfColumnNamesValue;
        }

        // Declare variables to be reused by the SupplyPageOfData method.
        public enum SQLSortOrder
        {
            asc,
            desc
        }

        private string columnToSortBy;
        private SQLSortOrder ColumnSortOrder = SQLSortOrder.desc;

        private MySqlDataAdapter adapter = new MySqlDataAdapter();

        public override DataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage)
        {
            String sqlString;

            // Store the name of the ID column. This column must contain unique 
            // values so the SQL below will work properly.
            if (columnToSortBy == null)
            {
                columnToSortBy = this.Columns[0].ColumnName;
            }

            if (!this.Columns[columnToSortBy].Unique)
            {
                throw new InvalidOperationException(String.Format(
                    "Column {0} must contain unique values.", columnToSortBy));
            }

            // Retrieve the specified number of rows from the database, starting
            // with the row specified by the lowerPageBoundary parameter.
            //sqlString = String.Format("select {0}" + 
            //                          " from {2} L1 left join (" +
            //                          "                         select time from {2} L3 order by L3.{3} {5} limit {4}" +
            //                          "                       ) L2 on L1.{3} = L2.{3}" +
            //                          " where L2.{3} is null order by L1.{3} {5} limit {1}", 
            //                          CommaSeparatedListOfColumnNames("L1."), 
            //                          rowsPerPage, 
            //                          tableName, 
            //                          columnToSortBy, 
            //                          lowerPageBoundary, 
            //                          ColumnSortOrder);


            Debug.Print("retrieve Page " + lowerPageBoundary + " (" + rowsPerPage + ")");

            if(lowerPageBoundary == 150)
                Debug.Print("stop");

            sqlString = String.Format("select * from ({0}) L1 left join (" +
                                      "                         select time from {2} L3 order by L3.{3} {5} limit {4}" +
                                      "                       ) L2 on L1.{3} = L2.{3}" +
                                      " where L2.{3} is null order by L1.{3} {5} limit {1}", 
                                      DataStatement, 
                                      rowsPerPage, 
                                      tableName, 
                                      columnToSortBy, 
                                      lowerPageBoundary, 
                                      ColumnSortOrder);


            // Retrieve the specified number of rows from the database, starting
            // with the row specified by the lowerPageBoundary parameter.
            command.CommandText = sqlString;
            adapter.SelectCommand = command;

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            adapter.Fill(table);
            return table;
        }

    }

#endregion

}

