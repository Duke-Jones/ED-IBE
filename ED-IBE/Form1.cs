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
using IBE.Enums_and_Utility_Classes;
using Microsoft.Win32;
using System.ComponentModel;
using IBE.EDDB_Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CodeProject.Dialog;
using IBE.SQL;
using IBE.MTCommandersLog;
using IBE.MTPriceAnalysis;
using IBE.MTSettings;
using System.Text.RegularExpressions;
using IBE.ExtData;
using IBE.Ocr;

namespace IBE
{
    public partial class Form1 : RNBaseForm
    {

/* ************************************************ */
 // new stuff

        private Dictionary<String, Boolean> m_IsRefreshed;

/* ************************************************ */

        private const string STR_START_MARKER = "<START>";

        public override string thisObjectName { get { return "Form1"; } }

        const string ID_DELIMITER = "empty";
        const int MAX_NAME_LENGTH = 120;

        const string ID_NEWITEM = "<NEW>";
        const string ID_NOT_SET = "<NOT_SET>";

        private delegate void delButtonInvoker(Button myButton, bool enable);
        private delegate void delCheckboxInvoker(CheckBox myCheckbox, bool setChecked);
        private delegate void delParamBool(bool Value);

        public static Form1 InstanceObject;
        
        public IBE.EDDN.EDDNCommunicator EDDNComm;
        public Random random = new Random();
        public Guid SessionGuid;
        public PropertyInfo[] LogEventProperties;
        public static GameSettings GameSettings;

        //public Dictionary<byte, string> CommodityLevel = new Dictionary<byte, string>();
        private Ocr.Ocr ocr; //TODO is this needed here?
        private ListViewColumnSorter _stationColumnSorter, _commodityColumnSorter, _allCommodityColumnSorter, _stationToStationColumnSorter, _stationToStationReturnColumnSorter, _commandersLogColumnSorter;
        private Thread _eddnSubscriberThread;
        
        public TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;
        public Levenshtein _levenshtein = new Levenshtein();
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
        private DateTime m_lastEDDNAutoImport                           = DateTime.MinValue;
        private System.Timers.Timer _AutoImportDelayTimer;

        public tabOCR cOcrCaptureAndCorrect               =  new tabOCR();

        [SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
        public Form1()
        {
            Program.SplashScreen.InfoAdd("initialize components...");
            InitializeComponent();
            Program.Logger.Log("  - initialised component");
            Program.SplashScreen.InfoAppendLast("<OK>");
        }

        private void doInit()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                TabPage     newTab;

                _InitDone = false ;

                InstanceObject = this;

                string FormName = this.GetType().Name;
                Program.SplashScreen.setPosition(this.GetWindowData());

                Boolean retry = false;
                do
                {
                    try
                    {
                        Program.SplashScreen.InfoAdd("load settings...");
                        SetProductPath();
                        Program.Logger.Log("  - product path set");
                        Program.SplashScreen.InfoAppendLast("<OK>");

                        SetProductAppDataPath();
                        Program.Logger.Log("  - product appdata set");

                        Program.SplashScreen.InfoAdd("load game settings...");
                        GameSettings = new GameSettings(this);
                        Program.Logger.Log("  - loaded game settings");
                        Program.SplashScreen.InfoAppendLast("<OK>");

                        retry = false;
                    }
                    catch (Exception ex)
                    {
                        if (retry)
                            throw new Exception("could't verify productpath and/or gamepath", ex);

                        Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ProductsPath", "");
                        Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GamePath", "");
                        retry = true;
                    }
                } while (retry);

                Program.SplashScreen.InfoAdd("prepare network interfaces...");
                PopulateNetworkInterfaces();
                Program.Logger.Log("  - populated network interfaces");
                Program.SplashScreen.InfoAppendLast("<OK>");

                /*Program.SplashScreen.InfoAdd("create OCR object...");
               // ocr = new Ocr.Ocr(this); //moved to ocrcaptureandcorrect
                Program.Logger.Log("  - created OCR object");
                Program.SplashScreen.InfoChange("create OCR object...<OK>"); */

                Application.ApplicationExit += Application_ApplicationExit;
                Program.Logger.Log("  - set application exit handler");

                Program.SplashScreen.InfoAdd("initiate ocr...");
                var OcrCapAndCorrectTabPage = new TabPage("Capture And Correct");
                OcrCapAndCorrectTabPage.Name = "OCR_CaptureAndCorrect";
                cOcrCaptureAndCorrect.Dock = DockStyle.Fill;
                cOcrCaptureAndCorrect._parent = this;

                if(Debugger.IsAttached)
                {
                    OcrCapAndCorrectTabPage.Controls.Add(cOcrCaptureAndCorrect);
                    tabCtrlOCR.Controls.Add(OcrCapAndCorrectTabPage);
                    Program.Logger.Log("  - initialised Ocr ");
                    Program.SplashScreen.InfoAppendLast("<OK>");

                    Program.SplashScreen.InfoAdd("create ocr calibrator...");
                    var OcrCalibratorTabPage = new TabPage("OCR Calibration");
                    OcrCalibratorTabPage.Name = "OCR_Calibration";
                    var oct = new OcrCalibratorTab { Dock = DockStyle.Fill };
                    OcrCalibratorTabPage.Controls.Add(oct);
                    tabCtrlOCR.Controls.Add(OcrCalibratorTabPage);
                    Program.Logger.Log("  - initialised Ocr Calibrator");
                    Program.SplashScreen.InfoAppendLast("<OK>");
                }

                Program.SplashScreen.InfoAdd("prepare EDDN interface...");
                EDDNComm = new IBE.EDDN.EDDNCommunicator(this);
                Program.Logger.Log("  - created EDDN object");
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("apply settings...");
                ApplySettings();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.Logger.Log("  - applied settings");

                if (!Directory.Exists(Program.GetDataPath("OCR Correction Images")))
                    Directory.CreateDirectory(Program.GetDataPath("OCR Correction Images"));

    //DEBUG: removed for the moment because I got strange behaviour in an full new/uninitialized environment (-> investigation needed)
                //setOCRTabsVisibility();

                Program.SplashScreen.InfoAdd("prepare system/location view...");
                //prePrepareSystemAndStationFields();
                if (Debugger.IsAttached && Program.showToDo)
                    MessageBox.Show("todo");
                Program.SplashScreen.InfoChange("prepare system/location view...<OK>");

                Program.SplashScreen.InfoAdd("prepare GUI elements...");
                SetupGui(true);
                Program.SplashScreen.InfoChange("prepare GUI elements...<OK>");

                if (((DateTime.Now.Day == 24 || DateTime.Now.Day == 25 || DateTime.Now.Day == 26) &&
                     DateTime.Now.Month == 12) || (DateTime.Now.Day == 31 && DateTime.Now.Month == 12) ||
                    (DateTime.Now.Day == 1 && DateTime.Now.Month == 1))
                {
                    _timer = new System.Windows.Forms.Timer();
                    _timer.Interval = 75;
                    _timer.Tick += OnTick;
                    _timer.Start();
                }

                Retheme();

                Clock = new System.Windows.Forms.Timer();
                Clock.Interval = 1000;
                Clock.Start();
                Clock.Tick += Clock_Tick;

                _AutoImportDelayTimer            = new System.Timers.Timer(10000);
                _AutoImportDelayTimer.AutoReset  = false;
                _AutoImportDelayTimer.Elapsed   += AutoImportDelayTimer_Elapsed;


                Program.SplashScreen.InfoAdd("initialize settings tab...");
                // Settings
                tabSettings newSControl           = new tabSettings();
                newSControl.DataSource            = Program.Settings;
                newTab                            = new TabPage("Settings");
                newTab.Name                       = newSControl.Name;
                newTab.Controls.Add(newSControl);
                tabCtrlMain.TabPages.Insert(5, newTab);
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("initialize settings tab...");
                // Price Analysis
                tabPriceAnalysis newPAControl     = new tabPriceAnalysis();
                newPAControl.DataSource           = Program.PriceAnalysis;
                newTab                            = new TabPage("Price Analysis");
                newTab.Name                       = newPAControl.Name;
                newTab.Controls.Add(newPAControl);
                tabCtrlMain.TabPages.Insert(2, newTab);
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("initialize settings tab...");
                // Commander's Log
                tabCommandersLog newCLControl     = new tabCommandersLog();
                newCLControl.DataSource           = Program.CommandersLog;
                newTab                            = new TabPage("Commander's Log");
                newTab.Name                       = newCLControl.Name;
                newTab.Controls.Add(newCLControl);
                tabCtrlMain.TabPages.Insert(3, newTab);
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("initialize message handlers...");
                // fill dictionary with "RefreshDone"-flags
                m_IsRefreshed      = new Dictionary<string,bool>();
                foreach (TabPage MainTabPage in tabCtrlMain.TabPages)
                    m_IsRefreshed.Add(MainTabPage.Name, false);

                // register events for getting new location-infos for the gui
                Program.LogfileScanner.LocationChanged += LogfileScanner_LocationChanged;

                Program.ExternalData.ExternalDataEvent += ExternalDataInterface_ExternalDataEvent;


                // until this is working again 
                tabCtrlMain.TabPages.Remove(tabCtrlMain.TabPages["tabSystemData"]);
                tabCtrlMain.TabPages.Remove(tabCtrlMain.TabPages["tabWebserver"]);
                tabCtrlMain.TabPages.Remove(tabCtrlMain.TabPages["tabEDDN"]);

                Cursor = Cursors.Default;
                _InitDone = true;

                Program.SplashScreen.InfoAppendLast("<OK>");
            }
            catch (Exception ex)
            {
                throw new Exception("Error in main-init-routine", ex);
            }
        }

