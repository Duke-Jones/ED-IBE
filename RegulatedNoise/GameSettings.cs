using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RegulatedNoise
{
    //GameSettings class interfaces with the actual Game configuration files.
    //Note only needed functions and properties are loaded.
    
    public class GameSettings
    {
        public AppConfig AppConfig;
        public EDDisplayConfig Display;
        
        public GameSettings()
        {
            //Load DisplaySettings from AppData
            loadDisplaySettings();

            //Load AppConfig
            loadAppConfig();

            //Set up some filewatchers, If user changes config its reflected here
            watcherDisplaySettings();
            watcherAppDataSettings(); //Currently disabled as we only check Verbose logging and that cant be changed from the game

            //Check and Request for Verbose Logging
            checkAndRequestVerboseLogging();
            /*watcher.Path = @"C:\Program Files (x86)\Frontier";
            watcher.Filter = ".";
            watcher.NotifyFilter = NotifyFilters.LastAccess |
                         NotifyFilters.LastWrite |
                         NotifyFilters.FileName |
                         NotifyFilters.DirectoryName;
            watcher.IncludeSubdirectories = true;

            watcher.Changed += new FileSystemEventHandler(OnChanged);

            watcher.EnableRaisingEvents = true;*/
/*            */
        }

        void checkAndRequestVerboseLogging()
        {
            if (AppConfig.Network.VerboseLogging != 1)
            {
                var setLog =
                    MessageBox.Show(
                        "Verbose logging isn't set in your Elite Dangerous AppConfig.xml, so I can't read system names. Would you like me to set it for you?",
                        "Set verbose logging?", MessageBoxButtons.YesNo);

                if (setLog == DialogResult.Yes)
                {
                    var appconfig = Path.Combine(Form1.RegulatedNoiseSettings.GamePath, "AppConfig.xml");

                    //Make backup
                    File.Copy(appconfig, appconfig+".bak", true);

                    //Set werbose to one
                    var doc = new XmlDocument();
                    doc.Load(appconfig);
                    var ie = doc.SelectNodes("/AppConfig/Network").GetEnumerator();

                    bool exist = false;
                    while (ie.MoveNext())
                    {
                        if ((ie.Current as XmlNode).Attributes["VerboseLogging"] != null)
                        {
                            (ie.Current as XmlNode).Attributes["VerboseLogging"].Value = "1";
                        }
                        else
                        {
                            var verb = doc.CreateAttribute("VerboseLogging");
                            verb.Value = "1";

                            (ie.Current as XmlNode).Attributes.Append(verb);
                        }
                    }

                    doc.Save(appconfig);

                    MessageBox.Show(
                        "AppConfig.xml updated.  You'll need to restart Elite Dangerous if it's already running.");
                }

                //Update config
                loadAppConfig();
            }
        }

        void loadAppConfig()
        {
            var configFile = Path.Combine(Form1.RegulatedNoiseSettings.GamePath, "AppConfig.xml");
            var serializer = new XmlSerializer(typeof (AppConfig));
            using (var myFileStream = new FileStream(configFile, FileMode.Open))
            {
                AppConfig = (AppConfig) serializer.Deserialize(myFileStream);    
            }
        }

        void loadDisplaySettings()
        {
            var configFile = Path.Combine(Form1.RegulatedNoiseSettings.ProductAppData, "Graphics" ,"DisplaySettings.xml");
            var serializer = new XmlSerializer(typeof(EDDisplayConfig));
            using (var myFileStream = new FileStream(configFile, FileMode.Open))
            {
                Display = (EDDisplayConfig)serializer.Deserialize(myFileStream);
            }
        }
       
        private readonly FileSystemWatcher displayWatcher = new FileSystemWatcher();
        void watcherDisplaySettings()
        {
            displayWatcher.Path = Path.Combine(Form1.RegulatedNoiseSettings.ProductAppData, "Graphics");
            displayWatcher.Filter = "DisplaySettings.xml";
            displayWatcher.NotifyFilter = NotifyFilters.LastWrite;
            displayWatcher.Changed += new FileSystemEventHandler(loadDisplaySettings);
            displayWatcher.EnableRaisingEvents = true;
        }
        private readonly FileSystemWatcher appdataWatcher = new FileSystemWatcher();
        void watcherAppDataSettings()
        {
            appdataWatcher.Path = Form1.RegulatedNoiseSettings.GamePath;
            appdataWatcher.Filter = "AppConfig.xml";
            appdataWatcher.NotifyFilter = NotifyFilters.LastWrite;
            appdataWatcher.Changed += new FileSystemEventHandler(loadAppConfig);
            appdataWatcher.EnableRaisingEvents = false; //Set to TRUE to enable watching!
        }
        private void loadAppConfig(object sender, FileSystemEventArgs e)
        {
            loadAppConfig();
        }
        private void loadDisplaySettings(object sender, FileSystemEventArgs e)
        {
            loadDisplaySettings();
        }
    }

    [System.Xml.Serialization.XmlRoot("AppConfig")]
    public class AppConfig
    {
         
        public EDNetwork Network { get; set; }
    }

    public class EDNetwork
    {
        [XmlAttribute("VerboseLogging")]
        public int VerboseLogging { get; set; }
    }

     [System.Xml.Serialization.XmlRoot("DisplayConfig")]
    public class EDDisplayConfig
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int FullScreen { get; set; }
    }
}
