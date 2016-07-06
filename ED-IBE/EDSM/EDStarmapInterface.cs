using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using IBE.SQL;
using IBE.Enums_and_Utility_Classes;

namespace IBE.EDSM
{
    public class EDStarmapInterface
    {
#region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<DataTransmittedEventArgs> DataTransmittedEvent;

        public class DataTransmittedEventArgs : EventArgs
        {
            public DataTransmittedEventArgs(enTransmittedStates tState, Int32 inQueue)
            {
                DataState = tState;
                InQueue   = inQueue;

 
            }

            public enTransmittedStates DataState              { get; set; }
            public Int32               InQueue                { get; set; }
        }


        public enum enTransmittedStates
        {
            Sent        =  0,
            Error       =  1
        }
        
 #endregion

        public enum ErrorCodes
        {
            No_Answer                                               =  -1,
            OK                                                      = 100,
            OK_NoData                                               = 101,
            Missing_commander_name                                  = 201,
            Missing_API_key                                         = 202,
            Commander_name_or_API_Key_not_found                     = 203,
            Missing_comment                                         = 204,
            Missing_date_visited                                    = 205,
            Date_is_not_in_the_correct_format                       = 206,
            Missing_system_name                                     = 301,
            System_not_in_database                                  = 302,
            System_probably_non_existant                            = 303,
            System_is_a_training_mission                            = 304,
            System_name_is_too_long_or_invalid                      = 305,
            System_already_exists_at_that_date                      = 401,
            System_already_exists_just_before_the_visited_date      = 402,
            System_already_exists_just_after_the_visited_date       = 403,
            Flight_log_entry_not_found                              = 404
        }

        private EDStarmapInterfaceView   m_GUI;

        String m_BaseURL                 = @"https://www.edsm.net";

        public const String                                     DB_GROUPNAME                    = "EDSM_API";
        private DBGuiInterface                                  m_GUIInterface;
        private FileScanner.EDLogfileScanner                    m_LogfileScanner;
        private String                                          m_CurrentVersion;
        private SingleThreadLogger                              m_LogFile;
        private System.Collections.Generic.Queue<String>        m_SendQueue;
        private System.Timers.Timer                             m_SendTimer;

        public EDStarmapInterface(DBConnector dbConnection)
        {
            try
            {
                m_SendQueue             = new System.Collections.Generic.Queue<String>(100);
                m_LogFile               = new SingleThreadLogger(ThreadLoggerType.EDSMInterface, null, true);
                m_CurrentVersion        = Enums_and_Utility_Classes.VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3);
                m_GUIInterface          = new DBGuiInterface(DB_GROUPNAME, dbConnection);

                m_SendTimer             = new System.Timers.Timer();
                m_SendTimer.AutoReset   = false;
                m_SendTimer.Elapsed    += i_TransmitLogEntry;
                m_SendTimer.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while initialization", ex);
            }
        }

        private void M_SendTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// access to the belonging gui object
        /// </summary>
        public EDStarmapInterfaceView GUI
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

        public DBGuiInterface GUIInterface
        {
            get
            {
                return m_GUIInterface;
            }
        }

