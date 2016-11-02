using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Data;
using IBE.Enums_and_Utility_Classes;
using Newtonsoft.Json.Linq;

namespace IBE
{
    /// <summary>
    /// class for holding the current state/location of the commander/ship
    /// </summary>
    public class Condition
    {
        private Point3Dbl                           _Coordinates;
        public const String                         DB_GROUPNAME                     = "Condition";
        private const string                        STR_CurrentSystem_ID             = "CurrentSystem";
        private const string                        STR_CurrentStation_ID            = "CurrentStation";
        private const string                        STR_CurrentSystemCoords          = "CurrentCoordinates";
        private const string                        STR_CurrentLocation              = "CurrentLocation";
        private const string                        STR_CurrentLocationType          = "CurrentLocationType";
        private MemoryCache                         m_DataCache                      = MemoryCache.Default;
        private FileScanner.EDJournalScanner        m_JournalScanner = null;

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
        /// the actual station (if existing)
        /// </summary>
        public String Station
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
        /// the actual location (if existing)
        /// </summary>
        public String Body
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, STR_CurrentLocation, "");
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, STR_CurrentLocation, value);
            }
        }

        /// <summary>
        /// the actual location type (if existing)
        /// </summary>
        public String BodyType
        {
            get
            {
                return Program.DBCon.getIniValue(DB_GROUPNAME, STR_CurrentLocationType, "");
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, STR_CurrentLocationType, value);
            }
        }

        /// <summary>
        /// the actual system coordinates in space
        /// </summary>
        public Point3Dbl Coordinates
        {
            get
            {
                Point3Dbl retValue = new Point3Dbl();
                Point3Dbl.TryParse(Program.DBCon.getIniValue(DB_GROUPNAME, STR_CurrentSystemCoords, ""), out retValue);

                return retValue;
            }
            set
            {
                Program.DBCon.setIniValue(DB_GROUPNAME, STR_CurrentSystemCoords, value.ToString());
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
        /// returns the id of the actual station if existing
        /// </summary>
        public int? Station_ID
        {
            get
            {
                int? retValue       = (int?)m_DataCache.Get(STR_CurrentStation_ID);

                if(retValue == null)
                { 
                    String sqlString    = "select St.ID from tbSystems Sy, tbStations St" +
                                          " where Sy.ID = St. System_ID" +
                                          " and   Sy.Systemname  = " + SQL.DBConnector.SQLAEscape(this.System) +
                                          " and   St.Stationname = " + SQL.DBConnector.SQLAEscape(this.Station);

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

        public void RegisterJournalScanner(FileScanner.EDJournalScanner journalScanner)
        {
            try
            {
                if(m_JournalScanner == null)
                { 
                    m_JournalScanner = journalScanner;
                    m_JournalScanner.JournalEventRecieved  += JournalEventRecieved;
                    m_JournalScanner.BasedataEventRecieved += BasedataEventRecieved;
                }
                else 
                    throw new Exception("LogfileScanner already registered");

            }
            catch (Exception ex)
            {
                throw new Exception("Error while registering the LogfileScanner", ex);
            }
        }

        /// <summary>
        /// event-worker for JournalEventRecieved-event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void JournalEventRecieved(object sender, FileScanner.EDJournalScanner.JournalEventArgs e)
        {
            try
            {
                switch (e.EventType)
                {
                    case FileScanner.EDJournalScanner.JournalEvent.Fileheader:
                        JournalHeaderObject = (JObject)e.Data;
                        break;
                }

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while processing the JournalEventRecieved-event");
            }
        }

        /// <summary>
        /// event-worker for BasedataEventRecieved-event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BasedataEventRecieved(object sender, FileScanner.EDJournalScanner.BasedataEventArgs e)
        {
            try
            {
                switch (e.EventType)
                {
                    case FileScanner.EDJournalScanner.JournalEvent.Basedata:
                        Program.Data.checkPotentiallyNewSystemOrStation(e.System, e.Station, e.Coordinates);
                        break;
                }

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while processing the JournalEventRecieved-event");
            }
        }

        public JObject JournalHeaderObject { get; set; }
        public JObject SupercruiseExitObject { get; set; }


        /// <summary>
        /// returns if the active E:D version is a beta or not
        /// </summary>
        public Boolean GameversionIsBeta
        {
            get
            {
                if((JournalHeaderObject != null) && (JournalHeaderObject.Values("gameversion") != null))
                {
                    // now we're sure we have a object
                    return JournalHeaderObject.SelectToken("gameversion").ToString().ToLower().Contains("beta");
                }
                else
                {
                    // we have no object  -> moral issue, decided for "no" beta
                    return false;
                }
            }
        }

        

    }
}
