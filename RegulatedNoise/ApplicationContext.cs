#region file header
// ////////////////////////////////////////////////////////////////////
// ///
// ///  
// /// 06.05.2015
// ///
// ///
// ////////////////////////////////////////////////////////////////////
#endregion

using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise
{
	internal static class ApplicationContext
	{

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