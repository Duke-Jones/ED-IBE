using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Diagnostics;
using IBE.Enums_and_Utility_Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IBE.FileScanner
{
    public class EDJournalScanner: IDisposable
    {

#region enums

        public enum JournalEvent
        {
            Undefined,
            Not_Found,
            Not_Supported,
            Fileheader,
            Location,
            Docked,
            Undocked,
            Liftoff,
            Touchdown,
            FSDJump,
            Died,
            Resurrect,
            SupercruiseEntry,
            SupercruiseExit, 
            Scan,
            Basedata, 
            MissionAccepted,
            MissionAbandoned,
            MissionCompleted,
            MissionFailed,
            LoadGame
        }

#endregion

#region event handler


        // "basedata" event
        [System.ComponentModel.Browsable(true)]
        public event EventHandler<BasedataEventArgs> BasedataEventRecieved;

        protected virtual void OnBasedataEventRecieved(BasedataEventArgs e)
        {
            EventHandler<BasedataEventArgs> myEvent = BasedataEventRecieved;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class BasedataEventArgs : EventArgs
        {
            public BasedataEventArgs()
            {
                EventType   = JournalEvent.Undefined;
                Coordinates = new Point3Dbl();
            }

            public JournalEvent                 EventType    { get; set; }

            public String                       System       { get; set; }

            public Point3Dbl                    Coordinates  { get; set; }

            public String                       Station      { get; set; }


            
        }


        // "event recieved" event
        [System.ComponentModel.Browsable(true)]
        public event EventHandler<JournalEventArgs> JournalEventRecieved;

        protected virtual void OnJournalEventRecieved(JournalEventArgs e)
        {
            EventHandler<JournalEventArgs> myEvent = JournalEventRecieved;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class JournalEventArgs : EventArgs
        {
            public JournalEventArgs()
            {
                EventType   = JournalEvent.Undefined;
                Data        = null;
            }

            public JournalEvent                 EventType    { get; set; }
            public JToken                       Data         { get; set; }
            public List<JournalEventArgs>       History      { get; set; }
        }

#endregion

#region disposing

    private bool disposed = false;

    //Implement IDisposable.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Stop();
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
            disposed = true;
        }
    }

    // Use C# destructor syntax for finalization code.
    ~EDJournalScanner()
    {
        // Simply call Dispose(false).
        Dispose (false);
    }

#endregion

    [System.Runtime.InteropServices.DllImport("shell32.dll", CharSet= System.Runtime.InteropServices.CharSet.Unicode)]
    static extern int SHGetKnownFolderPath([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);

        private const String        DB_GROUPNAME                    = "JournalScanner";

        static readonly Guid SAVED_GAMES            = new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4");     // GUID for getting the system path for "Saved games"

        private Thread                      m_JournalScanner_Thread;

        private String                      m_LastScan_JournalFile;
        private DateTime                    m_LastScan_Timestamp;
        private String                      m_LastScan_Event;

        private String                      m_SavedgamesPath;
        private Boolean                     m_Stop;
        private Boolean                     m_NewFileDetected;
        private FileSystemWatcher           m_FileWatcher;
        private Boolean                     m_extLogging = false;

        /// <summary>
        /// create a new LogFileScanner-object
        /// </summary>
        public EDJournalScanner()
        {
            try
            {
                if(true)
                {
                    m_LastScan_JournalFile                  = Program.DBCon.getIniValue<String>(DB_GROUPNAME,   "LastScan_JournalFile",  "", true);
                    m_LastScan_Event                        = Program.DBCon.getIniValue<String>(DB_GROUPNAME,   "LastScan_Event",        "", true);
                    m_LastScan_Timestamp                    = Program.DBCon.getIniValue<DateTime>(DB_GROUPNAME, "LastScan_TimeStamp",    new DateTime(2000, 1, 1).ToString(), false);

                    m_extLogging = Program.DBCon.getIniValue<Boolean>("Debug",   "extLog_Journal", false.ToString(), false);
                }
                else
                {
                    m_LastScan_JournalFile                  = "";
                    m_LastScan_Event                        = "";
                    m_LastScan_Timestamp                    = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating the object", ex);
            }
        }

        /// <summary>
        /// starts scanning of the logfile
        /// </summary>
        public void Start()
        {
            try
            {
                if (m_JournalScanner_Thread == null)
                {
                    m_SavedgamesPath = Program.DBCon.getIniValue<String>(IBESettingsView.DB_GROUPNAME, "JournalPath", "");

                    if(String.IsNullOrWhiteSpace(m_SavedgamesPath) || (!Directory.Exists(m_SavedgamesPath)))
                    {
                        if (SHGetKnownFolderPath(SAVED_GAMES, 0, IntPtr.Zero, out m_SavedgamesPath) != 0)
                            m_SavedgamesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games");

                        if(Directory.Exists(m_SavedgamesPath))
                            m_SavedgamesPath = Path.Combine(m_SavedgamesPath, @"Frontier Developments\Elite Dangerous");

                        if(!Directory.Exists(m_SavedgamesPath))
                        {
                            m_SavedgamesPath = null;
                            throw new Exception("ED-IBE can't find the \"Saved Games\" path to access the E:D journal file");
                        }
                        else
                        {
                            Program.DBCon.setIniValue(IBESettingsView.DB_GROUPNAME, "JournalPath", m_SavedgamesPath);
                        }
                    }

                    m_Stop = false;

                    m_JournalScanner_Thread                 = new Thread(new ThreadStart(JournalScannerWorker));
                    m_JournalScanner_Thread.Name            = "JournalScanner_Thread";
                    m_JournalScanner_Thread.IsBackground    = false;
                    m_JournalScanner_Thread.Start();

                    m_FileWatcher                           = new FileSystemWatcher(m_SavedgamesPath, "*.log");
                    m_FileWatcher.EnableRaisingEvents       = true;

                    m_FileWatcher.Created                  += FileWatcher_Created;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while starting the journal scanner", ex);
            }
            
        }

        /// <summary>
        /// stops scanning of the logfile
        /// </summary>
        public void Stop()
        {
            try
            {
                m_Stop = true;

                if(m_JournalScanner_Thread != null)
                {
                    do
                    {
                        // wait until thread is not running anymore
                        Thread.Sleep(25);
                    } while ((m_JournalScanner_Thread.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0);

                    m_FileWatcher.Dispose();
                    m_FileWatcher = null;

                    m_JournalScanner_Thread = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while stopping the journal scanner", ex);
            }
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Debug.Print("get Created Event : " + DateTime.Now);
            m_NewFileDetected = true;
        }



        private void JournalScannerWorker()
        {
            SingleThreadLogger logger           = new SingleThreadLogger(ThreadLoggerType.FileScanner);
            String latestFile                   = "";
            StreamReader journalStreamReader    = null;
            FileStream journalFileStream        = null;
            JournalEvent eventName;
            String rawEventName;
            DateTime rawTimeStamp;
            string dataLine;
            JToken journalEntry = "";
            List<String> newFiles = new List<string>();
            Boolean isFirstRun = true;
            Boolean gotLatestEvent = false;
            JToken latestLocationEvent = null;
            JToken latestFileHeader = null;
            String lastEvent = "";
            DateTime lastEventTime = DateTime.MinValue;
            List<JournalEventArgs> history = new List<JournalEventArgs>();
            Boolean isZeroRun = false;
            m_NewFileDetected = false;
            Boolean missingMessagePossible = true;
            Int32 errorCount = 0;
            Boolean parsingError = false;

            if(m_extLogging) logger.Log("scanning started");

            do
            {
                try
                {
                    if(!isFirstRun && missingMessagePossible && String.IsNullOrWhiteSpace(m_LastScan_JournalFile))
                    {
                        if(m_extLogging) logger.Log("Can't find E:D journal file!");
                        Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, "Can't find E:D journal file!");                        
                        missingMessagePossible = false;
                    }

                    // new files needed or notified ?
                    if(String.IsNullOrWhiteSpace(m_LastScan_JournalFile) && (newFiles.Count == 0))
                    {
                        if(m_extLogging) logger.Log("new files");
                        // get jounal for the first time, get only the latest
                        IOrderedEnumerable<string> journals = Directory.EnumerateFiles(m_SavedgamesPath, "Journal.*.log", SearchOption.TopDirectoryOnly).OrderByDescending(x => x);

                        if ((journals.Count() > 0) && (GetTimeValueFromFilename(journals.ElementAt<String>(0)) > 0))
                            m_LastScan_JournalFile  = journals.ElementAt<String>(0);

                        if(isFirstRun)
                            isZeroRun = true;
                    }
                    else if(m_NewFileDetected || isFirstRun)
                    {
                        if(m_extLogging) logger.Log("first run");

                        // check for new files
                        m_NewFileDetected = false;

                        IOrderedEnumerable<string> journals =Directory.EnumerateFiles(m_SavedgamesPath, "Journal.*.log", SearchOption.TopDirectoryOnly).OrderByDescending(File.GetLastWriteTime);

                        foreach (String newFile in journals)
                        {
                            Debug.Print(newFile);

                            // add every new file, but only if 
                            //  - it's "timevalue" is newer than the "timevalue" of the current file
                            //  - it's last write time is not longer than 24 hours ago
                            if((GetTimeValueFromFilename(newFile) > GetTimeValueFromFilename(m_LastScan_JournalFile)) && ((DateTime.Now - File.GetLastWriteTime(newFile)).TotalHours < 24))
                            {
                                if(!newFiles.Contains(newFile))
                                {
                                    var pos = newFiles.FindIndex(x => (GetTimeValueFromFilename(x) > GetTimeValueFromFilename(newFile))) + 1;
                                    newFiles.Insert(pos, newFile); 
                                }
                            }
                            else
                            {
                                // now comes the older files
                                break;
                            }
                        }
                    }

                    isFirstRun = false;

                    // check current file for existence, get another if necessary and existing, filter out "dead bodies"
                    if (!String.IsNullOrWhiteSpace(m_LastScan_JournalFile))
                    {
                        if(!File.Exists(m_LastScan_JournalFile))
                        {
                            if(m_extLogging) logger.Log("file not existing : " + m_LastScan_JournalFile);
                            m_LastScan_JournalFile = "";
                        }
                    }

                    if(String.IsNullOrWhiteSpace(m_LastScan_JournalFile) && (newFiles.Count > 0))
                    {
                        for (int i = (newFiles.Count-1); i >= 0; i--)
                        {
                            if(File.Exists(newFiles[i]))
                            {
                                // new "current" file
                                m_LastScan_JournalFile = newFiles[i];
                                newFiles.RemoveAt(i);
                                break;
                            }
                            else
                            {
                                // dead body
                                newFiles.RemoveAt(i);
                            }
                        }
                    }
                    

                    if (!String.IsNullOrWhiteSpace(m_LastScan_JournalFile))
                    {
                        if(m_extLogging) logger.Log("check file for new events : " +  Path.GetFileName(m_LastScan_JournalFile) + " (" + gotLatestEvent +")");

                        missingMessagePossible = false;

                        // we still have a current file
                        if(journalFileStream == null)
                        {
                            Program.DBCon.setIniValue(DB_GROUPNAME,   "LastScan_JournalFile",  m_LastScan_JournalFile);

                            journalFileStream     = File.Open(m_LastScan_JournalFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            journalStreamReader   = new StreamReader(journalFileStream);
                        }

                        while (!journalStreamReader.EndOfStream)
                        {
                            // get json object
                            dataLine     = journalStreamReader.ReadLine();

                            if(m_extLogging) logger.Log("new line from : " + Path.GetFileName(m_LastScan_JournalFile) + " : " + dataLine);

                            parsingError = false;

                            try
                            {
                                journalEntry = JsonConvert.DeserializeObject<JToken>(dataLine);
                            }
                            catch (Exception ex)
                            {
                                parsingError = true;
                                String msg = "Error while parsing json string from file '" + Path.GetFileName(m_LastScan_JournalFile) + "' : \n<" + dataLine + ">";
                                logger.Log(ErrorViewer.GetErrorMessage(ref msg, ex));
                            }

                            if(!parsingError)
                            {
                                // identify the event
                                rawEventName = journalEntry.Value<String>("event");
                                rawTimeStamp = journalEntry.Value<DateTime>("timestamp");

                                if(rawEventName == JournalEvent.Died.ToString())
                                    Debug.Print("here");

                                if ((rawEventName != null) && (rawTimeStamp != null))
                                {
                                    if(!Enum.TryParse<JournalEvent>(rawEventName, out eventName))
                                    {
                                        eventName = JournalEvent.Not_Supported;
                                    }

                                    if(gotLatestEvent)
                                    {
                                        // every recognized event is accepted as new
                                        lastEvent = rawEventName;
                                        lastEventTime = rawTimeStamp;

                                        SubmitReferenceEvents(ref latestLocationEvent, ref latestFileHeader, ref logger);

                                        // pre-check for base data which is currently not in the database.
                                        switch (eventName)
                                        {
                                            case JournalEvent.Location:
                                            case JournalEvent.Docked:
                                            case JournalEvent.FSDJump:
                                            case JournalEvent.Resurrect:

                                                if(m_extLogging) logger.Log("accepted (pre) : " + eventName.ToString());
                                                Debug.Print("accepted (pre) : " + eventName.ToString());

                                                BasedataEventArgs newBasedataArgItem = new BasedataEventArgs() {
                                                                                                  EventType = JournalEvent.Basedata,
                                                                                                  System    = journalEntry.Value<String>("StarSystem").NToString(""),
                                                                                                  Station   = journalEntry.Value<String>("StationName").NToString("")
                                                                                                };

                                                if(journalEntry.Value<Object>("StarPos") != null)
                                                {
                                                    newBasedataArgItem.Coordinates   = new Point3Dbl((Double)journalEntry["StarPos"][0], 
                                                                                                     (Double)journalEntry["StarPos"][1], 
                                                                                                     (Double)journalEntry["StarPos"][2]);
                                                }

                                                BasedataEventRecieved.Raise(this, newBasedataArgItem);

                                                break;
                                        }

                                        // switch what to do
                                        switch (eventName)
                                        {
                                            case JournalEvent.Fileheader:
                                            case JournalEvent.Location:

                                            case JournalEvent.Docked:
                                            case JournalEvent.Undocked:

                                            case JournalEvent.SupercruiseEntry:
                                            case JournalEvent.SupercruiseExit:

                                            case JournalEvent.Liftoff:
                                            case JournalEvent.Touchdown:

                                            case JournalEvent.FSDJump:

                                            case JournalEvent.Died:
                                            case JournalEvent.Resurrect:

                                            case JournalEvent.Scan:

                                            case JournalEvent.MissionAccepted:
                                            case JournalEvent.MissionCompleted:
                                            case JournalEvent.MissionAbandoned:
                                            case JournalEvent.MissionFailed:

                                            case JournalEvent.LoadGame:


                                                /*******************************************************/
                                                /***************       send events    ******************/
                                                /*******************************************************/
                                                if(eventName == JournalEvent.Docked)
                                                    Debug.Print("stop");

                                                if(m_extLogging) logger.Log("accepted : " + eventName.ToString());
                                                Debug.Print("accepted : " + eventName.ToString());
                                                JournalEventArgs newJournalArgItem = new JournalEventArgs() { EventType = eventName, Data = journalEntry, History = history };

                                                JournalEventRecieved.Raise(this, newJournalArgItem);

                                                newJournalArgItem.History = null;
                                                history.Insert(0, newJournalArgItem);
                                                if(history.Count > 5)
                                                    history.RemoveAt(5);

                                                break;

                                            default:
                                                Debug.Print("ignored (contact): <" + rawEventName + ">");

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if(isZeroRun)
                                        { 
                                            // every recognized event is accepted as new
                                            lastEvent       = rawEventName;
                                            lastEventTime   = rawTimeStamp;
                                        }

                                        // switch what to do
                                        switch (eventName)
                                        {
                                            case JournalEvent.Fileheader:
                                                latestFileHeader = journalEntry;
                                                Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, "Initial fileheader found");                        
                                                break;

                                            case JournalEvent.Location:
                                                latestLocationEvent = journalEntry;
                                                break;

                                            case JournalEvent.SupercruiseExit:
                                                if(latestLocationEvent != null)
                                                {
                                                    latestLocationEvent["StarSystem"]   = journalEntry["StarSystem"];
                                                    latestLocationEvent["Body"]         = journalEntry["Body"];
                                                    latestLocationEvent["BodyType"]     = journalEntry["BodyType"];
                                                }
                                                break;

                                            case JournalEvent.SupercruiseEntry:
                                                if(latestLocationEvent != null)
                                                {
                                                    latestLocationEvent["StarSystem"]   = journalEntry["StarSystem"];
                                                    latestLocationEvent["StationName"]  = "";
                                                    latestLocationEvent["Docked"]       = "false";
                                                    latestLocationEvent["Body"]         = "";
                                                    latestLocationEvent["BodyType"]     = "";
                                                    latestLocationEvent["StationType"]  = "";
                                                }

                                                break;

                                            case JournalEvent.FSDJump:
                                                if(latestLocationEvent != null)
                                                {
                                                    latestLocationEvent["StarSystem"]   = journalEntry["StarSystem"];
                                                    latestLocationEvent["StationName"]  = "";
                                                    latestLocationEvent["Docked"]       = "false";
                                                    latestLocationEvent["StarPos"]      = journalEntry["StarPos"];
                                                    latestLocationEvent["Body"]         = journalEntry["Body"];
                                                    latestLocationEvent["BodyType"]     = journalEntry["BodyType"];
                                                    latestLocationEvent["Faction"]      = journalEntry["Faction"];
                                                    latestLocationEvent["Allegiance"]   = journalEntry["Allegiance"];
                                                    latestLocationEvent["Economy"]      = journalEntry["Economy"];
                                                    latestLocationEvent["Government"]   = journalEntry["Government"];
                                                    latestLocationEvent["Security"]     = journalEntry["Security"];

                                                
                                                    latestLocationEvent["StationType"]  = "";
                                                }
                                            
                                                break;

                                            case JournalEvent.Docked:
                                                if(latestLocationEvent != null)
                                                {
                                                    latestLocationEvent["StarSystem"]   = journalEntry["StarSystem"];
                                                    latestLocationEvent["StationName"]  = journalEntry["StationName"];
                                                    latestLocationEvent["Docked"]       = "true";
                                                    latestLocationEvent["StationType"]  = journalEntry["StationType"];;
                                                }

                                                break;

                                            default:
                                                //Debug.Print("ignored (seeking) : <" + rawEventName + ">");
                                                break;
                                        }
                                    }
                                }
                            }
                        }

                        if(lastEventTime > DateTime.MinValue)
                        {
                            if(m_extLogging) logger.Log("write new time");
                            // only rewrite if we've got a new event
                            Program.DBCon.setIniValue(DB_GROUPNAME, "LastScan_Event",     lastEvent);
                            Program.DBCon.setIniValue(DB_GROUPNAME, "LastScan_TimeStamp", lastEventTime.ToString());

                            lastEventTime = DateTime.MinValue;
                        }

                        if(isZeroRun)
                            gotLatestEvent = true;

                        if(newFiles.Count > 0)
                        {
                            if(m_extLogging) logger.Log("still have new files");

                            // prepare switching to next file
                            if(journalFileStream != null)
                                journalFileStream.Dispose();

                            if(journalStreamReader != null)
                                journalStreamReader.Dispose();

                            journalFileStream = null;
                            journalStreamReader = null;

                            m_LastScan_JournalFile = "";
                        }
                        else if(!gotLatestEvent)
                        {
                            // it's the end of the actual file -> so we found the latest item
                            gotLatestEvent = true;
                            if(m_extLogging) logger.Log("force latest event");
                        }

                        if(gotLatestEvent)
                            SubmitReferenceEvents(ref latestLocationEvent, ref latestFileHeader, ref logger);

                    }

                    isZeroRun = false;
                    errorCount = 0;

                }
                catch (Exception ex)
                {
                    errorCount++;

                    Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, "Error while parsing E:D journal !");  
                                          
                    Debug.Print("AnalyseError");
                    
                    String msg = "Error in the journal scanner main routine";

                    logger.Log(ErrorViewer.GetErrorMessage(ref msg, ex));

                    if (lastEventTime > DateTime.MinValue)
                    {
                        // only rewrite if we've got a new event
                        Program.DBCon.setIniValue(DB_GROUPNAME, "LastScan_Event",     lastEvent);
                        Program.DBCon.setIniValue(DB_GROUPNAME, "LastScan_TimeStamp", lastEventTime.ToString());

                        lastEventTime = DateTime.MinValue;
                    }

                    if(errorCount > 1)
                    {
                        // prepare switching to next file
                        if(journalFileStream != null)
                            journalFileStream.Dispose();

                        if(journalStreamReader != null)
                            journalStreamReader.Dispose();

                        journalFileStream   = null;
                        journalStreamReader = null;
                        gotLatestEvent      = false;
                    }
                }

                if (newFiles.Count == 0)
                {
                    Thread.Sleep(1000);  
                    //  Because the current file is opened by ED with a permanent write stream no changed event 
                    //  raises reliably in the SystemFileWatcher.  With Sleep(1000) we get every second the chance to 
                    //  to detect new data with the line 
                    //     while (!journalStreamReader.EndOfStream)
                }
                else
                    Debug.Print("because new files : " + DateTime.Now);

            }while (!m_Stop);

            // clean up
            if(journalFileStream != null)
                journalFileStream.Dispose();

            if(journalStreamReader != null)
                journalStreamReader.Dispose();

            if(m_extLogging) logger.Log("stopped !");
        }

        private void SubmitReferenceEvents(ref JToken latestLocationEvent, ref JToken latestFileHeader, ref SingleThreadLogger logger)
        {
            try
            {
                if (latestFileHeader != null)
                {
                    logger.Log("submit ref event : latestFileHeader");
                    // memorize the latest header
                    JournalEventRecieved.Raise(this, new JournalEventArgs() { EventType = JournalEvent.Fileheader, Data = latestFileHeader });
                    latestFileHeader = null;
                }

                if (latestLocationEvent != null)
                {
                    logger.Log("submit ref event : latestLocationEvent");

                    // pre-check for base data which is currently not in the database.
                    BasedataEventArgs newBasedataArgItem = new BasedataEventArgs() {
                                                                        EventType = JournalEvent.Basedata,
                                                                        System    = latestLocationEvent.Value<String>("StarSystem").NToString(""),
                                                                        Station   = latestLocationEvent.Value<String>("StationName").NToString("")
                                                                    };

                    if(latestLocationEvent.Value<Object>("StarPos") != null)
                    {
                        newBasedataArgItem.Coordinates   = new Point3Dbl((Double)latestLocationEvent["StarPos"][0], 
                                                                         (Double)latestLocationEvent["StarPos"][1], 
                                                                         (Double)latestLocationEvent["StarPos"][2]);
                    }

                    BasedataEventRecieved.Raise(this, newBasedataArgItem);


                    // always inform about the latest location information
                    JournalEventRecieved.Raise(this, new JournalEventArgs() { EventType = JournalEvent.Location, Data = latestLocationEvent });
                    latestLocationEvent = null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing reference events", ex);
            }
        }

        /// <summary>
        /// returns a unique value from the filename
        /// </summary>
        /// <param name="journalFilename"></param>
        /// <returns></returns>
        private Int64 GetTimeValueFromFilename(String journalFilename)
        {
            Int64 retvalue = -1;

            try
            {
                journalFilename = Path.GetFileNameWithoutExtension(journalFilename.Substring(journalFilename.IndexOf(".")+1));
                journalFilename = journalFilename.Replace(".", "");

                if(journalFilename.Length == 14) 
                    Int64.TryParse(journalFilename, out retvalue);

                return retvalue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting timevalue from filename", ex);
            }
        }

        /// <summary>
        /// injects a event from outside into the journal event handling
        /// </summary>
        /// <param name="newJournalArgItem"></param>
        internal void InjectJournalEvent(JournalEventArgs newJournalArgItem)
        {
            try
            {
                JournalEventRecieved.Raise(this, newJournalArgItem);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while injecting a event", ex);
            }
        }
    }
}
