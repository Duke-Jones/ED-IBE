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
            Docked,
            Undocked,
            Liftoff,
            Touchdown,
            FSDJump,
            SupercruiseEntry,
            SupercruiseExit
        }

        #endregion

        #region event handler

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

            public JournalEvent EventType    { get; set; }
            public JToken       Data         { get; set; }
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

                do
                {
                    // wait until thread is not running anymore
                    Thread.Sleep(25);
                } while ((m_JournalScanner_Thread.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0);

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

        private Thread                  m_JournalScanner_Thread;

        private String                  m_LastScan_JournalFile;
        private DateTime                m_LastScan_Timestamp;
        private String                  m_LastScan_Event;

        private String                  m_SavedgamesPath;
        private Boolean                 m_Stop;
        private Boolean                 m_NewFileDetected;

        /// <summary>
        /// create a new LogFileScanner-object
        /// </summary>
        public EDJournalScanner()
        {
            try
            {
                m_JournalScanner_Thread                 = new Thread(() => this.UpdateSystemNameFromLogFile_worker());
                m_JournalScanner_Thread.Name            = "JournalScanner_Thread";
                m_JournalScanner_Thread.IsBackground    = true;

                m_LastScan_JournalFile                  = Program.DBCon.getIniValue<String>(DB_GROUPNAME,   "LastScan_JournalFile",  "", true);
                m_LastScan_Event                        = Program.DBCon.getIniValue<String>(DB_GROUPNAME,   "LastScan_Event",        "", true);
                m_LastScan_Timestamp                    = Program.DBCon.getIniValue<DateTime>(DB_GROUPNAME, "LastScan_TimeStamp",    new DateTime(2000, 1, 1).ToString(), false);
                
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
                if (! ((m_JournalScanner_Thread.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0))
                {
                    if (SHGetKnownFolderPath(SAVED_GAMES, 0, IntPtr.Zero, out m_SavedgamesPath) != 0)
                    {
                        m_SavedgamesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games");
                    }

                    if(Directory.Exists(m_SavedgamesPath))
                        m_SavedgamesPath = Path.Combine(m_SavedgamesPath, @"Frontier Developments\Elite Dangerous");

                    if(!Directory.Exists(m_SavedgamesPath))
                    {
                        m_SavedgamesPath = null;
                        throw new Exception("ED-IBE can't find the \"Saved Games\" path to access the E:D journal file");
                    }

                    m_Stop = false;

                    // thread is not running
                    m_JournalScanner_Thread.Start();
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error while stopping the journal scanner", ex);
            }
        }


        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Debug.Print("get Changed Event : " + DateTime.Now);
        }


        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Debug.Print("get Created Event : " + DateTime.Now);
        }


        private void UpdateSystemNameFromLogFile_worker()
        {
            SingleThreadLogger logger           = new SingleThreadLogger(ThreadLoggerType.FileScanner);
            String latestFile                   = "";
            FileStream journalFileStream        = null;
            StreamReader journalStreamReader    = null;
            var filewatchResetEvent             = new ManualResetEvent(false);
            var fileWatcher                     = new FileSystemWatcher(m_SavedgamesPath, "*.log");
            JournalEvent eventName;
            String rawEventName;
            DateTime rawTimeStamp;
            string dataLine;
            JToken journalEntry;
            IOrderedEnumerable<string> journals = null;
            List<String> newFiles = new List<string>();
            Boolean isFirstRun = true;
            Boolean gotConnexion = false;

            // let the filesystem watcher raise the "set" event of the AutoResetEvent
            fileWatcher.EnableRaisingEvents = true;
            fileWatcher.Changed += (s,e) => filewatchResetEvent.Set();

            fileWatcher.Changed += FileWatcher_Changed;

            fileWatcher.Created += FileWatcher_Created;

            m_NewFileDetected = false;

            do
            {
                try
                {

                    // new files needed or notified ?
                    if(String.IsNullOrWhiteSpace(m_LastScan_JournalFile))
                    {
                        // get jounal for the first time, get only the latest
                        journals = Directory.EnumerateFiles(m_SavedgamesPath, "Journal.*.log", SearchOption.TopDirectoryOnly).OrderByDescending(x => x);

                        if ((journals.Count() > 0) && (GetTimeValueFromFilename(journals.ElementAt<String>(0)) > 0))
                            m_LastScan_JournalFile  = journals.ElementAt<String>(0);

                        gotConnexion = true;
                    }
                    else if(m_NewFileDetected || isFirstRun)
                    {
                        // check for new files
                        journals = Directory.EnumerateFiles(m_SavedgamesPath, "Journal.*.log", SearchOption.TopDirectoryOnly).OrderByDescending(File.GetLastWriteTime);

                        foreach (String newFile in journals)
                        {
                            // add every new file, but only if it's "timevalue" is newer than the "timevalue" of the current file
                            if(GetTimeValueFromFilename(newFile) > GetTimeValueFromFilename(m_LastScan_JournalFile))
                            {
                                if(!newFiles.Contains(newFile))
                                    newFiles.Insert(newFiles.FindIndex(x => (GetTimeValueFromFilename(x) < GetTimeValueFromFilename(newFile))), newFile); 
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
                            m_LastScan_JournalFile = "";
                            gotConnexion = true;
                        }

                        if(String.IsNullOrWhiteSpace(m_LastScan_JournalFile) && (journals != null))
                        {
                            for (int i = (newFiles.Count-1); i > 0; i--)
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
                    }

                    if (!String.IsNullOrWhiteSpace(m_LastScan_JournalFile))
                    {
                        // we still have a current file

                        if(journalFileStream == null)
                        {
                            Program.DBCon.setIniValue(DB_GROUPNAME,   "LastScan_JournalFile",  m_LastScan_JournalFile);

                            journalFileStream       = File.Open(m_LastScan_JournalFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            journalStreamReader     = new StreamReader(journalFileStream);
                        }

                        while (!journalStreamReader.EndOfStream)
                        {
                            // get json object
                            dataLine     = journalStreamReader.ReadLine();
                            journalEntry = JsonConvert.DeserializeObject<JToken>(dataLine);

                            // identify the event
                            rawEventName = journalEntry.Value<String>("event");
                            rawTimeStamp = journalEntry.Value<DateTime>("timestamp");

                            if ((rawEventName != null) && (rawTimeStamp != null))
                            {
                                if(!Enum.TryParse<JournalEvent>(rawEventName, out eventName))
                                {
                                    eventName = JournalEvent.Not_Supported;
                                }

                                if((!gotConnexion) && (rawTimeStamp > m_LastScan_Timestamp))
                                {
                                    // jumped over the searched, process this one and all following will be processed
                                    gotConnexion = true;
                                }

                                if(gotConnexion)
                                {
                                    // every recognized event is accepted as new
                                    Program.DBCon.setIniValue(DB_GROUPNAME, "LastScan_Event",     rawEventName);
                                    Program.DBCon.setIniValue(DB_GROUPNAME, "LastScan_TimeStamp", rawTimeStamp.ToString());

                                    // switch what to do
                                    switch (eventName)
                                    {
                                        case JournalEvent.Fileheader:
                                        case JournalEvent.Docked:
                                            Debug.Print("accepted : " + eventName.ToString());
                                            JournalEventRecieved.Raise(this, new JournalEventArgs() { EventType = eventName, Data = journalEntry });
                                            break;

                                        default:
                                            Debug.Print("ignored : <" + rawEventName + ">");

                                            break;
                                    }
                                }
                                else
                                {
                                    // do we get connexion now ?
                                    if((rawTimeStamp == m_LastScan_Timestamp) && (rawEventName == m_LastScan_Event))
                                    {
                                        // got it exactly, next one and all following will be processed
                                        gotConnexion = true;
                                    }
                                }
                            }
                        }

                        if((journalStreamReader.EndOfStream) && (newFiles.Count > 0))
                        {
                            // prepare switching to next file
                            if(journalFileStream != null)
                                journalFileStream.Dispose();

                            if(journalStreamReader != null)
                                journalStreamReader.Dispose();

                            journalFileStream = null;
                            journalStreamReader = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("AnalyseError");
                    logger.Log(ex.Message + "\n" + ex.StackTrace + "\n\n");

                    // prepare switching to next file
                    if(journalFileStream != null)
                        journalFileStream.Dispose();

                    if(journalStreamReader != null)
                        journalStreamReader.Dispose();

                    journalFileStream = null;
                    journalStreamReader = null;
                }

                filewatchResetEvent.Reset();
                if (newFiles.Count == 0)
                {
                    if(filewatchResetEvent.WaitOne(10000))
                    {
                        Debug.Print("because fsw : " + DateTime.Now);
                    }
                    else
                    {
                        Debug.Print("because time : " + DateTime.Now);
                    }
                }
                else
                    Debug.Print("because new files : " + DateTime.Now);

            }while (!m_Stop);

            // clean up
            fileWatcher.Dispose();
            filewatchResetEvent.Dispose();

            if(journalFileStream != null)
                journalFileStream.Dispose();

            if(journalStreamReader != null)
                journalStreamReader.Dispose();
            
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
    }
}
