using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using EDCompanionAPI;
using EDCompanionAPI.Models;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace IBE.IBECompanion
{
    public class CompanionData : DataEventBase           
    {
        private EliteCompanion                          m_CompanionIO;
        private JObject                                 m_joCompanion = new JObject();
        private ProfileResponse                         m_cachedResponse;
        private System.Timers.Timer                     m_reGetTimer;
        private FileScanner.EDJournalScanner            m_JournalScanner;

        /// <summary>
        /// creates the interface object
        /// </summary>
        /// <param name="dataPath"></param>
        public CompanionData(String dataPath)
        {
            try
            {
                m_CompanionIO             = EliteCompanion.Instance;
                m_CompanionIO.DataPath    = dataPath;

                m_reGetTimer = new System.Timers.Timer();
                m_reGetTimer.Interval = 5000;
                m_reGetTimer.Elapsed += m_reGetTimer_Elapsed;


            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating the companion data interface", ex);
            }
        }


        /// <summary>
        /// Logins active profile
        /// </summary>
        /// <returns>Login response object</returns>
        public LoginResponse Login()
        {
            try
            {
                LoginResponse resp = m_CompanionIO.Login();
                CompanionStatus = resp.Status;

                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while logging in", ex);
            }
        }

        /// <summary>
        /// only try to login if already done before
        /// </summary>
        public Boolean ConditionalLogIn()
        {
            Boolean retValue = false;

            try
            {
                switch (CompanionStatus)
                {
                    case LoginStatus.NotSet:
                        break;

                    case LoginStatus.Ok:
                    case LoginStatus.NotAccessible:
                        var profileExists = Program.CompanionIO.LoadProfile(Program.DBCon.getIniValue(CompanioDataView.DB_GROUPNAME, "EmailAddress"));
                        if (profileExists)
                        {
                            var loginResult = Login();

                            if(loginResult.Status != LoginStatus.Ok)
                            {
                                CompanionStatus = LoginStatus.NotSet;

                                if(!Program.SplashScreen.IsDisposed)
                                    Program.SplashScreen.TopMost = false;

                                MessageBox.Show("Warning: can't connect to companion server : <" + loginResult.Status.ToString() + ">", 
                                                "Companion Interface",  
                                                MessageBoxButtons.OK, 
                                                MessageBoxIcon.Exclamation);

                                if(!Program.SplashScreen.IsDisposed)
                                    Program.SplashScreen.TopMost = true;
                            }
                            else
                            {
                                retValue = true;
                            }
                        }
                        else
                        {
                            CompanionStatus = LoginStatus.NotSet;
                        }
                        break;

                    case LoginStatus.PendingVerification:
                        CompanionStatus = LoginStatus.NotSet;
                        break;

                    case LoginStatus.IncorrectCredentials:
                        CompanionStatus = LoginStatus.NotSet;
                        break;

                    case LoginStatus.UnknownError:
                        CompanionStatus = LoginStatus.NotSet;
                        break;

                    default:
                        break;
                }

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while logging in", ex);
            }
        }

        /// <summary>
        /// get the data from the servers async
        /// </summary>
        public void GetProfileDataAsync()
        {
            try
            {
                var starter = new System.Threading.Thread(GetProfileData_i);

                starter.IsBackground = true;
                starter.Name = "Companion.GetProfileDataAsync";
                starter.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting data async", ex);
            }
        }

        /// <summary>
        /// internal entrypoint for getting the data async
        /// </summary>
        /// <returns></returns>
        private void GetProfileData_i()
        {
            Boolean useCachedData = false;
            GetProfileData(useCachedData);
        }

        /// <summary>
        /// gets the whole profile data, returns cached data if existing
        /// </summary>
        /// <returns></returns>
        internal ProfileResponse GetProfileData()
        {
            Boolean useCachedData = true;
            return GetProfileData(useCachedData);
        }

        /// <summary>
        /// gets the whole profile data from the FD-servers
        /// </summary>
        /// <returns></returns>
        internal ProfileResponse GetProfileData(Boolean useCachedData)
        {
            ProfileResponse response;

            try
            {
                if((m_cachedResponse == null) || (!useCachedData))
                {
                    response = m_CompanionIO.GetProfileData();

                    if (!response.Cached)
                    {
                        String json = response.Json ?? "{}";

                        m_joCompanion = JsonConvert.DeserializeObject<JObject>(json);

                        CompanionStatus = response.LoginStatus;
                    }

                    m_cachedResponse = response;

                }else
                {
                    response = m_cachedResponse;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting data from the servers", ex);
            }
        }

        /// <summary>
        /// returns the JSON data object
        /// </summary>
        /// <returns></returns>
        public JObject GetData()
        {
            try
            {
                GetProfileData();

                return m_joCompanion;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting json data", ex);
            }
        }

        /// <summary>
        /// return a string value from the json data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public String GetValue(string valuePath)
        {
            try
            {
                return GetData().SelectToken(valuePath).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting string data value", ex);
            }
        }

        /// <summary>
        /// return a typed value from the json data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valuePath"></param>
        /// <returns></returns>
        public T GetValue<T>(string valuePath)
        {
            try
            {
                return (T)Convert.ChangeType(GetData().SelectToken(valuePath), typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting typed data value", ex);
            }
        }

        /// <summary>
        /// returns the json raw data
        /// </summary>
        /// <returns></returns>
        public String GetRawData()
        {
            try
            {
                return (GetProfileData().Json ?? "").ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting raw data", ex);
            }
        }

        /// <summary>
        /// imports the prices
        /// </summary>
        /// <returns></returns>
        protected override int ImportPrices()
        {
            return Program.Data.ImportPrices(GetData());
        }

        /// <summary>
        /// gets or sets the currently known staus of the companion interface
        /// </summary>
        public LoginStatus CompanionStatus
        {
            get 
            { 
                return Program.DBCon.getIniValue<LoginStatus>(CompanioDataView.DB_GROUPNAME, "Status", LoginStatus.NotSet.ToString(), false);
            }
            set 
            {
                Program.DBCon.setIniValue(CompanioDataView.DB_GROUPNAME, "Status", value.ToString());; 
            }
        }

        internal bool LoadProfile(string email)
        {
            return m_CompanionIO.LoadProfile(email);
        }

        internal void CreateProfile(string email, string password)
        {
            m_CompanionIO.CreateProfile(email, password);
        }

        internal VerificationResponse SubmitVerification(string verificationCode)
        {
            return m_CompanionIO.SubmitVerification(verificationCode);
        }

        internal void DeleteProfile(string email)
        {
            m_CompanionIO.DeleteProfile(email);
        }

        /// <summary>
        /// returns the resttime before a new request will send to the FD servers
        /// </summary>
        /// <returns></returns>
        internal TimeSpan RestTime()
        {
            return m_CompanionIO.RestTime();
        }

        /// <summary>
        /// sets the cooldowntimer to "finished"
        /// </summary>
        /// <returns></returns>
        public void RestTimeReset()
        { 
            m_CompanionIO.RestTimeReset();
        }
        
        /// <summary>
        /// returns true if the current data has the landed flag
        /// </summary>
        /// <returns></returns>
        internal bool IsLanded()
        {
            try
            {
                return Program.CompanionIO.GetValue<Boolean>("commander.docked");
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking if landed", ex);
            }
        }

        /// <summary>
        /// returns true if the current data has at least one marketable item
        /// </summary>
        /// <returns></returns>
        internal bool StationHasMarketData()
        {
            try
            {
                IEnumerable<JToken> stationData = GetData().SelectTokens("lastStarport.commodities[*]");

                if((stationData != null) && (stationData.Count() > 0))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking if station has market data", ex);
            }
        }

        /// <summary>
        /// returns true if the current data has at least one outfitting item
        /// </summary>
        /// <returns></returns>
        internal bool StationHasOutfittingData()
        {
            try
            {
                IEnumerable<JToken> stationData = GetData().SelectTokens("lastStarport.modules.*"); 

                if((stationData != null) && (stationData.Count() > 0))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking if landed", ex);
            }
        }

        /// <summary>
        /// returns true if the current data has at least one ship in the shipyard
        /// </summary>
        /// <returns></returns>
        internal bool StationHasShipyardData()
        {
            try
            {
                IEnumerable<JToken> stationData = GetData().SelectTokens("lastStarport.ships.shipyard_list.*"); 

                if((stationData != null) && (stationData.Count() > 0))
                    return true;
                else
                {
                    stationData = GetData().SelectTokens("lastStarport.ships.unavailable_list.[*]"); 

                    if((stationData != null) && (stationData.Count() > 0))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking if landed", ex);
            }
        }

        /// <summary>
        /// sets the "docked"-flag without calling the FD-servers
        /// </summary>
        internal void SetDocked(Boolean isLanded)
        {
            try
            {
                m_joCompanion["commander"]["docked"] = isLanded;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking if station has market data", ex);
            }
        }


#region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<EventArgs> AsyncDataRecievedEvent;

        protected virtual void OnAsyncDataRecieved(EventArgs e)
        {
            EventHandler<EventArgs> myEvent = AsyncDataRecievedEvent;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

 #endregion

        /// <summary>
        /// retrys to get shipyard data
        /// </summary>
        public void ReGet_StationData()
        {
            try
            {
                m_reGetTimer.Stop();
                m_reGetTimer.Start();

            }
            catch (Exception ex)
            {
                throw new Exception("Error while starting async re-getter", ex);
            }
        }

        void m_reGetTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                m_reGetTimer.Stop();
                
                var response = m_CompanionIO.GetProfileData(true);

                if (!response.Cached)
                {
                    String json = response.Json ?? "{}";

                    m_joCompanion = JsonConvert.DeserializeObject<JObject>(json);

                    CompanionStatus = response.LoginStatus;
                }

                m_cachedResponse = response;
                
                AsyncDataRecievedEvent.Raise(this, new EventArgs());

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in m_reGetTimer_Elapsed");
            }
        }

        /// <summary>
        /// gets the current credits *save*
        /// save means, returns "0" if somethings wrong
        /// </summary>
        /// <returns></returns>
        public Int32 SGetCreditsTotal()
        {
            Int32 creditsTotal = 0;

            try
            {
                if (CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok)
                    creditsTotal = GetValue<Int32>("commander.credits");
            }
            catch (Exception)
            {
            }

            return creditsTotal;
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
                    m_JournalScanner.JournalEventRecieved += JournalEventRecievedAsync;
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
        public async void JournalEventRecievedAsync(object sender, FileScanner.EDJournalScanner.JournalEventArgs e)
        {
            try
            {
                switch (e.EventType)
                {
                    case  FileScanner.EDJournalScanner.JournalEvent.Docked:
                        RestTimeReset();
                        SetDocked(true);

                        if((!Program.actualCondition.System.EqualsNullOrEmpty(e.Data.Value<String>("StarSystem"))) || 
                           (!Program.actualCondition.Location.EqualsNullOrEmpty(e.Data.Value<String>("StationName"))))
                        {
                            var t = new Task(() => RefreshAndImport(e.Data.Value<String>("StarSystem"), e.Data.Value<String>("StationName")));
                            t.Start();
                            await t;
                        }

                        break;

                    case  FileScanner.EDJournalScanner.JournalEvent.Undocked:
                        SetDocked(false);

                        break;

                    case FileScanner.EDJournalScanner.JournalEvent.FSDJump:
                        // can't be docked anymore
                        SetDocked(false);

                        /// after a system jump you can get data immediately
                        RestTimeReset();

                        break;



                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the JournalEventRecieved-event", ex);
            }
        }

        public void RefreshAndImport(String systemName, String stationName)
        {
            String extSystem    = "";
            String extStation   = "";
            DialogResult MBResult;

            try
            {
                if(Program.CompanionIO.CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok)
                { 
                    // delay to ensure companion io has got the landing information from the FD servers
                    System.Threading.Thread.Sleep(2000);

                    // allow refresh of companion data
                    Program.CompanionIO.GetProfileData(false);

                    if(Program.CompanionIO.IsLanded())
                    {
                        extSystem  = Program.CompanionIO.GetValue("lastSystem.name");
                        extStation = Program.CompanionIO.GetValue("lastStarport.name");

                        if(!systemName.Equals(extSystem, StringComparison.InvariantCultureIgnoreCase) && (!Program.actualCondition.GameversionIsBeta))
                        {
                            Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, "external recieved system does not correspond to the system from the jounal !");
                        }
                        else
                        {
                            if(Program.CompanionIO.StationHasMarketData())
                            {
                                Int32 count = Program.CompanionIO.ImportMarketData();
                                
                                if(Program.MainForm.cbEDDNOverride.Checked)
                                {
                                    Program.EDDNComm.SendCommodityData(Program.CompanionIO.GetData());
                                }

                                if(count > 0)
                                    Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, String.Format("getting market data...{0} prices collected", count));                        
                                else
                                    Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, String.Format("getting market data...no market data available !"));        
                                                
                            }
                            Program.MainForm.SetQuickDecisionSwitch();

                            if(Program.CompanionIO.StationHasShipyardData())
                            {
                                Program.EDDNComm.SendShipyardData(Program.CompanionIO.GetData());
                            }
                            else if(Program.DBCon.Execute<Boolean>("select has_shipyard from tbStations where id = " + Program.actualCondition.Location_ID))
                            {
                                // probably companion error, try once again in 5 seconds
                                Program.CompanionIO.ReGet_StationData();                                
                            }

                            if(Program.CompanionIO.StationHasOutfittingData())
                                Program.EDDNComm.SendOutfittingData(Program.CompanionIO.GetData());
                            
                        }
                    }
                    else
                    { 
                        Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, "You're not docked");                        
                    }

                }
                else
                {
                    Program.MainForm.AddComboboxLine(Program.MainForm.txtEventInfo, "Can't comply, companion interface not ready !");                        
                }

                Program.MainForm.ShowStatus();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdEventLanded_Click");
            }
        }

    }
}
