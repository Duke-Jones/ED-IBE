using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace RegulatedNoise
{
    public class CommandersLog
    {
        public enum enGUIElements
        {
            cmbEventType
        }


        BindingSource m_BindingSource;
        DataTable     m_Datatable;

        public CommandersLog()
        {
            m_BindingSource = new BindingSource();
            m_Datatable     = new DataTable();

            m_BindingSource.DataSource = m_Datatable;
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
            //    CargoAction =_callingForm.cbCargoModifier.Text,
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
            //_callingForm.cbCargoModifier.Text = "";
            //_callingForm.cbLogQuantity.Text = "0";
            //_callingForm.tbLogNotes.Text = "";
            //_callingForm.nbTransactionAmount.Text = "0";

            //_callingForm.UpdateSystemNameFromLogFile();

            //if (_callingForm.tbCurrentSystemFromLogs.Text != "")
            //    _callingForm.cbLogSystemName.Text = _callingForm.tbCurrentSystemFromLogs.Text;

            //if (_callingForm.tbCurrentStationinfoFromLogs.Text != "")
            //    _callingForm.cbLogStationName.Text = _callingForm.tbCurrentStationinfoFromLogs.Text;
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

        internal void LoadData(DataGridView dataGridView, RequestParams Parameters)
        {
            try
            {
                
                dataGridView.SuspendLayout();

                String sqlString = "select L.time, S.systemname, St.stationname, E.event As eevent, C.action, Co.loccommodity, L.cargovolume, L.credits_transaction, L.credits_total, L.notes" +
                                   " from tbLog L left join tbEventType E   on L.event_id       = E.id" + 
                                   "              left join tbCargoAction C on L.cargoaction_id = C.id" +
                                   "              left join tbSystems S     on L.system_id      = S.id" +
                                   "              left join tbStations St   on L.station_id     = St.id" +
                                   "              left join tbCommodity Co  on L.commodity_id   = Co.id" +
                                   " order by time desc limit 1000";

                Program.DBCon.Execute(sqlString, ref m_Datatable);

                m_BindingSource.DataSource = m_Datatable;

                dataGridView.DataSource = null;
                dataGridView.DataSource = m_BindingSource;

                dataGridView.ResumeLayout();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Commanders Log", ex);    
            }
        }

        internal void prepareCmb_EventTypes(ref ComboBox_ro cbLogEventType, enGUIElements GUIElement, DataSet BaseData)
        {
            try
            {
                switch (GUIElement)
                {
                    case enGUIElements.cmbEventType:
                        cbLogEventType.DataSource       = BaseData.Tables["tbEventType"];
                        cbLogEventType.ValueMember      = "id";
                        cbLogEventType.DisplayMember    = "event";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error in <prepareCmb_EventTypes> while preparing '" + GUIElement.ToString() + "'", ex);
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
        public decimal  Credits { get; set; }
    }

}

