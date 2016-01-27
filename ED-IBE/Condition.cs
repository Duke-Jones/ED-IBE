using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBE.EDDB_Data;
using System.Data;

namespace IBE
{
    /// <summary>
    /// class for holding the current state/location of the commander/ship
    /// </summary>
    public class Condition
    {
        public const String        DB_GROUPNAME                    = "Condition";

        /// <summary>
        /// the actual system
        /// </summary>
        public String System
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, "CurrentSystem", "");
            }
            set
            {
                if(Program.DBCon.setIniValue(DB_GROUPNAME, "CurrentSystem", value) &&
                  (!String.IsNullOrEmpty(value)))
                    Program.Data.checkPotentiallyNewSystemOrStation(value, "");
            }
        }

        /// <summary>
        /// the actual location (if existing)
        /// </summary>
        public String Location
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, "CurrentStation", "");
            }
            set
            {
                if(Program.DBCon.setIniValue(DB_GROUPNAME, "CurrentStation", value) &&
                  (!String.IsNullOrEmpty(value)))
                    Program.Data.checkPotentiallyNewSystemOrStation(System, value);
            }
        }

        /// <summary>
        /// returns the id of the actual system if existing
        /// </summary>
        public int? System_ID
        {
            get
            {
                String sqlString    = "select ID from tbSystems where Systemname = " + SQL.DBConnector.SQLAEscape(this.System);
                DataTable Data      = new DataTable();
                int? retValue       = null;

                if(Program.DBCon.Execute(sqlString, Data) > 0)
                    retValue = (Int32)(Data.Rows[0]["ID"]);

                return retValue;
            }
        }

        /// <summary>
        /// returns the id of the actual location if existing
        /// </summary>
        public int? Location_ID
        {
            get
            {
                String sqlString    = "select St.ID from tbSystems Sy, tbStations St" +
                                      " where Sy.ID = St. System_ID" +
                                      " and   Sy.Systemname  = " + SQL.DBConnector.SQLAEscape(this.System) +
                                      " and   St.Stationname = " + SQL.DBConnector.SQLAEscape(this.Location);

                DataTable Data      = new DataTable();
                int? retValue       = null;

                if(Program.DBCon.Execute(sqlString, Data) > 0)
                    retValue = (Int32)(Data.Rows[0]["ID"]);

                return retValue;
            }
        }

    }
}
