using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using IBE.SQL;
using System.Threading;

namespace IBE
{
    internal class Updater
    {
        private static Version                              m_OldDBVersion;
        private static Version                              m_NewDBVersion;
        private static ManualResetEvent                     m_MREvent;                      // for updating the database with scripts
        private static Boolean                              m_gotScriptErrors = false;      // for updating the database with scripts

        /// <summary>
        /// Checks if there's a new version available.
        /// If available, "versionFound" holds the new version and "versionInfo" holds the detail information
        /// </summary>
        /// <param name="versionFound"></param>
        /// <param name="versionInfo"></param>
        /// <returns></returns>
        public static Boolean CheckVersion(out Version versionFound, out String versionInfo)
        {
            string sURL                 = @"https://api.github.com/repos/Duke-Jones/ED-IBE/releases";
            HttpWebRequest webRequest   = System.Net.WebRequest.Create(sURL) as HttpWebRequest;
            webRequest.Method           = "GET";
            webRequest.UserAgent        = "ED-IBE";
            webRequest.ServicePoint.Expect100Continue = false;
                
            Regex versionCheck          = new Regex("[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}");
            Version newestVersion       = new Version(0,0,0,0);
            Version releaseVersion;
            String releaseData;
            String releaseTag;
            Boolean isPrerRelease;
            Match match;
            Version current;
            Boolean retValue = false;

            versionFound = new Version(0,0,0,0);
            versionInfo  = "";

            try
            {
                current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    releaseData = responseReader.ReadToEnd();

                dynamic data            = JsonConvert.DeserializeObject<dynamic>(releaseData);
                dynamic releaseDetails  = null;

                foreach (var responsePart in data)
                {
                    releaseTag      = responsePart.tag_name;
                    isPrerRelease   = (bool)responsePart.prerelease;

                    if (isPrerRelease == false)
                    {
                        match = versionCheck.Match(releaseTag);
                        
                        if (Version.TryParse(match.Value, out releaseVersion))
                        {
                            if (newestVersion < releaseVersion)
                            {
                                newestVersion  = releaseVersion;
                                releaseDetails = responsePart;
                            }
                        }
                    }
                }

                current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                if (current < newestVersion)
                {
                    versionFound = newestVersion;
                    versionInfo  = releaseDetails.body;

                    retValue = true;
                }

                return retValue;
            }
            catch
            {
                // Not a disaster if we can't do the version check...
                return false;
            }

        }

