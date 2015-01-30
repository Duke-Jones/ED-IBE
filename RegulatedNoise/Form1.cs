using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Reflection;
using EdClasses.ClassDefinitions;

namespace RegulatedNoise
{
    public partial class Form1 : Form
    {
        public EDDN Eddn;
        public Random random = new Random();
        public Guid SessionGuid;
        public PropertyInfo[] LogEventProperties;
        public static RegulatedNoiseSettings RegulatedNoiseSettings;
        public CommandersLog CommandersLog;
        public ObjectDirectory StationDirectory = new StationDirectory();
        public ObjectDirectory CommodityDirectory = new CommodityDirectory();
        public Dictionary<string, Tuple<Point3D, List<string>>> SystemLocations = new Dictionary<string, Tuple<Point3D, List<string>>>();
        public List<Station> StationReferenceList = new List<Station>();
        //public Station CurrentStation = null; //Not in use, replaced by EdStation
        public static GameSettings GameSettings;
        public static OcrCalibrator OcrCalibrator;

        private Ocr ocr;
        private ListViewColumnSorter _stationColumnSorter, _commodityColumnSorter, _allCommodityColumnSorter, _stationToStationColumnSorter, _stationToStationReturnColumnSorter, _commandersLogColumnSorter;
        private Thread _eddnSubscriberThread;
        private FileSystemWatcher _fileSystemWatcher;
        private SingleThreadLogger _logger;
        private TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;
        private Levenshtein _levenshtein = new Levenshtein();

        
        //Implementation of the new classlibrary
        public EdSystem CurrentSystem;
//        public EdStation CurrentStation; //Old Station CurrentStation = null; was not in use so i took its name

