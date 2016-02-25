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
using IBE.MTSettings;
using IBE.ExtData;
using IBE.FileScanner;
using IBE.Enums_and_Utility_Classes;

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
                cErr.processError(ex, "Error in main routine !");
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
            if (ex == null)
                return;
            cErr.processError(ex, "Unhandled Exception", true);
            // ExceptionPolicy.HandleException(ex, "Default Policy");
            CreateMiniDump("RegulatedNoiseDump.dmp");
            MessageBox.Show("Fatal error.\r\n\r\nA dump file (\"RegulatedNoiseDump.dmp\" has been created in your RegulatedNoise directory.  \r\n\r\nPlease place this in a file-sharing service such as Google Drive or Dropbox, then link to the file in the Frontier forums or on the GitHub archive.  This will allow the developers to fix this problem.  \r\n\r\nThanks, and sorry about the crash...");
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
                                path = (String)myKey.GetValue("Data").ToString().Trim();
                            }
                        }

                        if (String.IsNullOrEmpty(path))
                            path = ".";
                    }
                    else
                    {
                        Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ED-IBE", false);

                        path = (String)myKey.GetValue("Data");
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

    #endregion// Exception Handling

    #region global objects

        private static Boolean                          m_initDone                  = false;

//        public static ProgramPaths                      Paths;
        public static GUIColors                         Colors;
        public static CompanionInterface                CompanionIO;
        public static ExternalDataInterface             ExternalData;
        public static DBConnector                       DBCon;
        //public static RegulatedNoiseSettings            Settings_old;
        public static STA.Settings.INIFile              IniFile;
        private static DBProcess                        EliteDBProcess;
        public static Settings                          Settings;
        public static CommandersLog                     CommandersLog;
        public static PriceAnalysis                     PriceAnalysis;
        public static EliteDBIO                         Data;
        public static Condition                         actualCondition;
        public static EDLogfileScanner                  LogfileScanner;
        public static SplashScreenForm                  SplashScreen;
        public static SingleThreadLogger                Logger;


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

                    Program.SplashScreen.InfoAdd("initializing logger...");
                    Program.Logger = new SingleThreadLogger(ThreadLoggerType.Form);
                    Program.Logger.Log("Initialising...\n");
                    Program.SplashScreen.InfoAppendLast("<OK>");

                    Program.SplashScreen.InfoAdd("starting sql server...");

                    // load settings from file
                    IniFile = new STA.Settings.INIFile(GetDataPath("ED-IBE.ini"), false, true);

                    // starT database process (if not running)
                    DBProcess.DBProcessParams newProcessParams  = new DBProcess.DBProcessParams() { };
                    newProcessParams.Commandline                = IniFile.GetValue("DB_Server",        "Commandline",      @"bin\mysqld.exe");    
                    newProcessParams.Commandargs                = IniFile.GetValue("DB_Server",        "CommandArgs",      @"--defaults-file=Elite.ini --console");
                    newProcessParams.Workingdirectory           = IniFile.GetValue("DB_Server",        "WorkingDirectory", @"..\..\..\RNDatabase\Database");
                    newProcessParams.Port                       = IniFile.GetValue<Int16>("DB_Server", "Port",             "3306");
                    newProcessParams.DBStartTimeout             = IniFile.GetValue<Int16>("DB_Server", "DBStartTimeout",   "60");
                
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
                    newConnectionParams.Database                = IniFile.GetValue("DB_Connection",          "Database",       "Elite_DB"); 
                    newConnectionParams.User                    = IniFile.GetValue("DB_Connection",          "User",           "RN_User");  
                    newConnectionParams.Pass                    = IniFile.GetValue("DB_Connection",          "Pass",           "Elite");    
                    newConnectionParams.ConnectTimeout          = IniFile.GetValue<Int16>("DB_Connection",   "ConnectTimeout", "60");   
                    newConnectionParams.StayAlive               = IniFile.GetValue<Boolean>("DB_Connection", "StayAlive",      "false");    
                    newConnectionParams.TimeOut                 = IniFile.GetValue<Int16>("DB_Connection",   "TimeOut",        "60");

                    DBCon                                       = new DBConnector(newConnectionParams);

                    DBCon.Connect();

                    Program.SplashScreen.InfoAppendLast("<OK>");

                    /* **************** database is running ********************** */
                    
                    Program.SplashScreen.InfoAdd("preparing global objects...");

                    // prepare colors-object
                    Colors                                      = new GUIColors();

                    // preprare main data object
                    Data                                        = new IBE.SQL.EliteDBIO();
                    Data.PrepareBaseTables();

                    // create global paths-object
                    //Paths                                       = new ProgramPaths();

                    // prepare settings
                    Settings                                    = new Settings();
                    Settings.BaseData                           = Data.BaseData;

                    // prepare commanders log 
                    CommandersLog                               = new CommandersLog();
                    CommandersLog.BaseData                      = Data.BaseData;

                    // prepare price analysis 
                    PriceAnalysis                               = new PriceAnalysis(new DBConnector(DBCon.ConfigData, true));
                    PriceAnalysis.BaseData                      = Data.BaseData;

                    // starting the external data interface
                    ExternalData                                = new ExternalDataInterface();

                    // initializing the object for the actual condition
                    actualCondition                             = new Condition();

                    // initializing the LogfileScanner
                    LogfileScanner                              = new EDLogfileScanner();


                    // forwards a potentially new system or station information to database
                    Program.LogfileScanner.LocationInfo += LogfileScanner_LocationInfo;
                    Program.ExternalData.LocationInfo   += ExternalData_LocationInfo;

                    // register the LogfileScanner in the CommandersLog for the ExternalDataEvent-event
                    CommandersLog.registerLogFileScanner(LogfileScanner);
                    CommandersLog.registerExternalTool(ExternalData);

                    PriceAnalysis.registerLogFileScanner(LogfileScanner);
                    PriceAnalysis.registerExternalTool(ExternalData);

                    Program.SplashScreen.InfoAppendLast("<OK>");

                    m_initDone = true;

                }
            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while initializing the program object", true);
            }

        }

        public static void Cleanup()
        {
            try
            {
                if(LogfileScanner != null)
                {
                    LogfileScanner.Dispose();
                    LogfileScanner = null;
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
        static void LogfileScanner_LocationInfo(object sender, EDLogfileScanner.LocationInfoEventArgs e)
        {
            try
            {
                Data.checkPotentiallyNewSystemOrStation(e.System, e.Location, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while in LogfileScanner_LocationInfo", ex); 
            }
        }

        /// <summary>
        /// forwards a potentially new system or station information to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ExternalData_LocationInfo(object sender, ExternalDataInterface.LocationInfoEventArgs e)
        {
            try
            {
                Data.checkPotentiallyNewSystemOrStation(e.System, e.Location, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while in ExternalData_LocationInfo", ex); 
            }
        }

        /// <summary>
        /// this sub starts special things to do if this version runs
        /// for the first time
        /// </summary>
        internal static void DoSpecial()
        {
            Version dbVersion;
            Version appVersion;
            Version testVersion = new Version();

            try
            {
                dbVersion   = Program.DBCon.getIniValue<Version>("Database", "Version", new Version(0,0,0,0).ToString(), false);
                appVersion  = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                if (dbVersion < appVersion)
                {
                    if (dbVersion < (testVersion = new Version(0,1,0)))
                    {
                        // here it's required to import all master data 
                        var DataIO = new frmDataIO();

                        Program.SplashScreen.InfoAdd("Importing master data...");
                        Thread.Sleep(3000);

                        DataIO.InfoTarget = Program.SplashScreen.SplashInfo;
                        DataIO.ReUseLine  = true;

                        DataIO.StartMasterImport(GetDataPath("Data"));

                        DataIO.Close();
                        DataIO.Dispose();
                        

                        Program.DBCon.setIniValue("Database", "Version", appVersion.ToString());
                    }

                    Program.DBCon.setIniValue("Database", "Version", appVersion.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while doing special things", ex);
            }
        }

    #endregion //global objects


    }
}
