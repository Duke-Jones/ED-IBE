using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    [Serializable]
    public class WindowData
    {
        public WindowData()
        {
            Position.X          = -1;
            Position.Y          = -1;
            Position.Width      = -1;
            Position.Height     = -1;

            State = FormWindowState.Normal;
        }

        public Rectangle Position;
        public FormWindowState State;
    }

    [Serializable]
    public class ColumnData
    {
        public ColumnData()
        { 
            ColumnName          = "";
            Width               = -1;
            Visible             = true;
        }

        public ColumnData(string Name)
        {
            ColumnName          = Name;
            Width               = -1;
            Visible             = true;
        }

        public string ColumnName;
        public Int32 Width;
        public Boolean Visible;
    }


    [Serializable]
    public class RegulatedNoiseSettings
    {
        public readonly decimal Version   = 1.84m;

#if DukeJones

        public readonly decimal VersionDJ = 0.18m;
#endif
        private int _isFirstRun = -1;

        public string ProductsPath = "";
        public string GamePath = ""; //Should Replace ProductsPath by always contain the newest FORC-FDEV dir.
        public string ProductAppData = ""; //2nd location for game configuration files
        public string WebserverIpAddress = "";
        public string WebserverPort = "8080";
        public bool StartWebserverOnLoad = false;
        public string WebserverBackgroundColor = "#FFFFFF";
        public string WebserverForegroundColor = "#000000";
        public string MostRecentOCRFolder = "";
        public bool StartOCROnLoad = false;
        public string UserName = "";
        public bool IncludeExtendedCSVInfo = true;
        public bool PostToEddnOnImport = false;
        public bool DeleteScreenshotOnImport = false;
        public bool UseEddnTestSchema = false;
        public string UiColour = "#FF8419";
        public string ForegroundColour = null;
        public string BackgroundColour = null;
        public bool AutoImport = false;
        public bool TestMode = false;
        public string TraineddataFile                                   = "big";
        public enLanguage Language                                      = enLanguage.eng;
        public int CmdrsLogSortColumn                                   = 0;
        public SortOrder CmdrsLogSortOrder                              = SortOrder.Descending;
        public bool AutoEvent_JumpedTo                                  = true;
        public bool AutoEvent_Visited                                   = true;
        public bool AutoEvent_MarketDataCollected                       = true;
        public bool AutoEvent_ReplaceVisited                            = true;
        public float EBPixelThreshold                                   = 0.6f;
        public int EBPixelAmount                                        = 22;
        public int lastStationCount                                     = 4;
        public bool lastStationCountActive                              = false;
        public bool limitLightYears                                     = false;
        public int lastLightYears                                       = 25;
        public bool StationToStar                                       = false;
        public int lastStationToStar                                    = 500;
        public int CBSortingSelection                                   = 1;
        public bool MaxRouteDistance                                     = false;
        public int lastMaxRouteDistance                                  = 20;
        public bool PerLightYearRoundTrip                               = false;
        public decimal lastVersion                                      = 0.00m;
        public decimal lastVersionDJ                                    = 0.00m;
        public int GUIColorCutoffLevel                                  = 150;
        public bool AutoActivateOCRTab                                  = true;
        public bool AutoActivateSystemTab                               = true;
        public string PilotsName                                        = String.Empty;
        public bool IncludeUnknownDTS                                   = true;
        public bool LoadStationsJSON                                    = false;
        public Int32 oldDataPurgeDeadlineDays                           = 14;   
        public bool checkedTestEDDNSetting                              = false;   

        public SerializableDictionary<string, WindowData> WindowBaseData = new SerializableDictionary<string, WindowData>() { 
                                                                                                                  {"Form1",                 new WindowData()},
                                                                                                                  {"EditOcrResults",        new WindowData()},
                                                                                                                  {"EditPriceData",         new WindowData()},
                                                                                                                  {"EDStationView",         new WindowData()},
                                                                                                                  {"EDCommodityView",       new WindowData()},
                                                                                                                  {"EDCommodityListView",   new WindowData()},
                                                                                                                  {"FilterTest",            new WindowData()},
                                                                                                                  {"HelpOCR",               new WindowData()},
                                                                                                                  {"HelpCommodities",       new WindowData()},
                                                                                                                  {"EBPixeltest",           new WindowData()},
                                                                                                                  {"ProgressView",          new WindowData()}
                                                                                                                };

        public SerializableDictionary<string, List<ColumnData>> ListViewColumnData = new SerializableDictionary<string, List<ColumnData>>() { 
                                                                                                                  {"lvCommandersLog",       new List<ColumnData>() { 
                                                                                                                                                     new ColumnData("EventDate"), 
                                                                                                                                                     new ColumnData("EventType"), 
                                                                                                                                                     new ColumnData("Station"), 
                                                                                                                                                     new ColumnData("System"), 
                                                                                                                                                     new ColumnData("Cargo"), 
                                                                                                                                                     new ColumnData("CargoAction"), 
                                                                                                                                                     new ColumnData("CargoVolume"), 
                                                                                                                                                     new ColumnData("Notes"), 
                                                                                                                                                     new ColumnData("EventID"), 
                                                                                                                                                     new ColumnData("TransactionAmount"), 
                                                                                                                                                     new ColumnData("Credits") }},
                                                                                                                  {"lvAllComms",            new List<ColumnData>() { new ColumnData("") }},
                                                                                                                  {"lbPrices",              new List<ColumnData>() { new ColumnData("") }}
                                                                                                                };

        public void CheckVersion2()
        {
            string sURL;
            sURL = @"https://api.github.com/repos/Duke-Jones/RegulatedNoise/releases";
            string response;

            HttpWebRequest webRequest = System.Net.WebRequest.Create(sURL) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "YourAppName";
                
            decimal maxVersion = -1;
            decimal maxVersionDJ = -1;

            try
            {
                string[] Versions;
                decimal MainVersion;
                decimal DJVersion;
                string release;
                bool PR;

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    response = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject<dynamic>(response);

                var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                dynamic releaseDetails = null;

                foreach (var x in data)
                {
                    release = x.tag_name;
                    PR      = (bool)x.prerelease;

                    if (PR == false)
                    {
                        release = release.Replace("v", "");
                        Versions = release.Split('_');

                        MainVersion = decimal.Parse(Versions[0], NumberStyles.Any, ci);

                        if (Versions.GetUpperBound(0) > 0)
                            DJVersion = decimal.Parse(Versions[1], NumberStyles.Any, ci);
                        else
                            DJVersion = decimal.Parse("0.00", NumberStyles.Any, ci);

                        if (maxVersion < MainVersion)
                        {
                            maxVersion      = MainVersion;
                            maxVersionDJ    = DJVersion;
                            releaseDetails = x;
                        }
                        else if ((maxVersion == MainVersion) && (maxVersionDJ < DJVersion))
                        {
                            maxVersion      = MainVersion;
                            maxVersionDJ    = DJVersion;
                            releaseDetails  = x;
                        }
                    }
                }

                if ((Version < maxVersion) || ((Version == maxVersion) && (VersionDJ < maxVersionDJ)))
                {
                
                    Form1.InstanceObject.lblUpdateInfo.Text = "newer DJ-version found!";
                    Form1.InstanceObject.lblUpdateInfo.ForeColor = Color.Black;
                    Form1.InstanceObject.lblUpdateInfo.BackColor = Color.Yellow;

                    Form1.InstanceObject.lblUpdateDetail.Text = maxVersion.ToString().Replace(",", ".") + "-" + maxVersionDJ.ToString().Replace(",", ".") + ":\r\n";

                    Form1.InstanceObject.lblUpdateDetail.Text += releaseDetails.body;

                }
                else
                { 
                    Form1.InstanceObject.lblUpdateInfo.Text = "you have the latest version of RegulatedNoise";
                    Form1.InstanceObject.lblUpdateInfo.ForeColor = Color.DarkGreen;

                    Form1.InstanceObject.lblUpdateDetail.Text = maxVersion.ToString().Replace(",", ".") + "-" + maxVersionDJ.ToString().Replace(",", ".") + ":\r\n";
                    Form1.InstanceObject.lblUpdateDetail.Text += releaseDetails.body;
                }
            }
            catch
            {
                // Not a disaster if we can't do the version check...
                return;
            }

        }

        /// <summary>
        /// checks if this is the first time of this version running
        /// </summary>
        /// <returns></returns>
        public bool isFirstVersionRun()
        {
            bool retValue = false;

            if (_isFirstRun == -1)
            { 
                if ((lastVersion < Version) || ((lastVersion == Version) && (lastVersionDJ < VersionDJ)))
                { 
                    retValue = true;
                }

                lastVersion     = Version;
                lastVersionDJ   = VersionDJ;

            }
            else
            { 
                if (_isFirstRun == 0)
                    retValue = false;
                else
                    retValue = true;
            }

            return retValue;
        }

        /// <summary>
        /// returns the UI color as color object
        /// </summary>
        /// <returns></returns>
        public Color getUiColor()
        {
            return Color.FromArgb(int.Parse(UiColour.Substring(1, 2), System.Globalization.NumberStyles.HexNumber), 
                                  int.Parse(UiColour.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                                  int.Parse(UiColour.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
        }

        /// <summary>
        /// returns the UI color as color object
        /// </summary>
        /// <returns></returns>
        public Color getForegroundColor()
        {
            return Color.FromArgb(int.Parse(ForegroundColour.Substring(1, 2), System.Globalization.NumberStyles.HexNumber), 
                                  int.Parse(ForegroundColour.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                                  int.Parse(ForegroundColour.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
        }

            /// <summary>
        /// returns the UI color as color object
        /// </summary>
        /// <returns></returns>
        public Color getBackgroundColor()
        {
            return Color.FromArgb(int.Parse(BackgroundColour.Substring(1, 2), System.Globalization.NumberStyles.HexNumber), 
                                  int.Parse(BackgroundColour.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                                  int.Parse(BackgroundColour.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
        }

}

    public partial class Form1
    {
        private void cbAutoImport_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.AutoImport = cbAutoImport.Checked;
        }

        private void cbStartWebserverOnLoad_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.StartWebserverOnLoad = cbStartWebserverOnLoad.Checked;
        }

        private void cbStartOCROnLoad_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStartOCROnLoad.Checked && RegulatedNoiseSettings.MostRecentOCRFolder == "")
            {
                MessageBox.Show("You need to pick a directory first, using the Monitor Directory button.  Once you've done that, you can enable Start OCR On Load.");
                RegulatedNoiseSettings.StartOCROnLoad = false;
                cbStartOCROnLoad.Checked = false;
            }
            else
            {
                RegulatedNoiseSettings.StartOCROnLoad = cbStartOCROnLoad.Checked;
            }
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.UserName = tbUsername.Text;
        }

        private void cbExtendedInfoInCSV_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.IncludeExtendedCSVInfo = cbExtendedInfoInCSV.Checked;
        }

        private void cbPostOnImport_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.PostToEddnOnImport = cbPostOnImport.Checked;
        }


        private void cbDeleteScreenshotOnImport_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.DeleteScreenshotOnImport = cbDeleteScreenshotOnImport.Checked;
        }

        private void cbUseEddnTestSchema_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.UseEddnTestSchema = cbUseEddnTestSchema.Checked;
        }

        #region Theming
        private void pbForegroundColour_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                RegulatedNoiseSettings.ForegroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                                          c.Color.B.ToString("X2");

                ShowSelectedUiColours();
                Retheme();
            }

        }

        private void pbBackgroundColour_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                RegulatedNoiseSettings.BackgroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                          c.Color.B.ToString("X2");
                ShowSelectedUiColours();
                Retheme();
            }
        }

        private void ShowSelectedUiColours()
        {
            if (pbForegroundColour.Image != null) pbForegroundColour.Image.Dispose();
            if (RegulatedNoiseSettings.ForegroundColour != null)
            {
                ForegroundSet.Visible = false;
                Bitmap b = new Bitmap(32, 32);
                int red = int.Parse(RegulatedNoiseSettings.ForegroundColour.Substring(1, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int green = int.Parse(RegulatedNoiseSettings.ForegroundColour.Substring(3, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int blue = int.Parse(RegulatedNoiseSettings.ForegroundColour.Substring(5, 2),
                    System.Globalization.NumberStyles.HexNumber);

                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.FromArgb(red, green, blue));
                }
                pbForegroundColour.Image = b;
            }
            else ForegroundSet.Visible = true;

            if (RegulatedNoiseSettings.BackgroundColour != null)
            {
                BackgroundSet.Visible = false;
                if (pbBackgroundColour.Image != null) pbBackgroundColour.Image.Dispose();
                Bitmap b = new Bitmap(32, 32);
                int red = int.Parse(RegulatedNoiseSettings.BackgroundColour.Substring(1, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int green = int.Parse(RegulatedNoiseSettings.BackgroundColour.Substring(3, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int blue = int.Parse(RegulatedNoiseSettings.BackgroundColour.Substring(5, 2),
                    System.Globalization.NumberStyles.HexNumber);
                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.FromArgb(red, green, blue));
                }
                pbBackgroundColour.Image = b;
            }
            else BackgroundSet.Visible = true;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.ForegroundColour = null;
            RegulatedNoiseSettings.BackgroundColour = null;
        }

        private void ForegroundSet_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                RegulatedNoiseSettings.ForegroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                                          c.Color.B.ToString("X2");

                ShowSelectedUiColours();
                Retheme();
            }
        }

        private void BackgroundSet_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                RegulatedNoiseSettings.BackgroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                          c.Color.B.ToString("X2");
                ShowSelectedUiColours();
                Retheme();
            }
        }
        #endregion
    }
}
