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
using RegulatedNoise.Enums_and_Utility_Classes;
using Microsoft.Win32;
using System.ComponentModel;
using RegulatedNoise.EDDB_Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using CodeProject.Dialog;

namespace RegulatedNoise
{
    public partial class Form1 : RNBaseForm
    {
        private const string STR_START_MARKER = "<START>";
        private SplashScreenForm _Splash;

        public override string thisObjectName { get { return "Form1"; } }

        const string ID_DELIMITER = "empty";
        const int MAX_NAME_LENGTH = 120;
        const long SEARCH_MAXLENGTH = 160;
        const long SEARCH_MINLENGTH = 5;

        const string ID_NEWITEM = "<NEW>";
        const string ID_NOT_SET = "<NOT_SET>";

        private delegate void delButtonInvoker(Button myButton, bool enable);
        private delegate void delCheckboxInvoker(CheckBox myCheckbox, bool setChecked);

        public static Form1 InstanceObject;
        
        public EDDN Eddn;
        public Random random = new Random();
        public Guid SessionGuid;
        public PropertyInfo[] LogEventProperties;
        public static RegulatedNoiseSettings RegulatedNoiseSettings;
        public CommandersLog CommandersLog;
        public ObjectDirectory StationDirectory = new StationDirectory();
        public ObjectDirectory CommodityDirectory = new CommodityDirectory();
        private EDMilkyway _Milkyway;
        public static GameSettings GameSettings;
        public static OcrCalibrator OcrCalibrator;
        public List<string> KnownCommodityNames = new List<string>();
        public Dictionary<byte, string> CommodityLevel = new Dictionary<byte, string>();
        private Ocr ocr;
        private ListViewColumnSorter _stationColumnSorter, _commodityColumnSorter, _allCommodityColumnSorter, _stationToStationColumnSorter, _stationToStationReturnColumnSorter, _commandersLogColumnSorter;
        private Thread _eddnSubscriberThread;
        private FileSystemWatcher _fileSystemWatcher;
        private SingleThreadLogger _logger;
        private TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;
        private Levenshtein _levenshtein = new Levenshtein();
        private dsCommodities _commodities = new dsCommodities();
        private TabPage _EDDNTabPage;
        private Int32 _EDDNTabPageIndex;
        private string _LoggedSystem        = ID_NOT_SET;
        private string _LoggedLocation      = ID_NOT_SET;
        private string _LoggedVisited       = ID_NOT_SET;
        private string _LoggedMarketData    = ID_NOT_SET;

        //Implementation of the new classlibrary
        public EdSystem CurrentSystem;

        private BindingSource _bs_Stations                              = new BindingSource();
        private BindingSource _bs_StationsFrom                          = new BindingSource();
        private BindingSource _bs_StationsTo                            = new BindingSource();
        private Dictionary<string, int> _StationIndices                 = new Dictionary<string,int>();
        private bool _InitDone                                          = false;
        private StationHistory _StationHistory                          = new StationHistory();

        private String m_lastestStationInfo                             = String.Empty;
        private System.Windows.Forms.Timer Clock; 
        private CommandersLogEvent m_RightMouseSelectedLogEvent         = null;
        private bool m_Closing = false;
        private AutoResetEvent m_LogfileScanner_ARE                     = new AutoResetEvent(false);
        private Thread m_LogfileScanner_Thread;
        private EDSystem m_loadedSystemdata                             = new EDSystem();
        private EDSystem m_currentSystemdata                            = new EDSystem();
        private EDStation m_loadedStationdata                           = new EDStation();
        private EDStation m_currentStationdata                          = new EDStation();
        private string m_lastSystemValue                                = String.Empty;
        private string m_lastStationValue                               = String.Empty;
        private Boolean m_SystemLoadingValues                           = false;
        private Boolean m_StationLoadingValues                          = false;
        private Boolean m_SystemIsNew                                   = false;
        private Boolean m_StationIsNew                                  = false;
        private DateTime m_SystemWarningTime                            = DateTime.Now;
        private DateTime m_StationWarningTime                           = DateTime.Now;

        private PerformanceTimer _pt                                    = new PerformanceTimer();
        private String _AppPath                                         = string.Empty;
        private String _oldSystemName                                   = null;
        private String _oldStationName                                  = null;
        private string _CmdrsLog_LastAutoEventID                        = string.Empty;
        

        [SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
        public Form1()
        {
            _InitDone = false ;

            InstanceObject = this;

            _Splash = new SplashScreenForm();

#if !ep_Debug
            _Splash.Show();
#endif
            Cursor = Cursors.WaitCursor;

            try
            {

                _logger = new SingleThreadLogger(ThreadLoggerType.Form);
                _logger.Log("Initialising...\n");

                _Splash.InfoAdd("load settings...");
                LoadSettings();
                _logger.Log("  - settings loaded");
                _Splash.InfoChange("load settings...<OK>");
                
                string FormName = this.GetType().Name;
                if(RegulatedNoiseSettings.WindowBaseData.ContainsKey(FormName))
                    _Splash.setPosition(RegulatedNoiseSettings.WindowBaseData[FormName]);

                _Splash.InfoAdd("doing special work if something to do...");
                doSpecial();
                _logger.Log("  - special things done");
                _Splash.InfoChange("doing special work if something to do...<OK>");

                _Splash.InfoAdd("load settings...");
                SetProductPath();
                _logger.Log("  - product path set");
                _Splash.InfoChange("load settings...<OK>");

                SetProductAppDataPath();
                _logger.Log("  - product appdata set");

                _Splash.InfoAdd("initialize components...");
                InitializeComponent();
                _logger.Log("  - initialised component");
                _Splash.InfoChange("initialize components...<OK>");

                _Splash.InfoAdd("load game settings...");
                GameSettings = new GameSettings(this);
                _logger.Log("  - loaded game settings");
                _Splash.InfoChange("load game settings...<OK>");

                _Splash.InfoAdd("prepare listviews...");
                SetListViewColumnsAndSorters();
                _logger.Log("  - set list views");
                _Splash.InfoChange("prepare listviews...<OK>");

                _Splash.InfoAdd("prepare network interfaces...");
                PopulateNetworkInterfaces();
                _logger.Log("  - populated network interfaces");
                _Splash.InfoChange("prepare network interfaces...<OK>");

                _Splash.InfoAdd("create OCR object...");
                ocr = new Ocr(this);
                _logger.Log("  - created OCR object");
                _Splash.InfoChange("create OCR object...<OK>");

                Application.ApplicationExit += Application_ApplicationExit;
                _logger.Log("  - set application exit handler");

                _Splash.InfoAdd("create ocr calibrator...");
                OcrCalibrator = new OcrCalibrator();
                OcrCalibrator.LoadCalibration();
                var OcrCalibratorTabPage = new TabPage("OCR Calibration");
                OcrCalibratorTabPage.Name = "OCR_Calibration";
                var oct = new OcrCalibratorTab { Dock = DockStyle.Fill };
                OcrCalibratorTabPage.Controls.Add(oct);
                tabCtrlOCR.Controls.Add(OcrCalibratorTabPage);
                _logger.Log("  - initialised Ocr Calibrator");
                _Splash.InfoChange("create ocr calibrator...<OK>");

                _Splash.InfoAdd("prepare EDDN interface...");
                Eddn = new EDDN(this);
                _logger.Log("  - created EDDN object");
                _Splash.InfoChange("prepare EDDN interface...<OK>");
                
                ImportSystemLocations();
                _logger.Log("  - system locations imported");

                _Splash.InfoAdd("prepare 'Commander's Log'...");
                CommandersLog = new CommandersLog(this);
                _logger.Log("  - created Commander's Log object");
                CommandersLog.LoadLog(true);
                _logger.Log("  - loaded Commander's Log");
                CommandersLog.UpdateCommandersLogListView();
                _logger.Log("  - updated Commander's Log List View");
                _Splash.InfoChange("prepare 'Commander's Log'...<OK>");
                

                _Splash.InfoAdd("load collected market data...");
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
                _Splash.InfoChange("load collected market data...<OK>");

                _Splash.InfoAdd("load station history...");
                _StationHistory.loadHistory(@".\Data\StationHistory.json", true);
                _Splash.InfoChange("load station history...<OK>");

                _Splash.InfoAdd("apply settings...");
                ApplySettings();
                _Splash.InfoChange("apply settings...<OK>");

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
                    tabCtrlMain.Controls.Add(testtab);
                }


                // two methods with the same functionality 
                // maybe this was the better way but I've already improved the other 
                // way (UpdateSystemNameFromLogFile()) 
                // maybe this will some day be reactivated
                //var edl = new EdLogWatcher();

                //subscribe to edlogwatcherevents
                //edl.ClientArrivedtoNewSystem += (OnClientArrivedtoNewSystem);

                //After event subscriptino we can initialize
                //edl.Initialize();
                //edl.StartWatcher();

                _Splash.InfoAdd("load and prepare international commodity names...");
                // read the commodities and prepare language depending list
                _commodities.ReadXml(".//Data//Commodities.xml");

                // depending of the language this will be removed
                _EDDNTabPageIndex = tabCtrlMain.TabPages.IndexOfKey("tabEDDN");
                _EDDNTabPage = tabCtrlMain.TabPages[_EDDNTabPageIndex];

                // set language
                setLanguageCombobox();

                // load commodities in the correct language
                loadCommodities(RegulatedNoiseSettings.Language);
                loadCommodityLevels(RegulatedNoiseSettings.Language);
                _Splash.InfoChange("load and prepare international commodity names...<OK>");

                setOCRCalibrationTabVisibility();

                _Splash.InfoAdd("load tool tips...");
                loadToolTips();
                _Splash.InfoChange("load tool tips...<OK>");

                _Splash.InfoAdd("prepare system/location view...");
                prePrepareSystemAndStationFields();
                _Splash.InfoChange("prepare system/location view...<OK>");

                _Splash.InfoAdd("prepare GUI elements...");
                SetupGui(true);
                _Splash.InfoChange("prepare GUI elements...<OK>");

                _Splash.InfoAdd("starting logfile watcher...");
                UpdateSystemNameFromLogFile();
                _logger.Log("  - fetched system name from file");
                _Splash.InfoChange("starting logfile watcher...<OK>");

            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                cErr.processError(ex, "Error in main init function");    
            }

            _Splash.InfoAdd("\nstart sequence finished !!!");

            Cursor = Cursors.Default;
            _InitDone = true;
        }

        private void loadToolTips()
        {
            toolTip1.SetToolTip(txtPixelAmount, "if the bitmap has less dark pixels it will not processed by EliteBrainerous, is set to 0 all bitmaps will be processed");
            toolTip1.SetToolTip(lblPixelAmount, "if the bitmap has less dark pixels it will not processed by EliteBrainerous, is set to 0 all bitmaps will be processed");

            toolTip1.SetToolTip(txtPixelThreshold, "defines what a dark pixel is 0.0 is black, 1.0 is white");
            toolTip1.SetToolTip(lblPixelThreshold, "defines what a dark pixel is 0.0 is black, 1.0 is white");

            toolTip1.SetToolTip(cbCheckAOne, "Activate the pixel check with a click on this button. Then buy -one- ton of a commodity and take a screenshot of the market with the \"1\" on it.\nSee how much dark pixels the 1 has and take approximately the half of this value as \"dark pixel amount\"");
            
        }

        //private void OnClientArrivedtoNewSystem(object sender, EdLogLineSystemArgs args)
        //{
        //    if (InvokeRequired)
        //    {
        //        this.Invoke(new Action<EdSystem>(ClientArrivedtoNewSystem), new object[] { args.System });
        //        return;
        //    }
        //    ClientArrivedtoNewSystem(args.System);
            
        //}

        //private void ClientArrivedtoNewSystem(EdSystem System)
        //{
        //    CurrentSystem = System;
        //    tbCurrentSystemFromLogs.Text  = System.Name;
        //    //replace UpdateSystemNameFromLogFile
        //}
       
        /// <summary>
        /// using the direct EDDB format 
        /// (see http://eddb.io/api)
        /// </summary>
        private void ImportSystemLocations()
        {
            // read file into a string and deserialize JSON to a type
            try
            {

                _Splash.InfoAdd("create milkyway...");
                _Milkyway = new EDMilkyway();
                
                // 1. load the EDDN data
                { 
                    bool needPriceCalculation = !myMilkyway.loadCommodityData(@"./Data/commodities.json", @"./Data/commodities_RN.json", true, true);

                    if(needPriceCalculation || RegulatedNoiseSettings.LoadStationsJSON)
                    {
                        _Splash.InfoAdd("...loading stations from <stations.json> (calculation of plausibility limits required)...");
                        myMilkyway.loadStationData(@"./Data/stations.json", EDMilkyway.enDataType.Data_EDDB, false);
                        _Splash.InfoChange("...loading stations from <stations.json> (calculation of plausibility limits required)...<OK> (" + myMilkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Count + " stations loaded)");
                    }
                    else
                    { 
                        // look which stations-file we can get
                        if (File.Exists(@"./Data/stations_lite.json"))
                        {
                            _Splash.InfoAdd("...loading stations from <stations_lite.json>...");
                            myMilkyway.loadStationData(@"./Data/stations_lite.json", EDMilkyway.enDataType.Data_EDDB, false);
                            _Splash.InfoChange("...loading stations from <stations_lite.json>...<OK> (" + myMilkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Count + " stations loaded)");
                        }
                        else
                        {
                            _Splash.InfoAdd("...loading stations from <stations.json>...");
                            myMilkyway.loadStationData(@"./Data/stations.json", EDMilkyway.enDataType.Data_EDDB, false);
                            _Splash.InfoChange("...loading stations from <stations.json>...<OK> (" + myMilkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Count + " stations loaded)");
                        }
                    }

                    // load the systems
                    _Splash.InfoAdd("...loading systems from <systems.json>...");
                    myMilkyway.loadSystemData(@"./Data/systems.json", EDMilkyway.enDataType.Data_EDDB, false);
                    _Splash.InfoChange("...loading systems from <systems.json>...<OK> (" + myMilkyway.getSystems(EDMilkyway.enDataType.Data_EDDB).Count + " systems loaded)");
                }
                    
                // 2. load own local data
                _Splash.InfoAdd("...loading own stations from <stations_own.json>...");
                myMilkyway.loadStationData(@"./Data/stations_own.json", EDMilkyway.enDataType.Data_Own, true);
                _Splash.InfoChange("...loading stations from <stations_own.json>...<OK> (" + myMilkyway.getStations(EDMilkyway.enDataType.Data_Own).Count + " stations loaded)");

                _Splash.InfoAdd("...loading own systems from <systems_own.json>...");
                myMilkyway.loadSystemData(@"./Data/systems_own.json", EDMilkyway.enDataType.Data_Own, true);
                _Splash.InfoChange("...loading own systems from <systems_own.json>...<OK> (" + myMilkyway.getSystems(EDMilkyway.enDataType.Data_Own).Count + " systems loaded)");

                _Splash.InfoAdd("...merging data...");
                if (myMilkyway.mergeData())
                { 
                    myMilkyway.saveStationData(@"./Data/stations_own.json", EDMilkyway.enDataType.Data_Own, true);
                    myMilkyway.saveSystemData(@"./Data/systems_own.json", EDMilkyway.enDataType.Data_Own, true);
                }    
                _Splash.InfoChange("...merging data...<OK>");

                _Splash.InfoAdd("...loading commodity data from <commodities.json>...");
                myMilkyway.loadCommodityData(@"./Data/commodities.json", @"./Data/commodities_RN.json", true);
                _Splash.InfoChange("...loading commodity data from <commodities.json>...<OK>");

                myMilkyway.calculateAveragePrices();

                _Splash.InfoAdd("create milkyway...<OK>");
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading system and station data", ex);   
            }
        }

        private void setColumns(ListView currentListView)
        {
            List<ColumnData> currentData = RegulatedNoiseSettings.ListViewColumnData[currentListView.Name];

            switch (currentListView.Name)
            {
                case "lvCommandersLog":
                    currentListView.Columns[0].Width 	=  113;
                    currentListView.Columns[1].Width 	=  119;
                    currentListView.Columns[2].Width 	=  122;
                    currentListView.Columns[3].Width 	=  141;
                    currentListView.Columns[4].Width 	=   96;
                    currentListView.Columns[5].Width 	=   72;
                    currentListView.Columns[6].Width 	=   77;
                    currentListView.Columns[7].Width 	=  127;
                    currentListView.Columns[8].Width 	=   60;
                    currentListView.Columns[9].Width 	=   63;
                    currentListView.Columns[10].Width 	=   60;
                    break;
            }

            foreach (ColumnHeader currentHeader in currentListView.Columns)
            {
                ColumnData Data = currentData.Find(x => x.ColumnName.Equals(currentHeader.Name, StringComparison.InvariantCultureIgnoreCase));
                if (Data.Width > -1)
                    currentHeader.Width = Data.Width;
            }

            currentListView.ColumnWidthChanged += lvCommandersLog_ColumnWidthChanged;
        }

        private void saveColumns(ListView currentListView)
        {
            List<ColumnData> currentData = RegulatedNoiseSettings.ListViewColumnData[currentListView.Name];

            foreach (ColumnHeader currentHeader in currentListView.Columns)
            {
                ColumnData Data = currentData.Find(x => x.ColumnName.Equals(currentHeader.Name, StringComparison.InvariantCultureIgnoreCase));
                Data.Width = currentHeader.Width;
            }

            SaveSettings();
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

            setColumns(lvCommandersLog);

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

            // nothing found ? then lets have a try with the MUICache
            string ProgramName = "Elite:Dangerous Executable";
            string ProgramPath = string.Empty;

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache");

            if (key != null)
            {
                string[] Names = key.GetValueNames();
                

                for (int i = 0; i < Names.Count(); i++)
                {
                    if (key.GetValue(Names[i]).ToString() == ProgramName)
                    {
                        ProgramPath = Names[i].ToString();
                        ProgramPath = ProgramPath.Substring(0, ProgramPath.LastIndexOf("\\Products\\") + 9);
                        return ProgramPath;
                    }

                }
               
            }

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

                var MBResult = MsgBox.Show(
                    "Hm, that doesn't seem right" +
                    (dialog.SelectedPath != "" ? ", " + dialog.SelectedPath + " isn't the Frontier 'Products' directory"  : "")
                + ". Please try again...", "", MessageBoxButtons.RetryCancel);

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Application.Exit();
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
                var MBResult = MsgBox.Show("Automatic discovery of Frontier directory failed, please point me to your Frontier 'Products' directory.", "", MessageBoxButtons.RetryCancel);

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Application.Exit();

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
                
                var MBResult = MsgBox.Show("Couldn't find a FORC-FDEV.. directory in the Frontier Products dir, please try again...", "", MessageBoxButtons.RetryCancel);

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Application.Exit();

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

                var MBResult = MsgBox.Show(
                    "Hm, that doesn't seem right, " + dialog.SelectedPath +
                    " is not the Game Options directory, Please try again", "", MessageBoxButtons.RetryCancel);

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Application.Exit();

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
                var MBResult = MsgBox.Show(@"Automatic discovery of the Game Options directory failed, please point me to it...", "", MessageBoxButtons.RetryCancel);

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Application.Exit();

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
                    MsgBox.Show("Couldn't load settings; maybe they are from a previous version.  A new settings file will be created on exit.");
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
            txtWebserverPort.Text = RegulatedNoiseSettings.WebserverPort;
            if (RegulatedNoiseSettings.WebserverIpAddress != "") cbInterfaces.Text = RegulatedNoiseSettings.WebserverIpAddress;

            
            cbAutoImport.Checked = RegulatedNoiseSettings.AutoImport;

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

            txtTraineddataFile.Text                 = RegulatedNoiseSettings.TraineddataFile;
            
            _commandersLogColumnSorter.SortColumn   = RegulatedNoiseSettings.CmdrsLogSortColumn;
            _commandersLogColumnSorter.Order        = RegulatedNoiseSettings.CmdrsLogSortOrder;

            cbAutoAdd_JumpedTo.Checked              = RegulatedNoiseSettings.AutoEvent_JumpedTo;
            cbAutoAdd_Visited.Checked               = RegulatedNoiseSettings.AutoEvent_Visited;
            cbAutoAdd_Marketdata.Checked            = RegulatedNoiseSettings.AutoEvent_MarketDataCollected;
            cbAutoAdd_ReplaceVisited.Checked        = RegulatedNoiseSettings.AutoEvent_ReplaceVisited;

            txtPixelThreshold.Text                  = RegulatedNoiseSettings.EBPixelThreshold.ToString("F1");
            txtPixelAmount.Text                     = RegulatedNoiseSettings.EBPixelAmount.ToString();
            txtGUIColorCutoffLevel.Text             = RegulatedNoiseSettings.GUIColorCutoffLevel.ToString();

            // perform the sort with the last sort options.
            this.lvCommandersLog.Sort();

            txtlastStationCount.Text                = RegulatedNoiseSettings.lastStationCount.ToString();
            cmbLightYears.Text                      = RegulatedNoiseSettings.lastLightYears.ToString();
            cblastVisitedFirst.Checked              = RegulatedNoiseSettings.lastStationCountActive;
            cbLimitLightYears.Checked               = RegulatedNoiseSettings.limitLightYears;
            cbPerLightYearRoundTrip.Checked         = RegulatedNoiseSettings.PerLightYearRoundTrip;
            cbAutoActivateOCRTab.Checked            = RegulatedNoiseSettings.AutoActivateOCRTab;
            cbAutoActivateSystemTab.Checked         = RegulatedNoiseSettings.AutoActivateSystemTab;

            cbIncludeUnknownDTS.Checked             = RegulatedNoiseSettings.IncludeUnknownDTS;
            cbLoadStationsJSON.Checked              = RegulatedNoiseSettings.LoadStationsJSON;

            cmbStationToStar.Text                   = RegulatedNoiseSettings.lastStationToStar.ToString();
            cbStationToStar.Checked                 = RegulatedNoiseSettings.StationToStar;

            cmbMaxRouteDistance.Text                = RegulatedNoiseSettings.lastMaxRouteDistance.ToString();
            cbMaxRouteDistance.Checked              = RegulatedNoiseSettings.MaxRouteDistance;

            switch (RegulatedNoiseSettings.CBSortingSelection)
            {
            	case 1:
                    rbSortBySystem.Checked = true;
                    break;
                case 2:
                    rbSortByStation.Checked = true;
                    break;
                case 3:
                    rbSortByDistance.Checked = true;
                    break;
                default:
                    rbSortBySystem.Checked = true;
            		RegulatedNoiseSettings.CBSortingSelection = 1;
            	    break;
            }
            
            // Set the MinDate and MaxDate.
            nudPurgeOldDataDays.Value = RegulatedNoiseSettings.oldDataPurgeDeadlineDays;
        }

