using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using IBE.Enums_and_Utility_Classes;

namespace IBE.EDDN
{
    class EDDNDuplicateFilter : IDisposable
    {
        private Dictionary<String, DateTime>              m_RecievedData;
        private SortedDictionary<DateTime, List<String>>  m_RecievedDataTimes;
        private System.Timers.Timer                       m_Releaser;
        private readonly object                           LockObject = new object();

        public void Dispose()
        {
            if (m_Releaser != null)
            {
                m_Releaser.Dispose();
                m_Releaser = null;
            }
        }

        public EDDNDuplicateFilter()
        {
            m_RecievedData          = new Dictionary<string,DateTime>(); 
            m_RecievedDataTimes     = new SortedDictionary<DateTime,List<string>>();

            m_Releaser              = new System.Timers.Timer();
            m_Releaser.Elapsed     += m_Releaser_Elapsed;
            m_Releaser.AutoReset    = false;
            m_Releaser.Interval     = 10000;
            m_Releaser.Start();
        }

        /// <summary>
        /// checks if the data is allowed to be imported.
        /// returns "false", if the data is to old (> 5 mins) or comes from future times (> 5 mins) or 
        /// is already recieved (e.g. from another relay or another user or simply "double sended")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public bool DataAccepted(string dataRow)
        {
            try
            {
                var converted = new CsvRow(dataRow);

                return DataAccepted(converted.SystemName, converted.StationName, converted.CommodityName, converted.SampleDate);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking EDDN data (dataRow)", ex);
            }
        }

        /// <summary>
        /// checks if the data is allowed to be imported.
        /// returns "false", if the data is to old (> 5 mins) or comes from future times (> 5 mins) or 
        /// is already recieved (e.g. from another relay or another user or simply "double sended")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public Boolean DataAccepted(String system, String station, String commodity, DateTime sampleDate)
        {
            String id = string.Format("{0}|{1}|{2}", system.ToUpper(), station.ToUpper(), commodity.ToUpper());
            Boolean retValue = true;
            DateTime limit1 = (DateTime.Now + new TimeSpan(0,5,0)).Truncate(TimeSpan.FromSeconds(1));
            DateTime limit2 = (DateTime.Now - new TimeSpan(0,5,0)).Truncate(TimeSpan.FromSeconds(1));

            lock(LockObject)
            {
                try
                {
                    // truncate milliseconds
                    sampleDate = sampleDate.Truncate(TimeSpan.FromSeconds(1));

                    if ((sampleDate <= limit1) && (sampleDate >= limit2))
                    { 
                        if(!m_RecievedData.ContainsKey(id))
                        { 
                            List<String> idList;
                            // save the new id
                            m_RecievedData.Add(id, sampleDate);

                            if(!m_RecievedDataTimes.TryGetValue(sampleDate, out idList))
                            {
                                // save the timestamp, if not existing yet and add id 
                                m_RecievedDataTimes.Add(sampleDate, new List<String>() {id});
                            }
                            else
                            {
                                // add new id to timestamp
                                idList.Add(id);
                            }
                        }
                        else
                        {
                            // already recieved
                            retValue = false;
                        }
                    }
                    else
                    {
                        // data is too old or comes from future times
                        retValue = false;
                    }

                    return retValue;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while checking EDDN data", ex);
                }
            }
        }

        /// <summary>
        /// Cleans up the collected filter-data by it's timestamp.
        /// If the timestamp is old enough, the ids will be removed from the 
        /// filter-list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Releaser_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock(LockObject)
            {
                try
                {
                    List<DateTime> removeStamps = new List<DateTime>();
                    DateTime limit = (DateTime.Now - new TimeSpan(0,5,0)).Truncate(TimeSpan.FromSeconds(1));

                    for (int i = 0; i < m_RecievedDataTimes.Count; i++)
			        {
                        Debug.Print("key = " + m_RecievedDataTimes.ElementAt(i).Key.ToString());
                        if (m_RecievedDataTimes.ElementAt(i).Key < limit)
                        {
                            // remove the ids of this timestamp
                            foreach (String id in m_RecievedDataTimes.ElementAt(i).Value)
                                m_RecievedData.Remove(id);

                            // mark this timestamp for remove
                            removeStamps.Add(m_RecievedDataTimes.ElementAt(i).Key);
                        }
                        else
                            break;
			        }

                    // remove the marked timestamps
                    foreach (var currentStamp in removeStamps)
                        m_RecievedDataTimes.Remove(currentStamp);    
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while cleaning EDDN filter", ex);
                }
            }
                
            m_Releaser.Start();
        }
    }
}
