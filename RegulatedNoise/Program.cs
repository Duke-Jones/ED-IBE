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
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Web;
using RegulatedNoise.SQL;
using RegulatedNoise.MTCommandersLog;
using RegulatedNoise.MTPriceAnalysis;
using RegulatedNoise.MTSettings;
using RegulatedNoise.ExtData;
using RegulatedNoise.FileScanner;


namespace RegulatedNoise
{
    public static class Program
    {

        public const Decimal DB_VERSION_CURRENT     = 1.0M;
        public const Decimal DB_VERSION_NONE        = 0.0M; 
        

        public const String NULLSTRING              = "?";
        public const String COMMODITY_NOT_SET       = "???";
        public const String BASE_LANGUAGE           = "eng";

        //public String Directory_Data        { get; set; }
        //public String Directory_Program     { get; set; }

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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Init();

            Application.Run(new Form1());

            Cleanup();
            try
            {

              

            }
            catch (Exception ex)
            {
                cErr.showError(ex, "Error in main routine !");
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
        /// Gets a path where logs and/or dumps can be saved.
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
                    Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ED-IBE", false);

                    path = (String)myKey.GetValue("Data");

                    if (String.IsNullOrEmpty(path))
                        path = ".";
                }

                if(!String.IsNullOrEmpty(subPath))
                    path = Path.Combine(path, subPath);
            }
            catch (Exception)
            {
                path = ".";
            }

            return path;
        }
    #endregion// Exception Handling

    #region global objects

        private static Boolean                          m_initDone                  = false;

        public static ProgramPaths                      Paths;
        public static GUIColors                         Colors;
        public static CompanionInterface                CompanionIO;
        public static ExternalDataInterface             ExternalData;
        public static DBConnector                       DBCon;
        public static RegulatedNoiseSettings            Settings_old;
        public static STA.Settings.INIFile              IniFile;
        private static DBProcess                        EliteDBProcess;
        public static Settings                          Settings;
        public static CommandersLog                     CommandersLog;
        public static PriceAnalysis                     PriceAnalysis;
        public static EliteDBIO                         Data;
        public static Condition                         actualCondition;
        public static EDLogfileScanner                  LogfileScanner;


        /// <summary>
        /// starts the initialization of the global objects
        /// </summary>
        public static void Init()
        {

            try
            {
                if(!m_initDone)
                { 
                    // load settings from file
                    Settings_old = RegulatedNoiseSettings.LoadSettings();
                    IniFile = new STA.Settings.INIFile(GetDataPath("ED-IBE.ini"), false, true);

                    // starT database process (if not running)
                    DBProcess.DBProcessParams newProcessParams  = new DBProcess.DBProcessParams() { };
                    newProcessParams.Commandline                = IniFile.GetValue("DB_Server",        "Commandline",      @"bin\mysqld.exe");    
                    newProcessParams.Commandargs                = IniFile.GetValue("DB_Server",        "CommandArgs",      @"--defaults-file=Elite.ini --console");
                    newProcessParams.Workingdirectory           = IniFile.GetValue("DB_Server",        "WorkingDirectory", @"..\..\..\RNDatabase\Database");
                    newProcessParams.Port                       = IniFile.GetValue<Int16>("DB_Server", "Port",             "3306");
                    newProcessParams.DBStartTimeout             = IniFile.GetValue<Int16>("DB_Server", "DBStartTimeout",   "60");
                
                    EliteDBProcess                              = new DBProcess(newProcessParams);


                    // connecT to the database
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

                    /* **************** database is running ********************** */
                    

                    // prepare colors-object
                    Colors                                      = new GUIColors();

                    // preprare main data object
                    Data                                        = new RegulatedNoise.SQL.EliteDBIO();
                    Data.InitializeData();
                    Data.PrepareBaseTables();

                    // create global paths-object
                    Paths                                       = new ProgramPaths();

                    // prepare settings
                    Settings                                    = new Settings();
                    Settings.BaseData                           = Data.BaseData;

                    // prepare commanders log 
                    CommandersLog                               = new CommandersLog();
                    CommandersLog.BaseData                      = Data.BaseData;

                    // prepare price analysis 
                    PriceAnalysis                               = new PriceAnalysis();
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
            if(DBCon != null)
            { 
                DBCon.Dispose();
                DBCon = null;
            }

            if(EliteDBProcess != null)
            { 
                EliteDBProcess.Dispose();
                EliteDBProcess = null;
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

    #endregion //global objects

    }
}
