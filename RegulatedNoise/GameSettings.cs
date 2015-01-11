using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace RegulatedNoise
{
    //GameSettings class interfaces with the actual Game configuration files.
    //Note only needed functions and properties are loaded.
    
    public class GameSettings
    {
        public AppConfig AppConfig;
        public EdDisplayConfig Display;
        
        public GameSettings()
        {
            //Load DisplaySettings from AppData
            LoadDisplaySettings();

            //Load AppConfig
            LoadAppConfig();

            //Set up some filewatchers, If user changes config its reflected here
            WatcherDisplaySettings();
            WatcherAppDataSettings(); //Currently disabled as we only check Verbose logging and that cant be changed from the game

            //Check and Request for Verbose Logging
            CheckAndRequestVerboseLogging();
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

        void CheckAndRequestVerboseLogging()
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
                LoadAppConfig();
            }
        }

        void LoadAppConfig()
        {
            var configFile = Path.Combine(Form1.RegulatedNoiseSettings.GamePath, "AppConfig.xml");
            var serializer = new XmlSerializer(typeof (AppConfig));
            using (var myFileStream = new FileStream(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                AppConfig = (AppConfig) serializer.Deserialize(myFileStream);    
            }
        }

        private void LoadAppConfig(object sender, FileSystemEventArgs e)
        {
            LoadAppConfig();
        }

        void LoadDisplaySettings()
        {
            var configFile = Path.Combine(Form1.RegulatedNoiseSettings.ProductAppData, "Graphics" ,"DisplaySettings.xml");
            var serializer = new XmlSerializer(typeof(EdDisplayConfig));
            using (var myFileStream = new FileStream(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Display = (EdDisplayConfig)serializer.Deserialize(myFileStream);
            }
        }

        private void LoadDisplaySettings(object sender, FileSystemEventArgs e)
        {
            LoadDisplaySettings();
        }

        private readonly FileSystemWatcher _displayWatcher = new FileSystemWatcher();
        void WatcherDisplaySettings()
        {
            _displayWatcher.Path = Path.Combine(Form1.RegulatedNoiseSettings.ProductAppData, "Graphics");
            _displayWatcher.Filter = "DisplaySettings.xml";
            _displayWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _displayWatcher.Changed += LoadDisplaySettings;
            _displayWatcher.EnableRaisingEvents = true;
        }

        private readonly FileSystemWatcher _appdataWatcher = new FileSystemWatcher();
        void WatcherAppDataSettings()
        {
            _appdataWatcher.Path = Form1.RegulatedNoiseSettings.GamePath;
            _appdataWatcher.Filter = "AppConfig.xml";
            _appdataWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _appdataWatcher.Changed += LoadAppConfig;
            _appdataWatcher.EnableRaisingEvents = false; //Set to TRUE to enable watching!
        }

    }
}
