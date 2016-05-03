using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBE.Enums_and_Utility_Classes;

namespace IBE.IBECompanion
{
    public class DataEventBase
    {
        #region LogEvents

        [Flags] public enum enExternalDataEvents
        {
            None         = 0,
            System          = 1,
            Location        = 2,
            Jump            = 4,
            Landed          = 8,
            DataCollected   = 16
        }

        public enum enExternalDataFunction
        {
            getLocation     = 0,
            getMarketdata   = 1,
        }

        public class ExternalDataEventData
        {
            public enExternalDataEvents EventType    { get; set; }
            public String               Value        { get; set; }
            public DateTime             Time         { get; set; }
        }



        #endregion

        #region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<LocationChangedEventArgs> ExternalDataEvent;

        protected virtual void OnLocationChanged(LocationChangedEventArgs e)
        {
            EventHandler<LocationChangedEventArgs> myEvent = ExternalDataEvent;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class LocationChangedEventArgs : EventArgs
        {
            public LocationChangedEventArgs()
            {
                Changed     = enExternalDataEvents.None;
                System      = "";
                Location    = "";
                
            }

            public String System                    { get; set; }
            public String Location                  { get; set; }
            public String OldSystem                 { get; set; }
            public String OldLocation               { get; set; }
            public enExternalDataEvents Changed     { get; set; }
            public Int32 Amount                     { get; set; }
        }

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<LocationInfoEventArgs> LocationInfo;

        protected virtual void OnLocationInfo(LocationInfoEventArgs e)
        {
            EventHandler<LocationInfoEventArgs> myEvent = LocationInfo;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class LocationInfoEventArgs : EventArgs
        {
            public LocationInfoEventArgs()
            {
                System      = "";
                Location     = "";
            }

            public String System            { get; set; }
            public String Location          { get; set; }
        }

        #endregion

        /// <summary>
        /// confirms the last retrieved location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ConfirmLocation(String system, String station)
        {
            String OldSystemString          = "";
            String OldLocationString        = "";
            enExternalDataEvents ChangedIs  = enExternalDataEvents.None;

            try
            {
                if(!Program.actualCondition.System.Equals(system) || !Program.actualCondition.Location.Equals(station))
                {
                    OldSystemString     = Program.actualCondition.System;
                    OldLocationString   = Program.actualCondition.Location;

                    ChangedIs = enExternalDataEvents.Landed;

                    if(!Program.actualCondition.System.Equals(system))
                        ChangedIs |= enExternalDataEvents.System;

                    if(!Program.actualCondition.Location.Equals(station))
                        ChangedIs |= enExternalDataEvents.Location;
                }

                Program.actualCondition.System   = system;
                Program.actualCondition.Location = station;

                if(ChangedIs != enExternalDataEvents.None)
                { 
                    // something has changed -> fire events
                    var LI = new LocationInfoEventArgs() { System        = Program.actualCondition.System,  
                                                           Location      = Program.actualCondition.Location};
                    LocationInfo.Raise(this, LI);


                    var EA = new LocationChangedEventArgs() { System        = Program.actualCondition.System,  
                                                              Location      = Program.actualCondition.Location,
                                                              OldSystem     = OldSystemString,  
                                                              OldLocation   = OldLocationString,
                                                              Changed       = ChangedIs};
                    ExternalDataEvent.Raise(this, EA);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while confirming retrieved location", ex);
            }
        }

        /// <summary>
        /// collects the marketdata from a external tool
        /// </summary>
        public void getMarketData()
        {
            Int32 DataCount = 0;

            try
            {
                DataCount = ImportPrices();

                // something has changed -> fire event
                var EA = new LocationChangedEventArgs() { System        = Program.actualCondition.System,  
                                                          Location      = Program.actualCondition.Location,
                                                          OldSystem     = Program.actualCondition.System,  
                                                          OldLocation   = Program.actualCondition.Location,
                                                          Changed       = enExternalDataEvents.DataCollected,
                                                          Amount        = DataCount};
                ExternalDataEvent.Raise(this, EA);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while collecting market data from external tool", ex);
            }
        }

        protected virtual int ImportPrices()
        {
            throw new NotImplementedException();
        }
    }

}
