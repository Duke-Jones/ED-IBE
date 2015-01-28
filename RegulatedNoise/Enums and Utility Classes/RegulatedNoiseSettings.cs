using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RegulatedNoise
{
    [Serializable]
    public class RegulatedNoiseSettings
    {
        public readonly decimal Version = 1.84m;

        public string ProductsPath = "";
        public string GamePath = ""; //Should Replace ProductsPath by always contain the newest FORC-FDEV dir.
        public string ProductAppData = ""; //2nd location for game configuration files
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
        public bool AutoImport = false;
        public bool TestMode = false;
        public bool AutoUppercase = true;

        public void CheckVersion()
        {
            string sURL;
            sURL = @"https://api.github.com/repos/stringandstickytape/RegulatedNoise/releases";
            string response;

            HttpWebRequest webRequest = System.Net.WebRequest.Create(sURL) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "YourAppName";

            decimal maxVersion = -1;

            try
            {
                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    response = responseReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject<dynamic>(response);

                var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                dynamic releaseDetails = null;

                foreach (var x in data)
                {
                    string release = x.name;
                    release = release.Replace("v", "");

                    if (release == "")
                        continue;

                    var thisVersion = decimal.Parse(release, NumberStyles.Any, ci);
                    if (maxVersion < thisVersion)
                    {
                        maxVersion = thisVersion;
                        releaseDetails = x;
                    }
                }

                if (Version < maxVersion)
                {
                    var dialogResult = MessageBox.Show("Newer version found! Quit RegulatedNoise and browse to GitHub to download it?\r\n\r\nv"+maxVersion+":\r\n"+releaseDetails.body,"Update?",
                        MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        Process.Start(@"https://github.com/stringandstickytape/RegulatedNoise/releases");
                        Application.Exit();
                    }
                }
            }
            catch
            {
                // Not a disaster if we can't do the version check...
                return;
            }

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
            if (RegulatedNoiseSettings.WarnedAboutEddnSchema == false)
            {
                var result = MessageBox.Show(
                    "Are you sure?  It's very important to get your System Names correct before uploading to the live schema...", "Are you sure?",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    RegulatedNoiseSettings.UseEddnTestSchema = cbUseEddnTestSchema.Checked;
                    RegulatedNoiseSettings.WarnedAboutEddnSchema = true;
                }
                else
                {
                    RegulatedNoiseSettings.UseEddnTestSchema = true;
                    cbUseEddnTestSchema.CheckedChanged -= cbUseEddnTestSchema_CheckedChanged;
                    cbUseEddnTestSchema.Checked = true;
                    cbUseEddnTestSchema.CheckedChanged += cbUseEddnTestSchema_CheckedChanged;
                }
            }
            else
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
