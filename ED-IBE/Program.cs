using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using IBE.EDDB_Data;
using IBE.Web;
using IBE.SQL;
using IBE.MTCommandersLog;
using IBE.MTPriceAnalysis;
using IBE.FileScanner;
using IBE.Enums_and_Utility_Classes;
using EDCompanionAPI;

#if useVNC 
using NVNC;
#endif

namespace IBE
{

    public static class Program
    {
        public static bool showToDo;

        public const String GIT_PATH                = @"https://github.com/Duke-Jones/ED-IBE";

        public const String NULLSTRING              = "?";
        public const String COMMODITY_NOT_SET       = "???";
        public const String BASE_LANGUAGE           = "eng";

#region enums

        public enum enVisitedFilter
        {
            showAll                 = 0,
            showOnlyVistedSystems   = 1,
            showOnlyVistedStations  = 2
        }

#endregion


    #region main object creation and disposing

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
#if ShowToDo
    showToDo = true;    
#else
    showToDo = false;
#endif

                bool blnOK;
                Cursor.Current = Cursors.WaitCursor;

                using (System.Threading.Mutex mut = new System.Threading.Mutex(true, "Anwendungsname", out blnOK))
                {
                    if (blnOK)
                    {
                        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                        Application.ThreadException += Application_ThreadException;

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);


                        Init();

                        Application.Run(new Form1());

                        Cleanup();
                    }
                    else
                    {
                        MessageBox.Show("A instance of ED-IBE is already running!", "Aborted !", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                CErr.processError(ex, "Error in main routine !");
            }
        }

    #endregion

