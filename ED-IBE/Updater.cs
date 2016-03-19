using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IBE
{
    class Updater
    {

        /// <summary>
        /// Checks if there'currentPriceData a new version available.
        /// If available, "versionFound" holds the new version and "versionInfo" holds the detail information
        /// </summary>
        /// <param name="versionFound"></param>
        /// <param name="versionInfo"></param>
        /// <returns></returns>
        public Boolean CheckVersion(out Version versionFound, out String versionInfo)
        {
            string sURL                 = @"https://api.github.com/repos/Duke-Jones/ED-IBE/releases";
            HttpWebRequest webRequest   = System.Net.WebRequest.Create(sURL) as HttpWebRequest;
            webRequest.Method           = "GET";
            webRequest.UserAgent        = "ED-IBE";
            webRequest.ServicePoint.Expect100Continue = false;
                
            Regex versionCheck          = new Regex("[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}");
            Version newestVersion       = new Version(0,0,0,0);
            Version releaseVersion;
            String releaseData;
            String releaseTag;
            Boolean isPrerRelease;
            Match match;
            Version current;
            Boolean retValue = false;

            versionFound = new Version(0,0,0,0);
            versionInfo  = "";

            try
            {
                current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    releaseData = responseReader.ReadToEnd();

                dynamic data            = JsonConvert.DeserializeObject<dynamic>(releaseData);
                dynamic releaseDetails  = null;

                foreach (var responsePart in data)
                {
                    releaseTag      = responsePart.tag_name;
                    isPrerRelease   = (bool)responsePart.prerelease;

                    if (isPrerRelease == false)
                    {
                        match = versionCheck.Match(releaseTag);
                        
                        if (Version.TryParse(match.Value, out releaseVersion))
                        {
                            if (newestVersion < releaseVersion)
                            {
                                newestVersion  = releaseVersion;
                                releaseDetails = responsePart;
                            }
                        }
                    }
                }

                current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                if (current < newestVersion)
                {
                    versionFound = newestVersion;
                    versionInfo  = releaseDetails.body;

                    retValue = true;
                }

                return retValue;
            }
            catch
            {
                // Not a disaster if we can't do the version check...
                return false;
            }

        }
    }
}
