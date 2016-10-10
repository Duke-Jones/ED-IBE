using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Reflection;
using IBE.Enums_and_Utility_Classes;
using Microsoft.Win32;
using IBE.EDDB_Data;
using CodeProject.Dialog;
using IBE.SQL;
using IBE.MTCommandersLog;
using IBE.MTPriceAnalysis;
using IBE.Ocr;

namespace IBE
{
    public partial class Form1 : RNBaseForm
    {

/* ************************************************ */
 // new stuff

        private Dictionary<String, Boolean> m_IsRefreshed;
        private TabPage                     m_PreviousTab = null;

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
        
        public Random random = new Random();
        public Guid SessionGuid;
        public PropertyInfo[] LogEventProperties;

        //public Dictionary<byte, string> CommodityLevel = new Dictionary<byte, string>();
        private Ocr.Ocr ocr; //TODO is this needed here?
        private ListViewColumnSorter _stationColumnSorter, _commodityColumnSorter, _allCommodityColumnSorter, _stationToStationColumnSorter, _stationToStationReturnColumnSorter, _commandersLogColumnSorter;
        
        public TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;
        public Levenshtein _levenshtein = new Levenshtein();
        private dsCommodities _commodities = new dsCommodities();
        private TabPage _EDDNTabPage;
        private Int32 _EDDNTabPageIndex;
        private string _LoggedSystem        = ID_NOT_SET;
        private string _LoggedLocation      = ID_NOT_SET;
        private string _LoggedVisited       = ID_NOT_SET;
        private string _LoggedMarketData    = ID_NOT_SET;

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

        public tabOCR cOcrCaptureAndCorrect               =  new tabOCR();

