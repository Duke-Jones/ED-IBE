using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegulatedNoise.EDDB_Data;

namespace RegulatedNoise
{
    /// <summary>
    /// class for holding the current state/location of the commander/ship
    /// </summary>
    public class Condition
    {
        public const String        DB_GROUPNAME                    = "Condition";

        public const string STR_Scanning = "scanning...";

        public String System
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, "CurrentSystem", STR_Scanning, false);
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, "CurrentSystem", value);
            }
        }
        public String Station
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, "CurrentStation", STR_Scanning, false);
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, "CurrentStation", value);
            }
        }

    }
}
