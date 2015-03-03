using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;


namespace RegulatedNoise.Enums_and_Utility_Classes
{
    internal class StationVisit
    {

        public StationVisit(string newStation, DateTime dateTime)
        {
            Station         = newStation;
            Visited         = dateTime;
        }
        [JsonProperty("station")]
        public string Station { get; set; }

        [JsonProperty("visited")]
        public DateTime Visited { get; set; }

    }

    internal class StationHistory
    {
        internal List<StationVisit> History;
        private int _maxLength;
        private string _FileName;
        private PerformanceTimer RetryTimer;
        private string m_lastInserted;

        public bool AutoSave { get; set; }

        internal StationHistory()
        { 
            AutoSave        = true;   
            _maxLength      = 100;
            History         = new List<StationVisit>();
            RetryTimer      = new PerformanceTimer();
            m_lastInserted  = String.Empty;

            RetryTimer.startMeasuring();
        }
            
        /// <summary>
        /// loads the history data from a file
        /// </summary>
        /// <param name="File">json-file to load</param>
        /// <param name="Stationtype"></param>
        internal void loadHistory(string Filename, bool createNonExistingFile)
        { 
            if (File.Exists(Filename))
                History  = JsonConvert.DeserializeObject<List<StationVisit>>(File.ReadAllText(Filename));
            else
            { 
                History = new List<StationVisit>();

                if (createNonExistingFile)
                    saveHistory(Filename, true);
            }
             
            _FileName = Filename;
        }

        /// <summary>
        /// adds a new visit with the current time
        /// </summary>
        /// <param name="newStation">Station ID (with Stationname and Systemname)</param>
        internal void addVisit(string newStation)
        { 
            StationVisit Visit;

            try
            {
                if (((RetryTimer.currentMeasuring() > 1000) || !m_lastInserted.Equals(newStation)) && !String.IsNullOrEmpty(newStation))
                {

                    int currentIndex = History.FindIndex(x => x.Station.Equals(newStation, StringComparison.InvariantCultureIgnoreCase));

                    if (currentIndex >= 0)
                    {
                        if (currentIndex == 0)
                        {
                            // refresh time only 
                            History[0].Visited = DateTime.Now;
                        }
                        else
                        {
                            // pull the existing item up and set current time
                            Visit = History[currentIndex];
                            History.Remove(Visit);
                            Visit.Visited = DateTime.Now;
                            History.Insert(0, Visit);
                        }
                    }
                    else
                    {
                        History.Insert(0, new StationVisit(newStation, DateTime.Now));
                    }

                    // cut to max. length
                    if (History.Count > _maxLength)
                        History.RemoveRange(_maxLength - 1, History.Count - _maxLength);

                    if (AutoSave && !String.IsNullOrEmpty(_FileName))
                        saveHistory(_FileName, true);

                    m_lastInserted = newStation;
                    RetryTimer.startMeasuring();
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message)    ;
            }
        }

        /// <summary>
        /// saves the history data to a file
        /// </summary>
        /// <param name="File">json-file to save</param>
        /// <param name="Stationtype"></param>
        internal void saveHistory(string Filename, bool BackupOldFile)
        { 
            string newFile, backupFile;

            newFile = String.Format("{0}_new{1}", Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename)), Path.GetExtension(Filename));
            backupFile = String.Format("{0}_bak{1}", Path.Combine(Path.GetDirectoryName(Filename), Path.GetFileNameWithoutExtension(Filename)), Path.GetExtension(Filename));

            File.WriteAllText(newFile, JsonConvert.SerializeObject(History));
            
            // we delete the current file not until the new file is written without errors

            rotateSaveFiles(Filename, newFile, backupFile, BackupOldFile);
        }

        internal static void rotateSaveFiles(string Filename, string newFile, string backupFile, bool BackupOldFile)
        {
            // delete old backup
            if (File.Exists(backupFile))
                File.Delete(backupFile);

            // rename current file to old backup
            if (BackupOldFile && File.Exists(Filename))
                File.Move(Filename, backupFile);

            // rename new file to current file
            File.Move(newFile, Filename);
        }

        /// <summary>
        /// renames a station
        /// </summary>
        /// <param name="existingStationName"></param>
        /// <param name="newStationName"></param>
        internal void RenameStation(string existingStationName, string newStationName)
        {
            int StationIndex = History.FindIndex(x => x.Station.Equals(existingStationName, StringComparison.InvariantCultureIgnoreCase));

            if (StationIndex >= 0)
                History[StationIndex].Station = newStationName;
        }
    }
}
