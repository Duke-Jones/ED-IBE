using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CodeProject.Dialog;
using Microsoft.Win32;
using Newtonsoft.Json;
using RegulatedNoise.Annotations;
using RegulatedNoise.Enums_and_Utility_Classes;
using RegulatedNoise.Exceptions;

namespace RegulatedNoise
{
	[Serializable]
	public class WindowData
	{
		public WindowData()
		{
			Position.X = -1;
			Position.Y = -1;
			Position.Width = -1;
			Position.Height = -1;

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
			ColumnName = "";
			Width = -1;
			Visible = true;
		}

		public ColumnData(string Name)
		{
			ColumnName = Name;
			Width = -1;
			Visible = true;
		}

		public string ColumnName;
		public Int32 Width;
		public Boolean Visible;
	}


	[Serializable]
	public class RegulatedNoiseSettings : INotifyPropertyChanged
	{
		private const string SETTINGS_FILENAME = "RegulatedNoiseSettings.xml";
		public const string EDDN_OUTPUT_FILEPATH = "EddnOutput.txt";
		public readonly decimal Version = 1.84m;
		public readonly decimal VersionDJ = 0.20m;
		public const string COMMODITIES_LOCALISATION_FILEPATH = "Data/Commodities.xml";

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
		private bool _startListeningEddnOnLoad = false;
		private string _userName = "";
		public bool IncludeExtendedCSVInfo = true;
		public bool PostToEddnOnImport = false;
		public bool DeleteScreenshotOnImport = false;
		public bool UseEddnTestSchema = false;
		public string UiColour = "#FF8419";
		public string ForegroundColour = null;
		public string BackgroundColour = null;
		public bool AutoImport = false;
		public bool TestMode = false;
		public string TraineddataFile = "big";
		public enLanguage Language = enLanguage.eng;
		public int CmdrsLogSortColumn = 0;
		public SortOrder CmdrsLogSortOrder = SortOrder.Descending;
		public bool AutoEvent_JumpedTo = true;
		public bool AutoEvent_Visited = true;
		public bool AutoEvent_MarketDataCollected = true;
		public bool AutoEvent_ReplaceVisited = true;
		public float EBPixelThreshold = 0.6f;
		public int EBPixelAmount = 22;
		public int lastStationCount = 4;
		public bool lastStationCountActive = false;
		public bool limitLightYears = false;
		public int lastLightYears = 25;
		public bool StationToStar = false;
		public int lastStationToStar = 500;
		public int CBSortingSelection = 1;
		public bool MaxRouteDistance = false;
		public int lastMaxRouteDistance = 20;
		public bool PerLightYearRoundTrip = false;
		private decimal _lastVersion = 0.00m;
		private decimal _lastVersionDj = 0.00m;
		public int GUIColorCutoffLevel = 150;
		public bool AutoActivateOCRTab = true;
		public bool AutoActivateSystemTab = true;
		public string PilotsName = String.Empty;
		public bool IncludeUnknownDTS = true;
		public bool LoadStationsJSON = false;
		public Int32 OldDataPurgeDeadlineDays = 14;
		private bool _checkedTestEddnSetting = false;

		public readonly SerializableDictionary<string, WindowData> WindowBaseData;

		public readonly SerializableDictionary<string, List<ColumnData>> ListViewColumnData;

		public event PropertyChangedEventHandler PropertyChanged;

		private static XmlSerializer _serializer;