        [SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
        public Form1()
        {
            _logger = new SingleThreadLogger(ThreadLoggerType.Form);
            _logger.Log("Initialising...");

            LoadSettings();

            _logger.Log("  - settings loaded");

            SetProductPath();

            _logger.Log("  - product path set");
            
            SetProductAppDataPath();

            _logger.Log("  - product appdata set");

            //if (Control.ModifierKeys == Keys.Shift)

            InitializeComponent();

            _logger.Log("  - initialised component");

            GameSettings = new GameSettings();
            
            _logger.Log("  - loaded game settings");

            SetListViewColumnsAndSorters();

            _logger.Log("  - set list views");

            PopulateNetworkInterfaces();

            _logger.Log("  - populated network interfaces");

            ocr = new Ocr(this);

            _logger.Log("  - created OCR object");

            CommandersLog = new CommandersLog(this);

            _logger.Log("  - created Commander's Log object");

            Eddn = new EDDN();

            _logger.Log("  - created EDDN object");

            Application.ApplicationExit += Application_ApplicationExit;

            _logger.Log("  - set application exit handler");

            OcrCalibrator = new OcrCalibrator();
            OcrCalibrator.LoadCalibration();
            var OcrCalibratorTabPage = new TabPage("OCR Calibration");
            var oct = new OcrCalibratorTab { Dock = DockStyle.Fill };
            OcrCalibratorTabPage.Controls.Add(oct);
            tabControl3.Controls.Add(OcrCalibratorTabPage);

            _logger.Log("  - initialised Ocr Calibrator");

            _logger.Log("  - created EDDN object");

            UpdateSystemNameFromLogFile();



            _logger.Log("  - fetched system name from file");

            CommandersLog.LoadLog(true);

            _logger.Log("  - loaded Commander's Log");

            CommandersLog.UpdateCommandersLogListView();

            _logger.Log("  - updated Commander's Log List View");

            ImportSystemLocations();

            _logger.Log("  - system locations imported");

            if (File.Exists("AutoSave.csv"))
            {
                _logger.Log("  - found autosaved CSV");
                var s = new string[1];
                s[0] = "AutoSave.csv";
                ImportListOfCsvs(s);
                _logger.Log("  - imported CSVs");
                SetupGui();
                _logger.Log("  - Updated UI");
            }

            ApplySettings();

            _logger.Log("  - applied settings");

            if (!Directory.Exists(".//OCR Correction Images"))
                Directory.CreateDirectory(".//OCR Correction Images");

            _logger.Log("Initialisation complete");


            if (RegulatedNoiseSettings.TestMode)
            {
                //Testing
                var testtab = new TabPage("MRmP Test Tab");
                var testtb = new MRmPTestTab.MRmPTestTab { Dock = DockStyle.Fill };
                testtab.Controls.Add(testtb);
                tabControl1.Controls.Add(testtab);
            }

            var edl = new EdLogWatcher();
            
            //subscribe to edlogwatcherevents
            edl.ClientArrivedtoNewSystem += (OnClientArrivedtoNewSystem);

            //After event subscriptino we can initialize
            edl.Initialize();
            edl.StartWatcher();
			
			// it's nice to write automatically uppercase
            if (cbAutoUppercase.Checked)
            {
                tbCommoditiesOcrOutput.CharacterCasing = CharacterCasing.Upper;
        }
            else
            {
                tbCommoditiesOcrOutput.CharacterCasing = CharacterCasing.Normal;
            }

        }

        private void OnClientArrivedtoNewSystem(object sender, EdLogLineSystemArgs args)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<EdSystem>(ClientArrivedtoNewSystem), new object[] { args.System });
                return;
            }
            ClientArrivedtoNewSystem(args.System);
            
        }

        private void ClientArrivedtoNewSystem(EdSystem System)
        {
            CurrentSystem = System;
            tbCurrentSystemFromLogs.Text  = System.Name;
            //replace UpdateSystemNameFromLogFile
        }
       

        private void ImportSystemLocations()
        {
            var reader = new StreamReader(File.OpenRead(".//Data//elite.json"));

            var strContent = reader.ReadToEnd();

            strContent = strContent.Substring(12);
            var systems = strContent.Split(new string[] { "{\"system\":\"" }, StringSplitOptions.RemoveEmptyEntries);

            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            var stationCount = 0;

            foreach (var x in systems)
            {
                var stationNames = new List<string>();
                var systemName = x.Substring(0, x.IndexOf("\""));
                var coords = x.Substring(x.IndexOf("coords\":[") + 9);
                coords = coords.Substring(0, coords.IndexOf("]"));
                var individualCoords = coords.Split(',');

                if (x.Contains("stations"))
                {
                    var stationTag = x.Substring(x.IndexOf("stations\":[") + 11);
                    //stationTag = stationTag.Substring(0, stationTag.IndexOf("]"));
                    var stations = stationTag.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                    // Well, I guess we might use this one day...
                    foreach (var y in stations)
                    {
                        stationCount++;
                        var stationNameParse1 = y.Substring(y.IndexOf(":\"") + 2);
                        var stationNameParse2 = stationNameParse1.Substring(0, stationNameParse1.IndexOf("\""));
                        stationNames.Add(stationNameParse2);
                    }
                }


                    if (!SystemLocations.ContainsKey(systemName.ToUpper()))
                        SystemLocations.Add(systemName.ToUpper(),
                            new Tuple<Point3D, List<string>>(
                                new Point3D(float.Parse(individualCoords[0], NumberStyles.Any, ci),
                                    float.Parse(individualCoords[1], NumberStyles.Any, ci),
                                    float.Parse(individualCoords[2], NumberStyles.Any, ci)), stationNames));
            }

            Debug.WriteLine(SystemLocations.Count + " systems, "+stationCount+" stations...");

            var csvReader = new StreamReader(File.OpenRead(".//Data//station.csv"));

            while (!csvReader.EndOfStream)
            {
                var csvLine = csvReader.ReadLine();
                var values = csvLine.Split(',');
                values[0] = values[0].Substring(1, values[0].Length - 2);
                values[1] = values[1].Substring(1, values[1].Length - 2);
                values[3] = values[3].Substring(1, values[3].Length - 2);
                values[4] = values[4].Substring(1, values[4].Length - 2);

                long lightYearsFromStar;
                long.TryParse(values[2], out lightYearsFromStar);

                StationHasBlackMarket stationHasBlackMarket;
                switch (values[3])
                {
                    case "N":
                        stationHasBlackMarket = StationHasBlackMarket.No;
                        break;
                    case "Y":
                        stationHasBlackMarket = StationHasBlackMarket.Yes;
                        break;
                    default:
                        stationHasBlackMarket = StationHasBlackMarket.Unknown;
                        break;
                }

                StationPadSize stationPadSize;
                switch (values[4])
                {
                    case "M":
                        stationPadSize = StationPadSize.Medium;
                        break;
                    case "L":
                        stationPadSize = StationPadSize.Large;
                        break;
                    default:
                        stationPadSize = StationPadSize.Unknown;
                        break;
                }

                StationReferenceList.Add(new Station
                    {
                        System = values[0],
                        Name = values[1],
                        LightSecondsFromStar =  lightYearsFromStar,
                        StationHasBlackMarket =  stationHasBlackMarket,
                        StationPadSize = stationPadSize
                    });
                
            }
        }

        private void SetListViewColumnsAndSorters()
        {
            lbPrices.Columns.Add("Commodity Name").Width = 150;
            lbPrices.Columns.Add("Sell Price");
            lbPrices.Columns.Add("Buy Price");
            lbPrices.Columns.Add("Demand");
            lbPrices.Columns.Add("Demand Level");
            lbPrices.Columns.Add("Supply");
            lbPrices.Columns.Add("Supply Level");
            lbPrices.Columns.Add("Best Buy").Width = 200;
            lbPrices.Columns.Add("Best Sell").Width = 200;
            lbPrices.Columns.Add("Difference").Width = 70;
            lbPrices.Columns.Add("Sample Date").Width = 150;
            lbPrices.Columns.Add("Source (Double-click to launch)").Width = 300;

            lbCommodities.Columns.Add("Station Name").Width = 150;
            lbCommodities.Columns.Add("Sell Price");
            lbCommodities.Columns.Add("Buy Price");
            lbCommodities.Columns.Add("Demand");
            lbCommodities.Columns.Add("Demand Level");
            lbCommodities.Columns.Add("Supply");
            lbCommodities.Columns.Add("Supply Level");
            lbCommodities.Columns.Add("Sample Date").Width = 150;

            lvAllComms.Columns.Add("Commodity Name").Width = 150;
            lvAllComms.Columns.Add("Best Buy Price");
            lvAllComms.Columns.Add("Best Buy Detail").Width = 150;
            lvAllComms.Columns.Add("Buy Locations");
            lvAllComms.Columns.Add("Best Sell Price");
            lvAllComms.Columns.Add("Best Sell Detail").Width = 150;
            lvAllComms.Columns.Add("Sell Locations");
            lvAllComms.Columns.Add("Difference").Width = 70;

            AddColumnsToStationToStationListView(lvStationToStation);
            AddColumnsToStationToStationListView(lvStationToStationReturn);

            var c = new ColumnHeader("EventDate") { Text = "EventDate", Name = "EventDate", ImageKey = "EventDate" };
            lvCommandersLog.Columns.Add(c);

            LogEventProperties = typeof(CommandersLogEvent).GetProperties();
            foreach (var property in LogEventProperties)
            {
                if (property.Name != "EventDate")
                {
                    c = new ColumnHeader(property.Name) { Name = property.Name, Text = property.Name, ImageKey = property.Name };
                    lvCommandersLog.Columns.Add(c);
                }
            }
            lvCommandersLog.Columns[0].Width = 150;
            lvCommandersLog.Columns[1].Width = 200;

            // Create an instance of a ListView column sorter and assign it 
            // to the ListView control.
            _stationColumnSorter = new ListViewColumnSorter(0);
            _commodityColumnSorter = new ListViewColumnSorter(1);
            _allCommodityColumnSorter = new ListViewColumnSorter(2);
            _stationToStationColumnSorter = new ListViewColumnSorter(3);
            _stationToStationReturnColumnSorter = new ListViewColumnSorter(3);
            _commandersLogColumnSorter = new ListViewColumnSorter(4);

            lbPrices.ListViewItemSorter = _stationColumnSorter;
            lbCommodities.ListViewItemSorter = _commodityColumnSorter;
            lvAllComms.ListViewItemSorter = _allCommodityColumnSorter;
            lvStationToStation.ListViewItemSorter = _stationToStationColumnSorter;
            lvStationToStationReturn.ListViewItemSorter = _stationToStationReturnColumnSorter;
            lvCommandersLog.ListViewItemSorter = _commandersLogColumnSorter;
        }

        private static void AddColumnsToStationToStationListView(ListView listView)
        {
            listView.Columns.Add("Commodity Name").Width = 150;
            listView.Columns.Add("Sell Price");
            listView.Columns.Add("Supply");
            listView.Columns.Add("Supply Level");
            listView.Columns.Add("Buy Price");
            listView.Columns.Add("Demand");
            listView.Columns.Add("Demand Level");
            listView.Columns.Add("Difference").Width = 70;
        }

        private string getProductPathAutomatically()
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
            if (returnValue != null) return returnValue;

            if(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Frontier_Developments\Products\"))
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Frontier_Developments\Products\";

            return null;
        }
        private string getProductPathManually()
        {
            var dialog = new FolderBrowserDialog { Description = "Please point me to your Frontier 'Products' directory." };

            while (true)
            {
                var dialogResult = dialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    if (Path.GetFileName(dialog.SelectedPath) == "Products")
                    {
                        return dialog.SelectedPath;
                        
                    }
                }

                MessageBox.Show(
                    "Hm, that doesn't seem right" +
                    (dialog.SelectedPath != "" ? ", " + dialog.SelectedPath + " isn't the Frontier 'Products' directory"  : "")
                + ". Please try again...", "", MessageBoxButtons.OK);
            }
        }
        private void SetProductPath()
        {
            //Already set, no reason to set it again :)
            if (RegulatedNoiseSettings.ProductsPath != "" && RegulatedNoiseSettings.GamePath != "") return;
            
            //Automatic
            var path = getProductPathAutomatically();

            //Automatic failed, Ask user to find it manually
            if (path == null)
            {
                MessageBox.Show("Automatic discovery of Frontier directory failed, please point me to your Frontier 'Products' directory.");
                path = getProductPathManually();

            }

            //Verify that path contains FORC-FDEV
            var dirs = Directory.GetDirectories(path);
                
            var b = false;
            while(!b)
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
                    RegulatedNoiseSettings.GamePath = gamedirs.OrderByDescending(x => x).ToArray()[0];
                    b = true;
                    continue;
                }
                
                MessageBox.Show("Couldn't find a FORC-FDEV.. directory in the Frontier Products dir, please try again...");
                path = getProductPathManually();
                dirs = Directory.GetDirectories(path);
            }

            RegulatedNoiseSettings.ProductsPath = path;
        }
        private string getProductAppDataPathAutomatically()
        {
            string[] autoSearchdir = { Environment.GetEnvironmentVariable("LOCALAPPDATA") };

            return (from directory in autoSearchdir from dir in Directory.GetDirectories(directory) where Path.GetFileName(dir) == "Frontier Developments" select Path.Combine(dir, "Elite Dangerous", "Options") into p select Directory.Exists(p) ? p : null).FirstOrDefault();
        }
        private string getProductAppDataPathManually()
        {
            var dialog = new FolderBrowserDialog { Description = @"Please point me to the Game Options directory, typically C:\Users\{username}\AppData\{Local or Roaming}\Frontier Developments\Elite Dangerous\Options\Graphics" };

            while (true)
            {
                var dialogResult = dialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    if (Path.GetFileName(dialog.SelectedPath) == "Options")
                    {
                        return dialog.SelectedPath;

                    }
                }

                MessageBox.Show(
                    "Hm, that doesn't seem right, " + dialog.SelectedPath +
                    " is not the Game Options directory, Please try again", "", MessageBoxButtons.OK);
            }
        }
        private void SetProductAppDataPath()
        {
            //Already set, no reason to set it again :)
            if (RegulatedNoiseSettings.ProductAppData != "") return;

            //Automatic
            var path = getProductAppDataPathAutomatically();

            //Automatic failed, Ask user to find it manually
            if (path == null)
            {
                MessageBox.Show(@"Automatic discovery of the Game Options directory failed, please point me to it...");
                path = getProductAppDataPathManually();
            }

            RegulatedNoiseSettings.ProductAppData = path;
        }

        private void LoadSettings()
        {
            var serializer = new XmlSerializer(typeof(RegulatedNoiseSettings));

            if (File.Exists("RegulatedNoiseSettings.xml"))
            {
                var fs = new FileStream("RegulatedNoiseSettings.xml", FileMode.Open);
                var reader = XmlReader.Create(fs);

                try
                {
                    RegulatedNoiseSettings = (RegulatedNoiseSettings)serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    _logger.Log("Error loading settings", true);
                    _logger.Log(ex.ToString(), true);
                    _logger.Log(ex.Message, true);
                    _logger.Log(ex.StackTrace, true);
                    if (ex.InnerException != null)
                        _logger.Log(ex.InnerException.ToString(), true);
                    MessageBox.Show("Couldn't load settings; maybe they are from a previous version.  A new settings file will be created on exit.");
                    RegulatedNoiseSettings = new RegulatedNoiseSettings();
                }
                fs.Close();
            }
            else RegulatedNoiseSettings = new RegulatedNoiseSettings();
        }


        private void ApplySettings()
        {
            if (RegulatedNoiseSettings.WebserverForegroundColor != "") tbForegroundColour.Text = RegulatedNoiseSettings.WebserverForegroundColor;
            if (RegulatedNoiseSettings.WebserverBackgroundColor != "") tbBackgroundColour.Text = RegulatedNoiseSettings.WebserverBackgroundColor;
            if (RegulatedNoiseSettings.WebserverIpAddress != "") cbInterfaces.Text = RegulatedNoiseSettings.WebserverIpAddress;
            cbAutoImport.Checked = RegulatedNoiseSettings.AutoImport;

			// it's nice to write automatically uppercase
            cbAutoUppercase.Checked = RegulatedNoiseSettings.AutoUppercase;

            RegulatedNoiseSettings.AutoImport = cbAutoImport.Checked;

            ShowSelectedUiColours();
            cbExtendedInfoInCSV.Checked = RegulatedNoiseSettings.IncludeExtendedCSVInfo;
            cbDeleteScreenshotOnImport.Checked = RegulatedNoiseSettings.DeleteScreenshotOnImport;
            cbUseEddnTestSchema.Checked = RegulatedNoiseSettings.UseEddnTestSchema;
            cbPostOnImport.Checked = RegulatedNoiseSettings.PostToEddnOnImport;

            if (RegulatedNoiseSettings.UserName != "")
                tbUsername.Text = RegulatedNoiseSettings.UserName;
            else
                tbUsername.Text = Guid.NewGuid().ToString();
            if (RegulatedNoiseSettings.StartWebserverOnLoad)
            {
                cbStartWebserverOnLoad.Checked = true;
                bStart_Click(null, null);
            }
            if (RegulatedNoiseSettings.StartOCROnLoad && RegulatedNoiseSettings.MostRecentOCRFolder != "")
            {
                cbStartOCROnLoad.Checked = true;
                //ocr.StartMonitoring(RegulatedNoiseSettings.MostRecentOCRFolder);

                if (_fileSystemWatcher == null)
                    _fileSystemWatcher = new FileSystemWatcher();

                _fileSystemWatcher.Path = RegulatedNoiseSettings.MostRecentOCRFolder;

                _fileSystemWatcher.Filter = "*.bmp";

                _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess |
                             NotifyFilters.LastWrite |
                             NotifyFilters.FileName |
                             NotifyFilters.DirectoryName;

                _fileSystemWatcher.IncludeSubdirectories = false;

                _fileSystemWatcher.Created += ScreenshotCreated;

                _fileSystemWatcher.EnableRaisingEvents = true;

                ocr.IsMonitoring = true;
            }


        }

        private Thread _ocrThread;
        private List<string> _preOcrBuffer = new List<string>();
        private System.Threading.Timer _preOcrBufferTimer;
        private void ScreenshotCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            Thread.Sleep(1000);

            while (!File.Exists(fileSystemEventArgs.FullPath))
            {
                //MessageBox.Show("File created... but it doesn't exist?!  Hit OK and I'll retry...");
                Thread.Sleep(100);
            }

            //MessageBox.Show("Good news! " + fileSystemEventArgs.FullPath +
            //                " exists!  Let's pause for a moment before opening it...");

            ScreenshotsQueued("(" + (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) + " queued)");
			// if the textfield support auto-uppercase we must consider
            string s = CommoditiesText("").ToString().ToUpper();

            if (s == "Imported!".ToUpper() || s == "Finished!".ToUpper() || s == "" || s == "No rows found...".ToUpper())
                CommoditiesText("Working...");



            if (_ocrThread == null || !_ocrThread.IsAlive)
            {
				// some stateful enabling for the buttons
                bClearOcrOutput.Enabled = false;
                bEditResults.Enabled = false;

                _ocrThread = new Thread(() => ocr.ScreenshotCreated(fileSystemEventArgs.FullPath, tbCurrentSystemFromLogs.Text));
                _ocrThread.Start();
            }
            else
            {
                _preOcrBuffer.Add(fileSystemEventArgs.FullPath);

                if (_preOcrBufferTimer == null)
                {
                    var autoEvent = new AutoResetEvent(false);
                    _preOcrBufferTimer = new System.Threading.Timer(CheckOcrBuffer, autoEvent, 1000, 1000);
                }
            }
        }

        private void CheckOcrBuffer(object sender)
        {
            if (!_ocrThread.IsAlive)
            {
                if (_preOcrBuffer.Count > 0)
                {
    				// some stateful enabling for the buttons
                    bClearOcrOutput.Enabled = false;
                    bEditResults.Enabled = false;

                    var s = _preOcrBuffer[0];
                    _preOcrBuffer.RemoveAt(0);
                    _ocrThread = new Thread(() => ocr.ScreenshotCreated(s, tbCurrentSystemFromLogs.Text));
                    _ocrThread.Start();
                    ScreenshotsQueued("(" +
                                      (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) +
                                      " queued)");
                }
            }
        }

        public delegate string CommoditiesTextDelegate(string s);

        public string CommoditiesText(string s)
        {
            if (InvokeRequired)
            {
                return (string)(Invoke(new CommoditiesTextDelegate(CommoditiesText), s));
            }

            if (s != "")
                tbCommoditiesOcrOutput.Text = s;

            return tbCommoditiesOcrOutput.Text;
        }



        public delegate void ScreenshotsQueuedDelegate(string s);

        public void ScreenshotsQueued(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new ScreenshotsQueuedDelegate(ScreenshotsQueued), s);
                return;
            }

            lblScreenshotsQueued.Text = s;
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (stateTimer != null) stateTimer.Dispose();
            if (_eddnSubscriberThread != null) _eddnSubscriberThread.Abort();

            SaveCommodityData(true);
            CommandersLog.SaveLog(true);

            SaveSettings();

            if (sws.Running)
                sws.Stop();
        }

        public void SaveSettings()
        {
            var fileName = "RegulatedNoiseSettings.xml";
            if (!File.Exists(fileName))
                File.Delete(fileName);

            var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            var x = new XmlSerializer(RegulatedNoiseSettings.GetType());
            x.Serialize(stream, RegulatedNoiseSettings);
            stream.Close();
        }



        public class CsvRow
        {
            public string SystemName;
            public string StationName;
            public string CommodityName;
            public decimal SellPrice;
            public decimal BuyPrice;
            public decimal Cargo;
            public decimal Demand;
            public string DemandLevel;
            public decimal Supply;
            public string SupplyLevel;
            public DateTime SampleDate;
            public string SourceFileName;

            public override string ToString()
            {
                return SystemName + ";" +
                            StationName.Replace(" [" + SystemName + "]", "") + ";" +
                            CommodityName + ";" +
                            (SellPrice != 0 ? SellPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                            (BuyPrice != 0 ? BuyPrice.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                            (Demand != 0 ? Demand.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                            DemandLevel + ";" +
                            (Supply != 0 ? Supply.ToString(CultureInfo.InvariantCulture) : "") + ";" +
                            SupplyLevel + ";" +
                            SampleDate.ToString("s").Substring(0, 16) + ";" +
                            SourceFileName;
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            SaveCommodityData();
        }

        private void SaveCommodityData(bool force = false)
        {
            SaveFileDialog saveFile = new SaveFileDialog();

            if (force)
                saveFile.FileName = "AutoSave.csv";
            else
                saveFile.FileName = "Unified EliteOCR Data " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".csv";

            saveFile.InitialDirectory = "\\.";
            saveFile.DefaultExt = "csv";
            saveFile.Filter = "CSV (*.csv)|*.csv";

            if (force || saveFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(saveFile.FileName))
                    File.Delete(saveFile.FileName);

                var writer = new StreamWriter(File.OpenWrite(saveFile.FileName));
                writer.WriteLine("System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;");

                foreach (var station in StationDirectory)
                {
                    foreach (var commodity in station.Value)
                    {
                        var output = string.Join(";", new[]
                        {
                            commodity.SystemName,
                            commodity.StationName.Replace(" [" + commodity.SystemName + "]", ""),
                            commodity.CommodityName,
                            commodity.SellPrice != 0 ? commodity.SellPrice.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.BuyPrice != 0 ? commodity.BuyPrice.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.Demand != 0 ? commodity.Demand.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.DemandLevel,
                            commodity.Supply != 0 ? commodity.Supply.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.SupplyLevel,
                            commodity.SampleDate.ToString("s").Substring(0, 16),
                            cbExtendedInfoInCSV.Checked ? commodity.SourceFileName : ""
                        });

                        if (cbExtendedInfoInCSV.Checked)
                            writer.WriteLine(output + ";");
                    }
                }
                writer.Close();
            }
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "csv";
            openFile.Multiselect = true;
            openFile.Filter = "CSV (*.csv)|*.csv";
            openFile.ShowDialog();

            if (openFile.FileNames.Length > 0)
            {
                var filenames = openFile.FileNames;

                ImportListOfCsvs(filenames);
            }

            SetupGui();

        }

        private void ImportListOfCsvs(string[] filenames)
        {
            foreach (string filename in filenames)
            {
                var reader = new StreamReader(File.OpenRead(filename));

                string header = reader.ReadLine();

                if (header != null && !header.StartsWith("System;"))
                {
                    MessageBox.Show("Error: " + filename + " is unreadable or in an old format.  Skipping...");
                    continue;
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    ImportCsvString(line);
                }
                reader.Close();
            }
        }

        private void ImportCsvString(string line, bool suspendDuplicateChecking = false, bool postToEddn = false)
        {
            var values = line.Split(';');

            CsvRow currentRow = new CsvRow();

            currentRow.SystemName = values[0];
            currentRow.StationName = _textInfo.ToTitleCase(values[1].ToLower()) + " [" + currentRow.SystemName + "]";
            currentRow.CommodityName = _textInfo.ToTitleCase(values[2].ToLower());
            Decimal.TryParse(values[3], out currentRow.SellPrice);
            Decimal.TryParse(values[4], out currentRow.BuyPrice);
            Decimal.TryParse(values[5], out currentRow.Demand);
            currentRow.DemandLevel = _textInfo.ToTitleCase(values[6].ToLower());
            Decimal.TryParse(values[7], out currentRow.Supply);
            currentRow.SupplyLevel = _textInfo.ToTitleCase(values[8].ToLower());
            DateTime.TryParse(values[9], out currentRow.SampleDate);

            #region Extended CSV Information
            if (values.GetLength(0) > 10)
            {
                currentRow.SourceFileName = values[10];
            }
            #endregion

            if (currentRow.CommodityName != "")
            {
                if (!StationDirectory.ContainsKey(currentRow.StationName))
                    StationDirectory.Add(currentRow.StationName, new List<CsvRow>());

                if (!suspendDuplicateChecking)
                {
                    var obsoleteData =
                        StationDirectory[currentRow.StationName].Where(
                            x =>
                                x.StationName == currentRow.StationName && x.CommodityName == currentRow.CommodityName &&
                                x.SampleDate <= currentRow.SampleDate).ToList();

                    foreach (var x in obsoleteData)
                    {
                        StationDirectory[currentRow.StationName].Remove(x);
                        CommodityDirectory[currentRow.CommodityName].Remove(x);
                    }
                }

                if (suspendDuplicateChecking || StationDirectory[currentRow.StationName].Count(x => x.StationName == currentRow.StationName && x.CommodityName == currentRow.CommodityName && x.SampleDate == currentRow.SampleDate) == 0)
                {
                    StationDirectory[currentRow.StationName].Add(currentRow);

                    if (!CommodityDirectory.ContainsKey(currentRow.CommodityName))
                        CommodityDirectory.Add(currentRow.CommodityName, new List<CsvRow>());

                    CommodityDirectory[currentRow.CommodityName].Add(currentRow);
                }

                if (postToEddn && cbPostOnImport.Checked && currentRow.SystemName != "SomeSystem")
                    PostJsonToEddn(currentRow);
            }
        }

        private Point3D _cachedSystemLocation;
        private string _cachedSystemName;
        private Dictionary<string, double> _cachedRemoteSystemDistances;

        private bool Distance(string remoteSystemName)
        {
            try
            {
                double dist;

                if (cbLightYears.Text == "")
                    return false;

                dist = DistanceInLightYears(remoteSystemName);

                var limit = float.Parse(cbLightYears.Text);
                if (dist < limit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private double DistanceInLightYears(string remoteSystemName)
        {
            double dist;

            string localSystem;


            localSystem = SystemToMeasureDistancesFrom();

            if (_cachedSystemName != localSystem)
            {
                _cachedRemoteSystemDistances = new Dictionary<string, double>();
                _cachedSystemName = localSystem.ToString();

                if(SystemLocations.ContainsKey(localSystem.ToUpper()))
                    _cachedSystemLocation = SystemLocations[localSystem.ToUpper()].Item1;
                else
                    _cachedSystemLocation = null;
            }


            remoteSystemName = remoteSystemName.ToUpper();


            if (_cachedRemoteSystemDistances.ContainsKey(remoteSystemName))
            {
                dist = _cachedRemoteSystemDistances[remoteSystemName];
            }
            else
            {
                if (!SystemLocations.ContainsKey(remoteSystemName) || _cachedSystemLocation == null)
                {
                    dist = double.MaxValue;
                }
                else
                {
                    var currentSystemLocation = _cachedSystemLocation;
                    dist = DistanceInLightYears(remoteSystemName, currentSystemLocation);
                    _cachedRemoteSystemDistances.Add(remoteSystemName, dist);
                }
            }
            if (remoteSystemName.Contains("LTT"))
                Debug.WriteLine(remoteSystemName + " - " + dist);
            return dist;
        }

        private double DistanceInLightYears(string remoteSystemName, Point3D currentSystemLocation)
        {
            double dist;
            if (!SystemLocations.ContainsKey(remoteSystemName))
                return double.MaxValue;

            var remoteSystemLocation = SystemLocations[remoteSystemName].Item1;

            var xDelta = currentSystemLocation.X - remoteSystemLocation.X;
            var yDelta = currentSystemLocation.Y - remoteSystemLocation.Y;
            var zDelta = currentSystemLocation.Z - remoteSystemLocation.Z;

            dist = Math.Sqrt(Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2) + Math.Pow(zDelta, 2));
            return dist;
        }

        private double DistanceInLightYears(string remoteSystemName, string homeSystemName)
        {
            if (!SystemLocations.ContainsKey(homeSystemName))
                return double.MaxValue;

            return DistanceInLightYears(remoteSystemName, SystemLocations[homeSystemName].Item1);
        }

        private string SystemToMeasureDistancesFrom()
        {
            string localSystem;
            if (cbIncludeWithinRegionOfStation.SelectedItem != null)
                localSystem = cbIncludeWithinRegionOfStation.SelectedItem.ToString() == "<Current System>"
                    ? tbCurrentSystemFromLogs.Text
                    : cbIncludeWithinRegionOfStation.SelectedItem.ToString();
            else
                localSystem = tbCurrentSystemFromLogs.Text;
            return localSystem;
        }

        private string CombinedNameToSystemName(string combinedName)
        {
            var ret = combinedName.Substring(combinedName.IndexOf("[") + 1);
            ret = ret.TrimEnd(']');
            return ret;
        }

        private string CombinedNameToStationName(string combinedName)
        {
            var ret = combinedName.Substring(0, combinedName.IndexOf("[")-1);
            return ret;
        }

        private void SetupGui()
        {
            cbStation.Items.Clear();
            cbCommodity.Items.Clear();
            cbStationToStationFrom.Items.Clear();
            cbStationToStationTo.Items.Clear();

            var a =
                StationDirectory.Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.Key)))
                    .ToList();

            var b = a.OrderBy(x => DistanceInLightYears(CombinedNameToSystemName(x.Key))).ToList();

            foreach (var station in b)
            {
                cbStation.Items.Add(station.Key);
                cbStationToStationFrom.Items.Add(station.Key);
                cbStationToStationTo.Items.Add(station.Key);
            }

            cbIncludeWithinRegionOfStation.SelectedIndexChanged -= cbIncludeWithinRegionOfStation_SelectedIndexChanged;
            var previouslySelectedValue = cbIncludeWithinRegionOfStation.SelectedItem;
            cbIncludeWithinRegionOfStation.Items.Clear();
            var systems = StationDirectory.Keys.Select(x => (object)(CombinedNameToSystemName(x))).OrderBy(x => x).Distinct().ToArray();
            cbIncludeWithinRegionOfStation.Items.Add("<Current System>");
            cbIncludeWithinRegionOfStation.Items.AddRange(systems);
            //cbIncludeWithinRegionOfStation.SelectedIndex = 0;
            cbIncludeWithinRegionOfStation.DropDownStyle = ComboBoxStyle.DropDownList;
            if (previouslySelectedValue != null)
                cbIncludeWithinRegionOfStation.SelectedItem = previouslySelectedValue;
            else
                cbIncludeWithinRegionOfStation.SelectedItem = "<Current System>";
            cbIncludeWithinRegionOfStation.SelectedIndexChanged += cbIncludeWithinRegionOfStation_SelectedIndexChanged;

            cbStation.SelectedItem = null;

            if (cbStation.Items.Count > 0)
                cbStation.SelectedItem = cbStation.Items[0];

            foreach (var commodity in CommodityDirectory.OrderBy(x => x.Key))
            {
                cbCommodity.Items.Add(commodity.Key);
            }

            cbCommodity.SelectedItem = null;

            if (cbCommodity.Items.Count > 0)
                cbCommodity.SelectedItem = cbCommodity.Items[0];

            lvAllComms.Items.Clear();

            // Populate all commodities tab
            foreach (var commodity in CommodityDirectory)
            {
                decimal bestBuyPrice;
                decimal bestSellPrice;
                string bestBuy;
                string bestSell;
                decimal buyers;
                decimal sellers;

                GetBestBuyAndSell(commodity.Key, out bestBuyPrice, out bestSellPrice, out bestBuy, out bestSell, out buyers, out sellers);

                lvAllComms.Items.Add(new ListViewItem(new[] 
                {   commodity.Key,
                    bestBuyPrice.ToString(CultureInfo.InvariantCulture) != "0" ? bestBuyPrice.ToString(CultureInfo.InvariantCulture) : "",
                    bestBuy,
                    buyers.ToString(CultureInfo.InvariantCulture),
                    bestSellPrice.ToString(CultureInfo.InvariantCulture) != "0" ? bestSellPrice.ToString(CultureInfo.InvariantCulture) : "",
                    bestSell,
                    sellers.ToString(CultureInfo.InvariantCulture),
                    bestBuyPrice != 0 && bestSellPrice != 0 ? (bestSellPrice - bestBuyPrice).ToString(CultureInfo.InvariantCulture) : ""
                }));
            }

            UpdateStationToStation();
        }

        private void PopulateNetworkInterfaces()
        {
            // from http://stackoverflow.com/questions/9855230/how-to-get-the-network-interface-and-its-right-ipv4-address
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    Console.WriteLine(ni.Name);
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            cbInterfaces.Items.Add(ip.Address);
                        }
                    }
                }
            }
        }

        private void cbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            var dist = DistanceInLightYears(CombinedNameToSystemName(cbStation.SelectedItem.ToString()));
            if (dist < double.MaxValue)
                lblLightYearsFromCurrentSystem.Text = "(" + String.Format("{0:0.00}", dist) + " light years)";
            else
                lblLightYearsFromCurrentSystem.Text = "(system location unknown)";

            lbPrices.Items.Clear();
            var stationName = (((ComboBox)sender).SelectedItem.ToString());

            var start = stationName.IndexOf("[", StringComparison.Ordinal);
            var end = stationName.IndexOf("]", StringComparison.Ordinal);

            tbStationRename.Text = stationName.Substring(0, start-1);
            tbSystemRename.Text = stationName.Substring(start + 1, end - (start + 1));

            foreach (var row in StationDirectory[stationName])
            {
                decimal bestBuyPrice;
                decimal bestSellPrice;
                string bestBuy;
                string bestSell;
                decimal buyers;
                decimal sellers;

                GetBestBuyAndSell(row.CommodityName, out bestBuyPrice, out bestSellPrice, out bestBuy, out bestSell, out buyers, out sellers);

                ListViewItem newItem = new ListViewItem(new[] 
                {   row.CommodityName, 
                    row.SellPrice.ToString(CultureInfo.InvariantCulture) != "0" ? row.SellPrice.ToString(CultureInfo.InvariantCulture) : "",
                    row.BuyPrice.ToString(CultureInfo.InvariantCulture) != "0" ? row.BuyPrice.ToString(CultureInfo.InvariantCulture) : "",
                    row.Demand.ToString(CultureInfo.InvariantCulture) != "0" ? row.Demand.ToString(CultureInfo.InvariantCulture) : "", 
                    row.DemandLevel,
                    row.Supply.ToString(CultureInfo.InvariantCulture) != "0" ? row.Supply.ToString(CultureInfo.InvariantCulture) : "",
                    row.SupplyLevel, 
                    bestBuy, 
                    bestSell,
                    bestSell!= "" && bestBuy != "" ? (bestSellPrice-bestBuyPrice).ToString(CultureInfo.InvariantCulture) : "",
                    row.SampleDate.ToString(CultureInfo.InvariantCulture) ,
                    row.SourceFileName
                });
                newItem.UseItemStyleForSubItems = false;
                if (bestBuy.Contains(stationName))
                {
                    newItem.SubItems[7].ForeColor = Color.DarkGreen;
                    newItem.SubItems[7].BackColor = Color.LightYellow;
                }

                if (bestSell.Contains(stationName))
                {
                    newItem.SubItems[8].ForeColor = Color.DarkRed;
                    newItem.SubItems[8].BackColor = Color.LightYellow;
                }

                lbPrices.Items.Add(newItem);

            }
        }

        private void GetBestBuyAndSell(string commodityName, out decimal bestBuyPrice, out decimal bestSellPrice, out string bestBuy, out string bestSell, out decimal buyers, out decimal sellers)
        {
            bestBuyPrice = 0;
            bestSellPrice = 0;

            bestBuy = "";
            bestSell = "";


            var l = CommodityDirectory[commodityName].Where(x => x.Supply != 0 && x.BuyPrice != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
            buyers = l.Count();

            if (l.Count() != 0)
            {
                bestBuyPrice = l.Min(y => y.BuyPrice);
                var bestBuyPriceCopy = bestBuyPrice;
                bestBuy = string.Join(" ", l.Where(x => x.BuyPrice == bestBuyPriceCopy).Select(x => x.StationName + " (" + x.BuyPrice + ")"));
            }

            var m = CommodityDirectory[commodityName].Where(x => x.SellPrice != 0 && x.Demand != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
            sellers = m.Count();
            if (m.Count() != 0)
            {
                bestSellPrice = m.Max(y => y.SellPrice);
                var bestSellPriceCopy = bestSellPrice;
                bestSell = string.Join(" ", m.Where(x => x.SellPrice == bestSellPriceCopy).Select(x => x.StationName + " (" + x.SellPrice + ")"));
            }
        }

        #region Column Click Handlers
        private void lbPrices_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _stationColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_stationColumnSorter.Order == SortOrder.Ascending)
                {
                    _stationColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _stationColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _stationColumnSorter.SortColumn = e.Column;
                _stationColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lbPrices.Sort();

        }

        private void lvStationToStation_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _stationToStationColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_stationToStationColumnSorter.Order == SortOrder.Ascending)
                {
                    _stationToStationColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _stationToStationColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _stationToStationColumnSorter.SortColumn = e.Column;
                _stationToStationColumnSorter.Order = e.Column == 7 ? SortOrder.Descending : SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvStationToStation.Sort();
        }

        private void lbCommodities_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _commodityColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_commodityColumnSorter.Order == SortOrder.Ascending)
                {
                    _commodityColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _commodityColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _commodityColumnSorter.SortColumn = e.Column;
                _commodityColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lbCommodities.Sort();

        }

        private void lvAllComms_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _allCommodityColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_allCommodityColumnSorter.Order == SortOrder.Ascending)
                {
                    _allCommodityColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _allCommodityColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _allCommodityColumnSorter.SortColumn = e.Column;
                _allCommodityColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvAllComms.Sort();

        }
        #endregion

        private void cbCommodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbCommodities.Items.Clear();
            foreach (var row in CommodityDirectory[(((ComboBox)sender).SelectedItem.ToString())].Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))))
            {
                lbCommodities.Items.Add(new ListViewItem(new[] 
                {   row.StationName,
                    row.SellPrice.ToString(CultureInfo.InvariantCulture) != "0" ? row.SellPrice.ToString(CultureInfo.InvariantCulture) : "",
                    row.BuyPrice.ToString(CultureInfo.InvariantCulture) != "0" ? row.BuyPrice.ToString(CultureInfo.InvariantCulture) : "",
                    row.Demand.ToString(CultureInfo.InvariantCulture) != "0" ? row.Demand.ToString(CultureInfo.InvariantCulture) : "",
                    row.DemandLevel,
                    row.Supply.ToString(CultureInfo.InvariantCulture) != "0" ? row.Supply.ToString(CultureInfo.InvariantCulture) : "",
                    row.SupplyLevel,
                    row.SampleDate.ToString(CultureInfo.InvariantCulture) 
                }));
            }

            var l = CommodityDirectory[(((ComboBox)sender).SelectedItem.ToString())].Where(x => x.BuyPrice != 0 && x.Supply > 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
            if (l.Count() > 0)
            {
                lblMin.Text = l.Min(x => x.BuyPrice).ToString(CultureInfo.InvariantCulture);
                lblMax.Text = l.Max(x => x.BuyPrice).ToString(CultureInfo.InvariantCulture);
                lblAvg.Text = l.Average(x => x.BuyPrice).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                lblMin.Text = "N/A";
                lblMax.Text = "N/A";
                lblAvg.Text = "N/A";
            }

            l = CommodityDirectory[(((ComboBox)sender).SelectedItem.ToString())].Where(x => x.SellPrice != 0 && x.Demand > 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
            if (l.Count() > 0)
            {
                lblMinSell.Text = l.Min(x => x.SellPrice).ToString(CultureInfo.InvariantCulture);
                lblMaxSell.Text = l.Max(x => x.SellPrice).ToString(CultureInfo.InvariantCulture);
                lblAvgSell.Text = l.Average(x => x.SellPrice).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                lblMinSell.Text = "N/A";
                lblMaxSell.Text = "N/A";
                lblAvgSell.Text = "N/A";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lblMin.Text != "N/A")
            {
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.BuyPrice != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
                var m = l.Where(x => x.BuyPrice == l.Min(y => y.BuyPrice));
                MessageBox.Show(string.Join(", ", m.Select(x => x.StationName)));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lblMinSell.Text != "N/A")
            {
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.SellPrice != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
                var m = l.Where(x => x.SellPrice == l.Min(y => y.SellPrice));
                MessageBox.Show(string.Join(", ", m.Select(x => x.StationName)));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lblMax.Text != "N/A")
            {
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.BuyPrice != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
                var m = l.Where(x => x.BuyPrice == l.Max(y => y.BuyPrice));
                MessageBox.Show(string.Join(", ", m.Select(x => x.StationName)));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lblMaxSell.Text != "N/A")
            {
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.SellPrice != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).ToList();
                var m = l.Where(x => x.SellPrice == l.Max(y => y.SellPrice));
                MessageBox.Show(string.Join(", ", m.Select(x => x.StationName)));
            }
        }

        private void lvAllComms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedItems.Count != 1)
                return;

            if (_tooltip != null) _tooltip.RemoveAll();
            if (_tooltip2 != null) _tooltip2.RemoveAll();

            var senderName = ((ListView)sender).SelectedItems[0].Text;

            chart1.ResetAutoValues();

            chart1.Series.Clear();
            var series1 = new Series
            {
                Name = "Series1",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column

            };

            chart1.Series.Add(series1);

            foreach (var price in CommodityDirectory[senderName].Where(x => x.BuyPrice != 0 && x.Supply != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).OrderBy(x => x.BuyPrice))
            {
                series1.Points.AddXY(price.StationName, price.BuyPrice);
            }

            chart1.Invalidate();

            chart2.ResetAutoValues();

            chart2.Series.Clear();
            var series2 = new Series
            {
                Name = "Series1",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column

            };

            chart2.Series.Add(series2);

            foreach (var price in CommodityDirectory[senderName].Where(x => x.SellPrice != 0 && x.Demand != 0).Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.SystemName))).OrderByDescending(x => x.SellPrice))
            {
                series2.Points.AddXY(price.StationName, price.SellPrice);
            }

            chart2.Invalidate();
        }

        #region Tooltip variables
        private Point? _prevPosition;
        private readonly ToolTip _tooltip = new ToolTip();
        private string _currentTooltip = "";
        private DataPoint _oldProp;

        private readonly ToolTip _tooltip2 = new ToolTip();
        #endregion

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (_prevPosition.HasValue && pos == _prevPosition.Value)
                return;

            _prevPosition = pos;
            var window = sender as Chart;
            if (window != null)
            {
                var results = window.HitTest(pos.X, pos.Y, false,
                    ChartElementType.DataPoint);
                foreach (var result in results)
                {
                    if (result.ChartElementType == ChartElementType.DataPoint)
                    {
                        var prop = result.Object as DataPoint;
                        if (prop != null && _currentTooltip != prop.AxisLabel)
                        {
                            if (_oldProp != null) _oldProp.Color = Color.Green;
                            prop.Color = Color.Red;
                            _tooltip.RemoveAll();
                            _tooltip.Show(prop.AxisLabel + " " + prop.YValues[0], window, pos.X + 10, pos.Y - 15);
                            _currentTooltip = prop.AxisLabel;
                            _oldProp = prop;
                        }
                    }
                }
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (_tooltip != null) _tooltip.RemoveAll();
            if (_tooltip2 != null) _tooltip2.RemoveAll();
        }

        private void RenameStation(object sender, EventArgs e)
        {
            var existingStationName = (cbStation.SelectedItem.ToString());

            var newStationName = tbStationRename.Text + " [" + tbSystemRename.Text + "]";

            List<CsvRow> newRows = new List<CsvRow>();

            foreach (var row in StationDirectory[existingStationName])
            {
                CsvRow newRow = new CsvRow
                {
                    BuyPrice = row.BuyPrice,
                    Cargo = row.Cargo,
                    CommodityName = row.CommodityName,
                    Demand = row.Demand,
                    DemandLevel = row.DemandLevel,
                    SampleDate = row.SampleDate,
                    SellPrice = row.SellPrice,
                    StationName = newStationName,
                    Supply = row.Supply,
                    SupplyLevel = row.SupplyLevel,
                    SystemName = tbSystemRename.Text,
                    SourceFileName = row.SourceFileName
                };

                newRows.Add(newRow);
            }


            StationDirectory.Remove(existingStationName);

            if (!StationDirectory.ContainsKey(newStationName))
                StationDirectory.Add(newStationName, newRows);
            else StationDirectory[newStationName].AddRange(newRows);

            var newCommodityDirectory = new CommodityDirectory();

            foreach (var collectionOfRows in CommodityDirectory)
            {
                newRows = new List<CsvRow>();

                foreach (var row in collectionOfRows.Value)
                {

                    CsvRow newRow = new CsvRow
                    {
                        BuyPrice = row.BuyPrice,
                        Cargo = row.Cargo,
                        CommodityName = row.CommodityName,
                        Demand = row.Demand,
                        DemandLevel = row.DemandLevel,
                        SampleDate = row.SampleDate,
                        SellPrice = row.SellPrice,
                        StationName = row.StationName == existingStationName ? newStationName : row.StationName,
                        Supply = row.Supply,
                        SupplyLevel = row.SupplyLevel,
                        SystemName = row.StationName == existingStationName ? tbSystemRename.Text : row.SystemName
                    };

                    newRows.Add(newRow);
                }

                newCommodityDirectory.Add(collectionOfRows.Key, newRows);
            }

            CommodityDirectory = newCommodityDirectory;

            //Remove duplicates (incl. historical)
            var newStationDirectory = new List<CsvRow>();

            var distinctCommodityNames = StationDirectory[newStationName].ToList().Select(x => x.CommodityName).Distinct();

            foreach (var c in distinctCommodityNames)
            {
                newStationDirectory.Add(StationDirectory[newStationName].ToList().Where(x => x.CommodityName == c).OrderByDescending(x => x.SampleDate).First());
            }

            StationDirectory[newStationName] = newStationDirectory;

            var newCommodityDirectory2 = new CommodityDirectory();

            for (int i = 0; i < CommodityDirectory.Keys.Count; i++)
            {
                var distinctStationNames = CommodityDirectory.ElementAt(i).Value.Select(x => x.StationName).Distinct();
                foreach (var d in distinctStationNames)
                {
                    if (!newCommodityDirectory2.Keys.Contains(CommodityDirectory.ElementAt(i).Key))
                        newCommodityDirectory2.Add(CommodityDirectory.ElementAt(i).Key, new List<CsvRow> { CommodityDirectory.ElementAt(i).Value.ToList().Where(x => x.StationName == d).OrderByDescending(x => x.SampleDate).First() });
                    else newCommodityDirectory2[CommodityDirectory.ElementAt(i).Key].Add(CommodityDirectory.ElementAt(i).Value.ToList().Where(x => x.StationName == d).OrderByDescending(x => x.SampleDate).First());
                }
            }

            CommodityDirectory = newCommodityDirectory2;
            SetupGui();
        }

        /// Webserver functionality
        private SimpleWebserver sws = new SimpleWebserver();

        private void bStart_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ws == null)
                //{
                //    ws = new WebServer((new[] { "http://" + cbInterfaces.SelectedItem.ToString() + ":8080/" }), SendResponse);
                //    ws.Start(); //  If the service was already started, the call has no effect
                //    ws.Run();
                //}

                //sws = new  SimpleWebserver(  WebServer((new[] { "http://" + cbInterfaces.SelectedItem.ToString() + ":8080/" }), SendResponse);

                IPAddress ip = IPAddress.Parse(cbInterfaces.SelectedItem.ToString());

                sws.Start(ip, 8080, 5, "", this);
                //                    = new WebServer((new[] { "http://" + cbInterfaces.SelectedItem.ToString() + ":8080/" }), SendResponse);
                UpdateUrl();
            }
            catch (Exception ex)
            {
                _logger.Log("Error starting webserver", true);
                _logger.Log(ex.ToString(), true);
                _logger.Log(ex.Message, true);
                _logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString(), true);
                MessageBox.Show(
                    "Couldn't start webserver.  Maybe something is already using port 8080...?");
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            sws.Stop();
            UpdateUrl();
        }

        public delegate string EventArgsDelegate();

        public string GetLvAllCommsItems()
        {
            if (InvokeRequired)
            {
                return (string)(Invoke(new EventArgsDelegate(GetLvAllCommsItems)));
            }

            if (StationDirectory.Count == 0)
                return "No data loaded :-(";

            var s = new StringBuilder();

            string links = "<BR><A style=\"font-size: 14pt\" HREF=\"#lvAllComms\">All Commodities - </A>" +
                "<A style=\"font-size: 14pt\" HREF=\"#lbPrices\">Station - </A>" +
                "<A style=\"font-size: 14pt\" HREF=\"#lbCommodities\">Commodity - </A>" +
                    "<A style=\"font-size: 14pt\" HREF=\"#lvStationToStation\">Station-to-Station</A><BR>";

            s.Append(links);

            s.Append("<A name=\"lvAllComms\"><P>All Commodities</P>");
            s.Append(GetHTMLForListView(lvAllComms));

            s.Append(links);

            s.Append("<A name=\"lbPrices\"><P>Station: " + cbStation.SelectedItem + "</P>");
            s.Append(GetHTMLForListView(lbPrices));

            s.Append(links);

            s.Append("<A name=\"lbCommodities\"><P>Commodity: " + cbCommodity.SelectedItem + "</P>");
            s.Append(GetHTMLForListView(lbCommodities));

            s.Append(links);

            s.Append("<A name=\"lvStationToStation\"><P>Station-to-Station: " + cbStationToStationFrom.SelectedItem + " => " + cbStationToStationTo.SelectedItem + "</P>");
            s.Append(GetHTMLForListView(lvStationToStation));

            s.Append(links);

            return s.ToString();
        }

        private string GetHTMLForListView(ListView listViewToDump)
        {
            var s = new StringBuilder();

            s.Append("<TABLE style=\"border:1px solid black;border-collapse:collapse;\">");

            var header = new StringBuilder();

            header.Append("<TR>");

            for (int i = 0; i < listViewToDump.Columns.Count; i++)
            {
                var style = "style=\"border:1px solid black; font-weight: bold;\"";
                header.Append("<TD " + style + "><A style=\"color: #" + RegulatedNoiseSettings.WebserverForegroundColor + "\" HREF=\"resortlistview.html?grid=" + listViewToDump.Name + "&col=" + i + "&rand=" + random.Next() + "#" + listViewToDump.Name + "\">" + listViewToDump.Columns[i].Text + "</A></TD>");
            }

            header.Append("</TR>");

            int ctr = 0;

            foreach (ListViewItem item in listViewToDump.Items)
            {
                if (ctr % 7 == 0)
                    s.Append(header);

                s.Append("<TR>");

                for (int i = 0; i < item.SubItems.Count; i++)
                    s.Append("<TD style=\"border:1px solid black;\">" + item.SubItems[i].Text + "</TD>");

                s.Append("</TR>");

                ctr++;
            }

            s.Append(header);

            s.Append("</TABLE>");
            return s.ToString();
        }

        public string SendResponse(HttpListenerRequest request)
        {
            //return string.Format("<HTML><BODY>My web page.<br>{0}<BR>The page you requested was "+request.Url+"</BODY></HTML>", DateTime.Now);

            StringBuilder s = new StringBuilder();

            s.Append("<HTML><BODY><FONT SIZE=\"8\"><FONT FACE=\"arial\">");

            WriteTables(s);

            s.Append("</BODY></HTML>");

            return s.ToString();
        }

        private void WriteTables(StringBuilder s)
        {
            var items = GetLvAllCommsItems();
            s.Append(items);
        }

        private void cbInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUrl();
        }

        private void UpdateUrl()
        {
            if (cbInterfaces.SelectedItem != null) lblURL.Text = "http://" + cbInterfaces.SelectedItem + ":8080/";
            else lblURL.Text = "Set Interface...";

            if (sws != null && sws.Running) lblURL.ForeColor = Color.Blue;
            else lblURL.ForeColor = Color.Black;

            if (cbInterfaces.SelectedItem != null) RegulatedNoiseSettings.WebserverIpAddress = cbInterfaces.SelectedItem.ToString();
        }

        private void lblURL_Click(object sender, EventArgs e)
        {
            if (lblURL.ForeColor == Color.Blue)
                Process.Start(lblURL.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (OcrCalibrator.CalibrationBoxes == null || OcrCalibrator.CalibrationBoxes.Count < 10)
            {
                MessageBox.Show("You need to calibrate first.  Go to the OCR Calibration tab to do so...");
                return;
            }

            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Environment.GetFolderPath((Environment.SpecialFolder.MyPictures)) + @"\Frontier Developments\Elite Dangerous";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                RegulatedNoiseSettings.MostRecentOCRFolder = dialog.SelectedPath;

                if (_fileSystemWatcher == null)
                    _fileSystemWatcher = new FileSystemWatcher();

                _fileSystemWatcher.Path = dialog.SelectedPath;

                _fileSystemWatcher.Filter = "*.bmp";

                _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess |
                             NotifyFilters.LastWrite |
                             NotifyFilters.FileName |
                             NotifyFilters.DirectoryName;

                _fileSystemWatcher.IncludeSubdirectories = false;

                _fileSystemWatcher.Created += ScreenshotCreated;

                _fileSystemWatcher.EnableRaisingEvents = true;

                ocr.IsMonitoring = true;
            }

        }

        #region UpdateOriginalImage
        public delegate Point[] UpdateOriginalImageDelegate(Bitmap b);

        public Point[] UpdateOriginalImage(Bitmap b)
        {
            try
            {
                if (InvokeRequired)
                {
                    return (Point[])(Invoke(new UpdateOriginalImageDelegate(UpdateOriginalImage), b));
                }

                if (pbOriginalImage.Image != null) pbOriginalImage.Image.Dispose();
                pbOriginalImage.Image = b;

                Point[] returnValue = new Point[12];

                int ctr = 0;
                foreach (CalibrationPoint o in Form1.OcrCalibrator.CalibrationBoxes)
                {
                    returnValue[ctr] = ((Point)o.Position);
                    ctr++;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Doh!");
                _logger.Log("Exception in UpdateOriginalImage", true);
                _logger.Log(ex.ToString(), true);
                _logger.Log(ex.Message, true);
                _logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString(), true);
                return new Point[12];
            }
        }
        #endregion

        #region UpdateTrimmedImage
        public delegate void UpdateTrimmedImageDelegate(Bitmap b, Bitmap bHeader);

        public void UpdateTrimmedImage(Bitmap b, Bitmap bHeader)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateTrimmedImageDelegate(UpdateTrimmedImage), b, bHeader);
                return;
            }

            if (pbTrimmed.Image != null) pbTrimmed.Image.Dispose();
            if (pbStation.Image != null) pbStation.Image.Dispose();

            if (b != null)
            {
                pbTrimmed.Image = (Bitmap)(b.Clone());
                pbStation.Image = (Bitmap)(bHeader.Clone());
            }
            else
            {
                pbTrimmed.Image = null;
                pbStation.Image = null;
            }
        }

        public delegate Bitmap ReturnTrimmedImageDelegate();

        public Bitmap ReturnTrimmedImage()
        {
            if (InvokeRequired)
            {
                return (Bitmap)(Invoke(new ReturnTrimmedImageDelegate(ReturnTrimmedImage)));
            }

            if (pbOcrCurrent.Image == null) return null;

            return (Bitmap)(pbOcrCurrent.Image.Clone());
        }

        public delegate bool ReturnOcrMonitoringStatusDelegate();

        public bool ReturnOcrMonitoringStatus()
        {
            if (InvokeRequired)
            {
                return (bool)(Invoke(new ReturnOcrMonitoringStatusDelegate(ReturnOcrMonitoringStatus)));
            }

            return ocr.IsMonitoring;
        }

        public delegate void ImportCurrentOcrDataDelegate();

        public void ImportCurrentOcrData()
        {
            if (InvokeRequired)
            {
                Invoke(new ImportCurrentOcrDataDelegate(ImportCurrentOcrData));
                return;
            }
            ImportFinalOcrOutput();
        }

        public delegate void SetOcrValueFromWebDelegate(string s);

        public void SetOcrValueFromWeb(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new SetOcrValueFromWebDelegate(SetOcrValueFromWeb), s);
                return;
            }

            tbCommoditiesOcrOutput.Text = s;
            bContinueOcr_Click(null, null);
        }

        public delegate void SetStationAndSystemFromWebDelegate(string s, string t);

        public void SetStationAndSystem(string s, string t)
        {
            if (InvokeRequired)
            {
                Invoke(new SetStationAndSystemFromWebDelegate(SetStationAndSystem), s, t);
                return;
            }

            tbOcrStationName.Text = s;
            tbOcrSystemName.Text = t;
        }

        public delegate string[] GetOcrValueForWebDelegate();

        public string[] GetOcrValueForWeb()
        {
            if (InvokeRequired)
            {
                return (string[])(Invoke(new GetOcrValueForWebDelegate(GetOcrValueForWeb)));
            }

            if (pbTrimmed.Image == null)
                return new[] { "<FINISHED>", "<FINISHED>", "<FINISHED>", "" };

            if (_commodityTexts != null && _correctionColumn >= _commodityTexts.GetLength(1))
            {
                var buffered = _screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count;
                if (buffered > 0)
                    return new[] { "Cached" + buffered, "Cached" + buffered, "Cached" + buffered, "" };

                return new[] { "<FINISHED>", "<FINISHED>", "<FINISHED>", "" };
            }

            return new[] { tbCommoditiesOcrOutput.Text, tbOcrStationName.Text, tbOcrSystemName.Text, _correctionColumn.ToString(CultureInfo.InvariantCulture) };
        }
        #endregion

        #region DisplayResults
        public delegate void DisplayResultsDelegate(string s);

        public void DisplayResults(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayResultsDelegate(DisplayResults), s);
                return;
            }

            tbOcrStationName.Text = s; // CLARK HUB

            var systemNames = StationDirectory.Keys.Where(x => x.ToUpper().Contains(s)).ToList();
            if (systemNames.Count == 1) // let's hope so!
            {
                var tempName = systemNames.First();
                var tempName2 = tempName.Substring(tempName.IndexOf("[", StringComparison.Ordinal)).Replace("[", "").Replace("]", "");
                tbOcrSystemName.Text = tempName2;
            }
            else
            {
                UpdateSystemNameFromLogFile();

                if (tbCurrentSystemFromLogs.Text != "")
                    tbOcrSystemName.Text = tbCurrentSystemFromLogs.Text;
                else
                    tbOcrSystemName.Text = "SomeSystem";

            }
        }

        public delegate void DisplayCommodityResultsDelegate(string[,] s, Bitmap[,] originalBitmaps, float[,] originalBitmapConfidences, string[] rowIds, string screenshotName);

        private int _correctionRow, _correctionColumn;

        private string[,] _commodityTexts;
        private Bitmap[,] _originalBitmaps;
        private float[,] _originalBitmapConfidences;
        private string[] _rowIds;
        private string _screenshotName;

        private List<ScreeenshotResults> _screenshotResultsBuffer = new List<ScreeenshotResults>();
        private string _csvOutputSoFar;
        private List<string> _commoditiesSoFar = new List<string>();

        public void DisplayCommodityResults(string[,] s, Bitmap[,] originalBitmaps, float[,] originalBitmapConfidences, string[] rowIds, string screenshotName)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayCommodityResultsDelegate(DisplayCommodityResults), s, originalBitmaps, originalBitmapConfidences, rowIds, screenshotName);
                return;
            }

            if (_commodityTexts != null && _correctionColumn < _commodityTexts.GetLength(1)) // there is an existing screenshot being processed...
            {
                _screenshotResultsBuffer.Add(new ScreeenshotResults { originalBitmapConfidences = originalBitmapConfidences, originalBitmaps = originalBitmaps, s = s, rowIds = rowIds, screenshotName = screenshotName });
                ScreenshotsQueued("(" + (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) + " queued)");
                return;
            }

            if (originalBitmaps.GetLength(0) != 0)
                BeginCorrectingScreenshot(s, originalBitmaps, originalBitmapConfidences, rowIds, screenshotName);
            else
                tbCommoditiesOcrOutput.Text = "No rows found...";
        }

        private void BeginCorrectingScreenshot(string[,] s, Bitmap[,] originalBitmaps, float[,] originalBitmapConfidences, string[] rowIds, string screenshotName)
        {
            _commodityTexts = s;
            _originalBitmaps = originalBitmaps;
            _originalBitmapConfidences = originalBitmapConfidences;
            _screenshotName = screenshotName;
            _rowIds = rowIds;
            _correctionColumn = 0;
            _correctionRow = -1;
            bContinueOcr.Text = "Continue";
            bClearOcrOutput.Enabled = false;
            bEditResults.Enabled = false;
            tbFinalOcrOutput.Enabled = false;
            ContinueDisplayingResults();
        }

        private void ContinueDisplayingResults()
        {
            
            do
            {
                _correctionRow++;
                if (_correctionRow > _commodityTexts.GetLength(0) - 1) { _correctionRow = 0; _correctionColumn++; }

                if (_commodityTexts.GetLength(0) == 0) return;

                if (_correctionColumn < _commodityTexts.GetLength(1))
                {
                    //Debug.WriteLine(correctionRow + " - " + correctionColumn);
                    pbOcrCurrent.Image = _originalBitmaps[_correctionRow, _correctionColumn];
                }

                if (_correctionColumn == 0) // hacks for commodity name
                {
                    var currentTextCamelCase =
                        _textInfo.ToTitleCase(_commodityTexts[_correctionRow, _correctionColumn].ToLower()); // There *was* a reason why I did this...

					
					// if the ocr have found no char so we dont need to ask Mr. Levenshtein 
                    if (currentTextCamelCase.Trim().Length > 0)
                    {
                    if (KnownCommodityNames.Contains(
                        currentTextCamelCase))
                        _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                    else
                    {
                        var replacedCamelCase = StripPunctuationFromScannedText(currentTextCamelCase); // ignore spaces when using levenshtein to find commodity names
                        var lowestLevenshteinNumber = 10000;
                        var nextLowestLevenshteinNumber = 10000;
                        var lowestMatchingCommodity = "";
                        var lowestMatchingCommodityRef = "";
                            double LevenshteinLimit = 0;

                        foreach (var reference in KnownCommodityNames)
                        {
                            var upperRef = StripPunctuationFromScannedText(reference);
                            var levenshteinNumber = _levenshtein.LD2(upperRef, replacedCamelCase);
                            //if(levenshteinNumber != _levenshtein.LD(upperRef, replacedCamelCase))
                            //    Debug.WriteLine("Doh!");

                            if (upperRef != lowestMatchingCommodityRef)
                            {
                                if (levenshteinNumber < lowestLevenshteinNumber)
                                {
                                    nextLowestLevenshteinNumber = lowestLevenshteinNumber;
                                    lowestLevenshteinNumber = levenshteinNumber;
                                    lowestMatchingCommodityRef = upperRef;
                                    lowestMatchingCommodity = reference.ToUpper();
                                }
                                else if (levenshteinNumber < nextLowestLevenshteinNumber)
                                {
                                    nextLowestLevenshteinNumber = levenshteinNumber;
                                }
                            }
                        }

                            // it's better if this depends on the length of the word - this factor works pretty good
                            LevenshteinLimit = Math.Round((currentTextCamelCase.Length * 0.7), 0);

                            if (lowestLevenshteinNumber <= LevenshteinLimit)
                        {
                            _originalBitmapConfidences[_correctionRow, _correctionColumn] = .9f;
                            _commodityTexts[_correctionRow, _correctionColumn] = lowestMatchingCommodity;
                        }

                            if (lowestLevenshteinNumber <= LevenshteinLimit && lowestLevenshteinNumber + 3 < nextLowestLevenshteinNumber) // INDIUM versus INDITE... could factor length in here
                            _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                    }

                    if (_commoditiesSoFar.Contains(_commodityTexts[_correctionRow, _correctionColumn]))
                    {
                        _commodityTexts[_correctionRow, _correctionColumn] = "";
                        _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                    }
                    else
                    {
                        _commoditiesSoFar.Add(_commodityTexts[_correctionRow, _correctionColumn]); // If we're doing a batch of screenshots, don't keep doing the same commodity when we keep finding it
                    }
                }
                    else
                    {
                        // that was nothing 
                        _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                        _commodityTexts[_correctionRow, _correctionColumn] = "";
                    }
                }
                else if (_correctionColumn == 5 || _correctionColumn == 7) // hacks for LOW/MED/HIGH
                {
	                var commodityLevelUpperCase = StripPunctuationFromScannedText(_commodityTexts[_correctionRow, _correctionColumn]);

	                var levenshteinLow = _levenshtein.LD2("LOW", commodityLevelUpperCase);
                    var levenshteinMed = _levenshtein.LD2("MED", commodityLevelUpperCase);
                    var levenshteinHigh = _levenshtein.LD2("HIGH", commodityLevelUpperCase);
                    var levenshteinBlank = _levenshtein.LD2("", commodityLevelUpperCase);

	                //Pick the lowest levenshtein number
	                var lowestLevenshtein = Math.Min(Math.Min(levenshteinLow, levenshteinMed), Math.Min(levenshteinHigh, levenshteinBlank));

                    if (lowestLevenshtein == levenshteinLow)
	                {
		                _commodityTexts[_correctionRow, _correctionColumn] = "LOW";
	                }
	                else if (lowestLevenshtein == levenshteinMed)
	                {
		                _commodityTexts[_correctionRow, _correctionColumn] = "MED";
	                }
	                else if (lowestLevenshtein == levenshteinHigh)
	                {
		                _commodityTexts[_correctionRow, _correctionColumn] = "HIGH";
	                }
	                else // lowestLevenshtein == levenshteinBlank
	                {
		                _commodityTexts[_correctionRow, _correctionColumn] = "";
	                }

                    // we will never be challenged on low/med/high again.  this doesn't get internationalized on foreign-language installs... does it? :)
                    _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                }
            }
            // Don't pause for cells which have a high confidence, or have no commodity name
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            while (_correctionColumn < _commodityTexts.GetLength(1) && (_originalBitmapConfidences[_correctionRow, _correctionColumn] > .9f || _originalBitmapConfidences[_correctionRow, _correctionColumn] == 0 || _commodityTexts[_correctionRow, 0] == ""));

            if (_correctionColumn < _commodityTexts.GetLength(1))
            {
                // doing again some stateful enabling
                tbCommoditiesOcrOutput.Text = _commodityTexts[_correctionRow, _correctionColumn];
                tbConfidence.Text = _originalBitmapConfidences[_correctionRow, _correctionColumn].ToString(CultureInfo.InvariantCulture);
                bContinueOcr.Enabled = true;
                bIgnoreTrash.Enabled = true;
            }
            else
            {
                bContinueOcr.Enabled = false;
                bIgnoreTrash.Enabled = false;

                string finalOutput = _csvOutputSoFar;

                for (int row = 0; row < _commodityTexts.GetLength(0); row++)
                {
                    if (_commodityTexts[row, 0] != "") // don't create CSV if there's no commodity name
                    {
                        finalOutput += tbOcrSystemName.Text + ";" + tbOcrStationName.Text + ";";

                        for (int col = 0; col < _commodityTexts.GetLength(1); col++)
                        {
                            _commodityTexts[row, col] = _commodityTexts[row, col].Replace("\r", "").Replace("\n", "");

                            if (col == 3) continue; // don't export cargo levels
                            finalOutput += _commodityTexts[row, col] + ";";
                        }
                        finalOutput += ocr.CurrentScreenshotDateTime.ToString("s").Substring(0, 16) + ";";

                        if (cbExtendedInfoInCSV.Checked)
                            finalOutput += Path.GetFileName(_screenshotName) + ";";

                        finalOutput += _rowIds[row]+"\r\n";
                    }
                }

                _csvOutputSoFar += finalOutput;

                if(pbOriginalImage.Image != null)
                    pbOriginalImage.Image.Dispose();

                UpdateOriginalImage(null);
                UpdateTrimmedImage(null, null);

                if (RegulatedNoiseSettings.DeleteScreenshotOnImport)
                    File.Delete(_screenshotName);

                if (_screenshotResultsBuffer.Count == 0)
                {
                    tbFinalOcrOutput.Text += _csvOutputSoFar;
                    _csvOutputSoFar = null;
                    

                    pbOcrCurrent.Image = null;
                    if (_preOcrBuffer.Count == 0 && ocr.ScreenshotBuffer.Count == 0)
                    {
                        
                        tbFinalOcrOutput.Enabled = true;

                        if (RegulatedNoiseSettings.AutoImport)
                        {
                            tbCommoditiesOcrOutput.Text = "Imported!";
                            ImportFinalOcrOutput();
                            tbFinalOcrOutput.Text = "";
                            _csvOutputSoFar = "";
                            _commoditiesSoFar = new List<string>();
                            bClearOcrOutput.Enabled = false;
                            bEditResults.Enabled = false;
                        }
                        else
                        {
                            tbCommoditiesOcrOutput.Text = "Finished!";
                            bContinueOcr.Text = "Import";
                            bContinueOcr.Enabled = true;
                            bIgnoreTrash.Enabled = false;
                            bClearOcrOutput.Enabled = true;
                            bEditResults.Enabled = true;
                        }
                    }
                    else
                    {
                        tbCommoditiesOcrOutput.Text = "Working...!";
                        bClearOcrOutput.Enabled = false;
                        bEditResults.Enabled = false;
                    }

                }
                else
                {
                    var nextScreenshot = _screenshotResultsBuffer[0];
                    _screenshotResultsBuffer.Remove(nextScreenshot);
                    ScreenshotsQueued("(" + (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) + " queued)");
                    BeginCorrectingScreenshot(nextScreenshot.s, nextScreenshot.originalBitmaps, nextScreenshot.originalBitmapConfidences, nextScreenshot.rowIds, nextScreenshot.screenshotName);
                }
            }
        }

        private string StripPunctuationFromScannedText(string input)
        {
            return _textInfo.ToUpper(input.Replace(" ", "").Replace("-", "").Replace(".", "").Replace(",", ""));
        }

        #endregion

        #region Generic Delegates

        public delegate void GenericSingleParameterMessageDelegate(Object o, AppDelegateType a);

        public void GenericSingleParameterMessage(Object o, AppDelegateType a)
        {
            if (InvokeRequired)
            {
                Invoke(new GenericSingleParameterMessageDelegate(GenericSingleParameterMessage), o, a);
                return;
            }

            switch (a)
            {
                case AppDelegateType.AddEventToLog:
                    CommandersLog.CreateEvent((CommandersLogEvent)o);
                    break;
                case AppDelegateType.UpdateSystemNameLiveFromLog:
                    UpdateSystemNameFromLogFile(false);
                    break;
                case AppDelegateType.ChangeGridSort:
                    ChangeGridSort((string)o);
                    break;
                case AppDelegateType.MaximizeWindow:
                    WindowState = FormWindowState.Minimized;
                    Show();
                    WindowState = FormWindowState.Normal;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void ChangeGridSort(string s)
        {
            var q = s.Substring(26).Replace("&col=", ";").Replace("&rand=", ";").Split(';');

            var x = new ColumnClickEventArgs(int.Parse(q[1]));

            switch (q[0])
            {
                case "lvAllComms":
                    lvAllComms_ColumnClick(null, x);
                    break;
                case "lbPrices":
                    lbPrices_ColumnClick(null, x);
                    break;
                case "lbCommodities":
                    lbCommodities_ColumnClick(null, x);
                    break;
                case "lvStationToStation":
                    lvStationToStation_ColumnClick(null, x);
                    break;
            }
        }

        #endregion

        #region UpdateCurrentImage
        public delegate void UpdateCurrentImageDelegate(Bitmap b);

        public void UpdateCurrentImage(Bitmap b)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateCurrentImageDelegate(UpdateCurrentImage), b);
                return;
            }

            pbOcrCurrent.Image = (Bitmap)(b.Clone());
        }
        #endregion

        private void bContinueOcr_Click(object sender, EventArgs e)
        {
            if (_commodityTexts == null || _correctionColumn >= _commodityTexts.GetLength(1))
            {
                if (MessageBox.Show("Import this?", "Import?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ImportFinalOcrOutput();
                    tbFinalOcrOutput.Text = "";
                    bContinueOcr.Enabled = false;
                    bIgnoreTrash.Enabled = false;
                    _commoditiesSoFar = new List<string>();
                    bClearOcrOutput.Enabled = false;
                    bEditResults.Enabled = false;
                }
            }
            else
            {
                _commodityTexts[_correctionRow, _correctionColumn] = tbCommoditiesOcrOutput.Text;

                ContinueDisplayingResults();
            }
        }

        private void ImportFinalOcrOutput()
        {
            foreach (var s in tbFinalOcrOutput.Text.Replace("\r", "").Split('\n'))
            {
                if (s.Contains(";"))
                {
                    ImportCsvString(s, false, true);
                }
            }

            SetupGui();
        }

        private string _oldOcrName;

        private void tbOcrStationName_TextChanged(object sender, EventArgs e)
        {
            if (tbOcrStationName.Text != _oldOcrName && _oldOcrName != null)
            {
                var rows = tbFinalOcrOutput.Text.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                string newRows = "";

                foreach (var row in rows)
                {
                    var newRow1 = row.Substring(0, row.IndexOf(";"));
                    var newRow2 = tbOcrStationName.Text;
                    var newRow3 = row.Substring(row.IndexOf(";", 1));
                    newRow3 = newRow3.Substring(newRow3.IndexOf(";", 1));
                    newRows = newRows + newRow1 +";"+ newRow2 + newRow3 + "\r\n";

                }
                tbFinalOcrOutput.Text = newRows;
            }

            _oldOcrName = tbOcrStationName.Text;
        }

        private string _oldOcrSystemName;

        private void tbOcrSystemName_TextChanged(object sender, EventArgs e)
        {
            if (tbOcrSystemName.Text != _oldOcrSystemName && _oldOcrSystemName != null)
            {
                var rows = tbFinalOcrOutput.Text.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                string newRows = "";

                foreach (var row in rows)
                {
                    var newRow1 = row.Substring(row.IndexOf(";"));
                    newRows += tbOcrSystemName.Text + newRow1+"\r\n";

                }
                tbFinalOcrOutput.Text = newRows;
            }

            _oldOcrSystemName = tbOcrSystemName.Text;
        }

        private void cbColourScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)(sender)).Text)
            {
                case "Black on White":
                    sws.ForegroundColour = "#000000";
                    sws.BackgroundColour = "#FFFFFF";
                    break;
                case "White on Black":
                    sws.ForegroundColour = "#FFFFFF";
                    sws.BackgroundColour = "#000000";
                    break;
                case "Orange on Black":
                    sws.ForegroundColour = "#FF8C0D";
                    sws.BackgroundColour = "#000000";
                    break;
            }

            tbForegroundColour.Text = sws.ForegroundColour;
            tbBackgroundColour.Text = sws.BackgroundColour;
        }

        private void tbForegroundColour_TextChanged(object sender, EventArgs e)
        {
            sws.ForegroundColour = tbForegroundColour.Text;
            cbColourScheme.SelectedItem = null;
            RegulatedNoiseSettings.WebserverForegroundColor = tbForegroundColour.Text;
        }

        private void tbBackgroundColour_TextChanged(object sender, EventArgs e)
        {
            sws.BackgroundColour = tbBackgroundColour.Text;
            cbColourScheme.SelectedItem = null;
            RegulatedNoiseSettings.WebserverBackgroundColor = tbBackgroundColour.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CommandersLog.SaveLog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CommandersLog.LoadLog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            CommandersLog.CreateNewEvent();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            foreach (var s in StationDirectory)
            {
                var eventDates = s.Value.ToList().Select(x => x.SampleDate.AddSeconds(0 - x.SampleDate.Second)).Distinct();

                foreach (var d in eventDates)
                {
                    CommandersLog.CreateEvent("Market Data Collected", s.Key, s.Key, null, null, 0, null, d);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            _eddnSubscriberThread = new Thread(() => Eddn.Subscribe(this));
            _eddnSubscriberThread.Start();
        }

        #region EDDN Delegates
        private DateTime _lastGuiUpdate;

        private delegate void SetTextCallback(object text);

        private bool harvestStations = false;
        private int harvestStationsCount = -1;
        private int harvestCommsCount = -1;
        private StreamWriter _eddnSpooler = null;

        public void OutputEddnRawData(object text)
        {
            if (InvokeRequired)
            {
                SetTextCallback d = OutputEddnRawData;
                BeginInvoke(d, new { text });
            }
            else
            {
                tbEDDNOutput.Text = text.ToString();

                if (checkboxSpoolEddnToFile.Checked)
                {
                    if (_eddnSpooler == null)
                    {
                        if (!File.Exists(".//EddnOutput.txt"))
                            _eddnSpooler = File.CreateText(".//EddnOutput.txt");
                        else
                            _eddnSpooler = File.AppendText(".//EddnOutput.txt");
                    }

                    _eddnSpooler.WriteLine(text);
                }

                var headerDictionary = new Dictionary<string, string>();
                var messageDictionary = new Dictionary<string, string>();

                ParseEddnJson(text, headerDictionary, messageDictionary, checkboxImportEDDN.Checked);

                if (harvestStations && StationDirectory.Count > harvestStationsCount)
                {
                    if (File.Exists("stations.txt"))
                        File.Delete("stations.txt");

                    TextWriter f = new StreamWriter(File.OpenWrite("stations.txt"));
                    foreach (var x in StationDirectory.OrderBy(x => x.Key))
                    {
                        f.WriteLine(x.Key);
                    }
                    f.Close();
                    harvestStationsCount = StationDirectory.Count;
                }

                if (harvestStations && CommodityDirectory.Count > harvestCommsCount)
                {
                    if (File.Exists("commodities.txt"))
                        File.Delete("commodities.txt");

                    TextWriter f = new StreamWriter(File.OpenWrite("commodities.txt"));
                    foreach (var x in CommodityDirectory.OrderBy(x => x.Key))
                    {
                        f.WriteLine(x.Key);
                    }
                    f.Close();
                    harvestCommsCount = CommodityDirectory.Count;
                }
            }
        }

        private Dictionary<string, EddnPublisherVersionStats> _eddnPublisherStats = new Dictionary<string, EddnPublisherVersionStats>();
        private void ParseEddnJson(object text, Dictionary<string, string> headerDictionary, IDictionary<string, string> messageDictionary, bool import)
        {
            string txt = text.ToString();
            // .. we're here because we've received some data from EDDN

            if(txt!="")
                try
                {
                    // ReSharper disable StringIndexOfIsCultureSpecific.1
                    var headerRawStart = txt.IndexOf(@"""header""") + 12;
                    var headerRawLength = txt.Substring(headerRawStart).IndexOf("}");
                    var headerRawData = txt.Substring(headerRawStart, headerRawLength);

                    var messageRawStart = txt.IndexOf(@"""message"":") + 12;
                    var messageRawLength = txt.Substring(messageRawStart).IndexOf("}");
                    var messageRawData = txt.Substring(messageRawStart, messageRawLength);
                    // ReSharper restore StringIndexOfIsCultureSpecific.1
                    var headerRawPairs = headerRawData.Replace(@"""", "").Split(',');
                    var messageRawPairs = messageRawData.Replace(@"""", "").Split(',');


                    foreach (var rawHeaderPair in headerRawPairs)
                    {
                        var splitPair = new string[2];
                        splitPair[0] = rawHeaderPair.Substring(0, rawHeaderPair.IndexOf(':'));
                        splitPair[1] = rawHeaderPair.Substring(splitPair[0].Length + 1);
                        if (splitPair[0].StartsWith(" ")) splitPair[0] = splitPair[0].Substring(1);
                        if (splitPair[1].StartsWith(" ")) splitPair[1] = splitPair[1].Substring(1);
                        headerDictionary.Add(splitPair[0], splitPair[1]);
                    }

                    foreach (var rawMessagePair in messageRawPairs)
                    {
                        var splitPair = new string[2];
                        splitPair[0] = rawMessagePair.Substring(0, rawMessagePair.IndexOf(':'));
                        splitPair[1] = rawMessagePair.Substring(splitPair[0].Length + 1);
                        if (splitPair[0].StartsWith(" ")) splitPair[0] = splitPair[0].Substring(1);
                        if (splitPair[1].StartsWith(" ")) splitPair[1] = splitPair[1].Substring(1);
                        messageDictionary.Add(splitPair[0], splitPair[1]);
                    }

                    var nameAndVersion = (headerDictionary["softwareName"] + " / " + headerDictionary["softwareVersion"]);
                    if (!_eddnPublisherStats.ContainsKey(nameAndVersion))
                        _eddnPublisherStats.Add(nameAndVersion, new EddnPublisherVersionStats());

                    _eddnPublisherStats[nameAndVersion].MessagesReceived++;

                    var output = "";
                    foreach (var appVersion in _eddnPublisherStats)
                    {
                        output = output + appVersion.Key + " : " + appVersion.Value.MessagesReceived + " messages\r\n";
                    }
                    tbEddnStats.Text = output;

                    //System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;
                    if (import && headerDictionary["uploaderID"] != tbUsername.Text) // Don't import our own uploads...
                    {
                        string csvFormatted = messageDictionary["systemName"] + ";" +
                                              messageDictionary["stationName"] + ";" +
                                              messageDictionary["itemName"] + ";" +
                                              messageDictionary["sellPrice"] + ";" +
                                              messageDictionary["buyPrice"] + ";" +
                                              messageDictionary["demand"] + ";" +
                                              ";" +
                                              messageDictionary["stationStock"] + ";" +
                                              ";" +
                                              messageDictionary["timestamp"] + ";"
                                              +
                                              "<From EDDN>" + ";";
                        ImportCsvString(csvFormatted);

                        if ((DateTime.Now - _lastGuiUpdate) > TimeSpan.FromSeconds(10))
                        {
                            SetupGui();
                            _lastGuiUpdate = DateTime.Now;
                        }
                    }
                }
                catch (Exception e)
                {
                    tbEDDNOutput.Text = "Couldn't parse JSON!\r\n\r\n" + tbEDDNOutput.Text;
                }
        }

        private delegate void SetListeningDelegate();

        public void SetListening()
        {
            if (tbEDDNOutput.InvokeRequired)
            {
                SetListeningDelegate d = SetListening;
                BeginInvoke(d);
            }
            else
            {
                tbEDDNOutput.Text = "Listening...";
            }
        }
        #endregion

        private void button16_Click(object sender, EventArgs e)
        {
            if (_eddnSubscriberThread != null && _eddnSubscriberThread.IsAlive)
                _eddnSubscriberThread.Abort();
        }

        #region Station-To-Station
        private void cbStationToStationFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStationToStation();
        }

        private void cbStationToStationTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStationToStation();
        }

        private void UpdateStationToStation()
        {
            if (cbStationToStationTo.SelectedItem == null || cbStationToStationFrom.SelectedItem == null)
                return;

            lvStationToStation.Items.Clear();
            lvStationToStationReturn.Items.Clear();


            var stationFrom = (string)(cbStationToStationFrom.SelectedItem);
            var stationTo = (string)(cbStationToStationTo.SelectedItem);

            int bestRoundTrip;
            var results = GetBestRoundTripForTwoStations(stationFrom, stationTo, out bestRoundTrip);

            lblStationToStationMax.Text = bestRoundTrip.ToString();

            if (results.Item1 != null)
                foreach (var lvi in results.Item1)
                    lvStationToStation.Items.Add(lvi);

            if (results.Item2 != null)
                foreach (var lvi in results.Item2)
                    lvStationToStationReturn.Items.Add(lvi);

            if (_stationToStationColumnSorter.SortColumn != 7)
                lvStationToStation_ColumnClick(null, new ColumnClickEventArgs(7));

            if (_stationToStationReturnColumnSorter.SortColumn != 7)
                lvStationToStationReturn_ColumnClick(null, new ColumnClickEventArgs(7));

            if (SystemLocations.ContainsKey(CombinedNameToSystemName(cbStationToStationFrom.SelectedItem.ToString()).ToUpper()))
            {
                var dist = DistanceInLightYears(
                                                     CombinedNameToSystemName(cbStationToStationTo.SelectedItem.ToString()).ToUpper(),
                                                     SystemLocations[CombinedNameToSystemName(cbStationToStationFrom.SelectedItem.ToString()).ToUpper()]
                                                         .Item1);
                if(dist < double.MaxValue)
                    lblStationToStationLightYears.Text = "(" +
                                                     String.Format("{0:0.00}",dist
                                                     ) + " light years each way)";
                else lblStationToStationLightYears.Text = "(system(s) not recognised)";
            }
            else lblStationToStationLightYears.Text = "(system(s) not recognised)";
        }

        private Tuple<List<ListViewItem>,List<ListViewItem>> GetBestRoundTripForTwoStations(string stationFrom, string stationTo, out int bestRoundTrip)
        {
            if (stationFrom == null || stationTo == null) { bestRoundTrip = 0; return null; }
            var resultsOutbound = new List<ListViewItem>();
            var resultsReturn = new List<ListViewItem>();

            foreach (var commodity in CommodityDirectory)
            {
                var from = StationDirectory[stationFrom].Where(x => x.CommodityName == commodity.Key).ToList();
                var from2 = @from.Where(x => x.BuyPrice != 0 && x.Supply != 0).ToList();
                var to = StationDirectory[stationTo].Where(x => x.CommodityName == commodity.Key).ToList();
                var to2 = to.Where(x => x.SellPrice != 0 && x.Demand != 0).ToList();

                if (from2.Count() == 1 && to2.Count() == 1)
                {
                    var fromRow = from2.Single();
                    var toRow = to2.Single();

                    decimal sellPrice = toRow.SellPrice;
                    decimal supply = fromRow.Supply;
                    string supplyLevel = fromRow.SupplyLevel;
                    decimal buyPrice = fromRow.BuyPrice;
                    decimal demand = toRow.Demand;
                    string demandLevel = toRow.DemandLevel;
                    decimal difference = toRow.SellPrice - fromRow.BuyPrice;

                    var newItem = new
                        ListViewItem(new[]
                        {
                            commodity.Key,
                            buyPrice.ToString(CultureInfo.InvariantCulture),
                            supply.ToString(CultureInfo.InvariantCulture),
                            supplyLevel,
                            sellPrice.ToString(CultureInfo.InvariantCulture),
                            demand.ToString(CultureInfo.InvariantCulture),
                            demandLevel,
                            difference.ToString(CultureInfo.InvariantCulture)
                        });

                    if (difference > 0)
                    {
                        resultsOutbound.Add(newItem);
                    }
                }

                // Return trip


                @from = StationDirectory[stationTo].Where(x => x.CommodityName == commodity.Key).ToList();
                from2 = @from.Where(x => x.BuyPrice != 0 && x.Supply != 0).ToList();
                to = StationDirectory[stationFrom].Where(x => x.CommodityName == commodity.Key).ToList();
                to2 = to.Where(x => x.SellPrice != 0 && x.Demand != 0).ToList();

                if (from2.Count() == 1 && to2.Count() == 1)
                {
                    var fromRow = from2.Single();
                    var toRow = to2.Single();

                    decimal sellPrice = toRow.SellPrice;
                    decimal supply = fromRow.Supply;
                    string supplyLevel = fromRow.SupplyLevel;
                    decimal buyPrice = fromRow.BuyPrice;
                    decimal demand = toRow.Demand;
                    string demandLevel = toRow.DemandLevel;
                    decimal difference = toRow.SellPrice - fromRow.BuyPrice;

                    if (difference > 0)
                        resultsReturn.Add(new ListViewItem(new[]
                        {
                            commodity.Key,
                            buyPrice.ToString(CultureInfo.InvariantCulture),
                            supply.ToString(CultureInfo.InvariantCulture),
                            supplyLevel,
                            sellPrice.ToString(CultureInfo.InvariantCulture),
                            demand.ToString(CultureInfo.InvariantCulture),
                            demandLevel,
                            difference.ToString(CultureInfo.InvariantCulture)
                        }));
                }
            }

            var q = resultsOutbound.Count > 0 ? resultsOutbound.Max(x => int.Parse(x.SubItems[7].Text)) : 0;
            var r = resultsReturn.Count > 0 ? resultsReturn.Max(x => int.Parse(x.SubItems[7].Text)) : 0;

            bestRoundTrip = q + r;
            return new Tuple<List<ListViewItem>, List<ListViewItem>>(resultsOutbound, resultsReturn);
        }

        #endregion



        private void PostJsonToEddn(CsvRow rowToPost)
        {
            string json;

            if (RegulatedNoiseSettings.UseEddnTestSchema)
            {
                json =
                    @"{""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1/test"",""header"": {""uploaderID"": ""$0$"",""softwareName"": ""RegulatedNoise"",""softwareVersion"": ""v" +
                    RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture) +
                    @"""},""message"": {""buyPrice"": $2$,""timestamp"": ""$3$"",""stationStock"": $4$,""stationName"": ""$5$"",""systemName"": ""$6$"",""demand"": $7$,""sellPrice"": $8$,""itemName"": ""$9$""}}";
            }
            else
            {
                json =
                    @"{""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1"",""header"": {""uploaderID"": ""$0$"",""softwareName"": ""RegulatedNoise"",""softwareVersion"": ""v" +
                    RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture) +
                    @"""},""message"": {""buyPrice"": $2$,""timestamp"": ""$3$"",""stationStock"": $4$,""stationName"": ""$5$"",""systemName"": ""$6$"",""demand"": $7$,""sellPrice"": $8$,""itemName"": ""$9$""}}";
            }


            string commodityJson = json.Replace("$0$", tbUsername.Text.Replace("$1$", ""))
                .Replace("$2$", (rowToPost.BuyPrice.ToString(CultureInfo.InvariantCulture)))
                .Replace("$3$", (rowToPost.SampleDate.ToString("s")))
                .Replace("$4$", (rowToPost.Supply.ToString(CultureInfo.InvariantCulture)))
                .Replace("$5$", (rowToPost.StationName.Replace(" [" + rowToPost.SystemName + "]", "")))
                .Replace("$6$", (rowToPost.SystemName))
                .Replace("$7$", (rowToPost.Demand.ToString(CultureInfo.InvariantCulture)))
                .Replace("$8$", (rowToPost.SellPrice.ToString(CultureInfo.InvariantCulture)))
                .Replace("$9$", (rowToPost.CommodityName)
                );

            using (var client = new WebClient())
            {
                try
                {
                    client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", commodityJson);
                }
                catch (WebException ex)
                {
                    _logger.Log("Error uploading Json", true);
                    _logger.Log(ex.ToString(), true);
                    _logger.Log(ex.Message, true);
                    _logger.Log(ex.StackTrace, true);
                    if (ex.InnerException != null)
                        _logger.Log(ex.InnerException.ToString(), true);

                    using (WebResponse response = ex.Response)
                    {
                        using (Stream data = response.GetResponseStream())
                        {
                            if (data != null)
                            {
                                StreamReader sr = new StreamReader(data);
                                MessageBox.Show(sr.ReadToEnd(), "Error while uploading to EDDN");
                            }
                        }
                    }
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SetupGui();
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(
                "If you leave the Commodity Name blank in the UI or webpage, that entire row will be ignored on import (though it will still appear in the CSV). This is really useful when half a row has been OCR'ed and it's all gone horribly wrong :)",
                "Really Useful Tip...");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFile = new FolderBrowserDialog();
            openFile.SelectedPath = Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments));
            openFile.ShowDialog();


            _filesFound = new List<string>();

            if (openFile.SelectedPath != "")
            {
                _filesFound = new List<string>();

                DirSearch(openFile.SelectedPath);

                ImportListOfCsvs(_filesFound.ToArray());

            }

            SetupGui();
        }

        private List<string> _filesFound;

        private void DirSearch(string sDir)
        {

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d, "*.csv"))
                    {
                        _filesFound.Add(f);
                    }
                    DirSearch(d);
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Error recursing directories:", true);
                _logger.Log(ex.ToString(), true);
                _logger.Log(ex.Message, true);
                _logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString(), true);
                Console.WriteLine(ex.Message);
            }
        }

        private System.Threading.Timer stateTimer;

        public void UpdateSystemNameFromLogFile(bool updateCommandersLogUi = true)
        {
            var appConfigPath = RegulatedNoiseSettings.ProductsPath;

            if (Directory.Exists(appConfigPath))
            {
                var versions = Directory.GetDirectories(appConfigPath).ToList().OrderByDescending(x => x).ToList();

                if (versions[0].Contains("FORC-FDEV"))
                {
                    // We'll just go right ahead and use the latest log...
                    var netLogs =
                        Directory.GetFiles(versions[0] + "\\Logs", "netLog*.log")
                            .OrderByDescending(File.GetLastWriteTime)
                            .ToArray();

                    if (netLogs.Length != 0)
                    {
                        var newestNetLog = netLogs[0];

                        // Reading backwards from the end of the file in 8K blocks...

                        var fs = new FileStream(newestNetLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        fs.Seek(0, SeekOrigin.End);

                        var moveBack = 65536;

                        if (fs.Position - moveBack < 0)
                            moveBack = (int)(fs.Position);

                        fs.Seek(0 - moveBack, SeekOrigin.Current);

                        var sr = new StreamReader(fs);
                        string systemName = "";
                        while (!sr.EndOfStream)
                        {
                            string logLump = sr.ReadLine();

                            if (logLump != null && logLump.Contains("System:"))
                            {
                                systemName = logLump.Substring(logLump.IndexOf("(", StringComparison.Ordinal) + 1);
                                systemName = systemName.Substring(0, systemName.IndexOf(")", StringComparison.Ordinal));
                            }
                        }
                        sr.Close();

                        if (systemName != "")
                        {
                            if (tbCurrentSystemFromLogs.Text != systemName)
                            {
                                CommandersLog.CreateEvent("Jumped to", "", systemName, "", "", 0, "", DateTime.Now);

                                //tbCurrentSystemFromLogs.Text = systemName;
                            }
                            if (tbLogEventID.Text != "" && tbLogEventID.Text != systemName)
                            {
                                if (updateCommandersLogUi)
                                    cbLogSystemName.Text = systemName;
                            }
                        }
                    }

                    if (stateTimer == null)
                    {
                        var autoEvent = new AutoResetEvent(false);
                        TimerCallback timerCallback = TimerCallback;
                        stateTimer = new System.Threading.Timer(timerCallback, autoEvent, 10000, 10000);
                    }

                }
            }
        }

        private void TimerCallback(object state)
        {
            GenericSingleParameterMessage(null, AppDelegateType.UpdateSystemNameLiveFromLog);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (tbLogEventID.Text == "")
            {
                var newGuid = Guid.NewGuid().ToString();
                tbLogEventID.Text = newGuid;
                CommandersLog.CreateEvent();
                CommandersLog.UpdateCommandersLogListView();
                CommandersLog.CreateNewEvent();
            }
            else
            {
                var logEvent = CommandersLog.LogEvents.Single(x => x.EventID == tbLogEventID.Text);

                logEvent.EventType = cbLogEventType.Text;
                logEvent.EventID = tbLogEventID.Text;
                logEvent.Notes = tbLogNotes.Text;
                logEvent.Credits = nbCurrentCredits.Value;
                logEvent.TransactionAmount = nbTransactionAmount.Value;
                logEvent.CargoVolume = cbLogQuantity.Value;
                logEvent.Station = cbLogStationName.Text;
                logEvent.System = cbLogSystemName.Text;
                logEvent.CargoAction = cbCargoModifier.Text;
                logEvent.EventDate = dtpLogEventDate.Value;

                var listViewData = new string[LogEventProperties.Count()];

                listViewData[0] = logEvent.EventDate.ToString(CultureInfo.InvariantCulture);

                int ctr = 1;
                foreach (var y in LogEventProperties)
                {
                    if (y.Name != "EventDate")
                    {
                        listViewData[ctr] = y.GetValue(logEvent).ToString();
                        ctr++;
                    }
                }

                lvCommandersLog.SelectedIndexChanged -= lvCommandersLog_SelectedIndexChanged;
                var columnIndex = lvCommandersLog.Items.IndexOf(_commandersLogSelectedItem);
                lvCommandersLog.Items.RemoveAt(columnIndex);
                var newItem = new ListViewItem(listViewData);
                _commandersLogSelectedItem = newItem;
                lvCommandersLog.Items.Insert(columnIndex, newItem);
                lvCommandersLog.SelectedIndexChanged += lvCommandersLog_SelectedIndexChanged;
                CommandersLog.CreateNewEvent();
            }
        }

        private ListViewItem _commandersLogSelectedItem;

        private void lvCommandersLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv = ((ListView)sender);
            if (lv.SelectedItems.Count == 0) return;
            _commandersLogSelectedItem = lv.SelectedItems[0];
            var selectedGuid = _commandersLogSelectedItem.SubItems[lv.Columns.IndexOfKey("EventID")].Text;

            var logEvent = CommandersLog.LogEvents.Single(x => x.EventID == selectedGuid);

            cbLogEventType.Text = logEvent.EventType;
            tbLogEventID.Text = logEvent.EventID;
            tbLogNotes.Text = logEvent.Notes;
            nbCurrentCredits.Text = logEvent.Credits.ToString(CultureInfo.InvariantCulture);
            nbTransactionAmount.Text = logEvent.TransactionAmount.ToString(CultureInfo.InvariantCulture);
            cbLogQuantity.Text = logEvent.CargoVolume.ToString(CultureInfo.InvariantCulture);
            cbLogStationName.Text = logEvent.Station;
            cbLogSystemName.Text = logEvent.System;
            cbCargoModifier.Text = logEvent.CargoAction;
            cbLogCargoName.Text = logEvent.Cargo;
            dtpLogEventDate.Value = logEvent.EventDate;
            button21.Text = "Edit This Entry And Clear";

        }

        private void cbLogStationName_DropDown(object sender, EventArgs e)
        {
            cbLogStationName.Items.Clear();

            foreach (var x in StationDirectory)
                cbLogStationName.Items.Add(x.Key);
        }

        private void cbLogCargoName_DropDown(object sender, EventArgs e)
        {
            cbLogCargoName.Items.Clear();

            foreach (var x in CommodityDirectory)
                cbLogCargoName.Items.Add(x.Key);
        }

        private void lvCommandersLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _commandersLogColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_commandersLogColumnSorter.Order == SortOrder.Ascending)
                {
                    _commandersLogColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _commandersLogColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _commandersLogColumnSorter.SortColumn = e.Column;
                _commandersLogColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvCommandersLog.Sort();
        }

        private void cbLogSystemName_DropDown(object sender, EventArgs e)
        {
            cbLogSystemName.Items.Clear();
            foreach (var q in StationDirectory.Select(x => x.Key.Substring(x.Key.IndexOf("[", StringComparison.Ordinal) + 1, x.Key.IndexOf("]", StringComparison.Ordinal) - (x.Key.IndexOf("[", StringComparison.Ordinal) + 1))))
                cbLogSystemName.Items.Add(q);
        }

        #region Help button handlers
        private void ShowOcrHelpClick(object sender, EventArgs e)
        {
            HelpOCR h = new HelpOCR();
            h.Show();
        }

        private void ShowCommodityHelpClick(object sender, EventArgs e)
        {
            HelpCommodities h = new HelpCommodities();
            h.Show();
        }
        #endregion

        private void lbPrices_Click(object sender, EventArgs e)
        {
            Point mousePosition = lbPrices.PointToClient(MousePosition);
            ListViewHitTestInfo hit = lbPrices.HitTest(mousePosition);
            int columnindex = hit.Item.SubItems.IndexOf(hit.SubItem);

            if (columnindex == 11 && lbPrices.SelectedItems.Count == 1 && lbPrices.SelectedItems[0].SubItems[11].Text != "" && File.Exists(Environment.GetFolderPath((Environment.SpecialFolder.MyPictures)) + @"\Frontier Developments\Elite Dangerous\" + lbPrices.SelectedItems[0].SubItems[11].Text))
                Process.Start(Environment.GetFolderPath((Environment.SpecialFolder.MyPictures)) +
                              @"\Frontier Developments\Elite Dangerous\" +
                              lbPrices.SelectedItems[0].SubItems[11].Text);
        }

        // Recurse controls on form
        public IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl))
                                      .Concat(controls);
        }

        #region Christmas!
        System.Windows.Forms.Timer _timer;

        private void Form_Load(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.CheckVersion();

            Text += RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture);

            if (((DateTime.Now.Day == 24 || DateTime.Now.Day == 25 || DateTime.Now.Day == 26) &&
                 DateTime.Now.Month == 12) || (DateTime.Now.Day == 31 && DateTime.Now.Month == 12) ||
                (DateTime.Now.Day == 1 && DateTime.Now.Month == 1))
            {
                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = 75;
                _timer.Tick += OnTick;
                _timer.Start();
            }

            tabControl1.SelectedTab = tabPriceAnalysis;
            tabControl2.SelectedTab = tabPage3;
            tabControl2.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage2;
            tabControl2.SelectedTab = tabStationToStation;
            tabControl2.SelectedTab = tabPage3;
            tabControl1.SelectedTab = tabHelpAndChangeLog;

            Retheme();

        }

        private void Retheme()
        {
            if (RegulatedNoiseSettings.ForegroundColour == null || RegulatedNoiseSettings.BackgroundColour == null) return;

            var x = GetAll(this);

            int redF = int.Parse(RegulatedNoiseSettings.ForegroundColour.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int greenF = int.Parse(RegulatedNoiseSettings.ForegroundColour.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int blueF = int.Parse(RegulatedNoiseSettings.ForegroundColour.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            var f = Color.FromArgb(redF, greenF, blueF);
            int redB = int.Parse(RegulatedNoiseSettings.BackgroundColour.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int greenB = int.Parse(RegulatedNoiseSettings.BackgroundColour.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int blueB = int.Parse(RegulatedNoiseSettings.BackgroundColour.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            var b = Color.FromArgb(redB, greenB, blueB);
            foreach (Control c in x)
            {
                var props = c.GetType().GetProperties().Select(y => y.Name);


                c.BackColor = b;
                c.ForeColor = f;
                if (props.Contains("FlatStyle"))
                {
                    var prop = c.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

                    prop.SetValue(c, FlatStyle.Flat);
                }
                if (props.Contains("BorderStyle") && c.GetType() != typeof(Label))
                {
                    var prop = c.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

                    prop.SetValue(c, BorderStyle.FixedSingle);
                }
                if (props.Contains("LinkColor"))
                {
                    var prop = c.GetType().GetProperty("LinkColor", BindingFlags.Public | BindingFlags.Instance);

                    prop.SetValue(c, f);
                }
            }

            BackColor = b;
        }

        int animPhase;
        int phaseCtr;
        private void OnTick(object sender, EventArgs args)
        {
            switch (animPhase)
            {
                case 0:
                    if (phaseCtr == 20)
                    {
                        phaseCtr = 0;
                        animPhase = 1;
                    }
                    else
                    {
                        lblRegulatedNoise.ForeColor = phaseCtr % 2 == 0 ? Color.Red : Color.Black;
                        phaseCtr++;
                    }
                    break;
                case 1:
                    if (lblRegulatedNoise.Text.Length > 0)
                        lblRegulatedNoise.Text = lblRegulatedNoise.Text.Substring(0, lblRegulatedNoise.Text.Length - 1);
                    else
                    {
                        phaseCtr = 0;
                        animPhase = 2;
                    }
                    break;
                case 2:
                    //Merry Christmas!//16
                    if (phaseCtr < 17)
                    {
                        lblRegulatedNoise.ForeColor = Color.FromArgb(0, phaseCtr * 15, 0, 0);
                        if ((DateTime.Now.Day == 24 || DateTime.Now.Day == 25 || DateTime.Now.Day == 26) &&
                            DateTime.Now.Month == 12)
                            lblRegulatedNoise.Text = "Merry Christmas!".Substring(0, phaseCtr);
                        else
                            lblRegulatedNoise.Text = "Happy New Year!!".Substring(0, phaseCtr);
                        phaseCtr++;
                    }
                    else
                    {
                        phaseCtr = 0;
                        animPhase = 3;
                    }
                    break;
                case 3:
                    _timer.Stop();
                    break;
            }
        }

        #endregion

        private List<string> _knownCommodityNames = new List<string>
        {
            "Gallite", "Indite", "Lepidolite", "Rutile", "Uraninite", "Imperial Slaves", "Slaves", "Bioreducing Lichen",
            "H.E. Suits", "Biowaste", "Non-Lethal Weapons", "Personal Weapons", "Reactive Armour", "Wine",
            "Mineral Extractors", "Power Generators", "Water Purifiers", "Basic Medicines", "Combat Stabilisers",
            "Performance Enhancers", "Progenitor Cells", "Cobalt", "Gold", "Palladium", "Silver", "Bauxite",
            "Bertrandite", "Explosives", "Hydrogen Fuel", "Clothing", "Consumer Technology", "Domestic Appliances",
            "Animal Meat", "Coffee", "Fish", "Food Cartridges", "Fruit And Vegetables", "Grain", "Synthetic Meat", "Tea",
            "Beer", "Liquor", "Narcotics", "Tobacco", "Coltan", "Mineral Oil", "Pesticides", "Algae",
            "Atmospheric Processors", "Crop Harvesters", "Marine Equipment", "Agri-Medicines", "Animal Monitors",
            "Aquaponic Systems", "Land Enrichment Systems", "Leather", "Natural Fabrics", "Polymers", "Semiconductors",
            "Superconductors", "Aluminium", "Beryllium", "Copper", "Gallium", "Lithium", "Platinum", "Tantalum",
            "Titanium", "Uranium", "Auto-Fabricators", "Computer Components", "Robotics", "Synthetic Fabrics", "Scrap",
            "Battle Weapons", "Indium", "Resonating Separators"
        };

        private int keysInDirectory = -1;
        private List<string> cachedKnownCommodityNames;
        public List<string> KnownCommodityNames
        {
            get
            {
                if (CommodityDirectory.Keys.Count != keysInDirectory)
                {
                    keysInDirectory = CommodityDirectory.Keys.Count;
                    var s = CommodityDirectory.Keys.ToList();
                    var t = s.Union(_knownCommodityNames).ToList();
                    cachedKnownCommodityNames = t;
                }

                return cachedKnownCommodityNames;
            }
        }

        private void bSwapStationToStations_Click(object sender, EventArgs e)
        {
            var q = cbStationToStationFrom.SelectedItem;
            cbStationToStationFrom.SelectedItem = cbStationToStationTo.SelectedItem;
            cbStationToStationTo.SelectedItem = q;
        }

        private void lbPrices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPrices.SelectedItems.Count != 1)
                bStationEditRow.Enabled = false;
            else
                bStationEditRow.Enabled = true;

            if (lbPrices.SelectedItems.Count == 0)
                bStationDeleteRow.Enabled = false;
            else
                bStationDeleteRow.Enabled = true;
        }

        private void bStationEditRow_Click(object sender, EventArgs e)
        {
            var csvrow =
                StationDirectory[cbStation.SelectedItem.ToString()].First(
                    x => x.CommodityName == lbPrices.SelectedItems[0].Text);

            var csvrow2 =
                CommodityDirectory[lbPrices.SelectedItems[0].Text].First(
                    x => x.StationName == cbStation.SelectedItem.ToString());
            
            var f = new EditPriceData(csvrow, CommodityDirectory.Keys.ToList());
            var q = f.ShowDialog();

            if (q == DialogResult.OK)
            {
                StationDirectory[cbStation.SelectedItem.ToString()].Remove(csvrow);
                CommodityDirectory[lbPrices.SelectedItems[0].Text].Remove(csvrow2);
                ImportCsvString(f.RowToEdit.ToString());
                SetupGui();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var csvrow =
                StationDirectory[lbCommodities.SelectedItems[0].Text].First(
                        x => x.CommodityName == cbCommodity.SelectedItem.ToString());

            var csvrow2 =
                CommodityDirectory[cbCommodity.SelectedItem.ToString()].First(
                    x => x.StationName == lbCommodities.SelectedItems[0].Text);

            var f = new EditPriceData(csvrow, CommodityDirectory.Keys.ToList());
            var q = f.ShowDialog();

            if (q == DialogResult.OK)
            {
                StationDirectory[lbCommodities.SelectedItems[0].Text].Remove(csvrow);
                CommodityDirectory[cbCommodity.SelectedItem.ToString()].Remove(csvrow2);
                ImportCsvString(f.RowToEdit.ToString());
                SetupGui();
            }
        }

        private void lbCommodities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbCommodities.SelectedItems.Count != 1)
                bEditCommodity.Enabled = false;
            else
                bEditCommodity.Enabled = true;

            if (lbCommodities.SelectedItems.Count == 0)
                bCommodityDeleteRow.Enabled = false;
            else
                bCommodityDeleteRow.Enabled = true;
        }

        private void bStationDeleteRow_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lbPrices.SelectedItems)
            {
                var csvrow =
                    StationDirectory[cbStation.SelectedItem.ToString()].First(
                        x => x.CommodityName == item.Text);

                var csvrow2 =
                    CommodityDirectory[item.Text].First(
                        x => x.StationName == cbStation.SelectedItem.ToString());

                StationDirectory[cbStation.SelectedItem.ToString()].Remove(csvrow);
                CommodityDirectory[item.Text].Remove(csvrow2);
            }
            SetupGui();
        }

        private void bCommodityDeleteRow_Click(object sender, EventArgs e)
        {
           foreach (ListViewItem item in lbCommodities.SelectedItems)
            {
                var csvrow =
                    StationDirectory[item.Text].First(
                        x => x.CommodityName == cbCommodity.SelectedItem.ToString());

                var csvrow2 =
                    CommodityDirectory[cbCommodity.SelectedItem.ToString()].First(
                        x => x.StationName == item.Text);

                StationDirectory[item.Text].Remove(csvrow);
                CommodityDirectory[cbCommodity.SelectedItem.ToString()].Remove(csvrow2);
            }
            SetupGui();
        }

        private void button12_Click_2(object sender, EventArgs e)
        {
            lbAllRoundTrips.Items.Clear();
            int bestRoundTrip = -1;
            string stationA = "", stationB = "";
            List<Tuple<string, double>> allRoundTrips = new List<Tuple<string, double>>();

            foreach (var a in StationDirectory.Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.Key))))
                foreach (var b in StationDirectory.Where(x => !checkboxLightYears.Checked || Distance(CombinedNameToSystemName(x.Key))))
                {
                    int bestThisTrip;
                    GetBestRoundTripForTwoStations(a.Key, b.Key, out bestThisTrip);
                    if (bestThisTrip > 0)
                    {
                        string key1, key2;

                        if (string.Compare(a.Key, b.Key) < 0)
                        {
                            key1 = a.Key;
                            key2 = b.Key;
                        }
                        else
                        {
                            key1 = b.Key;
                            key2 = a.Key;                            
                        }

                        string credits;
                        double creditsDouble;
                        double distance = 1d;
                        if (checkboxPerLightYearRoundTrip.Checked)
                        {
                            distance = 2 * DistanceInLightYears(CombinedNameToSystemName(a.Key).ToUpper(), CombinedNameToSystemName(b.Key).ToUpper());
                            creditsDouble = bestThisTrip / distance;
                            credits = String.Format("{0:0.000}", creditsDouble / distance) + " Cr/Ly";
                        }
                        else
                        {
                            creditsDouble = bestThisTrip;
                            credits = (bestThisTrip + " Cr");
                        }

                        allRoundTrips.Add(
                            new Tuple<string, double>(
                                credits.PadRight(13) + " :" + 
                                key1
                                + "..." + 
                                key2
                                , creditsDouble));

                        if (bestThisTrip > bestRoundTrip)
                        {
                            bestRoundTrip = bestThisTrip;
                            stationA = a.Key;
                            stationB = b.Key;
                        }
                    }
                }

            var ordered = allRoundTrips.OrderByDescending(x => x.Item2).Select(x => x.Item1).Distinct().ToList().Cast<object>().ToArray();

            lbAllRoundTrips.Items.AddRange(ordered);
            if(lbAllRoundTrips.Items.Count > 0)
                lbAllRoundTrips.SelectedIndex = 0;

            //if (bestRoundTrip > 0)
            //{
            //    for (int i = 0; i < cbStationToStationFrom.Items.Count; i++)
            //    {
            //        if ((string)(cbStationToStationFrom.Items[i]) == stationA)
            //        {
            //            cbStationToStationFrom.SelectedIndex = i;
            //            break;
            //        }
            //    }
            //
            //    for (int i = 0; i < cbStationToStationTo.Items.Count; i++)
            //    {
            //        if ((string)(cbStationToStationTo.Items[i]) == stationB)
            //        {
            //            cbStationToStationTo.SelectedIndex = i;
            //            break;
            //        }
            //    }
            //}
        }

        private void cbLightYears_TextChanged(object sender, EventArgs e)
        {
            if (checkboxLightYears.Checked)
                SetupGui();
        }

        private void checkboxLightYears_CheckedChanged(object sender, EventArgs e)
        {
            SetupGui();
        }


        private void cbIncludeWithinRegionOfStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupGui();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            StationDirectory = PurgeEddnFromDirectory(StationDirectory);
            CommodityDirectory = PurgeEddnFromDirectory(CommodityDirectory);
            SetupGui();
        }

        private static ObjectDirectory PurgeEddnFromDirectory(ObjectDirectory directory)
        {
            ObjectDirectory newDirectory;
            
            if(directory.GetType() == typeof(StationDirectory))
                newDirectory = new StationDirectory();
            else
                newDirectory = new CommodityDirectory();

            foreach (var x in directory)
            {
                var newList = new List<CsvRow>();
                foreach (var y in x.Value)
                    if (y.SourceFileName != "<From EDDN>")
                        newList.Add(y);

                if(newList.Count > 0)
                    newDirectory.Add(x.Key, newList);
            }
            return newDirectory;
        }

        private void bEditResults_Click(object sender, EventArgs e)
        {
            var f = new EditOcrResults(tbFinalOcrOutput.Text);
            var q = f.ShowDialog();

            if (q == DialogResult.OK)
            {
                tbFinalOcrOutput.Text = f.ReturnValue;
            }
        }

        private void lbAllRoundTrips_SelectedIndexChanged(object sender, EventArgs e)
        {
            var t = lbAllRoundTrips.Text;
            var fromStation = t.Substring(t.IndexOf(':') + 1);
            fromStation = fromStation.Substring(0, fromStation.IndexOf("..."));
            var toStation = t.Substring(t.IndexOf("...") + 3);

            cbStationToStationFrom.Text = fromStation;
            cbStationToStationTo.Text = toStation;
            UpdateStationToStation();
        }

        private void lvStationToStationReturn_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _stationToStationReturnColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_stationToStationReturnColumnSorter.Order == SortOrder.Ascending)
                {
                    _stationToStationReturnColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _stationToStationReturnColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _stationToStationReturnColumnSorter.SortColumn = e.Column;
                _stationToStationReturnColumnSorter.Order = e.Column == 7 ? SortOrder.Descending : SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvStationToStationReturn.Sort();
        }

        private void bClearOcrOutput_Click(object sender, EventArgs e)
        {
			// when we do clear so we must consider all dependences (!->_commoditiesSoFar)
            // doing some stateful enabling of button again
            tbFinalOcrOutput.Text = "";
            bContinueOcr.Enabled = false;
            bIgnoreTrash.Enabled = false;
            _commoditiesSoFar = new List<string>();
            bClearOcrOutput.Enabled = false;
            bEditResults.Enabled = false;

        }

        /// <summary>
        /// Perform an "ignoring" of the current value (it's cosier)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdIgnore_Click(object sender, EventArgs e)
        {

            tbCommoditiesOcrOutput.Text = "";

            bContinueOcr_Click(sender, e);

    }

		
        private void cbAutoUppercase_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoUppercase.Checked)
	        {
		        tbCommoditiesOcrOutput.CharacterCasing = CharacterCasing.Upper;
        	} else
        	{
                tbCommoditiesOcrOutput.CharacterCasing = CharacterCasing.Normal;
            }

            RegulatedNoiseSettings.AutoUppercase = cbAutoUppercase.Checked;

        }
         
   
        /// <summary>
        /// direct submitting of the commodities with "Enter" if changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCommoditiesOcrOutput_Keypress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Return)
            {
                if (bContinueOcr.Enabled)
                { 
                    bContinueOcr_Click(sender, new EventArgs());
                }
            }

         }

    }

}
