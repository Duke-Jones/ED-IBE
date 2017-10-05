using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Security;
using EDCompanionAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EDCompanionAPI
{
    public sealed class EliteCompanion
    {
        private static volatile EliteCompanion _Instance;
        private static object syncRoot = new Object();
        private static MemoryCache _Cache = MemoryCache.Default;
        private static HttpHelper _Http;
        private static Profile _CurrentProfile;
        private static string _DataPath;
        private static PerformanceTimer _sWatch = new PerformanceTimer();

        public string DataPath
        {
            get
            {
                return _DataPath;
            }
            set
            {
                _DataPath = value;
            }
        }
        
        private EliteCompanion()
        {
            _DataPath = "./";
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[Constants.APPSETTING_DATAPATH]))
            {
                _DataPath = ConfigurationManager.AppSettings[Constants.APPSETTING_DATAPATH];
                if (_DataPath.StartsWith("/") || _DataPath.StartsWith("~"))
                {
                    _DataPath = HostingEnvironment.MapPath(_DataPath);
                }
            }
        }

        #region Singleton
        public static EliteCompanion Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_Instance == null)
                            _Instance = new EliteCompanion();
                    }
                }
                return _Instance;
            }
        }
        #endregion

        #region Profiles
        /// <summary>
        /// Creates a user profile, which is stored on disk. Password will be encrypted using machine key. Created profile will be set as active profile.
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <param name="password">Password of user</param>
        public void CreateProfile(string email, string password)
        {
            Profile profile = new Profile();
            profile.Email = email.ToLower();
            //Encrypt password
            var passwordData = Encoding.UTF8.GetBytes(password);
            var encryptedData = MachineKey.Protect(passwordData);
            profile.Password = Convert.ToBase64String(encryptedData);
            profile.Save();
            _Cache.Remove(Constants.CACHE_PROFILEJSON);
            _sWatch.stopMeasuring();
            _CurrentProfile = profile;
            _Http = new HttpHelper(_CurrentProfile);
        }

        /// <summary>
        /// Loads a profile and sets it as active.
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <returns>false if profile was not created</returns>
        public bool LoadProfile(string email)
        {
            var profile = Profile.Load(email);
            if (profile == null)
            {
                return false;
            }
            _Cache.Remove(Constants.CACHE_PROFILEJSON);
            _sWatch.stopMeasuring();
            _CurrentProfile = profile;
            _Http = new HttpHelper(_CurrentProfile);
            return true;
        }

        public void DeleteProfile(string email)
        {
            Profile.Delete(email);

            if(_CurrentProfile.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
            { 
                _Cache.Remove(Constants.CACHE_PROFILEJSON);
                _sWatch.stopMeasuring();
                _CurrentProfile = null;;
            }

        }
        #endregion

        #region Login
        /// <summary>
        /// Logins active profile
        /// </summary>
        /// <returns>Login response object</returns>
        public LoginResponse Login()
        {
            return LoginInternal();
        }

        private LoginResponse LoginInternal()
        {
            if (_CurrentProfile.LoggedIn)
            {
                return new LoginResponse {
                    HttpStatusCode = HttpStatusCode.OK,
                    Status = LoginStatus.Ok
                };
            }

            //Get login, to make sure cookies are set
            using (var response = _Http.Get(Constants.URL_LOGIN, null))
            {
                //If we get redirected to "/" we are already authenticated, due to existing cookie.
                if (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    if (response.Headers[HttpResponseHeader.Location] == "/")
                    {
                        _CurrentProfile.LoggedIn = true;
                        return new LoginResponse
                        {
                            HttpStatusCode = response.StatusCode,
                            Status = LoginStatus.Ok
                        };
                    }
                }
                else if (response.StatusCode != HttpStatusCode.OK)
                {
                    //Got some http error
                    return new LoginResponse
                    {
                        HttpStatusCode = response.StatusCode,
                        Status = LoginStatus.UnknownError
                    };
                }
            }

            //Post login
            var loginResponse = new LoginResponse();

            //Decrypt password
            var encryptedData = Convert.FromBase64String(_CurrentProfile.Password);
            var unencryptedData = MachineKey.Unprotect(encryptedData);
            var password = Encoding.UTF8.GetString(unencryptedData);

            var data = new NameValueCollection {
                { "email", _CurrentProfile.Email },
                { "password", password }
            };

            try
            {
                using (var response = _Http.Post(Constants.URL_LOGIN, data, true))
                {
                    loginResponse.HttpStatusCode = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.MovedPermanently)
                    {
                        //Response redirected
                        var redirect = response.Headers[HttpResponseHeader.Location];
                        if (!String.IsNullOrEmpty(redirect))
                        {
                            //Redirect to verification?
                            if (redirect.TrimEnd('/').Equals(Constants.VERIFICATION_REDIRECT_PATH, StringComparison.CurrentCultureIgnoreCase))
                            {
                                loginResponse.Status = LoginStatus.PendingVerification;
                            }
                            //Redirect home?
                            else if (redirect.Equals("/"))
                            {
                                loginResponse.Status = LoginStatus.Ok;
                            }
                        }

                    }
                    else if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //No redirect on login, probably wrong password
                        loginResponse.Status = LoginStatus.IncorrectCredentials;
                    }
                    else
                    {
                        //Other response code
                        loginResponse.Status = LoginStatus.UnknownError;
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex.GetBaseException().Message.Contains("(429)"))
                {
                    loginResponse.Status = LoginStatus.TooManyRequests;
                    loginResponse.HttpStatusCode = (HttpStatusCode)(429);

                }
                else
                {
                    System.Text.RegularExpressions.Regex httpPattern = new System.Text.RegularExpressions.Regex(@"\([0-9]{3}\)");
                    if(httpPattern.IsMatch(ex.GetBaseException().Message))
                    {

                        loginResponse.Status            = LoginStatus.HttpError;

                        Int32 errNumber = 0;
                        if(Int32.TryParse(httpPattern.Match(ex.GetBaseException().Message).Value.Substring(1,3), out errNumber))
                            loginResponse.HttpStatusCode    = (HttpStatusCode)(errNumber);

                    }
                    else
                    {
                        loginResponse.Status = LoginStatus.UnknownError;
                        throw new Exception("Error while logging comapanion IO on to FD servers", ex);
                    }
                }
            }

            if (loginResponse.Status == LoginStatus.Ok)
            {
                _CurrentProfile.LoggedIn = true;
            }

            return loginResponse;
        }
        #endregion

        #region Verification
        /// <summary>
        /// Use to submit verification code, which is sent as an email to the user on first log in or cookie has expired.
        /// </summary>
        /// <param name="verificationCode">The five character code sent to the user</param>
        /// <returns>Verification response object</returns>
        public VerificationResponse SubmitVerification(string verificationCode)
        {
            if (String.IsNullOrEmpty(verificationCode))
                throw new ArgumentNullException("verificationCode");
            return SubmitVerificationInternal(verificationCode);
        }

        private VerificationResponse SubmitVerificationInternal(string verificationCode)
        {
            var data = new NameValueCollection
            {
                { "code", verificationCode }
            };

            var verificationResponse = new VerificationResponse();
            using (var response = _Http.Post(Constants.URL_VERIFICATION, data))
            {
                verificationResponse.HttpStatusCode = response.StatusCode;
                if (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    verificationResponse.Success = true;
                }
                else
                {
                    verificationResponse.Success = false;
                }
            }

            return verificationResponse;
        }
        #endregion

        #region Json Profile
        /// <summary>
        /// Get json profile data for active profile. Will attempt login. It's only possible to get fresh data every 60 seconds
        /// </summary>
        /// <returns>Profile response object</returns>
        public ProfileResponse GetProfileData(bool force = false)
        {
            force |= (RestTime().TotalSeconds <= 0);
            return GetProfileDataInternal(force);
        }

        private ProfileResponse GetProfileDataInternal(bool force = false)
        {
            //We don't want to allow hammering of the API, so we cache response for 60 seconds.
            var profileResponse = new ProfileResponse();
            profileResponse.LoginStatus = LoginStatus.Ok;

            string cachedResponse = _Cache.Get(Constants.CACHE_PROFILEJSON) as string;
            if (!String.IsNullOrEmpty(cachedResponse) && !force)
            {
                profileResponse.Cached = true;
                profileResponse.HttpStatusCode = HttpStatusCode.OK;
                profileResponse.Json = cachedResponse;
                return profileResponse;
            }

            if (!_CurrentProfile.LoggedIn)
            {
                var loginResponse = LoginInternal();
                if (loginResponse.Status != LoginStatus.Ok)
                {
                    profileResponse.LoginStatus = loginResponse.Status;
                    profileResponse.HttpStatusCode = loginResponse.HttpStatusCode;
                    profileResponse.Cached = false;
                    return profileResponse;
                }
            }

            StringBuilder fullJson = new StringBuilder("{\"profile\" : **PH1** " +
                                                       "," +
                                                       "\"market\"   : **PH2** " +
                                                       "," +
                                                       "\"shipyard\" : **PH3** " +
                                                       "}");

            using (var response = _Http.Get(Constants.URL_BASE + Constants.URL_ADD_PROFILE, null))
            {
                string tempData;
                profileResponse.Cached = false;
                profileResponse.HttpStatusCode = response.StatusCode;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        tempData = sr.ReadToEnd();

                        if(tempData.ToLower().Equals("profile unavailable"))
                        {
                            profileResponse.LoginStatus = LoginStatus.UnknownError;
                            tempData = "{}";
                        }
                    }
                }
                else
                {
                    profileResponse.LoginStatus = LoginStatus.NotAccessible;
                    tempData = "{}";
                }

                fullJson.Replace("**PH1**", tempData);

                if (profileResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    // load more data ?
                    var profileJson = JsonConvert.DeserializeObject<JObject>(tempData);
                    if (profileJson["commander"].Value<Boolean>("docked"))
                    {
                        if (profileJson["lastStarport"]["services"].SelectToken("commodities") != null)
                        {
                            using (var response_md = _Http.Get(Constants.URL_BASE + Constants.URL_ADD_MARKET, null))
                            {
                                string tempData_md;
                                profileResponse.Cached = false;
                                profileResponse.HttpStatusCode = response_md.StatusCode;
                                if (response_md.StatusCode == HttpStatusCode.OK)
                                {
                                    using (StreamReader sr = new StreamReader(response_md.GetResponseStream()))
                                    {
                                        tempData_md = sr.ReadToEnd();

                                        if (tempData_md.ToLower().Equals("profile unavailable"))
                                        {
                                            profileResponse.LoginStatus = LoginStatus.UnknownError;
                                            tempData_md = "{}";
                                        }
                                    }
                                }
                                else
                                {
                                    profileResponse.LoginStatus = LoginStatus.NotAccessible;
                                    tempData_md = "{}";
                                }

                                fullJson.Replace("**PH2**", tempData_md);
                            }
                        }
                        else
                        {
                            fullJson.Replace("**PH2**", "{}");
                        }

                        if((profileJson["lastStarport"]["services"].SelectToken("outfitting") != null) || (profileJson["lastStarport"]["services"].SelectToken("shipyard") != null))
                        {
                            using (var response_sy = _Http.Get(Constants.URL_BASE + Constants.URL_ADD_SHIPYARD, null))
                            {
                                string tempData_sy;
                                profileResponse.Cached = false;
                                profileResponse.HttpStatusCode = response_sy.StatusCode;
                                if (response_sy.StatusCode == HttpStatusCode.OK)
                                {
                                    using (StreamReader sr = new StreamReader(response_sy.GetResponseStream()))
                                    {
                                        tempData_sy = sr.ReadToEnd();

                                        if (tempData_sy.ToLower().Equals("profile unavailable"))
                                        {
                                            profileResponse.LoginStatus = LoginStatus.UnknownError;
                                            tempData_sy = "{}";
                                        }
                                    }
                                }
                                else
                                {
                                    profileResponse.LoginStatus = LoginStatus.NotAccessible;
                                    tempData_sy = "{}";
                                }

                                fullJson.Replace("**PH3**", tempData_sy);
                            }
                        }
                        else
                        {
                            fullJson.Replace("**PH3**", "{}");
                        }
                    }
                }
                else
                {
                    fullJson.Replace("**PH2**", "{}");
                    fullJson.Replace("**PH3**", "{}");
                }
            }

            profileResponse.Json = fullJson.ToString();

            _Cache.Set(Constants.CACHE_PROFILEJSON, profileResponse.Json, DateTimeOffset.Now.AddSeconds(Constants.CACHE_PROFILE_SECONDS));

            _sWatch.Restart();

            return profileResponse;
        }
        #endregion

        /// <summary>
        /// returns the time until cache is no more valid
        /// </summary>
        /// <returns></returns>
        public TimeSpan RestTime()
        { 
            TimeSpan retValue;

            if(_sWatch.isStarted)
            {
                if(_sWatch.currentMeasuring() < (Constants.CACHE_PROFILE_SECONDS * 1000))
                { 
                    retValue = new TimeSpan(0, 0, 0, 0, (Int32)((Constants.CACHE_PROFILE_SECONDS * 1000) - _sWatch.currentMeasuring()));

                    if(retValue.TotalMilliseconds <= 0)
                        retValue = new TimeSpan(0);
                }
                else
                    retValue = new TimeSpan(0);

            }
            else
            {
                retValue = new TimeSpan(0);
            }

            return retValue;
        }

        /// <summary>
        /// sets the cooldowntimer to "finished"
        /// </summary>
        /// <returns></returns>
        public void RestTimeReset()
        { 
            _sWatch.setcurrentMeasuring(Constants.CACHE_PROFILE_SECONDS * 1000);
        }
    }
}