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
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise
{
	internal static class ApplicationContext
	{
		public const string LOGS_PATH = "Logs";

		static ApplicationContext()
		{
			Trace.UseGlobalLock = false;
			Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(LOGS_PATH, "RegulatedNoise-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")+ "-" + Guid.NewGuid()+".log")) { Name = "RegulatedNoise" });
		}

		private static RegulatedNoiseSettings _settings;
		public static RegulatedNoiseSettings RegulatedNoiseSettings
		{
			get
			{
				if (_settings == null)
				{
					_settings = RegulatedNoiseSettings.LoadSettings();
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
					_milkyway.ImportSystemLocations();
				}
				return _milkyway;
			}
		}
	}
}