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
            String FileName = String.Format("ed-ibe-dump-v{0}.dmp", VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3)).Replace(".", "_");
            if (ex == null)
                return;

            if(!Program.SplashScreen.IsDisposed)
                Program.SplashScreen.TopMost = false;

            cErr.processError(ex, "Unhandled Exception", true);
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

        public static SingleThreadLogger                MainLog = new SingleThreadLogger(ThreadLoggerType.App);

        private static Boolean                          m_initDone                  = false;

        public static GUIColors                         Colors;
        public static CompanionInterface                CompanionIO;
        public static ExternalDataInterface             ExternalData;
        public static DBConnector                       DBCon = null;
        public static STA.Settings.INIFile              IniFile;
        private static DBProcess                        EliteDBProcess;
        public static CommandersLog                     CommandersLog;
        public static PriceAnalysis                     PriceAnalysis;
        public static EliteDBIO                         Data;
        public static Condition                         actualCondition;
        public static EDLogfileScanner                  LogfileScanner;
        public static SplashScreenForm                  SplashScreen;
        public static EDDN.EDDNCommunicator             EDDNComm;
        public static PlausibiltyChecker                PlausibiltyCheck;
        public static GameSettings                      GameSettings;

        private static ManualResetEvent                 m_MREvent;                      // for updating the database with scripts
        private static Boolean                          m_gotScriptErrors = false;      // for updating the database with scripts
        private static Version                          m_OldDBVersion;

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
                    m_OldDBVersion = Program.DBUpdate();

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

                    // starting the external data interface
                    ExternalData                                = new ExternalDataInterface();

                    // initializing the object for the actual condition
                    actualCondition                             = new Condition();

                    // initializing the LogfileScanner
                    LogfileScanner                              = new EDLogfileScanner();


                    // forwards a potentially new system or station information to database
                    Program.LogfileScanner.LocationInfo += LogfileScanner_LocationInfo;
                    Program.ExternalData.LocationInfo   += ExternalData_LocationInfo;

                    // register the LogfileScanner in the CommandersLog for the DataSavedEvent-event
                    CommandersLog.registerLogFileScanner(LogfileScanner);
                    CommandersLog.registerExternalTool(ExternalData);
                    
                    PriceAnalysis.registerLogFileScanner(LogfileScanner);
                    PriceAnalysis.registerExternalTool(ExternalData);

                    PlausibiltyCheck = new PlausibiltyChecker();

                    EDDNComm = new IBE.EDDN.EDDNCommunicator();

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
                if(EDDNComm != null)
                {
                    EDDNComm.Dispose();
                    EDDNComm = null;
                }

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
        internal static Version DBUpdate()
        {
            Version dbVersion;
            Version appVersion;
            Boolean foundError = false;
    
            try
            {
                Program.SplashScreen.InfoAdd("check for required structure updates...");
                dbVersion   = Program.DBCon.getIniValue<Version>("Database", "Version", new Version(0,0,0,0).ToString(), false);
                appVersion  = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                if (dbVersion < appVersion)
                {
                    if (dbVersion < new Version(0,1,1))
                    {
                        String sqlString;

                        Program.SplashScreen.InfoAdd("...updating from v0.1.0...");

                        sqlString = "SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;                       " +
                                    "SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;        " +
                                    "SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';      " +
                                    "                                                                               " +
                                    "ALTER TABLE `elite_db`.`tbLevelLocalization`                                   " +
                                    "DROP FOREIGN KEY `fk_tbEconomyLevel_has_tbLanguage_tbEconomyLevel1`,           " +
                                    "DROP FOREIGN KEY `fk_tbEconomyLevel_has_tbLanguage_tbLanguage1`;               " +
                                    "                                                                               " +
                                    "ALTER TABLE `elite_db`.`tbLevelLocalization`                                   " +
                                    "ADD CONSTRAINT `fk_tbEconomyLevel_has_tbLanguage_tbEconomyLevel1`              " +
                                    "  FOREIGN KEY (`economylevel_id`)                                              " +
                                    "  REFERENCES `elite_db`.`tbEconomyLevel` (`id`)                                " +
                                    "  ON DELETE CASCADE                                                            " +
                                    "  ON UPDATE CASCADE,                                                           " +
                                    "ADD CONSTRAINT `fk_tbEconomyLevel_has_tbLanguage_tbLanguage1`                  " +
                                    "  FOREIGN KEY (`language_id`)                                                  " +
                                    "  REFERENCES `elite_db`.`tbLanguage` (`id`)                                    " +
                                    "  ON DELETE CASCADE                                                            " +
                                    "  ON UPDATE CASCADE;                                                           " +
                                    "                                                                               " +
                                    "                                                                               " +
                                    "SET SQL_MODE=@OLD_SQL_MODE;                                                    " +
                                    "SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;                                " +
                                    "SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;                                          ";

                        try
                        {
                            DBCon.Execute(sqlString);

                            Program.DBCon.setIniValue("Database", "Version", appVersion.ToString());
                            Program.SplashScreen.InfoAppendLast("<OK>");
                        }
                        catch (Exception ex)
                        {
                            Program.SplashScreen.InfoAppendLast("<failed>");
                            foundError = true;
                            MessageBox.Show("Error: could not update database to v0.1.1");   
                        }
                    }

                    if (dbVersion < new Version(0,1,3))
                    {
                        // there was a bug while writing default files with special characters like '\'
                        Program.DBCon.setIniValue("General", "Path_Import", Program.GetDataPath("data"));
                    }

                    if (dbVersion < new Version(0,1,4) && (Program.DBCon.ConfigData.TimeOut < 1000))
                    {
                        // there was a bug while writing default files with special characters like '\'
                        IniFile.SetValue("DB_Connection",   "TimeOut",        "10000");
                        if(!Program.SplashScreen.IsDisposed)
                            Program.SplashScreen.TopMost = false;

                        MessageBox.Show("DB-Timeoutsetting changed. Please restart ED-IBE", "Restart required",  MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if(!Program.SplashScreen.IsDisposed)
                            Program.SplashScreen.TopMost = false;
                    }

                    if (dbVersion < new Version(0,1,5))
                    {
                        // switch off the general log for the database
                        STA.Settings.INIFile dbIniFile;

                        if(Debugger.IsAttached)
                           dbIniFile = new STA.Settings.INIFile(Path.Combine(IniFile.GetValue("DB_Server", "WorkingDirectory", @"..\..\..\RNDatabase\Database"), "Elite.ini"), false, true, true);
                        else
                           dbIniFile = new STA.Settings.INIFile(GetDataPath(@"Database\Elite.ini"), false, true, true);

                        dbIniFile.RemoveValue("mysqld",   "general-log");
                    }

                    if (dbVersion < new Version(0,2,0))
                    {
                        String sqlString;

                        Program.SplashScreen.InfoAdd("...updating structure of database to v0.2.0...");
                        Program.SplashScreen.InfoAdd("...please be patient, this can take a few minutes depending on your system and data...");
                        Program.SplashScreen.InfoAdd("...");

                        // add changes to the database
                        sqlString = "-- MySQL Workbench Synchronization                                                                                                                                            \n" +
                                    "-- Generated: 2016-03-25 20:24                                                                                                                                                \n" +
                                    "-- Model: New Model                                                                                                                                                           \n" +
                                    "-- Version: 1.0                                                                                                                                                               \n" +
                                    "-- Project: Name of the project                                                                                                                                               \n" +
                                    "-- Author: Duke                                                                                                                                                               \n" +
                                    "                                                                                                                                                                              \n" +
                                    "SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;                                                                                                                      \n" +
                                    "SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;                                                                                                       \n" +
                                    "SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';                                                                                                     \n" +
                                    "                                                                                                                                                                              \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tbCommodityData`                                                                                                                                      \n" +
                                    "DROP PRIMARY KEY,                                                                                                                                                             \n" +
                                    "ADD PRIMARY KEY (`id`),                                                                                                                                                       \n" +
                                    "CHANGE COLUMN `id` `id` BIGINT(20) NOT NULL AUTO_INCREMENT;                                                                                                                   \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tbPriceHistory`                                                                                                                                       \n" +
                                    "DROP FOREIGN KEY `fk_tbPriceHistory_tbSources1`;                                                                                                                              \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tbPriceHistory`                                                                                                                                       \n" +
                                    "DROP INDEX `fk_tbPriceHistory_tbSources1_idx`;                                                                                                                                \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tbPriceHistory`                                                                                                                                       \n" +
                                    "DROP COLUMN `Source_id`,                                                                                                                                                      \n" +
                                    "CHANGE COLUMN `id` `id`  BIGINT(20) NOT NULL AUTO_INCREMENT ,                                                                                                                 \n" +
                                    "CHANGE COLUMN `timestamp` `timestamp` DATETIME NOT NULL ,                                                                                                                     \n" +
                                    "ADD COLUMN `Sources_id` INT(11) NOT NULL AFTER `SupplyLevel`;                                                                                                                 \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tbPriceHistory`                                                                                                                                       \n" +
                                    "ADD INDEX `fk_tbPriceHistory_tbSources1_idx` (`Sources_id` ASC);                                                                                                              \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tbPriceHistory` ADD CONSTRAINT `fk_tbPriceHistory_tbSources1`                                                                                         \n" +
                                    "  FOREIGN KEY (`Sources_id`)                                                                                                                                                  \n" +
                                    "  REFERENCES `elite_db`.`tbSource` (`id`)                                                                                                                                     \n" +
                                    "  ON DELETE NO ACTION                                                                                                                                                         \n" +
                                    "  ON UPDATE NO ACTION;                                                                                                                                                        \n" +
                                    "                                                                                                                                                                              \n" +
                                    "INSERT INTO `Elite_DB`.`tbInitValue` (`InitGroup`, `InitKey`, `InitValue`) VALUES ('Database', 'CollectPriceHistory', 'True');                                                \n" +
                                    "                                                                                                                                                                              \n" +
                                    "DELIMITER $$                                                                                                                                                                  \n" +
                                    "                                                                                                                                                                              \n" +
                                    "USE `elite_db`$$                                                                                                                                                              \n" +
                                    "CREATE DEFINER = CURRENT_USER TRIGGER `elite_db`.`tbCommodityData_AFTER_INSERT` AFTER INSERT ON `tbCommodityData` FOR EACH ROW                                                \n" +
                                    "BEGIN                                                                                                                                                                         \n" +
                                    "	DECLARE isActive BOOLEAN;                                                                                                                                                  \n" +
                                    "                                                                                                                                                                              \n" +
                                    "    SELECT ((InitValue <> '0') and (InitValue <> 'False')) INTO isActive                                                                                                      \n" +
                                    "    FROM tbInitValue                                                                                                                                                          \n" +
                                    "    WHERE InitGroup = 'Database'                                                                                                                                              \n" +
                                    "    AND   InitKey   = 'CollectPriceHistory';                                                                                                                                  \n" +
                                    "                                                                                                                                                                              \n" +
                                    "    IF isActive THEN                                                                                                                                                          \n" +
                                    "		INSERT INTO `elite_db`.`tbPriceHistory`                                                                                                                                \n" +
                                    "		(`station_id`, `commodity_id`, `Sell`, `Buy`, `Demand`, `DemandLevel`, `Supply`, `SupplyLevel`, `Sources_id`, `timestamp`)                                             \n" +
                                    "		VALUES                                                                                                                                                                 \n" +
                                    "		(NEW.`station_id`, NEW.`commodity_id`, NEW.`Sell`, NEW.`Buy`, NEW.`Demand`, NEW.`DemandLevel`, NEW.`Supply`, NEW.`SupplyLevel`, NEW.`Sources_id`, NEW.`timestamp`);	   \n" +
                                    "	END IF;                                                                                                                                                                    \n" +
                                    "END$$                                                                                                                                                                         \n" +
                                    "                                                                                                                                                                              \n" +
                                    "USE `elite_db`$$                                                                                                                                                              \n" +
                                    "CREATE DEFINER = CURRENT_USER TRIGGER `elite_db`.`tbCommodityData_AFTER_UPDATE` AFTER UPDATE ON `tbCommodityData` FOR EACH ROW                                                \n" +
                                    "BEGIN                                                                                                                                                                         \n" +
                                    "	DECLARE isActive BOOLEAN;                                                                                                                                                  \n" +
                                    "                                                                                                                                                                              \n" +
                                    "    SELECT ((InitValue <> '0') and (InitValue <> 'False')) INTO isActive                                                                                                      \n" +
                                    "    FROM tbInitValue                                                                                                                                                          \n" +
                                    "    WHERE InitGroup = 'Database'                                                                                                                                              \n" +
                                    "    AND   InitKey   = 'CollectPriceHistory';                                                                                                                                  \n" +
                                    "                                                                                                                                                                              \n" +
                                    "    IF isActive THEN                                                                                                                                                          \n" +
                                    "		IF (NEW.Sell <> OLD.Sell) OR (NEW.Buy <> OLD.Buy) OR (NEW.Sources_id <> OLD.Sources_id) OR                                                                             \n" +
                                    "		   (TIMESTAMPDIFF(hour, OLD.timestamp, NEW.timestamp) > 24) THEN                                                                                                       \n" +
                                    "			INSERT INTO `elite_db`.`tbPriceHistory`                                                                                                                            \n" +
                                    "			(`station_id`, `commodity_id`, `Sell`, `Buy`, `Demand`, `DemandLevel`, `Supply`, `SupplyLevel`, `Sources_id`, `timestamp`)                                         \n" +
                                    "			VALUES                                                                                                                                                             \n" +
                                    "			(NEW.`station_id`, NEW.`commodity_id`, NEW.`Sell`, NEW.`Buy`, NEW.`Demand`, NEW.`DemandLevel`, NEW.`Supply`, NEW.`SupplyLevel`, NEW.`Sources_id`, NEW.`timestamp`);\n" +
                                    "		END IF;                                                                                                                                                                \n" +
                                    "	END IF;                                                                                                                                                                    \n" +
                                    "END$$                                                                                                                                                                         \n" +
                                    "                                                                                                                                                                              \n" +
                                    "                                                                                                                                                                              \n" +
                                    "DELIMITER ;                                                                                                                                                                   \n" +
                                    "                                                                                                                                                                              \n" +
                                    "-- shift id-values to right, because we need 0 as undefined data                                                                                                              \n" +
                                    "update tbSource set source = 'IBE' where id = 1;                                                                                                                              \n" +
                                    "update tbSource set source = 'EDDN' where id = 2;                                                                                                                             \n" +
                                    "insert into tbSource(id, source) values (3, 'FILE');                                                                                                                          \n" +
                                    "                                                                                                                                                                              \n" +
                                    "update tbCommodityData set Sources_id = 1;                                                                                                                                    \n" +
                                    "update tbPriceHistory set Sources_id = 1;                                                                                                                                     \n" +
                                    "delete from tbSource where id = 0;                                                                                                                                            \n" +
                                    "                                                                                                                                                                              \n" +
                                    "INSERT ignore INTO `Elite_DB`.`tbPriceHistory`                                                                                                                                \n" + 
	                                " (`station_id`, `commodity_id`, `Sell`, `Buy`, `Demand`, `DemandLevel`, `Supply`, `SupplyLevel`, `Sources_id`, `timestamp`)                                                   \n" + 
	                                " select `station_id`, `commodity_id`, `Sell`, `Buy`, `Demand`, `DemandLevel`, `Supply`, `SupplyLevel`, `Sources_id`, `timestamp` from tbcommoditydata;                        \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tmPA_S2S_StationData`                                                                                                                                 \n" +
                                    "ADD COLUMN `Sources_id` INT(11) NULL DEFAULT NULL AFTER `Profit`;                                                                                                             \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tmPA_ByCommodity`                                                                                                                                     \n" +
                                    "ADD COLUMN `Sources_id` INT(11) NULL DEFAULT NULL AFTER `Timestamp`;                                                                                                          \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tmPA_S2S_BestTrips`                                                                                                                                   \n" +
                                    "ADD COLUMN `DistanceToRoute` DOUBLE NULL DEFAULT NULL AFTER `DistanceToStar_2`;                                                                                               \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tmPA_ByStation`                                                                                                                                       \n" +
                                    "CHANGE COLUMN `Source` `Sources_id` INT(11) NULL DEFAULT NULL ;                                                                                                               \n" +
                                    "                                                                                                                                                                              \n" +
                                    "ALTER TABLE `elite_db`.`tmPA_AllCommodities`                                                                                                                                  \n" +
                                    "ADD COLUMN `Buy_Sources_id` INT(11) NULL DEFAULT NULL AFTER `Buy_Timestamp`,                                                                                                  \n" +
                                    "ADD COLUMN `Sell_Sources_id` INT(11) NULL DEFAULT NULL AFTER `Sell_Timestamp`;                                                                                                \n" +
                                    "                                                                                                                                                                              \n" +
                                    "CREATE TABLE IF NOT EXISTS `elite_db`.`tbTrustedSenders` (                                                                                                                    \n" +
                                    "  `Name` VARCHAR(255) NOT NULL,                                                                                                                                               \n" +
                                    "  PRIMARY KEY (`Name`))                                                                                                                                                       \n" +
                                    "ENGINE = InnoDB                                                                                                                                                               \n" +
                                    "DEFAULT CHARACTER SET = utf8;                                                                                                                                                 \n" +
                                    "                                                                                                                                                                              \n" +
                                    "INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('E:D Market Connector [Windows]');                                                                                 \n" +
                                    "INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('EDAPI Trade Dangerous Plugin');                                                                                   \n" +
                                    "INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('E:D Market Connector [Mac OS]');                                                                                  \n" +
                                    "INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('ED-IBE (API)');                                                                                                   \n" +
                                    "                                                                                                                                                                              \n" +
                                    "SET SQL_MODE=@OLD_SQL_MODE;                                                                                                                                                   \n" +
                                    "SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;                                                                                                                               \n" +
                                    "SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;                                                                                                                                         \n";

                        var sqlScript = new MySql.Data.MySqlClient.MySqlScript((MySql.Data.MySqlClient.MySqlConnection)Program.DBCon.Connection);
                        sqlScript.Query = sqlString;

                        sqlScript.Error             += sqlScript_Error;
                        sqlScript.ScriptCompleted   += sqlScript_ScriptCompleted;
                        sqlScript.StatementExecuted += sqlScript_StatementExecuted;

                        m_MREvent = new ManualResetEvent(false);

                        sqlScript.ExecuteAsync();

                        sqlScript.Error             -= sqlScript_Error;
                        sqlScript.ScriptCompleted   -= sqlScript_ScriptCompleted;
                        sqlScript.StatementExecuted -= sqlScript_StatementExecuted;

                        if (!m_MREvent.WaitOne(new TimeSpan(0, 5, 0)))
                        {
                            foundError = true;
                            Program.SplashScreen.InfoAppendLast("finished with errors !");
                        }
                        else if (m_gotScriptErrors)
                        {
                            foundError = true;
                            Program.SplashScreen.InfoAppendLast("finished with errors !");
                        }
                        else
                            Program.SplashScreen.InfoAdd("...updating structure of database to v0.2.0...<OK>");

                        Data.PrepareBaseTables("tbsource");
                    }
                    

                    if (!foundError) 
                        Program.DBCon.setIniValue("Database", "Version", appVersion.ToString());
                    else
                    {
                        Boolean oldValue = false;
                        if(!Program.SplashScreen.IsDisposed)
                        {
                            oldValue = Program.SplashScreen.TopMost;
                            Program.SplashScreen.TopMost = false;
                        }
                        MessageBox.Show("Critical : There was errors during updating the database to the current version.\n" +
                                        "Please save current logs form the <Logs> subdirectory and send them to the developer !", 
                                        "Updating Database",  MessageBoxButtons.OK, MessageBoxIcon.Error) ;

                        if(!Program.SplashScreen.IsDisposed)
                        {
                            Program.SplashScreen.TopMost = oldValue;
                        }
                    }

                }
                else
                {
                    Program.SplashScreen.InfoAppendLast("<OK>");
                }

                return dbVersion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while doing special things", ex);
            }
        }

        /// <summary>
        /// this sub starts special things to do if this version runs
        /// for the first time
        /// </summary>
        internal static void DoSpecial()
        {
            try
            {

                if (!Program.Data.InitImportDone)
                {
                    if (m_OldDBVersion != new Version(0,1,0))
                    { 
                        // here it's required to import all master data 
                        var DataIO = new frmDataIO();

                        Program.SplashScreen.InfoAdd("Importing master data...");
                        Thread.Sleep(1500);

                        DataIO.InfoTarget = Program.SplashScreen.SplashInfo;
                        DataIO.ReUseLine  = true;

                        DataIO.StartMasterImport(GetDataPath("Data"));

                        DataIO.Close();
                        DataIO.Dispose();
                        
                        Program.SplashScreen.InfoAdd("Importing master data...<OK>");
                    }

                    Program.Data.InitImportDone = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while doing special things", ex);
            }
        }

        static void sqlScript_ScriptCompleted(object sender, EventArgs e)
        {
            m_MREvent.Set();   
            Debug.Print("RE");
        }

        static void sqlScript_StatementExecuted(object sender, MySql.Data.MySqlClient.MySqlScriptEventArgs args)
        {
            Program.MainLog.Log(String.Format("...executed : pos={0},  line={1}, command=<{2}>", args.Position, args.Line, args.StatementText));
            Program.SplashScreen.InfoAppendLast("√");
            Debug.Print("executed");
        }

        static void sqlScript_Error(object sender, MySql.Data.MySqlClient.MySqlScriptErrorEventArgs args)
        {
            Program.MainLog.Log(String.Format("...executed : pos={0},  line={1},\n - command=<{2}>,\n - error=<{3}>", args.Position, args.Line, args.StatementText, args.Exception));
            Program.SplashScreen.InfoAppendLast("X");
            args.Ignore = false;
            m_gotScriptErrors = true;
            Debug.Print("error");
        }

    #endregion //global objects

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


    }
}