        private string EscapeLikeValue(string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                switch (c)
                {
                    case ']':
                    case '[':
                    case '%':
                    case '*':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        private string getProductPathAutomatically()
        {
            // check in typical directorys
            string[] autoSearchdir = { Environment.GetEnvironmentVariable("ProgramW6432"), 
                                       Environment.GetEnvironmentVariable("PROGRAMFILES(X86)"), 
                                       Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432"), @"Steam\steamapps\common"), 
                                       Path.Combine(Environment.GetEnvironmentVariable("PROGRAMFILES(X86)"), @"Steam\steamapps\common") };

            string returnValue = null;
            foreach (var directory in autoSearchdir)
            { 
                if (directory == null) continue;

                if(Directory.Exists(directory))
                {
                    foreach (var dir in Directory.GetDirectories(directory))
                    {
                        if ((Path.GetFileName(dir) != "Frontier") && 
                            (Path.GetFileName(dir) != "Frontier_Developments") && 
                            (Path.GetFileName(dir) != "Elite Dangerous") && 
                            (Path.GetFileName(dir) != "Elite Dangerous Horizons")) 
                            continue;
                       String p;

                        p = Path.Combine(dir, "Products");
                        if(Directory.Exists(p))
                        {
                            returnValue = p;
                            break;
                        }

                        p = Path.Combine(dir, "EDLaunch", "Products");
                        if(Directory.Exists(p))
                        {
                            returnValue = p;
                            break;
                        }
                    }
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
                    Debug.Print(key.GetValue(Names[i]).ToString());
                    if (key.GetValue(Names[i]).ToString() == ProgramName)
                    {
                        ProgramPath = Names[i].ToString();

					    ProgramPath = ProgramPath.Substring(0, ProgramPath.LastIndexOf(@"\Products\") + 9);
						returnValue = Directory.Exists(ProgramPath) ? ProgramPath : null;
                        break;					
                    }

                }
            }

            return returnValue;
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
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(dialog.SelectedPath);
                        String parentDir = di.Parent.FullName;              

                        if (Path.GetFileName(parentDir) == "Products")
                            return parentDir;
                    }


                }
                else
                    return null;

                var MBResult = MsgBox.Show(
                    "Hm, that doesn't seem right" +
                    (dialog.SelectedPath != "" ? ", " + dialog.SelectedPath + " isn't the Frontier 'Products' directory"  : "")
                + ". Please try again...", "", MessageBoxButtons.RetryCancel);

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Environment.Exit(-1);
            }
        }
        private void SetProductPath()
        {
            //Already set, no reason to set it again :)
            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ProductsPath", "") != "" && Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GamePath", "") != "") return;
            
            //Automatic
            var path = getProductPathAutomatically();

            //Automatic failed, Ask user to find it manually
            if (path == null)
            {
                var MBResult = MsgBox.Show("Automatic discovery of Frontier directory failed, please point me to your Frontier 'Products' directory.", "", MessageBoxButtons.OKCancel);

                if (MBResult != System.Windows.Forms.DialogResult.Cancel)
                    path = getProductPathManually();
                else
                   Environment.Exit(-1);
            }

            if (path != null)
            {
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
                        if (Path.GetFileName(dir).StartsWith("elite-dangerous-64"))
                        {
                            gamedirs.Add(dir);
                        }
                    }

                    if (gamedirs.Count > 0)
                    {
                        DateTime youngestDate = new DateTime(2000, 1, 1);
                        String   youngestPath = "";

                        foreach (String foundDir in gamedirs)
                        {
                            String currentDir = Path.Combine(foundDir, "Logs");

                            if (Directory.Exists(currentDir))
                            {
                                FileInfo[] filesInDirectory = new DirectoryInfo(currentDir).GetFiles("netLog.*.log", SearchOption.TopDirectoryOnly);
                                List<FileInfo> ordered      = (from te in filesInDirectory orderby te.CreationTimeUtc descending select te).ToList();

                                if((ordered.Count > 0) && (ordered[0].CreationTimeUtc > youngestDate))
                                {
                                    youngestDate  = ordered[0].CreationTimeUtc;
                                    youngestPath  = foundDir;
                                }
                            }
                        }

                        if (youngestPath != "")
                        {
                            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GamePath", youngestPath);
                        }
                        else
                        {
                            String x64Dir = gamedirs.FirstOrDefault(x => Path.GetFileName(x) == "elite-dangerous-64");

                            if(x64Dir != null)
                            {
                                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GamePath", x64Dir);
                            }
                            else
                            {
                                //Get highest Forc-fdev dir.
                                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "GamePath", gamedirs.OrderByDescending(x => x).ToArray()[0]);
                            }
                        }

