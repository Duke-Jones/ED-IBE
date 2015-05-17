using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using RegulatedNoise.Annotations;

namespace RegulatedNoise.EliteInteractions
{
    public class LogFilesScanner : IDisposable
    {
        public event EventHandler<LocationUpdateEventArgs> OnCurrentLocationUpdate;

        readonly SingleThreadLogger _filescanningLog;

        const long SEARCH_MAXLENGTH = 160;
        const long SEARCH_MINLENGTH = 5;

        private readonly RegulatedNoiseSettings _settings;
        private Timer _pollingTimer;
        private bool _disposed;

        public LogFilesScanner([NotNull] RegulatedNoiseSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _filescanningLog = new SingleThreadLogger(ThreadLoggerType.FileScanner);
        }

        public int PollingPeriod { get; set; }

        public void UpdateSystemNameFromLogFile()
        {
            if (_disposed) return;
            if (_pollingTimer == null)
            {
                _pollingTimer = new Timer(state => { ParseNetLogFiles(); }, null, 0, PollingPeriod);
            }
            else
            {
                _pollingTimer.Change(0, PollingPeriod);
            }
        }

        private string[] FindNetLogs()
        {
            string[] netLogs = null;
            string appConfigPath = _settings.ProductsPath;
            if (Directory.Exists(appConfigPath))
            {
                var folder = Directory.GetDirectories(appConfigPath)
                        .Where(x => x.Contains("FORC-FDEV"))
                        .OrderByDescending(x => x)
                        .FirstOrDefault();
                if (folder != null)
                {
#if extScanLog
                    _filescanningLog.Log("lookin' for files in <" + versions[0] + ">");
#endif
                    // We'll just go right ahead and use the latest log...
                    netLogs =
                        Directory.GetFiles(Path.Combine(folder, "Logs"), "netLog*.log")
                            .OrderByDescending(File.GetLastWriteTime)
                            .ToArray();
                }
                else
                {
#if extScanLog
                    _filescanningLog.Log("no dirs with <FORC-FDEV> found");
                    var versions2 = Directory.GetDirectories(appConfigPath).ToList().OrderByDescending(x => x).ToList();
                    foreach (string SubPath in versions2)
                    {
                        _filescanningLog.Log("but found <" +  SubPath + ">");   
                    }
#endif
                }
            }
            return netLogs ?? new string[0];
        }

        private void ParseNetLogFiles()
        {
            try
            {
#if extScanLog
                    _filescanningLog.Log("start, RegEx = <" + String.Format("FindBestIsland:.+:.+:.+:.+", Regex.Escape(RegulatedNoiseSettings.PilotsName)) + ">");
#endif
                var netLogs = FindNetLogs();
                if (netLogs.Length != 0)
                {
                    var newestNetLog = netLogs[0];

                    Debug.Print("File opened : <" + newestNetLog + ">");
#if extScanLog
                    _filescanningLog.Log("File opened : <" + newestNetLog + ">");
#endif
                    LocationUpdateEventArgs evt = null;
                    using (FileStream datei = new FileStream(newestNetLog, FileMode.Open
                                                                , FileAccess.Read, FileShare.ReadWrite))
                    {
                        evt = ParseNetLogFile(datei);
                    }
                    Debug.Print("Datei geschlossen");
#if extScanLog
                    _filescanningLog.Log("File closed");
#endif
                    if (evt != null)
                    {
                        RaiseCurrentLocationUpdate(evt);
                    }

                    //                                if (systemName != "")
                    //                                {
                    //                                    Debug.Print("<" + systemName + "> - <" + tbCurrentSystemFromLogs.Text + ">");

                    //                                    setSystemInfo(systemName);

                    //                                }

                    //                                if (stationName != "")
                    //                                {
                    //                                    Debug.Print("<" + systemName + "> - <" + tbCurrentSystemFromLogs.Text + ">");

                    //                                    setSystemInfo(systemName);

                    //                                }

                    //                                    if (_LoggedSystem != systemName)
                    //                                    {
                    //#if extScanLog
                    //                                        _filescanningLog.Log("1 <" + systemName + "> - <" + tbCurrentSystemFromLogs.Text + ">");
                    //                                        _filescanningLog.Log("1 <" + stationName + "> - <" + tbCurrentStationinfoFromLogs.Text + ">");
                    //#endif

                    //                                        // "ClientArrivedtoNewSystem()" was often faster - so nothing was logged
                    //                                        if (cbAutoAdd_JumpedTo.Checked)
                    //                                        {
                    //                                            CommandersLog_CreateJumpedToEvent(systemName);
                    //                                        }

                    //                                        _LoggedSystem = systemName;

                    //                                        if (!String.IsNullOrEmpty(stationName))
                    //                                            m_lastestStationInfo = stationName;
                    //                                        else
                    //                                            m_lastestStationInfo = "scanning...";
                    //                                    }
                    //                                    else if (!String.IsNullOrEmpty(stationName))
                    //                                    { 
                    //#if extScanLog
                    //                                        _filescanningLog.Log("2 <" + stationName + "> - <" + tbCurrentStationinfoFromLogs.Text + ">");
                    //#endif
                    //                                        m_lastestStationInfo = stationName;
                    //                                    }

                    //                                    //if (tbLogEventID.Text != "" && tbLogEventID.Text != systemName)
                    //                                    //{
                    //                                        setSystemInfo(systemName);
                    //                                    //}

                    //#if extScanLog
                    //                                    _filescanningLog.Log("Found <" + systemName + "> - <" + m_lastestStationInfo + ">");
                    //                                    _filescanningLog.Log("GUI   <" + tbCurrentSystemFromLogs.Text + "> - <" + tbCurrentStationinfoFromLogs.Text + ">");
                    //#endif
                    //                                }


                    //                                setStationInfo();
                }
            }
            catch (Exception ex)
            {
                Debug.Print("AnalyseError");
                _filescanningLog.Log(ex.Message + "\n" + ex.StackTrace + "\n\n");
            }
        }

