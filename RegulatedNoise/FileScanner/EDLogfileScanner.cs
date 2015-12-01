using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Diagnostics;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.FileScanner
{
    public class EDLogfileScanner: IDisposable
    {
        #region LogEvents

        [Flags] public enum enLogEvents
        {
            None      = 0,
            System    = 1,
            Location  = 2,
            Jump      = 4,
        }

        public class LogEvent
        {
            public enLogEvents EventType    { get; set; }
            public String      Value        { get; set; }
            public DateTime    Time         { get; set; }
        }

        #endregion

        #region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<LocationChangedEventArgs> LocationChanged;

        protected virtual void OnLocationChanged(LocationChangedEventArgs e)
        {
            EventHandler<LocationChangedEventArgs> myEvent = LocationChanged;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class LocationChangedEventArgs : EventArgs
        {
            public LocationChangedEventArgs()
            {
                Changed     = enLogEvents.Location;
                System      = "";
                Location     = "";
            }

            public String System            { get; set; }
            public String Location          { get; set; }
            public String OldSystem         { get; set; }
            public String OldLocation       { get; set; }
            public enLogEvents Changed      { get; set; }
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
                m_Closing   = true;
                m_LogfileScanner_ARE.Set();

                do
                {
                    // wait until thread is not running anymore
                    Thread.Sleep(25);
                } while ((m_LogfileScanner_Thread.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0);

                if (m_stateTimer != null)
                { 
                    m_stateTimer.Dispose();
                    m_stateTimer = null;
                }

            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
            disposed = true;
        }
    }

    // Use C# destructor syntax for finalization code.
    ~EDLogfileScanner()
    {
        // Simply call Dispose(false).
        Dispose (false);
    }

    #endregion

        private const String        DB_GROUPNAME                    = "LogfileScanner";

        const long SEARCH_MAXLENGTH     = 160;
        const long SEARCH_MINLENGTH     = 5;

        private System.Threading.Timer  m_stateTimer;
        private Thread                  m_LogfileScanner_Thread;
        private DateTime                m_TimestampLastScan;
        private AutoResetEvent          m_LogfileScanner_ARE;
        private bool                    m_Closing;
        private String                  m_CommandersName;

        /// <summary>
        /// create a new LogFileScanner-object
        /// </summary>
        public EDLogfileScanner()
        {
            try
            {
                m_LogfileScanner_Thread                 = new Thread(() => this.UpdateSystemNameFromLogFile_worker());
                m_LogfileScanner_Thread.Name            = "LogfileScanner_Thread";
                m_LogfileScanner_Thread.IsBackground    = true;

                m_LogfileScanner_ARE                    = new AutoResetEvent(false);
                m_TimestampLastScan                     = Program.DBCon.getIniValue<DateTime>(DB_GROUPNAME, "TimestampLastScan", new DateTime(2000, 1, 1).ToString(), false);
                m_Closing                               = false;

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
                if (! ((m_LogfileScanner_Thread.ThreadState & (System.Threading.ThreadState.Stopped | System.Threading.ThreadState.Unstarted)) == 0))
                {
                    // thread is not running
                    m_LogfileScanner_Thread.Start();
                }

                // initialize and start the timer object
                if (m_stateTimer != null)
                { 
                    m_stateTimer.Dispose();
                    m_stateTimer = null;
                }

                var autoEvent = new AutoResetEvent(false);
                TimerCallback _timerCallback = TimerCallbackFunction;
                m_stateTimer = new System.Threading.Timer(_timerCallback, autoEvent, 10000, 10000);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while starting the logfile scanner", ex);
            }
            
        }

        /// <summary>
        /// stops scanning of the logfile
        /// </summary>
        public void Stop()
        {
            try
            {
                m_stateTimer.Dispose();
                m_stateTimer = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while stopping the logfile scanner", ex);
            }
        }


        private void TimerCallbackFunction(Object state)
        {
            try
            {
                m_LogfileScanner_ARE.Set();
            }
            catch (Exception ex)
            {
                m_stateTimer.Dispose();
                m_stateTimer = null;
                cErr.showError(ex, "Error while pushing logfile scan");
            }
            
        }



        private void UpdateSystemNameFromLogFile_worker()
        {
            SingleThreadLogger logger           = new SingleThreadLogger(ThreadLoggerType.FileScanner);
            Regex RegExTest_FindBestIsland      = new Regex(String.Format("FindBestIsland:.+:.+:.+:.+", Regex.Escape(Program.Settings_old.PilotsName)), RegexOptions.IgnoreCase);
            Regex RegExTest_Island_Claimed      = new Regex(String.Format("vvv------------ ISLAND .+ CLAIMED ------------vvv"), RegexOptions.IgnoreCase);

            do
            {
                try
                {
                    Boolean EndNow = false;
                    string Systemname = "";
                    string Locationname = "";
                    string currentLogString;
                    Match m = null;
                    Boolean Got_Jump = false;
                    List<String> PossibleLocations = new List<string>();
                    List<LogEvent> LoggedEvents = new List<LogEvent>();  
                    DateTime TimestampCurrentLine       = DateTime.MinValue;
                    DateTime TimestampLastRecognized    = DateTime.MinValue;

                    #if extScanLog
                        logger.Log("start, RegEx = <" + String.Format("FindBestIsland:.+:.+:.+:.+", Regex.Escape(Program.RegulatedNoiseSettings.PilotsName)) + ">");
                    #endif

                    var appConfigPath = Program.Settings_old.ProductsPath;

                    if (Directory.Exists(appConfigPath))
                    {
                        var versions = Directory.GetDirectories(appConfigPath).Where(x => x.Contains("FORC-FDEV")).ToList().OrderByDescending(x => x).ToList();

                        if (versions.Count() == 0)
                        {
                            #if extScanLog
                                logger.Log("no dirs with <FORC-FDEV> found");
                                var versions2 = Directory.GetDirectories(appConfigPath).ToList().OrderByDescending(x => x).ToList();
                                foreach (string SubPath in versions2)
                                {
                                    logger.Log("but found <" +  SubPath + ">");   
                                }
                            #endif
                        }
                        else
                        {
                            #if extScanLog
                                logger.Log("lookin' for files in <" + versions[0] + ">");
                            #endif

                            // We'll just go right ahead and use the latest log...
                            var netLogs =
                                Directory.GetFiles(versions[0] + "\\Logs", "netLog*.log")
                                    .OrderByDescending(File.GetLastWriteTime)
                                    .ToArray();

                            if (netLogs.Length != 0)
                            {
                                Systemname          = "";
                                Locationname         = "";
                                LoggedEvents.Clear();
                                var newestNetLog    = netLogs[0];

                                #if extScanLog
                                    Debug.Print("File opened : <" + newestNetLog + ">");
                                    logger.Log("File opened : <" + newestNetLog + ">");
                                #endif

                                FileStream Datei = new FileStream(newestNetLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                Byte[] ByteBuffer = new Byte[1];
                                Byte[] LineBuffer = new Byte[SEARCH_MAXLENGTH];

                                Datei.Seek(0, SeekOrigin.End);

                                while (!EndNow && (Datei.Position >= 2))
                                {
                                    long StartPos   = -1;
                                    long EndPos     = -1;

                                    do
                                    {
                                        Datei.Read(ByteBuffer, 0, ByteBuffer.Length);
                                        //Debug.Print(ByteBuffer[0].ToString("x") + " ");
                                        if ((ByteBuffer[0] == 0x0A) || (ByteBuffer[0] == 0x0D))
                                            if (EndPos == -1)
                                            {
                                                if (ByteBuffer[0] == 0x0D)
                                                    EndPos = Datei.Position + 1;
                                                else
                                                    EndPos = Datei.Position;

                                                Datei.Seek(-3, SeekOrigin.Current);
                                            }
                                            else
                                            {
                                                if (ByteBuffer[0] == 0x0D)
                                                    StartPos = Datei.Position + 1;
                                                else
                                                    StartPos = Datei.Position;
                                            }
                                        else
                                        {
                                            if(TimestampLastRecognized.Equals(DateTime.MinValue) && (EndPos == -1))
                                            { 
                                                EndPos = Datei.Position;
                                            }

                                            Datei.Seek(-3, SeekOrigin.Current);
                                        }
                                            

                                    } while (StartPos == -1 && Datei.Position >= 3);

                                    if((StartPos == -1) && ((EndPos - StartPos) > SEARCH_MINLENGTH))
                                        StartPos = 0;

                                    if ((StartPos >= 0) && ((EndPos - StartPos) <= SEARCH_MAXLENGTH))
                                    {
                                        // found a line and it's not too long
                                        // read
                                        Datei.Read(LineBuffer, 0, (int)(EndPos - StartPos));
                                        // and convert to string
                                        currentLogString = Encoding.ASCII.GetString(LineBuffer, 0, (int)(EndPos - StartPos) );

                                        //' Debug.Print("log - scanning :" + currentLogString);

                                        if (currentLogString != null)
                                        {

                                            // *********************************************
                                            // check the timestamp of the current line to avoid to re-analyse older data
                                            if(TryGetTimeFromLine(currentLogString, ref TimestampCurrentLine))
                                            { 
                                                if(TimestampLastRecognized.Equals(DateTime.MinValue))
                                                    TimestampLastRecognized = TimestampCurrentLine;

                                                if(TimestampCurrentLine <= m_TimestampLastScan)
                                                { 
                                                    // everything is coming now is older
                                                    EndNow = true;
                                                }

                                                if(!EndNow)
                                                {
                                                    // first: check if we've jumped
                                                    m = RegExTest_Island_Claimed.Match(currentLogString);
                                                    if (m.Success)
                                                    {
                                                        if(!Got_Jump)
                                                        { 
                                                            LoggedEvents.Add(new LogEvent() { EventType = enLogEvents.Jump, Value = "", Time = TimestampCurrentLine});
                                                            Got_Jump = true;
                                                        }

                                                        #if extScanLog
                                                            Debug.Print("Jump Recognized");
                                                            logger.Log("Jump Recognized : " + currentLogString.Replace("\n", "").Replace("\r", ""));
                                                        #endif
                                                    }

                                                    // *********************************************
                                                    // second: looking for the systemname
                                                    if(String.IsNullOrEmpty(Systemname))
                                                    {
                                                        if (currentLogString.Contains("System:"))
                                                        {
                                                            #if extScanLog
                                                                Debug.Print("Systemstring:" + currentLogString);
                                                                logger.Log("Systemstring:" + currentLogString.Replace("\n", "").Replace("\r", ""));
                                                            #endif

                                                            Systemname = currentLogString.Substring(currentLogString.IndexOf("(", StringComparison.Ordinal) + 1);
                                                            Systemname = Systemname.Substring(0, Systemname.IndexOf(")", StringComparison.Ordinal));

                                                            LoggedEvents.Add(new LogEvent() { EventType = enLogEvents.System, Value = Systemname, Time = TimestampCurrentLine});

                                                            #if extScanLog
                                                                Debug.Print("System: " + systemName);
                                                                logger.Log("System: " + systemName);
                                                            #endif

                                                            // preparing search for location info
                                                            RegExTest_FindBestIsland = new Regex(String.Format("FindBestIsland:.+:.+:.+:{0}", Regex.Escape(Systemname)), RegexOptions.IgnoreCase);

                                                            #if extScanLog
                                                                logger.Log("new Regex : <" + String.Format("FindBestIsland:.+:.+:.+:{0}", Regex.Escape(systemName)) + ">");
                                                            #endif

                                                            // we may have candidates, check them and if nothing found search from the current position
                                                            foreach (string candidate in PossibleLocations)
                                                            {
                                                                #if extScanLog
                                                                    Debug.Print("check candidate : " + candidate);
                                                                    logger.Log("check candidate : " + candidate.Replace("\n", "").Replace("\r", ""));
                                                                #endif

                                                                m = RegExTest_FindBestIsland.Match(candidate);
                                                                //Debug.Print(currentLogString);
                                                                //if (currentLogString.Contains("Duke Jones"))
                                                                //    Debug.Print("Stop");
                                                                if (m.Success)
                                                                {
                                                                    #if extScanLog
                                                                        Debug.Print("locationstring from candidate : " + candidate);
                                                                        logger.Log("locationstring from candidate : " + candidate.Replace("\n", "").Replace("\r", ""));
                                                                    #endif

                                                                    getLocation(ref Locationname, m);

                                                                    DateTime CurrentTimestamp = new DateTime();
                                                                    TryGetTimeFromLine(currentLogString, ref CurrentTimestamp);
                                                                    LoggedEvents.Add(new LogEvent() { EventType = enLogEvents.Location, Value = Locationname, Time = CurrentTimestamp});

                                                                    EndNow = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        else 
                                                        {
                                                            m = RegExTest_FindBestIsland.Match(currentLogString);
                                                            if (m.Success)
                                                            {
                                                                #if extScanLog
                                                                    Debug.Print("Candidate : " + currentLogString);
                                                                    logger.Log("Candidate added : " + currentLogString.Replace("\n", "").Replace("\r", ""));
                                                                #endif

                                                                PossibleLocations.Add(currentLogString);
                                                            }
                                                        }
                                                    }
                                                }

                                                if(!EndNow)
                                                {
                                                    // if we have the systemname we're looking for the locationname
                                                    if (!string.IsNullOrEmpty(Systemname) && string.IsNullOrEmpty(Locationname))
                                                    {
                                                        m = RegExTest_FindBestIsland.Match(currentLogString);
                                                        //Debug.Print(currentLogString);
                                                        //if (currentLogString.Contains("Duke Jones"))
                                                        //    Debug.Print("Stop");
                                                        if (m.Success)
                                                        {
                                                            #if extScanLog
                                                                Debug.Print("locationstring (direct) : " + currentLogString);
                                                                logger.Log("locationstring (direct) : " + currentLogString.Replace("\n", "").Replace("\r", ""));
                                                            #endif

                                                            getLocation(ref Locationname, m);
                                                            LoggedEvents.Add(new LogEvent() { EventType = enLogEvents.Location, Value = Locationname, Time = TimestampCurrentLine});

                                                            EndNow = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if(!EndNow)
                                    { 
                                        if (StartPos >= 3)
                                        {
                                            Datei.Seek(StartPos-1, SeekOrigin.Begin);
                                        }
                                        else
                                            Datei.Seek(0, SeekOrigin.Begin);
                                    }
                                }

                                if(m_TimestampLastScan < TimestampLastRecognized)
                                { 
                                    m_TimestampLastScan = TimestampLastRecognized;
                                    Program.DBCon.setIniValue(DB_GROUPNAME, "TimestampLastScan", m_TimestampLastScan.ToString());
                                }

                                Datei.Close();
                                Datei.Dispose();

                                #if extScanLog
                                    Debug.Print("Datei geschlossen");
                                    logger.Log("File closed");
                                #endif

                                processingLocationInfo(LoggedEvents);

                                LoggedEvents.Clear();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("AnalyseError");
                    logger.Log(ex.Message + "\n" + ex.StackTrace + "\n\n");
                }

                #if extScanLog
                    logger.Log("sleeping...");
                    logger.Log("\n\n\n");
                    Debug.Print("\n\n\n");
                #endif

                m_LogfileScanner_ARE.WaitOne();

                #if extScanLog
                    logger.Log("awake...");
                #endif

            }while (!m_Closing);

            #if extScanLog
                Debug.Print("out");
            #endif

        }

        /// <summary>
        /// tries to extract the timestamp from a logline
        /// </summary>
        /// <param name="currentLogString"></param>
        /// <param name="extractedTimeStamp"></param>
        /// <returns></returns>
        private Boolean TryGetTimeFromLine(string currentLogString, ref DateTime extractedTimeStamp)
        {
            Int32 StartBracket = -1;
            Int32 EndBracket = -1;
            Boolean success;

            try
            {
                success             = false;
                extractedTimeStamp  = DateTime.MaxValue;

                try
                {
                    StartBracket        = currentLogString.IndexOf('{', 0, 5);
                    EndBracket          = currentLogString.IndexOf('}', 0, 15);
                }
                catch (Exception)
                {
                }

                if((StartBracket >= 0) && (EndBracket >= 0) && ((EndBracket - StartBracket) > 0))
                {
                    success = DateTime.TryParse(currentLogString.Substring(StartBracket+1, EndBracket - (StartBracket+1)), out extractedTimeStamp);
                }

                return success;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the time from a logline", ex);
            }
        }

        /// <summary>
        /// extracting the station form a regex-match
        /// </summary>
        /// <param name="stationName"></param>
        /// <param name="m"></param>
        private void getLocation(ref string stationName, Match m)
        {
            string[] parts = m.Groups[0].ToString().Split(':');

            if (parts.GetUpperBound(0) >= 3)
            {
                stationName = parts[parts.GetUpperBound(0)-1];
            }
        }

        /// <summary>
        /// processing the collected informations
        /// </summary>
        /// <param name="LoggedEvents"></param>
        private void processingLocationInfo(List<LogEvent> LoggedEvents)
        {
            //Boolean SystemHasChanged   = false;
            //Boolean LocationHasChanged = false;
            String OldSystemString;
            String OldLocationString;
            enLogEvents EventFlags = enLogEvents.None;

            try
            {
                if(LoggedEvents.Count() > 0)
                { 
                    OldSystemString   = Program.actualCondition.System;
                    OldLocationString = Program.actualCondition.Location;

                    // order by date
                    LoggedEvents = LoggedEvents.OrderBy(x => x.Time).ToList();

                    // scan sequence
                    foreach (LogEvent Event in LoggedEvents)
                    {
                        switch (Event.EventType)
                        {
                            case enLogEvents.Jump:
                                // after a jump we are no longer on a station
                                if(!String.IsNullOrEmpty(Program.actualCondition.Location))
                                { 
                                    EventFlags |= enLogEvents.Location;
                                    Program.actualCondition.Location  = Event.Value;
                                }
                                    
                                EventFlags |= enLogEvents.Jump;

                                Debug.Print("log - scanning : jump found");
                                break;

                            case enLogEvents.System:
                                // a new system is everytime valid, check if the system has changed
                                if((Event.Value != "") && (!Event.Value.Equals(Program.actualCondition.System, StringComparison.InvariantCultureIgnoreCase)))
                                { 
                                    EventFlags |= enLogEvents.System;
                                    Program.actualCondition.System  = Event.Value;

                                    // after a jump we are no longer on a station
                                    if(!String.IsNullOrEmpty(Program.actualCondition.Location))
                                    { 
                                        EventFlags |= enLogEvents.Location;
                                        Program.actualCondition.Location  = "";
                                    }
                                    Debug.Print("log - scanning : system found : " + Event.Value);
                                }
                                break;

                            case enLogEvents.Location:
                                // a new station is everytime valid, check if the station has changed
                                if((Event.Value != "") && (!Event.Value.Equals(Program.actualCondition.Location, StringComparison.InvariantCultureIgnoreCase)))
                                { 
                                    EventFlags |= enLogEvents.Location;
                                    Program.actualCondition.Location  = Event.Value;
                                }
                                Debug.Print("log - scanning : location found : " + Event.Value);
                                break;

                        }    
                    }

                    if(EventFlags != enLogEvents.None)
                    { 
                        // something has changed -> fire event
                        var EA = new LocationChangedEventArgs() { System        = Program.actualCondition.System,  
                                                                  Location      = Program.actualCondition.Location,
                                                                  OldSystem     = OldSystemString,  
                                                                  OldLocation   = OldLocationString,
                                                                  Changed       = EventFlags};
                        LocationChanged.Raise(this, EA);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing location info", ex);
            }
        }
    }
}
