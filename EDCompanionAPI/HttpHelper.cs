using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Web;
using System.Text.RegularExpressions;
using EDCompanionAPI.Models;

namespace EDCompanionAPI
{
    internal class HttpHelper
    {
        private Profile _CurrentProfile;

        public HttpHelper(Profile profile)
        {
            _CurrentProfile = profile;
            if (profile.Cookies == null)
            {
                profile.Cookies = new CookieContainer();
            }
        }

        public HttpWebResponse Get(string url, NameValueCollection query)
        {
            if (query != null && query.Count > 0)
            {
                var strQuery = String.Join("&", query.AllKeys.Select(a => a + "=" + HttpUtility.UrlEncode(query[a])));
                url += "?" + strQuery;
            }

            var request = GetRequest(url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            UpdateCookies(response);
            return response;
        }

        public HttpWebResponse Post(string url, NameValueCollection data = null, bool urlAsReferrer = false)
        {
            var request = GetRequest(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if (urlAsReferrer)
            {
                request.Referer = url;
            }
            if (data != null && data.Count > 0)
            {
                var strData = String.Join("&", data.AllKeys.Select(a => a + "=" + HttpUtility.UrlEncode(data[a])));
                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(strData);
                }
            }

            var response = (HttpWebResponse)request.GetResponse();
            UpdateCookies(response);
            return response;
        }

        private HttpWebRequest GetRequest(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Accept = "text/html, application/json";
            request.AllowAutoRedirect = false;
            request.UserAgent = Constants.REQUEST_USER_AGENT;
            request.CookieContainer = _CurrentProfile.Cookies;
            return request;
        }

        //Multiple cookies are sent as comma-separated in a single set-cookie header
        private void UpdateCookies(HttpWebResponse response)
        {
            bool cookiesTouched = false;
            for (int i = 0; i < response.Headers.Count; i++)
            {
                string name = response.Headers.GetKey(i);
                if (!name.Equals("set-cookie", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                string cookieData = response.Headers.Get(i);
                if (!String.IsNullOrEmpty(cookieData))
                {
                    cookiesTouched = true;
                    cookieData += ";";
                    //Replace data formats since they contain ','
                    cookieData = Regex.Replace(cookieData, @"expires=[a-zA-Z]{2,10}\,([^;,]+[;,]{1})", "expires=$1", RegexOptions.IgnoreCase);
                    var cookies = cookieData.Split(',');
                    foreach (var strCookie in cookies)
                    {
                        var parts = strCookie.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        var nameVal = parts[0].Split('=');

                        //Update existing or create new cookie
                        Cookie cookie = new Cookie(nameVal[0], nameVal[1]);
                        foreach (var part in parts.Skip(1))
                        {
                            var keyval = part.Split('=');
                            switch (keyval[0].Trim().ToLower())
                            {
                                case "domain":
                                    cookie.Domain = keyval[1].Trim();
                                    break;
                                case "path":
                                    cookie.Path = keyval[1].Trim();
                                    break;
                                case "secure":
                                    cookie.Secure = true;
                                    break;
                                case "expires":
                                    var date = DateTime.ParseExact(keyval[1].Replace("UTC", "").Trim(), "d-MMM-yyyy HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AssumeUniversal);
                                    cookie.Expires = date.ToUniversalTime();
                                    break;
                            }
                        }

                        if (String.IsNullOrEmpty(cookie.Domain))
                        {
                            cookie.Domain = Constants.COOKIE_URI.Host;
                        }

                        _CurrentProfile.Cookies.Add(cookie);
                    }
                }
            }

            if (cookiesTouched)
            {
                _CurrentProfile.Save();
            }
        }
    }
}