        private LocationUpdateEventArgs ParseNetLogFile(Stream netlogFile)
        {
            string stationName = "";
            string systemName = "";
            var regExTest = new Regex("FindBestIsland:.+:.+:.+:.+", RegexOptions.IgnoreCase);
            var byteBuffer = new Byte[1];
            var lineBuffer = new Byte[SEARCH_MAXLENGTH];
            var possibleStations = new List<string>();

            netlogFile.Seek(0, SeekOrigin.End);

            while (String.IsNullOrEmpty(stationName) && (netlogFile.Position >= 2))
            {
                long startPos = -1;
                long endPos = -1;
                do
                {
                    netlogFile.Read(byteBuffer, 0, byteBuffer.Length);

                    if ((byteBuffer[0] == 0x0A) || (byteBuffer[0] == 0x0D))
                        if (endPos == -1)
                        {
                            if (byteBuffer[0] == 0x0D)
                                endPos = netlogFile.Position + 1;
                            else
                                endPos = netlogFile.Position;

                            netlogFile.Seek(-3, SeekOrigin.Current);
                        }
                        else
                        {
                            if (byteBuffer[0] == 0x0D)
                                startPos = netlogFile.Position + 1;
                            else
                                startPos = netlogFile.Position;
                        }
                    else
                        netlogFile.Seek(-3, SeekOrigin.Current);
                } while (startPos == -1 && netlogFile.Position >= 3);

                if ((startPos == -1) && ((endPos - startPos) > SEARCH_MINLENGTH))
                    startPos = 0;

                if ((startPos >= 0) && ((endPos - startPos) <= SEARCH_MAXLENGTH))
                {
                    // found a line and it's not too long
                    // read
                    netlogFile.Read(lineBuffer, 0, (int)(endPos - startPos));
                    // and convert to string
                    var logLump = Encoding.ASCII.GetString(lineBuffer, 0, (int)(endPos - startPos));

                    // first looking for the systemname
                    Match m;
                    if (String.IsNullOrEmpty(systemName))
                    {
                        if (logLump.Contains("System:"))
                        {
                            Debug.Print("Systemstring:" + logLump);
#if extScanLog
                            _filescanningLog.Log("Systemstring:" + logLump.Replace("\n", "").Replace("\r", ""));
#endif
                            systemName =
                                logLump.Substring(logLump.IndexOf("(", StringComparison.Ordinal) + 1);
                            systemName = systemName.Substring(0,
                                systemName.IndexOf(")", StringComparison.Ordinal));

                            Debug.Print("System: " + systemName);
#if extScanLog
                            _filescanningLog.Log("System: " + systemName);
#endif
                            // preparing search for station info
                            regExTest =
                                new Regex(
                                    String.Format("FindBestIsland:.+:.+:.+:{0}",
                                        Regex.Escape(systemName)), RegexOptions.IgnoreCase);
#if extScanLog
                            _filescanningLog.Log("new Regex : <" + String.Format("FindBestIsland:.+:.+:.+:{0}", Regex.Escape(systemName)) + ">");
#endif
                            // start search at the beginning
                            // we may have candidates, check them and if nothing found search from the current position
                            foreach (string candidate in possibleStations)
                            {
                                Debug.Print("check candidate : " + candidate);
#if extScanLog
                            _filescanningLog.Log("check candidate : " + candidate.Replace("\n", "").Replace("\r", ""));
#endif
                                m = regExTest.Match(candidate);
                                //Debug.Print(logLump);
                                //if (logLump.Contains("Duke Jones"))
                                //    Debug.Print("Stop");
                                if (m.Success)
                                {
#if extScanLog
                            _filescanningLog.Log("Stationstring from candidate : " + candidate.Replace("\n", "").Replace("\r", ""));
#endif
                                    Debug.Print("Stationstring from candidate : " + candidate);
                                    GetStation(ref stationName, m);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            m = regExTest.Match(logLump);
                            //Debug.Print(logLump);
                            //if (logLump.Contains("Duke Jones"))
                            //    Debug.Print("Stop");
                            if (m.Success)
                            {
#if extScanLog
                                _filescanningLog.Log("Candidate added : " + logLump.Replace("\n", "").Replace("\r", ""));
#endif
                                Debug.Print("Candidate : " + logLump);
                                possibleStations.Add(logLump);
                            }
                        }
                    }

                    // if we have the systemname we're looking for the stationname
                    if (!string.IsNullOrEmpty(systemName) && string.IsNullOrEmpty(stationName))
                    {
                        m = regExTest.Match(logLump);
                        //Debug.Print(logLump);
                        //if (logLump.Contains("Duke Jones"))
                        //    Debug.Print("Stop");
                        if (m.Success)
                        {
#if extScanLog
                            _filescanningLog.Log("Stationstring (direct) : " + logLump.Replace("\n", "").Replace("\r", ""));
#endif
                            Debug.Print("Stationstring (direct) : " + logLump);
                            GetStation(ref stationName, m);
                        }
                    }
                }

                if (startPos >= 3)
                {
                    netlogFile.Seek(startPos - 1, SeekOrigin.Begin);
                }
                else
                    netlogFile.Seek(0, SeekOrigin.Begin);
            }
            return new LocationUpdateEventArgs(systemName, stationName);
        }

        protected void RaiseCurrentLocationUpdateEvent(string systemName, string stationName)
        {
            RaiseCurrentLocationUpdate(new LocationUpdateEventArgs(systemName, stationName));
        }

        private void GetStation(ref string stationName, Match m)
        {
            string[] parts = m.Groups[0].ToString().Split(':');
            if (parts.GetUpperBound(0) >= 3)
            {
                stationName = parts[parts.GetUpperBound(0) - 1];

                if (parts[0].Equals("FindBestIsland", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (String.IsNullOrEmpty(_settings.PilotsName))
                        _settings.PilotsName = parts[1];
                }
                else
                {
                    if (String.IsNullOrEmpty(_settings.PilotsName))
                        _settings.PilotsName = parts[0];
                }
            }
        }

        protected virtual void RaiseCurrentLocationUpdate(LocationUpdateEventArgs e)
        {
            var handler = OnCurrentLocationUpdate;
            if (handler != null)
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("current location update notification failure: " + ex);
                }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                if (_pollingTimer != null)
                {
                    _pollingTimer.Dispose();
                    _pollingTimer = null;
                }
            }
        }
    }

    public class LocationUpdateEventArgs : EventArgs
    {
        public readonly string System;

        public readonly string Station;

        public LocationUpdateEventArgs(string system, string station)
        {
            System = system;
            Station = station;
        }

        public override string ToString()
        {
            return System + " [" + Station + "]";
        }
    }
}