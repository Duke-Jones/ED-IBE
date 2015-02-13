using System;
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
        // Commander's Log Functionality
        public SortableBindingList<CommandersLogEvent> LogEvents { get; set; }

        private readonly Form1 _callingForm;

        public CommandersLog(Form1 callingForm)
        {
            _callingForm = callingForm;
            
            LogEvents = new SortableBindingList<CommandersLogEvent>();
        }

        public void CreateEvent()
        {
            _callingForm.tbLogEventID.Text = Guid.NewGuid().ToString();

            LogEvents.Add(new CommandersLogEvent 
            {
                EventType =_callingForm.cbLogEventType.Text,
                Station =_callingForm.cbLogStationName.Text,
                System =_callingForm.cbLogSystemName.Text,
                Cargo =_callingForm.cbLogCargoName.Text,
                CargoAction =_callingForm.cbCargoModifier.Text,
                CargoVolume =int.Parse(_callingForm.cbLogQuantity.Text),
                Notes =_callingForm.tbLogNotes.Text,
                EventDate =DateTime.Parse(_callingForm.dtpLogEventDate.Text),
                EventID =_callingForm.tbLogEventID.Text
            });
        }

        public void CreateEvent(string eventType, string station, string system, string cargo, string cargoAction, int cargoVolume, string notes, DateTime eventDate)
        {
            LogEvents.Add(new CommandersLogEvent
            {
                EventType =               eventType                  ,
                Station =                 station                    ,
                System =                  system                     ,
                Cargo =                   cargo                      ,
                CargoAction =             cargoAction                ,
                CargoVolume =             cargoVolume                ,
                Notes =                   notes                      ,
                EventDate        =        eventDate                  ,
                EventID = Guid.NewGuid().ToString()
            });

            UpdateCommandersLogListView();
        }

        public void CreateNewEvent() // Clears the fields ready for input
        {
            var now =DateTime.UtcNow;
            ClearLogEventFields();
            _callingForm.dtpLogEventDate.Value =now;
            _callingForm.tbLogEventID.Text ="";
            _callingForm.button21.Text = "Create This Entry And Clear";
        }

        public void CreateEvent(CommandersLogEvent partiallyCompleteCommandersLogEventEvent) // when we create from the webserver
        {
            var now = DateTime.UtcNow;
            var newGuid = Guid.NewGuid().ToString();
            ClearLogEventFields();
            _callingForm.dtpLogEventDate.Value = now;
            _callingForm.tbLogEventID.Text = newGuid;
            partiallyCompleteCommandersLogEventEvent.EventID = Guid.NewGuid().ToString();
            partiallyCompleteCommandersLogEventEvent.EventDate = now;
            
            LogEvents.Add(partiallyCompleteCommandersLogEventEvent);
        }

        private void ClearLogEventFields()
        {
            _callingForm.cbLogEventType.Text = "";
            _callingForm.cbLogStationName.Text = "";
            _callingForm.cbLogSystemName.Text = "";
            _callingForm.cbLogCargoName.Text = "";
            _callingForm.cbCargoModifier.Text = "";
            _callingForm.cbLogQuantity.Text = "0";
            _callingForm.tbLogNotes.Text = "";
            _callingForm.nbTransactionAmount.Text = "0";

            _callingForm.UpdateSystemNameFromLogFile();

            if (_callingForm.tbCurrentSystemFromLogs.Text != "")
                _callingForm.cbLogSystemName.Text = _callingForm.tbCurrentSystemFromLogs.Text;
        }

        public void SaveLog(bool force = false)
        {
            string newFile, backupFile, currentFile;

            if (force)
                currentFile = "CommandersLogAutoSave.xml";
            else
                currentFile = "Commander's Log Events to " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".xml";

            newFile     = String.Format("{0}_new{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));
            backupFile  = String.Format("{0}_bak{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));

            Stream stream = new FileStream(newFile, FileMode.Create, FileAccess.Write, FileShare.None);
            var x =new XmlSerializer(LogEvents.GetType());
            x.Serialize(stream, LogEvents);
            stream.Close();

            // we delete the current file not until the new file is written without errors

            if (force)
            {
                // delete old backup
                if (File.Exists(backupFile))
                    File.Delete(backupFile);

                // rename current file to old backup
                if (File.Exists(currentFile))
                    File.Move(currentFile, backupFile);
            }
            else
            {
                // delete file if exists
                if (File.Exists(currentFile))
                    File.Delete(currentFile);

            }

            // rename new file to current file
            File.Move(newFile, currentFile);

        }

        public void LoadLog(bool force = false)
        {
            try
            {
                var openFile = new OpenFileDialog
                {
                    DefaultExt = "xml",
                    Multiselect = false,
                    Filter = "XML (*.xml)|*.xml",
                    InitialDirectory = Environment.CurrentDirectory
                };

                if (!force)
                    openFile.ShowDialog();

                if (force || openFile.FileNames.Length > 0)
                {
                    var serializer = new XmlSerializer(typeof(SortableBindingList<CommandersLogEvent>));

                    if (force && !File.Exists("CommandersLogAutoSave.xml"))
                        return;

                    var fs = new FileStream(force ? "CommandersLogAutoSave.xml" : openFile.FileName, FileMode.Open);
                    var reader = XmlReader.Create(fs);

                    var logEvents2 = (SortableBindingList<CommandersLogEvent>)serializer.Deserialize(reader);
                    LogEvents = logEvents2;
                    fs.Close();
                }

                UpdateCommandersLogListView();
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading CommandersLog", ex);
            }
        }

        public void UpdateCommandersLogListView()
        {
            _callingForm.lvCommandersLog.Items.Clear();
            foreach (var x in LogEvents)
            {
                var listViewData = new string[_callingForm.LogEventProperties.Count()];

                listViewData[0] = x.EventDate.ToString(CultureInfo.InvariantCulture);

                int ctr = 1;
                foreach (var y in _callingForm.LogEventProperties)
                {
                    if (y.Name != "EventDate")
                    {
                        listViewData[ctr] = y.GetValue(x).ToString();
                        ctr++;
                    }
                }

                _callingForm.lvCommandersLog.Items.Add(new ListViewItem(listViewData));
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
