using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegulatedNoise.EDDB_Data;
using System.Data;

namespace RegulatedNoise
{
    /// <summary>
    /// class for holding the current state/location of the commander/ship
    /// </summary>
    public class Condition
    {
        public const String        DB_GROUPNAME                    = "Condition";

        public String System
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, "CurrentSystem", "");
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, "CurrentSystem", value);
            }
        }

        public String Location
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, "CurrentStation", "");
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, "CurrentStation", value);
            }
        }

        /// <summary>
        /// returns the id of the system if existing
        /// </summary>
        public int? System_ID
        {
            get
            {
                String sqlString    = "select ID from tbSystems where Systemname = " + SQL.DBConnector.SQLAString(this.System);
                DataTable Data      = new DataTable();
                int? retValue       = null;

                if(Program.DBCon.Execute(sqlString, Data) > 0)
                    retValue = (Int32)(Data.Rows[0]["ID"]);

                return retValue;
            }
        }

        /// <summary>
        /// returns the id of the station if existing
        /// </summary>
        public int? Location_ID
        {
            get
            {
                String sqlString    = "select St.ID from tbSystems Sy, tbStations St" +
                                      " where Sy.ID = St. System_ID" +
                                      " and   Sy.Systemname  = " + SQL.DBConnector.SQLAString(this.System) +
                                      " and   St.Stationname = " + SQL.DBConnector.SQLAString(this.Location);

                DataTable Data      = new DataTable();
                int? retValue       = null;

                if(Program.DBCon.Execute(sqlString, Data) > 0)
                    retValue = (Int32)(Data.Rows[0]["ID"]);

                return retValue;
            }
        }

    }
}
