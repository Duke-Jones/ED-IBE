using System;

namespace RegulatedNoise
{
    [Serializable]
    public class RegulatedNoiseSettings
    {
        public string ProductsPath = "";
        public string WebserverIpAddress = "";
        public bool StartWebserverOnLoad = false;
        public string WebserverBackgroundColor = "#FFFFFF";
        public string WebserverForegroundColor = "#000000";
        public string MostRecentOCRFolder = "";
        public bool StartOCROnLoad = false;
        public string UserName = "";
        public bool IncludeExtendedCSVInfo = true;
        public bool PostToEddnOnImport = false;
        public bool DeleteScreenshotOnImport = false;
        public bool WarnedAboutEddnSchema = false;
        public bool UseEddnTestSchema = true;
        public string UiColour = "#FF8419";
        public string ForegroundColour = null;
        public string BackgroundColour = null;
    }
}