        /// <summary>
        /// prepares the commodities in the correct language
        /// </summary>
        /// <param name="Language"></param>
        private void loadCommodities(enLanguage Language)
        {
            KnownCommodityNames.Clear();

            foreach (dsCommodities.NamesRow currentCommodity in _commodities.Names)
            {
                if (Language == enLanguage.eng)
                    KnownCommodityNames.Add(currentCommodity.eng);

                else if (Language == enLanguage.ger)
                    KnownCommodityNames.Add(currentCommodity.ger);

                else
                    KnownCommodityNames.Add(currentCommodity.fra);

            }

        }

        /// <summary>
        /// prepares the commodities in the correct language
        /// </summary>
        public string getCommodityBasename(string CommodityName)
        {
            enLanguage language = RegulatedNoiseSettings.Language;
            return getCommodityBasename(language, CommodityName);
        }

        /// <summary>
        /// prepares the commodities in the correct language
        /// </summary>
        /// <param name="Language"></param>
        public string getCommodityBasename(enLanguage Language, string CommodityName)
        {
            string BaseName                             = String.Empty;
            dsCommodities.NamesRow[] currentCommodity   = null;

            switch (Language)
            {
                case enLanguage.eng:
                    currentCommodity = (dsCommodities.NamesRow[])(_commodities.Names.Select("eng='" + CommodityName + "'"));
                    break;
                case enLanguage.ger:
                    currentCommodity = (dsCommodities.NamesRow[])(_commodities.Names.Select("ger='" + CommodityName + "'"));
                    break;
                case enLanguage.fra:
                    currentCommodity = (dsCommodities.NamesRow[])(_commodities.Names.Select("fra='" + CommodityName + "'"));
                    break;
            }
            
            if (currentCommodity.Count() > 0)
                BaseName = currentCommodity[0].eng;

            return BaseName;

        }

        /// <summary>
        /// prepares the commodities in the correct language
        /// </summary>
        /// <param name="Language"></param>
        public string getLocalizedCommodity(enLanguage Language, string CommodityName)
        {
            string BaseName = String.Empty;

            List<dsCommodities.NamesRow> currentCommodity = _commodities.Names.Where(x => ((x.eng.Equals(CommodityName, StringComparison.InvariantCultureIgnoreCase)) ||
                                                                                           (x.ger.Equals(CommodityName, StringComparison.InvariantCultureIgnoreCase)) ||
                                                                                           (x.fra.Equals(CommodityName, StringComparison.InvariantCultureIgnoreCase)))).ToList();

            if (currentCommodity.Count() > 0)
            {
                switch (Language)
                {
                    case enLanguage.eng:
                        BaseName = currentCommodity[0].eng;
                        break;
                    case enLanguage.ger:
                        BaseName = currentCommodity[0].ger;
                        break;
                    case enLanguage.fra:
                        BaseName = currentCommodity[0].fra;
                        break;
                }

            }

            return BaseName;

        }

        /// <summary>
        /// prepares the commoditylevels in the correct language
        /// </summary>
        /// <param name="Language"></param>
        private void loadCommodityLevels(enLanguage Language)
        {
            dsCommodities.LevelsRow[] Level;

            CommodityLevel.Clear();

            for (int i = 0; i <= 2; i++)
            {
                if (i == 0)
                    Level = (dsCommodities.LevelsRow[])_commodities.Levels.Select("ID=" + (byte)enCommodityLevel.LOW);

                else if (i == 1)
                    Level = (dsCommodities.LevelsRow[])_commodities.Levels.Select("ID=" + (byte)enCommodityLevel.MED);

                else
                    Level = (dsCommodities.LevelsRow[])_commodities.Levels.Select("ID=" + (byte)enCommodityLevel.HIGH);

                if (Language == enLanguage.eng)
                    CommodityLevel.Add(Level[0].ID, Level[0].eng);

                else if (Language == enLanguage.ger)
                    CommodityLevel.Add(Level[0].ID, Level[0].ger);

                else
                    CommodityLevel.Add(Level[0].ID, Level[0].fra);

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
                //MsgBox.Show("File created... but it doesn't exist?!  Hit OK and I'll retry...");
                Thread.Sleep(100);
            }

            //MsgBox.Show("Good news! " + fileSystemEventArgs.FullPath +
            //                " exists!  Let's pause for a moment before opening it...");

            ScreenshotsQueued("(" + (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) + " queued)");
            // if the textfield support auto-uppercase we must consider
            string s = CommoditiesText("").ToString().ToUpper();

            if (s == "Imported!".ToUpper() || s == "Finished!".ToUpper() || s == "" || s == "No rows found...".ToUpper())
                CommoditiesText("Working...");



            if (_ocrThread == null || !_ocrThread.IsAlive)
            {
                Form1.InstanceObject.ActivateOCRTab();

                // some stateful enabling for the buttons
                setButton(bClearOcrOutput, false);
                setButton(bEditResults, false);

                _ocrThread = new Thread(() => ocr.ScreenshotCreated(fileSystemEventArgs.FullPath, tbCurrentSystemFromLogs.Text));
                _ocrThread.IsBackground = false;
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
                    setButton(bClearOcrOutput, false);
                    setButton(bEditResults, false);

                    var s = _preOcrBuffer[0];
                    _preOcrBuffer.RemoveAt(0);
                    _ocrThread = new Thread(() => ocr.ScreenshotCreated(s, tbCurrentSystemFromLogs.Text));
                    _ocrThread.IsBackground = false;
                    _ocrThread.Start();
                    ScreenshotsQueued("(" +
                                      (_screenshotResultsBuffer.Count + ocr.ScreenshotBuffer.Count + _preOcrBuffer.Count) +
                                      " queued)");
                }
                }
            }

