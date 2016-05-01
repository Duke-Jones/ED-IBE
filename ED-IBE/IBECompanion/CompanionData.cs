using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using EDCompanionAPI;
using EDCompanionAPI.Models;
using System.Windows.Forms;

namespace IBE.IBECompanion
{
    public class CompanionData           
    {
        private EliteCompanion                  m_CompanionIO;
        private JObject                         m_joCompanion = new JObject();

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
        public void ConditionalLogIn()
        {
            try
            {
                if(CompanionStatus == LoginStatus.Ok)
                { 
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while logging in", ex);
            }
        }

        /// <summary>
        /// gets the whole profile data from the FD-servers
        /// </summary>
        /// <returns></returns>
        internal ProfileResponse GetProfileData()
        {
            try
            {
                ProfileResponse response = m_CompanionIO.GetProfileData();

                if (!response.Cached)
                {
                    var json = response.Json ?? "";
                    m_joCompanion = JObject.Parse(json);

                    CompanionStatus = response.LoginStatus;
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
    }
}
