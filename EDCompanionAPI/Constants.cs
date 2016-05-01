using System;

namespace EDCompanionAPI
{
    internal static class Constants
    {
        public static readonly string REQUEST_USER_AGENT = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257";
        public static readonly string URL_LOGIN = "https://companion.orerve.net/user/login";
        public static readonly string URL_VERIFICATION = "https://companion.orerve.net/user/confirm";
        public static readonly string URL_PROFILE = "https://companion.orerve.net/profile";
        public static readonly Uri COOKIE_URI = new Uri("https://companion.orerve.net");
        public static readonly string VERIFICATION_REDIRECT_PATH = "/user/confirm";
        public static readonly string CACHE_PROFILEJSON = "edcompanion.profilejson";
        public static readonly int CACHE_PROFILE_SECONDS = 59;
        public static readonly string APPSETTING_DATAPATH = "edcompanion.datapath";
    }
}
