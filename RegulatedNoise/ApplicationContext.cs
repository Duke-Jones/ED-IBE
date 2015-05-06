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
using System.Xml;
using System.Xml.Serialization;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise
{
	internal static class ApplicationContext
	{
		public static event EventHandler<InitializationEventArgs> OnInitializationProgress;

		private static RegulatedNoiseSettings _settings;
		public static RegulatedNoiseSettings RegulatedNoiseSettings
		{
			get
			{
				if (_settings == null)
				{
					_settings = LoadSettings();
				}
				return _settings;
			}
		}

		private static EDMilkyway _milkyway;
		public static EDMilkyway Milkyway
		{
			get
			{
				if (_milkyway == null)
				{
					_milkyway = new EDMilkyway();
					_milkyway.OnInitializationProgress += (sender, args) => RaiseInitializationEvent(args.Message, args.Event);
					_milkyway.ImportSystemLocations();
				}
				return _milkyway;
			}
		}

		private static RegulatedNoiseSettings LoadSettings()
		{
			NotifyStart("load settings");
			var serializer = new XmlSerializer(typeof(RegulatedNoiseSettings));
			RegulatedNoiseSettings settings;

			if (File.Exists("RegulatedNoiseSettings.xml"))
			{
				var fs = new FileStream("RegulatedNoiseSettings.xml", FileMode.Open);
				var reader = XmlReader.Create(fs);
				try
				{
					settings = (RegulatedNoiseSettings)serializer.Deserialize(reader);
				}
				catch (Exception ex)
				{
					Trace.TraceError("Error loading settings: " + ex);
					NotifyInfo("Couldn't load settings; maybe they are from a previous version.  A new settings file will be created on exit.");
					settings = new RegulatedNoiseSettings();
				}
				fs.Close();
			}
			else
			{
				settings = new RegulatedNoiseSettings();
			}
			NotifyCompleted("load settings");
			return settings;
		}

		private static void NotifyStart(string message)
		{
			RaiseInitializationEvent(message + "...", InitializationEventArgs.EventType.Info);
		}

		private static void NotifyInfo(string message)
		{
			RaiseInitializationEvent(message, InitializationEventArgs.EventType.Info);
		}

		private static void NotifyCompleted(string message)
		{
			RaiseInitializationEvent("..." + message + "...<OK>", InitializationEventArgs.EventType.Update);
		}

		private static void RaiseInitializationEvent(string message, InitializationEventArgs.EventType eventType)
		{
			var handler = OnInitializationProgress;
			if (handler != null)
			{
				try
				{
					handler(null, new InitializationEventArgs(message, eventType));
				}
				catch (Exception ex)
				{
					Trace.TraceError("initialization progress failure: " + ex);
				}
			}
		}
	}
}