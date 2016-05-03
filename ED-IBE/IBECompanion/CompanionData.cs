using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using EDCompanionAPI;
using EDCompanionAPI.Models;
using System.Windows.Forms;
using IBE.Enums_and_Utility_Classes;

namespace IBE.IBECompanion
{
    public class CompanionData : DataEventBase           
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
                switch (CompanionStatus)
                {
                    case LoginStatus.NotSet:
                        break;

                    case LoginStatus.Ok:
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
                    var json = response.Json ?? "{}";
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
            String system;
            String starPort;
            Int32 commditCount = 0;
            List<String> csvStrings = new List<string>();

            try
            {
                system   = GetValue("lastSystem.name");
                starPort = GetValue("lastStarport.name");

                foreach (JToken commodity in GetData().SelectTokens("lastStarport.commodities[*]"))
                {                                                  
                    if(!commodity.Value<String>("categoryname").Equals("NonMarketable", StringComparison.InvariantCultureIgnoreCase))
                    {
                        CsvRow csvData = new CsvRow();

                        csvData.SystemName          = system;
                        csvData.StationName         = starPort;
                        csvData.StationID           = String.Format("{0}[{1}]", starPort, system);
                        csvData.CommodityName       = commodity.Value<String>("name");
                        csvData.SellPrice           = commodity.Value<Int32>("sellPrice");
                        csvData.BuyPrice            = commodity.Value<Int32>("buyPrice");
                        csvData.Demand              = commodity.Value<Int32>("demand");
                        csvData.Supply              = commodity.Value<Int32>("stock");
                        csvData.SampleDate          = DateTime.Now;

                        if(commodity.Value<Int32>("demandBracket") > 0)
                            csvData.DemandLevel         = (String)Program.Data.BaseTableIDToName("economylevel", commodity.Value<Int32>("demandBracket") - 1, "level");

                        if(commodity.Value<Int32>("stockBracket") > 0)
                            csvData.SupplyLevel         = (String)Program.Data.BaseTableIDToName("economylevel", commodity.Value<Int32>("stockBracket") - 1, "level");

                        csvData.SourceFileName      = "";
                        csvData.DataSource          = "";

                        csvStrings.Add(csvData.ToString());

                        commditCount++;
                    }
                } 

                Program.Data.ImportPricesFromCSVStrings(csvStrings.ToArray(), SQL.EliteDBIO.enImportBehaviour.OnlyNewer, SQL.EliteDBIO.enDataSource.fromIBE);

                return commditCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing prices from companion interface", ex);
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
