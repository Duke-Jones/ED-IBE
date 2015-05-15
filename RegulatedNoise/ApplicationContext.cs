#region file header
// ////////////////////////////////////////////////////////////////////
// ///
// ///  
// /// 06.05.2015
// ///
// ///
// ////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Diagnostics;
using System.IO;
using RegulatedNoise.DomainModel;
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.EliteInteractions;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
    internal static class ApplicationContext
    {
        public const string LOGS_PATH = "Logs";

        static ApplicationContext()
        {
            Trace.UseGlobalLock = false;
            Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(LOGS_PATH, "RegulatedNoise-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-" + Guid.NewGuid() + ".log")) { Name = "RegulatedNoise" });
            Trace.AutoFlush = true;
            Trace.TraceInformation("Application context set up");
        }

        private static RegulatedNoiseSettings _settings;
        public static RegulatedNoiseSettings RegulatedNoiseSettings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = RegulatedNoiseSettings.LoadSettings();
                    _settings.PropertyChanged += (sender, args) => _settings.Save();
                }
                return _settings;
            }
        }

        private static EDMilkyway _milkyway;
        private static dsCommodities _commoditiesLocalisation;
        private static EDDN _eddn;
        private static GalacticMarket _galacticMarket;
        private static LogFilesScanner _eliteLogFilesScanner;

        public static EDMilkyway Milkyway
        {
            get
            {
                if (_milkyway == null)
                {
                    _milkyway = new EDMilkyway();
                    _milkyway.ImportSystemLocations();
                    Trace.TraceInformation("  - system locations imported");
                }
                return _milkyway;
            }
        }

        /// <summary>
        /// Checks whether a control or its parent is in design mode.
        /// </summary>
        /// <param name="c">The control to check.</param>
        /// <returns>Returns TRUE if in design mode, false otherwise.</returns>
        public static bool IsDesignMode(System.Windows.Forms.Control c )
        {
          if ( c == null )
          {
            return false;
          }
          else
          {
            while ( c != null )
            {
              if ( c.Site != null && c.Site.DesignMode )
              {
                return true;
              }
              else
              {
                c = c.Parent;
              }
            }
 
            return false;
          }
        }

        public static dsCommodities CommoditiesLocalisation
        {
            get
            {
                if (_commoditiesLocalisation == null)
                {
                    _commoditiesLocalisation = new dsCommodities();
                    _commoditiesLocalisation.ReadXml(RegulatedNoiseSettings.COMMODITIES_LOCALISATION_FILEPATH);
                }
                return _commoditiesLocalisation;
            }
        }

        public static EDDN Eddn
        {
            get
            {
                if(_eddn == null)
                {
                    EventBus.InitializationStart("prepare EDDN interface");
                    _eddn = new EDDN(ApplicationContext.CommoditiesLocalisation, ApplicationContext.RegulatedNoiseSettings);
                    Trace.TraceInformation("  - EDDN object created");
                    if (RegulatedNoiseSettings.StartListeningEddnOnLoad)
                    {
                        EventBus.InitializationStart("subscribing to EDDN");
                        Eddn.Subscribe();
                        EventBus.InitializationCompleted("now listening to EDDN");
                    }
                    EventBus.InitializationCompleted("prepare EDDN interface");

                }
                return _eddn;
            }
        }

        public static GalacticMarket GalacticMarket
        {
            get
            {
                if (_galacticMarket == null)
                    _galacticMarket = new GalacticMarket();
                return _galacticMarket;
            }
            set { _galacticMarket = value; }
        }

        public static LogFilesScanner EliteLogFilesScanner
        {
            get
            {
                if (_eliteLogFilesScanner == null)
                {
                    _eliteLogFilesScanner = new LogFilesScanner(RegulatedNoiseSettings);
                }
                return _eliteLogFilesScanner;
            }
            set { _eliteLogFilesScanner = value; }
        }
    }
}