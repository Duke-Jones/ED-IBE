using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using IBE.SQL;
using IBE.Enums_and_Utility_Classes;
using System.Collections.Generic;

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
            Recieved    =  1,
            Error       =  2
        }
        
 #endregion
        
        const String REPLACESTRING_OLD_COMMENT = "!$!OC!$!";

        public enum TransmissionType
        {
            Visit               = 0, 
            CommentExtension    = 1
        }


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

        public const String                              DB_GROUPNAME                    = "EDSM_API";
        private DBGuiInterface                           m_GUIInterface;
        //private FileScanner.EDLogfileScanner             m_LogfileScanner;
        private FileScanner.EDJournalScanner             m_JournalScanner;
        private String                                   m_CurrentVersion;
        private SingleThreadLogger                       m_LogFile;
        private Queue<EDSMTransmissionData>              m_SendQueue;
        private System.Timers.Timer                      m_SendTimer;

        public EDStarmapInterface(DBConnector dbConnection)
        {
            try
            {
                m_SendQueue             = new Queue<EDSMTransmissionData>(100);
                m_LogFile               = new SingleThreadLogger(ThreadLoggerType.EDSMInterface, null, true);
                m_CurrentVersion        = Enums_and_Utility_Classes.VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3);
                m_GUIInterface          = new DBGuiInterface(DB_GROUPNAME, dbConnection);

                m_SendTimer             = new System.Timers.Timer();
                m_SendTimer.AutoReset   = false;
                m_SendTimer.Elapsed    += i_TransmitQueuedData;
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
        /// <param name="JournalScanner"></param>
        public void registerJournalScanner(FileScanner.EDJournalScanner JournalScanner)
        {
            try
            {
                if(m_JournalScanner == null)
                { 
                    m_JournalScanner = JournalScanner;
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
        /// event-worker for JournalEventRecieved-event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void JournalEventRecieved(object sender, FileScanner.EDJournalScanner.JournalEventArgs e)
        {
            try
            {
                if(e.EventType == FileScanner.EDJournalScanner.JournalEvent.FSDJump) 
                    TransmitVisit(e.Data.Value<String>("StarSystem"), (Double)e.Data["StarPos"][0], (Double)e.Data["StarPos"][0], (Double)e.Data["StarPos"][0], e.Data.Value<DateTime>("timestamp"));

            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the JournalEventRecieved-event", ex);
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
            String cmdrName = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "CommandersName", "");
            String apiKey   = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "API_Key", "");

            try
            {

                if(String.IsNullOrWhiteSpace(cmdrName) || String.IsNullOrWhiteSpace(apiKey)) 
                    throw new Exception("Invalid credentials for EDSM");

                answer = GetDataFromServer(String.Format("/api-logs-v1/get-position?commanderName={0}&apiKey={1}",
                                                          System.Web.HttpUtility.UrlEncode(cmdrName), apiKey));

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

        public void TransmitCommentExtension(String systemName, String stationName, string commentExtension, DateTime dateVisited)
        {
            String transmissionString;

            try
            {
                m_SendQueue.Enqueue(new EDSMTransmissionData() { TType          = TransmissionType.CommentExtension,
                                                                 SystemName     = systemName,
                                                                 Comment        = commentExtension,
                                                                 StationName    = stationName,
                                                                 DateVisited    = dateVisited});
            }
            catch (Exception ex)
            {
                throw new Exception("Error while login test", ex);
            }
        }

        public class EDSMTransmissionData
        {
            public TransmissionType    TType;
            public String              SystemName;
            public String              StationName;
            public Double?             X;
            public Double?             Y;
            public Double?             Z;
            public DateTime            DateVisited;
            public String              Comment;
        }
        

        public void TransmitVisit(String systemName, Double? x, Double? y, Double? z, DateTime dateVisited)
        {
            try
            {
                m_SendQueue.Enqueue(new EDSMTransmissionData() { TType          = TransmissionType.Visit,
                                                                 SystemName     = systemName,
                                                                 X              = x,
                                                                 Y              = y,
                                                                 Z              = z,
                                                                 DateVisited    = dateVisited,
                                                                 Comment        = ""});
            }
            catch (Exception ex)
            {
                throw new Exception("Error while login test", ex);
            }
        }

        public void i_TransmitQueuedData(object sender, System.Timers.ElapsedEventArgs e)
        {
            EDSMTransmissionData currentData; 

            try
            {

                m_SendTimer.Stop();

                if(m_SendQueue.Count > 0)
                {
                    while (m_SendQueue.Count > 0)
                    {
                        currentData     = m_SendQueue.Dequeue();

                        if(m_GUIInterface.GetIniValue<Boolean>("SendToEDSM", true.ToString(), false))
                        {
                            switch (currentData.TType)
                            {
                                case TransmissionType.Visit:
                                    i_TransmitVisit(currentData);
                                    break;
                                case TransmissionType.CommentExtension:
                                    String oldComment = GetSystemComment(currentData.SystemName, currentData.DateVisited);

                                    if(!String.IsNullOrWhiteSpace(oldComment))
                                    {
                                        currentData.Comment = oldComment + "\r\n" + currentData.Comment;
                                    }
                                    
                                    i_TransmitComment(currentData);
                                    break;
                                default:
                                    break;
                            }
                            
                        }

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

        private void i_TransmitVisit(EDSMTransmissionData data)
        {
            dynamic answer = null; 
            ErrorCodes retValue = ErrorCodes.No_Answer;
            String transmissionString;
            String cmdrName = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "CommandersName", "");
            String apiKey   = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "API_Key", "");

            try
            {

                if(String.IsNullOrWhiteSpace(cmdrName) || String.IsNullOrWhiteSpace(apiKey)) 
                    throw new Exception("Invalid credentials for EDSM");

                if (data.X.HasValue && data.Y.HasValue && data.Z.HasValue)
                {
                    transmissionString = String.Format("/api-logs-v1/set-log" +
                                            "?commanderName={0}" +
                                            "&apiKey={1}" +
                                            "&systemName={2}" +
                                            "&x={3}" +
                                            "&y={4}" +
                                            "&z={5}" +
                                            "&fromSoftware={6}" +
                                            "&fromSoftwareVersion={7}" +
                                            "&dateVisited={8:yyyy-MM-dd HH:mm:ss}",
                                            System.Web.HttpUtility.UrlEncode(cmdrName),
                                            apiKey,
                                            System.Web.HttpUtility.UrlEncode(data.SystemName),
                                            data.X.ToString().Replace(",", "."),
                                            data.Y.ToString().Replace(",", "."),
                                            data.Z.ToString().Replace(",", "."),
                                            "ED-IBE",
                                            m_CurrentVersion,
                                            data.DateVisited.ToUniversalTime());
                }
                else
                {
                    transmissionString = String.Format("/api-logs-v1/set-log" +
                                            "?commanderName={0}" +
                                            "&apiKey={1}" +
                                            "&systemName={2}" +
                                            "&fromSoftware={3}" +
                                            "&fromSoftwareVersion={4}" +
                                            "&dateVisited={5:yyyy-MM-dd HH:mm:ss}",
                                            System.Web.HttpUtility.UrlEncode(cmdrName),
                                            apiKey,
                                            System.Web.HttpUtility.UrlEncode(data.SystemName),
                                            "ED-IBE",
                                            m_CurrentVersion,
                                            data.DateVisited.ToUniversalTime());

                }

                if(m_GUIInterface.GetIniValue<Boolean>("SaveToFile", false.ToString(), false))
                    m_LogFile.Log(RemoveApiKey(transmissionString));

                answer = GetDataFromServer(transmissionString);

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

            }
            catch (Exception ex)
            {
                throw new Exception("Error while transmitting visit", ex);
            }
        }

        public String GetSystemComment(String systemName)
        {
            return GetSystemComment(systemName, new DateTime(1900,1,1,0,0,0));
        }

        public String GetSystemComment(String systemName, DateTime timestamp)
        {
            dynamic answer = null; 
            ErrorCodes retValue = ErrorCodes.No_Answer;
            String transmissionString;
            String commentString;
            String cmdrName = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "CommandersName", "");
            String apiKey   = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "API_Key", "");

            try
            {

                if(String.IsNullOrWhiteSpace(cmdrName) || String.IsNullOrWhiteSpace(apiKey)) 
                    throw new Exception("Invalid credentials for EDSM");

                if(timestamp.Year > 1970)
                {
                    transmissionString = String.Format("/api-logs-v1/get-comment" +
                                            "?commanderName={0}" +
                                            "&apiKey={1}" +
                                            "&systemName={2}" +
                                            "&dateVisited={3:yyyy-MM-dd HH:mm:ss}",
                                            System.Web.HttpUtility.UrlEncode(cmdrName),
                                            apiKey,
                                            System.Web.HttpUtility.UrlEncode(systemName), 
                                            timestamp.ToUniversalTime());
                }
                else
                {
                    transmissionString = String.Format("/api-logs-v1/get-comment" +
                                            "?commanderName={0}" +
                                            "&apiKey={1}" +
                                            "&systemName={2}",
                                            System.Web.HttpUtility.UrlEncode(cmdrName),
                                            apiKey,
                                            System.Web.HttpUtility.UrlEncode(systemName));
                }

                if(m_GUIInterface.GetIniValue<Boolean>("SaveToFile", false.ToString(), false))
                    m_LogFile.Log(RemoveApiKey(transmissionString));

                answer = GetDataFromServer(transmissionString);

                if(answer != null)
                {
                    if(m_GUIInterface.GetIniValue<Boolean>("SaveToFile", false.ToString(), false))
                        m_LogFile.Log(RemoveApiKey(answer.ToString()));

                    retValue = answer.msgnum;
                }

                switch (retValue)
                {
                    case ErrorCodes.OK:
                        commentString = answer.comment;
                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedStates.Recieved, m_SendQueue.Count));
                        break;
                    case ErrorCodes.OK_NoData:
                        commentString = "";
                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedStates.Recieved, m_SendQueue.Count));
                        break;
                    default:
                        commentString = null;
                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedStates.Error, m_SendQueue.Count));
                        break;
                }

                return commentString;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while recieving comment", ex);
            }
        }

        private void i_TransmitComment(EDSMTransmissionData data)
        {
            dynamic answer = null; 
            ErrorCodes retValue = ErrorCodes.No_Answer;
            String transmissionString;
            String cmdrName = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "CommandersName", "");
            String apiKey   = m_GUIInterface.DBConnection.getIniValue(DB_GROUPNAME, "API_Key", "");

            try
            {

                if(String.IsNullOrWhiteSpace(cmdrName) || String.IsNullOrWhiteSpace(apiKey)) 
                    throw new Exception("Invalid credentials for EDSM");

                transmissionString = String.Format("/api-logs-v1/set-comment" +
                                        "?commanderName={0}" +
                                        "&apiKey={1}" +
                                        "&systemName={2}" +
                                        "&fromSoftware={3}" +
                                        "&fromSoftwareVersion={4}" +
                                        "&dateVisited={5:yyyy-MM-dd HH:mm:ss}" +
                                        "&comment={6}",
                                        System.Web.HttpUtility.UrlEncode(cmdrName),
                                        apiKey,
                                        System.Web.HttpUtility.UrlEncode(data.SystemName),
                                        "ED-IBE",
                                        m_CurrentVersion,
                                        data.DateVisited.ToUniversalTime(),
                                        System.Web.HttpUtility.UrlEncode(data.Comment));


                if(m_GUIInterface.GetIniValue<Boolean>("SaveToFile", false.ToString(), false))
                    m_LogFile.Log(RemoveApiKey(transmissionString));

                answer = GetDataFromServer(transmissionString);

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

            }
            catch (Exception ex)
            {
                throw new Exception("Error while transmitting comment", ex);
            }
        }

    }
}