                        b = true;
                        continue;

                    }

                    var MBResult = MsgBox.Show("Couldn't find a FORC-FDEV.. directory in the Frontier Products dir, please try again...", "", MessageBoxButtons.RetryCancel);

                    if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                        Environment.Exit(-1);

                    path = getProductPathManually();
                    dirs = Directory.GetDirectories(path);
                }

                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ProductsPath", path);
            }
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
            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ProductAppData") != "") return;

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

            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ProductAppData", path);
        }

        private void ApplySettings()
        {
            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverForegroundColor") != "") 
                tbForegroundColour.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverForegroundColor");

            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverBackgroundColor") != "") 
                tbBackgroundColour.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverBackgroundColor");

            txtWebserverPort.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverPort");

            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverIpAddress") != "") 
                cbInterfaces.Text = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverIpAddress");
            
            ShowSelectedUiColours();

            if (Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "StartWebserverOnLoad", false.ToString(), false, true))
            {
                cbStartWebserverOnLoad.Checked = true;
                bStart_Click(null, null);
            }
   

            cbSpoolEddnToFile.Checked               = Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "SpoolEddnToFile", false.ToString(), false, true);
            cbSpoolImplausibleToFile.Checked        = Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "SpoolImplausibleToFile", false.ToString(), false, true);
            cbEDDNAutoListen.Checked                = Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "EDDNAutoListen", false.ToString(), false, true);
            checkboxImportEDDN.Checked              = Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "EDDNAutoImport", false.ToString(), false, true);
        }

       
      

        public void ActivateOCRTab()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ActivateOCRTab));
                return;
            }

            if (Program.DBCon.getIniValue<Boolean>(MTSettings.tabSettings.DB_GROUPNAME, "AutoActivateOCRTab", true.ToString(), false, true) && !Program.DBCon.getIniValue<Boolean>(MTSettings.tabSettings.DB_GROUPNAME, "CheckNextScreenshotForOne", false.ToString(), false, true))
                try
                {
                    tabCtrlMain.SelectedTab = tabCtrlMain.TabPages["tabOCRGroup"];
                    tabCtrlOCR.SelectedTab  = tabCtrlOCR.TabPages["tabOCR"];
                }
                catch (Exception)
                {
                }
        }

     

        void Application_ApplicationExit(object sender, EventArgs e)
        {

            if (_eddnSubscriberThread != null) _eddnSubscriberThread.Abort();

            if (sws.Running)
                sws.Stop();
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

                throw new NotImplementedException();
                //ImportListOfCsvs(filenames);
            }

            SetupGui();

        }

        public void SetupGui(bool force= false){

            //if(this.InvokeRequired)
            //    this.Invoke(new delParamBool(iSetupGui), force);
            //else
            //    iSetupGui(force);
        }

        //private void iSetupGui(bool force= false)
        //{
            //System.Windows.Forms.Cursor oldCursor = Cursor;
            //Cursor = Cursors.WaitCursor;

            ////_cbIncludeWithinRegionOfStation_IndexChanged = false;

            //if (!_InitDone && !force)
            //    return;

            //_pt.startMeasuring();

            //cmbStation.BeginUpdate();
            //cmbStationToStationFrom.BeginUpdate();
            //cmbStationToStationTo.BeginUpdate();
            //cbCommodity.BeginUpdate();

            //_pt.PrintAndReset("1");

            //// notice the current selected items
            //string Key_cmbStation               = getCmbItemKey(cmbStation.SelectedItem);
            //string Key_cmbStationToStationFrom  = getCmbItemKey(cmbStationToStationFrom.SelectedItem);
            //string Key_cmbStationToStationTo    = getCmbItemKey(cmbStationToStationTo.SelectedItem);

            //_pt.PrintAndReset("2");

            //BindingList<System.Collections.Generic.KeyValuePair<string,string>> BaseList;
            //IFormatter formatter        = new BinaryFormatter();
            //MemoryStream SerialListCopy = new MemoryStream();

            //_pt.PrintAndReset("3");

            //BaseList = getDropDownStationsItems(ref _StationIndices);

            //formatter.Serialize(SerialListCopy, BaseList);

            //_bs_Stations.DataSource = BaseList;
            //SerialListCopy.Seek(0,0);
            //_bs_StationsFrom.DataSource = (BindingList<System.Collections.Generic.KeyValuePair<string,string>>)formatter.Deserialize(SerialListCopy);
            //SerialListCopy.Seek(0,0);
            //_bs_StationsTo.DataSource = (BindingList<System.Collections.Generic.KeyValuePair<string,string>>)formatter.Deserialize(SerialListCopy);

            //_pt.PrintAndReset("4");

            //SerialListCopy.Dispose();

            //if (!_InitDone)
            //{ 
            //    cmbStation.DataSource = _bs_Stations;
            //    cmbStation.DisplayMember = "Value";
            //    cmbStation.ValueMember = "Key";

            //    cmbStationToStationFrom.DataSource = _bs_StationsFrom;
            //    cmbStationToStationFrom.DisplayMember = "Value";
            //    cmbStationToStationFrom.ValueMember = "Key";

            //    cmbStationToStationTo.DataSource = _bs_StationsTo;
            //    cmbStationToStationTo.DisplayMember = "Value";
            //    cmbStationToStationTo.ValueMember = "Key";
            //}


            //_pt.PrintAndReset("5");
            //cbIncludeWithinRegionOfStation.SelectedIndexChanged -= cbIncludeWithinRegionOfStation_SelectionChangeCommitted;

            //var previouslySelectedValue = cbIncludeWithinRegionOfStation.SelectedItem;
            //cbIncludeWithinRegionOfStation.Items.Clear();
            //var systems = StationDirectory.Keys.Select(x => (object)(StructureHelper.CombinedNameToSystemName(x))).OrderBy(x => x).Distinct().ToArray();
            //cbIncludeWithinRegionOfStation.Items.Add("<Current System>");
            //cbIncludeWithinRegionOfStation.Items.AddRange(systems);

            ////cbIncludeWithinRegionOfStation.SelectedIndex = 0;
            //cbIncludeWithinRegionOfStation.DropDownStyle = ComboBoxStyle.DropDownList;

            //_pt.PrintAndReset("6");

            //if (previouslySelectedValue != null)
            //    cbIncludeWithinRegionOfStation.SelectedItem = previouslySelectedValue;
            //else
            //    cbIncludeWithinRegionOfStation.SelectedItem = "<Current System>";

            //cbIncludeWithinRegionOfStation.SelectedIndexChanged += cbIncludeWithinRegionOfStation_SelectionChangeCommitted;

            //int ListIndex;

            
            //_pt.PrintAndReset("7");

            //if ((Key_cmbStation != null) && _StationIndices.TryGetValue(Key_cmbStation, out ListIndex))
            //    cmbStation.SelectedIndex = ListIndex;

            //if ((Key_cmbStation != null) && _StationIndices.TryGetValue(Key_cmbStationToStationFrom, out ListIndex))
            //    cmbStationToStationFrom.SelectedIndex = ListIndex;

            //if ((Key_cmbStation != null) && _StationIndices.TryGetValue(Key_cmbStationToStationTo, out ListIndex))
            //    cmbStationToStationTo.SelectedIndex = ListIndex;

            
            //cbCommodity.Items.Clear();

            //_pt.PrintAndReset("8");

            //foreach (var commodity in CommodityDirectory.OrderBy(x => x.Key))
            //{
            //    cbCommodity.Items.Add(commodity.Key);
            //}

            //cbCommodity.SelectedItem = null;

            //if (cbCommodity.Items.Count > 0)
            //    cbCommodity.SelectedItem = cbCommodity.Items[0];

            //lvAllComms.Items.Clear();

            ////_pt.PrintAndReset("9");

            //Debug.Print("Anzahl = " + CommodityDirectory.Count.ToString());
            //// Populate all commodities tab
            //foreach (var commodity in CommodityDirectory)
            //{
            //    decimal bestBuyPrice;
            //    decimal bestSellPrice;
            //    string bestBuy;
            //    string bestSell;
            //    decimal buyers;
            //    decimal sellers;

            //    //_pt.PrintAndReset("9_1");

            //    GetBestBuyAndSell(commodity.Key, out bestBuyPrice, out bestSellPrice, out bestBuy, out bestSell, out buyers, out sellers);

            //    //_pt.PrintAndReset("9_2");

            //    lvAllComms.Items.Add(new ListViewItem(new[] 
            //    {   commodity.Key,
            //        bestBuyPrice.ToString(CultureInfo.InvariantCulture) != "0" ? bestBuyPrice.ToString(CultureInfo.InvariantCulture) : "",
            //        bestBuy,
            //        buyers.ToString(CultureInfo.InvariantCulture),
            //        bestSellPrice.ToString(CultureInfo.InvariantCulture) != "0" ? bestSellPrice.ToString(CultureInfo.InvariantCulture) : "",
            //        bestSell,
            //        sellers.ToString(CultureInfo.InvariantCulture),
            //        bestBuyPrice != 0 && bestSellPrice != 0 ? (bestSellPrice - bestBuyPrice).ToString(CultureInfo.InvariantCulture) : ""
            //    }));

            //    //_pt.PrintAndReset("9_3");
            //}

            ////_pt.PrintAndReset("10");

            //cmbStation.EndUpdate();
            //cmbStationToStationFrom.EndUpdate();
            //cmbStationToStationTo.EndUpdate();
            //cbCommodity.EndUpdate();
            ////_pt.PrintAndReset("11");

            //UpdateStationToStation();
            ////_pt.PrintAndReset("12");

            //Cursor = oldCursor;
        //}


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


        private void lvAllComms_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (((ListView)sender).SelectedItems.Count != 1)
            //    return;

            //if (_tooltip != null) _tooltip.RemoveAll();
            //if (_tooltip2 != null) _tooltip2.RemoveAll();

            //var senderName = ((ListView)sender).SelectedItems[0].Text;

            //chart1.ResetAutoValues();

            //chart1.Series.Clear();
            //var series1 = new Series
            //{
            //    Name = "Series1",
            //    Color = Color.Green,
            //    IsVisibleInLegend = false,
            //    IsXValueIndexed = true,
            //    ChartType = SeriesChartType.Column

            //};

            //chart1.Series.Add(series1);

            //foreach (var price in CommodityDirectory[senderName].Where(x => x.BuyPrice != 0 && x.Supply != 0).Where(x => getStationSelection(x)).OrderBy(x => x.BuyPrice))
            //{
            //    series1.Points.AddXY(price.StationID, price.BuyPrice);
            //}

            //chart1.Invalidate();

            //chart2.ResetAutoValues();

            //chart2.Series.Clear();
            //var series2 = new Series
            //{
            //    Name = "Series1",
            //    Color = Color.Green,
            //    IsVisibleInLegend = false,
            //    IsXValueIndexed = true,
            //    ChartType = SeriesChartType.Column

            //};

            //chart2.Series.Add(series2);

            //foreach (var price in CommodityDirectory[senderName].Where(x => x.SellPrice != 0 && x.Demand != 0).Where(x => getStationSelection(x)).OrderByDescending(x => x.SellPrice))
            //{
            //    series2.Points.AddXY(price.StationID, price.SellPrice);
            //}

            //chart2.Invalidate();
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

        /// <summary>
        /// new tab on main TabControl selected 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (_tooltip != null) _tooltip.RemoveAll();
                if (_tooltip2 != null) _tooltip2.RemoveAll();

                RefreshCurrentTab();

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error when selecting a new tab on main tabcontrol");
            }
        }

        /// <summary>
        /// initiates a refresh of the current tab if necessary
        /// </summary>
        private void RefreshCurrentTab()
        {
            try
            {
                if(!m_IsRefreshed[tabCtrlMain.SelectedTab.Name])
                {
                    switch (tabCtrlMain.SelectedTab.Name)
                    {
                        case "tabPriceAnalysis":
                            Program.PriceAnalysis.GUI.RefreshData();
                            break;
                        case "tabCommandersLog":
                            Program.PriceAnalysis.GUI.RefreshData();
                            break;
                    }
                    MethodInfo RefreshMethod = tabCtrlMain.SelectedTab.GetType().GetMethod("RefreshData");

                    if(RefreshMethod != null)
                        RefreshMethod.Invoke(tabCtrlMain.SelectedTab, null);

                    m_IsRefreshed[tabCtrlMain.SelectedTab.Name] = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while refreshing the current tab", ex);
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
                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverIpAddress", cbInterfaces.SelectedItem.ToString());
            }

            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverPort", txtWebserverPort.Text);

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



        public bool checkPricePlausibility(string[] DataRows, bool simpleEDDNCheck = false)
        {
            bool implausible = false;
            SQL.Datasets.dsEliteDB.tbcommodityRow[] CommodityData;

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

                    CommodityData = (SQL.Datasets.dsEliteDB.tbcommodityRow[])
                                    Program.Data.BaseData.tbcommodity.Select("commodity    = " + DBConnector.SQLAString(currentRow.CommodityName) + 
                                                                             " or " +
                                                                             "loccommodity = " + DBConnector.SQLAString(currentRow.CommodityName));
                    

                    if (currentRow.CommodityName == "Panik")
                        Debug.Print("STOP");
                            
                    if ((CommodityData != null) && (CommodityData.GetUpperBound(0) >= 0))
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

                            if (((CommodityData[0].pwl_supply_sell_low  >= 0) && (currentRow.SellPrice < CommodityData[0].pwl_supply_sell_low)) ||
                                ((CommodityData[0].pwl_supply_sell_high >= 0) && (currentRow.SellPrice > CommodityData[0].pwl_supply_sell_high)))
                            {
                                // sell price is out of range
                                implausible = true;
                            }

                            if (((CommodityData[0].pwl_supply_buy_low  >= 0) && (currentRow.BuyPrice  < CommodityData[0].pwl_supply_buy_low)) ||
                                ((CommodityData[0].pwl_supply_buy_high >= 0) && (currentRow.SellPrice > CommodityData[0].pwl_supply_buy_high)))
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

                            if (((CommodityData[0].pwl_demand_sell_low  >= 0) && (currentRow.SellPrice < CommodityData[0].pwl_demand_sell_low)) ||
                                ((CommodityData[0].pwl_demand_sell_high >= 0) && (currentRow.SellPrice > CommodityData[0].pwl_demand_sell_high)))
                            {
                                // buy price is out of range
                                implausible = true;
                            }

                            if (currentRow.BuyPrice >= 0) 
                                if (((CommodityData[0].pwl_demand_buy_low  >= 0) && (currentRow.BuyPrice < CommodityData[0].pwl_demand_buy_low)) ||
                                    ((CommodityData[0].pwl_demand_buy_high >= 0) && (currentRow.BuyPrice > CommodityData[0].pwl_demand_buy_high)))
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
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverForegroundColor", tbForegroundColour.Text);
        }

        private void tbBackgroundColour_TextChanged(object sender, EventArgs e)
        {
            sws.BackgroundColour = tbBackgroundColour.Text;
            cbColourScheme.SelectedItem = null;
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverBackgroundColor", tbBackgroundColour.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            startEDDNListening();
        }

        private void startEDDNListening()
        {
            _eddnSubscriberThread = new Thread(() => EDDNComm.Subscribe());
            _eddnSubscriberThread.IsBackground = true;
            _eddnSubscriberThread.Start();

            EDDNComm.DataRecieved += RecievedEDDNData;

        }

        private void RecievedEDDNData(object sender, EDDN.RecievedEDDNArgs e)
        {
            String[] DataRows           = new String[0];
            String   nameAndVersion     = String.Empty;
            String   name               = String.Empty;
            String   uploaderID         = String.Empty; 
            Boolean  SimpleEDDNCheck    = false;

            try{
                setText(tbEDDNOutput, String.Format("{0}\n{1}", e.Message, e.RawData));

                if (cbSpoolEddnToFile.Checked){
                    if (_eddnSpooler == null){
                        if (!File.Exists(Program.GetDataPath("EddnOutput.txt")))
                            _eddnSpooler = File.CreateText(Program.GetDataPath("EddnOutput.txt"));
                        else
                            _eddnSpooler = File.AppendText(Program.GetDataPath("EddnOutput.txt"));
                    }
                    _eddnSpooler.WriteLine(e.RawData);
                }

                switch (e.InfoType){

                	case EDDN.RecievedEDDNArgs.enMessageInfo.Commodity_v1_Recieved:
                        
                        // process only if it's the correct schema
                        if(!(Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "UseEddnTestSchema", false.ToString(), false, true) ^ ((EDDN.Schema_v1)e.Data).isTest()))
                        {
                            Debug.Print("handle v1 message");
                            EDDN.Schema_v1 DataObject   = (EDDN.Schema_v1)e.Data;

                            // Don't import our own uploads...
                            if(DataObject.Header.UploaderID != Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "UserID")) 
                            { 
                                DataRows                    = new String[1] {DataObject.getEDDNCSVImportString()};
                                nameAndVersion              = String.Format("{0} / {1}", DataObject.Header.SoftwareName, DataObject.Header.SoftwareVersion);
                                name                        = String.Format("{0}", DataObject.Header.SoftwareName);
                                uploaderID                  = DataObject.Header.UploaderID;
                                SimpleEDDNCheck             = true;
                            }
                            else
                                Debug.Print("handle v1 rejected (it's our own message)");
                            
                        }else 
                            Debug.Print("handle v1 rejected (wrong schema)");

                		break;

                	case EDDN.RecievedEDDNArgs.enMessageInfo.Commodity_v2_Recieved:

                        // process only if it's the correct schema
                        if(!(Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "UseEddnTestSchema", false.ToString(), false, true) ^ ((EDDN.Schema_v2)e.Data).isTest()))
                        {
                            Debug.Print("handle v2 message");

                                
                            EDDN.Schema_v2 DataObject   = (EDDN.Schema_v2)e.Data;

                            // Don't import our own uploads...
                            if(DataObject.Header.UploaderID != Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "UserID")) 
                            { 
                                DataRows                    = DataObject.getEDDNCSVImportStrings();
                                nameAndVersion              = String.Format("{0} / {1}", DataObject.Header.SoftwareName, DataObject.Header.SoftwareVersion);
                                name                        = String.Format("{0}", DataObject.Header.SoftwareName);
                                uploaderID                  = DataObject.Header.UploaderID;
                            }
                            else
                                Debug.Print("handle v2 rejected (it's our own message)");

                        }else  
                            Debug.Print("handle v2 rejected (wrong schema)");

                		break;

                	case EDDN.RecievedEDDNArgs.enMessageInfo.UnknownData:
                        setText(tbEDDNOutput, "Recieved a unknown EDDN message:" + Environment.NewLine + e.Message + Environment.NewLine + e.RawData);
                		Debug.Print("handle unkown message");
                		break;

                	case EDDN.RecievedEDDNArgs.enMessageInfo.ParseError:
                		Debug.Print("handle error message");
                        setText(tbEDDNOutput, "Error while processing recieved EDDN data:" + Environment.NewLine + e.Message + Environment.NewLine + e.RawData);

                		break;

                } 

                if(DataRows != null && DataRows.GetUpperBound(0) >= 0)
                {
                    updatePublisherStats(nameAndVersion, DataRows.GetUpperBound(0)+1);

                    List<String> trustedSenders = Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "trustedSenders", "").Split(new char[] {'|'}).ToList();

                    bool isTrusty = trustedSenders.Exists(x => x.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                    foreach (String DataRow in DataRows){

                        // data is plausible ?
                        if(isTrusty || (!checkPricePlausibility(new string[] {DataRow}, SimpleEDDNCheck))){

                            // import is wanted ?
                            if(checkboxImportEDDN.Checked)
                            {
                                Debug.Print("import :" + DataRow);
                                throw new NotImplementedException();
                                //ImportCsvString(DataRow);
                            }

                        }else{
                            Debug.Print("implausible :" + DataRow);
                            // data is implausible
                            string InfoString = string.Format("IMPLAUSIBLE DATA : \"{2}\" from {0}/ID=[{1}]", nameAndVersion, uploaderID, DataRow);

                            addTextLine(lbEddnImplausible, InfoString);

                            if(cbSpoolImplausibleToFile.Checked){

                                FileStream LogFileStream = null;
                                string FileName = Program.GetDataPath("EddnImplausibleOutput.txt");

                                if(File.Exists(FileName))
                                    LogFileStream = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                                else
                                    LogFileStream = File.Create(FileName);

                                LogFileStream.Write(System.Text.Encoding.Default.GetBytes(InfoString + "\n"), 0, System.Text.Encoding.Default.GetByteCount(InfoString + "\n"));
                                LogFileStream.Close();
                            }
                        }
                    }  
 

                    if(!_AutoImportDelayTimer.Enabled)
                        _AutoImportDelayTimer.Start();

                }
            }catch (Exception ex){
                setText(tbEDDNOutput, "Error while processing recieved EDDN data:" + Environment.NewLine + ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
            }      
        }

        private void AutoImportDelayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e){
            SetupGui();   
        }

        /// <summary>
        /// refreshes the info about software versions and recieved messages
        /// </summary>
        /// <param name="SoftwareID"></param>
        /// <param name="MessageCount"></param>
        private void updatePublisherStats(string SoftwareID,int MessageCount)
        {
            if (!_eddnPublisherStats.ContainsKey(SoftwareID))
                _eddnPublisherStats.Add(SoftwareID, new EddnPublisherVersionStats());

            _eddnPublisherStats[SoftwareID].MessagesReceived += MessageCount;

            

            System.Text.StringBuilder output = new System.Text.StringBuilder();
            foreach (var appVersion in _eddnPublisherStats.OrderByDescending(x => x.Value.MessagesReceived))
            {
                output.Append(appVersion.Key + " : " + appVersion.Value.MessagesReceived + " messages\r\n");
            }

            setText(tbEddnStats, output.ToString());
        }

        #region EDDNCommunicator Delegates
        private DateTime _lastGuiUpdate;

        private delegate void SetTextCallback(object text);

        private StreamWriter _eddnSpooler = null;

        //public void OutputEddnRawData(object text)
        //{
        //    if (InvokeRequired)
        //    {
        //        SetTextCallback d = OutputEddnRawData;
        //        BeginInvoke(d, new { text });
        //    }
        //    else
        //    {
        //        tbEDDNOutput.Text = text.ToString();

        //        if (cbSpoolEddnToFile.Checked)
        //        {
        //            if (_eddnSpooler == null)
        //            {
        //                if (!File.Exists(".//EddnOutput.txt"))
        //                    _eddnSpooler = File.CreateText(".//EddnOutput.txt");
        //                else
        //                    _eddnSpooler = File.AppendText(".//EddnOutput.txt");
        //            }

        //            _eddnSpooler.WriteLine(text);
        //        }

        //        var headerDictionary    = new Dictionary<string, string>();
        //        var messageDictionary   = new Dictionary<string, string>();

        //        ParseEddnJson(text, headerDictionary, messageDictionary, checkboxImportEDDN.Checked);

        //    }
        //}

        private Dictionary<string, EddnPublisherVersionStats> _eddnPublisherStats = new Dictionary<string, EddnPublisherVersionStats>();
        private EDSystem  cachedSystem;
        private EDStation cachedStation;

        private void ParseEddnJson(object text, Dictionary<string, string> headerDictionary, IDictionary<string, string> messageDictionary, bool import)
        {
            string txt = text.ToString();
            // .. we're here because we've received some data from EDDNCommunicator

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


                    if((Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "UseEddnTestSchema", false.ToString(), false, true)  && (schemaRawData.IndexOf("Test", StringComparison.InvariantCultureIgnoreCase) >= 0)) ||
                       (!Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "UseEddnTestSchema", false.ToString(), false, true) && (schemaRawData.IndexOf("Test", StringComparison.InvariantCultureIgnoreCase)  < 0)))
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

                        if (Debugger.IsAttached && Program.showToDo)
                            MessageBox.Show("TODO");
                        string commodity = ""; //' getLocalizedCommodity(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "Language, messageDictionary["itemName"]);

                        if((cachedSystem == null) || (!messageDictionary["systemName"].Equals(cachedSystem.Name, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            throw new NotImplementedException();
                            //cachedSystem = _Milkyway.getSystem(messageDictionary["Systemname"]);
                        }
                        if(cachedSystem == null)
                        {
                            cachedSystem = new EDSystem();
                            cachedSystem.Name = messageDictionary["systemName"];
                        }

                        if((cachedSystem != null) && ((cachedStation == null) || (!messageDictionary["stationName"].Equals(cachedStation.Name, StringComparison.InvariantCultureIgnoreCase))))
                        {
                            throw new NotImplementedException();
                            //cachedStation = _Milkyway.getLocation(messageDictionary["Systemname"], messageDictionary["Locationname"]);
                        }
                        if(cachedStation == null)
                        {
                            cachedStation = new EDStation();
                            cachedStation.Name = messageDictionary["stationName"];
                        }

                        if(!String.IsNullOrEmpty(commodity))
                        {

                            //System;Location;Commodity_Class;Sell;Buy;Demand;;Supply;;Date;
                            if (headerDictionary["uploaderID"] != Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "UserName")) // Don't import our own uploads...
                            {
                                string csvFormatted = cachedSystem.Name + ";" +
                                                      cachedStation.Name + ";" +
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
                                    if(import)
                                        throw new NotImplementedException();
                                        //ImportCsvString(csvFormatted);

                                }else{

                                    string InfoString = string.Format("IMPLAUSIBLE DATA : \"{3}\" from {0}/{1}/ID=[{2}]", headerDictionary["softwareName"], headerDictionary["softwareVersion"], headerDictionary["uploaderID"], csvFormatted );

                                    lbEddnImplausible.Items.Add(InfoString);
                                    lbEddnImplausible.SelectedIndex = lbEddnImplausible.Items.Count-1;
                                    lbEddnImplausible.SelectedIndex = -1;

                                    if(cbSpoolImplausibleToFile.Checked)
                                    {
                                        FileStream LogFileStream = null;
                                        string FileName = Program.GetDataPath("EddnImplausibleOutput.txt");

                                        if(File.Exists(FileName))
                                        { 
                                            LogFileStream = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                                        }
                                        else
                                        {
                                            LogFileStream = File.Create(FileName);
                                        }

                                       LogFileStream.Write(System.Text.Encoding.Default.GetBytes(InfoString + "\n"), 0, System.Text.Encoding.Default.GetByteCount(InfoString + "\n"));
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
                            string InfoString = string.Format("UNKNOWN COMMODITY : \"{3}\" from {0}/{1}/ID=[{2}]", headerDictionary["softwareName"], headerDictionary["softwareVersion"], headerDictionary["uploaderID"], csvFormatted );

                            lbEddnImplausible.Items.Add(InfoString);
                            lbEddnImplausible.SelectedIndex = lbEddnImplausible.Items.Count-1;
                            lbEddnImplausible.SelectedIndex = -1;

                            if(cbSpoolImplausibleToFile.Checked)
                            {

                                FileStream LogFileStream = null;
                                string FileName = Program.GetDataPath("EddnImplausibleOutput.txt");

                                if(File.Exists(FileName))
                                { 
                                    LogFileStream = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                                }
                                else
                                {
                                    LogFileStream = File.Create(FileName);
                                }

                                LogFileStream.Write(System.Text.Encoding.Default.GetBytes(InfoString + "\n" ), 0, System.Text.Encoding.Default.GetByteCount(InfoString + "\n"));
                                LogFileStream.Close();
                            }

                        }
                    }
                }
                catch
                {
                    tbEDDNOutput.Text = "Couldn't parse JSON!\r\n\r\n" + tbEDDNOutput.Text;
                }
        }

        private delegate void del_setText(Control Destination, String newText);

        public void setText(Control Destination, String newText)
        {
            if(Destination.InvokeRequired)
                Destination.Invoke(new tabOCR.del_setControlText(setText), Destination, newText);
            else
                Destination.Text = newText;
        }

        private delegate void del_addText(ListBox Destination, String newLine);

        public void addTextLine(ListBox Destination, String newLine)
        {
            if(Destination.InvokeRequired)
                Destination.Invoke(new del_addText(addTextLine), Destination, newLine);
            else
            {
                Destination.Items.Add(newLine);
                Destination.SelectedIndex = lbEddnImplausible.Items.Count-1;
                Destination.SelectedIndex = -1;
            }
        }

        #endregion

        private void cmdStopEDDNListening_Click(object sender, EventArgs e)
        {
            if (_eddnSubscriberThread != null && _eddnSubscriberThread.IsAlive)
                _eddnSubscriberThread.Abort();
        }
   

        private void button17_Click(object sender, EventArgs e)
        {
            //SetupGui();
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
                
                throw new NotImplementedException();
                //ImportListOfCsvs(_filesFound.ToArray());

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
                Program.Logger.Log("Error recursing directories:", true);
                Program.Logger.Log(ex.ToString(), true);
                Program.Logger.Log(ex.Message, true);
                Program.Logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    Program.Logger.Log(ex.InnerException.ToString(), true);
                Console.WriteLine(ex.Message);
            }
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

        // Recurse controls on form
        public IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl))
                                      .Concat(controls);
        }

        System.Windows.Forms.Timer _timer;

        private void Form_Shown(object sender, System.EventArgs e)
        {
            try
            {
                Version newVersion;
                String newInfo;

                Program.SplashScreen.InfoAdd("checking for updates");
                Updater updater = new Updater();

                if (updater.CheckVersion(out newVersion, out newInfo))
                {
                    lblUpdateInfo.Text = "newer version of ED-IBE found!";
                    lblUpdateInfo.ForeColor = Color.Black;
                    lblUpdateInfo.BackColor = Color.Yellow;

                    lblUpdateDetail.Text = String.Format("ED-IBE v{0} :\r\n{1}", newVersion.ToString(), newInfo);
                }
                else 
                {
                    lblUpdateInfo.Text = "you have the latest version of ED-IBE";
                    lblUpdateInfo.ForeColor = Color.DarkGreen;

                    if (newVersion != new Version(0,0,0,0))
                        lblUpdateDetail.Text = String.Format("ED-IBE v{0} :\r\n{1}", newVersion.ToString(), newInfo);
                }
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("load system data...");
                loadSystemData(Program.actualCondition.System);
                loadStationData(Program.actualCondition.System, Program.actualCondition.Location);
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("init settings gui...");
                Program.Settings.GUI.Init();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("init price analysis gui");
                Program.PriceAnalysis.GUI.Init();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("init commanders log gui");
                Program.CommandersLog.GUI.Init();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("init system numbers");
                showSystemNumbers();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("init main gui");
                SetupGui();

                this.tbCurrentSystemFromLogs.Text       = Program.actualCondition.System;
                this.tbCurrentStationinfoFromLogs.Text  = Program.actualCondition.Location;
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("starting logfile scanner");
                Program.LogfileScanner.Start();
                Program.SplashScreen.InfoAppendLast("<OK>");

                
                Program.DoSpecial();

                if(cbEDDNAutoListen.Checked)
                {
                    Program.SplashScreen.InfoAdd("starting eddn listening");
                    startEDDNListening();
                    Program.SplashScreen.InfoAppendLast("<OK>");
                }


                Program.SplashScreen.InfoAdd("init sequence finished");
                Program.SplashScreen.CloseDelayed();

                
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error in Form_Shown");
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {

            Text += VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3);
            testToolStripMenuItem.Visible = Debugger.IsAttached;

            doInit();

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

        private void Retheme()
        {
            bool noBackColor = false;

            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour", "") == "" || 
                Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour", "") == "") return;

            var x = GetAll(this);

            int redF = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour").Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int greenF = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour").Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int blueF = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour").Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            var f = Color.FromArgb(redF, greenF, blueF);
            int redB = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour").Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int greenB = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour").Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int blueB = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour").Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
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

        private int getCalculations(int Total)
        {
            Int32 retValue = 0;

            for (int i = 0; i <Total; i++)
            {
                retValue += i;
            }
            return retValue;

        }

        private void cmdPurgeEDDNData(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

      

        

        public void setOCRTabsVisibility()
        {
            try
            {
                try
                {
                    if (this.InvokeRequired)
                        this.Invoke(new MethodInvoker(setOCRTabsVisibility));
                    else
                    {
                        if (GameSettings != null && tabCtrlOCR.TabPages["OCR_Calibration"] != null)
                        {
                            var OCRTabPage = tabCtrlOCR.TabPages["OCR_Calibration"];
                            OCRTabPage.Enabled = (GameSettings.Display != null);
                            var TabControl = (OcrCalibratorTab)(OCRTabPage.Controls[0]);
                
                            TabControl.lblWarning.Visible = (GameSettings.Display == null); 
                        }

                        if (GameSettings != null && tabCtrlOCR.TabPages["OCR_CaptureAndCorrect"] != null)
                        {
                            var OCRTabPage = tabCtrlOCR.TabPages["OCR_CaptureAndCorrect"];
                            OCRTabPage.Enabled = (GameSettings.Display != null);
                            var TabControl = (tabOCR)(OCRTabPage.Controls[0]);
                            TabControl.startOcrOnload(Program.DBCon.getIniValue<Boolean>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "StartOCROnLoad", false.ToString(), false, true) && Directory.Exists(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "MostRecentOCRFolder", "")));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in setOCRTabsVisibility (inline)", ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in setOCRTabsVisibility (outline)", ex);
            }
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
        //            Program.actualCondition.Location = m_lastestStationInfo;
        //            _LoggedLocation = m_lastestStationInfo;
    
        //            if(Program.DBCon.getIniValue<Boolean>(Settings_old.tabSettings.DB_GROUPNAME, "AutoActivateSystemTab", true.ToString(), false, true)
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
        //        Program.actualCondition.System = SystemInfo;
        //    }
        //}

      

#region System / Location Tab

        /*/////////////////////////////////////////////////////////////////////////////////////////
        *******************************************************************************************    
         *
         *                             System / Location Tab
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

            //throw new NotImplementedException();
            //lblSystemCountTotal.Text    = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Count.ToString();
            //lblStationCountTotal.Text   = _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Count.ToString();
        }


        private void cmdLoadCurrentSystem_Click(object sender, EventArgs e)
        {

            loadSystemData(Program.actualCondition.System);
            loadStationData(Program.actualCondition.System, Program.actualCondition.Location);

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

                if (Debugger.IsAttached && Program.showToDo)
                    MessageBox.Show("TODO");
                // throw new NotImplementedException();
                //m_loadedSystemdata = _Milkyway.getSystem(Systemname);
                m_loadedSystemdata = null;
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
                txtSystemUpdatedAt.Text = DateTimeOffset.FromUnixTimeSeconds(m_loadedSystemdata.UpdatedAt).DateTime.ToString(CultureInfo.CurrentUICulture);
                cbSystemNeedsPermit.CheckState = m_loadedSystemdata.NeedsPermit.toCheckState();
                cmbSystemPrimaryEconomy.Text = m_loadedSystemdata.PrimaryEconomy.NToString();
                cmbSystemSecurity.Text = m_loadedSystemdata.Security.NToString();
                cmbSystemState.Text = m_loadedSystemdata.State.NToString();
                cmbSystemAllegiance.Text = m_loadedSystemdata.Allegiance.NToString();
                cmbSystemGovernment.Text = m_loadedSystemdata.Government.NToString();

                setSystemEditable(isNew);

                if(!isNew)
                { 
                    cmdSystemNew.Enabled            = true;
                    cmdSystemEdit.Enabled           = true;
                    cmdSystemSave.Enabled           = false;
                    cmdSystemCancel.Enabled         = cmdSystemSave.Enabled;

                    cmdStationNew.Enabled           = true;
                    cmdStationEdit.Enabled          = false;
                    cmdStationSave.Enabled          = false;
                    cmdStationCancel.Enabled        = cmdStationSave.Enabled;

				    cmbSystemsAllSystems.ReadOnly   = false;
                    cmbStationStations.ReadOnly     = false;
                }

                throw new NotImplementedException();
                List<EDStation> StationsInSystem = null; // _Milkyway.getStations(Systemname);
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

                if (Debugger.IsAttached && Program.showToDo)
                    MessageBox.Show("TODO");
                //m_loadedStationdata                 = _Milkyway.getLocation(Systemname, Locationname);
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

                txtStationUpdatedAt.Text = DateTimeOffset.FromUnixTimeSeconds(m_loadedStationdata.UpdatedAt).DateTime.ToString(CultureInfo.CurrentUICulture);

                lbStationEconomies.Items.Clear();

                foreach (string Economy in m_loadedStationdata.Economies)
                    lbStationEconomies.Items.Add(Economy);

                cbStationHasMarket.CheckState = m_loadedStationdata.HasMarket.toCheckState();
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

//                if (_Milkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Exists(x => (x.Name.Equals(m_loadedStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) &&
//                                                                                      (x.SystemId == m_loadedStationdata.SystemId)))
                if (Debugger.IsAttached && Program.showToDo)
                    MessageBox.Show("TODO");

                if(false)
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

                cbStationHasMarket.CheckState = CheckState.Unchecked;
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
            cmbSystemState.Items.Add("Boom");
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


            // ********************** Location *******************

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


            this.cbStationHasMarket.CheckedChanged += new System.EventHandler(this.CheckBox_StationSystem_CheckedChanged);
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

            throw new NotImplementedException();
            this.cmbSystemsAllSystems.DataSource      = null; //_Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).OrderBy(x => x.Name).ToList();
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
                        throw new NotImplementedException();
                        EDSystem existing = null; //_Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => x.Name.Equals(txtSystemName.Text, StringComparison.InvariantCultureIgnoreCase));
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
                        throw new NotImplementedException();
                        EDStation existing = null; //_Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(txtStationName.Text, StringComparison.InvariantCultureIgnoreCase)) &&
                                                   //                                                             (x.SystemId == m_currentStationdata.SystemId));
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
                    m_currentStationdata.HasMarket = cbStationHasMarket.toNInt();
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
                throw new NotImplementedException();
                existing = null; //_Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => x.Name.Equals(m_currentSystemdata.Name, StringComparison.InvariantCultureIgnoreCase));
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
                throw new NotImplementedException();
                existing = null; //_Milkyway.getSystems(EDMilkyway.enDataType.Data_EDDB).Find(x => x.Name.Equals(_oldSystemName, StringComparison.InvariantCultureIgnoreCase));
                if (existing != null)
                {
                    MsgBox.Show("It's not allowed to rename a EDDB system", "renaming system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                throw new NotImplementedException();
                existing = null; //_Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => x.Name.Equals(m_currentSystemdata.Name, StringComparison.InvariantCultureIgnoreCase));
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

                throw new NotImplementedException();
                //_Milkyway.ChangeAddSystem(m_currentSystemdata, _oldSystemName);
                setSystemEditable(false);

                if(newComboBoxRefresh)
                { 
                    m_SystemLoadingValues = true;
                    cmbSystemsAllSystems.BeginUpdate();

                    cmbSystemsAllSystems.DataSource      = null;
                    throw new NotImplementedException();
                    //cmbSystemsAllSystems.DataSource      = _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).OrderBy(x => x.Name).ToList();
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
                // adding a new Location
                throw new NotImplementedException();
                existing = null; // _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(m_currentStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) && 
                                 //                                                              (x.SystemId.Equals(m_currentSystemdata.Id)));
                if (existing != null)
                {
                    MsgBox.Show("A station with this name already exists", "Adding a new station", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if(!_oldStationName.Equals(m_currentStationdata.Name))
            {
                // changing Location name
                throw new NotImplementedException();
                existing = null; //_Milkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Find(x => (x.Name.Equals(_oldStationName, StringComparison.InvariantCultureIgnoreCase)) && 
                                 //                                                           (x.SystemId.Equals(m_currentSystemdata.Id)));
                if (existing != null)
                {
                    MsgBox.Show("It's not allowed to rename a EDDB station", "renaming station", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                throw new NotImplementedException();
                existing = null; // _Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(m_currentStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) && 
                                 //                                                              (x.SystemId.Equals(m_currentSystemdata.Id)));
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
                throw new NotImplementedException();
                //_Milkyway.ChangeAddStation(m_currentSystemdata.Name, m_currentStationdata, _oldStationName);

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

            string newSystemname = Program.actualCondition.System;

            if(InpBox.Show("create a new system", "insert the name of the new system", ref newSystemname) == System.Windows.Forms.DialogResult.OK)
            { 
                
                throw new NotImplementedException();
                EDSystem existing = null; // _Milkyway.getSystems(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(newSystemname, StringComparison.InvariantCultureIgnoreCase)));

                if (existing != null)
                {
                    MsgBox.Show("A system with this name already exists", "Adding a new system", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {

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

                    setSystemEditable(true);

                }
            }
        }

        private void cmdStationNew_Click(object sender, EventArgs e)
        {
            _oldSystemName  = m_currentSystemdata.Name;
            _oldStationName = m_currentStationdata.Name;

            string newStationname = Program.actualCondition.Location;

            throw new NotImplementedException();
            EDStation existing = null; //_Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(newStationname, StringComparison.InvariantCultureIgnoreCase)) && 
                                   //                                                                 (x.SystemId.Equals(m_currentSystemdata.Id)));
            if (existing != null)
                newStationname = String.Empty;

            if(InpBox.Show("create a new station", "insert the name of the new station", ref newStationname) == System.Windows.Forms.DialogResult.OK)
            { 
                throw new NotImplementedException();
                existing = null ; //_Milkyway.getStations(EDMilkyway.enDataType.Data_Merged).Find(x => (x.Name.Equals(newStationname, StringComparison.InvariantCultureIgnoreCase)) && 
                                  //                                                           (x.SystemId.Equals(m_currentSystemdata.Id)));

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
                {
                    EDSystem System = null;
                    //if (_Milkyway.getSystems(EDMilkyway.enDataType.Data_EDDB).Exists(x => x.Name.Equals(m_loadedSystemdata.Name, StringComparison.InvariantCultureIgnoreCase)))

                    throw new NotImplementedException();

                    if(false)
                    {
                        txtSystemName.ReadOnly      = true;
                        lblSystemRenameHint.Visible = true;
                    }
                    else
                    {
                        txtSystemName.ReadOnly      = false;
                        lblSystemRenameHint.Visible = false;
                    }
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

                cbStationHasMarket.ReadOnly        = !enabled;
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
                {
                    throw new NotImplementedException();
                    //if (_Milkyway.getStations(EDMilkyway.enDataType.Data_EDDB).Exists(x => (x.Name.Equals(m_loadedStationdata.Name, StringComparison.InvariantCultureIgnoreCase)) && 
                    //                                                                       (x.SystemId.Equals(m_currentSystemdata.Id))))
                    if(false)
                    {
                        txtStationName.ReadOnly      = true;
                        lblStationRenameHint.Visible = true;
                    }
                    else
                    {
                        txtStationName.ReadOnly      = false;
                        lblStationRenameHint.Visible = false;
                    }
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
                cErr.processError(ex, "Error when starting station edit");
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
                cErr.processError(ex, "Error when starting station edit");
            }
        }


#endregion

        public string getAppPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

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

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            Process.Start(Program.GIT_PATH + "/releases");
        }

        private void llVisitUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void cbSpoolEddnToFile_CheckedChanged(object sender, EventArgs e)
        {
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "SpoolEddnToFile", cbSpoolEddnToFile.Checked.ToString());
        }

        private void cbSpoolImplausibleToFile_CheckedChanged(object sender, EventArgs e)
        {
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "SpoolImplausibleToFile", cbSpoolImplausibleToFile.Checked.ToString());
        }

        private void cbEDDNAutoListen_CheckedChanged(object sender, EventArgs e)
        {
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "EDDNAutoListen", cbEDDNAutoListen.Checked.ToString());
        }

        private void checkboxImportEDDN_CheckedChanged(object sender, EventArgs e)
        {
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "EDDNAutoImport", checkboxImportEDDN.Checked.ToString());
        }

        private void cbStartWebserverOnLoad_CheckedChanged(object sender, EventArgs e)
        {
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "StartWebserverOnLoad", cbStartWebserverOnLoad.Checked.ToString());
        }

     

        

        #region Theming
        private void pbForegroundColour_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour", "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                                          c.Color.B.ToString("X2"));

                ShowSelectedUiColours();
                Retheme();
            }

        }

        private void pbBackgroundColour_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour", "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                          c.Color.B.ToString("X2"));
                ShowSelectedUiColours();
                Retheme();
            }
        }

        private void ShowSelectedUiColours()
        {
            if (pbForegroundColour.Image != null) pbForegroundColour.Image.Dispose();
            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour") != "")
            {
                ForegroundSet.Visible = false;
                Bitmap b = new Bitmap(32, 32);
                int red = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour").Substring(1, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int green = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour").Substring(3, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int blue = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour").Substring(5, 2),
                    System.Globalization.NumberStyles.HexNumber);

                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.FromArgb(red, green, blue));
                }
                pbForegroundColour.Image = b;
            }
            else ForegroundSet.Visible = true;

            if (Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour", "") != "")
            {
                BackgroundSet.Visible = false;
                if (pbBackgroundColour.Image != null) pbBackgroundColour.Image.Dispose();
                Bitmap b = new Bitmap(32, 32);
                int red = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour").Substring(1, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int green = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour").Substring(3, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int blue = int.Parse(Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour").Substring(5, 2),
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
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour", "");
            Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour", "");
        }

        private void ForegroundSet_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "ForegroundColour", "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                                          c.Color.B.ToString("X2"));

                ShowSelectedUiColours();
                Retheme();
            }
        }

        private void BackgroundSet_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                Program.DBCon.setIniValue(IBE.MTSettings.tabSettings.DB_GROUPNAME, "BackgroundColour", "#" + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") +
                                          c.Color.B.ToString("X2"));
                ShowSelectedUiColours();
                Retheme();
            }
        }

        #endregion


        /// <summary>
        /// opens the data import dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var DataIO = new frmDataIO();

                DataIO.ShowDialog(this);

                DataIO.Dispose();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while opening import tool");
            }
        }

      

        #region HTML

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
                Program.Logger.Log("Error starting webserver", true);
                Program.Logger.Log(ex.ToString(), true);
                Program.Logger.Log(ex.Message, true);
                Program.Logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    Program.Logger.Log(ex.InnerException.ToString(), true);
                MsgBox.Show(
                    "Couldn't start webserver.  Maybe something is already using port 8080...?");
            }
        }
        
        public string GetLvAllCommsItems()
        {
            //if (InvokeRequired)
            //{
            //    return (string)(Invoke(new EventArgsDelegate(GetLvAllCommsItems)));
            //}

            //if (StationDirectory.Count == 0)
            //    return "No data loaded :-(";

            var s = new StringBuilder();

            //string links = "<BR><A style=\"font-size: 14pt\" HREF=\"#lvAllComms\">All newCommodityClassification - </A>" +
            //    "<A style=\"font-size: 14pt\" HREF=\"#lbPrices\">Location - </A>" +
            //    "<A style=\"font-size: 14pt\" HREF=\"#lbCommodities\">Classification - </A>" +
            //        "<A style=\"font-size: 14pt\" HREF=\"#lvStationToStation\">Location-to-Location</A><BR>";

            //s.Append(links);

            //s.Append("<A name=\"lvAllComms\"><P>All newCommodityClassification</P>");
            //s.Append(GetHTMLForListView(lvAllComms));

            //s.Append(links);

            //s.Append("<A name=\"lbPrices\"><P>Location: " + getCmbItemKey(cmbStation.SelectedItem) + "</P>");
            //s.Append(GetHTMLForListView(lbPrices));

            //s.Append(links);

            //s.Append("<A name=\"lbCommodities\"><P>Classification: " + cbCommodity.SelectedItem + "</P>");
            //s.Append(GetHTMLForListView(lbCommodities));

            //s.Append(links);

            //s.Append("<A name=\"lvStationToStation\"><P>Location-to-Location: " + getCmbItemKey(cmbStationToStationFrom.SelectedItem) + " => " + getCmbItemKey(cmbStationToStationTo.SelectedItem) + "</P>");
            //s.Append(GetHTMLForListView(lvStationToStation));

            //s.Append(links);

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
                header.Append("<TD " + style + "><A style=\"color: #" + Program.DBCon.getIniValue<String>(IBE.MTSettings.tabSettings.DB_GROUPNAME, "WebserverForegroundColor", "") + "\" HREF=\"resortlistview.html?grid=" + listViewToDump.Name + "&col=" + i + "&rand=" + random.Next() + "#" + listViewToDump.Name + "\">" + listViewToDump.Columns[i].Text + "</A></TD>");
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



        #endregion

        #region ExternalTool

        private FileScanner.EDLogfileScanner    m_LogfileScanner;

        private void cmdLanded_Click(object sender, EventArgs e)
        {
            String extSystem    = "";
            String extStation   = "";
            String extInfo      = "";
            String ErrInfo      = "";
            Boolean ContinueChecking = true;
            try
            {
                
                cmdConfirm.Enabled = false;

                do
                {
                    Cursor = Cursors.WaitCursor;

                    txtExtInfo.Text = "checking for current station...";
                    txtExtInfo.Refresh();

                    txtRecievedSystem.Text          = "";
                    txtRecievedStation.Text         = "";

                    if(!Program.ExternalData.getLocation(out extSystem, out extStation, out extInfo, out ErrInfo))
                    { 
                        txtExtInfo.Text             = "Message:\n" + extInfo;
                        txtExtInfo2.Text            = "Message:\n" + ErrInfo;
                        ContinueChecking            = false;
                    }
                    else
                    {
                        txtExtInfo.Text             = "checking for current station...<ok>";
                        txtExtInfo2.Text            = "Message:\n" + ErrInfo;
                        txtRecievedSystem.Text      = extSystem;
                        txtRecievedStation.Text     = extStation;
                        ContinueChecking            = false;

                        if(Program.actualCondition.System.Equals(txtRecievedSystem.Text))
                            cmdConfirm.Enabled      = true;
                        else
                            txtExtInfo.Text             = "The external recieved system does not correspond to the system from the logfile!";
                    }


                } while (ContinueChecking);
                
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                throw new Exception("Error while retreiving station information from external Interface", ex);
            }
        }

        /// <summary>
        /// confirms the retrieved location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdConfirm_Click(object sender, EventArgs e)
        {
            DialogResult MBResult = System.Windows.Forms.DialogResult.OK;

            try
            {
                Cursor = Cursors.WaitCursor;

                cmdGetMarketData.Enabled = false;

                if(!Program.actualCondition.System.Equals(txtRecievedSystem.Text))
                {
                    MBResult = MessageBox.Show("The external recieved system does not correspond to the system from the logfile!\n" +
                                               "Confirm even so ?", "Unexpected system retrieved !", 
                                               MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                }

                if(MBResult == System.Windows.Forms.DialogResult.OK)
                {
                    Program.ExternalData.Confirm();
                    cmdGetMarketData.Enabled = true;
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                throw new Exception("Error while confirming retrieved location", ex);
            }
        }

        private void cmdGetMarketData_Click(object sender, EventArgs e)
        {
            Int32 DataCount     = 0;
            String extSystem    = "";
            String extStation   = "";
            String extInfo      = "";
            String ErrInfo      = "";

            try
            {
                Cursor = Cursors.WaitCursor;

                txtExtInfo.Text = "getting market data...";
                txtExtInfo.Refresh();

                if(!Program.ExternalData.getMarketData(out extSystem, out extStation, out extInfo, out ErrInfo, out DataCount))
                { 
                    txtExtInfo.Text             = "Message:\n" + extInfo;
                    txtExtInfo2.Text            = "Message:\n" + ErrInfo;
                }
                else
                {
                    txtLocalDataCollected.Text  = String.Format("{0} (utc): {1} prices collected", DateTime.UtcNow, DataCount);
                    txtExtInfo.Text             = "getting market data......<ok>";
                    txtExtInfo2.Text            = "Message:\n" + ErrInfo;
                    txtRecievedSystem.Text      = extSystem;
                    txtRecievedStation.Text     = extStation;
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                throw new Exception("/* whatever */", ex);
            }
        }

        void LogfileScanner_LocationChanged(object sender, FileScanner.EDLogfileScanner.LocationChangedEventArgs e)
        {
            try
            {

                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.Jump) > 0)
                {
                    setText(txtExtInfo,             "jump recognized...");
                    setText(txtRecievedSystem,      "");
                    setText(txtRecievedStation,     "");
                    setText(txtLocalDataCollected,  "");

                    setButton(cmdConfirm, false);
                    setButton(cmdGetMarketData, false);

                    Program.actualCondition.Location = e.Location;
                    setText(tbCurrentStationinfoFromLogs, Program.actualCondition.Location);
                }

                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.System) >  0)
                {
                    // the location has changed -> the reference for all distances has changed  
                    Program.PriceAnalysis.GUI.SignalizeChangedData();

                    Program.actualCondition.System   = e.System;
                    setText(tbCurrentSystemFromLogs,      Program.actualCondition.System);
                }

                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.Location) > 0)
                {
                    Program.actualCondition.Location   = e.Location;
                    setText(tbCurrentStationinfoFromLogs,  Program.actualCondition.Location);
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error in m_LogfileScanner_LocationChanged", ex);
            }
        }

        void ExternalDataInterface_ExternalDataEvent(object sender, ExternalDataInterface.LocationChangedEventArgs e)
        {
            try
            {
                if((e.Changed & ExternalDataInterface.enExternalDataEvents.Landed) != 0)
                {
                    Program.actualCondition.System   = e.System;
                    Program.actualCondition.Location = e.Location;
                    
                    setText(tbCurrentSystemFromLogs,      Program.actualCondition.System);
                    setText(tbCurrentStationinfoFromLogs, Program.actualCondition.Location);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the LocationChanged-event", ex);
            }
        }

#endregion

        private void editLocalizationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var newForm = new LanguageEdit();

                newForm.ShowDialog(this);

                newForm.Dispose();
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while opening localization editing tool");
            }
        }

#region "Test"

        private void createJumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Program.LogfileScanner.RaiseTestEvent();
            }
            catch (Exception ex)
            {
                cErr.processError(ex);
            }
        }

#endregion

    }

}