        public void ActivateOCRTab()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ActivateOCRTab));
                return;
            }

            if (cbAutoActivateOCRTab.Checked && !cbCheckAOne.Checked)
                try
                {
                    tabCtrlMain.SelectedTab = tabCtrlMain.TabPages["tabOCRGroup"];
                    tabCtrlOCR.SelectedTab  = tabCtrlOCR.TabPages["tabOCR"];
                }
                catch (Exception)
                {
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
        public delegate void del_setControlText(Control CtrlObject, string newText);
        public delegate void del_setLocationInfo(string System, string Location);

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
            m_Closing = true;
            m_LogfileScanner_ARE.Set();

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
            string newFile, backupFile, currentFile;

            currentFile = "RegulatedNoiseSettings.xml";

            newFile = String.Format("{0}_new{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));
            backupFile = String.Format("{0}_bak{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));

            var stream = new FileStream(newFile, FileMode.Create, FileAccess.Write, FileShare.None);
            var x = new XmlSerializer(RegulatedNoiseSettings.GetType());
            x.Serialize(stream, RegulatedNoiseSettings);
            stream.Close();

            // we delete the current file not until the new file is written without errors

            // delete old backup
            if (File.Exists(backupFile))
                File.Delete(backupFile);

            // rename current file to old backup
            if (File.Exists(currentFile))
                File.Move(currentFile, backupFile);

            // rename new file to current file
            File.Move(newFile, currentFile);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveCommodityData();
        }

        public void setButton(Button myButton, bool enable)
        {
            if (myButton.InvokeRequired)
                myButton.Invoke(new delButtonInvoker(setButton), myButton, enable);
            else
                myButton.Enabled = enable;
        }

        public void setCheckbox(CheckBox myCheckbox, bool setChecked)
        {
            if (myCheckbox.InvokeRequired)
                myCheckbox.Invoke(new delCheckboxInvoker(setCheckbox), myCheckbox, setChecked);
            else
                myCheckbox.Checked = setChecked;
        }

        private void SaveCommodityData(bool force = false)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            string newFile, backupFile, currentFile;

            if (force)
                saveFile.FileName = "AutoSave.csv";
            else
                saveFile.FileName = "Unified EliteOCR Data " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".csv";

            saveFile.InitialDirectory = "\\.";
            saveFile.DefaultExt = "csv";
            saveFile.Filter = "CSV (*.csv)|*.csv";


            if (force || saveFile.ShowDialog() == DialogResult.OK)
            {

                currentFile = saveFile.FileName;
                newFile = String.Format("{0}_new{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));
                backupFile = String.Format("{0}_bak{1}", Path.GetFileNameWithoutExtension(currentFile), Path.GetExtension(currentFile));

                var writer = new StreamWriter(File.OpenWrite(newFile));

                writer.WriteLine("System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;");

                foreach (var station in StationDirectory)
                {
                    foreach (var commodity in station.Value)
                    {
                        var output = string.Join(";", new[]
                        {
                            commodity.SystemName,
                            commodity.StationID.Replace(" [" + commodity.SystemName + "]", ""),
                            commodity.CommodityName,
                            commodity.SellPrice != 0 ? commodity.SellPrice.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.BuyPrice != 0 ? commodity.BuyPrice.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.Demand != 0 ? commodity.Demand.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.DemandLevel,
                            commodity.Supply != 0 ? commodity.Supply.ToString(CultureInfo.InvariantCulture) : "",
                            commodity.SupplyLevel,
                            commodity.SampleDate.ToString("s", CultureInfo.CurrentCulture).Substring(0, 16),
                            cbExtendedInfoInCSV.Checked ? commodity.SourceFileName : ""
                        });

                        // I'm sure that's not wanted vv
                        //if (cbExtendedInfoInCSV.Checked)
                        ///    writer.WriteLine(output + ";");

                        writer.WriteLine(output + ";");
                    }
                }
                writer.Close();

                // we delete the current file not until the new file is written without errors

                if (force)
                {
                    // delete old backup
                    if (File.Exists(backupFile))
                        File.Delete(backupFile);

                    // rename current file to old backup
                    if (File.Exists(currentFile))
                        File.Move(currentFile, backupFile);
                }
                else
                {
                    // delete existing file
                    if (File.Exists(currentFile))
                        File.Delete(currentFile);
                }

                // rename new file to current file
                File.Move(newFile, currentFile);

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
                    MsgBox.Show("Error: " + filename + " is unreadable or in an old format.  Skipping...");
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

        private void ImportCsvString(string line, bool suspendDuplicateChecking = false, bool postToEddn = false, bool updateStationVisitations = false)
        {
            var values = line.Split(';');
            bool ignoreThisRecord = false;

            CsvRow currentRow = new CsvRow();

            currentRow.SystemName = values[0];
            currentRow.StationName = _textInfo.ToTitleCase(values[1].ToLower());
            currentRow.StationID = _textInfo.ToTitleCase(values[1].ToLower()) + " [" + currentRow.SystemName + "]";
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
                if (updateStationVisitations)
                    _StationHistory.addVisit(currentRow.StationID);

                if (!suspendDuplicateChecking)
                {
                    
                    if (StationDirectory.ContainsKey(currentRow.StationID))
                    {
                        var obsoleteData =
                            StationDirectory[currentRow.StationID].Where(
                                x =>
                                    x.StationID == currentRow.StationID && x.CommodityName == currentRow.CommodityName &&
                                    x.SampleDate <= currentRow.SampleDate).ToList();

                        // is there older data for delete ?
                        foreach (var x in obsoleteData)
                        {
                            StationDirectory[currentRow.StationID].Remove(x);
                            CommodityDirectory[currentRow.CommodityName].Remove(x);
                        }

                        // is there already data that is younger than this new record
                        var selfIsObsolete =
                            StationDirectory[currentRow.StationID].Where(
                                x =>
                                    x.StationID == currentRow.StationID && x.CommodityName == currentRow.CommodityName &&
                                    x.SampleDate > currentRow.SampleDate).ToList();
                        if (selfIsObsolete.Count > 0)
                        { 
                            ignoreThisRecord = true;  
                        }
                    }
                }

                if (!ignoreThisRecord) 
                {
                    if (!StationDirectory.ContainsKey(currentRow.StationID))
                        StationDirectory.Add(currentRow.StationID, new List<CsvRow>());


                    if (suspendDuplicateChecking || StationDirectory[currentRow.StationID].Count(x => x.StationID == currentRow.StationID && x.CommodityName == currentRow.CommodityName && x.SampleDate == currentRow.SampleDate) == 0)
                    {
                        StationDirectory[currentRow.StationID].Add(currentRow);

                        if (!CommodityDirectory.ContainsKey(currentRow.CommodityName))
                            CommodityDirectory.Add(currentRow.CommodityName, new List<CsvRow>());

                        CommodityDirectory[currentRow.CommodityName].Add(currentRow);
                    }

                    if (postToEddn && cbPostOnImport.Checked && currentRow.SystemName != "SomeSystem")
                        Eddn.sendToEdDDN(currentRow);
                }
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

                if (cmbLightYears.Text == "")
                    return false;

                dist = DistanceInLightYears(remoteSystemName);

                var limit = float.Parse(cmbLightYears.Text);
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

        private bool StationDistance(string SystemName, string StationName)
        {
            try
            {
                int? dist;

                if (cmbStationToStar.Text == "")
                    return false;

                dist = _Milkyway.getStationDistance(SystemName, StationName);

                if((!RegulatedNoiseSettings.IncludeUnknownDTS) && (dist == -1))
                    return false;

                var limit = int.Parse(cmbStationToStar.Text);
                if ((dist == null) || (dist < limit))
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

                _cachedSystemLocation = myMilkyway.getSystemCoordinates(localSystem);
            }


            remoteSystemName = remoteSystemName.ToUpper();

            if (_cachedRemoteSystemDistances.ContainsKey(remoteSystemName))
            {
                dist = _cachedRemoteSystemDistances[remoteSystemName];
            }
            else
            {
                if (!myMilkyway.existSystem(localSystem) || _cachedSystemLocation == null)
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

            return dist;
        }

        private double DistanceInLightYears(string remoteSystemName, Point3D currentSystemLocation)
        {
            double dist;

            Point3D remoteSystemLocation = myMilkyway.getSystemCoordinates(remoteSystemName);

            if (remoteSystemLocation == null)
                return double.MaxValue;

            double xDelta = currentSystemLocation.X - remoteSystemLocation.X;
            double yDelta = currentSystemLocation.Y - remoteSystemLocation.Y;
            double zDelta = currentSystemLocation.Z - remoteSystemLocation.Z;

            dist = Math.Sqrt(Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2) + Math.Pow(zDelta, 2));

            return dist;
        }
//        private Dictionary<string, Point3D>_chachedSystemLocations = new Dictionary<string, Point3D>();

        private double DistanceInLightYears(string remoteSystemName, string homeSystemName)
        {
            double retValue;

            Point3D HomeCoordinates = myMilkyway.getSystemCoordinates(homeSystemName);

            if (HomeCoordinates == null)
            { 
              return double.MaxValue;
            }

            retValue = DistanceInLightYears(remoteSystemName, HomeCoordinates);

            return retValue;
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

        private string CombinedNameToStationName(string combinedName)
        {
            var ret = combinedName.Substring(0, combinedName.IndexOf("[") - 1);
            return ret;
        }

        private string CombinedNameToSystemName(string combinedName)
        {
            try
            {
                var ret = combinedName.Substring(combinedName.IndexOf("[") + 1);
                ret = ret.TrimEnd(']');
                return ret;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

            
        }

        private void SetupGui(bool force= false)
        {
            System.Windows.Forms.Cursor oldCursor = Cursor;
            Cursor = Cursors.WaitCursor;

            //_cbIncludeWithinRegionOfStation_IndexChanged = false;

            if (!_InitDone && !force)
                return;

            _pt.startMeasuring();

            cmbStation.BeginUpdate();
            cmbStationToStationFrom.BeginUpdate();
            cmbStationToStationTo.BeginUpdate();
            cbCommodity.BeginUpdate();

            _pt.PrintAndReset("1");

            // notice the current selected items
            string Key_cmbStation               = getCmbItemKey(cmbStation.SelectedItem);
            string Key_cmbStationToStationFrom  = getCmbItemKey(cmbStationToStationFrom.SelectedItem);
            string Key_cmbStationToStationTo    = getCmbItemKey(cmbStationToStationTo.SelectedItem);

            _pt.PrintAndReset("2");

            BindingList<System.Collections.Generic.KeyValuePair<string,string>> BaseList;
            IFormatter formatter        = new BinaryFormatter();
            MemoryStream SerialListCopy = new MemoryStream();

            _pt.PrintAndReset("3");

            BaseList = getDropDownStationsItems(ref _StationIndices);

            formatter.Serialize(SerialListCopy, BaseList);

            _bs_Stations.DataSource = BaseList;
            SerialListCopy.Seek(0,0);
            _bs_StationsFrom.DataSource = (BindingList<System.Collections.Generic.KeyValuePair<string,string>>)formatter.Deserialize(SerialListCopy);
            SerialListCopy.Seek(0,0);
            _bs_StationsTo.DataSource = (BindingList<System.Collections.Generic.KeyValuePair<string,string>>)formatter.Deserialize(SerialListCopy);

            _pt.PrintAndReset("4");

            SerialListCopy.Dispose();

            if (!_InitDone)
            { 
                cmbStation.DataSource = _bs_Stations;
                cmbStation.DisplayMember = "Value";
                cmbStation.ValueMember = "Key";

                cmbStationToStationFrom.DataSource = _bs_StationsFrom;
                cmbStationToStationFrom.DisplayMember = "Value";
                cmbStationToStationFrom.ValueMember = "Key";

                cmbStationToStationTo.DataSource = _bs_StationsTo;
                cmbStationToStationTo.DisplayMember = "Value";
                cmbStationToStationTo.ValueMember = "Key";
            }


            _pt.PrintAndReset("5");
            cbIncludeWithinRegionOfStation.SelectedIndexChanged -= cbIncludeWithinRegionOfStation_SelectionChangeCommitted;

            var previouslySelectedValue = cbIncludeWithinRegionOfStation.SelectedItem;
            cbIncludeWithinRegionOfStation.Items.Clear();
            var systems = StationDirectory.Keys.Select(x => (object)(CombinedNameToSystemName(x))).OrderBy(x => x).Distinct().ToArray();
            cbIncludeWithinRegionOfStation.Items.Add("<Current System>");
            cbIncludeWithinRegionOfStation.Items.AddRange(systems);

            //cbIncludeWithinRegionOfStation.SelectedIndex = 0;
            cbIncludeWithinRegionOfStation.DropDownStyle = ComboBoxStyle.DropDownList;

            _pt.PrintAndReset("6");

            if (previouslySelectedValue != null)
                cbIncludeWithinRegionOfStation.SelectedItem = previouslySelectedValue;
            else
                cbIncludeWithinRegionOfStation.SelectedItem = "<Current System>";

            cbIncludeWithinRegionOfStation.SelectedIndexChanged += cbIncludeWithinRegionOfStation_SelectionChangeCommitted;

            int ListIndex;

            
            _pt.PrintAndReset("7");

            if ((Key_cmbStation != null) && _StationIndices.TryGetValue(Key_cmbStation, out ListIndex))
                cmbStation.SelectedIndex = ListIndex;

            if ((Key_cmbStation != null) && _StationIndices.TryGetValue(Key_cmbStationToStationFrom, out ListIndex))
                cmbStationToStationFrom.SelectedIndex = ListIndex;

            if ((Key_cmbStation != null) && _StationIndices.TryGetValue(Key_cmbStationToStationTo, out ListIndex))
                cmbStationToStationTo.SelectedIndex = ListIndex;

            
            cbCommodity.Items.Clear();

            _pt.PrintAndReset("8");

            foreach (var commodity in CommodityDirectory.OrderBy(x => x.Key))
            {
                cbCommodity.Items.Add(commodity.Key);
            }

            cbCommodity.SelectedItem = null;

            if (cbCommodity.Items.Count > 0)
                cbCommodity.SelectedItem = cbCommodity.Items[0];

            lvAllComms.Items.Clear();

            //_pt.PrintAndReset("9");

            Debug.Print("Anzahl = " + CommodityDirectory.Count.ToString());
            // Populate all commodities tab
            foreach (var commodity in CommodityDirectory)
            {
                decimal bestBuyPrice;
                decimal bestSellPrice;
                string bestBuy;
                string bestSell;
                decimal buyers;
                decimal sellers;

                //_pt.PrintAndReset("9_1");

                GetBestBuyAndSell(commodity.Key, out bestBuyPrice, out bestSellPrice, out bestBuy, out bestSell, out buyers, out sellers);

                //_pt.PrintAndReset("9_2");

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

                //_pt.PrintAndReset("9_3");
            }

            //_pt.PrintAndReset("10");

            cmbStation.EndUpdate();
            cmbStationToStationFrom.EndUpdate();
            cmbStationToStationTo.EndUpdate();
            cbCommodity.EndUpdate();
            //_pt.PrintAndReset("11");

            UpdateStationToStation();
            //_pt.PrintAndReset("12");

            Cursor = oldCursor;
        }


        private void setupCombobox(ComboBox CBRefreshed, List<KeyValuePair<string, string>> DDItems)
        {
            CBRefreshed.DataSource = null;

            CBRefreshed.Items.Clear();

            CBRefreshed.DataSource = DDItems;

            CBRefreshed.ValueMember = "Key";
            CBRefreshed.DisplayMember = "Value";

            if (CBRefreshed.Items.Count > 0)
                CBRefreshed.SelectedItem = CBRefreshed.Items[0];
            else
                CBRefreshed.SelectedItem = null;

            CBRefreshed.Refresh();
        }

        private int GetTextLengthInPixels(string myText)
        {
                return TextRenderer.MeasureText(myText, cmbStation.Font).Width;
        }

        private BindingList<System.Collections.Generic.KeyValuePair<string,string>> getDropDownStationsItems(ref Dictionary<string, int> StationIndices)
        {
            int SpaceWidth = GetTextLengthInPixels("                    ")/20;
            int maxLength = 0;
            int Spaces= 0;
            int last_i = 0;
            List<int> LengthInfo1 = new List<int>();
            List<int> LengthInfo2 = new List<int>();

            // clear the old index list
            StationIndices.Clear();

            BindingList<System.Collections.Generic.KeyValuePair<string,string>> DDItems = new BindingList<System.Collections.Generic.KeyValuePair<string,string>>();
            List<KeyValuePair<string,List<CsvRow>>> SelectionRaw;
            List<KeyValuePair<string,List<CsvRow>>> SelectionOrdered         = new List<KeyValuePair<string,List<CsvRow>>>();
            List<KeyValuePair<string,List<CsvRow>>> SelectionPreordered;

            // get the relevant stations
            SelectionRaw = StationDirectory.Where(x => getStationSelection(x)).ToList();
            
            if (rbSortBySystem.Checked)
            {
                // get the list ordered as wanted -> order by system
                SelectionPreordered = SelectionRaw.OrderBy(x => CombinedNameToSystemName(x.Key)).ThenBy(x => CombinedNameToStationName(x.Key)).ToList();

                if (cblastVisitedFirst.Checked)
                {
                    getVisitedListPart(ref maxLength, LengthInfo1, SelectionOrdered, SelectionPreordered);
                }


                // be aware of the length of each string in the remaining list
                for (int i = 0; i < SelectionPreordered.Count(); i++)
                {
                    int tempLength;
                    try
                    {
                        tempLength = GetTextLengthInPixels(SelectionPreordered[i].Value[0].SystemName);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("error while getting text length in pixels", ex);    
                    }
                    if (maxLength < tempLength)
                    { 
                        if(MAX_NAME_LENGTH < tempLength)
                            tempLength = MAX_NAME_LENGTH;
                        maxLength = tempLength;
                    }
                        
                    LengthInfo2.Add(tempLength);
                }

                last_i = 0;

                if (cblastVisitedFirst.Checked)
                { 
                    // insert get the visited (lengths are in LengthInfo1)
                    for (int i = 0; i < SelectionOrdered.Count(); i++)
                    {
                        Spaces = (int)Math.Ceiling((Double)(maxLength - LengthInfo1[i]) / (Double)SpaceWidth);
                        DDItems.Add(new KeyValuePair<string, string>(SelectionOrdered[i].Key,String.Format("{0}{2}     {1}", SelectionOrdered[i].Value[0].SystemName, SelectionOrdered[i].Value[0].StationName, "".PadLeft(Spaces))));
                        StationIndices.Add(SelectionOrdered[i].Key, i);

                        last_i = i+1;
                    }
                
                    // insert separator
                    DDItems.Add(new KeyValuePair<string, string>(ID_DELIMITER,String.Format("-----------------------")));
                    StationIndices.Add(ID_DELIMITER, last_i);
                    LengthInfo1.Add(0);
                    last_i++;
                }

                // insert get the visited (lengths are in LengthInfo1)
                for (int i = 0; i < SelectionPreordered.Count(); i++)
                {
                    Spaces = (int)Math.Ceiling((Double)(maxLength - LengthInfo2[i]) / (Double)SpaceWidth);
                    DDItems.Add(new KeyValuePair<string, string>(SelectionPreordered[i].Key,String.Format("{0}{2}     {1}", SelectionPreordered[i].Value[0].SystemName, SelectionPreordered[i].Value[0].StationName, "".PadLeft(Spaces))));
                    StationIndices.Add(SelectionPreordered[i].Key, i+last_i);
                }
            }
            else if (rbSortByStation.Checked)
            {
                // get the list ordered as wanted -> order by station
                SelectionPreordered = SelectionRaw.OrderBy(x => CombinedNameToStationName(x.Key)).ToList();;

                if (cblastVisitedFirst.Checked)
                {
                    getVisitedListPart(ref maxLength, LengthInfo1, SelectionOrdered, SelectionPreordered);
                }

                // be aware of the length of each string in the remaining list
                for (int i = 0; i < SelectionPreordered.Count(); i++)
                { 
                    int tempLength = GetTextLengthInPixels(SelectionPreordered[i].Value[0].StationName);
                    if (maxLength < tempLength)
                    { 
                        if(MAX_NAME_LENGTH < tempLength)
                            tempLength = MAX_NAME_LENGTH;
                        maxLength = tempLength;
                    }
                    LengthInfo2.Add(tempLength);
                }

                last_i = 0;

                if (cblastVisitedFirst.Checked)
                { 
                    // insert get the visited (lengths are in LengthInfo1)
                    for (int i = 0; i < SelectionOrdered.Count(); i++)
                    {
                        Spaces = (int)Math.Ceiling((Double)(maxLength - LengthInfo1[i]) / (Double)SpaceWidth);
                        DDItems.Add(new KeyValuePair<string, string>(SelectionOrdered[i].Key,String.Format("{1}{2}     {0}", SelectionOrdered[i].Value[0].SystemName, SelectionOrdered[i].Value[0].StationName, "".PadLeft(Spaces))));
                        StationIndices.Add(SelectionOrdered[i].Key, i);

                        last_i = i+1;
                    }
                
                    // insert separator
                    DDItems.Add(new KeyValuePair<string, string>(ID_DELIMITER,String.Format("-----------------------")));
                    StationIndices.Add(ID_DELIMITER, last_i);
                    LengthInfo1.Add(0);
                    last_i++;
                }

                // insert get the visited (lengths are in LengthInfo1)
                for (int i = 0; i < SelectionPreordered.Count(); i++)
                {
                    Spaces = (int)Math.Ceiling((Double)(maxLength - LengthInfo2[i]) / (Double)SpaceWidth);
                    DDItems.Add(new KeyValuePair<string, string>(SelectionPreordered[i].Key,String.Format("{1}{2}     {0}", SelectionPreordered[i].Value[0].SystemName, SelectionPreordered[i].Value[0].StationName, "".PadLeft(Spaces))));
                    StationIndices.Add(SelectionPreordered[i].Key, i+last_i);
                }
            }
            else if (rbSortByDistance.Checked)
            {
                // get the list ordered as wanted -> order by distance 
                SelectionPreordered = SelectionRaw.OrderBy(x => DistanceInLightYears(CombinedNameToSystemName(x.Key))).ToList();

                if (cblastVisitedFirst.Checked)
                {
                    getVisitedListPart(ref maxLength, LengthInfo1, SelectionOrdered, SelectionPreordered);
                }

                // be aware of the length of each string in the remaining list
                for (int i = 0; i < SelectionPreordered.Count(); i++)
                { 
                    int tempLength = GetTextLengthInPixels(SelectionPreordered[i].Value[0].SystemName);
                    if (maxLength < tempLength)
                    { 
                        if(MAX_NAME_LENGTH < tempLength)
                            tempLength = MAX_NAME_LENGTH;
                        maxLength = tempLength;
                    }
                    LengthInfo2.Add(tempLength);
                }

                last_i = 0;

                if (cblastVisitedFirst.Checked)
                { 
                    // insert get the visited (lengths are in LengthInfo1)
                    for (int i = 0; i < SelectionOrdered.Count(); i++)
                    {
                        Spaces = (int)Math.Ceiling((Double)(maxLength - LengthInfo1[i]) / (Double)SpaceWidth);
                        DDItems.Add(new KeyValuePair<string, string>(SelectionOrdered[i].Key,String.Format("{0}{2}     \t{1}", SelectionOrdered[i].Value[0].SystemName, SelectionOrdered[i].Value[0].StationName, "".PadLeft(Spaces))));
                        StationIndices.Add(SelectionOrdered[i].Key, i);

                        last_i = i+1;
                    }
                
                    // insert separator
                    DDItems.Add(new KeyValuePair<string, string>(ID_DELIMITER,String.Format("-----------------------")));
                    StationIndices.Add(ID_DELIMITER, last_i);
                    LengthInfo1.Add(0);
                    last_i++;
                }

                // insert get the visited (lengths are in LengthInfo1)
                for (int i = 0; i < SelectionPreordered.Count(); i++)
                {
                    Spaces = (int)Math.Ceiling((Double)(maxLength - LengthInfo2[i]) / (Double)SpaceWidth);
                    DDItems.Add(new KeyValuePair<string, string>(SelectionPreordered[i].Key,String.Format("{0}{2}     \t{1}", SelectionPreordered[i].Value[0].SystemName, SelectionPreordered[i].Value[0].StationName, "".PadLeft(Spaces))));
                    StationIndices.Add(SelectionPreordered[i].Key, i+last_i);
                }
            }
            
            return DDItems;
            
        }

        /// <summary>
        /// get the Stations which are visited the last time and remove them from the base list
        /// </summary>
        /// <param name="maxLength"></param>
        /// <param name="LengthInfo1"></param>
        /// <param name="SelectionOrdered"></param>
        /// <param name="SelectionPreordered"></param>
        private void getVisitedListPart(ref int maxLength, List<int> LengthInfo1, List<KeyValuePair<string, List<CsvRow>>> SelectionOrdered, List<KeyValuePair<string, List<CsvRow>>> SelectionPreordered)
        {
            int lastVisitCount = int.Parse(txtlastStationCount.Text);

            for (int i = 0; (i < _StationHistory.History.Count) && (SelectionOrdered.Count < lastVisitCount); i++)
            {
                int foundIndex = SelectionPreordered.FindIndex(x => x.Key.Equals(_StationHistory.History[i].Station, StringComparison.InvariantCultureIgnoreCase));

                if (foundIndex >= 0)
                {
                    int tempLength=0;

                    // put the found item in the lastvisited list
                    SelectionOrdered.Add(SelectionPreordered[foundIndex]);

                    // be aware of the length of each string
                    if (rbSortBySystem.Checked)
                        tempLength = GetTextLengthInPixels(SelectionPreordered[foundIndex].Value[0].SystemName);
                    else if(rbSortByStation.Checked)
                        tempLength = GetTextLengthInPixels(SelectionPreordered[foundIndex].Value[0].StationName);
                    else if(rbSortByDistance.Checked)
                        tempLength = GetTextLengthInPixels(SelectionPreordered[foundIndex].Value[0].SystemName);
                    
                    if (maxLength < tempLength)
                    { 
                        if(MAX_NAME_LENGTH < tempLength)
                            tempLength = MAX_NAME_LENGTH;
                        maxLength = tempLength;
                    }
                    LengthInfo1.Add(tempLength);

                    // remove item from preordered list
                    SelectionPreordered.RemoveAt(foundIndex);

                }
            }
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
            var selectedItem = cmbStation.SelectedItem;

            if (selectedItem != null)
            { 
                var dist = DistanceInLightYears(CombinedNameToSystemName(getCmbItemKey(selectedItem)));

                if (dist < double.MaxValue)
                    lblLightYearsFromCurrentSystem.Text = "(" + String.Format("{0:0.00}", dist) + " light years)";
                else
                    lblLightYearsFromCurrentSystem.Text = "(system location unknown)";

                lbPrices.Items.Clear();
                var stationName =  getCmbItemKey(((ComboBox)sender).SelectedItem); 

                if (stationName != ID_DELIMITER)
                { 
                    var start = stationName.IndexOf("[", StringComparison.Ordinal);
                    var end = stationName.IndexOf("]", StringComparison.Ordinal);

                    tbStationRename.Text = stationName.Substring(0, start - 1);
                    tbSystemRename.Text = stationName.Substring(start + 1, end - (start + 1));

                    foreach (var row in StationDirectory[stationName])
                    {
                        decimal bestBuyPrice;
                        decimal bestSellPrice;
                        string bestBuy;
                        string bestSell;
                        decimal buyers;
                        decimal sellers;
                       
                        _pt.stopMeasuring();

                        GetBestBuyAndSell(row.CommodityName, out bestBuyPrice, out bestSellPrice, out bestBuy, out bestSell, out buyers, out sellers);
                        
                        _pt.PrintAndReset(row.CommodityName);

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
                            row.SampleDate.ToString(CultureInfo.CurrentCulture) ,
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

                    cmdApplySystemRename.Enabled = true;

                }
                else
                {
                    tbStationRename.Text = String.Empty;
                    tbSystemRename.Text  = String.Empty;
                    cmdApplySystemRename.Enabled = false;
                    lbPrices.Items.Clear();
                }
            }
            else
            {
                tbStationRename.Text = String.Empty;
                tbSystemRename.Text  = String.Empty;
                cmdApplySystemRename.Enabled = false;
                lbPrices.Items.Clear();
            }
        }

        private void GetBestBuyAndSell(string commodityName, out decimal bestBuyPrice, out decimal bestSellPrice, out string bestBuy, out string bestSell, out decimal buyers, out decimal sellers)
        {
            bestBuyPrice = 0;
            bestSellPrice = 0;

            bestBuy = "";
            bestSell = "";

            var l = CommodityDirectory[commodityName].Where(x => x.Supply != 0 && x.BuyPrice != 0).Where(x => getStationSelection(x, !_InitDone)).ToList();
            buyers = l.Count();

            if (l.Count() != 0)
            {
                bestBuyPrice = l.Min(y => y.BuyPrice);
                var bestBuyPriceCopy = bestBuyPrice;
                bestBuy = string.Join(" ", l.Where(x => x.BuyPrice == bestBuyPriceCopy).Select(x => x.StationID + " (" + x.BuyPrice + ")"));
            }

            var m = CommodityDirectory[commodityName].Where(x => x.SellPrice != 0 && x.Demand != 0).Where(x => getStationSelection(x, !_InitDone)).ToList();
            sellers = m.Count();
            if (m.Count() != 0)
            {
                bestSellPrice = m.Max(y => y.SellPrice);
                var bestSellPriceCopy = bestSellPrice;
                bestSell = string.Join(" ", m.Where(x => x.SellPrice == bestSellPriceCopy).Select(x => x.StationID + " (" + x.SellPrice + ")"));
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
            var selectedCmbItem = ((ComboBox)sender).SelectedItem;
            lbCommodities.Items.Clear();

            if (selectedCmbItem != null)
            { 
                foreach (var row in CommodityDirectory[(selectedCmbItem.ToString())].Where(x => getStationSelection(x)))
                {
                    lbCommodities.Items.Add(new ListViewItem(new[] 
                    {   row.StationID,
                        row.SellPrice.ToString(CultureInfo.InvariantCulture) != "0" ? row.SellPrice.ToString(CultureInfo.InvariantCulture) : "",
                        row.BuyPrice.ToString(CultureInfo.InvariantCulture) != "0" ? row.BuyPrice.ToString(CultureInfo.InvariantCulture) : "",
                        row.Demand.ToString(CultureInfo.InvariantCulture) != "0" ? row.Demand.ToString(CultureInfo.InvariantCulture) : "",
                        row.DemandLevel,
                        row.Supply.ToString(CultureInfo.InvariantCulture) != "0" ? row.Supply.ToString(CultureInfo.InvariantCulture) : "",
                        row.SupplyLevel,
                        row.SampleDate.ToString(CultureInfo.CurrentCulture) 
                    }));
                }

                var l = CommodityDirectory[(selectedCmbItem.ToString())].Where(x => x.BuyPrice != 0 && x.Supply > 0).Where(x => getStationSelection(x)).ToList();
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

                l = CommodityDirectory[(selectedCmbItem.ToString())].Where(x => x.SellPrice != 0 && x.Demand > 0).Where(x => getStationSelection(x)).ToList();
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
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.BuyPrice != 0).Where(x => getStationSelection(x)).ToList();
                var m = l.Where(x => x.BuyPrice == l.Min(y => y.BuyPrice));
                MsgBox.Show(string.Join(", ", m.Select(x => x.StationID)));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lblMinSell.Text != "N/A")
            {
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.SellPrice != 0).Where(x => getStationSelection(x)).ToList();
                var m = l.Where(x => x.SellPrice == l.Min(y => y.SellPrice));
                MsgBox.Show(string.Join(", ", m.Select(x => x.StationID)));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lblMax.Text != "N/A")
            {
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.BuyPrice != 0).Where(x => getStationSelection(x)).ToList();
                var m = l.Where(x => x.BuyPrice == l.Max(y => y.BuyPrice));
                MsgBox.Show(string.Join(", ", m.Select(x => x.StationID)));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lblMaxSell.Text != "N/A")
            {
                var l = CommodityDirectory[cbCommodity.SelectedItem.ToString()].Where(x => x.SellPrice != 0).Where(x => getStationSelection(x)).ToList();
                var m = l.Where(x => x.SellPrice == l.Max(y => y.SellPrice));
                MsgBox.Show(string.Join(", ", m.Select(x => x.StationID)));
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

            foreach (var price in CommodityDirectory[senderName].Where(x => x.BuyPrice != 0 && x.Supply != 0).Where(x => getStationSelection(x)).OrderBy(x => x.BuyPrice))
            {
                series1.Points.AddXY(price.StationID, price.BuyPrice);
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

            foreach (var price in CommodityDirectory[senderName].Where(x => x.SellPrice != 0 && x.Demand != 0).Where(x => getStationSelection(x)).OrderByDescending(x => x.SellPrice))
            {
                series2.Points.AddXY(price.StationID, price.SellPrice);
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
            var existingStationName = getCmbItemKey(cmbStation.SelectedItem);

            tbStationRename.Text    = _textInfo.ToTitleCase(tbStationRename.Text.ToLower());

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
                    StationName = tbStationRename.Text,
                    StationID = newStationName,
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
                        StationID = row.StationID == existingStationName ? newStationName : row.StationID,
                        Supply = row.Supply,
                        SupplyLevel = row.SupplyLevel,
                        SystemName = row.StationID == existingStationName ? tbSystemRename.Text : row.SystemName
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
                var distinctStationNames = CommodityDirectory.ElementAt(i).Value.Select(x => x.StationID).Distinct();
                foreach (var d in distinctStationNames)
                {
                    if (!newCommodityDirectory2.Keys.Contains(CommodityDirectory.ElementAt(i).Key))
                        newCommodityDirectory2.Add(CommodityDirectory.ElementAt(i).Key, new List<CsvRow> { CommodityDirectory.ElementAt(i).Value.ToList().Where(x => x.StationID == d).OrderByDescending(x => x.SampleDate).First() });
                    else newCommodityDirectory2[CommodityDirectory.ElementAt(i).Key].Add(CommodityDirectory.ElementAt(i).Value.ToList().Where(x => x.StationID == d).OrderByDescending(x => x.SampleDate).First());
                }
            }

            CommodityDirectory = newCommodityDirectory2;

            _StationHistory.RenameStation(existingStationName, newStationName);

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

                sws.Start(ip, Int32.Parse(txtWebserverPort.Text) , 5, "", this);
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
                MsgBox.Show(
                    "Couldn't start webserver.  Maybe something is already using port 8080...?");
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            sws.Stop();
            UpdateUrl();
        }

        public delegate string EventArgsDelegate();

        private System.String getCmbItemKey(object Item)
        {
            if (Item != null)
                return ((KeyValuePair<string, string>)Item).Key;
            else
                return null;
        }

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

            s.Append("<A name=\"lbPrices\"><P>Station: " + getCmbItemKey(cmbStation.SelectedItem) + "</P>");
            s.Append(GetHTMLForListView(lbPrices));

            s.Append(links);

            s.Append("<A name=\"lbCommodities\"><P>Commodity: " + cbCommodity.SelectedItem + "</P>");
            s.Append(GetHTMLForListView(lbCommodities));

            s.Append(links);

            s.Append("<A name=\"lvStationToStation\"><P>Station-to-Station: " + getCmbItemKey(cmbStationToStationFrom.SelectedItem) + " => " + getCmbItemKey(cmbStationToStationTo.SelectedItem) + "</P>");
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
            Int32 intPort = 0;

            if(String.IsNullOrEmpty(txtWebserverPort.Text) || !Int32.TryParse(txtWebserverPort.Text, out intPort))
                intPort = 8080;

            txtWebserverPort.Text = intPort.ToString();

            if (cbInterfaces.SelectedItem != null) 
            {
                lblURL.Text = "http://" + cbInterfaces.SelectedItem + ":" + intPort;
            }

            else lblURL.Text = "Set Interface...";

            if (sws != null && sws.Running) lblURL.ForeColor = Color.Blue;
            else lblURL.ForeColor = Color.Black;

            if (cbInterfaces.SelectedItem != null) 
            {
                RegulatedNoiseSettings.WebserverIpAddress = cbInterfaces.SelectedItem.ToString();
            }

            RegulatedNoiseSettings.WebserverPort = txtWebserverPort.Text;

        }

        void txtWebserverPort_LostFocus(object sender, System.EventArgs e)
        {
            UpdateUrl();
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
                MsgBox.Show("You need to calibrate first.  Go to the OCR Calibration tab to do so...");
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

            try
            {
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
            catch (Exception ex)
            {
                cErr.processError(ex);
            }
        }

        public delegate Bitmap ReturnTrimmedImageDelegate();

        public Bitmap ReturnTrimmedImage()
        {
            if (InvokeRequired)
            {
                return (Bitmap)(Invoke(new ReturnTrimmedImageDelegate(ReturnTrimmedImage)));
            }

            try
            {
                if (pbOcrCurrent.Image == null) return null;

                return (Bitmap)(pbOcrCurrent.Image.Clone());
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
                return null;
            }
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
                Int32 currentTry=0;
                bool Retry = false;

                do
                {
                    Retry = false;
                    try
                    {
                        Invoke(new DisplayResultsDelegate(DisplayResults), s);
                        return;
                    }
                    catch (Exception ex)
                    {
                        if (currentTry >= 3)
                            throw ex;
                        else
                        { 
                            Thread.Sleep(333);
                            currentTry++;
                            Retry = true;
                        }
                    }
                } while (Retry);
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
            bContinueOcr.Text = "C&ontinue";
            bClearOcrOutput.Enabled = false;
            bEditResults.Enabled = false;
            tbFinalOcrOutput.Enabled = false;
            ContinueDisplayingResults();
        }

        private void ContinueDisplayingResults()
        {

            try
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

                            if (_commodityTexts[_correctionRow, _correctionColumn].Equals("Getreide", StringComparison.InvariantCultureIgnoreCase))
                                Debug.Print("STOP");

                            if (_commoditiesSoFar.Contains(_commodityTexts[_correctionRow, _correctionColumn].ToUpper()))
                            {
                                _commodityTexts[_correctionRow, _correctionColumn] = "";
                                _originalBitmapConfidences[_correctionRow, _correctionColumn] = 1;
                            }

                            // If we're doing a batch of screenshots, don't keep doing the same commodity when we keep finding it
                            // but only if it's sure - otherwise it will be registered later
                            if (_originalBitmapConfidences[_correctionRow, _correctionColumn] == 1)
                            {
                                _commoditiesSoFar.Add(_commodityTexts[_correctionRow, _correctionColumn].ToUpper());
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

                        var levenshteinLow = _levenshtein.LD2(CommodityLevel[(byte)enCommodityLevel.LOW].ToUpper(), commodityLevelUpperCase);
                        var levenshteinMed = _levenshtein.LD2(CommodityLevel[(byte)enCommodityLevel.MED].ToUpper(), commodityLevelUpperCase);
                        var levenshteinHigh = _levenshtein.LD2(CommodityLevel[(byte)enCommodityLevel.HIGH].ToUpper(), commodityLevelUpperCase);
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

                            finalOutput += _rowIds[row] + "\r\n";
                        }
                    }

                    _csvOutputSoFar += finalOutput;

                    if (pbOriginalImage.Image != null)
                        pbOriginalImage.Image.Dispose();

                    UpdateOriginalImage(null);
                    UpdateTrimmedImage(null, null);

                    if (RegulatedNoiseSettings.DeleteScreenshotOnImport)
                        File.Delete(_screenshotName);

                    Acquisition();

                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
            }
        }

        private void Acquisition(bool noAutoImport = false)
        {

            if (_screenshotResultsBuffer.Count == 0)
            {
                tbFinalOcrOutput.Text += _csvOutputSoFar;
                _csvOutputSoFar = null;


                pbOcrCurrent.Image = null;
                if (_preOcrBuffer.Count == 0 && ocr.ScreenshotBuffer.Count == 0)
                {

                    if (checkPricePlausibility(tbFinalOcrOutput.Text.Replace("\r", "").Split('\n')))
                    {
                        tbFinalOcrOutput.Enabled = true;
                        tbCommoditiesOcrOutput.Text = "Implausible Results!";
                        bContinueOcr.Text = "Check Implausible";
                        bContinueOcr.Enabled = true;
                        bIgnoreTrash.Enabled = false;
                        bClearOcrOutput.Enabled = true;
                        bEditResults.Enabled = true;
                    }
                    else
                    {
                        tbFinalOcrOutput.Enabled = true;

                        if ((!noAutoImport) && RegulatedNoiseSettings.AutoImport)
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
                            bContinueOcr.Text = "Imp&ort";
                            bContinueOcr.Enabled = true;
                            bIgnoreTrash.Enabled = false;
                            bClearOcrOutput.Enabled = true;
                            bEditResults.Enabled = true;
                        }
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

            try
            {
                pbOcrCurrent.Image = (Bitmap)(b.Clone());
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
            }

        }
        #endregion

        private void bContinueOcr_Click(object sender, EventArgs e)
        {
            Boolean isOK = false;
            Boolean finished = false;
            DialogResult Answer;
            string commodity;


            commodity = _textInfo.ToTitleCase(tbCommoditiesOcrOutput.Text.ToLower().Trim());
            if (commodity.ToUpper() == "Implausible Results!".ToUpper())
            {
                // check results
                var f = new EditOcrResults(tbFinalOcrOutput.Text);
                f.onlyImplausible = true;
                var q = f.ShowDialog();

                if (q == DialogResult.OK)
                {
                    tbFinalOcrOutput.Text = f.ReturnValue;
                }

                Acquisition(true);

                isOK = false;
            }
            else if (commodity.ToUpper() == "Imported!".ToUpper() || commodity.ToUpper() == "Finished!".ToUpper() || commodity.ToUpper() == "No rows found...".ToUpper())
            {
                // its the end
                isOK = true;
                finished = true;
            }
            else if (commodity.Length == 0 || KnownCommodityNames.Contains(commodity))
            {
                // ok, no typing error
                isOK = true;
            }
            else
            {
                // unknown commodity, is it a new one or a typing error ?
                Answer = MsgBox.Show(String.Format("Do you want to add '{0}' to the known commodities ?", commodity), "Unknown commodity !",
                                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (Answer == System.Windows.Forms.DialogResult.OK)
                {
                    // yes, it's really new
                    addCommodity(commodity, RegulatedNoiseSettings.Language);
                    isOK = true;
                }
            }

            if (isOK)
            {
                if (_commodityTexts == null || _correctionColumn >= _commodityTexts.GetLength(1) || finished)
                {
                    if (MsgBox.Show("Import this?", "Import?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ImportFinalOcrOutput();
                        tbFinalOcrOutput.Text = "";
                        bContinueOcr.Enabled = false;
                        bIgnoreTrash.Enabled = false;
                        _commoditiesSoFar = new List<string>();
                        bClearOcrOutput.Enabled = false;
                        bEditResults.Enabled = false;

                        CommandersLog_MarketDataCollectedEvent(tbCurrentSystemFromLogs.Text, tbCurrentStationinfoFromLogs.Text);

                        // save the new data immediately
                        SaveCommodityData(true);


                    }
                }
                else
                {
                    _commodityTexts[_correctionRow, _correctionColumn] = commodity.ToUpper();
                    _commoditiesSoFar.Add(_commodityTexts[_correctionRow, _correctionColumn].ToUpper());

                    ContinueDisplayingResults();
                }
            }

        }

        public bool checkPricePlausibility(string[] DataRows, bool simpleEDDNCheck = false)
        {
            bool implausible = false;

            foreach (string s in DataRows)
            {
                if (s.Contains(";"))
                {
                    string[] values = s.Split(';');
                    CsvRow currentRow = new CsvRow();

                    currentRow.SellPrice    = -1;
                    currentRow.BuyPrice     = -1;
                    currentRow.Demand       = -1;
                    currentRow.Supply       = -1;

                    currentRow.SystemName       = values[0];
                    currentRow.StationName      = _textInfo.ToTitleCase(values[1].ToLower());
                    currentRow.StationID        = _textInfo.ToTitleCase(values[1].ToLower()) + " [" + currentRow.SystemName + "]";
                    currentRow.CommodityName    = _textInfo.ToTitleCase(values[2].ToLower());

                    if (!String.IsNullOrEmpty(values[3]))
                        Decimal.TryParse(values[3], out currentRow.SellPrice);
                    if (!String.IsNullOrEmpty(values[4]))
                        Decimal.TryParse(values[4], out currentRow.BuyPrice);
                    if (!String.IsNullOrEmpty(values[5]))
                        Decimal.TryParse(values[5], out currentRow.Demand);
                    if (!String.IsNullOrEmpty(values[7]))
                        Decimal.TryParse(values[7], out currentRow.Supply);

                    currentRow.DemandLevel      = _textInfo.ToTitleCase(values[6].ToLower());
                    currentRow.SupplyLevel      = _textInfo.ToTitleCase(values[8].ToLower());

                    DateTime.TryParse(values[9], out currentRow.SampleDate);

                    EDCommoditiesExt CommodityData = myMilkyway.getCommodity(getCommodityBasename(RegulatedNoiseSettings.Language, currentRow.CommodityName));
    
                    if (currentRow.CommodityName == "Panik")
                        Debug.Print("STOP");
                            
                    if (CommodityData != null)
                    { 
                        if ((!String.IsNullOrEmpty(currentRow.SupplyLevel)) && (!String.IsNullOrEmpty(currentRow.DemandLevel)))
                        {
                            // demand AND supply !?
                            implausible = true;
                        }
                        else if ((!String.IsNullOrEmpty(currentRow.SupplyLevel)) || (simpleEDDNCheck && (currentRow.Supply > 0)))
                        { 
                            // check supply data             

                            if ((currentRow.SellPrice <= 0) || (currentRow.BuyPrice <= 0))
                            { 
                                // both on 0 is not plausible
                                implausible = true;
                            }

                            if (((CommodityData.PriceWarningLevel_Supply_Sell_Low  >= 0) && (currentRow.SellPrice < CommodityData.PriceWarningLevel_Supply_Sell_Low)) ||
                                ((CommodityData.PriceWarningLevel_Supply_Sell_High >= 0) && (currentRow.SellPrice > CommodityData.PriceWarningLevel_Supply_Sell_High)))
                            {
                                // sell price is out of range
                                implausible = true;
                            }

                            if (((CommodityData.PriceWarningLevel_Supply_Buy_Low  >= 0) && (currentRow.BuyPrice  < CommodityData.PriceWarningLevel_Supply_Buy_Low)) ||
                                ((CommodityData.PriceWarningLevel_Supply_Buy_High >= 0) && (currentRow.SellPrice > CommodityData.PriceWarningLevel_Supply_Buy_High)))
                            {
                                // buy price is out of range
                                implausible = true;
                            }

                            if (currentRow.Supply.Equals(-1))
                            {   
                                // no supply quantity
                                implausible = true;
                            }

                        }
                        else if ((!String.IsNullOrEmpty(currentRow.DemandLevel)) || (simpleEDDNCheck && (currentRow.Demand > 0)))
                        { 
                            // check demand data

                            if (currentRow.SellPrice <= 0)
                            {
                                // at least the sell price must be present
                                implausible = true;
                            }

                            if (((CommodityData.PriceWarningLevel_Demand_Sell_Low  >= 0) && (currentRow.SellPrice < CommodityData.PriceWarningLevel_Demand_Sell_Low)) ||
                                ((CommodityData.PriceWarningLevel_Demand_Sell_High >= 0) && (currentRow.SellPrice > CommodityData.PriceWarningLevel_Demand_Sell_High)))
                            {
                                // buy price is out of range
                                implausible = true;
                            }

                            if (currentRow.BuyPrice >= 0) 
                                if (((CommodityData.PriceWarningLevel_Demand_Buy_Low  >= 0) && (currentRow.BuyPrice < CommodityData.PriceWarningLevel_Demand_Buy_Low)) ||
                                    ((CommodityData.PriceWarningLevel_Demand_Buy_High >= 0) && (currentRow.BuyPrice > CommodityData.PriceWarningLevel_Demand_Buy_High)))
                                {
                                    // buy price is out of range
                                    implausible = true;
                                }

                            if (currentRow.Demand.Equals(-1))
                            {
                                // no supply quantity
                                implausible = true;
                            }
                        }
                        else
                        { 
                            // nothing ?!
                            implausible = true;
                        }
                    }
                }

                if (implausible)
                    break;
            }

            return implausible;
        }



        private void ImportFinalOcrOutput()
        {
            foreach (var s in tbFinalOcrOutput.Text.Replace("\r", "").Split('\n'))
            {
                if (s.Contains(";"))
                {
                    
                    ImportCsvString(s, false, true, true);
                }
            }

            SetupGui();
        }

        private string _oldOcrName;

        private void tbOcrStationName_TextChanged(object sender, EventArgs e)
        {
            if (tbOcrStationName.Text != _oldOcrName && _oldOcrName != null)
            {
                var rows = tbFinalOcrOutput.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string newRows = "";

                foreach (var row in rows)
                {
                    var newRow1 = row.Substring(0, row.IndexOf(";"));
                    var newRow2 = tbOcrStationName.Text;
                    var newRow3 = row.Substring(row.IndexOf(";", 1));
                    newRow3 = newRow3.Substring(newRow3.IndexOf(";", 1));
                    newRows = newRows + newRow1 + ";" + newRow2 + newRow3 + "\r\n";

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
                var rows = tbFinalOcrOutput.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string newRows = "";

                foreach (var row in rows)
                {
                    var newRow1 = row.Substring(row.IndexOf(";"));
                    newRows += tbOcrSystemName.Text + newRow1 + "\r\n";

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
            try
            {
                CommandersLog.LoadLog();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while loading Commanders Log");
            }
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
            _eddnSubscriberThread = new Thread(() => Eddn.Subscribe());
            _eddnSubscriberThread.IsBackground = true;
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

                if (cbSpoolEddnToFile.Checked)
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

                var headerDictionary    = new Dictionary<string, string>();
                var messageDictionary   = new Dictionary<string, string>();

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

            if (txt != "")
                try
                {
                    // ReSharper disable StringIndexOfIsCultureSpecific.1
                    var headerRawStart = txt.IndexOf(@"""header""") + 12;
                    var headerRawLength = txt.Substring(headerRawStart).IndexOf("}");
                    var headerRawData = txt.Substring(headerRawStart, headerRawLength);

                    var schemaRawStart = txt.IndexOf(@"""$schemaRef""") + 14;
                    var schemaRawLength = txt.Substring(schemaRawStart).IndexOf(@"""message"":");
                    var schemaRawData = txt.Substring(schemaRawStart, schemaRawLength);

                    var messageRawStart = txt.IndexOf(@"""message"":") + 12;
                    var messageRawLength = txt.Substring(messageRawStart).IndexOf("}");
                    var messageRawData = txt.Substring(messageRawStart, messageRawLength);
                    // ReSharper restore StringIndexOfIsCultureSpecific.1

                    schemaRawData = schemaRawData.Replace(@"""", "").Replace(",","");
                    var headerRawPairs = headerRawData.Replace(@"""", "").Split(',');
                    var messageRawPairs = messageRawData.Replace(@"""", "").Split(',');


                    if((RegulatedNoiseSettings.UseEddnTestSchema  && (schemaRawData.IndexOf("Test", StringComparison.InvariantCultureIgnoreCase) >= 0)) ||
                       (!RegulatedNoiseSettings.UseEddnTestSchema && (schemaRawData.IndexOf("Test", StringComparison.InvariantCultureIgnoreCase)  < 0)))
                    {
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

                        string commodity = getLocalizedCommodity(RegulatedNoiseSettings.Language, messageDictionary["itemName"]);

                        if(!String.IsNullOrEmpty(commodity))
                        {

                            //System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;
                            if (import && headerDictionary["uploaderID"] != tbUsername.Text) // Don't import our own uploads...
                            {
                                string csvFormatted = messageDictionary["systemName"] + ";" +
                                                      messageDictionary["stationName"] + ";" +
                                                      commodity + ";" +
                                                      (messageDictionary["sellPrice"] == "0" ? "" : messageDictionary["sellPrice"]) + ";" +
                                                      (messageDictionary["buyPrice"] == "0" ? "" : messageDictionary["buyPrice"]) + ";" +
                                                      messageDictionary["demand"] + ";" +
                                                      ";" +
                                                      messageDictionary["stationStock"] + ";" +
                                                      ";" +
                                                      messageDictionary["timestamp"] + ";"
                                                      +
                                                      "<From EDDN>" + ";";

                                if(!checkPricePlausibility(new string[] {csvFormatted}, true))
                                {
                                    ImportCsvString(csvFormatted);
                                }
                                else
                                {
                                    string InfoString = string.Format("IMPLAUSIBLE DATA : \"{3}\" from {0}/{1}/ID=[{2}]", headerDictionary["softwareName"], headerDictionary["softwareVersion"], headerDictionary["uploaderID"], csvFormatted );

                                    lbEddnImplausible.Items.Add(InfoString);
                                    lbEddnImplausible.SelectedIndex = lbEddnImplausible.Items.Count-1;
                                    lbEddnImplausible.SelectedIndex = -1;

                                    if(cbSpoolImplausibleToFile.Checked)
                                    {
                                        FileStream LogFileStream = null;
                                        string FileName = @".\EddnImplausibleOutput.txt";

                                        if(File.Exists(FileName))
                                        { 
                                            LogFileStream = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                                        }
                                        else
                                        {
                                            LogFileStream = File.Create(FileName);
                                        }

                                       LogFileStream.Write(System.Text.Encoding.Default.GetBytes(InfoString), 0, System.Text.Encoding.Default.GetByteCount(InfoString));
                                       LogFileStream.Close();
                                    }

                                    Debug.Print("Implausible EDDN Data: " + csvFormatted);
                                }
                            }
                        

                            if ((DateTime.Now - _lastGuiUpdate) > TimeSpan.FromSeconds(10))
                            {
                                SetupGui();
                                _lastGuiUpdate = DateTime.Now;
                            }
                        }
                        else 
                        { 
                            string csvFormatted = messageDictionary["systemName"] + ";" +
                                                    messageDictionary["stationName"] + ";" +
                                                    messageDictionary["itemName"] + ";" +
                                                    (messageDictionary["sellPrice"] == "0" ? "" : messageDictionary["sellPrice"]) + ";" +
                                                    (messageDictionary["buyPrice"] == "0" ? "" : messageDictionary["buyPrice"]) + ";" +
                                                    messageDictionary["demand"] + ";" +
                                                    ";" +
                                                    messageDictionary["stationStock"] + ";" +
                                                    ";" +
                                                    messageDictionary["timestamp"] + ";"
                                                    +
                                                    "<From EDDN>" + ";";

                            lbEddnImplausible.Items.Add(string.Format("UNKNOWN COMMODITY : \"{3}\" from {0}/{1}/ID=[{2}]", headerDictionary["softwareName"], headerDictionary["softwareVersion"], headerDictionary["uploaderID"], csvFormatted ));
                            lbEddnImplausible.SelectedIndex = lbEddnImplausible.Items.Count-1;
                            lbEddnImplausible.SelectedIndex = -1;
                        }
                    }
                }
                catch
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

        private void cmdStopEDDNListening_Click(object sender, EventArgs e)
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
            try
            {
                Cursor = Cursors.WaitCursor;

                lvStationToStation.Items.Clear();
                lvStationToStationReturn.Items.Clear();

                if (cmbStationToStationTo.SelectedItem == null || cmbStationToStationFrom.SelectedItem == null || getCmbItemKey(cmbStationToStationFrom.SelectedItem) == ID_DELIMITER || getCmbItemKey(cmbStationToStationTo.SelectedItem) == ID_DELIMITER)
                {
                    Cursor = Cursors.Default;
                    return;
                }

                var stationFrom = getCmbItemKey(cmbStationToStationFrom.SelectedItem);
                var stationTo = getCmbItemKey(cmbStationToStationTo.SelectedItem);

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

                if (myMilkyway.existSystem(CombinedNameToSystemName(stationFrom)))
                {
                    var dist = DistanceInLightYears(
                                                         CombinedNameToSystemName(stationFrom).ToUpper(),
                                                         myMilkyway.getSystemCoordinates(CombinedNameToSystemName(stationTo)));

                    if (dist < double.MaxValue)
                        lblStationToStationLightYears.Text = "(" +
                                                         String.Format("{0:0.00}", dist
                                                         ) + " light years each way)";
                    else lblStationToStationLightYears.Text = "(system(s) not recognised)";
                }
                else lblStationToStationLightYears.Text = "(system(s) not recognised)";

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private Tuple<List<ListViewItem>, List<ListViewItem>> GetBestRoundTripForTwoStations(string stationFrom, string stationTo, out int bestRoundTrip)
        {
            if (stationFrom == null || stationTo == null) { bestRoundTrip = 0; return null; }
            var resultsOutbound = new List<ListViewItem>();
            var resultsReturn = new List<ListViewItem>();

            foreach (var commodity in CommodityDirectory)
            {
                var from = StationDirectory[stationFrom].Where(x => x.CommodityName == commodity.Key).ToList();
                var from2 = @from.Where(x => x.BuyPrice != 0 && x.Supply != 0).OrderByDescending(x => x.SampleDate).ToList();
                var to = StationDirectory[stationTo].Where(x => x.CommodityName == commodity.Key).ToList();
                var to2 = to.Where(x => x.SellPrice != 0 && x.Demand != 0).OrderByDescending(x => x.SampleDate).ToList();

                if (from2.Count() >= 1 && to2.Count() >= 1)
                {
                    var fromRow = from2[0];
                    var toRow = to2[0];

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
                from2 = @from.Where(x => x.BuyPrice != 0 && x.Supply != 0).OrderByDescending(x => x.SampleDate).ToList();
                to = StationDirectory[stationFrom].Where(x => x.CommodityName == commodity.Key).ToList();
                to2 = to.Where(x => x.SellPrice != 0 && x.Demand != 0).OrderByDescending(x => x.SampleDate).ToList();

                if (from2.Count() >= 1 && to2.Count() >= 1)
                {
                    var fromRow = from2[0];
                    var toRow = to2[0];

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


        private void button17_Click(object sender, EventArgs e)
        {
            SetupGui();
        }

        private void cmdHint_Click(object sender, EventArgs e)
        {
            MsgBox.Show(
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

        public void UpdateSystemNameFromLogFile()
        {
            if (m_LogfileScanner_Thread == null)
            {
                m_LogfileScanner_Thread = new Thread(() => this.UpdateSystemNameFromLogFile_worker());
                m_LogfileScanner_Thread.Name = "LogfileScanner_Thread";
                m_LogfileScanner_Thread.IsBackground = true;
                m_LogfileScanner_Thread.Start();
            }

            if (stateTimer == null)
            {
                var autoEvent = new AutoResetEvent(false);
                TimerCallback timerCallback = TimerCallback;
                stateTimer = new System.Threading.Timer(timerCallback, autoEvent, 10000, 10000);
            }


            m_LogfileScanner_ARE.Set();
        }

        public void UpdateSystemNameFromLogFile_worker()
        {
            SingleThreadLogger logger = new SingleThreadLogger(ThreadLoggerType.FileScanner);

            do
            {
                try
                {
                    string systemName = "";
                    string stationName = "";
                    string logLump;
                    Regex RegExTest = null;
                    Match m = null;
                    List<String> PossibleStations = new List<string>();

#if extScanLog
                    logger.Log("start, RegEx = <" + String.Format("FindBestIsland:.+:.+:.+:.+", Regex.Escape(RegulatedNoiseSettings.PilotsName)) + ">");
#endif
                    RegExTest = new Regex(String.Format("FindBestIsland:.+:.+:.+:.+", Regex.Escape(RegulatedNoiseSettings.PilotsName)), RegexOptions.IgnoreCase);

                    var appConfigPath = RegulatedNoiseSettings.ProductsPath;

                    if (Directory.Exists(appConfigPath))
                    {
                        var versions = Directory.GetDirectories(appConfigPath).Where(x => x.Contains("FORC-FDEV")).ToList().OrderByDescending(x => x).ToList();

                        if (versions.Count() == 0)
                        {
                            #if extScanLog
                                logger.Log("no dirs with <FORC-FDEV> found");
                                var versions2 = Directory.GetDirectories(appConfigPath).ToList().OrderByDescending(x => x).ToList();
                                foreach (string SubPath in versions2)
                                {
                                    logger.Log("but found <" +  SubPath + ">");   
                                }
                            #endif
                        }
                        else
                        {
                            #if extScanLog
                                logger.Log("lookin' for files in <" + versions[0] + ">");
                            #endif

                            // We'll just go right ahead and use the latest log...
                            var netLogs =
                                Directory.GetFiles(versions[0] + "\\Logs", "netLog*.log")
                                    .OrderByDescending(File.GetLastWriteTime)
                                    .ToArray();

                            if (netLogs.Length != 0)
                            {
                                var newestNetLog = netLogs[0];

                                Debug.Print("File opened : <" + newestNetLog + ">");
#if extScanLog
                                logger.Log("File opened : <" + newestNetLog + ">");
#endif
                                FileStream Datei = new FileStream(newestNetLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                Byte[] ByteBuffer = new Byte[1];
                                Byte[] LineBuffer = new Byte[SEARCH_MAXLENGTH];

                                Datei.Seek(0, SeekOrigin.End);

                                while (String.IsNullOrEmpty(stationName) && (Datei.Position >= 2))
                                {
                                    long StartPos = -1;
                                    long EndPos = -1;

                                    do
                                    {
                                        Datei.Read(ByteBuffer, 0, ByteBuffer.Length);

                                        if ((ByteBuffer[0] == 0x0A) || (ByteBuffer[0] == 0x0D))
                                            if (EndPos == -1)
                                            {
                                                if (ByteBuffer[0] == 0x0D)
                                                    EndPos = Datei.Position + 1;
                                                else
                                                    EndPos = Datei.Position;

                                                Datei.Seek(-3, SeekOrigin.Current);
                                            }
                                            else
                                            {
                                                if (ByteBuffer[0] == 0x0D)
                                                    StartPos = Datei.Position + 1;
                                                else
                                                    StartPos = Datei.Position;
                                            }
                                        else
                                            Datei.Seek(-3, SeekOrigin.Current);

                                    } while (StartPos == -1 && Datei.Position >= 3);

                                    if((StartPos == -1) && ((EndPos - StartPos) > SEARCH_MINLENGTH))
                                        StartPos = 0;

                                    if ((StartPos >= 0) && ((EndPos - StartPos) <= SEARCH_MAXLENGTH))
                                    {
                                        // found a line and it's not too long
                                        // read
                                        Datei.Read(LineBuffer, 0, (int)(EndPos - StartPos));
                                        // and convert to string
                                        logLump = Encoding.ASCII.GetString(LineBuffer, 0, (int)(EndPos - StartPos) );

                                        // first looking for the systemname
                                        if (logLump != null && String.IsNullOrEmpty(systemName))
                                        {
                                            if (logLump.Contains("System:"))
                                            {
                                                Debug.Print("Systemstring:" + logLump);
#if extScanLog
                                                logger.Log("Systemstring:" + logLump.Replace("\n", "").Replace("\r", ""));
#endif
                                                systemName = logLump.Substring(logLump.IndexOf("(", StringComparison.Ordinal) + 1);
                                                systemName = systemName.Substring(0, systemName.IndexOf(")", StringComparison.Ordinal));

                                                Debug.Print("System: " + systemName);
#if extScanLog
                                                logger.Log("System: " + systemName);
#endif

                                                // preparing search for station info
                                                RegExTest = new Regex(String.Format("FindBestIsland:.+:.+:.+:{0}", Regex.Escape(systemName)), RegexOptions.IgnoreCase);
#if extScanLog
                                                logger.Log("new Regex : <" + String.Format("FindBestIsland:.+:.+:.+:{0}", Regex.Escape(systemName)) + ">");
#endif

                                                // start search at the beginning

                                                if (RegExTest != null)
                                                {
                                                    // we may have candidates, check them and if nothing found search from the current position
                                                    foreach (string candidate in PossibleStations)
                                                    {
                                                        Debug.Print("check candidate : " + candidate);
#if extScanLog
                                                        logger.Log("check candidate : " + candidate.Replace("\n", "").Replace("\r", ""));
#endif
                                                        m = RegExTest.Match(candidate);
                                                        //Debug.Print(logLump);
                                                        //if (logLump.Contains("Duke Jones"))
                                                        //    Debug.Print("Stop");
                                                        if (m.Success)
                                                        {
#if extScanLog
                                                            logger.Log("Stationstring from candidate : " + candidate.Replace("\n", "").Replace("\r", ""));
#endif
                                                            Debug.Print("Stationstring from candidate : " + candidate);
                                                            getStation(ref stationName, m);
                                                            break;
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    // we must start from the end
                                                    Datei.Seek(0, SeekOrigin.End);
                                                }
                                            }
                                            else if (RegExTest != null)
                                            {
                                                m = RegExTest.Match(logLump);
                                                //Debug.Print(logLump);
                                                //if (logLump.Contains("Duke Jones"))
                                                //    Debug.Print("Stop");
                                                if (m.Success)
                                                {
#if extScanLog
                                                    logger.Log("Candidate added : " + logLump.Replace("\n", "").Replace("\r", ""));
#endif
                                                    Debug.Print("Candidate : " + logLump);
                                                    PossibleStations.Add(logLump);
                                                }

                                            }
                                        }

                                        // if we have the systemname we're looking for the stationname
                                        if (!string.IsNullOrEmpty(systemName) && string.IsNullOrEmpty(stationName))
                                        {
                                            m = RegExTest.Match(logLump);
                                            //Debug.Print(logLump);
                                            //if (logLump.Contains("Duke Jones"))
                                            //    Debug.Print("Stop");
                                            if (m.Success)
                                            {
#if extScanLog
                                                logger.Log("Stationstring (direct) : " + logLump.Replace("\n", "").Replace("\r", ""));
#endif
                                                Debug.Print("Stationstring (direct) : " + logLump);
                                                getStation(ref stationName, m);
                                            }
                                        }
                                    }

                                    if (StartPos >= 3)
                                    {
                                        Datei.Seek(StartPos-1, SeekOrigin.Begin);
                                    }
                                    else
                                        Datei.Seek(0, SeekOrigin.Begin);
                                }

                                Datei.Close();
                                Datei.Dispose();
                                Debug.Print("Datei geschlossen");
#if extScanLog
                                logger.Log("File closed");
#endif

                                setLocationInfo(systemName, stationName);

//                                if (systemName != "")
//                                {
//                                    Debug.Print("<" + systemName + "> - <" + tbCurrentSystemFromLogs.Text + ">");

//                                    setSystemInfo(systemName);

//                                }

//                                if (stationName != "")
//                                {
//                                    Debug.Print("<" + systemName + "> - <" + tbCurrentSystemFromLogs.Text + ">");

//                                    setSystemInfo(systemName);

//                                }

//                                    if (_LoggedSystem != systemName)
//                                    {
//#if extScanLog
//                                        logger.Log("1 <" + systemName + "> - <" + tbCurrentSystemFromLogs.Text + ">");
//                                        logger.Log("1 <" + stationName + "> - <" + tbCurrentStationinfoFromLogs.Text + ">");
//#endif

//                                        // "ClientArrivedtoNewSystem()" was often faster - so nothing was logged
//                                        if (cbAutoAdd_JumpedTo.Checked)
//                                        {
//                                            CommandersLog_CreateJumpedToEvent(systemName);
//                                        }

//                                        _LoggedSystem = systemName;

//                                        if (!String.IsNullOrEmpty(stationName))
//                                            m_lastestStationInfo = stationName;
//                                        else
//                                            m_lastestStationInfo = "scanning...";
//                                    }
//                                    else if (!String.IsNullOrEmpty(stationName))
//                                    { 
//#if extScanLog
//                                        logger.Log("2 <" + stationName + "> - <" + tbCurrentStationinfoFromLogs.Text + ">");
//#endif
//                                        m_lastestStationInfo = stationName;
//                                    }

//                                    //if (tbLogEventID.Text != "" && tbLogEventID.Text != systemName)
//                                    //{
//                                        setSystemInfo(systemName);
//                                    //}

//#if extScanLog
//                                    logger.Log("Found <" + systemName + "> - <" + m_lastestStationInfo + ">");
//                                    logger.Log("GUI   <" + tbCurrentSystemFromLogs.Text + "> - <" + tbCurrentStationinfoFromLogs.Text + ">");
//#endif
//                                }

                                

//                                setStationInfo();

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("AnalyseError");
                    logger.Log(ex.Message + "\n" + ex.StackTrace + "\n\n");
                }

#if extScanLog
                logger.Log("sleeping...");
                logger.Log("\n\n\n");
#endif
                Debug.Print("\n\n\n");
                m_LogfileScanner_ARE.WaitOne();
#if extScanLog
                logger.Log("awake...");
#endif

            }while (!this.Disposing && !m_Closing);

            Debug.Print("out");
        }

        private void CommandersLog_CreateJumpedToEvent(string Systemname)
        {
            if (InvokeRequired)
            {
                Invoke(new ScreenshotsQueuedDelegate(CommandersLog_CreateJumpedToEvent), Systemname);
            }
            else
            {
                _CmdrsLog_LastAutoEventID = CommandersLog.CreateEvent("Jumped To", "", Systemname, "", "", 0, "", DateTime.Now);
                setActiveItem(_CmdrsLog_LastAutoEventID);
            }
        }

        private void CommandersLog_StationVisitedEvent(string Systemname, string StationName)
        {
            if (InvokeRequired)
            {
                Invoke(new del_setLocationInfo(CommandersLog_StationVisitedEvent), Systemname, StationName);
            }
            else
            {
                if(!_LoggedVisited.Equals(Systemname + "|" + StationName, StringComparison.InvariantCultureIgnoreCase))
                {
                    bool noLogging = _LoggedVisited.Equals(ID_NOT_SET);

                    _LoggedVisited = Systemname + "|" + StationName;

                    if(cbAutoAdd_Visited.Checked && !noLogging)
                    { 
                        _CmdrsLog_LastAutoEventID = CommandersLog.CreateEvent("Visited", StationName, Systemname, "", "", 0, "", DateTime.Now);
                        setActiveItem(_CmdrsLog_LastAutoEventID);
                    }
                }
            }
        }

        private void CommandersLog_MarketDataCollectedEvent(string Systemname, string StationName)
        {
            if (InvokeRequired)
            {
                Invoke(new del_setLocationInfo(CommandersLog_MarketDataCollectedEvent), Systemname, StationName);
            }
            else
            {
                try
                {
                    if (!_LoggedMarketData.Equals(Systemname + "|" + StationName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _LoggedMarketData = Systemname + "|" + StationName;

                        if (cbAutoAdd_Marketdata.Checked)
                        {
                            if (cbAutoAdd_ReplaceVisited.Checked)
                            {
                                var logEvent = CommandersLog.LogEvents.SingleOrDefault(x => x.EventID == _CmdrsLog_LastAutoEventID);

                                if (logEvent != null &&
                                   logEvent.System.Equals(Systemname, StringComparison.InvariantCultureIgnoreCase) &&
                                   logEvent.Station.Equals(StationName, StringComparison.InvariantCultureIgnoreCase) &&
                                   logEvent.EventType.Equals("Visited", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    logEvent.EventType = "Market Data Collected";
                                    CommandersLog.UpdateCommandersLogListView();
                                }
                                else
                                {
                                    _CmdrsLog_LastAutoEventID = CommandersLog.CreateEvent("Market Data Collected", StationName, Systemname, "", "", 0, "", DateTime.Now);
                                    setActiveItem(_CmdrsLog_LastAutoEventID);
                                }
                            }
                            else
                            {
                                _CmdrsLog_LastAutoEventID = CommandersLog.CreateEvent("Market Data Collected", StationName, Systemname, "", "", 0, "", DateTime.Now);
                                setActiveItem(_CmdrsLog_LastAutoEventID);
                            }

                            setActiveItem(_CmdrsLog_LastAutoEventID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void getStation(ref string stationName, Match m)
        {
            string[] parts = m.Groups[0].ToString().Split(':');
            if (parts.GetUpperBound(0) >= 3)
            {
                stationName = parts[parts.GetUpperBound(0)-1];

                if (parts[0].Equals("FindBestIsland", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (String.IsNullOrEmpty(RegulatedNoiseSettings.PilotsName))
                        RegulatedNoiseSettings.PilotsName = parts[1];
                }
                else
                {
                    if (String.IsNullOrEmpty(RegulatedNoiseSettings.PilotsName))
                        RegulatedNoiseSettings.PilotsName = parts[0];
                }
            }
        }

        private void TimerCallback(object state)
        {
            UpdateSystemNameFromLogFile();
        }

        private void saveLogEntry(object sender, EventArgs e)
        {
            if (tbLogEventID.Text == "")
            {
                // this is done by CommandersLog.CreateEvent() itself
                // var newGuid = Guid.NewGuid().ToString();
                // tbLogEventID.Text = newGuid;

                var newGuid = CommandersLog.CreateEvent();
                CommandersLog.UpdateCommandersLogListView();
                //CommandersLog.CreateNewEvent();
                setActiveItem(newGuid);
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

                listViewData[0] = logEvent.EventDate.ToString(CultureInfo.CurrentCulture);

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
                var columnIndex = lvCommandersLog.Items.IndexOf(theClickedOne);
                lvCommandersLog.Items.RemoveAt(columnIndex);
                var newItem = new ListViewItem(listViewData);
                theClickedOne = newItem;
                lvCommandersLog.Items.Insert(columnIndex, newItem);
                lvCommandersLog.SelectedIndexChanged += lvCommandersLog_SelectedIndexChanged;
                //CommandersLog.CreateNewEvent();

                setActiveItem(((ListViewItem)newItem).SubItems[8].Text);
            }

            // save new datat immediatly
            CommandersLog.SaveLog(true);
        }

        private ListViewItem theClickedOne;

        private void lvCommandersLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv = ((ListView)sender);
            if (lv.SelectedItems.Count == 0) return;
            theClickedOne = lv.SelectedItems[0];
            var selectedGuid = theClickedOne.SubItems[lv.Columns.IndexOfKey("EventID")].Text;

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
            btCreateAddEntry.Text = "Save Changed Data";

        }

        bool _cbLogStationNameIsDirty = false;

        private void cbLogStationName_DropDown(object sender, EventArgs e)
        {

            if(_cbLogStationNameIsDirty)
            {
                cbLogStationName.Items.Clear();

                List<EDStation> StationsInSystem = _Milkyway.getStations(cbLogSystemName.Text);

                if(StationsInSystem != null)
                {
                    foreach (EDStation Station in StationsInSystem)
                        cbLogStationName.Items.Add(Station.Name);
                }

                _cbLogStationNameIsDirty = false;
            }
        }

        private void cbLogSystemName_TextChanged(object sender, System.EventArgs e)
        {
            _cbLogStationNameIsDirty = true;
        }

        private void cbLogSystemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLogStationName.Items.Clear();
            
            List<EDStation> StationsInSystem = _Milkyway.getStations(cbLogSystemName.Text);

            if(StationsInSystem != null)
            {
                foreach (EDStation Station in StationsInSystem)
                    cbLogStationName.Items.Add(Station.Name);
            }

            _cbLogStationNameIsDirty = false;

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

            RegulatedNoiseSettings.CmdrsLogSortColumn = _commandersLogColumnSorter.SortColumn;
            RegulatedNoiseSettings.CmdrsLogSortOrder = _commandersLogColumnSorter.Order;
        }

        /// <summary>
        /// handles mouse clicks on the Commanders Log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lvCommandersLog_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ListView currentListView = ((ListView)sender);

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                m_RightMouseSelectedLogEvent = null;
                ListViewItem theClickedOne = currentListView.GetItemAt(e.X, e.Y);

                if(theClickedOne != null)
                {
                    var selectedGuid = theClickedOne.SubItems[currentListView.Columns.IndexOfKey("EventID")].Text;
                    m_RightMouseSelectedLogEvent = CommandersLog.LogEvents.Single(x => x.EventID == selectedGuid);

                    contextMenuStrip1.Show(currentListView.PointToScreen(e.Location));
                }

            }
        }

        void lvCommandersLog_ColumnWidthChanged(object sender, System.Windows.Forms.ColumnWidthChangedEventArgs e)
        {
            saveColumns((ListView)sender);            
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

        private void Form_Shown(object sender, System.EventArgs e)
        {
            _Splash.CloseDelayed();

            loadSystemData(tbCurrentSystemFromLogs.Text);
            loadStationData(tbCurrentSystemFromLogs.Text, tbCurrentStationinfoFromLogs.Text);

            showSystemNumbers();

            SetupGui();

        }

        private void Form_Load(object sender, EventArgs e)
        {

            Text += RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture);

#if DukeJones
            RegulatedNoiseSettings.CheckVersion2();
            Text += "_" + RegulatedNoiseSettings.VersionDJ.ToString(CultureInfo.InvariantCulture);
#endif


            if (((DateTime.Now.Day == 24 || DateTime.Now.Day == 25 || DateTime.Now.Day == 26) &&
                 DateTime.Now.Month == 12) || (DateTime.Now.Day == 31 && DateTime.Now.Month == 12) ||
                (DateTime.Now.Day == 1 && DateTime.Now.Month == 1))
            {
                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = 75;
                _timer.Tick += OnTick;
                _timer.Start();
            }

            tabCtrlMain.SelectedTab = tabPriceAnalysis;
            tabControl2.SelectedTab = tabPage3;
            tabControl2.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage2;
            tabControl2.SelectedTab = tabStationToStation;
            tabControl2.SelectedTab = tabPage3;
            tabCtrlMain.SelectedTab = tabHelpAndChangeLog;
            
            Retheme();

            Clock = new System.Windows.Forms.Timer();
            Clock.Interval = 1000;
            Clock.Start();
            Clock.Tick += Clock_Tick;

            cmdTest.Visible = System.Diagnostics.Debugger.IsAttached;

        }

        private void Clock_Tick(object sender, EventArgs e)
        {
            setClock();    
        }

        private void setClock()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(setClock));
            else
            { 
                txtLocalTime.Text = DateTime.Now.ToString("T");
                txtEDTime.Text = DateTime.UtcNow.ToString("T");
            }
        }

        private void doSpecial()
        {
            decimal lastVersion   = RegulatedNoiseSettings.lastVersion;
            decimal lastVersionDJ = RegulatedNoiseSettings.lastVersionDJ;

            if (RegulatedNoiseSettings.isFirstVersionRun())
            {
                // do all the things that must be done for the new versions
                if ((RegulatedNoiseSettings.Version == 1.84m) && (RegulatedNoiseSettings.VersionDJ == 0.09m))
                { 
                    // this value works much better
                    RegulatedNoiseSettings.EBPixelThreshold = 0.6f;
                    RegulatedNoiseSettings.EBPixelAmount    = 22;
                }

                // do all the things that must be done for the new versions
                if ((RegulatedNoiseSettings.Version == 1.84m) && (RegulatedNoiseSettings.VersionDJ == 0.17m))
                { 
                    if(RegulatedNoiseSettings.UseEddnTestSchema)
                    { 
                        RegulatedNoiseSettings.UseEddnTestSchema = false;
                        SaveSettings();
                        if(RegulatedNoiseSettings.PostToEddnOnImport)
                        { 
                            MsgBox.Show("Set EDDN-mode uniquely to <non-test>-mode. \n" +
                                            "If you know, what you're doing (e.g. you're developer) you can change it back again to <test>-mode", 
                                            "Changing a mistakable setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }


                if(!RegulatedNoiseSettings.checkedTestEDDNSetting)
                { 
                    if((lastVersion.Equals(1.84m) && lastVersionDJ.Equals(0.17m)))
                    {
                        // last was 0.17 - so we can be sure, we did the check
                        RegulatedNoiseSettings.checkedTestEDDNSetting = true;
                        SaveSettings();
                    }
                    else
                    {
                        // check did never run yet
                        if(RegulatedNoiseSettings.UseEddnTestSchema)
                        { 
                            RegulatedNoiseSettings.UseEddnTestSchema = false;
                            SaveSettings();
                            if(RegulatedNoiseSettings.PostToEddnOnImport)
                            { 
                                MsgBox.Show("Set EDDN-mode uniquely to <non-test>-mode. \n" +
                                                "If you know, what you're doing (e.g. you're developer) you can change it back again to <test>-mode", 
                                                "Changing a mistakable setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        RegulatedNoiseSettings.checkedTestEDDNSetting = true;
                        SaveSettings();
                    }
                }
            }
        }

        private void Retheme()
        {
            bool noBackColor = false;

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

                noBackColor = false;

                if(!(c.Name == "lblUpdateInfo" && lblUpdateInfo.BackColor == Color.Yellow))
                { 
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
                    if (props.Contains("BackColor_ro"))
                    {
                        var prop = c.GetType().GetProperty("BackColor_ro", BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(c, b);
                    }
                    if (props.Contains("ForeColor_ro"))
                    {
                        var prop = c.GetType().GetProperty("ForeColor_ro", BindingFlags.Public | BindingFlags.Instance);
                        prop.SetValue(c, f);
                    }
                    
                }
                else 
                    noBackColor = true;
            }

            if(!noBackColor)
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

        private void bSwapStationToStations_Click(object sender, EventArgs e)
        {
            int selIndex = cmbStationToStationFrom.SelectedIndex;
            cmbStationToStationFrom.SelectedIndex = cmbStationToStationTo.SelectedIndex;
            cmbStationToStationTo.SelectedIndex = selIndex;
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
            string ComboboxKey = getCmbItemKey(cmbStation.SelectedItem);

            var csvrow  = StationDirectory[ComboboxKey].First(x => x.CommodityName == lbPrices.SelectedItems[0].Text);
            var csvrow2 = CommodityDirectory[lbPrices.SelectedItems[0].Text].First(x => x.StationID == ComboboxKey);
            
            var f = new EditPriceData(csvrow, CommodityDirectory.Keys.ToList());
            var q = f.ShowDialog();

            if (q == DialogResult.OK)
            {
                StationDirectory[ComboboxKey].Remove(csvrow);
                CommodityDirectory[lbPrices.SelectedItems[0].Text].Remove(csvrow2);
                ImportCsvString(f.RowToEdit.ToString());

                SetupGui();
                cbStation_SelectedIndexChanged(cmbStation, new EventArgs());
            }
        }

        private void bCommodityEditRow_Click(object sender, EventArgs e)
        {
            var csvrow =
                StationDirectory[lbCommodities.SelectedItems[0].Text].First(
                        x => x.CommodityName == cbCommodity.SelectedItem.ToString());

            var csvrow2 =
                CommodityDirectory[cbCommodity.SelectedItem.ToString()].First(
                    x => x.StationID == lbCommodities.SelectedItems[0].Text);

            var f = new EditPriceData(csvrow, CommodityDirectory.Keys.ToList());
            var q = f.ShowDialog();

            if (q == DialogResult.OK)
            {
                StationDirectory[lbCommodities.SelectedItems[0].Text].Remove(csvrow);
                CommodityDirectory[cbCommodity.SelectedItem.ToString()].Remove(csvrow2);
                ImportCsvString(f.RowToEdit.ToString());
                SetupGui();
                cbCommodity_SelectedIndexChanged(cbCommodity, new EventArgs());
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
            try
            {
                foreach (ListViewItem item in lbPrices.SelectedItems)
                {
                    var csvrow =
                        StationDirectory[getCmbItemKey(cmbStation.SelectedItem)].First(
                            x => x.CommodityName == item.Text);

                    var csvrow2 =
                        CommodityDirectory[item.Text].First(
                            x => x.StationID == getCmbItemKey(cmbStation.SelectedItem));

                    StationDirectory[getCmbItemKey(cmbStation.SelectedItem)].Remove(csvrow);
                    CommodityDirectory[item.Text].Remove(csvrow2);

                    if (StationDirectory[getCmbItemKey(cmbStation.SelectedItem)].Count == 0)
                    {
                        // if theres no commodity price anymore we can (must) delete the history data
                        StationVisit StationInHistory = _StationHistory.History.Find(x => x.Station == getCmbItemKey(cmbStation.SelectedItem));
                        if (StationInHistory != null)
                            _StationHistory.History.Remove(StationInHistory);

                        // and also the station itself
                        StationDirectory.Remove(getCmbItemKey(cmbStation.SelectedItem));
                    }
                }

                SetupGui();
                cbStation_SelectedIndexChanged(cmbStation, new EventArgs());
            }
            catch (Exception ex)
            {
                throw ex  ;  
            }
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
                        x => x.StationID == item.Text);

                StationDirectory[item.Text].Remove(csvrow);
                CommodityDirectory[cbCommodity.SelectedItem.ToString()].Remove(csvrow2);

                if (StationDirectory[item.Text].Count == 0)
                {
                    // if theres no commodity price anymore we can (must) delete the history data
                    StationVisit StationInHistory = _StationHistory.History.Find(x => x.Station == item.Text);
                    if (StationInHistory != null)
                        _StationHistory.History.Remove(StationInHistory);

                    // and also the station itself
                    StationDirectory.Remove(item.Text);
                }
            }

            SetupGui();
            cbCommodity_SelectedIndexChanged(cbCommodity, new EventArgs());
        }

        private void btnBestRoundTrip_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            lbAllRoundTrips.Items.Clear();
            int bestRoundTrip = -1;
            string stationA = "", stationB = "";
            ProgressView progress   = new ProgressView();
            List<Tuple<string, double>> allRoundTrips = new List<Tuple<string, double>>();
            
            var selectedStations = StationDirectory.Where(x => getStationSelection(x));

            Int32 Total             = getCalculations(selectedStations.Count());
            Int32 Current           = 0;

            progress.progressStart(string.Format(string.Format("calculating best routes: {0} abilities from {1} stations", Total, selectedStations.Count())));

            for (int i = 0; i < selectedStations.Count()-1; i++)
            {
                for (int j = i+1; j < selectedStations.Count(); j++)
                {
                    var a = selectedStations.ElementAt(i);        
                    var b = selectedStations.ElementAt(j);

                    Current+=1;
                    progress.progressUpdate(Current, Total);

                    Debug.Print(Current +"/"+ Total);

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

                        distance = DistanceInLightYears(CombinedNameToSystemName(a.Key).ToUpper(), CombinedNameToSystemName(b.Key).ToUpper());

                        if (cbPerLightYearRoundTrip.Checked)
                        {
                            creditsDouble = bestThisTrip / (2.0 * distance);
                            credits = String.Format("{0:0.000}", creditsDouble / (2.0 * distance)) + " Cr/Ly";
                        }
                        else
                        {
                            creditsDouble = bestThisTrip;
                            credits = (bestThisTrip + " Cr");
                        }

                        if ((!cbMaxRouteDistance.Checked) || (double.Parse(cmbMaxRouteDistance.Text) >= distance))
                        {
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

                    if (progress.Cancelled)
                        break;
                }

                if (progress.Cancelled)
                    break;
            }

            progress.progressStop();

            var ordered = allRoundTrips.OrderByDescending(x => x.Item2).Select(x => x.Item1).Distinct().ToList().Cast<object>().ToArray();

            lbAllRoundTrips.Items.AddRange(ordered);
            if(lbAllRoundTrips.Items.Count > 0)
                lbAllRoundTrips.SelectedIndex = 0;

            this.Cursor = Cursors.Default;
        }

        private int getCalculations(int Total)
        {
            Int32 retValue = 0;

            for (int i = 0; i <Total; i++)
            {
                retValue += i;
            }
            return retValue;

        }

        private void checkboxLightYears_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            RegulatedNoiseSettings.limitLightYears = cbLimitLightYears.Checked;
            SetupGui();
            Cursor = Cursors.Default;
        }

        private void cbStationToStar_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            RegulatedNoiseSettings.StationToStar = cbStationToStar.Checked;
            SetupGui();
            Cursor = Cursors.Default;
        }

        private void cbIncludeWithinRegionOfStation_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            SetupGui();
            Cursor = Cursors.Default;
        }

        private void cmdPurgeEDDNData(object sender, EventArgs e)
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

//            Debug.Print("v : " + fromStation + " / c:" + cmbStationToStationFrom.SelectedValue+ " / c:" + cmbStationToStationFrom.SelectedItem, cmbStationToStationFrom.SelectedIndex);
            int fromIndex = -1;
            int toIndex = -1;

            if (!_StationIndices.TryGetValue(fromStation, out fromIndex))
                fromIndex = -1;

            if (!_StationIndices.TryGetValue(toStation, out toIndex))
                toIndex = -1;

            try
            {
                cmbStationToStationFrom.SelectedIndex = fromIndex;
                cmbStationToStationTo.SelectedIndex = toIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            Debug.Print("n : " + fromStation + " / c:" + cmbStationToStationFrom.SelectedValue+ " / c:" + cmbStationToStationFrom.SelectedItem, cmbStationToStationFrom.SelectedIndex);
            Debug.Print("");

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
            clearOcrOutput();
        }

        public void clearOcrOutput()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(clearOcrOutput));
                return;
            }

            // when we do clear so we must consider all dependences (!->_commoditiesSoFar)
            // doing some stateful enabling of button again
            tbFinalOcrOutput.Text = "";
            bContinueOcr.Enabled = false;
            bIgnoreTrash.Enabled = false;
            _commoditiesSoFar = new List<string>();
            bClearOcrOutput.Enabled = false;
            bEditResults.Enabled = false;
            tbCommoditiesOcrOutput.Text = "Finished!";

            tbOcrStationName.Text = "";
            tbOcrSystemName .Text = "";
            pbOcrCurrent.Image = null;
            UpdateOriginalImage(null);
            UpdateTrimmedImage(null, null);


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

        /// <summary>
        /// selects another "traineddata" file for TesseractOCR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSelectTraineddataFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OCRFile = new OpenFileDialog();

            OCRFile.Filter = "Tesseract-Files|*.traineddata|All Files|*.*";
            OCRFile.FileName = RegulatedNoiseSettings.TraineddataFile;
            OCRFile.InitialDirectory = System.IO.Path.GetFullPath("./tessdata");  
            OCRFile.Title = "select Tesseract Traineddata-File...";

            if (OCRFile.ShowDialog(this) == DialogResult.OK)
            {
                RegulatedNoiseSettings.TraineddataFile = System.IO.Path.GetFileNameWithoutExtension(OCRFile.FileName);
                txtTraineddataFile.Text = RegulatedNoiseSettings.TraineddataFile;

                SaveSettings();
            }

                

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

        /// <summary>
        /// prepares the "Language" combobox
        /// </summary>
        private void setLanguageCombobox()
        {
            List<enumBindTo> lstEnum = new List<enumBindTo>();
            Array Names;

            // Speicherstruktur
            lstEnum.Clear();
            Names = Enum.GetValues(Type.GetType("RegulatedNoise.enLanguage", true));

            for (int i = 0; i <= Names.GetUpperBound(0); i++)
            {
                enumBindTo cls = new enumBindTo();

                cls.EnumValue = (Int32)Names.GetValue(i);
                cls.EnumString = Names.GetValue(i).ToString();

                lstEnum.Add(cls);
            }

            cmbLanguage.ValueMember = "EnumValue";
            cmbLanguage.DisplayMember = "EnumString";
            cmbLanguage.DataSource = lstEnum;

            cmbLanguage.SelectedValue = (Int32)RegulatedNoiseSettings.Language;

            // now we activate the EventHandler
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);

        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_InitDone)
            {
                RegulatedNoiseSettings.Language = (enLanguage)cmbLanguage.SelectedValue;

                // prepare language depending list
                loadCommodities(RegulatedNoiseSettings.Language);
                loadCommodityLevels(RegulatedNoiseSettings.Language);

                SaveSettings();

            }
        }

        /// <summary>
        /// adds a new commodity the the dictionary
        /// </summary>
        /// <param name="commodity"></param>
        /// <param name="language"></param>
        private void addCommodity(string commodity, enLanguage language)
        {
            dsCommodities.NamesRow newCommodity = (dsCommodities.NamesRow)_commodities.Names.NewRow();

            newCommodity.eng = "???";
            newCommodity.ger = "???";
            newCommodity.fra = "???";

            if (language == enLanguage.eng)
                newCommodity.eng = commodity;

            else if (language == enLanguage.ger)
                newCommodity.ger = commodity;

            else
                newCommodity.fra = commodity;

            _commodities.Names.AddNamesRow(newCommodity);

            // save to file
            _commodities.WriteXml(".//Data//Commodities.xml");

            // reload in working array
            loadCommodities(RegulatedNoiseSettings.Language);
        }

        public void setOCRCalibrationTabVisibility()
        {
            TabPage OCRTabPage;
            OcrCalibratorTab TabControl;

            if (GameSettings != null && tabCtrlOCR.TabPages["OCR_Calibration"] != null)
            {
                OCRTabPage = tabCtrlOCR.TabPages["OCR_Calibration"];
                OCRTabPage.Enabled = (GameSettings.Display != null);
                TabControl = (OcrCalibratorTab)(OCRTabPage.Controls[0]);
                TabControl.lblWarning.Visible = (GameSettings.Display == null); 
            }
        }

        private void cbAutoAdd_JumpedTo_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.AutoEvent_JumpedTo = cbAutoAdd_JumpedTo.Checked;
            SaveSettings();
        }

        /// <summary>
        /// selects the wanted ListViewItem
        /// </summary>
        /// <param name="wantedItem"></param>
        public void setActiveItem(String wantedItem)
        {
            int EventIDIndex = lvCommandersLog.Columns.IndexOfKey("EventID");

            foreach (ListViewItem x in lvCommandersLog.Items)
            {
                if (x.SubItems[EventIDIndex].Text == wantedItem)
                {
                    x.Selected = true;
                }

            }

        }

        private void txtPixelThreshold_LostFocus(object sender, EventArgs e)
        {
            float newValue;

            if (float.TryParse(txtPixelThreshold.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out newValue))
                if (newValue >= 0.0f && newValue <= 1.0)
                    RegulatedNoiseSettings.EBPixelThreshold = newValue;
                else
                    txtPixelThreshold.Text = RegulatedNoiseSettings.EBPixelThreshold.ToString("F1");
            else
                txtPixelThreshold.Text = RegulatedNoiseSettings.EBPixelThreshold.ToString("F1");
        }

        private void txtPixelAmount_LostFocus(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(txtPixelAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out newValue))
                if (newValue >= 0 && newValue <= 99)
                    RegulatedNoiseSettings.EBPixelAmount = newValue;
                else
                    txtPixelAmount.Text = RegulatedNoiseSettings.EBPixelAmount.ToString();
            else
                txtPixelAmount.Text = RegulatedNoiseSettings.EBPixelAmount.ToString();
        }

        private void txtGUIColorCutoffLevel_LostFocus(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(txtGUIColorCutoffLevel.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out newValue))
                if (newValue >= 0 && newValue <= 255)
                    RegulatedNoiseSettings.GUIColorCutoffLevel = newValue;
                else
                    txtGUIColorCutoffLevel.Text = RegulatedNoiseSettings.GUIColorCutoffLevel.ToString();
            else
                txtGUIColorCutoffLevel.Text = RegulatedNoiseSettings.GUIColorCutoffLevel.ToString();
        }

        /// <summary>
        /// get the Milkyway
        /// </summary>
        public EDMilkyway myMilkyway
        {
            get
            {
                return _Milkyway;
            }
        }

        private void rbSortBy_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                switch (((RadioButton)sender).Name)
                {
                    case "rbSortBySystem":
                        RegulatedNoiseSettings.CBSortingSelection = 1;
                        break;
                    case "rbSortByStation":
                        RegulatedNoiseSettings.CBSortingSelection = 2;
                        break;
                    case "rbSortByDistance":
                        RegulatedNoiseSettings.CBSortingSelection = 3;
                        break;
                }

                SetupGui();
            }
        }

        private void cblastVisitedFirst_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.lastStationCountActive = cblastVisitedFirst.Checked;
            SetupGui();
        }

        private void txtlastStationCount_LostFocus(object sender, EventArgs e)
        {
            checkLastStationCountInput();
        }

        private void txtlastStationCount_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            { 
                checkLastStationCountInput();   
            }
        }

        private void checkLastStationCountInput()
        {
            int value;
            bool valueOK = false;

            if (int.TryParse(txtlastStationCount.Text, out value))
            {
                if (value >= 1 && value <= 99)
                {
                    RegulatedNoiseSettings.lastStationCount = value;
                    valueOK = true;
                }
                else
                    txtlastStationCount.Text = RegulatedNoiseSettings.lastStationCount.ToString();
            }
            else
            {
                txtlastStationCount.Text = RegulatedNoiseSettings.lastStationCount.ToString();
            }

            if (valueOK && cblastVisitedFirst.Checked)
                SetupGui();
        }

        private void cmbLightYearsInput_LostFocus(object sender, EventArgs e)
        {
            checkcbLightYearsInput();
        }

        private void cmbLightYearsInput_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            { 
                checkcbLightYearsInput();   
            }
        }

        private void cmbLightYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkcbLightYearsInput();   
        }

        private void cmbStationToStarInput_LostFocus(object sender, System.EventArgs e)
        {
            checkcmbStationToStarInput();
        }

        private void cmbStationToStar_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            checkcmbStationToStarInput();
        }
        private void cmbStationToStarInput_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            { 
                checkcmbStationToStarInput();   
            }
        }

        private void cmbMaxRouteDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMaxRouteDistanceInput();   
        }

        private void cmbMaxRouteDistance_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            { 
                cmbMaxRouteDistanceInput();   
            }
        }

        private void cmbMaxRouteDistance_LostFocus(object sender, System.EventArgs e)
        {
            cmbMaxRouteDistanceInput();   
        }

        private void checkcbLightYearsInput()
        {
            int value;
            bool valueOK = false;

            Debug.Print("1");

            if (int.TryParse(cmbLightYears.Text, out value))
            {
                if (value >= 0)
                {
                    RegulatedNoiseSettings.lastLightYears = value;
                    valueOK = true;
                }
                else
                    cmbLightYears.Text = RegulatedNoiseSettings.lastLightYears.ToString();
            }
            else
            {
                cmbLightYears.Text = RegulatedNoiseSettings.lastLightYears.ToString();
            }

            if (valueOK && cbLimitLightYears.Checked)
                SetupGui();
        }

        private void checkcmbStationToStarInput()
        {
            int value;
            bool valueOK = false;

            Debug.Print("1");

            if (int.TryParse(cmbStationToStar.Text, out value))
            {
                if (value >= 0)
                {
                    RegulatedNoiseSettings.lastStationToStar = value;
                    valueOK = true;
                }
                else
                    cmbStationToStar.Text = RegulatedNoiseSettings.lastStationToStar.ToString();
            }
            else
            {
                cmbStationToStar.Text = RegulatedNoiseSettings.lastStationToStar.ToString();
            }

            if (valueOK && cbStationToStar.Checked)
                SetupGui();
        }

        private void cmbMaxRouteDistanceInput()
        {
            int value;

            if (int.TryParse(cmbMaxRouteDistance.Text, out value))
            {
                if (value >= 0)
                {
                    RegulatedNoiseSettings.lastMaxRouteDistance = value;
                }
                else
                    cmbMaxRouteDistance.Text = RegulatedNoiseSettings.lastMaxRouteDistance.ToString();
            }
            else
            {
                cmbMaxRouteDistance.Text = RegulatedNoiseSettings.lastMaxRouteDistance.ToString();
            }

        }

        private void cbPerLightYearRoundTrip_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.PerLightYearRoundTrip = cbPerLightYearRoundTrip.Checked;
        }

        /// <summary>
        /// starts the filter test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdFilter_Click(object sender, EventArgs e)
        {

            Bitmap _refbmp = getReferenceScreenshot();

            if (_refbmp == null)
            {
                return;
            }

            FilterTest FTest = new FilterTest();

            FTest.CutoffLevel = RegulatedNoiseSettings.GUIColorCutoffLevel;
            FTest.TestBitmap = _refbmp;

            FTest.ShowDialog(this);

            if (FTest.DialogResult == System.Windows.Forms.DialogResult.OK)
            { 
                txtGUIColorCutoffLevel.Text = FTest.CutoffLevel.ToString();
                RegulatedNoiseSettings.GUIColorCutoffLevel = FTest.CutoffLevel;
                SaveSettings();            
            }
        }

        private Bitmap getReferenceScreenshot()
        {
            var openFile = new OpenFileDialog
            {
                DefaultExt = "bmp",
                Multiselect = true,
                Filter = "BMP (*.bmp)|*.bmp",
                InitialDirectory =
                    Environment.GetFolderPath((Environment.SpecialFolder.MyPictures)) +
                    @"\Frontier Developments\Elite Dangerous",
                Title = "Open a screenshot for calibration"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var bmp = new Bitmap(openFile.FileName);

                if (bmp.Height == Form1.GameSettings.Display.Resolution.Y &&
                    bmp.Width == Form1.GameSettings.Display.Resolution.X) return bmp;
                var wrongres = MsgBox.Show("The selected image has a different resolution from your current game settings. Do you want to pick another image?", "Ooops...", MessageBoxButtons.YesNo);
                if (wrongres == DialogResult.Yes)
                {
                    return getReferenceScreenshot();
                }
                
                return bmp;
            }
            return null;
        }

        private void cmdWarnLevels_Click(object sender, EventArgs e)
        {
            string Commodity = String.Empty;

            EDCommodityListView CView = new EDCommodityListView();

            CView.ShowDialog(this);

        }

        private void cbActivateOCRTab_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.AutoActivateOCRTab = cbAutoActivateOCRTab.Checked;
            SaveSettings();
        }

        private void cmdDonate_Click(object sender, EventArgs e)
        {
            string url = "";
 
            string ButtonID     = "CMH6HZK37VGHY";  // your paypal email
 
            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_s-xclick" +
                "&hosted_button_id=" + ButtonID;
 
            System.Diagnostics.Process.Start(url);

        }

        //private bool _firstRunDone = false;
        //public void setStationInfo()
        //{
        //    if (InvokeRequired)
        //        Invoke(new MethodInvoker(setStationInfo));
        //    else
        //    {

        //        if (!_LoggedLocation.Equals(m_lastestStationInfo, StringComparison.InvariantCultureIgnoreCase))
        //        {                    
        //            tbCurrentStationinfoFromLogs.Text = m_lastestStationInfo;
        //            _LoggedLocation = m_lastestStationInfo;
    
        //            if(cbAutoActivateSystemTab.Checked)
        //            { 
        //                tabCtrlMain.SelectedTab = tabCtrlMain.TabPages["tabSystemData"];
        //                loadSystemData(_LoggedSystem);
        //                loadStationData(_LoggedSystem, _LoggedLocation);
        //            }
        //            else if(!_firstRunDone)
        //            {
        //                loadSystemData(_LoggedSystem);
        //                loadStationData(_LoggedSystem, _LoggedLocation);
        //            }

        //            _firstRunDone = true;
        //        } 
        //    }
        //}

        //public void setSystemInfo(string SystemInfo)
        //{
        //    if (InvokeRequired)
        //        Invoke(new ScreenshotsQueuedDelegate(setSystemInfo), SystemInfo);
        //    else
        //    { 
        //        tbCurrentSystemFromLogs.Text = SystemInfo;
        //    }
        //}

        public void setControlText(Control CtrlObject, string newText)
        {
            if (CtrlObject.InvokeRequired)
                CtrlObject.Invoke(new del_setControlText(setControlText), CtrlObject, newText);
            else
            { 
                CtrlObject.Text = newText;
            }
        }

        private void setLocationInfo(string systemName, string stationName)
        {

            if(InvokeRequired)
            { 
                Invoke(new del_setLocationInfo(setLocationInfo), systemName, stationName);
                return;
            }

            //bool Jumped_To      = false;
            bool newSystem      = false;
            bool newLocation    = false;
            bool InitialRun     = false;

            if(!String.IsNullOrEmpty(systemName))
            { 
                // system info found
                if(!tbCurrentSystemFromLogs.Text.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                { 
                    // it's a new system
                    Debug.Print("tbCurrentSystemFromLogs=" + tbCurrentSystemFromLogs);
                    tbCurrentSystemFromLogs.Text = systemName;
                    newSystem = true;
                }

                // system info found
                if(!_LoggedSystem.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                { 
                    // system is not logged yet

                    // update Cmdr's Log ?
                    if(_LoggedSystem != ID_NOT_SET)
                    { 
                        // it's not the first run, create a event if wanted
                        if (cbAutoAdd_JumpedTo.Checked)
                        {
                            // create event is enabled
                            CommandersLog_CreateJumpedToEvent(systemName);
                        }
                    }
                    else
                    {
                        InitialRun = true;
                    }

                    //Jumped_To = true;
                    _LoggedSystem = systemName;
                }

            }

            if(!String.IsNullOrEmpty(stationName))
            { 
                // system info found
                if(!tbCurrentStationinfoFromLogs.Text.Equals(stationName, StringComparison.InvariantCultureIgnoreCase))
                { 
                    // it's a new location
                    tbCurrentStationinfoFromLogs.Text = stationName;
                    newLocation = true;

                    List<EDStation> SystemStations = _Milkyway.getStations(systemName);

                    if((SystemStations != null) && (SystemStations.Find(x => x.Name.Equals(stationName, StringComparison.InvariantCultureIgnoreCase)) != null))
                        if (cbAutoAdd_Visited.Checked)
                        {
                            // create event is enabled
                            CommandersLog_StationVisitedEvent(systemName, stationName);
                        }

                    _LoggedLocation = stationName;

                    _LoggedMarketData = "";
                    _LoggedVisited = "";

                }
            }

            if((newSystem || newLocation) && (!InitialRun))
            { 
                loadSystemData(_LoggedSystem);
                loadStationData(_LoggedSystem, _LoggedLocation);

                if(cbAutoActivateSystemTab.Checked)
                    tabCtrlMain.SelectedTab = tabCtrlMain.TabPages["tabSystemData"];
            }

        }

        /// <summary>
        /// copies the systemname of the CmdrLog entry to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copySystenmameToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(m_RightMouseSelectedLogEvent != null)
            {
                 Clipboard.SetText(m_RightMouseSelectedLogEvent.System);
            }
        }

        /// <summary>
        /// gets the reduced selection of station due to lightyear limit and distance to star limit
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private bool getStationSelection(KeyValuePair<string, List<CsvRow>> x)
        {
            return (!cbLimitLightYears.Checked || Distance(CombinedNameToSystemName(x.Key))) &&
                   (!cbStationToStar.Checked   || StationDistance(CombinedNameToSystemName(x.Key), CombinedNameToStationName(x.Key)));

        }

        private bool getStationSelection(CsvRow x, bool noRestriction=false)
        {
            if(noRestriction)
                return true;
            else
                return (!cbLimitLightYears.Checked || Distance(x.SystemName)) &&
                       (!cbStationToStar.Checked   || StationDistance(x.SystemName, x.StationName));
        }

        private void label63_Click(object sender, EventArgs e)
        {

                    }

#region System / Station Tab

        /*/////////////////////////////////////////////////////////////////////////////////////////
        *******************************************************************************************    
         *
         *                             System / Station Tab
         * 
        ******************************************************************************************* 
         /*//////////////////////////////////////////////////////////////////////////////////////*/

        private void showSystemNumbers()
        {
            if(this.InvokeRequired)
            { 
                this.Invoke(new MethodInvoker(showSystemNumbers));
                return;
            }

            lblSystemCountTotal.Text    = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Count.ToString();
            lblStationCountTotal.Text   = _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Count.ToString();
        }


        private void cmdLoadCurrentSystem_Click(object sender, EventArgs e)
        {

            loadSystemData(tbCurrentSystemFromLogs.Text);
            loadStationData(tbCurrentSystemFromLogs.Text, tbCurrentStationinfoFromLogs.Text);

            tabCtrlMain.SelectedTab = tabCtrlMain.TabPages["tabSystemData"];

        }

        private void loadSystemData(string Systemname, bool isNew=false)
        {

            m_SystemLoadingValues = true;

            if (isNew)
            {
                cmbSystemsAllSystems.SelectedIndex = 0;
                m_loadedSystemdata = new EDSystem();
                m_loadedSystemdata.Name = Systemname;
                m_SystemIsNew = true;
            }
            else
            {
                cmbSystemsAllSystems.SelectedValue = Systemname;
                m_loadedSystemdata = _Milkyway.getSystem(Systemname);
                m_SystemIsNew = false;
            }

            cmbStationStations.Items.Clear();
            cmbStationStations.Items.Add("");


            if (m_loadedSystemdata != null)
            {
                m_currentSystemdata.getValues(m_loadedSystemdata, true);

                txtSystemId.Text = m_loadedSystemdata.Id.ToString(CultureInfo.CurrentCulture);
                txtSystemName.Text = m_loadedSystemdata.Name;
                txtSystemX.Text = m_loadedSystemdata.X.ToString("0.00000", CultureInfo.CurrentCulture);
                txtSystemY.Text = m_loadedSystemdata.Y.ToString("0.00000", CultureInfo.CurrentCulture);
                txtSystemZ.Text = m_loadedSystemdata.Z.ToString("0.00000", CultureInfo.CurrentCulture);
                txtSystemFaction.Text = m_loadedSystemdata.Faction.NToString();
                txtSystemPopulation.Text = m_loadedSystemdata.Population.ToNString("#,##0.", CultureInfo.CurrentCulture);
                txtSystemUpdatedAt.Text = m_loadedSystemdata.UpdatedAt.ToString();
                cbSystemNeedsPermit.CheckState = m_loadedSystemdata.NeedsPermit.toCheckState();
                cmbSystemPrimaryEconomy.Text = m_loadedSystemdata.PrimaryEconomy.NToString();
                cmbSystemSecurity.Text = m_loadedSystemdata.Security.NToString();
                cmbSystemState.Text = m_loadedSystemdata.State.NToString();
                cmbSystemAllegiance.Text = m_loadedSystemdata.Allegiance.NToString();
                cmbSystemGovernment.Text = m_loadedSystemdata.Government.NToString();

                setSystemEditable(isNew);

                if(!isNew)
                { 
                    cmdSystemNew.Enabled        = true;
                    cmdSystemEdit.Enabled       = true;
                    cmdSystemSave.Enabled       = false;
                    cmdSystemCancel.Enabled     = cmdSystemSave.Enabled;

                    cmdStationNew.Enabled       = true;
                    cmdStationEdit.Enabled      = false;
                    cmdStationSave.Enabled      = false;
                    cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                    cmbSystemsAllSystems.ReadOnly = isNew;
                }

                List<EDStation> StationsInSystem = _Milkyway.getStations(Systemname);
                foreach (var Station in StationsInSystem)
                {
                    cmbStationStations.Items.Add(Station.Name);
                }

                lblStationCount.Text = StationsInSystem.Count().ToString();

                cmbStationStations.SelectedIndex = 0;

            }
            else
            {
                m_currentSystemdata.clear();

                txtSystemId.Text = Program.NULLSTRING;
                txtSystemName.Text = Program.NULLSTRING;
                txtSystemX.Text = Program.NULLSTRING;
                txtSystemY.Text = Program.NULLSTRING;
                txtSystemZ.Text = Program.NULLSTRING;
                txtSystemFaction.Text = Program.NULLSTRING;
                txtSystemPopulation.Text = Program.NULLSTRING;
                txtSystemUpdatedAt.Text = Program.NULLSTRING;
                cbSystemNeedsPermit.CheckState = CheckState.Unchecked;
                cmbSystemPrimaryEconomy.Text = Program.NULLSTRING;
                cmbSystemSecurity.Text = Program.NULLSTRING;
                cmbSystemState.Text = Program.NULLSTRING;
                cmbSystemAllegiance.Text = Program.NULLSTRING;
                cmbSystemGovernment.Text = Program.NULLSTRING;

                setSystemEditable(false);

                cmdSystemNew.Enabled        = true;
                cmdSystemEdit.Enabled       = false;
                cmdSystemSave.Enabled       = false;
                cmdSystemCancel.Enabled     = cmdSystemSave.Enabled;

                cmdStationNew.Enabled       = false;
                cmdStationEdit.Enabled      = false;
                cmdStationSave.Enabled      = false;
                cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                cmbSystemsAllSystems.ReadOnly = false;

                txtSystemName.ReadOnly = true;
                lblSystemRenameHint.Visible = false;

                lblStationCount.Text = "-";
            }

            m_SystemLoadingValues = false;

        }

        private void loadStationData(string Systemname, string Stationname, bool isNew=false)
        {

            m_StationLoadingValues = true;

            if (isNew)
            {
                cmbStationStations.SelectedIndex    = -1;
                m_loadedStationdata                 = new EDStation();
                m_loadedStationdata.Name            = Stationname;
                m_StationIsNew                      = true;
            }
            else
            {
                cmbStationStations.SelectedItem    = Stationname;
                m_loadedStationdata                 = _Milkyway.getStation(Systemname, Stationname);
                m_StationIsNew                      = false;
            }

            if (m_loadedStationdata != null)
            {
                m_currentStationdata.getValues(m_loadedStationdata, true);

                txtStationId.Text = m_loadedStationdata.Id.ToString(CultureInfo.CurrentCulture);
                txtStationName.Text = m_loadedStationdata.Name.ToString();
                cmbStationMaxLandingPadSize.Text = m_loadedStationdata.MaxLandingPadSize.NToString();
                txtStationDistanceToStar.Text = m_loadedStationdata.DistanceToStar.ToNString();
                txtStationFaction.Text = m_loadedStationdata.Faction.NToString();
                cmbStationGovernment.Text = m_loadedStationdata.Government.NToString();
                cmbStationAllegiance.Text = m_loadedStationdata.Allegiance.NToString();
                cmbStationState.Text = m_loadedStationdata.State.NToString();
                cmbStationType.Text = m_loadedStationdata.Type.NToString();

                txtStationUpdatedAt.Text = m_loadedStationdata.UpdatedAt.ToString();

                lbStationEconomies.Items.Clear();

                foreach (string Economy in m_loadedStationdata.Economies)
                    lbStationEconomies.Items.Add(Economy);

                cbStationHasCommodities.CheckState = m_loadedStationdata.HasCommodities.toCheckState();
                cbStationHasBlackmarket.CheckState = m_loadedStationdata.HasBlackmarket.toCheckState();
                cbStationHasOutfitting.CheckState = m_loadedStationdata.HasOutfitting.toCheckState();
                cbStationHasShipyard.CheckState = m_loadedStationdata.HasShipyard.toCheckState();
                cbStationHasRearm.CheckState = m_loadedStationdata.HasRearm.toCheckState();
                cbStationHasRefuel.CheckState = m_loadedStationdata.HasRefuel.toCheckState();
                cbStationHasRepair.CheckState = m_loadedStationdata.HasRepair.toCheckState();

                setStationEditable(isNew);

                if(!isNew)
                {
                    cmdStationNew.Enabled       = true;
                    cmdStationEdit.Enabled      = true;
                    cmdStationSave.Enabled      = false;
                    cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                    cmbStationStations.ReadOnly = false;

                }

                if (_Milkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Exists(x => (x.Name.Equals(m_loadedStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) &&
                                                                                      (x.SystemId == m_loadedStationdata.SystemId)))
                {
                    txtStationName.ReadOnly = true;
                    lblStationRenameHint.Visible = true;
                }
                else
                {
                    txtStationName.ReadOnly = false;
                    lblStationRenameHint.Visible = false;
                }

            }
            else
            {
                m_currentStationdata.clear();

                txtStationId.Text = Program.NULLSTRING;
                txtStationName.Text = Program.NULLSTRING;
                cmbStationMaxLandingPadSize.Text = Program.NULLSTRING;
                txtStationDistanceToStar.Text = Program.NULLSTRING;
                txtStationFaction.Text = Program.NULLSTRING;
                cmbStationGovernment.Text = Program.NULLSTRING;
                cmbStationAllegiance.Text = Program.NULLSTRING;
                cmbStationState.Text = Program.NULLSTRING;
                txtStationUpdatedAt.Text = Program.NULLSTRING;
                cmbStationType.Text = Program.NULLSTRING;

                lbStationEconomies.Items.Clear();

                cbStationHasCommodities.CheckState = CheckState.Unchecked;
                cbStationHasBlackmarket.CheckState = CheckState.Unchecked;
                cbStationHasOutfitting.CheckState = CheckState.Unchecked;
                cbStationHasShipyard.CheckState = CheckState.Unchecked;
                cbStationHasRearm.CheckState = CheckState.Unchecked;
                cbStationHasRefuel.CheckState = CheckState.Unchecked;
                cbStationHasRepair.CheckState = CheckState.Unchecked;

                setStationEditable(false);

                cmdStationNew.Enabled    = cmdStationNew.Enabled;
                cmdStationEdit.Enabled   = false;
                cmdStationSave.Enabled   = false;

                lblStationRenameHint.Visible = false;

                cmbStationStations.Text = "";

            }

            m_StationLoadingValues = false;

        }

        internal void prePrepareSystemAndStationFields()
        {

            cmbSystemGovernment.Items.Add(Program.NULLSTRING);
            cmbSystemGovernment.Items.Add("Anarchy");
            cmbSystemGovernment.Items.Add("Communism");
            cmbSystemGovernment.Items.Add("Confederacy");
            cmbSystemGovernment.Items.Add("Corporate");
            cmbSystemGovernment.Items.Add("Coperative");
            cmbSystemGovernment.Items.Add("Democracy");
            cmbSystemGovernment.Items.Add("Dictatorship");
            cmbSystemGovernment.Items.Add("Feudal");
            cmbSystemGovernment.Items.Add("Imperial");
            cmbSystemGovernment.Items.Add("Patronage");
            cmbSystemGovernment.Items.Add("Colony");
            cmbSystemGovernment.Items.Add("Prison Colony");
            cmbSystemGovernment.Items.Add("Theocracy");
            cmbSystemGovernment.Items.Add("None");

            cmbSystemState.Items.Add(Program.NULLSTRING);
            cmbSystemState.Items.Add("Bust");
            cmbSystemState.Items.Add("Civil Unrest");
            cmbSystemState.Items.Add("Civil War");
            cmbSystemState.Items.Add("Expansion");
            cmbSystemState.Items.Add("Lockdown");
            cmbSystemState.Items.Add("Outbreak");
            cmbSystemState.Items.Add("War");
            cmbSystemState.Items.Add("None");

            cmbSystemAllegiance.Items.Add(Program.NULLSTRING);
            cmbSystemAllegiance.Items.Add("Alliance");
            cmbSystemAllegiance.Items.Add("Empire");
            cmbSystemAllegiance.Items.Add("Federation");
            cmbSystemAllegiance.Items.Add("Independent");
            cmbSystemAllegiance.Items.Add("None");

            cmbSystemSecurity.Items.Add(Program.NULLSTRING);
            cmbSystemSecurity.Items.Add("Low");
            cmbSystemSecurity.Items.Add("Medium");
            cmbSystemSecurity.Items.Add("High");

            cmbSystemPrimaryEconomy.Items.Add(Program.NULLSTRING);
            cmbSystemPrimaryEconomy.Items.Add("Agriculture");
            cmbSystemPrimaryEconomy.Items.Add("Extraction");
            cmbSystemPrimaryEconomy.Items.Add("High Tech");
            cmbSystemPrimaryEconomy.Items.Add("Industrial");
            cmbSystemPrimaryEconomy.Items.Add("Military");
            cmbSystemPrimaryEconomy.Items.Add("Refinery");
            cmbSystemPrimaryEconomy.Items.Add("Service");
            cmbSystemPrimaryEconomy.Items.Add("Terraforming");
            cmbSystemPrimaryEconomy.Items.Add("Tourism");
            cmbSystemPrimaryEconomy.Items.Add("None");

            txtSystemPopulation.Culture = CultureInfo.CurrentCulture;

            this.txtSystemName.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.txtSystemX.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.txtSystemY.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.txtSystemZ.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.txtSystemFaction.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.txtSystemPopulation.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.txtSystemUpdatedAt.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.cmbSystemPrimaryEconomy.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.cmbSystemSecurity.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.cmbSystemState.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.cmbSystemAllegiance.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);
            this.cmbSystemGovernment.TextChanged += new System.EventHandler(this.txtSystem_TextChanged);

            this.txtSystemName.LostFocus += txtSystem_LostFocus;
            this.txtSystemX.LostFocus += txtSystem_LostFocus;
            this.txtSystemY.LostFocus += txtSystem_LostFocus;
            this.txtSystemZ.LostFocus += txtSystem_LostFocus;
            this.txtSystemFaction.LostFocus += txtSystem_LostFocus;
            this.txtSystemPopulation.LostFocus += txtSystem_LostFocus;
            this.txtSystemUpdatedAt.LostFocus += txtSystem_LostFocus;
            this.cmbSystemPrimaryEconomy.LostFocus += txtSystem_LostFocus;
            this.cmbSystemSecurity.LostFocus += txtSystem_LostFocus;
            this.cmbSystemState.LostFocus += txtSystem_LostFocus;
            this.cmbSystemAllegiance.LostFocus += txtSystem_LostFocus;
            this.cmbSystemGovernment.LostFocus += txtSystem_LostFocus;

            this.txtSystemName.GotFocus += txtSystem_GotFocus;
            this.txtSystemX.GotFocus += txtSystem_GotFocus;
            this.txtSystemY.GotFocus += txtSystem_GotFocus;
            this.txtSystemZ.GotFocus += txtSystem_GotFocus;
            this.txtSystemFaction.GotFocus += txtSystem_GotFocus;
            this.txtSystemPopulation.GotFocus += txtSystem_GotFocus;
            this.txtSystemUpdatedAt.GotFocus += txtSystem_GotFocus;
            this.cmbSystemPrimaryEconomy.GotFocus += txtSystem_GotFocus;
            this.cmbSystemSecurity.GotFocus += txtSystem_GotFocus;
            this.cmbSystemState.GotFocus += txtSystem_GotFocus;
            this.cmbSystemAllegiance.GotFocus += txtSystem_GotFocus;
            this.cmbSystemGovernment.GotFocus += txtSystem_GotFocus;

            this.cbSystemNeedsPermit.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);


            // ********************** Station *******************

            cmbStationType.Items.Add(Program.NULLSTRING);
            cmbStationType.Items.Add("Civilian Outpost");
            cmbStationType.Items.Add("Commercial Outpost");
            cmbStationType.Items.Add("Coriolis Starport");
            cmbStationType.Items.Add("Industrial Outpost");
            cmbStationType.Items.Add("Military Outpost");
            cmbStationType.Items.Add("Mining Outpost");
            cmbStationType.Items.Add("Ocellus Starport");
            cmbStationType.Items.Add("Orbis Starport");
            cmbStationType.Items.Add("Scientific Outpost");
            cmbStationType.Items.Add("Unsanctioned Outpost");
            cmbStationType.Items.Add("Unknown Outpost");
            cmbStationType.Items.Add("Unknown Starport");

            cmbStationMaxLandingPadSize.Items.Add(Program.NULLSTRING);
            cmbStationMaxLandingPadSize.Items.Add("M");
            cmbStationMaxLandingPadSize.Items.Add("L");

            cmbStationGovernment.Items.Add(Program.NULLSTRING);
            cmbStationGovernment.Items.Add("Anarchy");
            cmbStationGovernment.Items.Add("Communism");
            cmbStationGovernment.Items.Add("Confederacy");
            cmbStationGovernment.Items.Add("Corporate");
            cmbStationGovernment.Items.Add("Coperative");
            cmbStationGovernment.Items.Add("Democracy");
            cmbStationGovernment.Items.Add("Dictatorship");
            cmbStationGovernment.Items.Add("Feudal");
            cmbStationGovernment.Items.Add("Imperial");
            cmbStationGovernment.Items.Add("Patronage");
            cmbStationGovernment.Items.Add("Colony");
            cmbStationGovernment.Items.Add("Prison Colony");
            cmbStationGovernment.Items.Add("Theocracy");
            cmbStationGovernment.Items.Add("None");

            cmbStationAllegiance.Items.Add(Program.NULLSTRING);
            cmbStationAllegiance.Items.Add("Alliance");
            cmbStationAllegiance.Items.Add("Empire");
            cmbStationAllegiance.Items.Add("Federation");
            cmbStationAllegiance.Items.Add("Independent");
            cmbStationAllegiance.Items.Add("None");

            cmbStationState.Items.Add(Program.NULLSTRING);
            cmbStationState.Items.Add("Boom");
            cmbStationState.Items.Add("Bust");
            cmbStationState.Items.Add("Civil Unrest");
            cmbStationState.Items.Add("Civil War");
            cmbStationState.Items.Add("Expansion");
            cmbStationState.Items.Add("Lockdown");
            cmbStationState.Items.Add("Outbreak");
            cmbStationState.Items.Add("War");
            cmbStationState.Items.Add("None");

            this.txtStationName.LostFocus += txtStation_LostFocus;
            this.cmbStationMaxLandingPadSize.LostFocus += txtStation_LostFocus;
            this.txtStationDistanceToStar.LostFocus += txtStation_LostFocus;
            this.txtStationFaction.LostFocus += txtStation_LostFocus;
            this.cmbStationGovernment.LostFocus += txtStation_LostFocus;
            this.cmbStationAllegiance.LostFocus += txtStation_LostFocus;
            this.cmbStationState.LostFocus += txtStation_LostFocus;
            this.cmbStationType.LostFocus += txtStation_LostFocus;

            this.txtStationName.GotFocus += txtStation_GotFocus;
            this.cmbStationMaxLandingPadSize.GotFocus += txtStation_GotFocus;
            this.txtStationDistanceToStar.GotFocus += txtStation_GotFocus;
            this.txtStationFaction.GotFocus += txtStation_GotFocus;
            this.cmbStationGovernment.GotFocus += txtStation_GotFocus;
            this.cmbStationAllegiance.GotFocus += txtStation_GotFocus;
            this.cmbStationState.GotFocus += txtStation_GotFocus;
            this.cmbStationType.GotFocus += txtStation_GotFocus;

            this.txtStationName.TextChanged += txtStation_TextChanged;
            this.cmbStationMaxLandingPadSize.TextChanged += txtStation_TextChanged;
            this.txtStationDistanceToStar.TextChanged += txtStation_TextChanged;
            this.txtStationFaction.TextChanged += txtStation_TextChanged;
            this.cmbStationGovernment.TextChanged += txtStation_TextChanged;
            this.cmbStationAllegiance.TextChanged += txtStation_TextChanged;
            this.cmbStationState.TextChanged += txtStation_TextChanged;
            this.cmbStationType.TextChanged += txtStation_TextChanged;


            this.cbStationHasCommodities.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
            this.cbStationHasBlackmarket.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
            this.cbStationHasOutfitting.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
            this.cbStationHasShipyard.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
            this.cbStationHasRearm.CheckedChanged  += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
            this.cbStationHasRefuel.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
            this.cbStationHasRepair.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
            this.cmbStationStations.SelectedIndexChanged += new System.EventHandler(this.cmbStationStations_SelectedIndexChanged);

            this.cbStationEcoAgriculture.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoExtraction.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoHighTech.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoIndustrial.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoMilitary.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoNone.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoRefinery.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoService.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoTerraforming.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);
            this.cbStationEcoTourism.CheckedChanged += new System.EventHandler(this.cmbStationEconomies_SelectedIndexChanged);

            m_SystemLoadingValues = true;
            this.cmbSystemsAllSystems.BeginUpdate();
            this.cmbSystemsAllSystems.DataSource      = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).OrderBy(x => x.Name).ToList();
            this.cmbSystemsAllSystems.ValueMember     = "Name";
            this.cmbSystemsAllSystems.DisplayMember   = "Name";
            this.cmbSystemsAllSystems.EndUpdate();
            m_SystemLoadingValues = false;
        }

        private void cmbStationEconomies_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBox_ro SenderCheckBox = (CheckBox_ro)sender;

            if(SenderCheckBox.Checked)
                if(SenderCheckBox.Name.Equals("cbStationEcoNone"))
                {
                    this.cbStationEcoAgriculture.Checked = false;
                    this.cbStationEcoExtraction.Checked = false;
                    this.cbStationEcoHighTech.Checked = false;
                    this.cbStationEcoIndustrial.Checked = false;
                    this.cbStationEcoMilitary.Checked = false;
                    this.cbStationEcoRefinery.Checked = false;
                    this.cbStationEcoService.Checked = false;
                    this.cbStationEcoTerraforming.Checked = false;
                    this.cbStationEcoTourism.Checked = false;
                }
                else
                {
                    this.cbStationEcoNone.Checked = false;
                }
        }

        void txtSystem_GotFocus(object sender, EventArgs e)
        {
            try
            {
                PropertyInfo _propertyInfo = null;
                Type _type = Type.GetType("RegulatedNoise.EDDB_Data.EDSystem");


                switch (sender.GetType().Name)
                {
                    case "TextBox":
                        _propertyInfo = _type.GetProperty(((TextBox)sender).Name.Substring(9));
                        break;
                    case "MaskedTextBox":
                        _propertyInfo = _type.GetProperty(((MaskedTextBox)sender).Name.Substring(9));
                        break;
                    case "ComboBox":
                    case "ComboBox_ro":
                        _propertyInfo = _type.GetProperty(((ComboBox)sender).Name.Substring(9));
                        break;
                }

                m_lastSystemValue = (_propertyInfo.GetValue(m_currentSystemdata, null)).NToString();

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtSystem_GotFocus-Event");                
            }
        }

        void txtStation_GotFocus(object sender, EventArgs e)
        {
            try
            {
                PropertyInfo _propertyInfo = null;
                Type _type = Type.GetType("RegulatedNoise.EDDB_Data.EDStation");


                switch (sender.GetType().Name)
                {
                    case "TextBox":
                        _propertyInfo = _type.GetProperty(((TextBox)sender).Name.Substring(10));
                        break;
                    case "MaskedTextBox":
                        _propertyInfo = _type.GetProperty(((MaskedTextBox)sender).Name.Substring(10));
                        break;
                    case "ComboBox":
                    case "ComboBox_ro":
                        _propertyInfo = _type.GetProperty(((ComboBox)sender).Name.Substring(10));
                        break;
                }
                m_lastStationValue = (_propertyInfo.GetValue(m_currentStationdata, null)).NToString();

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in txtSystem_GotFocus-Event");                
            }
        }

        void txtSystem_LostFocus(object sender, EventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "txtSystemName":
                    if (m_SystemIsNew)
                    {
                        EDSystem existing = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => x.Name.Equals(txtSystemName.Text, StringComparison.InvariantCultureIgnoreCase));
                        if (existing != null)
                            if (DateTime.Now.Subtract(m_SystemWarningTime).TotalSeconds > 5)
                            {
                                m_SystemWarningTime = DateTime.Now;
                                MsgBox.Show("A system with this name already exists", "Adding a new system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                    }
                    m_currentSystemdata.Name = txtSystemName.Text;

                    break;
                case "txtSystemX":
                    m_currentSystemdata.X = txtSystemX.Text.ToDouble(m_lastSystemValue);
                    txtSystemX.Text = m_currentSystemdata.X.ToString("0.00000", CultureInfo.CurrentCulture);
                    break;

                case "txtSystemY":
                    m_currentSystemdata.Y = txtSystemY.Text.ToDouble(m_lastSystemValue);
                    txtSystemY.Text = m_currentSystemdata.Y.ToString("0.00000", CultureInfo.CurrentCulture);
                    break;

                case "txtSystemZ":
                    m_currentSystemdata.Z = txtSystemZ.Text.ToDouble(m_lastSystemValue);
                    txtSystemZ.Text = m_currentSystemdata.Z.ToString("0.00000", CultureInfo.CurrentCulture);
                    break;

                case "txtSystemFaction":
                    m_currentSystemdata.Faction = txtSystemFaction.Text;
                    break;

                case "txtSystemPopulation":
                    m_currentSystemdata.Population = txtSystemPopulation.Text.ToNLong(m_lastSystemValue);
                    txtSystemPopulation.Text = m_currentSystemdata.Population.ToNString("#,##0.", CultureInfo.CurrentCulture);

                    break;
                case "cbSystemNeedsPermit":
                    m_currentSystemdata.NeedsPermit = cbSystemNeedsPermit.toNInt();

                    break;
                case "cmbSystemPrimaryEconomy":
                    m_currentSystemdata.PrimaryEconomy = cmbSystemPrimaryEconomy.Text.ToNString();
                    break;

                case "cmbSystemSecurity":
                    m_currentSystemdata.Security = cmbSystemSecurity.Text.ToNString();

                    break;
                case "cmbSystemState":
                    m_currentSystemdata.State = cmbSystemState.Text.ToNString();

                    break;
                case "cmbSystemAllegiance":
                    m_currentSystemdata.Allegiance = cmbSystemAllegiance.Text.ToNString();

                    break;
                case "cmbSystemGovernment":
                    m_currentSystemdata.Government = cmbSystemGovernment.Text.ToNString();

                    break;
            }

        }

        void txtStation_LostFocus(object sender, EventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "txtStationName":
                    if (m_StationIsNew)
                    {
                        EDStation existing = _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(txtStationName.Text, StringComparison.InvariantCultureIgnoreCase)) &&
                                                                                                                (x.SystemId == m_currentStationdata.SystemId));
                        if (existing != null)
                        {
                            if (DateTime.Now.Subtract(m_StationWarningTime).TotalSeconds > 5)
                            {
                                m_StationWarningTime = DateTime.Now;
                                MsgBox.Show("A Station with this name already exists", "Adding a new Station", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                    m_currentStationdata.Name = txtStationName.Text;
                    break;

                case "cmbStationMaxLandingPadSize":
                    m_currentStationdata.MaxLandingPadSize = cmbStationMaxLandingPadSize.Text.ToNString();
                    break;

                case "txtStationDistanceToStar":
                     m_currentStationdata.DistanceToStar = txtStationDistanceToStar.Text.Replace(",", "").Replace(".", "").ToNInt(m_lastStationValue);
                    txtStationDistanceToStar.Text = m_currentStationdata.DistanceToStar.ToNString("#,##0.", CultureInfo.CurrentCulture);
                    break;

                case "txtStationFaction":
                    m_currentStationdata.Faction = txtStationFaction.Text.ToNString();
                    break;

                case "lbStationEconomies":
                    m_currentStationdata.Economies = new String[lbStationEconomies.Items.Count];
                    for (int i = 0; i < lbStationEconomies.Items.Count; i++)
                    {
                        m_currentStationdata.Economies[i] = lbStationEconomies.Items[i].ToString();
                    }
                    break;

                case "cmbStationState":
                    m_currentStationdata.State = cmbStationState.Text.ToNString();
                    break;

                case "cmbStationType":
                    m_currentStationdata.Type = cmbStationType.Text.ToNString();
                    break;

                case "cmbStationAllegiance":
                    m_currentStationdata.Allegiance = cmbStationAllegiance.Text.ToNString();
                    break;

                case "cmbStationGovernment":
                    m_currentStationdata.Government = cmbStationGovernment.Text.ToNString();
                    break;
            }

        }

        private void CheckBox_StationSystem_CheckedChanged(object sender, EventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "cbSystemNeedsPermit":
                    m_currentSystemdata.NeedsPermit = cbSystemNeedsPermit.toNInt();
                    break;

                case "cbStationHasCommodities":
                    m_currentStationdata.HasCommodities = cbStationHasCommodities.toNInt();
                    break;

                case "cbStationHasBlackmarket":
                    m_currentStationdata.HasBlackmarket = cbStationHasBlackmarket.toNInt();
                    break;

                case "cbStationHasOutfitting":
                    m_currentStationdata.HasOutfitting = cbStationHasOutfitting.toNInt();
                    break;

                case "cbStationHasShipyard":
                    m_currentStationdata.HasShipyard = cbStationHasShipyard.toNInt();
                    break;

                case "cbStationHasRearm":
                    m_currentStationdata.HasRearm = cbStationHasRearm.toNInt();
                    break;

                case "cbStationHasRefuel":
                    m_currentStationdata.HasRefuel = cbStationHasRefuel.toNInt();
                    break;

                case "cbStationHasRepair":
                    m_currentStationdata.HasRepair = cbStationHasRepair.toNInt();
                    break;
            }

        }

        private void txtSystem_TextChanged(object sender, EventArgs e)
        {
            if (!m_SystemLoadingValues)
            {
                cmdSystemNew.Enabled    = false;
                cmdSystemEdit.Enabled   = false;
                cmdSystemSave.Enabled   = true;
                cmdSystemCancel.Enabled = cmdSystemSave.Enabled;

                cmdStationNew.Enabled       = false;
                cmdStationEdit.Enabled      = false;
                cmdStationSave.Enabled      = false;
                cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                cmbSystemsAllSystems.ReadOnly = true;
            }
        }

        private void txtStation_TextChanged(object sender, EventArgs e)
        {
            if (!m_StationLoadingValues)
            {
                cmdStationNew.Enabled    = false;
                cmdStationEdit.Enabled   = false;
                cmdStationSave.Enabled   = true;
            }
        }

        private void cmdSystemSave_Click(object sender, EventArgs e)
        {
            EDSystem existing = null;
            bool newComboBoxRefresh = false;

            if (m_SystemIsNew)
            {
                // adding a new system
                existing = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => x.Name.Equals(m_currentSystemdata.Name, StringComparison.InvariantCultureIgnoreCase));
                if (existing != null)
                {
                    MsgBox.Show("A system with this name already exists", "Adding a new system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //MsgBox.Show("A system with this name already exists", "Adding a new system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                newComboBoxRefresh = true;
            }
            else if(!_oldSystemName.Equals(m_currentSystemdata.Name))
            {
                // changing system name
                existing = _Milkyway.getSystems(EDMilkyway.enDataType.Data_EDDB).Find(x => x.Name.Equals(_oldSystemName, StringComparison.InvariantCultureIgnoreCase));
                if (existing != null)
                {
                    MsgBox.Show("It's not allowed to rename a EDDB system", "renaming system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                existing = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => x.Name.Equals(m_currentSystemdata.Name, StringComparison.InvariantCultureIgnoreCase));
                if (existing != null)
                {
                    MsgBox.Show("A system with the new name's already existing", "renaming system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                newComboBoxRefresh = true;
            }

            if (MsgBox.Show("Save changes on current system ?", "Stationdata changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                if (m_SystemIsNew)
                    _oldSystemName = "";
                
                _Milkyway.ChangeAddSystem(m_currentSystemdata, _oldSystemName);
                setSystemEditable(false);

                if(newComboBoxRefresh)
                { 
                    m_SystemLoadingValues = true;
                    cmbSystemsAllSystems.BeginUpdate();

                    cmbSystemsAllSystems.DataSource      = null;
                    cmbSystemsAllSystems.DataSource      = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).OrderBy(x => x.Name).ToList();
                    cmbSystemsAllSystems.ValueMember     = "Name";
                    cmbSystemsAllSystems.DisplayMember   = "Name";
                    cmbSystemsAllSystems.Refresh();

                    cmbSystemsAllSystems.EndUpdate();
                    m_SystemLoadingValues = false;
                }

                loadSystemData(m_currentSystemdata.Name);
                loadStationData(m_currentSystemdata.Name, txtStationName.Text);

                showSystemNumbers();

                Cursor = Cursors.Default;
            }
        }

        private void cmdStationSave_Click(object sender, EventArgs e)
        {
            EDStation existing = null;

            if (m_StationIsNew)
            {
                // adding a new Station
                existing = _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(m_currentStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) && 
                                                                                              (x.SystemId.Equals(m_currentSystemdata.Id)));
                if (existing != null)
                {
                    MsgBox.Show("A station with this name already exists", "Adding a new station", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if(!_oldStationName.Equals(m_currentStationdata.Name))
            {
                // changing Station name
                existing = _Milkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Find(x => (x.Name.Equals(_oldStationName, StringComparison.InvariantCultureIgnoreCase)) && 
                                                                                            (x.SystemId.Equals(m_currentSystemdata.Id)));
                if (existing != null)
                {
                    MsgBox.Show("It's not allowed to rename a EDDB station", "renaming station", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                existing = _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(m_currentStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) && 
                                                                                              (x.SystemId.Equals(m_currentSystemdata.Id)));
                if (existing != null)
                {
                    MsgBox.Show("A station with the new name's already existing", "renaming station", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (MsgBox.Show("Save changes on current station ?", "stationdata changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
            {
                if (m_StationIsNew)
                    _oldStationName = "";

                Cursor = Cursors.WaitCursor;
                _Milkyway.ChangeAddStation(m_currentSystemdata.Name, m_currentStationdata, _oldStationName);

                cmbSystemsAllSystems.ReadOnly   = false;
                cmbStationStations.ReadOnly     = false;

                setStationEditable(false);

                loadSystemData(m_currentSystemdata.Name);
                loadStationData(m_currentSystemdata.Name, m_currentStationdata.Name);

                showSystemNumbers();

                Cursor = Cursors.Default;
            }
        }

        private void cmdSystemNew_Click(object sender, EventArgs e)
        {

            _oldSystemName  = m_currentSystemdata.Name;
            _oldStationName = m_currentStationdata.Name;

            string newSystemname = tbCurrentSystemFromLogs.Text;

            if(InpBox.Show("create a new system", "insert the name of the new system", ref newSystemname) == System.Windows.Forms.DialogResult.OK)
            { 
                
                EDSystem existing = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(newSystemname, StringComparison.InvariantCultureIgnoreCase)));
                if (existing != null)
                {
                    MsgBox.Show("A system with this name already exists", "Adding a new system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    setSystemEditable(true);

                    cmdSystemNew.Enabled        = false;
                    cmdSystemEdit.Enabled       = false;
                    cmdSystemSave.Enabled       = true;
                    cmdSystemCancel.Enabled     = cmdSystemSave.Enabled;

                    cmdStationNew.Enabled       = false;
                    cmdStationEdit.Enabled      = false;
                    cmdStationSave.Enabled      = false;
                    cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                    this.cmbSystemsAllSystems.SelectedIndexChanged -= new System.EventHandler(this.cmbAllStations_SelectedIndexChanged);
                    // it twice needed (?!)
                    cmbSystemsAllSystems.SelectedIndex = -1;
                    cmbSystemsAllSystems.SelectedIndex = -1;
                    this.cmbSystemsAllSystems.SelectedIndexChanged += new System.EventHandler(this.cmbAllStations_SelectedIndexChanged);

                    cmbSystemsAllSystems.ReadOnly = true;

                    cmbStationStations.ReadOnly = false;

                    loadSystemData(newSystemname, true);
                    loadStationData(newSystemname, "", false);
                }
            }
        }

        private void cmdStationNew_Click(object sender, EventArgs e)
        {
            _oldSystemName  = m_currentSystemdata.Name;
            _oldStationName = m_currentStationdata.Name;

            string newStationname = tbCurrentStationinfoFromLogs.Text;

            EDStation existing = _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(newStationname, StringComparison.InvariantCultureIgnoreCase)) && 
                                                                                                    (x.SystemId.Equals(m_currentSystemdata.Id)));
            if (existing != null)
                newStationname = String.Empty;

            if(InpBox.Show("create a new station", "insert the name of the new station", ref newStationname) == System.Windows.Forms.DialogResult.OK)
            { 
                existing = _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(newStationname, StringComparison.InvariantCultureIgnoreCase)) && 
                                                                                              (x.SystemId.Equals(m_currentSystemdata.Id)));

                if (existing != null)
                {
                    MsgBox.Show("A station with this name already exists in this system", "Adding a new station", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    cmdSystemNew.Enabled        = false;
                    cmdSystemEdit.Enabled       = false;
                    cmdSystemSave.Enabled       = false;
                    cmdSystemCancel.Enabled     = cmdSystemSave.Enabled;

                    cmdStationNew.Enabled       = false;
                    cmdStationEdit.Enabled      = false;
                    cmdStationSave.Enabled      = true;
                    cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                    loadStationData(txtSystemName.Text, newStationname, true);

                    cmbSystemsAllSystems.ReadOnly = true;

                    cmbStationStations.ReadOnly = true;

                    setStationEditable(true);

                }
            }
        }

        void lbStationEconomies_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(!lbStationEconomies.Tag.Equals("ReadOnly"))
            {
                foreach (var item in paEconomies.Controls)
	            {
                    Debug.Print(item.GetType().ToString());
		            if(item.GetType() == typeof(CheckBox_ro))
                    {
                        var EcoName = ((CheckBox)item).Text;
                        ((CheckBox)item).Checked = lbStationEconomies.Items.Contains(EcoName);
                    }
	            }
                paEconomies.Location = lbStationEconomies.Location;
                paEconomies.Visible = true;
            }
        }

        private void cmbStationStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!m_StationLoadingValues && !m_SystemLoadingValues)
                loadStationData(txtSystemName.Text, cmbStationStations.Text);
        }

        public delegate void delBoolean(bool value);

        private void setSystemEditable(bool enabled)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new delBoolean(setSystemEditable), enabled);
                    return;
                }

                txtSystemId.ReadOnly                = true;
                txtSystemX.ReadOnly                 = !enabled;
                txtSystemY.ReadOnly                 = !enabled;
                txtSystemZ.ReadOnly                 = !enabled;
                txtSystemFaction.ReadOnly           = !enabled;
                txtSystemPopulation.ReadOnly        = !enabled;
                cmbSystemGovernment.ReadOnly        = !enabled;
                cmbSystemAllegiance.ReadOnly        = !enabled;
                cmbSystemState.ReadOnly             = !enabled;
                cmbSystemSecurity.ReadOnly          = !enabled;
                cmbSystemPrimaryEconomy.ReadOnly    = !enabled;
                cbSystemNeedsPermit.ReadOnly        = !enabled;
                txtSystemUpdatedAt.ReadOnly         = true;

                if(enabled)
                    if (_Milkyway.getSystems(EDMilkyway.enDataType.Data_EDDB).Exists(x => x.Name.Equals(m_loadedSystemdata.Name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        txtSystemName.ReadOnly      = true;
                        lblSystemRenameHint.Visible = true;
                    }
                    else
                    {
                        txtSystemName.ReadOnly      = false;
                        lblSystemRenameHint.Visible = false;
                    }
                else
                {
                    txtSystemName.ReadOnly          = true;
                    lblSystemRenameHint.Visible     = false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting readOnly-property for system data", ex);       
            }
        }

        private void setStationEditable(bool enabled)
        {
            try
            {

                if(this.InvokeRequired)
                { 
                    this.Invoke(new delBoolean(setStationEditable), enabled);
                    return;
                }

                txtStationId.ReadOnly                   = true;
                cmbStationMaxLandingPadSize.ReadOnly    = !enabled;
                txtStationDistanceToStar.ReadOnly       = !enabled;
                txtStationFaction.ReadOnly              = !enabled;
                cmbStationGovernment.ReadOnly           = !enabled;
                cmbStationAllegiance.ReadOnly           = !enabled;
                cmbStationState.ReadOnly                = !enabled;
                cmbStationType.ReadOnly                 = !enabled;

                cbStationHasCommodities.ReadOnly        = !enabled;
                cbStationHasBlackmarket.ReadOnly        = !enabled;
                cbStationHasOutfitting.ReadOnly         = !enabled;
                cbStationHasShipyard.ReadOnly           = !enabled;
                cbStationHasRearm.ReadOnly              = !enabled;
                cbStationHasRefuel.ReadOnly             = !enabled;
                cbStationHasRepair.ReadOnly             = !enabled;

                txtStationUpdatedAt.ReadOnly            = true;

                if(enabled)
                    lbStationEconomies.Tag = "";
                else
                    lbStationEconomies.Tag = "ReadOnly";

                if(enabled)
                    if (_Milkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Exists(x => (x.Name.Equals(m_loadedStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) && 
                                                                                           (x.SystemId.Equals(m_currentSystemdata.Id))))
                    {
                        txtStationName.ReadOnly      = true;
                        lblStationRenameHint.Visible = true;
                    }
                    else
                    {
                        txtStationName.ReadOnly      = false;
                        lblStationRenameHint.Visible = false;
                    }
                else
                {
                    txtStationName.ReadOnly          = true;
                    lblStationRenameHint.Visible     = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting readOnly-property for station data", ex);       
            }

        }


        private void cmdSystemEdit_Click(object sender, EventArgs e)
        {
            try
            {
                setSystemEditable(true);

                cmdSystemNew.Enabled        = false;
                cmdSystemEdit.Enabled       = false;
                cmdSystemSave.Enabled       = true;
                cmdSystemCancel.Enabled     = cmdSystemSave.Enabled;

                cmdStationNew.Enabled       = false;
                cmdStationEdit.Enabled      = false;
                cmdStationSave.Enabled      = false;
                cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                cmbSystemsAllSystems.ReadOnly = true;

                cmbStationStations.ReadOnly = true;

                _oldSystemName  = m_currentSystemdata.Name;
                _oldStationName = m_currentStationdata.Name;

            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error when starting station edit");
            }
        }

        private void cmdStationEdit_Click(object sender, EventArgs e)
        {
            try
            {
                setStationEditable(true);

                cmdSystemNew.Enabled        = false;
                cmdSystemEdit.Enabled       = false;
                cmdSystemSave.Enabled       = false;
                cmdSystemCancel.Enabled     = cmdSystemSave.Enabled;

                cmdStationNew.Enabled       = false;
                cmdStationEdit.Enabled      = false;
                cmdStationSave.Enabled      = true;
                cmdStationCancel.Enabled    = cmdStationSave.Enabled;

                cmbSystemsAllSystems.ReadOnly = true;

                cmbStationStations.ReadOnly = true;

                _oldSystemName  = m_currentSystemdata.Name;
                _oldStationName = m_currentStationdata.Name;

            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error when starting station edit");
            }
        }


#endregion

        
        private void cbIncludeUnknownDTS_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.IncludeUnknownDTS = cbIncludeUnknownDTS.Checked;
            SaveSettings();
            SetupGui();
        }

        private void cbMaxRouteDistance_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.MaxRouteDistance = cbMaxRouteDistance.Checked;
            SaveSettings();

            
        }

        private void cbAutoActivateSystem_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.AutoActivateSystemTab = cbAutoActivateSystemTab.Checked;
            SaveSettings();
        }

        private void cbLoadStationsJSON_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.LoadStationsJSON = cbLoadStationsJSON.Checked;
        }

        public string getAppPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            this.cmbSystemsAllSystems.SelectedIndexChanged -= new System.EventHandler(this.cmbAllStations_SelectedIndexChanged);
            cmbSystemsAllSystems.SelectedIndex = -1;
            cmbSystemsAllSystems.SelectedIndex = -1;
            this.cmbSystemsAllSystems.SelectedIndexChanged += new System.EventHandler(this.cmbAllStations_SelectedIndexChanged);
        }

        private void cmbAllStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_SystemLoadingValues)
            {
                loadSystemData(cmbSystemsAllSystems.SelectedValue.ToString());
                loadStationData(cmbSystemsAllSystems.SelectedValue.ToString(), "");
            }
        }

        private void cmdSystemCancel_Click(object sender, EventArgs e)
        {
            if (MsgBox.Show("Dismiss changes ?", "Systemdata changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
            {
                cmbSystemsAllSystems.ReadOnly   = false;
                cmbStationStations.ReadOnly     = false;

                loadSystemData(_oldSystemName);
                loadStationData(_oldSystemName, _oldStationName);
            }
        }

        private void cmdStationCancel_Click(object sender, EventArgs e)
        {
            if (MsgBox.Show("Dismiss changes ?", "Stationdata changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
            {
                cmbSystemsAllSystems.ReadOnly   = false;
                cmbStationStations.ReadOnly     = false;

                loadSystemData(_oldSystemName);
                loadStationData(_oldSystemName, _oldStationName);
            }

        }

        void tabCtrlMain_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        void tabCtrlMain_Selecting(object sender, System.Windows.Forms.TabControlCancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
        }

        private void cmdStationEco_OK_Click(object sender, EventArgs e)
        {
            lbStationEconomies.Items.Clear();
            

            foreach (var item in paEconomies.Controls)
	        {
		        if(item.GetType() == typeof(CheckBox_ro))
                {
                    var EcoName = ((CheckBox)item).Text;
                    if(((CheckBox)item).Checked)
                        lbStationEconomies.Items.Add(EcoName);
                       
                }
	        }

            m_currentStationdata.Economies = new String[lbStationEconomies.Items.Count];
            lbStationEconomies.Items.CopyTo(m_currentStationdata.Economies, 0);

            paEconomies.Visible = false;
        }

        private void cmdPurgeOldData_Click(object sender, EventArgs e)
        {

            if(MsgBox.Show(String.Format("Delete all data older than {0} days", nudPurgeOldDataDays.Value), "Delete old price data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.OK)
            {
                DateTime deadline = DateTime.Now.AddDays(-1*(Int32)(nudPurgeOldDataDays.Value)).Date;
                StationDirectory = PurgeOldDataFromDirectory(StationDirectory, deadline);
                CommodityDirectory = PurgeOldDataFromDirectory(CommodityDirectory, deadline);
                SetupGui();
            }

        }

        private static ObjectDirectory PurgeOldDataFromDirectory(ObjectDirectory directory, DateTime deadline)
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
                    if (y.SampleDate >= deadline)
                        newList.Add(y);

                if(newList.Count > 0)
                    newDirectory.Add(x.Key, newList);
            }
            return newDirectory;
        }

        private void nudPurgeOldDataDays_ValueChanged(object sender, EventArgs e)
        {
            if(_InitDone)
            {
                RegulatedNoiseSettings.oldDataPurgeDeadlineDays = (Int32)(nudPurgeOldDataDays.Value);
            }

        }

        private void cbAutoAdd_Visited_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.AutoEvent_Visited              = cbAutoAdd_Visited.Checked;
        }

        private void cbAutoAdd_Marketdata_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.AutoEvent_MarketDataCollected  = cbAutoAdd_Marketdata.Checked;      
        }

        private void cbAutoAdd_ReplaceVisited_CheckedChanged(object sender, EventArgs e)
        {
            RegulatedNoiseSettings.AutoEvent_ReplaceVisited       = cbAutoAdd_ReplaceVisited.Checked;  
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://github.com/Duke-Jones/RegulatedNoise/releases");
        }

        private void llVisitUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

    }
}