    #region Exception Handling

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)(e.ExceptionObject));
        }


        private static void HandleException(Exception ex)
        {
            String FileName = String.Format("ed-ibe-dump-v{0}.dmp", VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3)).Replace(".", "_");
            if (ex == null)
                return;

            if(!Program.SplashScreen.IsDisposed)
                Program.SplashScreen.TopMost = false;

            CErr.processError(ex, "Unhandled Exception");
            // ExceptionPolicy.HandleException(ex, "Default Policy");
            CreateMiniDump(FileName);
            MessageBox.Show("Fatal error.\r\n\r\nA dump file (\"" + FileName + "\" has been created in your data directory.  \r\n\r\nPlease place this in a file-sharing service such as SendSpace, Google Drive or Dropbox, then link to the file in the Frontier forums or on the GitHub archive or send e mail to Duke.Jones@gmx.de.  This will allow the developers to fix this problem.  \r\n\r\nThanks, and sorry about the crash...");
            Application.Exit();
        }

        // From http://brakertech.com/howto-c-generate-dump-file-on-crash/
        public class MINIDUMP_TYPE
        {
            public const int MiniDumpNormal = 0x00000000;
            public const int MiniDumpWithDataSegs = 0x00000001;
            public const int MiniDumpWithFullMemory = 0x00000002;
            public const int MiniDumpWithHandleData = 0x00000004;
            public const int MiniDumpFilterMemory = 0x00000008;
            public const int MiniDumpScanMemory = 0x00000010;
            public const int MiniDumpWithUnloadedModules = 0x00000020;
            public const int MiniDumpWithIndirectlyReferencedMemory = 0x00000040;
            public const int MiniDumpFilterModulePaths = 0x00000080;
            public const int MiniDumpWithProcessThreadData = 0x00000100;
            public const int MiniDumpWithPrivateReadWriteMemory = 0x00000200;
            public const int MiniDumpWithoutOptionalData = 0x00000400;
            public const int MiniDumpWithFullMemoryInfo = 0x00000800;
            public const int MiniDumpWithThreadInfo = 0x00001000;
            public const int MiniDumpWithCodeSegs = 0x00002000;
        }

        [DllImport("dbghelp.dll")]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess,
                                                    Int32 ProcessId,
                                                    IntPtr hFile,
                                                    int DumpType,
                                                    IntPtr ExceptionParam,
                                                    IntPtr UserStreamParam,
                                                    IntPtr CallackParam);

        public static void CreateMiniDump(string Filename)
        {

            

            using (FileStream fs = new FileStream(GetDataPath(Filename), FileMode.Create))
            {
                using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
                {
                    MiniDumpWriteDump(process.Handle,
                                                     process.Id,
                                                     fs.SafeFileHandle.DangerousGetHandle(),
                                                     MINIDUMP_TYPE.MiniDumpWithFullMemory,
                                                     IntPtr.Zero,
                                                     IntPtr.Zero,
                                                     IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// Gets a path where data (logs/dumps/other data) can be saved.
        /// Default is the program path (development) or the data path which is by default "{localappdata}\ED-IBE\"
        /// </summary>
        /// <param name="subPath">subpath to be added (optional)</param>
        /// <returns></returns>
        public static String GetDataPath(String subPath = "")
        {
            String path;
            try
            {
                try
                {
                    subPath = subPath.Replace("\"", "");

                    if (System.Diagnostics.Debugger.IsAttached)
                        path = ".";
                    else
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            // special to find on x64-systems the value while debugging
                            using (var hklm = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64))
                            {   // key now points to the 64-bit key
                                using (var myKey = hklm.OpenSubKey(@"SOFTWARE\ED-IBE", false))
                                {
                                    path = (String)myKey.GetValue("Data").ToString().Trim().Replace("\"", "");
                                }
                            }

                            if (String.IsNullOrEmpty(path))
                                path = ".";
                        }
                        else
                        {
                            Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ED-IBE", false);

                            path = (String)myKey.GetValue("Data").ToString().Replace("\"", "");
                        }
                    }
                }
                catch (Exception)
                {
                    path = ".";
                }

                if(!String.IsNullOrEmpty(subPath))
                    path = Path.Combine(path, subPath);

                Boolean isFile = Path.GetFileName(path).Contains('.');
                String fullPath;

                if(isFile)
                    fullPath = Path.GetDirectoryName(Path.GetFullPath(path));
                else
                    fullPath = Path.GetFullPath(path);

                if(!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                return Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving a data path", ex);
            }
        }

    #endregion// Exception Handling

    #region global objects

        public static SingleThreadLogger                    MainLog = new SingleThreadLogger(ThreadLoggerType.App);

        private static Boolean                              m_initDone                  = false;

        public static GUIColors                             Colors;
        public static IBECompanion.CompanionData            CompanionIO;
//        public static ExternalDataInterface                 ExternalData;
        public static DBConnector                           DBCon = null;
        public static STA.Settings.INIFile                  IniFile;
        private static DBProcess                            EliteDBProcess;
        public static CommandersLog                         CommandersLog;
        public static PriceAnalysis                         PriceAnalysis;
        public static EliteDBIO                             Data;
        public static Condition                             actualCondition;
        public static EDJournalScanner                      JournalScanner;
        public static SplashScreenForm                      SplashScreen;
        public static EDDN.EDDNCommunicator                 EDDNComm;
        public static EDSM.EDStarmapInterface               EDSMComm;
        public static PlausibiltyChecker                    PlausibiltyCheck;
        public static GameSettings                          GameSettings;
#if useVNC 
        public static VncServer                             VNCAppServer;
#endif
        public static System.Threading.Thread               VNCServerThread;

        /// <summary>
        /// starts the initialization of the global objects
        /// </summary>
        public static void Init()
        {

            try
            {
                if(!m_initDone)
                { 
                    Program.SplashScreen = new SplashScreenForm();
                    Program.SplashScreen.Show();

                    Program.SplashScreen.Logger = MainLog;

                    Program.SplashScreen.InfoAdd("initializing logger...");
                    Program.MainLog.Log("Initialising...\n");
                    Program.SplashScreen.InfoAppendLast("<OK>");

                    Program.SplashScreen.InfoAdd("starting sql server...");

                    // load settings from file
                    IniFile = new STA.Settings.INIFile(GetDataPath("ED-IBE.ini"), false, true);


                    // prepare architecture-dependend files
                    PrepareDepFiles();

                    // start database process (if not running)
                    DBProcess.DBProcessParams newProcessParams  = new DBProcess.DBProcessParams() { };
                    newProcessParams.Commandline                = IniFile.GetValue("DB_Server",         "Commandline",      @"bin\mysqld.exe");    
                    newProcessParams.Commandargs                = IniFile.GetValue("DB_Server",         "CommandArgs",      @"--defaults-file=Elite.ini --console");
                    newProcessParams.Workingdirectory           = IniFile.GetValue("DB_Server",         "WorkingDirectory", @"..\..\..\RNDatabase\Database");
                    newProcessParams.Port                       = IniFile.GetValue<UInt16>("DB_Server", "Port",             "3306");
                    newProcessParams.DBStartTimeout             = IniFile.GetValue<Int16>("DB_Server",  "DBStartTimeout",   "60");

                    Program.SplashScreen.InfoAppendLast("on port " + newProcessParams.Port + "...");

                    EliteDBProcess                              = new DBProcess(newProcessParams);

                    if (EliteDBProcess.WasRunning)
                        Program.SplashScreen.InfoAppendLast("already running...<OK>"); 
                    else
                        Program.SplashScreen.InfoAppendLast("<OK>");
                        


                    // connecT to the database
                    Program.SplashScreen.InfoAdd("connect to sql server...");

                    DBConnector.ConnectionParams newConnectionParams = new DBConnector.ConnectionParams() { };

                    newConnectionParams.Name                    = IniFile.GetValue("DB_Connection",          "Name",           "master");   
                    newConnectionParams.Server                  = IniFile.GetValue("DB_Connection",          "Server",         "localhost");
                    newConnectionParams.Port                    = IniFile.GetValue<UInt16>("DB_Server",      "Port",           "3306");
                    newConnectionParams.Database                = IniFile.GetValue("DB_Connection",          "Database",       "Elite_DB"); 
                    newConnectionParams.User                    = IniFile.GetValue("DB_Connection",          "User",           "RN_User");  
                    newConnectionParams.Pass                    = IniFile.GetValue("DB_Connection",          "Pass",           "Elite");    
                    newConnectionParams.ConnectTimeout          = IniFile.GetValue<Int16>("DB_Connection",   "ConnectTimeout", "60");   
                    newConnectionParams.StayAlive               = IniFile.GetValue<Boolean>("DB_Connection", "StayAlive",      "false");    
                    newConnectionParams.TimeOut                 = IniFile.GetValue<Int16>("DB_Connection",   "TimeOut",        "10000");

                    DBCon                                       = new DBConnector(newConnectionParams);

                    DBCon.Connect();

                    Program.SplashScreen.InfoAppendLast("<OK>");

                    /* **************** database is running ********************** */
                    
                    /* perform updates */
                    Updater.DBUpdate();

                    Program.SplashScreen.InfoAdd("preparing global objects...");

                    // prepare colors-object
                    Colors                                      = new GUIColors();

                    // preprare main data object
                    Data                                        = new IBE.SQL.EliteDBIO();
                    Data.PrepareBaseTables();

                    // create global paths-object
                    //Paths                                       = new ProgramPaths();

                    // prepare settings
//                    Settings                                    = new Settings();
//                    Settings.BaseData                           = Data.BaseData;

                    // prepare commanders log 
                    CommandersLog                               = new CommandersLog();
                    CommandersLog.BaseData                      = Data.BaseData;

                    // prepare price analysis 
                    PriceAnalysis                               = new PriceAnalysis(new DBConnector(DBCon.ConfigData, true));
                    PriceAnalysis.BaseData                      = Data.BaseData;

                    //// starting the external data interface
                    //ExternalData                                = new ExternalDataInterface();

                    // Companion IO
                    CompanionIO = new IBECompanion.CompanionData(Program.GetDataPath());
                    if(CompanionIO.ConditionalLogIn())
                        CompanionIO.GetProfileDataAsync();

                    // initializing the object for the actual condition
                    actualCondition                             = new Condition();

                    // initializing the LogfileScanner
                    //LogfileScanner                              = new EDLogfileScanner();
                    JournalScanner                              = new EDJournalScanner();
                    JournalScanner.Start();

                    // EDDN Interface
                    EDDNComm = new IBE.EDDN.EDDNCommunicator();

                    // EDSMComm Interface
                    EDSMComm = new IBE.EDSM.EDStarmapInterface(Program.DBCon);

                    // forwards a potentially new system or station information to database
                    Program.JournalScanner.JournalEventRecieved += JournalEventRecieved;
                    Program.CompanionIO.LocationInfo            += ExternalData_LocationInfo;

                    // register the LogfileScanner in the CommandersLog for the DataSavedEvent-event
                    CommandersLog.registerLogFileScanner(JournalScanner);
                    CommandersLog.registerExternalTool(CompanionIO);
                    
                    PriceAnalysis.registerLogFileScanner(JournalScanner);
                    PriceAnalysis.registerExternalTool(CompanionIO);

                    EDSMComm.registerLogFileScanner(JournalScanner);

                    // Plausibility-Checker
                    PlausibiltyCheck = new PlausibiltyChecker();

                    /// early variant of DoSpecial();
                    Updater.DoSpecial_Early();

                    Program.SplashScreen.InfoAppendLast("<OK>");

                    m_initDone = true;

                }
            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while initializing the program object");
            }

        }

        public static void Cleanup()
        {
            try
            {
                if(EDDNComm != null)
                {
                    EDDNComm.Dispose();
                    EDDNComm = null;
                }

                if(JournalScanner != null)
                {
                    JournalScanner.Dispose();
                    JournalScanner = null;
                }

                if(DBCon != null)
                { 
                    DBCon.Dispose();
                    DBCon = null;
                }

                // if EliteDBProcess is not null the process is created 
                // by this program, so we also have to do the cleanup
                if((EliteDBProcess != null) && (!EliteDBProcess.WasRunning))
                { 
                    String user = IniFile.GetValue("DB_Connection", "User", "RN_User");  
                    String pass = IniFile.GetValue("DB_Connection", "Pass", "Elite");    

                    EliteDBProcess.StopServer(user, pass);
                    EliteDBProcess.Dispose();
                    EliteDBProcess = null;
                }

#if useVNC 
                if(VNCAppServer != null)
                    VNCAppServer.Stop();
#endif

                //VNCServerThread = new System.Threading.Thread(new System.Threading.ThreadStart(s.Start));

            }
            catch (Exception ex)
            {
                throw new Exception("Error while cleaning up", ex);
            }

        }

        /// <summary>
        /// forwards a potentially new system or station information to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void JournalEventRecieved(object sender, FileScanner.EDJournalScanner.JournalEventArgs e)
        {
            try
            {
                if(e.EventType == FileScanner.EDJournalScanner.JournalEvent.FSDJump) 
                    Data.checkPotentiallyNewSystemOrStation(e.Data.Value<String>("StarSystem"), "", new Point3Dbl(e.Data.Value<Double>("StarPos[0]"), e.Data.Value<Double>("StarPos[1]"), e.Data.Value<Double>("StarPos[2]")), false);

            }
            catch (Exception ex)
            {
                throw new Exception("Error in JournalEventRecieved-event", ex); 
            }
        }

        /// <summary>
        /// forwards a potentially new system or station information to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ExternalData_LocationInfo(object sender, IBE.IBECompanion.DataEventBase.LocationInfoEventArgs e)
        {                                                       
            try
            {
                Data.checkPotentiallyNewSystemOrStation(e.System, e.Location, null, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while in ExternalData_LocationInfo", ex); 
            }
        }

        private static void PrepareDepFiles()
        {
            String mainPath;
            String sourceName;
            String destName;
            String dllDir;
            String archDir;

            try
            {

                if (Debugger.IsAttached)
                {
                    // For debugging in x86 and x64 mode.
                    // Release use zeromq.dll (x64) from main path .
                    mainPath = System.Windows.Forms.Application.StartupPath;

                    if (Environment.Is64BitProcess)
                        archDir = "x64";
                    else
                        archDir = "x86";
                
                    dllDir      = "zeromq";
                    sourceName  = "libzmq*.dll";
                    destName    = "libzmq.dll";

                    dllDir = System.IO.Path.Combine(mainPath, Path.Combine(dllDir, archDir));

                    string[] files = Directory.GetFiles(dllDir, sourceName, SearchOption.TopDirectoryOnly);

                    if(File.Exists(Path.Combine(mainPath, destName)))
                        File.Delete(Path.Combine(mainPath, destName));

                    File.Copy(files[0], Path.Combine(mainPath, destName));
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while preparing dep-files", ex);
            }
        }


    #endregion //global objects



        internal static void StartVNCServer(Form form1)
        {
            try
            {

#if useVNC 

                if(Program.DBCon.getIniValue<Boolean>("Settings", "ActivateVNC", false.ToString(), false))
                {
                    VNCAppServer = new NVNC.VncServer("", "", 5901, 5900, "ED-IBE Remote", form1);

                    VNCServerThread = new System.Threading.Thread(new System.Threading.ThreadStart(VNCAppServer.Start));
                    VNCServerThread.Start();
                }

#endif
            }
            catch (Exception ex)
            {
                Program.DBCon.setIniValue("Settings", "ActivateVNC", false.ToString());
                throw new Exception("Error while starting VNC server", ex);
            }
            
        }
    }
}
