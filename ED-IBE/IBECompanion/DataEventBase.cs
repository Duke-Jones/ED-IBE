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
            }

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
        /// collects the marketdata from a external tool
        /// </summary>
        public Int32 ImportMarketData()
        {
            Int32 DataCount = 0;

            try
            {
                DataCount = ImportPrices();

                // something has changed -> fire event
                var EA = new LocationChangedEventArgs() { Changed       = enExternalDataEvents.DataCollected,
                                                          Amount        = DataCount};
                ExternalDataEvent.Raise(this, EA);

                return DataCount;
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
