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
	}

}