        [SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
        public Form1()
        {
            Program.SplashScreen.InfoAdd("initialize components...");
            InitializeComponent();
            Program.MainLog.Log("  - initialised component");
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
                        Program.MainLog.Log("  - product path set");
                        Program.SplashScreen.InfoAppendLast("<OK>");

                        SetProductAppDataPath();
                        Program.MainLog.Log("  - product appdata set");

                        Program.SplashScreen.InfoAdd("load game settings...");
                        Program.GameSettings = new GameSettings();
                        Program.MainLog.Log("  - loaded game settings");
                        Program.SplashScreen.InfoAppendLast("<OK>");

                        retry = false;
                    }
                    catch (Exception ex)
                    {
                        if (retry)
                            throw new Exception("could't verify productpath and/or gamepath", ex);

                        Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "ProductsPath", "");
                        Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "GamePath", "");
                        retry = true;
                    }
                } while (retry);

                Program.SplashScreen.InfoAdd("prepare network interfaces...");
                PopulateNetworkInterfaces();
                Program.MainLog.Log("  - populated network interfaces");
                Program.SplashScreen.InfoAppendLast("<OK>");

                /*Program.SplashScreen.InfoAdd("create OCR object...");
               // ocr = new Ocr.Ocr(this); //moved to ocrcaptureandcorrect
                Program.MainLog.Log("  - created OCR object");
                Program.SplashScreen.InfoChange("create OCR object...<OK>"); */

                Application.ApplicationExit += Application_ApplicationExit;
                Program.MainLog.Log("  - set application exit handler");

                Program.SplashScreen.InfoAdd("initiate ocr...");
                var OcrCapAndCorrectTabPage = new TabPage("Capture And Correct");
                OcrCapAndCorrectTabPage.Name = "OCR_CaptureAndCorrect";
                cOcrCaptureAndCorrect.Dock = DockStyle.Fill;
                cOcrCaptureAndCorrect._parent = this;

                //if(Debugger.IsAttached)
                //{
                //    OcrCapAndCorrectTabPage.Controls.Add(cOcrCaptureAndCorrect);
                //    tabCtrlOCR.Controls.Add(OcrCapAndCorrectTabPage);
                //    Program.MainLog.Log("  - initialised Ocr ");
                //    Program.SplashScreen.InfoAppendLast("<OK>");

                //    Program.SplashScreen.InfoAdd("create ocr calibrator...");
                //    var OcrCalibratorTabPage = new TabPage("OCR Calibration");
                //    OcrCalibratorTabPage.Name = "OCR_Calibration";
                //    var oct = new OcrCalibratorTab { Dock = DockStyle.Fill };
                //    OcrCalibratorTabPage.Controls.Add(oct);
                //    tabCtrlOCR.Controls.Add(OcrCalibratorTabPage);
                //    Program.MainLog.Log("  - initialised Ocr Calibrator");
                //    Program.SplashScreen.InfoAppendLast("<OK>");
                //}

                Program.SplashScreen.InfoAdd("apply settings...");
                ApplySettings();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.MainLog.Log("  - applied settings");

                if (!Directory.Exists(Program.GetDataPath("OCR Correction Images")))
                    Directory.CreateDirectory(Program.GetDataPath("OCR Correction Images"));

    //DEBUG: removed for the moment because I got strange behaviour in an full new/uninitialized environment (-> investigation needed)
                //setOCRTabsVisibility();

                Program.SplashScreen.InfoAdd("prepare system/location view...");
                //prePrepareSystemAndStationFields();
                if (Debugger.IsAttached && Program.showToDo)
                    MessageBox.Show(this, "todo");
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

                // start clock on form
                Clock = new System.Windows.Forms.Timer();
                Clock.Interval = 1000;
                Clock.Start();
                Clock.Tick += Clock_Tick;

                Program.SplashScreen.InfoAdd("initialize Priceanalysis tab...");
                // Price Analysis
                tabPriceAnalysis newPAControl     = new tabPriceAnalysis();
                newPAControl.DataSource           = Program.PriceAnalysis;
                newTab                            = new TabPage("Price Analysis");
                newTab.Name                       = newPAControl.Name;
                newTab.Controls.Add(newPAControl);
                tabCtrlMain.TabPages.Insert(2, newTab);
                Program.SplashScreen.InfoAppendLast("<OK>");

                Program.SplashScreen.InfoAdd("initialize Commander's Log tab...");
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

                // register events for getting new location- and data infos for the gui
                Program.LogfileScanner.LocationChanged += LogfileScanner_LocationChanged;
                
                Program.CompanionIO.ExternalDataEvent  += ExternalDataInterface_ExternalDataEvent;
                Program.EDDNComm.DataChangedEvent      += EDDNComm_DataChangedEvent;
                Program.EDDNComm.DataTransmittedEvent  += EDDNComm_DataTransmittedEvent;


                // until this is working again 
                tabCtrlMain.TabPages.Remove(tabCtrlMain.TabPages["tabSystemData"]);
                tabCtrlMain.TabPages.Remove(tabCtrlMain.TabPages["tabWebserver"]);

                Retheme();

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
                                       Path.Combine(Environment.GetEnvironmentVariable("PROGRAMFILES(X86)"), @"Steam\steamapps\common"), 
                                       @"F:\" };

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
                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = false;

                var dialogResult = dialog.ShowDialog();

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = true;

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

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = false;

                var MBResult = MsgBox.Show(
                    "Hm, that doesn't seem right" +
                    (dialog.SelectedPath != "" ? ", " + dialog.SelectedPath + " isn't the Frontier 'Products' directory"  : "")
                + ". Please try again...", "", MessageBoxButtons.RetryCancel);

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = true;

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Environment.Exit(-1);
            }
        }
        private void SetProductPath()
        {
            //Already set, no reason to set it again :)
            if (Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "ProductsPath", "") != "" && Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "GamePath", "") != "") return;
            
            //Automatic
            var path = getProductPathAutomatically();

            //Automatic failed, Ask user to find it manually
            if (path == null)
            {
                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = false;

                var MBResult = MsgBox.Show("Automatic discovery of Frontier directory failed, please point me to your Frontier 'Products' directory.", "", MessageBoxButtons.OKCancel);

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = true;

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
                            Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "GamePath", youngestPath);
                        }
                        else
                        {
                            String x64Dir = gamedirs.FirstOrDefault(x => Path.GetFileName(x) == "elite-dangerous-64");

                            if(x64Dir != null)
                            {
                                Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "GamePath", x64Dir);
                            }
                            else
                            {
                                //Get highest Forc-fdev dir.
                                Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "GamePath", gamedirs.OrderByDescending(x => x).ToArray()[0]);
                            }
                        }

                        b = true;
                        continue;

                    }

                    if(!Program.SplashScreen.IsDisposed)
                        Program.SplashScreen.TopMost = false;

                    var MBResult = MsgBox.Show("Couldn't find a FORC-FDEV.. directory in the Frontier Products dir, please try again...", "", MessageBoxButtons.RetryCancel);

                    if(!Program.SplashScreen.IsDisposed)
                        Program.SplashScreen.TopMost = true;

                    if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                        Environment.Exit(-1);

                    path = getProductPathManually();
                    dirs = Directory.GetDirectories(path);
                }

                Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "ProductsPath", path);
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
                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = false;

                var dialogResult = dialog.ShowDialog();

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = true;

                if (dialogResult == DialogResult.OK)
                {
                    if (Path.GetFileName(dialog.SelectedPath) == "Options")
                    {
                        return dialog.SelectedPath;

                    }
                }

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = false;

                var MBResult = MsgBox.Show(
                    "Hm, that doesn't seem right, " + dialog.SelectedPath +
                    " is not the Game Options directory, Please try again", "", MessageBoxButtons.RetryCancel);

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = true;

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Application.Exit();

            }
        }
        private void SetProductAppDataPath()
        {
            //Already set, no reason to set it again :)
            if (Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "ProductAppData") != "") return;

            //Automatic
            var path = getProductAppDataPathAutomatically();

            //Automatic failed, Ask user to find it manually
            if (path == null)
            {
                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = false;

                var MBResult = MsgBox.Show(@"Automatic discovery of the Game Options directory failed, please point me to it...", "", MessageBoxButtons.RetryCancel);

                if(!Program.SplashScreen.IsDisposed)
                    Program.SplashScreen.TopMost = true;

                if (MBResult == System.Windows.Forms.DialogResult.Cancel)
                    Application.Exit();

                path = getProductAppDataPathManually();
            }

            Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "ProductAppData", path);
        }

        private void ApplySettings()
        {
            if (Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverForegroundColor") != "") 
                tbForegroundColour.Text = Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverForegroundColor");

            if (Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverBackgroundColor") != "") 
                tbBackgroundColour.Text = Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverBackgroundColor");

            txtWebserverPort.Text = Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverPort");

            if (Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverIpAddress") != "") 
                cbInterfaces.Text = Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverIpAddress");
            
            if (Program.DBCon.getIniValue<Boolean>(IBE.IBESettingsView.DB_GROUPNAME, "StartWebserverOnLoad", false.ToString(), false, true))
            {
                cbStartWebserverOnLoad.Checked = true;
                bStart_Click(null, null);
            }
   
        }

       
      

        //public void ActivateOCRTab()
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new MethodInvoker(ActivateOCRTab));
        //        return;
        //    }

        //    if (Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "AutoActivateOCRTab", true.ToString(), false, true) && !Program.DBCon.getIniValue<Boolean>(IBESettingsView.DB_GROUPNAME, "CheckNextScreenshotForOne", false.ToString(), false, true))
        //        try
        //        {
        //            tabCtrlMain.SelectedTab = tabCtrlMain.TabPages["tabOCRGroup"];
        //            tabCtrlOCR.SelectedTab  = tabCtrlOCR.TabPages["tabOCR"];
        //        }
        //        catch (Exception)
        //        {
        //        }
        //}

     

        void Application_ApplicationExit(object sender, EventArgs e)
        {


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
                Cursor = Cursors.WaitCursor;

                if (_tooltip != null) _tooltip.RemoveAll();
                if (_tooltip2 != null) _tooltip2.RemoveAll();

                CleanPreviousTab();
                RefreshCurrentTab();

                m_PreviousTab = e.TabPage;

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                CErr.processError(ex, "Error when selecting a new tab on main tabcontrol");
            }
        }

        /// <summary>
        /// start cleanups on last tab if necessary
        /// </summary>
        private void CleanPreviousTab()
        {
            try
            {
                if(m_PreviousTab != null)
                {
                    switch (m_PreviousTab.Name)
                    {
                        case "tabCommandersLog":
                            Program.CommandersLog.GUI.Unselected();
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
                throw new Exception("Error while initiate cleanup on last tab", ex);
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
                Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "WebserverIpAddress", cbInterfaces.SelectedItem.ToString());
            }

            Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "WebserverPort", txtWebserverPort.Text);

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
            Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "WebserverForegroundColor", tbForegroundColour.Text);
        }

        private void tbBackgroundColour_TextChanged(object sender, EventArgs e)
        {
            sws.BackgroundColour = tbBackgroundColour.Text;
            cbColourScheme.SelectedItem = null;
            Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "WebserverBackgroundColor", tbBackgroundColour.Text);
        }

        private delegate void del_setText(Control Destination, String newText);

        public void setText(Control Destination, String newText)
        {
            if(Destination.InvokeRequired)
                Destination.Invoke(new tabOCR.del_setControlText(setText), Destination, newText);
            else
                Destination.Text = newText;
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

        System.Windows.Forms.Timer _timer;

        private async void Form_Shown_async(object sender, System.EventArgs e)
        {
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                Version newVersion;
                String newInfo;

                Debug.Print("Zeit (1) : " + st.ElapsedMilliseconds);
                st.Start();

                Program.SplashScreen.InfoAdd("checking for updates");

                if (Updater.CheckVersion(out newVersion, out newInfo))
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

                Debug.Print("Zeit (2) : " + st.ElapsedMilliseconds);
                st.Start();

                Program.SplashScreen.InfoAdd("load system data...");
                loadSystemData(Program.actualCondition.System);
                loadStationData(Program.actualCondition.System, Program.actualCondition.Location);
                Program.SplashScreen.InfoAppendLast("<OK>");

                Debug.Print("Zeit (3) : " + st.ElapsedMilliseconds);
                st.Start();

                Debug.Print("Zeit (4) : " + st.ElapsedMilliseconds);
                st.Start();

                Program.SplashScreen.InfoAdd("init price analysis gui");
                Program.PriceAnalysis.GUI.Init();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Debug.Print("Zeit (5) : " + st.ElapsedMilliseconds);
                st.Start();

                Program.SplashScreen.InfoAdd("init commanders log gui");
                Program.CommandersLog.GUI.Init();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Debug.Print("Zeit (6) : " + st.ElapsedMilliseconds);
                st.Start();

                Program.SplashScreen.InfoAdd("init system numbers");
                showSystemNumbers();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Debug.Print("Zeit (7) : " + st.ElapsedMilliseconds);
                st.Start();

                Program.SplashScreen.InfoAdd("init main gui");
                SetupGui();

                ShowLocationData();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Debug.Print("Zeit (8) : " + st.ElapsedMilliseconds);
                st.Start();

                Program.SplashScreen.InfoAdd("starting logfile scanner");
                Program.LogfileScanner.Start();
                Program.SplashScreen.InfoAppendLast("<OK>");

                Debug.Print("Zeit (9) : " + st.ElapsedMilliseconds);
                st.Start();
                
                // *******************************************************************
                await Updater.DoSpecial(this);
                // *******************************************************************

                if (Program.DBCon.getIniValue<Boolean>("EDDN", "AutoListen", false.ToString(), false))
                {
                    Program.SplashScreen.InfoAdd("starting eddn listening");
                    Program.EDDNComm.StartEDDNListening();
                    Program.SplashScreen.InfoAppendLast("<OK>");
                }

                if(Program.DBCon.getIniValue<Boolean>("EDDN", "AutoSend", true.ToString(), false))
                    Program.EDDNComm.ActivateSender();

                SetQuickDecisionSwitch();

                cbEDDNOverride.CheckedChanged += cbEDDNOverride_CheckedChanged;

                Debug.Print("Zeit (11) : " + st.ElapsedMilliseconds);
                st.Start();

                if(Program.DBCon.getIniValue<Boolean>(frmDataIO.DB_GROUPNAME, "AutoImportEDCDData", true.ToString(), false))
                {
                    // import new edcd data if available
                    var DataIO = new frmDataIO();

                    DataIO.InfoTarget = Program.SplashScreen.SplashInfo;

                    DataIO.StartEDCDCheck();

                    DataIO.Close();
                    DataIO.Dispose();

                }

                Program.SplashScreen.InfoAdd("init sequence finished !");
                Program.SplashScreen.CloseDelayed();

                Debug.Print("Zeit (12) : " + st.ElapsedMilliseconds);
                st.Start();
                
                /// last preparations for companion io 
                ShowStatus();
                Program.CompanionIO.AsyncDataRecievedEvent += CompanionIO_AsyncDataRecievedEvent;
                if((Program.CompanionIO.CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok) && (!Program.CompanionIO.StationHasShipyardData()))
                {
                    int? stationID = Program.actualCondition.Location_ID;

                    if((stationID != null) && Program.DBCon.Execute<Boolean>("select has_shipyard from tbStations where id = " + stationID))
                    {
                        // probably companion error, try once again in 5 seconds
                        Program.CompanionIO.ReGet_StationData();
                    }
                }
                
                // inform GUI from EDSM
                Program.EDSMComm.DataTransmittedEvent += EDSMComm_DataTransmittedEvent;

                this.Enabled = true;

                Program.StartVNCServer(this);
                
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                CErr.processError(ex, "Error in Form_Shown");
            }
        }

        /// <summary>
        /// sets the "Quick Decision Checkbox" value in dependance of the settings
        /// </summary>
        private void SetQuickDecisionSwitch()
        {
            try
            { 

                if (Program.EDDNComm.SenderIsActivated)
                {
                    cbEDDNOverride.Enabled = true;

                    String decValue = Program.DBCon.getIniValue<String>(IBE.EDDN.EDDNView.DB_GROUPNAME, "QuickDecisionDefault", "Hold", false);

                    switch (decValue)
                    {
                        case "Send":
                            cbEDDNOverride.Checked = true;
                            break;

                        case "NotSend":
                            cbEDDNOverride.Checked = false;
                            break;

                        case "Hold":
                            cbEDDNOverride.Checked = Program.DBCon.getIniValue<Boolean>(IBE.EDDN.EDDNView.DB_GROUPNAME, "QuickDecisionValue", false.ToString(), false);
                            break;

                        default:
                            Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "QuickDecisionDefault", "Hold");
                            Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "QuickDecisionValue", false.ToString());
                            cbEDDNOverride.Checked = false;
                            break;
                    }
                }
                else
                {
                    cbEDDNOverride.Checked = false;
                    cbEDDNOverride.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting the quick decision switch", ex);
            }
        }

        void cbEDDNOverride_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "QuickDecisionValue", cbEDDNOverride.Checked.ToString());
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cbEDDNOverride_CheckedChanged");
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

        //public void setOCRTabsVisibility()
        //{
        //    try
        //    {
        //        try
        //        {
        //            if (this.InvokeRequired)
        //                this.Invoke(new MethodInvoker(setOCRTabsVisibility));
        //            else
        //            {
        //                if (Program.GameSettings != null && tabCtrlOCR.TabPages["OCR_Calibration"] != null)
        //                {
        //                    var OCRTabPage = tabCtrlOCR.TabPages["OCR_Calibration"];
        //                    OCRTabPage.Enabled = (Program.GameSettings.Display != null);
        //                    var TabControl = (OcrCalibratorTab)(OCRTabPage.Controls[0]);

        //                    TabControl.lblWarning.Visible = (Program.GameSettings.Display == null); 
        //                }

        //                if (Program.GameSettings != null && tabCtrlOCR.TabPages["OCR_CaptureAndCorrect"] != null)
        //                {
        //                    var OCRTabPage = tabCtrlOCR.TabPages["OCR_CaptureAndCorrect"];
        //                    OCRTabPage.Enabled = (Program.GameSettings.Display != null);
        //                    var TabControl = (tabOCR)(OCRTabPage.Controls[0]);
        //                    TabControl.startOcrOnload(Program.DBCon.getIniValue<Boolean>(IBE.IBESettingsView.DB_GROUPNAME, "StartOCROnLoad", false.ToString(), false, true) && Directory.Exists(Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "MostRecentOCRFolder", "")));
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("Error in setOCRTabsVisibility (inline)", ex);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error in setOCRTabsVisibility (outline)", ex);
        //    }
        //}

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
                    MessageBox.Show(this, "TODO");
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
                cbSystemNeedsPermit.Checked = m_loadedSystemdata.NeedsPermit;
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
                    MessageBox.Show(this, "TODO");
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

                cbStationHasMarket.Checked = m_loadedStationdata.HasMarket;
                cbStationHasBlackmarket.Checked = m_loadedStationdata.HasBlackmarket;
                cbStationHasOutfitting.Checked = m_loadedStationdata.HasOutfitting;
                cbStationHasShipyard.Checked = m_loadedStationdata.HasShipyard;
                cbStationHasRearm.Checked = m_loadedStationdata.HasRearm;
                cbStationHasRefuel.Checked = m_loadedStationdata.HasRefuel;
                cbStationHasRepair.Checked = m_loadedStationdata.HasRepair;

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
                    MessageBox.Show(this, "TODO");

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
                CErr.processError(ex, "Error in txtSystem_GotFocus-Event");                
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
                CErr.processError(ex, "Error in txtSystem_GotFocus-Event");                
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
                    m_currentSystemdata.NeedsPermit = cbSystemNeedsPermit.Checked;

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
                    m_currentSystemdata.NeedsPermit = cbSystemNeedsPermit.Checked;
                    break;

                case "cbStationHasCommodities":
                    m_currentStationdata.HasMarket = cbStationHasMarket.Checked;
                    break;

                case "cbStationHasBlackmarket":
                    m_currentStationdata.HasBlackmarket = cbStationHasBlackmarket.Checked;
                    break;

                case "cbStationHasOutfitting":
                    m_currentStationdata.HasOutfitting = cbStationHasOutfitting.Checked;
                    break;

                case "cbStationHasShipyard":
                    m_currentStationdata.HasShipyard = cbStationHasShipyard.Checked;
                    break;

                case "cbStationHasRearm":
                    m_currentStationdata.HasRearm = cbStationHasRearm.Checked;
                    break;

                case "cbStationHasRefuel":
                    m_currentStationdata.HasRefuel = cbStationHasRefuel.Checked;
                    break;

                case "cbStationHasRepair":
                    m_currentStationdata.HasRepair = cbStationHasRepair.Checked;
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
                CErr.processError(ex, "Error when starting station edit");
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
                CErr.processError(ex, "Error when starting station edit");
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

        private void cbStartWebserverOnLoad_CheckedChanged(object sender, EventArgs e)
        {
            Program.DBCon.setIniValue(IBE.IBESettingsView.DB_GROUPNAME, "StartWebserverOnLoad", cbStartWebserverOnLoad.Checked.ToString());
        }

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
                CErr.processError(ex, "Error while opening import tool");
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
                Program.MainLog.Log("Error starting webserver");
                Program.MainLog.Log(ex.ToString());
                Program.MainLog.Log(ex.Message);
                Program.MainLog.Log(ex.StackTrace);
                if (ex.InnerException != null)
                    Program.MainLog.Log(ex.InnerException.ToString());
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

            //currentPriceData.Append(links);

            //currentPriceData.Append("<A name=\"lvAllComms\"><P>All newCommodityClassification</P>");
            //currentPriceData.Append(GetHTMLForListView(lvAllComms));

            //currentPriceData.Append(links);

            //currentPriceData.Append("<A name=\"lbPrices\"><P>Location: " + getCmbItemKey(cmbStation.SelectedItem) + "</P>");
            //currentPriceData.Append(GetHTMLForListView(lbPrices));

            //currentPriceData.Append(links);

            //currentPriceData.Append("<A name=\"lbCommodities\"><P>Classification: " + cbCommodity.SelectedItem + "</P>");
            //currentPriceData.Append(GetHTMLForListView(lbCommodities));

            //currentPriceData.Append(links);

            //currentPriceData.Append("<A name=\"lvStationToStation\"><P>Location-to-Location: " + getCmbItemKey(cmbStationToStationFrom.SelectedItem) + " => " + getCmbItemKey(cmbStationToStationTo.SelectedItem) + "</P>");
            //currentPriceData.Append(GetHTMLForListView(lvStationToStation));

            //currentPriceData.Append(links);

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
                header.Append("<TD " + style + "><A style=\"color: #" + Program.DBCon.getIniValue<String>(IBE.IBESettingsView.DB_GROUPNAME, "WebserverForegroundColor", "") + "\" HREF=\"resortlistview.html?grid=" + listViewToDump.Name + "&col=" + i + "&rand=" + random.Next() + "#" + listViewToDump.Name + "\">" + listViewToDump.Columns[i].Text + "</A></TD>");
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