        /// <summary>
        /// this sub starts special things to do if this version runs
        /// for the first time
        /// </summary>
        internal static void DBUpdate()
        {
            Version dbVersion;
            Version appVersion;
            Boolean foundError = false;
    
            try
            {
                Program.SplashScreen.InfoAdd("check for required structure updates...");
                dbVersion   = Program.DBCon.getIniValue<Version>("Database", "Version", new Version(0,0,0,0).ToString(), false);
                appVersion  = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                m_OldDBVersion = dbVersion;
                m_NewDBVersion = appVersion;

                if (dbVersion < appVersion)
                {
                    if (dbVersion < new Version(0,1,1))
                        UpdateTo_0_1_1(appVersion, ref foundError);

                    if (dbVersion < new Version(0,1,3))
                        UpdateTo_0_1_3();

                    if (dbVersion < new Version(0,1,4) && (Program.DBCon.ConfigData.TimeOut < 1000))
                        UpdateTo_0_1_4();

                    if (dbVersion < new Version(0,1,5))
                        UpdateTo_0_1_5();

                    if (dbVersion < new Version(0,2,0))
                        UpdateTo_0_2_0(ref foundError);

                    if (dbVersion < new Version(0, 2, 1))
                        UpdateTo_0_2_1(ref foundError);

                    if (dbVersion < new Version(0, 2, 3))
                        UpdateTo_0_2_3(ref foundError);

                    if (dbVersion < new Version(0, 3, 0))
                        UpdateTo_0_3_0(ref foundError);

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

                // if not already set...
                SetGridDefaults(false);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while doing special things", ex);
            }
        }

#region update functions DB

        private static void UpdateTo_0_1_1(Version appVersion, ref Boolean foundError)
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
                Program.DBCon.Execute(sqlString);

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
        private static void UpdateTo_0_1_3()
        {
            // there was a bug while writing default files with special characters like '\'
            Program.DBCon.setIniValue("General", "Path_Import", Program.GetDataPath("data"));
        }
        private static void UpdateTo_0_1_4()
        {
            // there was a bug while writing default files with special characters like '\'
            Program.IniFile.SetValue("DB_Connection", "TimeOut", "10000");
            if (!Program.SplashScreen.IsDisposed)
                Program.SplashScreen.TopMost = false;

            MessageBox.Show("DB-Timeoutsetting changed. Please restart ED-IBE", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (!Program.SplashScreen.IsDisposed)
                Program.SplashScreen.TopMost = true;
        }
        private static void UpdateTo_0_1_5()
        {
            // switch off the general log for the database
            STA.Settings.INIFile dbIniFile;

            if (Debugger.IsAttached)
                dbIniFile = new STA.Settings.INIFile(Path.Combine(Program.IniFile.GetValue("DB_Server", "WorkingDirectory", @"..\..\..\RNDatabase\Database"), "Elite.ini"), false, true, true);
            else
                dbIniFile = new STA.Settings.INIFile(Program.GetDataPath(@"Database\Elite.ini"), false, true, true);

            dbIniFile.RemoveValue("mysqld", "general-log");
        }
        private static void UpdateTo_0_2_0(ref Boolean foundError)
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

            sqlScript.Error += sqlScript_Error;
            sqlScript.ScriptCompleted += sqlScript_ScriptCompleted;
            sqlScript.StatementExecuted += sqlScript_StatementExecuted;

            m_MREvent = new ManualResetEvent(false);

            sqlScript.ExecuteAsync();

            sqlScript.Error -= sqlScript_Error;
            sqlScript.ScriptCompleted -= sqlScript_ScriptCompleted;
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
        }

        private static void UpdateTo_0_2_1(ref Boolean foundError)
        {
            String sqlString;

            Program.SplashScreen.InfoAdd("...updating structure of database to v0.2.1...");
            Program.SplashScreen.InfoAdd("...please be patient, this can take a few minutes depending on your system and data...");

            // insert settings for new columns
            EliteDBIO.InsertColumnDefinition(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "StationToStationRoutes_ColumnSettings", 6, 6, "True/NotSet/40/100/5");
            EliteDBIO.InsertColumnDefinition(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "StationToStationRoutes_ColumnSettings", 13, 13, "True/NotSet/40/100/5");

            Program.SplashScreen.InfoAdd("...");

            // add changes to the database
            sqlString = "-- MySQL Workbench Synchronization                                                           \n" +
                        "-- Generated: 2016-04-18 12:27                                                               \n" +
                        "-- Model: New Model                                                                          \n" +
                        "-- Version: 1.0                                                                              \n" +
                        "-- Project: Name of the project                                                              \n" +
                        "-- Author: Duke                                                                              \n" +
                        "                                                                                             \n" +
                        "SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;                                     \n" +
                        "SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;                      \n" +
                        "SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';                    \n" +
                        "                                                                                             \n" +
                        "ALTER TABLE `elite_db`.`tmPA_S2S_BestTrips`                                                  \n" +
                        "ADD COLUMN `Station_Location_1` VARCHAR(80) NULL DEFAULT NULL AFTER `TimeStamp_1`,           \n" +
                        "ADD COLUMN `Station_Location_2` VARCHAR(80) NULL DEFAULT NULL AFTER `TimeStamp_2`;           \n" +
                        "                                                                                             \n" +
                        "ALTER TABLE `elite_db`.`tbStations`                                                          \n" +
                        "CHANGE COLUMN `max_landing_pad_size` `max_landing_pad_size` VARCHAR(80) NULL DEFAULT NULL ;  \n" +
                        "                                                                                             \n" +
                        "ALTER TABLE `elite_db`.`tbStations_org`                                                      \n" +
                        "CHANGE COLUMN `max_landing_pad_size` `max_landing_pad_size` VARCHAR(80) NULL DEFAULT NULL ;  \n" +
                        "                                                                                             \n" +
                        "                                                                                             \n" +
                        "SET SQL_MODE=@OLD_SQL_MODE;                                                                  \n" +
                        "SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;                                              \n" +
                        "SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;                                                        \n";


            var sqlScript = new MySql.Data.MySqlClient.MySqlScript((MySql.Data.MySqlClient.MySqlConnection)Program.DBCon.Connection);
            sqlScript.Query = sqlString;

            sqlScript.Error += sqlScript_Error;
            sqlScript.ScriptCompleted += sqlScript_ScriptCompleted;
            sqlScript.StatementExecuted += sqlScript_StatementExecuted;

            m_MREvent = new ManualResetEvent(false);

            sqlScript.ExecuteAsync();

            sqlScript.Error -= sqlScript_Error;
            sqlScript.ScriptCompleted -= sqlScript_ScriptCompleted;
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
                Program.SplashScreen.InfoAdd("...updating structure of database to v0.2.1...<OK>");
        }


        private static void UpdateTo_0_2_3(ref Boolean foundError)
        {
            String sqlString;

            Program.SplashScreen.InfoAdd("...updating structure of database to v0.2.3...");
            Program.SplashScreen.InfoAdd("...please be patient, this can take a few minutes depending on your system and data...");

            // insert settings for new columns
            EliteDBIO.InsertColumnDefinition(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "Station1_ColumnSettings", 0, 0, "False/NotSet/40/100/5");
            EliteDBIO.InsertColumnDefinition(IBE.MTPriceAnalysis.tabPriceAnalysis.DB_GROUPNAME, "Station2_ColumnSettings", 0, 0, "False/NotSet/40/100/5");

            Program.SplashScreen.InfoAdd("...");

            // add changes to the database
            sqlString = "-- MySQL Workbench Synchronization                                          \n" +
                        "-- Generated: 2016-04-26 14:30                                              \n" +
                        "-- Model: New Model                                                         \n" +
                        "-- Version: 1.0                                                             \n" +
                        "-- Project: Name of the project                                             \n" +
                        "-- Author: Duke                                                             \n" +
                        "                                                                            \n" +
                        "SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;                    \n" +
                        "SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;     \n" +
                        "SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';   \n" +
                        "                                                                            \n" +
                        "ALTER TABLE `elite_db`.`tmPA_S2S_StationData`                               \n" +
                        "ADD COLUMN `Station_ID` INT(11) NULL DEFAULT NULL FIRST;                    \n" +
                        "                                                                            \n" +
                        "SET SQL_MODE=@OLD_SQL_MODE;                                                 \n" +
                        "SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;                             \n" +
                        "SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;                                       \n";

            var sqlScript = new MySql.Data.MySqlClient.MySqlScript((MySql.Data.MySqlClient.MySqlConnection)Program.DBCon.Connection);
            sqlScript.Query = sqlString;

            sqlScript.Error += sqlScript_Error;
            sqlScript.ScriptCompleted += sqlScript_ScriptCompleted;
            sqlScript.StatementExecuted += sqlScript_StatementExecuted;

            m_MREvent = new ManualResetEvent(false);

            sqlScript.ExecuteAsync();

            sqlScript.Error -= sqlScript_Error;
            sqlScript.ScriptCompleted -= sqlScript_ScriptCompleted;
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
                Program.SplashScreen.InfoAdd("...updating structure of database to v0.2.3...<OK>");

            // forcing once
            SetGridDefaults(true);
        }

        private static void UpdateTo_0_3_0(ref Boolean foundError)
        {
            try
            {
                String sqlString;

                Program.SplashScreen.InfoAdd("...updating structure of database to v0.3.0...");
                Program.SplashScreen.InfoAdd("...please be patient, this can take a few minutes depending on your system and data...");
                Program.SplashScreen.InfoAdd("...");

                // add changes to the database
                sqlString = "-- MySQL Workbench Synchronization                                               \n" +
                            "-- Generated: 2016-05-08 19:14                                                   \n" +
                            "-- Model: New Model                                                              \n" +
                            "-- Version: 1.0                                                                  \n" +
                            "-- Project: Name of the project                                                  \n" +
                            "-- Author: Duke                                                                  \n" +
                            "                                                                                 \n" +
                            "SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;                         \n" +
                            "SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;          \n" +
                            "SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';        \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbCommodityMapping` (                     \n" +
                            "  `Name` VARCHAR(80) NOT NULL,                                                   \n" +
                            "  `MappedName` VARCHAR(80) NOT NULL,                                             \n" +
                            "  PRIMARY KEY (`Name`))                                                          \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Armour` (                         \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Weapon` (                         \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_MissileType` (                    \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_WeaponMount` (                    \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_WeaponClass` (                    \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_WeaponRating` (                   \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_OldVariant` (                     \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_CounterMeasure` (                 \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Utility` (                        \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Rating` (                         \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_RatingPlanet` (                   \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Standard` (                       \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Internal` (                       \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Internal_Misc` (                  \n" +
                            "  `CompanionName` VARCHAR(255) NOT NULL,                                         \n" +
                            "  `CompanionAddition` VARCHAR(255) NOT NULL,                                     \n" +
                            "  `GameName` VARCHAR(255) NOT NULL,                                              \n" +
                            "  `GameAddition` VARCHAR(255) NOT NULL,                                          \n" +
                            "  PRIMARY KEY (`CompanionName`, `CompanionAddition`))                            \n" +
                            "ENGINE = InnoDB                                                                  \n" +
                            "DEFAULT CHARACTER SET = utf8;                                                    \n" +
                            "                                                                                 \n" +
                            "                                                                                 \n" +
                            "SET SQL_MODE=@OLD_SQL_MODE;                                                      \n" +
                            "SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;                                  \n" +
                            "SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;                                            \n" +
                            "                                                                                 \n" +
                            "                                                                                 \n" +
                            "                                                                                 \n" +
                            "-- Data for table `elite_db`.`tbDNMap_Armour`                                                                                                                                                                          \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Armour` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('grade1', '', 'Lightweight Alloy', '');                                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Armour` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('grade2', '', 'Reinforced Alloy', '');                                                              \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Armour` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('grade3', '', 'Military Grade Composite', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Armour` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('mirrored', '', 'Mirrored Surface Composite', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Armour` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('reactive', '', 'Reactive Surface Composite', '');                                                  \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_Weapon`                                                                                                                                                                          \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('advancedtorppylon', '', 'Torpedo Pylon', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('basicmissilerack', '', 'Missile Rack', '');                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('beamlaser', '', 'Beam Laser', '');                                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('beamlaser', 'heat', 'Retributor Beam Laser', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('cannon', '', 'Cannon', '');                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('drunkmissilerack', '', 'Pack-Hound Missile Rack', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('dumbfiremissilerack', '', 'Missile Rack', '');                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('minelauncher', '', 'Mine Launcher', '');                                                           \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('minelauncher', 'impulse', 'Shock Mine Launcher', '');                                              \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('mininglaser', '', 'Mining Laser', '');                                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('mininglaser', 'advanced', 'Mining Lance Beam Laser', '');                                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('multicannon', '', 'Multi-Cannon', '');                                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('multicannon', 'strong', 'Enforcer Cannon', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('plasmaaccelerator', '', 'Plasma Accelerator', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('plasmaaccelerator', 'advanced', 'Advanced Plasma Accelerator', '');                                \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('pulselaser', '', 'Pulse Laser', '');                                                               \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('pulselaser', 'disruptor', 'Pulse Disruptor Laser', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('pulselaserburst', '', 'Burst Laser', '');                                                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('pulselaserburst', 'scatter', 'Cytoscrambler Burst Laser', '');                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('railgun', '', 'Rail Gun', '');                                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('railgun', 'burst', 'Imperial Hammer Rail Gun', '');                                                \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('slugshot', '', 'Fragment Cannon', '');                                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Weapon` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('slugshot', 'range', 'Pacifier Frag-Cannon', '');                                                   \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_MissileType`                                                                                                                                                                     \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_MissileType` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('advancedtorppylon', '', 'Seeker', '');                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_MissileType` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('basicmissilerack', '', 'Seeker', '');                                                         \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_MissileType` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('drunkmissilerack', '', 'Swarm', '');                                                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_MissileType` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('dumbfiremissilerack', '', 'Dumbfire', '');                                                    \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_WeaponMount`                                                                                                                                                                     \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponMount` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('fixed', '', 'Fixed', '');                                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponMount` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('gimbal', '', 'Gimballed', '');                                                                \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponMount` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('turret', '', 'Turreted', '');                                                                 \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_WeaponClass`                                                                                                                                                                     \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponClass` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('tiny', '', '0', '');                                                                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponClass` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('small', '', '1', '');                                                                         \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponClass` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('medium', '', '2', '');                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponClass` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('large', '', '3', '');                                                                         \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponClass` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('huge', '', '4', '');                                                                          \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_WeaponRating`                                                                                                                                                                    \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_advancedtorppylon_fixed_small', '', 'I', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_advancedtorppylon_fixed_medium', '', 'I', '');                                           \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_basicmissilerack_fixed_small', '', 'B', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_basicmissilerack_fixed_medium', '', 'B', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_fixed_small', '', 'E', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_fixed_medium', '', 'D', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_fixed_large', '', 'C', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_gimbal_small', '', 'E', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_gimbal_medium', '', 'D', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_gimbal_large', '', 'C', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_turret_small', '', 'F', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_turret_medium', '', 'E', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_beamlaser_turret_large', '', 'D', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_fixed_small', '', 'D', '');                                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_fixed_medium', '', 'D', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_fixed_large', '', 'C', '');                                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_fixed_huge', '', 'B', '');                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_gimbal_small', '', 'E', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_gimbal_medium', '', 'D', '');                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_gimbal_large', '', 'C', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_gimbal_huge', '', 'B', '');                                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_turret_small', '', 'F', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_turret_medium', '', 'E', '');                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_cannon_turret_large', '', 'D', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_drunkmissilerack_fixed_medium', '', 'B', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_dumbfiremissilerack_fixed_small', '', 'B', '');                                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_dumbfiremissilerack_fixed_medium', '', 'B', '');                                         \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_minelauncher_fixed_small', '', 'I', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_minelauncher_fixed_medium', '', 'I', '');                                                \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_mininglaser_fixed_small', '', 'D', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_mininglaser_fixed_medium', '', 'D', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_multicannon_fixed_small', '', 'F', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_multicannon_fixed_medium', '', 'E', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_multicannon_gimbal_small', '', 'G', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_multicannon_gimbal_medium', '', 'F', '');                                                \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_multicannon_turret_small', '', 'G', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_multicannon_turret_medium', '', 'F', '');                                                \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_plasmaaccelerator_fixed_medium', '', 'C', '');                                           \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_plasmaaccelerator_fixed_large', '', 'B', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_plasmaaccelerator_fixed_huge', '', 'A', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_fixed_small', '', 'F', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_fixed_medium', '', 'E', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_fixed_large', '', 'D', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_gimbal_small', '', 'G', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_gimbal_medium', '', 'F', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_gimbal_large', '', 'E', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_turret_small', '', 'G', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_turret_medium', '', 'F', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaser_turret_large', '', 'F', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_fixed_small', '', 'F', '');                                              \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_fixed_medium', '', 'E', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_fixed_large', '', 'D', '');                                              \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_gimbal_small', '', 'G', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_gimbal_medium', '', 'F', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_gimbal_large', '', 'E', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_turret_small', '', 'G', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_turret_medium', '', 'F', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_pulselaserburst_turret_large', '', 'E', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_railgun_fixed_small', '', 'D', '');                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_railgun_fixed_medium', '', 'B', '');                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_fixed_small', '', 'E', '');                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_fixed_medium', '', 'A', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_fixed_large', '', 'C', '');                                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_gimbal_small', '', 'E', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_gimbal_medium', '', 'D', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_gimbal_large', '', 'C', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_turret_small', '', 'E', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_turret_medium', '', 'D', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_WeaponRating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hpt_slugshot_turret_large', '', 'C', '');                                                    \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_OldVariant`                                                                                                                                                                      \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_OldVariant` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('f', '', 'Focussed', '');                                                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_OldVariant` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hi', '', 'High Impact', '');                                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_OldVariant` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('lh', '', 'Low Heat', '');                                                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_OldVariant` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('oc', '', 'Overcharged', '');                                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_OldVariant` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('ss', '', 'Scatter Spray', '');                                                                 \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_CounterMeasure`                                                                                                                                                                  \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_CounterMeasure` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('chafflauncher', '', 'Chaff Launcher', 'I');                                                \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_CounterMeasure` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('electroniccountermeasure', '', 'Electronic Countermeasure', 'F');                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_CounterMeasure` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('heatsinklauncher', '', 'Heat Sink Launcher', 'I');                                         \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_CounterMeasure` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('plasmapointdefence', '', 'Point Defence', 'I');                                            \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_Utility`                                                                                                                                                                         \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Utility` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('cargoscanner', '', 'Cargo Scanner', '');                                                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Utility` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('cloudscanner', '', 'Frame Shift Wake Scanner', '');                                               \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Utility` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('crimescanner', '', 'Kill Warrant Scanner', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Utility` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('shieldbooster', '', 'Shield Booster', '');                                                        \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_Rating`                                                                                                                                                                          \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Rating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('1', '', 'E', '');                                                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Rating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('2', '', 'D', '');                                                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Rating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('3', '', 'C', '');                                                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Rating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('4', '', 'B', '');                                                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Rating` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('5', '', 'A', '');                                                                                  \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_RatingPlanet`                                                                                                                                                                    \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_RatingPlanet` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('1', '', 'H', '');                                                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_RatingPlanet` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('2', '', 'G', '');                                                                            \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_Standard`                                                                                                                                                                        \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Standard` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('engine', '', 'Thrusters', '');                                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Standard` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('fueltank', '', 'Fuel Tank', '');                                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Standard` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hyperdrive', '', 'Frame Shift Drive', '');                                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Standard` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('lifesupport', '', 'Life Support', '');                                                           \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Standard` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('powerdistributor', '', 'Power Distributor', '');                                                 \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Standard` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('powerplant', '', 'Power Plant', '');                                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Standard` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('sensors', '', 'Sensors', '');                                                                    \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_Internal`                                                                                                                                                                        \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('buggybay', '', 'Planetary Vehicle Hangar', '');                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('cargorack', '', 'Cargo Rack', '');                                                               \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('collection', '', 'Collector Limpet Controller', '');                                             \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('fsdinterdictor', '', 'Frame Shift Drive Interdictor', '');                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('fuelscoop', '', 'Fuel Scoop', '');                                                               \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('fueltransfer', '', 'Fuel Transfer Limpet Controller', '');                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('hullreinforcement', '', 'Hull Reinforcement Package', '');                                       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('prospector', '', 'Prospector Limpet Controller', '');                                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('refinery', '', 'Refinery', '');                                                                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('repairer', '', 'Auto Field-Maintenance Unit', '');                                               \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('resourcesiphon', '', 'Hatch Breaker Limpet Controller', '');                                     \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('shieldcellbank', '', 'Shield Cell Bank', '');                                                    \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('shieldgenerator', '', 'Shield Generator', '');                                                   \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('shieldgenerator', 'fast', 'Bi-Weave Shield Generator', '');                                      \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('shieldgenerator', 'strong', 'Prismatic Shield Generator', '');                                   \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;                                                                                                                                                                                                                \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "-- Data for table `elite_db`.`tbDNMap_Internal_Misc`                                                                                                                                                                   \n" +
                            "-- -----------------------------------------------------                                                                                                                                                               \n" +
                            "START TRANSACTION;                                                                                                                                                                                                     \n" +
                            "USE `elite_db`;                                                                                                                                                                                                        \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal_Misc` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('detailedsurfacescanner', 'tiny', 'Detailed Surface Scanner', 'C');                          \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal_Misc` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('dockingcomputer', 'standard', 'Standard Docking Computer', 'E');                            \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal_Misc` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('stellarbodydiscoveryscanner', 'standard', 'Basic Discovery Scanner', 'E');                  \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal_Misc` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('stellarbodydiscoveryscanner', 'intermediate', 'Intermediate Discovery Scanner', 'D');       \n" +
                            "INSERT INTO `elite_db`.`tbDNMap_Internal_Misc` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`) VALUES ('stellarbodydiscoveryscanner', 'advanced', 'Advanced Discovery Scanner', 'C');               \n" +
                            "                                                                                                                                                                                                                       \n" +
                            "COMMIT;\n";


                var sqlScript = new MySql.Data.MySqlClient.MySqlScript((MySql.Data.MySqlClient.MySqlConnection)Program.DBCon.Connection);
                sqlScript.Query = sqlString;

                sqlScript.Error += sqlScript_Error;
                sqlScript.ScriptCompleted += sqlScript_ScriptCompleted;
                sqlScript.StatementExecuted += sqlScript_StatementExecuted;

                m_MREvent = new ManualResetEvent(false);

                sqlScript.ExecuteAsync();

                sqlScript.Error -= sqlScript_Error;
                sqlScript.ScriptCompleted -= sqlScript_ScriptCompleted;
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
                {
                    Program.SplashScreen.InfoAdd("...updating structure of database to v0.3.0...<OK>");

                    InsertCommodityMappings();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating to v0.3.0", ex);
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

#endregion

        /// <summary>
        /// 
        /// this sub starts special things to do if this version runs
        /// for the first time
        /// </summary>
        /// <param name="parent"></param>
        internal static void DoSpecial(Form parent)
        {
            try
            {

                if (!Program.Data.InitImportDone)
                {
                    if (m_OldDBVersion != new Version(0,1,0))
                    { 
                        // here it's required to import all master data 
                        var DataIO = new frmDataIO();

                        Program.SplashScreen.InfoAdd("importing master data...");
                        Thread.Sleep(1500);

                        DataIO.InfoTarget = Program.SplashScreen.SplashInfo;
                        DataIO.ReUseLine  = true;

                        DataIO.StartMasterImport(Program.GetDataPath("Data"));

                        InsertCommodityMappings();
                        Program.Data.PrepareBaseTables("tbcommoditymapping");
                            
                        if(!Program.SplashScreen.IsDisposed)
                            Program.SplashScreen.TopMost = false;

                        MessageBox.Show(parent, "Do you want to get a starters data kit ?\r\n\r\n" +
                                                "You will get the existing market data from all stations in\r\n" +
                                                "the bubble of 20 ly around your current location.\r\n\r\n" +
                                                "Go to the 'Data' => 'Import&Export' menu.\r\n" +
                                                "Download the latest EDDB files and start\r\n" +
                                                "the import with the 'Starters Kit' option enabled.\r\n" +
                                                "More information in the 'StarterTipps' files.", 
                                                "wanna get a starters data kit ?", 
                                                MessageBoxButtons.OK, 
                                                MessageBoxIcon.Information);

                        if(!Program.SplashScreen.IsDisposed)
                            Program.SplashScreen.TopMost = true;

                        DataIO.Close();
                        DataIO.Dispose();
                        
                        Program.SplashScreen.InfoAdd("importing master data...<OK>");
                    }

                    Program.Data.InitImportDone = true;
                }
                else if(m_OldDBVersion < m_NewDBVersion)
                { 
                    // new version installed
                    if(!Program.SplashScreen.IsDisposed)
                        Program.SplashScreen.TopMost = false;

                    var dResult = MessageBox.Show(parent, "Want to update your master data using the supplied EDDB dump files ?", 
                                                          "Update master data", 
                                                          MessageBoxButtons.YesNo, 
                                                          MessageBoxIcon.Question, 
                                                          MessageBoxDefaultButton.Button1);

                    if(!Program.SplashScreen.IsDisposed)
                        Program.SplashScreen.TopMost = true;

                    if(dResult ==  System.Windows.Forms.DialogResult.Yes)
                    {
                        var DataIO = new frmDataIO();

                        Program.SplashScreen.InfoAdd("updating master data...");
                        Thread.Sleep(1500);

                        DataIO.InfoTarget = Program.SplashScreen.SplashInfo;
                        DataIO.ReUseLine  = true;

                        DataIO.StartMasterUpdate(Program.GetDataPath("Data"));

                        DataIO.Close();
                        DataIO.Dispose();
                        
                        Program.SplashScreen.InfoAdd("updating master data...<OK>");
                    }

                    if(m_NewDBVersion == new Version(0,2,1,0))
                    {

                        if(  Program.DBCon.getIniValue<Boolean>(IBE.EDDN.EDDNView.DB_GROUPNAME, "AutoListen", false.ToString(), false) && 
                           (!Program.DBCon.getIniValue<Boolean>(IBE.EDDN.EDDNView.DB_GROUPNAME, "AutoSend",   false.ToString(), false)))
                        {
                            if(!Program.SplashScreen.IsDisposed)
                                Program.SplashScreen.TopMost = false;

                            if(MessageBox.Show(parent, "You decided to recieve data from the EDDN permanently\r\n" +
                                                       "but not to send to EDDN.\r\n\r\n" +
                                                       "The EDDN/EDDB lives from the data. If you want to receive data\r\n" +
                                                       "permanently, it would be fair in return also to send data.\r\n\r\n" +
                                                       "Shall I activate sending of market data for you?", 
                                                       "EDDN Network", 
                                                       MessageBoxButtons.YesNo, 
                                                       MessageBoxIcon.Question, 
                                                       MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                            {
                                Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "AutoSend", true.ToString());

                                if(!Program.EDDNComm.SenderIsActivated)
                                    Program.EDDNComm.ActivateSender();
                            }

                            if(!Program.SplashScreen.IsDisposed)
                                Program.SplashScreen.TopMost = true;

                        }
                    }

                    if(m_NewDBVersion == new Version(0,3,0,0))
                    {
                        Program.SplashScreen.InfoAdd("checking spell of commodity names once...");

                        var count = Program.Data.CorrectMisspelledCommodities();

                        if(count > 0)
                            Program.SplashScreen.InfoAdd("checking spell of commodity names once...<OK>");
                        else
                            Program.SplashScreen.InfoAppendLast("<OK>");
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while doing special things", ex);
            }
        }

#region update functions other

        /// <summary>
        /// inserts the initial mapping values for the commodity names
        /// </summary>
        private static void InsertCommodityMappings()
        {
            try
            {
                Dictionary<String,String> mappings = new Dictionary<string,string>() {{"Agricultural Medicines",      "Agri-Medicines"},
                                                                                      {"Atmospheric Extractors",      "Atmospheric Processors"},
                                                                                      {"Auto Fabricators",            "Auto-Fabricators"},
                                                                                      {"Basic Narcotics",             "Narcotics"},
                                                                                      {"Bio Reducing Lichen",         "Bioreducing Lichen"},
                                                                                      {"Hazardous Environment Suits", "H.E. Suits"},
                                                                                      {"Heliostatic Furnaces",        "Microbial Furnaces"},
                                                                                      {"Marine Supplies",             "Marine Equipment"},
                                                                                      {"Non Lethal Weapons",          "Non-Lethal Weapons"},
                                                                                      {"Terrain Enrichment Systems",  "Land Enrichment Systems"},
                                                                                      {"Advanceo Catalysers",         "Advanced Catalysers"},
                                                                                      {"Meta Alloys",                 "Meta-Alloys"},
                                                                                      {"Mu Tom Imager",               "Muon Imager"},
                                                                                      {"Skimer Components",           "Skimmer Components"}};

                String sqlString = "insert ignore into tbCommodityMapping(Name,MappedName) values ({0},{1})";

                foreach (var mapping in mappings)
                    Program.DBCon.Execute(String.Format(sqlString, DBConnector.SQLAEscape(mapping.Key), DBConnector.SQLAEscape(mapping.Value)));

            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting the mapping defaults", ex);
            }
        }

        /// <summary>
        /// sets defaultvalues to the columns of the datagrids
        /// </summary>
        /// <param name="overwrite"></param>
        private static void SetGridDefaults(bool overwrite)
        {
            try
            {
                String[] settings = {"CmdrsLog"     ,"CommandersLog_ColumnSettings"         ,"0/True/NotSet/70/100/5;1/True/NotSet/136/100/5;2/True/NotSet/159/100/5;3/True/NotSet/189/100/5;4/True/NotSet/118/100/5;5/True/NotSet/166/100/5;6/True/NotSet/97/100/5;7/True/NotSet/70/100/5;8/True/NotSet/62/100/5;9/True/NotSet/60/100/5;10/True/Fill/365/100/5;", 
                                     "PriceAnalysis","AllCommodities_ColumnSettings"        ,"0/False/NotSet/100/100/5;1/True/Fill/197/100/5;2/True/NotSet/80/43/5;3/False/NotSet/100/100/5;4/True/Fill/196/100/5;5/False/NotSet/100/100/5;6/True/Fill/197/100/5;7/True/NotSet/80/100/5;8/True/NotSet/80/61/5;9/True/NotSet/80/63/5;10/True/NotSet/80/100/5;11/False/NotSet/184/120/5;12/True/Fill/196/100/5;13/False/NotSet/195/120/5;14/True/Fill/197/100/5;15/True/NotSet/80/55/5;16/True/NotSet/80/62/5;17/True/NotSet/80/59.17297/5;18/True/Fill/196/100/5;",
                                     "PriceAnalysis","byCcommodity_ColumnSettings"          ,"0/False/NotSet/100/100/5;1/True/Fill/292/100/5;2/False/Fill/100/100/5;3/True/Fill/291/100/5;4/True/NotSet/80/100/5;5/True/NotSet/80/100/5;6/True/NotSet/80/100/5;7/True/NotSet/100/100/5;8/True/NotSet/80/100/5;9/True/NotSet/80/100/5;10/True/NotSet/100/100/5;11/True/NotSet/70/100/5;12/True/Fill/583/200/5;",
                                     "PriceAnalysis","byStation_ColumnSettings"             ,"0/False/NotSet/100/100/5;1/True/Fill/174/100/5;2/True/NotSet/50/100/5;3/True/NotSet/70/100/5;4/True/NotSet/70/100/5;5/True/NotSet/50/100/5;6/True/NotSet/70/100/5;7/True/NotSet/70/100/5;8/True/NotSet/70/100/5;9/True/NotSet/50/100/5;10/True/NotSet/50/100/5;11/True/NotSet/50/100/5;12/True/Fill/523/300/5;",
                                     "PriceAnalysis","Station1_ColumnSettings"              ,"0/False/NotSet/40/100/5;1/False/NotSet/100/100/5;2/True/Fill/114/184/5;3/True/Fill/34/55/5;4/True/Fill/34/55/5;5/True/Fill/34/54/5;6/True/Fill/41/67/5;7/True/Fill/34/55/5;8/True/Fill/35/55/5;9/True/Fill/33/54/5;10/True/NotSet/68/100/5;11/True/NotSet/53/100/5;12/True/NotSet/60/100/5;",
                                     "PriceAnalysis","Station2_ColumnSettings"              ,"0/False/NotSet/40/100/5;1/False/NotSet/100/100/5;2/True/Fill/114/184/5;3/True/Fill/34/55/5;4/True/Fill/34/55/5;5/True/Fill/34/54/5;6/True/Fill/41/67/5;7/True/Fill/34/55/5;8/True/Fill/35/55/5;9/True/Fill/33/54/5;10/True/NotSet/68/100/5;11/True/NotSet/53/100/5;12/True/NotSet/60/100/5;",
                                     "PriceAnalysis","StationToStationRoutes_ColumnSettings","0/False/NotSet/100/100/5;1/True/Fill/78/100/5;2/False/NotSet/100/100/5;3/True/Fill/77/100/5;4/True/NotSet/55/100/5;5/False/NotSet/53/100/5;6/True/NotSet/40/100/5;7/False/NotSet/100/100/5;8/True/Fill/78/100/5;9/False/NotSet/100/100/5;10/True/Fill/77/100/5;11/True/NotSet/53/100/5;12/False/NotSet/53/100/5;13/True/NotSet/40/100/5;14/True/NotSet/54/100/5;15/True/NotSet/53/100/5;16/True/NotSet/60/100/5;"};

                for (int i = 0; i < settings.GetUpperBound(0); i+=3)
                {
                    if(overwrite || String.IsNullOrEmpty(Program.DBCon.getIniValue(settings[i], settings[i+1], "")))
                    {
                        Program.DBCon.setIniValue(settings[i], settings[i+1], settings[i+2]);
                    }

                }

                
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting grid defaults", ex);
            }
        }

#endregion


    }
}
