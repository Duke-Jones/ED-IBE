using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Data;

namespace IBE
{
    /// <summary>
    /// class for holding the current state/location of the commander/ship
    /// </summary>
    public class Condition
    {
        public const String        DB_GROUPNAME                     = "Condition";
        private const string       STR_CurrentSystem_ID             = "CurrentSystem";
        private const string       STR_CurrentStation_ID            = "CurrentStation";
        private MemoryCache        m_DataCache                      = MemoryCache.Default;

        public Condition()
        {

        }

        /// <summary>
        /// the actual system
        /// </summary>
        public String System
        {
            get
            {
            return Program.DBCon.getIniValue(DB_GROUPNAME, STR_CurrentSystem_ID, "");
            }
            set
            {
                if(Program.DBCon.setIniValue(DB_GROUPNAME, STR_CurrentSystem_ID, value) &&
                  (!String.IsNullOrEmpty(value)))
                    Program.Data.checkPotentiallyNewSystemOrStation(value, "");

                m_DataCache.Remove(STR_CurrentSystem_ID);
            }
        }

        /// <summary>
        /// the actual location (if existing)
        /// </summary>
        public String Location
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, STR_CurrentStation_ID, "");
            }
            set
            {
                if(Program.DBCon.setIniValue(DB_GROUPNAME, STR_CurrentStation_ID, value) &&
                  (!String.IsNullOrEmpty(value)))
                    Program.Data.checkPotentiallyNewSystemOrStation(System, value);

                m_DataCache.Remove(STR_CurrentStation_ID);
            }
        }

        /// <summary>
        /// returns the id of the actual system if existing
        /// </summary>
        public int? System_ID
        {
            get
            {
                int? retValue       = (int?)m_DataCache.Get(STR_CurrentSystem_ID);;

                if(retValue == null)
                { 
                    String sqlString    = "select ID from tbSystems where Systemname = " + SQL.DBConnector.SQLAEscape(this.System);
                    DataTable Data      = new DataTable();
                

                    if(Program.DBCon.Execute(sqlString, Data) > 0)
                        retValue = (Int32)(Data.Rows[0]["ID"]);

                    
                    if(retValue == null)
                        m_DataCache.Set(STR_CurrentSystem_ID, 0, DateTimeOffset.Now.AddSeconds(10));
                    else
                        m_DataCache.Set(STR_CurrentSystem_ID, retValue, DateTimeOffset.Now.AddSeconds(10));
                }
                else if(retValue == 0)
                {
                    retValue = null;
                }

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
                int? retValue       = (int?)m_DataCache.Get(STR_CurrentStation_ID);

                if(retValue == null)
                { 
                    String sqlString    = "select St.ID from tbSystems Sy, tbStations St" +
                                          " where Sy.ID = St. System_ID" +
                                          " and   Sy.Systemname  = " + SQL.DBConnector.SQLAEscape(this.System) +
                                          " and   St.Stationname = " + SQL.DBConnector.SQLAEscape(this.Location);

                    DataTable Data      = new DataTable();

                    if(Program.DBCon.Execute(sqlString, Data) > 0)
                        retValue = (Int32)(Data.Rows[0]["ID"]);

                    if(retValue == null)
                        m_DataCache.Set(STR_CurrentStation_ID, 0, DateTimeOffset.Now.AddSeconds(10));
                    else
                        m_DataCache.Set(STR_CurrentStation_ID, retValue, DateTimeOffset.Now.AddSeconds(10));
                }
                else if(retValue == 0)
                {
                    retValue = null;
                }

                return retValue;
            }
        }

    }
}