#if false



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
                    MBResult = MessageBox.Show(this, "The external recieved system does not correspond to the system from the logfile!\n" +
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

#endif

        void LogfileScanner_LocationChanged(object sender, FileScanner.EDLogfileScanner.LocationChangedEventArgs e)
        {
            try
            {
                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.Jump) > 0)
                {
                    setText(txtEventInfo,             "...jump recognized...");

                    Program.actualCondition.Location = e.Location;

                    // can't be docked anymore
                    Program.CompanionIO.SetDocked(false);

                    // from now it is allowed to send data to eddn immediately again
                    Program.EDDNComm.SendingReset();

                    ShowLocationData();
                    ShowStatus();
                }

                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.System) >  0)
                {
                    // the location has changed -> the reference for all distances has changed  
                    Program.PriceAnalysis.GUI.SignalizeChangedData();

                    Program.actualCondition.System      = e.System;
                    Program.actualCondition.Coordinates = e.Position;

                    /// after a system jump you can get data immediately
                    Program.CompanionIO.RestTimeReset();

                    ShowLocationData();

                }

                if((e.Changed & FileScanner.EDLogfileScanner.enLogEvents.Location) > 0)
                {
                    Program.actualCondition.Location   = e.Location;
                    ShowLocationData();   
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error in m_LogfileScanner_LocationChanged", ex);
            }
        }

        void ExternalDataInterface_ExternalDataEvent(object sender, IBE.IBECompanion.DataEventBase.LocationChangedEventArgs e)
        {
            try
            {
                if((e.Changed & IBE.IBECompanion.DataEventBase.enExternalDataEvents.Landed) != 0)
                {
                    ShowLocationData();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the LocationChanged-event", ex);
            }
        }

        void EDDNComm_DataChangedEvent(object sender, EDDN.EDDNCommunicator.DataChangedEventArgs e)
        {
            try
            {
                if(e.DataType == EDDN.EDDNCommunicator.enDataTypes.DataImported)
                {
                    Program.PriceAnalysis.GUI.setFilterHasChanged(true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the LocationChanged-event", ex);
            }
        }

        void EDDNComm_DataTransmittedEvent(object sender, EDDN.EDDNCommunicator.DataTransmittedEventArgs e)
        {
            try
            {
                ShowStatus();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the EDDNComm_DataTransmittedEvent", ex);
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
                CErr.processError(ex, "Error while opening localization editing tool");
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
                CErr.processError(ex);
            }
        }

#endregion

        private void directDBAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var newForm = new DirectSQL(Program.DBCon);
                newForm.Show(this);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while opening direct data-access tool");
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                var newForm = new IBESettingsView();
                newForm.Show(this);

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while opening direct data-access tool");
            }
        }

        private void eDDNInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var newForm = new EDDN.EDDNView(Program.EDDNComm);
                newForm.Show(this);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while opening EDDN interface");
            }
        }

        private void companionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var newForm = new IBECompanion.CompanioDataView();
                newForm.ShowDialog(this);

                if((Program.CompanionIO.CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok))
                {
                    // set the commanders name if not already set
                    if ((Program.DBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Identification", "useUserName") == "useUserID") && 
                        String.IsNullOrEmpty(Program.DBCon.getIniValue<String>(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserName")))
                    {
                        String userName = (String)Program.CompanionIO.GetData().SelectToken("commander.name", false) ?? "";

                        if(!String.IsNullOrEmpty(userName))
                        {
                            Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserName", userName);
                            Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Identification", "useUserName");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while opening Companion interface");
            }
        }

        private void cmdEventLanded_Click(object sender, EventArgs e)
        {
            String extSystem    = "";
            String extStation   = "";
            DialogResult MBResult;

            try
            {
                if(Program.CompanionIO.CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok)
                { 
                    Cursor = Cursors.WaitCursor;

                    MBResult = System.Windows.Forms.DialogResult.OK;

                    txtEventInfo.Text = "checking for current station...";
                    txtEventInfo.Refresh();

                    // allow refresh of companion data
                    Program.CompanionIO.GetProfileData(false);

                    if(Program.CompanionIO.IsLanded())
                    {
                        extSystem  = Program.CompanionIO.GetValue("lastSystem.name");
                        extStation = Program.CompanionIO.GetValue("lastStarport.name");

                        if(!Program.actualCondition.System.Equals(extSystem))
                        {
                            MBResult = MessageBox.Show(this, "The external recieved system does not correspond to the system from the logfile!\n" +
                                                                                    "Confirm even so ?", "Unexpected system retrieved !",
                                                                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        }

                        if(MBResult == System.Windows.Forms.DialogResult.OK)
                        {
                            Program.CompanionIO.ConfirmLocation(extSystem, extStation);
                            txtEventInfo.Text             = String.Format("landed on '{1}' in '{0}'", extSystem, extStation);                        

                            if(Program.CompanionIO.StationHasShipyardData())
                            {
                                Program.EDDNComm.SendShipyardData(Program.CompanionIO.GetData());
                            }
                            else if(Program.DBCon.Execute<Boolean>("select has_shipyard from tbStations where id = " + Program.actualCondition.Location_ID))
                            {
                                // probably companion error, try once again in 5 seconds
                                Program.CompanionIO.ReGet_StationData();                                
                            }

                            if(Program.CompanionIO.StationHasOutfittingData())
                                Program.EDDNComm.SendOutfittingData(Program.CompanionIO.GetData());
                            
                        }
                        else
                        {
                            txtEventInfo.Text             = String.Format("location '{1}' in '{0}' not confirmed !", extSystem, extStation);                        
                        }
                    }
                    else
                    { 
                        txtEventInfo.Text             = "You're not docked";                        
                    }

                    Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show(this, "Can't comply, companion interface not ready !", "Companion IO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtEventInfo.Text = "Can't comply, companion interface not ready !";                        
                }

                ShowStatus();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdEventLanded_Click");
            }
        }

        void CompanionIO_AsyncDataRecievedEvent(object sender, EventArgs e)
        {
            try
            {
                if(Program.CompanionIO.StationHasShipyardData())
                    Program.EDDNComm.SendShipyardData(Program.CompanionIO.GetData());

                ShowStatus();
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in CompanionIO_AsyncDataRecievedEvent");
            }    
        }
        private void EDSMComm_DataTransmittedEvent(object sender, EventArgs e)
        {
            try
            {
                if(this.InvokeRequired)
                    this.Invoke(new EventDelegate(EDSMComm_DataTransmittedEvent), sender, e);
                else
                {
                    Int32 inQueue = ((EDSM.EDStarmapInterface.DataTransmittedEventArgs)e).InQueue;

                    tsEDSMQueue.Text = "EDSM messages in send-queue : " + inQueue;
                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in EDSMComm_DataTransmittedEvent");
            }    
        }

        
        /// <summary>
        /// shows location data like system/station/ccordinates on gui
        /// </summary>
        private void ShowLocationData()
        {
            try
            {
                if(this.InvokeRequired)
                    this.Invoke(new MethodInvoker(ShowLocationData));
                else
                {
                    Point3Dbl coords        = new Point3Dbl();
                    String currentSystem    = Program.actualCondition.System;
                    String currentLocation  = Program.actualCondition.Location;

                    this.tbCurrentSystemFromLogs.Text       = currentSystem;
                    this.tbCurrentStationinfoFromLogs.Text  = currentLocation;

                    coords = Program.actualCondition.Coordinates;
                    if(coords.Valid)
                    {
                        txtPosition_X.Text = coords.X.Value.ToString("f3");
                        txtPosition_Y.Text = coords.Y.Value.ToString("f3");
                        txtPosition_Z.Text = coords.Z.Value.ToString("f3");
                    }
                    else
                    {
                        txtPosition_X.Text = "n/a";
                        txtPosition_Y.Text = "n/a";
                        txtPosition_Z.Text = "n/a";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while showing location infos", ex);
            }
        }

        private delegate void EventDelegate(Object sender, EventArgs e);


        private void eDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var newForm = new EDSM.EDStarmapInterfaceView(Program.EDSMComm);
                newForm.Show(this);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while opening EDSM interface");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// update the status information of companion io
        /// </summary>
        private void ShowStatus()
        {
            try
            {
                if(this.InvokeRequired)
                    this.Invoke(new MethodInvoker(ShowStatus));
                else
                {
                    if((Program.CompanionIO.CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok) && Program.CompanionIO.IsLanded())
                    {
                        int? stationID = Program.actualCondition.Location_ID;

                        if(stationID == null)
                        {
                            var extSystem  = Program.CompanionIO.GetValue("lastSystem.name");
                            var extStation = Program.CompanionIO.GetValue("lastStarport.name");

                            String sqlString = String.Format("select St.id from tbSystems Sy, tbStations St" +
                                                             " where Sy.id = St.System_id" +
                                                             " and   Sy.systemname = {0}" +
                                                             " and   St.stationname = {1}",
                                                             DBConnector.SQLAEscape(extSystem), 
                                                             DBConnector.SQLAEscape(extStation));

                            stationID = Program.DBCon.Execute<int?>(sqlString);
                        }

                        pbStatus_IsLanded.Image = Properties.Resources.ledgreen_on;

                        if(Program.CompanionIO.StationHasMarketData())
                            pbStatus_MarketData.Image       = Properties.Resources.ledorange_on;
                        else if((stationID != null) && (Program.DBCon.Execute<Boolean>("select has_market from tbStations where id = " + stationID)))
                            pbStatus_MarketData.Image       = Properties.Resources.ledred_on;
                        else
                            pbStatus_MarketData.Image       = Properties.Resources.ledorange_off;

                        if(Program.CompanionIO.StationHasOutfittingData())
                            pbStatus_OutfittingData.Image       = Properties.Resources.ledorange_on;
                        else if((stationID != null) && (Program.DBCon.Execute<Boolean>("select has_outfitting from tbStations where id = " + stationID)))
                            pbStatus_OutfittingData.Image       = Properties.Resources.ledred_on;
                        else
                            pbStatus_OutfittingData.Image       = Properties.Resources.ledorange_off;

                        if(Program.CompanionIO.StationHasShipyardData())
                            pbStatus_ShipyardData.Image       = Properties.Resources.ledorange_on;
                        else if((stationID != null) && (Program.DBCon.Execute<Boolean>("select has_shipyard from tbStations where id = " + stationID)))
                            pbStatus_ShipyardData.Image       = Properties.Resources.ledred_on;
                        else
                            pbStatus_ShipyardData.Image       = Properties.Resources.ledorange_off;

                        if(Program.EDDNComm.CommodityDataTransmitted == EDDN.EDDNCommunicator.SendingState.Send)
                            pbStatus_MarketDataEDDN.Image       = Properties.Resources.ledorange_on;
                        else if(Program.EDDNComm.CommodityDataTransmitted == EDDN.EDDNCommunicator.SendingState.Error)
                            pbStatus_MarketDataEDDN.Image       = Properties.Resources.ledred_on;
                        else
                            pbStatus_MarketDataEDDN.Image       = Properties.Resources.ledorange_off;
                        
                        if(Program.EDDNComm.OutfittingDataTransmitted == EDDN.EDDNCommunicator.SendingState.Send)
                            pbStatus_OutfittingDataEDDN.Image       = Properties.Resources.ledorange_on;
                        else if(Program.EDDNComm.OutfittingDataTransmitted == EDDN.EDDNCommunicator.SendingState.Error)
                            pbStatus_OutfittingDataEDDN.Image       = Properties.Resources.ledred_on;
                        else
                            pbStatus_OutfittingDataEDDN.Image       = Properties.Resources.ledorange_off;

                        if(Program.EDDNComm.ShipyardDataTransmitted == EDDN.EDDNCommunicator.SendingState.Send)
                            pbStatus_ShipyardDataEDDN.Image       = Properties.Resources.ledorange_on;
                        else if(Program.EDDNComm.ShipyardDataTransmitted == EDDN.EDDNCommunicator.SendingState.Error)
                            pbStatus_ShipyardDataEDDN.Image       = Properties.Resources.ledred_on;
                        else
                            pbStatus_ShipyardDataEDDN.Image       = Properties.Resources.ledorange_off;

                        // can only collect market data if landed andn confirmed
                        cmdEventMarketData.Enabled = (Program.actualCondition.Location_ID != null);
                    }
                    else if((Program.CompanionIO.CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok))
                    { 
                        pbStatus_IsLanded.Image             = Properties.Resources.ledgreen_off;
                        pbStatus_MarketData.Image           = Properties.Resources.ledorange_off;
                        pbStatus_OutfittingData.Image       = Properties.Resources.ledorange_off;
                        pbStatus_ShipyardData.Image         = Properties.Resources.ledorange_off;

                        pbStatus_MarketDataEDDN.Image       = Properties.Resources.ledorange_off;
                        pbStatus_OutfittingDataEDDN.Image   = Properties.Resources.ledorange_off;
                        pbStatus_ShipyardDataEDDN.Image     = Properties.Resources.ledorange_off;

                        cmdEventMarketData.Enabled      = false;
                    }
                    else
                    { 
                        pbStatus_IsLanded.Image             = Properties.Resources.ledred_on;
                        pbStatus_MarketData.Image           = Properties.Resources.ledred_on;
                        pbStatus_OutfittingData.Image       = Properties.Resources.ledred_on;
                        pbStatus_ShipyardData.Image         = Properties.Resources.ledred_on;

                        pbStatus_MarketDataEDDN.Image       = Properties.Resources.ledorange_off;
                        pbStatus_OutfittingDataEDDN.Image   = Properties.Resources.ledorange_off;
                        pbStatus_ShipyardDataEDDN.Image     = Properties.Resources.ledorange_off;

                        txtEventInfo.Text                   = "No data recieved, servers may in maintenance mode !";

                        cmdEventMarketData.Enabled      = false;
                    }
                }
            }
        catch (Exception ex)
        {
            throw new Exception("Error while showing status infos", ex);
        }
    }

        private void cmdEventMarketData_Click(object sender, EventArgs e)
        {
            try
            {
                if(Program.CompanionIO.CompanionStatus == EDCompanionAPI.Models.LoginStatus.Ok)
                { 
                    txtEventInfo.Text             = "getting market data...";                        

                    if(Program.CompanionIO.StationHasMarketData())
                    { 
                        Int32 count = Program.CompanionIO.ImportMarketData();

                        if(cbEDDNOverride.Checked)
                        {
                            Program.EDDNComm.SendCommodityData(Program.CompanionIO.GetData());
                        }

                        if(count > 0)
                            txtEventInfo.Text             = String.Format("getting market data...{0} prices collected", count);                        
                        else
                            txtEventInfo.Text             = String.Format("getting market data...no market data available !");                        
                    }
                    else
                        txtEventInfo.Text             = String.Format("...no market data available !");                        
                    
                }
                else
                {
                    MessageBox.Show(this, "Can't comply, companion interface not ready !", "Companion IO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtEventInfo.Text = "Can't comply, companion interface not ready !";                        
                }

                SetQuickDecisionSwitch();

                ShowStatus();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in cmdEventMarketData_Click");
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan rest = Program.CompanionIO.RestTime();

                txtRestTime.Text = rest.TotalSeconds.ToString("00");
            }
            catch (Exception)
            {
            }

        }

        private void commodityMappingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var mappingForm = new CommodityMappingsView();

                mappingForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in commodityMappingsToolStripMenuItem_Click");
            }
        }

        private void colorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var ToolForm = new GUIColorsView();

                ToolForm.ShowDialog(this);

                if(ToolForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Retheme();
                }
                
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error in colorsToolStripMenuItem_Click");
            }
        }
    }
}