        /// <summary>
        /// register the LogfileScanner in the CommandersLog for the DataEvent
        /// </summary>
        /// <param name="LogfileScanner"></param>
        public void registerLogFileScanner(FileScanner.EDLogfileScanner LogfileScanner)
        {
            try
            {
                if(m_LogfileScanner == null)
                { 
                    m_LogfileScanner = LogfileScanner;
                    m_LogfileScanner.LocationChanged += LogfileScanner_LocationChanged;
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
        /// event-worker for DataSavedEvent-event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LogfileScanner_LocationChanged(object sender, FileScanner.EDLogfileScanner.LocationChangedEventArgs e)
        {
            try
            {
                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.System) > 0)
                {
                    if(e.Position.Valid)
                        TransmitLogEntry(e.System, e.Position.X.Value, e.Position.Y.Value, e.Position.Z.Value, e.TimeStamp);
                    else
                        TransmitLogEntry(e.System, null, null, null, e.TimeStamp);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the LocationChanged-event", ex);
            }
        }


        private string RemoveApiKey(string request)
        {
            var apiKeyMatch = System.Text.RegularExpressions.Regex.Match(request, "apiKey=.*?&").ToString();

            if((apiKeyMatch != null) && (apiKeyMatch.ToString().Length > 0))
            {
                request = request.Replace(apiKeyMatch, "apiKey=APIKEY_REMOVED&");
            }
            
            return request;

        }

        public dynamic GetDataFromServer(String commandstring)
        {
            
            HttpWebRequest webRequest   = System.Net.WebRequest.Create(m_BaseURL + commandstring) as HttpWebRequest;
            webRequest.Method           = "GET";
            webRequest.UserAgent        = "ED-IBE";
            webRequest.ServicePoint.Expect100Continue = false;
                
            String responseData;
            dynamic data = null;

            try
            {
                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    responseData = responseReader.ReadToEnd();

                try
                {
                    data = JsonConvert.DeserializeObject<dynamic>(responseData);
                }
                catch (Exception)
                {
                }

                return data;
            }
            catch
            {
                return data;
            }
        }

        public ServerStatus ServerStatus()
        {
            dynamic answer;
            ServerStatus status = new ServerStatus() { LastUpdate = new DateTime(1900,1,1,0,0,0), Message="Server not accessible or unknown response", Status = ServerStates.Danger, Type = "Error"};

            try
            {
                answer = GetDataFromServer("/api-status-v1/elite-server");

                if(answer != null)
                {
                    status.LastUpdate   = answer.lastUpdate;
                    status.Type         = answer.type;
                    status.Message      = answer.message;
                    status.Status       = answer.status;
                }

                return status;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while login test", ex);
            }
        }

        public ErrorCodes LoginTest()
        {
            dynamic answer;
            ErrorCodes retValue = ErrorCodes.No_Answer;

            try
            {
                answer = GetDataFromServer(String.Format("/api-logs-v1/get-position?commanderName={0}&apiKey={1}",
                                                          m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "CommandersName", ""),
                                                          m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "API_Key", "")));

                if(answer != null)
                {
                    retValue = answer.msgnum;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while login test", ex);
            }
        }

        public void TransmitLogEntry(String systemName, Double? x, Double? y, Double? z, DateTime dateVisited)
        {
            dynamic answer = null; 
            ErrorCodes retValue = ErrorCodes.No_Answer;
            String request;

            try
            {
                if(x.HasValue && y.HasValue && z.HasValue)
                {
                    request = String.Format("/api-logs-v1/set-log" +
                                            "?commanderName={0}" +
                                            "&apiKey={1}" +
                                            "&systemName={2}" +
                                            "&x={3}" +
                                            "&y={4}" +
                                            "&z={5}" +
                                            "&fromSoftware={6}" +
                                            "&fromSoftwareVersion={7}" +
                                            "&dateVisited={8:yyyy-MM-dd HH:mm:ss}",
                                            m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "CommandersName", ""),
                                            m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "API_Key", ""), 
                                            systemName, 
                                            x.ToString().Replace(",","."), 
                                            y.ToString().Replace(",","."), 
                                            z.ToString().Replace(",","."),
                                            "ED-IBE",
                                            m_CurrentVersion, 
                                            dateVisited.ToUniversalTime());
                }
                else
                {
                    request = String.Format("/api-logs-v1/set-log" +
                                            "?commanderName={0}" +
                                            "&apiKey={1}" +
                                            "&systemName={2}" +
                                            "&fromSoftware={3}" +
                                            "&fromSoftwareVersion={4}" +
                                            "&dateVisited={5:yyyy-MM-dd HH:mm:ss}",
                                            m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "CommandersName", ""),
                                            m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "API_Key", ""), 
                                            systemName, 
                                            "ED-IBE",
                                            m_CurrentVersion, 
                                            dateVisited.ToUniversalTime());

                }


                m_SendQueue.Enqueue(request);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while login test", ex);
            }
        }

        public void i_TransmitLogEntry(object sender, System.Timers.ElapsedEventArgs e)
        {
            dynamic answer = null; 
            ErrorCodes retValue = ErrorCodes.No_Answer;
            String request;
            try
            {

                m_SendTimer.Stop();

                if(m_SendQueue.Count > 0)
                {
                    while (m_SendQueue.Count > 0)
                    {
                        retValue = ErrorCodes.No_Answer;

                        request = m_SendQueue.Dequeue();

                        if(m_GUIInterface.GetIniValue<Boolean>("SaveToFile", false.ToString(), false))
                        {
                            m_LogFile.Log(RemoveApiKey(request));
                        }

                        if(m_GUIInterface.GetIniValue<Boolean>("SendToEDSM", true.ToString(), false))
                            answer = GetDataFromServer(request);

                        if(answer != null)
                        {
                            if(m_GUIInterface.GetIniValue<Boolean>("SaveToFile", false.ToString(), false))
                                m_LogFile.Log(RemoveApiKey(answer.ToString()));

                            retValue = answer.msgnum;
                        }

                        if(retValue == ErrorCodes.OK)
                            DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedStates.Sent, m_SendQueue.Count));
                        else
                            DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedStates.Error, m_SendQueue.Count));

                        System.Threading.Thread.Sleep(25);
                    }

                    
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }
               
                m_SendTimer.Start();         

            }
            catch (Exception ex)
            {
                DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedStates.Error, m_SendQueue.Count));
                m_SendTimer.Start();         
                m_LogFile.Log("Exception: /n/d" + ex.Message + "/n/d" + ex.StackTrace);
            }
        }

    }
}
