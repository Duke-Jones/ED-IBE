using System;

namespace EDCompanionAPI
{
    public static class Constants
    {
        public const string REQUEST_USER_AGENT = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257";
        public const string URL_LOGIN = "https://companion.orerve.net/user/login";
        public const string URL_VERIFICATION = "https://companion.orerve.net/user/confirm";
        public const string URL_BASE = "https://companion.orerve.net";
        public const string URL_ADD_PROFILE = "/profile";
        public const string URL_ADD_MARKET = "/market";
        public const string URL_ADD_SHIPYARD = "/shipyard";
        public static readonly Uri COOKIE_URI = new Uri("https://companion.orerve.net");
        public const string VERIFICATION_REDIRECT_PATH = "/user/confirm";
        public const string CACHE_PROFILEJSON = "edcompanion.profilejson";
        public const int CACHE_PROFILE_SECONDS = 59;
        public const string APPSETTING_DATAPATH = "edcompanion.datapath";

        public const string RESPONSE_PATTERN = "{\"profile\" : **PH1** " +
                                               "," +
                                               "\"market\"   : **PH2** " +
                                               "," +
                                               "\"shipyard\" : **PH3** " +
                                               "}";

        public const string RESPONSE_EMPTY = "{\"profile\"  : {} " +
                                              "," +
                                              "\"market\"   : {} " +
                                              "," +
                                              "\"shipyard\" : {} " +
                                              "}";
    }
}