		public RegulatedNoiseSettings()
		{
			WindowBaseData = new SerializableDictionary<string, WindowData>() { 
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
			ListViewColumnData = new SerializableDictionary<string, List<ColumnData>>() { 
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
		}

		private static XmlSerializer Serializer
		{
			get
			{
				if (_serializer == null)
				{
					_serializer = new XmlSerializer(typeof(RegulatedNoiseSettings));
				}
				return _serializer;
			}
		}

		public bool StartListeningEddnOnLoad
		{
			get { return _startListeningEddnOnLoad; }
			set
			{
				if (!Equals(_startListeningEddnOnLoad, value))
				{
					_startListeningEddnOnLoad = value;
					RaisePropertyChanged();
				}
			}
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				if (value == _userName) return;
				_userName = value;
				RaisePropertyChanged();
			}
		}

		public void CheckVersion2()
		{
			string sURL;
			sURL = @"https://api.github.com/repos/Duke-Jones/RegulatedNoise/releases";

			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(sURL);
			webRequest.Method = "GET";
			webRequest.ServicePoint.Expect100Continue = false;
			webRequest.UserAgent = "YourAppName";

			decimal maxVersion = -1;
			decimal maxVersionDJ = -1;

			try
			{
				string response;
				using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
					response = responseReader.ReadToEnd();

				dynamic data = JsonConvert.DeserializeObject<dynamic>(response);

				var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
				ci.NumberFormat.CurrencyDecimalSeparator = ".";

				dynamic releaseDetails = null;

				foreach (var x in data)
				{
					string release = x.tag_name;
					var prerelease = (bool)x.prerelease;

					if (prerelease == false)
					{
						release = release.Replace("v", "");
						var versions = release.Split('_');

						var mainVersion = Decimal.Parse(versions[0], NumberStyles.Any, ci);

						decimal DJVersion;
						if (versions.GetUpperBound(0) > 0)
							DJVersion = Decimal.Parse(versions[1], NumberStyles.Any, ci);
						else
							DJVersion = Decimal.Parse("0.00", NumberStyles.Any, ci);

						if (maxVersion < mainVersion)
						{
							maxVersion = mainVersion;
							maxVersionDJ = DJVersion;
							releaseDetails = x;
						}
						else if ((maxVersion == mainVersion) && (maxVersionDJ < DJVersion))
						{
							maxVersion = mainVersion;
							maxVersionDJ = DJVersion;
							releaseDetails = x;
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
		private bool IsFirstVersionRun()
		{
			bool retValue = (_lastVersion < Version) || ((_lastVersion == Version) && (_lastVersionDj < VersionDJ));
			_lastVersion = Version;
			_lastVersionDj = VersionDJ;
			return retValue;
		}

		/// <summary>
		/// returns the UI color as color object
		/// </summary>
		/// <returns></returns>
		public Color GetUiColor()
		{
			return Color.FromArgb(Int32.Parse(UiColour.Substring(1, 2), NumberStyles.HexNumber),
												  Int32.Parse(UiColour.Substring(3, 2), NumberStyles.HexNumber),
												  Int32.Parse(UiColour.Substring(5, 2), NumberStyles.HexNumber));
		}

		/// <summary>
		/// returns the UI color as color object
		/// </summary>
		/// <returns></returns>
		public Color GetForegroundColor()
		{
			return Color.FromArgb(Int32.Parse(ForegroundColour.Substring(1, 2), NumberStyles.HexNumber),
												  Int32.Parse(ForegroundColour.Substring(3, 2), NumberStyles.HexNumber),
												  Int32.Parse(ForegroundColour.Substring(5, 2), NumberStyles.HexNumber));
		}

		/// <summary>
		/// returns the UI color as color object
		/// </summary>
		/// <returns></returns>
		public Color GetBackgroundColor()
		{
			return Color.FromArgb(Int32.Parse(BackgroundColour.Substring(1, 2), NumberStyles.HexNumber),
												  Int32.Parse(BackgroundColour.Substring(3, 2), NumberStyles.HexNumber),
												  Int32.Parse(BackgroundColour.Substring(5, 2), NumberStyles.HexNumber));
		}

		private void ExtraCheck()
		{
			if (IsFirstVersionRun())
			{
				// do all the things that must be done for the new versions
				if ((Version == 1.84m) && (VersionDJ == 0.09m))
				{
					// this value works much better
					EBPixelThreshold = 0.6f;
					EBPixelAmount = 22;
				}

				// do all the things that must be done for the new versions
				if ((Version == 1.84m) && (VersionDJ == 0.17m))
				{
					if (UseEddnTestSchema)
					{
						UseEddnTestSchema = false;
						Save();
						if (PostToEddnOnImport)
						{
							EventBus.Information(@"Set EDDN-mode uniquely to <non-test>-mode.
												 If you know, what you're doing (e.g. you're developer) you can change it back again to <test>-mode", "Changing a mistakable setting");
							//MsgBox.Show("Set EDDN-mode uniquely to <non-test>-mode. \n" +
							//					 "If you know, what you're doing (e.g. you're developer) you can change it back again to <test>-mode",
							//					 "Changing a mistakable setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				}


				if (!_checkedTestEddnSetting)
				{
					if ((_lastVersion.Equals(1.84m) && _lastVersionDj.Equals(0.17m)))
					{
						// last was 0.17 - so we can be sure, we did the check
						_checkedTestEddnSetting = true;
						Save();
					}
					else
					{
						// check did never run yet
						if (UseEddnTestSchema)
						{
							UseEddnTestSchema = false;
							Save();
							if (PostToEddnOnImport)
							{
								EventBus.Information(@"Set EDDN-mode uniquely to <non-test>-mode.
													 If you know, what you're doing (e.g. you're developer) you can change it back again to <test>-mode",
															"Changing a mistakable setting");
							}
						}
						_checkedTestEddnSetting = true;
						Save();
					}
				}
			}
		}

		private void SetProductPath()
		{
			//Already set, no reason to set it again :)
			if (ProductsPath != "" && GamePath != "") return;
			EventBus.InitializationStart("product pathes set");
			//Automatic
			var path = GetProductPathAutomatically();
			//Automatic failed, Ask user to find it manually
			if (path == null)
			{
				var ok = EventBus.Request("Automatic discovery of Frontier directory failed, please point me to your Frontier 'Products' directory.");

				if (!ok)
					throw new InitializationException("unable to find product path");

				path = GetProductPathManually();
			}

			//Verify that path contains FORC-FDEV
			var dirs = Directory.GetDirectories(path);

			var b = false;
			while (!b)
			{
				var gamedirs = new List<string>();
				foreach (var dir in dirs)
				{
					if (Path.GetFileName(dir).StartsWith("FORC-FDEV"))
					{
						gamedirs.Add(dir);
					}
				}

				if (gamedirs.Count > 0)
				{
					//Get highest Forc-fdev dir.
					GamePath = gamedirs.OrderByDescending(x => x).ToArray()[0];
					b = true;
					continue;
				}

				var ok = EventBus.Request("Couldn't find a FORC-FDEV.. directory in the Frontier Products dir, please try again...");

				if (!ok)
					throw new InitializationException("unable to find FORC-DEV subfolder");

				path = GetProductPathManually();
				dirs = Directory.GetDirectories(path);
			}
			ProductsPath = path;
			EventBus.InitializationCompleted("product pathes set");
		}

		private static string GetProductAppDataPathAutomatically()
		{
			string[] autoSearchdir = { Environment.GetEnvironmentVariable("LOCALAPPDATA") };
			return (autoSearchdir.SelectMany(directory => Directory.GetDirectories(directory),
				 (directory, dir) => new { directory, dir })
				 .Where(@t => Path.GetFileName(@t.dir) == "Frontier Developments")
				 .Select(@t => Path.Combine(@t.dir, "Elite Dangerous", "Options"))
				 .Select(p => Directory.Exists(p) ? p : null)).FirstOrDefault();
		}

		private static string GetProductAppDataPathManually()
		{
			while (true)
			{
				string filePath = EventBus.FileRequest(@"Please point me to the Game Options directory, typically C:\Users\{username}\AppData\{Local or Roaming}\Frontier Developments\Elite Dangerous\Options\Graphics");

				if (filePath != null)
				{
					if (Path.GetFileName(filePath) == "Options")
					{
						return filePath;
					}
				}

				var ok = EventBus.Request(
					  "Hm, that doesn't seem right, " + filePath +
					  " is not the Game Options directory, Please try again", "");

				if (!ok)
					throw new InitializationException("Elite Dangerous Appdata not provided");

			}
		}

		private void SetProductAppDataPath()
		{
			//Already set, no reason to set it again :)
			if (ProductAppData != "") return;
			EventBus.InitializationStart("product appdata set");
			//Automatic
			var path = GetProductAppDataPathAutomatically();

			//Automatic failed, Ask user to find it manually
			if (path == null)
			{
				var ok = EventBus.Request(@"Automatic discovery of the Game Options directory failed, please point me to it...");

				if (!ok)
					Application.Exit();

				path = GetProductAppDataPathManually();
			}
			ProductAppData = path;
			EventBus.InitializationCompleted("product appdata set");
		}

		private static string GetProductPathAutomatically()
		{
			string[] autoSearchdir = { Environment.GetEnvironmentVariable("ProgramW6432"), 
                                       Environment.GetEnvironmentVariable("PROGRAMFILES(X86)") };

			string returnValue = null;
			foreach (var directory in autoSearchdir)
			{
				if (directory == null) continue;
				foreach (var dir in Directory.GetDirectories(directory))
				{
					if (Path.GetFileName(dir) != "Frontier") continue;
					var p = Path.Combine(dir, "EDLaunch", "Products");
					returnValue = Directory.Exists(p) ? p : null;
					break;
				}
			}
			if (returnValue != null)
			{
				return returnValue;
			}

			var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Frontier_Developments\Products\";
			if (Directory.Exists(path))
			{
				return path;
			}

			// nothing found ? then lets have a try with the MUICache
			const string programName = "Elite:Dangerous Executable";
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache");
			if (key != null)
			{
				string[] names = key.GetValueNames();
				for (int i = 0; i < names.Count(); i++)
				{
					if (key.GetValue(names[i]).ToString() == programName)
					{
						string programPath = names[i];
						var lastIndexOf = programPath.LastIndexOf("\\Products\\");
						if (lastIndexOf != -1)
						{
							programPath = programPath.Substring(0, lastIndexOf + 9);
							if (Directory.Exists(programPath))
							{
								return programPath;
							}
						}
					}
				}
			}
			return null;
		}

		private static string GetProductPathManually()
		{
			while (true)
			{
				var filepath = EventBus.FileRequest("Please point me to your Frontier 'Products' directory.");

				if (Path.GetFileName(filepath) == "Products")
				{
					return filepath;
				}

				var ok = EventBus.Request(
					  "Hm, that doesn't seem right" +
					  (filepath != "" ? ", " + filepath + " isn't the Frontier 'Products' directory" : "")
				+ ". Please try again...");

				if (!ok)
					throw new InitializationException("unable to find Elite 'Products' folder");
			}
		}

		public static RegulatedNoiseSettings LoadSettings()
		{
			EventBus.InitializationStart("load settings");
			RegulatedNoiseSettings settings;

			if (File.Exists(SETTINGS_FILENAME))
			{
				using (var fs = new FileStream(SETTINGS_FILENAME, FileMode.Open))
				{
					var reader = XmlReader.Create(fs);
					try
					{
						settings = (RegulatedNoiseSettings)Serializer.Deserialize(reader);
					}
					catch (Exception ex)
					{
						Trace.TraceError("Error loading settings: " + ex);
						EventBus.InitializationProgress("Couldn't load settings; maybe they are from a previous version.  A new settings file will be created on exit.");
						settings = new RegulatedNoiseSettings();
					}
				}
			}
			else
			{
				settings = new RegulatedNoiseSettings();
			}
			EventBus.InitializationCompleted("load settings");
			settings.ExtraCheck();
#if(!NO_PATH_INIT)
			settings.SetProductPath();
			settings.SetProductAppDataPath();
#endif
			return settings;
		}

		public void Save()
		{
			var newFile = String.Format("{0}_new{1}", Path.GetFileNameWithoutExtension(SETTINGS_FILENAME), Path.GetExtension(SETTINGS_FILENAME));
			var backupFile = String.Format("{0}_bak{1}", Path.GetFileNameWithoutExtension(SETTINGS_FILENAME), Path.GetExtension(SETTINGS_FILENAME));

			using (var stream = new FileStream(newFile, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				Serializer.Serialize(stream, this);
			}

			// we delete the current file not until the new file is written without errors
			// delete old backup
			if (File.Exists(backupFile))
			{
				File.Delete(backupFile);
			}
			// rename current file to old backup
			if (File.Exists(SETTINGS_FILENAME))
			{
				File.Move(SETTINGS_FILENAME, backupFile);
			}
			// rename new file to current file
			File.Move(newFile, SETTINGS_FILENAME);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
				try
				{
					handler(this, new PropertyChangedEventArgs(propertyName));
				}
				catch (Exception ex)
				{
					Trace.TraceError(propertyName + " notification failed " + ex);
				}
		}
	}

	public partial class Form1
	{
		private void cbAutoImport_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationContext.RegulatedNoiseSettings.AutoImport = cbAutoImport.Checked;
		}

		private void cbStartWebserverOnLoad_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationContext.RegulatedNoiseSettings.StartWebserverOnLoad = cbStartWebserverOnLoad.Checked;
		}

		private void cbStartOCROnLoad_CheckedChanged(object sender, EventArgs e)
		{
			if (cbStartOCROnLoad.Checked && ApplicationContext.RegulatedNoiseSettings.MostRecentOCRFolder == "")
			{
				MessageBox.Show("You need to pick a directory first, using the Monitor Directory button.  Once you've done that, you can enable Start OCR On Load.");
				ApplicationContext.RegulatedNoiseSettings.StartOCROnLoad = false;
				cbStartOCROnLoad.Checked = false;
			}
			else
			{
				ApplicationContext.RegulatedNoiseSettings.StartOCROnLoad = cbStartOCROnLoad.Checked;
			}
		}

		private void tbUsername_TextChanged(object sender, EventArgs e)
		{
			ApplicationContext.RegulatedNoiseSettings.UserName = tbUsername.Text;
		}

		private void cbExtendedInfoInCSV_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationContext.RegulatedNoiseSettings.IncludeExtendedCSVInfo = cbExtendedInfoInCSV.Checked;
		}

		private void cbPostOnImport_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationContext.RegulatedNoiseSettings.PostToEddnOnImport = cbPostOnImport.Checked;
		}

		private void cbDeleteScreenshotOnImport_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationContext.RegulatedNoiseSettings.DeleteScreenshotOnImport = cbDeleteScreenshotOnImport.Checked;
		}

		private void cbUseEddnTestSchema_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationContext.RegulatedNoiseSettings.UseEddnTestSchema = cbUseEddnTestSchema.Checked;
		}

		#region Theming
		private void pbForegroundColour_Click(object sender, EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			if (c.ShowDialog() == DialogResult.OK)
			{
				ApplicationContext.RegulatedNoiseSettings.ForegroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
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
				ApplicationContext.RegulatedNoiseSettings.BackgroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
															 c.Color.B.ToString("X2");
				ShowSelectedUiColours();
				Retheme();
			}
		}

		private void ShowSelectedUiColours()
		{
			if (pbForegroundColour.Image != null) pbForegroundColour.Image.Dispose();
			if (ApplicationContext.RegulatedNoiseSettings.ForegroundColour != null)
			{
				ForegroundSet.Visible = false;
				Bitmap b = new Bitmap(32, 32);
				int red = int.Parse(ApplicationContext.RegulatedNoiseSettings.ForegroundColour.Substring(1, 2),
					  NumberStyles.HexNumber);
				int green = int.Parse(ApplicationContext.RegulatedNoiseSettings.ForegroundColour.Substring(3, 2),
					  NumberStyles.HexNumber);
				int blue = int.Parse(ApplicationContext.RegulatedNoiseSettings.ForegroundColour.Substring(5, 2),
					  NumberStyles.HexNumber);

				using (var g = Graphics.FromImage(b))
				{
					g.Clear(Color.FromArgb(red, green, blue));
				}
				pbForegroundColour.Image = b;
			}
			else ForegroundSet.Visible = true;

			if (ApplicationContext.RegulatedNoiseSettings.BackgroundColour != null)
			{
				BackgroundSet.Visible = false;
				if (pbBackgroundColour.Image != null) pbBackgroundColour.Image.Dispose();
				Bitmap b = new Bitmap(32, 32);
				int red = int.Parse(ApplicationContext.RegulatedNoiseSettings.BackgroundColour.Substring(1, 2),
					  NumberStyles.HexNumber);
				int green = int.Parse(ApplicationContext.RegulatedNoiseSettings.BackgroundColour.Substring(3, 2),
					  NumberStyles.HexNumber);
				int blue = int.Parse(ApplicationContext.RegulatedNoiseSettings.BackgroundColour.Substring(5, 2),
					  NumberStyles.HexNumber);
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
			ApplicationContext.RegulatedNoiseSettings.ForegroundColour = null;
			ApplicationContext.RegulatedNoiseSettings.BackgroundColour = null;
		}

		private void ForegroundSet_Click(object sender, EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			if (c.ShowDialog() == DialogResult.OK)
			{
				ApplicationContext.RegulatedNoiseSettings.ForegroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
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
				ApplicationContext.RegulatedNoiseSettings.BackgroundColour = "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
															 c.Color.B.ToString("X2");
				ShowSelectedUiColours();
				Retheme();
			}
		}
		#endregion
	}
}
