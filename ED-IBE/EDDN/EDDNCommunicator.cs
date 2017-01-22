using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Threading;
using ZeroMQ;
using IBE.Enums_and_Utility_Classes;
using System.Globalization;
using System.Net;
using CodeProject.Dialog;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Text;

namespace IBE.EDDN
{

    public class EDDNCommunicator : IDisposable
    {

        private enum enSchema
        {
            Real = 0,
            Test = 1
        }

        public enum enInterface
        {
            API = 0,
            OCR = 1
        }

#region dispose region

        // Flag: Has Dispose already been called?
        private bool m_SenderIsActivated;
bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();

                // Free any other managed objects here.
                StopEDDNListening();

                if (m_DuplicateFilter != null)
                    m_DuplicateFilter.Dispose(); 

                m_DuplicateFilter = null;

                if (m_DuplicateRelayFilter != null)
                    m_DuplicateRelayFilter.Dispose(); 

                m_lDBCon.Dispose();

                m_DuplicateRelayFilter = null;
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

#endregion

#region event handler

        [System.ComponentModel.Browsable(true)]
        public event EventHandler<DataChangedEventArgs> DataChangedEvent;

        protected virtual void OnLocationChanged(DataChangedEventArgs e)
        {
            EventHandler<DataChangedEventArgs> myEvent = DataChangedEvent;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class DataChangedEventArgs : EventArgs
        {
            public DataChangedEventArgs(enDataTypes dType)
            {
                DataType = dType;    
            }

            public enDataTypes DataType               { get; set; }
        }

        [Flags] public enum enDataTypes
        {
            NoChanges           =  0,
            RecieveData         =  1,
            ImplausibleData     =  2,
            Statistics          =  4,
            DataImported        =  8    
        }

        public enum enMessageTypes
        {
            unknown             =  0,
            Commodity_V1        =  1,
            Commodity_V2        =  2,
            Commodity_V3        =  3,
            Shipyard_V1         =  11,
            Shipyard_V2         =  12,
            Outfitting_V1       =  21,    
            Outfitting_V2       =  22    
        }


        [System.ComponentModel.Browsable(true)]
        public event EventHandler<DataTransmittedEventArgs> DataTransmittedEvent;

        protected virtual void OnLocationChanged(DataTransmittedEventArgs e)
        {
            EventHandler<DataTransmittedEventArgs> myEvent = DataTransmittedEvent;
            if (myEvent != null)
            {
                myEvent(this, e);
            }
        }

        public class DataTransmittedEventArgs : EventArgs
        {
            public DataTransmittedEventArgs(enTransmittedTypes tType, enTransmittedStates tState)
            {
                DataType  = tType;   
                DataState = tState;
 
            }

            public enTransmittedTypes  DataType               { get; set; }
            public enTransmittedStates DataState              { get; set; }
        }

        public enum enTransmittedTypes
        {
            Commodity_V3        =  0,
            Shipyard_V1         =  1,
            Outfitting_V2       =  2, 
            Journal_V1          =  3        
        }

        public enum enTransmittedStates
        {
            Sent        =  0,
            Error       =  1
        }
        
 #endregion

        private Thread                              _Spool2EDDN_Market;   
        private Thread                              _Spool2EDDN_Commodity;   
        private Thread                              _Spool2EDDN_Outfitting;   
        private Thread                              _Spool2EDDN_Shipyard;   
        private Thread                              _Spool2EDDN_Journal;   
        private Queue                               _Send_MarketData_API     = new Queue(100,10);
        private Queue                               _Send_MarketData_OCR     = new Queue(100,10);
        private Queue                               _Send_Commodity         = new Queue(100,10);
        private Queue                               _Send_Outfitting         = new Queue(100,10);
        private Queue                               _Send_Shipyard           = new Queue(100,10);
        private Queue                               _Send_Journal            = new Queue(100,10);
        private SingleThreadLogger                  _logger;
        private System.Timers.Timer                 _SendDelayTimer_Commodity;
        private System.Timers.Timer                 _SendDelayTimer_Market;
        private System.Timers.Timer                 _SendDelayTimer_Outfitting;
        private System.Timers.Timer                 _SendDelayTimer_Shipyard;
        private System.Timers.Timer                 _SendDelayTimer_Journal;
        private StreamWriter                        m_EDDNSpooler = null;
        private Dictionary<String, EDDNStatistics>  m_StatisticDataSW = new Dictionary<String, EDDNStatistics>();
        private Dictionary<String, EDDNStatistics>  m_StatisticDataRL = new Dictionary<String, EDDNStatistics>();
        private Dictionary<String, EDDNStatistics>  m_StatisticDataCM = new Dictionary<String, EDDNStatistics>();
        private Dictionary<String, EDDNStatistics>  m_StatisticDataMT = new Dictionary<String, EDDNStatistics>();
        private List<String>                        m_RejectedData;
        private List<String>                        m_RawData;
        private EDDNDuplicateFilter                 m_DuplicateFilter       = new EDDNDuplicateFilter(new TimeSpan(0,5,0));
        private EDDNDuplicateFilter                 m_DuplicateRelayFilter  = new EDDNDuplicateFilter(new TimeSpan(0,0,30));
        private Dictionary<String, EDDNReciever>    m_Reciever;
        private FileScanner.EDJournalScanner        m_JournalScanner = null;
        private List<String>                        m_Relays    = new List<string>() { "tcp://eddn-relay.elite-markets.net:9500", 
                                                                                       "tcp://eddn-relay.ed-td.space:9500"};

        private Tuple<String, DateTime>             m_ID_of_Commodity_Station = new Tuple<String, DateTime>("", new DateTime());
        private Tuple<String, DateTime>             m_ID_of_Outfitting_Station = new Tuple<String, DateTime>("", new DateTime());
        private Tuple<String, DateTime>             m_ID_of_Shipyard_Station = new Tuple<String, DateTime>("", new DateTime());
        private Tuple<String, DateTime>             m_ID_of_Journal_Station = new Tuple<String, DateTime>("", new DateTime());

        private Boolean                             m_CommoditySendingError { get; set; }
        private Boolean                             m_OutfittingSendingError { get; set; }
        private Boolean                             m_ShipyardSendingError { get; set; }
        private Boolean                             m_JournalSendingError { get; set; }
        private SQL.DBConnector                     m_lDBCon;


        public EDDNCommunicator()
        {
            m_lDBCon = new SQL.DBConnector(Program.DBCon.ConfigData, true);
            
            m_Reciever = new Dictionary<String, EDDNReciever>();

            _SendDelayTimer_Market                = new System.Timers.Timer(2000);
            _SendDelayTimer_Market.AutoReset      = false;
            _SendDelayTimer_Market.Elapsed        += new System.Timers.ElapsedEventHandler(this.SendDelayTimerMarket_Elapsed);

            _SendDelayTimer_Commodity             = new System.Timers.Timer(2000);
            _SendDelayTimer_Commodity.AutoReset   = false;
            _SendDelayTimer_Commodity.Elapsed     += new System.Timers.ElapsedEventHandler(this.SendDelayTimerCommodity_Elapsed);

            _SendDelayTimer_Outfitting            = new System.Timers.Timer(2000);
            _SendDelayTimer_Outfitting.AutoReset  = false;
            _SendDelayTimer_Outfitting.Elapsed    += new System.Timers.ElapsedEventHandler(this.SendDelayTimerOutfitting_Elapsed);

            _SendDelayTimer_Shipyard              = new System.Timers.Timer(2000);
            _SendDelayTimer_Shipyard.AutoReset    = false;
            _SendDelayTimer_Shipyard.Elapsed      += new System.Timers.ElapsedEventHandler(this.SendDelayTimerShipyard_Elapsed);

            _SendDelayTimer_Journal               = new System.Timers.Timer(2000);
            _SendDelayTimer_Journal.AutoReset     = false;
            _SendDelayTimer_Journal.Elapsed       += new System.Timers.ElapsedEventHandler(this.SendDelayTimerJournal_Elapsed);
                                                  
            _logger                     = new SingleThreadLogger(ThreadLoggerType.EddnSubscriber);

            m_RejectedData              = new List<String>();
            m_RawData                   = new List<String>();

            UserIdentification();
        }

#region receive

        /// <summary>
        /// starts EDDN-listening (if already started the EDDN-lister will be stopped and restarted)
        /// </summary>
        public void StartEDDNListening()
        {
            try
            {
                StopEDDNListening();

                foreach (String adress in m_Relays)
                {
                    var newReciever = new EDDNReciever(adress);
                    newReciever.StartListen();
                    newReciever.DataRecieved += RecievedEDDNData;

                    m_Reciever.Add(adress, newReciever);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error while starting the EDDN-Listener", ex);
            }
        }

        /// <summary>
        /// stops EDDN-listening if started
        /// </summary>
        public void StopEDDNListening()
        {
            try
            {
                foreach (var recieverKVP in m_Reciever)
                    recieverKVP.Value.Dispose();

                m_Reciever.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while stopping the EDDN-Listener", ex);
            }
        }

        /// <summary>
        /// processing of the recieved data
        /// </summary>                     
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecievedEDDNData(object sender, EDDN.EDDNRecievedArgs e)
        {
            String[] DataRows = new String[0];
            String nameAndVersion = String.Empty;
            String name = String.Empty;
            String uploaderID = String.Empty;
            Boolean SimpleEDDNCheck = false;
            List<String> importData = new List<String>();
            enSchema ownSchema;
            enSchema dataSchema;
            bool isTrusty = false;

            try
            {

                if (m_lDBCon.getIniValue<Boolean>("EDDN", "SpoolEDDNToFile", false.ToString(), false))
                {
                    if (m_EDDNSpooler == null)
                    {
                        if (!File.Exists(Program.GetDataPath(@"Logs\EddnOutput.txt")))
                            m_EDDNSpooler = File.CreateText(Program.GetDataPath(@"Logs\EddnOutput.txt"));
                        else
                            m_EDDNSpooler = File.AppendText(Program.GetDataPath(@"Logs\EddnOutput.txt"));
                    }
                    m_EDDNSpooler.WriteLine(e.RawData);
                }

                ownSchema = m_lDBCon.getIniValue<enSchema>(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false);

                switch (e.InfoType)
                {

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Commodity_v1_Recieved:

                        UpdateStatisticDataMsg(enMessageTypes.Commodity_V1);
                        //Debug.Print("recieved commodity message ignored");
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));

                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Commodity_v2_Recieved:

                        UpdateStatisticDataMsg(enMessageTypes.Commodity_V2);
                        //Debug.Print("recieved commodity message ignored");
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));

                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Commodity_v3_Recieved:

                        JObject dataJObject = (JObject)e.Data;

                        if(m_DuplicateRelayFilter.DataAccepted(dataJObject.SelectToken("header.uploaderID").ToString(), dataJObject.SelectToken("message.systemName") + "|" + dataJObject.SelectToken("message.stationName"), 
                                              dataJObject.SelectToken("message.commodities").Count().ToString(), (DateTime)dataJObject.SelectToken("message.timestamp")))
                        { 
                            UpdateStatisticDataMsg(enMessageTypes.Commodity_V3);
                            UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));

                            // process only if it's the correct schema
                            dataSchema = dataJObject.SelectToken("$schemaRef").Contains("/test") ? enSchema.Test : enSchema.Real;

                            if (ownSchema == dataSchema)
                            {
                                //Debug.Print("handle v3 message");

                                // Don't import our own uploads...
                                if (dataJObject.SelectToken("header.uploaderID").ToString() != UserIdentification())
                                {
                                    DataRows = ConvertCommodityV3_To_CSVRows(dataJObject);
                                    nameAndVersion = String.Format("{0} / {1}", dataJObject.SelectToken("header.softwareName"), dataJObject.SelectToken("header.softwareVersion"));
                                    name = String.Format("{0}", dataJObject.SelectToken("header.softwareName"));
                                    uploaderID = dataJObject.SelectToken("header.uploaderID").ToString();

                                    if(name == "ED-IBE (API)")
                                        Debug.Print("handle v3 ^recieved : " + name);
                                }
                                //else
                                    //Debug.Print("handle v3 rejected (it's our own message)");
                            }
                            //else
                                //Debug.Print("handle v3 rejected (wrong schema)");
                        }
                        //else
                            //Debug.Print("handle v3 rejected (double recieved)");

                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Outfitting_v1_Recieved:
                        UpdateStatisticDataMsg(enMessageTypes.Outfitting_V1);
                        //UpdateRawData("recieved outfitting message ignored (coming feature)");
                        //Debug.Print("recieved outfitting message ignored");
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));
                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Outfitting_v2_Recieved:
                        UpdateStatisticDataMsg(enMessageTypes.Outfitting_V2);
                        //UpdateRawData("recieved outfitting message ignored (coming feature)");
                        //Debug.Print("recieved outfitting message ignored");
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));
                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Shipyard_v1_Recieved:
                        UpdateStatisticDataMsg(enMessageTypes.Shipyard_V1);
                        //UpdateRawData("recieved shipyard message ignored (coming feature)");
                        //Debug.Print("recieved shipyard message ignored");
                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Shipyard_v2_Recieved:
                        UpdateStatisticDataMsg(enMessageTypes.Shipyard_V2);
                        //UpdateRawData("recieved shipyard message ignored (coming feature)");
                        //Debug.Print("recieved shipyard message ignored");
                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.UnknownData:
                        UpdateStatisticDataMsg(enMessageTypes.unknown);
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));
                        UpdateRawData("Recieved a unknown EDDN message:" + Environment.NewLine + e.Message + Environment.NewLine + e.RawData);
                        //Debug.Print("handle unkown message");
                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.ParseError:
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));
                        Debug.Print("handle error message");
                        UpdateRawData("Error while processing recieved EDDN data:" + Environment.NewLine + e.Message + Environment.NewLine + e.RawData);

                        break;

                }

                if (DataRows != null && DataRows.GetUpperBound(0) >= 0)
                {
                    isTrusty = (Program.Data.BaseData.tbtrustedsenders.Rows.Find(name) != null);

                    UpdateStatisticData(DataRows.GetUpperBound(0) + 1, nameAndVersion, e.Adress, uploaderID);

                    foreach (String DataRow in DataRows)
                    {
                        if(m_DuplicateFilter.DataAccepted(DataRow))
                        {
                            

                            // data is plausible ?
                            if (isTrusty || (!Program.PlausibiltyCheck.CheckPricePlausibility(new string[] { DataRow }, SimpleEDDNCheck)))
                            {

                                // import is wanted ?
                                if (m_lDBCon.getIniValue<Boolean>("EDDN", "ImportEDDN", false.ToString(), false))
                                {
                                    // collect importable data
                                    Debug.Print("import :" + DataRow);
                                    importData.Add(DataRow);
                                }

                            }
                            else
                            {
                                Debug.Print("implausible :" + DataRow);
                                // data is implausible
                                string InfoString = string.Format("IMPLAUSIBLE DATA : \"{2}\" from {0}/ID=[{1}]", nameAndVersion, uploaderID, DataRow);

                                UpdateRejectedData(InfoString);

                                if (m_lDBCon.getIniValue<Boolean>("EDDN", "SpoolImplausibleToFile", false.ToString(), false))
                                {

                                    FileStream LogFileStream = null;
                                    string FileName = Program.GetDataPath(@"Logs\EddnImplausibleOutput.txt");

                                    if (File.Exists(FileName))
                                        LogFileStream = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                                    else
                                        LogFileStream = File.Create(FileName);

                                    LogFileStream.Write(System.Text.Encoding.Default.GetBytes(InfoString + "\n"), 0, System.Text.Encoding.Default.GetByteCount(InfoString + "\n"));
                                    LogFileStream.Close();
                                }
                            }
                        }
                    }

                    // have we collected importable data -> then import now
                    if (importData.Count() > 0)
                    {
                        Program.Data.ImportPricesFromCSVStrings(importData.ToArray(), SQL.EliteDBIO.enImportBehaviour.OnlyNewer, (isTrusty ? SQL.EliteDBIO.enDataSource.fromEDDN_T : SQL.EliteDBIO.enDataSource.fromEDDN));
                        DataChangedEvent.Raise(this, new DataChangedEventArgs(enDataTypes.DataImported));
                    }

                }
            }
            catch (Exception ex)
            {
                UpdateRawData("Error while processing recieved EDDN data:" + Environment.NewLine + ex.GetBaseException().Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// returns the number of active listeners
        /// </summary>
        public Int32 ListenersRunning
        {
            get
            {
                Int32 count = 0;

                foreach (var recieverKVP in m_Reciever)
                {
                    if(recieverKVP.Value.IsListening)
                        count++;
                }

                return count;
            }
        }

        /// <summary>
        /// updates the content of the "recieve" data
        /// </summary>
        /// <param name="info"></param>
        private void UpdateRawData(String info)
        {
            Int32 maxSize = 10;

            try
            {
                m_RawData.Add(info);

                if (m_RawData.Count() > maxSize)
                    m_RawData.RemoveRange(0, m_RawData.Count()-maxSize);

                DataChangedEvent.Raise(this, new DataChangedEventArgs(enDataTypes.RecieveData));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating recieve data", ex);
            }
        }

        /// <summary>
        /// updates the content of the "implausible" data
        /// </summary>
        /// <param name="info"></param>
        private void UpdateRejectedData(String info)
        {
            Int32 maxSize = 100;

            try
            {
                m_RejectedData.Add(info);

                if (m_RejectedData.Count() > maxSize)
                    m_RejectedData.RemoveRange(0, m_RejectedData.Count()-maxSize);

                DataChangedEvent.Raise(this, new DataChangedEventArgs(enDataTypes.ImplausibleData));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating implausible data", ex);
            }
        }

        /// <summary>
        /// refreshes the info about recieved messagetypes
        /// </summary>
        /// <param name="dataCount"></param>
        /// <param name="SoftwareID"></param>
        private void UpdateStatisticDataMsg(enMessageTypes mType)
        {
            try
            {
                if (!m_StatisticDataMT.ContainsKey(mType.ToString()))
                    m_StatisticDataMT.Add(mType.ToString(), new EDDNStatistics());

                m_StatisticDataMT[mType.ToString()].MessagesReceived += 1;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating statistics (mt)", ex);
            }
        }

        /// <summary>
        /// refreshes the info about software versions and recieved messages
        /// </summary>
        /// <param name="dataCount"></param>
        /// <param name="SoftwareID"></param>
        private void UpdateStatisticData(int dataCount,string SoftwareID, String relay, String commander)
        {
            try
            {
                if (!m_StatisticDataSW.ContainsKey(SoftwareID))
                    m_StatisticDataSW.Add(SoftwareID, new EDDNStatistics());

                m_StatisticDataSW[SoftwareID].MessagesReceived += 1;
                m_StatisticDataSW[SoftwareID].DatasetsReceived += dataCount;

                if (!m_StatisticDataRL.ContainsKey(relay))
                    m_StatisticDataRL.Add(relay, new EDDNStatistics());

                m_StatisticDataRL[relay].MessagesReceived += 1;
                m_StatisticDataRL[relay].DatasetsReceived += dataCount;

                if (!m_StatisticDataCM.ContainsKey(commander))
                    m_StatisticDataCM.Add(commander, new EDDNStatistics());

                m_StatisticDataCM[commander].MessagesReceived += 1;
                m_StatisticDataCM[commander].DatasetsReceived += dataCount;

                DataChangedEvent.Raise(this, new DataChangedEventArgs(enDataTypes.Statistics));

            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating statistics", ex);
            }
        }

        private String[] ConvertCommodityV3_To_CSVRows(JObject commodityV3Data)
        {
            String system;
            String starPort;
            Int32 commodityCount = 0;
            List<String> csvStrings = new List<string>();

            try
            {

                system   = commodityV3Data["message"]["systemName"].ToString();
                starPort = commodityV3Data["message"]["stationName"].ToString();

                foreach (Newtonsoft.Json.Linq.JToken commodity in commodityV3Data.SelectTokens("message.commodities[*]"))
                {                                                  
                    CsvRow csvData = new CsvRow();

                    csvData.SystemName          = system;
                    csvData.StationName         = starPort;
                    csvData.StationID           = String.Format("{0} [{1}]", starPort, system);
                    csvData.CommodityName       = commodity.Value<String>("name");
                    csvData.SellPrice           = commodity.Value<Int32>("sellPrice");
                    csvData.BuyPrice            = commodity.Value<Int32>("buyPrice");
                    csvData.Demand              = commodity.Value<Int32>("demand");
                    csvData.Supply              = commodity.Value<Int32>("stock");
                    csvData.SampleDate          = DateTime.UtcNow;

                    if((!String.IsNullOrEmpty(commodity.Value<String>("demandBracket"))) && (commodity.Value<Int32>("demandBracket") > 0))
                        csvData.DemandLevel         = (String)Program.Data.BaseTableIDToName("economylevel", commodity.Value<Int32>("demandBracket") - 1, "level");
                    else
                        csvData.DemandLevel = null;

                    if((!String.IsNullOrEmpty(commodity.Value<String>("stockBracket"))) && (commodity.Value<Int32>("stockBracket") > 0))
                        csvData.SupplyLevel         = (String)Program.Data.BaseTableIDToName("economylevel", commodity.Value<Int32>("stockBracket") - 1, "level");
                    else
                        csvData.SupplyLevel = null;

                    csvData.SourceFileName      = "";
                    csvData.DataSource          = "";

                    csvStrings.Add(csvData.ToString());

                    commodityCount++;
                } 
                
                return csvStrings.ToArray();

            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting commodity v3 data to csv rows", ex);
            }
        }

        public List<String> RawData
        {
            get
            {
                return m_RawData;
            }
        }
        public Dictionary<String, EDDNStatistics> StatisticDataSW
        {
            get
            {
                return m_StatisticDataSW;
            }
        }
        public Dictionary<String, EDDNStatistics> StatisticDataRL
        {
            get
            {
                return m_StatisticDataRL;
            }
        }
        public Dictionary<String, EDDNStatistics> StatisticDataCM
        {
            get
            {
                return m_StatisticDataCM;
            }
        }
        public Dictionary<String, EDDNStatistics> StatisticDataMT
        {
            get
            {
                return m_StatisticDataMT;
            }
        }
        public List<String> RejectedData
        {
            get
            {
                return m_RejectedData;
            }
        }

#endregion

#region send

        /// <summary>
        /// returns if the sender is active
        /// </summary>
        public bool SenderIsActivated
        {
            get
            {
                return m_SenderIsActivated;
            }
        }

        /// <summary>
        /// activatesender the sender
        /// </summary>
        public void ActivateSender()
        {
            m_SenderIsActivated = true;
        }

        /// <summary>
        /// deactivates the sender
        /// </summary> 
        public void DeactivateSender()
        {
            m_SenderIsActivated = false;

            SendingReset();
        }

        /// <summary>
        /// send the commodity data of this station
        /// </summary>
        /// <param name="stationData">json object with companion data</param>
        public void SendCommodityData(JObject dataObject)
        {
            Int32 objectCount = 0;
            Boolean writeToFile = false;
            StreamWriter writer = null;
            String debugFile = @"C:\temp\commodity_ibe.csv";
            SQL.Datasets.dsEliteDB.tbcommoditybaseDataTable baseData;

            try
            {
                if(m_SenderIsActivated && 
                   m_lDBCon.getIniValue<Boolean>(IBE.EDDN.EDDNView.DB_GROUPNAME, "EDDNPostCompanionData", true.ToString(), false))
                {
                    IBECompanion.CompanionConverter cmpConverter = new IBECompanion.CompanionConverter();
                    String systemName   = dataObject.SelectToken("lastSystem.name").ToString();
                    String stationName  = dataObject.SelectToken("lastStarport.name").ToString();

                    if((m_ID_of_Commodity_Station.Item1 != systemName + "|" + stationName) || ((DateTime.UtcNow - m_ID_of_Commodity_Station.Item2).TotalMinutes >= 60))
                    { 
                        m_ID_of_Commodity_Station = new Tuple<String, DateTime>(systemName +"|" + stationName, DateTime.UtcNow);

                        StringBuilder commodityStringEDDN = new StringBuilder();

                        commodityStringEDDN.Append(String.Format("\"message\": {{"));

                        commodityStringEDDN.Append(String.Format("\"systemName\":\"{0}\", "    , dataObject.SelectToken("lastSystem.name").ToString()));
                        //commodityStringEDDN.Append(String.Format("\"systemId\":\"{0}\", "      , dataObject.SelectToken("lastSystem.id").ToString()));
                        //commodityStringEDDN.Append(String.Format("\"systemAddress\":\"{0}\", " , dataObject.SelectToken("lastSystem.address").ToString()));
                        

                        commodityStringEDDN.Append(String.Format("\"stationName\":\"{0}\", " , dataObject.SelectToken("lastStarport.name").ToString()));
                        //commodityStringEDDN.Append(String.Format("\"stationId\":\"{0}\", "   , dataObject.SelectToken("lastStarport.id").ToString()));

                        commodityStringEDDN.Append(String.Format("\"timestamp\":\"{0}\", ", DateTime.UtcNow.ToString("o")));

                        commodityStringEDDN.Append(String.Format("\"commodities\": ["));

                        if(writeToFile)
                        { 
                            if(File.Exists(debugFile))
                                File.Delete(debugFile);

                            writer = new StreamWriter(File.OpenWrite(debugFile));
                        }

                        baseData = new SQL.Datasets.dsEliteDB.tbcommoditybaseDataTable();
                        m_lDBCon.Execute("select * from tbcommodityBase;", (System.Data.DataTable)baseData);

                        foreach (JToken commodityItem in dataObject.SelectTokens("lastStarport.commodities[*]"))
                        {

                            if(!commodityItem.Value<String>("categoryname").Equals("NonMarketable", StringComparison.InvariantCultureIgnoreCase))
                            {

                                CommodityObject commodity = cmpConverter.GetCommodityFromFDevIDs(baseData, commodityItem, false);
                                //commodityObject commodity = cmpConverter.GetcommodityFromCompanion(commodityItem, false);

                                int? dbValue = String.IsNullOrWhiteSpace(commodityItem.Value<String>("demandBracket")) ? null : commodityItem.Value<int?>("demandBracket");

                                if((commodity != null) && (dbValue.HasValue))
                                { 

                                    if (objectCount > 0)
                                        commodityStringEDDN.Append(", {");
                                    else
                                        commodityStringEDDN.Append("{");

                                    if(writeToFile)
                                    {
                                        writer.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", 
                                            systemName, stationName, commodity.Id, commodity.Name, commodity.Category, commodity.Average, 
                                            DateTime.UtcNow.ToString("o")));
                                    }

                                    commodityStringEDDN.Append(String.Format("\"name\":\"{0}\", ",    commodityItem.Value<String>("name")));
                                    //commodityStringEDDN.Append(String.Format("\"id\":\"{0}\", ",    commodity.Id));
                                    commodityStringEDDN.Append(String.Format("\"meanPrice\":{0}, ",   commodityItem.Value<Int32>("meanPrice")));
                                    commodityStringEDDN.Append(String.Format("\"buyPrice\":{0}, ",    commodityItem.Value<Int32>("buyPrice")));
                                    commodityStringEDDN.Append(String.Format("\"sellPrice\":{0}, ",   commodityItem.Value<Int32>("sellPrice")));
                                    

                                    commodityStringEDDN.Append(String.Format("\"demandBracket\":{0}, ", commodityItem.Value<int?>("demandBracket")));

                                    if (commodityItem.Value<int?>("demandBracket") == 0)
                                        commodityStringEDDN.Append(String.Format("\"demand\":{0}, ",      0));
                                    else
                                        commodityStringEDDN.Append(String.Format("\"demand\":{0}, ",      commodityItem.Value<Int32>("demand")));


                                    commodityStringEDDN.Append(String.Format("\"stockBracket\":{0}, ", commodityItem.Value<int?>("stockBracket")));

                                    if (commodityItem.Value<int?>("stockBracket") == 0)
                                        commodityStringEDDN.Append(String.Format("\"stock\":{0}, ",      0));
                                    else
                                        commodityStringEDDN.Append(String.Format("\"stock\":{0}, ",      commodityItem.Value<Int32>("demand")));

                                    
                                    if(commodityItem.SelectTokens("statusFlags.[*]").Count() > 0 )
                                    {
                                        commodityStringEDDN.Append(String.Format("\"statusFlags\": ["));
                                        foreach (JToken statusItem in commodityItem.SelectTokens("statusFlags.[*]"))
                                        {
                                            commodityStringEDDN.Append(String.Format("\"{0}\", ",      statusItem.Value<String>()));
                                        }

                                        commodityStringEDDN.Remove(commodityStringEDDN.Length-1, 1);
                                        commodityStringEDDN.Replace(",", "], ", commodityStringEDDN.Length-1, 1);
                                    }

                                    commodityStringEDDN.Remove(commodityStringEDDN.Length-1, 1);
                                    commodityStringEDDN.Replace(",", "}", commodityStringEDDN.Length-1, 1);

                                    objectCount++;
                                }
                            }
                        } 

                        commodityStringEDDN.Append("]}");

                        if(objectCount > 0)
                        { 
                            _Send_Commodity.Enqueue(commodityStringEDDN);
                            _SendDelayTimer_Commodity.Start();
                            m_ID_of_Commodity_Station = new Tuple<String, DateTime>(systemName +"|" + stationName, DateTime.UtcNow);
                        }

                        if(writeToFile)
                        {
                            writer.Close();
                            writer.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while extracting commodity data for eddn", ex);
            }
        }

        /// <summary>
        /// send the outfitting data of this station
        /// </summary>
        /// <param name="stationData">json object with companion data</param>
        public void SendOutfittingData(JObject dataObject)
        {
            Int32 objectCount = 0;
            Boolean writeToFile = false;
            StreamWriter writer = null;
            String debugFile = @"C:\temp\outfitting_ibe.csv";
            SQL.Datasets.dsEliteDB.tboutfittingbaseDataTable baseData;
            System.Text.RegularExpressions.Regex allowedPattern = new System.Text.RegularExpressions.Regex("(^Hpt_|^Int_|_Armour_)");

            try
            {
                if(m_SenderIsActivated && m_lDBCon.getIniValue<Boolean>(IBE.EDDN.EDDNView.DB_GROUPNAME, "EDDNPostOutfittingData", true.ToString(), false))
                {
                    IBECompanion.CompanionConverter cmpConverter = new IBECompanion.CompanionConverter();
                    String systemName   = dataObject.SelectToken("lastSystem.name").ToString();
                    String stationName  = dataObject.SelectToken("lastStarport.name").ToString();

                    if((m_ID_of_Outfitting_Station.Item1 != systemName + "|" + stationName) || ((DateTime.UtcNow - m_ID_of_Outfitting_Station.Item2).TotalMinutes >= 60))
                    { 
                        m_ID_of_Outfitting_Station = new Tuple<String, DateTime>(systemName +"|" + stationName, DateTime.UtcNow);

                        StringBuilder outfittingStringEDDN = new StringBuilder();

                        outfittingStringEDDN.Append(String.Format("\"message\": {{"));

                        outfittingStringEDDN.Append(String.Format("\"systemName\":\"{0}\", "    , dataObject.SelectToken("lastSystem.name").ToString()));
                        //outfittingStringEDDN.Append(String.Format("\"systemId\":\"{0}\", "      , dataObject.SelectToken("lastSystem.id").ToString()));
                        //outfittingStringEDDN.Append(String.Format("\"systemAddress\":\"{0}\", " , dataObject.SelectToken("lastSystem.address").ToString()));
                        

                        outfittingStringEDDN.Append(String.Format("\"stationName\":\"{0}\", " , dataObject.SelectToken("lastStarport.name").ToString()));
                        //outfittingStringEDDN.Append(String.Format("\"stationId\":\"{0}\", "   , dataObject.SelectToken("lastStarport.id").ToString()));

                        outfittingStringEDDN.Append(String.Format("\"timestamp\":\"{0}\", ", DateTime.UtcNow.ToString("o")));

                        outfittingStringEDDN.Append(String.Format("\"modules\": ["));

                        if(writeToFile)
                        { 
                            if(File.Exists(debugFile))
                                File.Delete(debugFile);

                            writer = new StreamWriter(File.OpenWrite(debugFile));
                        }

                        baseData = new SQL.Datasets.dsEliteDB.tboutfittingbaseDataTable();
                        m_lDBCon.Execute("select * from tbOutfittingBase;", (System.Data.DataTable)baseData);
                        

                        foreach (JToken outfittingItem in dataObject.SelectTokens("lastStarport.modules.*"))
                        {

                            if(allowedPattern.IsMatch(outfittingItem.Value<String>("name")) && 
                              ((outfittingItem.Value<String>("sku") == null) || (outfittingItem.Value<String>("sku").Equals("ELITE_HORIZONS_V_PLANETARY_LANDINGS"))) && 
                              (!outfittingItem.Value<String>("name").Equals("Int_PlanetApproachSuite")))
                            { 
                                OutfittingObject outfitting = cmpConverter.GetOutfittingFromFDevIDs(baseData, outfittingItem, false);

                                if(objectCount > 0)
                                    outfittingStringEDDN.Append(", ");

                                if(writeToFile)
                                {
                                    writer.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", 
                                        systemName, stationName, outfitting.Category, outfitting.Name, outfitting.Mount, 
                                        outfitting.Guidance, outfitting.Ship, outfitting.Class, outfitting.Rating, 
                                        DateTime.UtcNow.ToString("o")));
                                }

                                outfittingStringEDDN.Append(String.Format("\"{0}\", ", outfittingItem.Value<String>("name")));

                                outfittingStringEDDN.Remove(outfittingStringEDDN.Length-1, 1);
                                outfittingStringEDDN.Replace(",", "", outfittingStringEDDN.Length-1, 1);

                                objectCount++;
                            }
                        } 

                        outfittingStringEDDN.Append("]}");

                        if(objectCount > 0)
                        { 
                            _Send_Outfitting.Enqueue(outfittingStringEDDN);
                            _SendDelayTimer_Outfitting.Start();
                            m_ID_of_Outfitting_Station = new Tuple<String, DateTime>(systemName +"|" + stationName, DateTime.UtcNow);
                        }

                        if(writeToFile)
                        {
                            writer.Close();
                            writer.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while extracting outfitting data for eddn", ex);
            }
        }

        /// <summary>
        /// send the shipyard data of this station
        /// </summary>
        /// <param name="stationData">json object with companion data</param>
        public void SendShipyardData(JObject dataObject)
        {
            Int32 objectCount = 0;
            Boolean writeToFile = false;
            StreamWriter writer = null;
            String debugFile = @"C:\temp\shipyard_ibe.csv";
            SQL.Datasets.dsEliteDB.tbshipyardbaseDataTable baseData;

            try
            {
                if(m_SenderIsActivated && m_lDBCon.getIniValue<Boolean>(IBE.EDDN.EDDNView.DB_GROUPNAME, "EDDNPostShipyardData", true.ToString(), false))
                {
                    IBECompanion.CompanionConverter cmpConverter = new IBECompanion.CompanionConverter();  

                    String systemName   = dataObject.SelectToken("lastSystem.name").ToString();
                    String stationName = dataObject.SelectToken("lastStarport.name").ToString();

                    if((m_ID_of_Shipyard_Station.Item1 != systemName + "|" + stationName) || ((DateTime.UtcNow - m_ID_of_Shipyard_Station.Item2).TotalMinutes >= 60))
                    { 
                        StringBuilder shipyardStringEDDN = new StringBuilder();

                        shipyardStringEDDN.Append(String.Format("\"message\": {{"));

                        shipyardStringEDDN.Append(String.Format("\"systemName\":\"{0}\", ",dataObject.SelectToken("lastSystem.name").ToString()));
                        shipyardStringEDDN.Append(String.Format("\"stationName\":\"{0}\", ",dataObject.SelectToken("lastStarport.name").ToString()));

                        shipyardStringEDDN.Append(String.Format("\"timestamp\":\"{0}\", ", DateTime.UtcNow.ToString("o")));

                        shipyardStringEDDN.Append(String.Format("\"ships\": ["));

                        if(writeToFile)
                        { 
                            if(File.Exists(debugFile))
                                File.Delete(debugFile);

                            writer = new StreamWriter(File.OpenWrite(debugFile));
                        }

                        if(dataObject.SelectToken("lastStarport.ships", false) != null)
                        { 
                            baseData = new SQL.Datasets.dsEliteDB.tbshipyardbaseDataTable();
                            m_lDBCon.Execute("select * from tbShipyardBase;", (System.Data.DataTable)baseData);

                            List<JToken> allShips = dataObject.SelectTokens("lastStarport.ships.shipyard_list.*").ToList();
                            allShips.AddRange(dataObject.SelectTokens("lastStarport.ships.unavailable_list.[*]").ToList());

                            foreach (JToken outfittingItem in allShips)
                            {
                                if(!String.IsNullOrWhiteSpace(outfittingItem.Value<String>("name")))
                                {
                                    ShipyardObject shipyardItem = cmpConverter.GetShipFromFDevIDs(baseData, outfittingItem, false);
                                    //ShipyardObject shipyardItem = cmpConverter.GetShipFromCompanion(outfittingItem, false);

                                    if(writeToFile)
                                    {
                                        writer.WriteLine(String.Format("{0},{1},{2},{3}", 
                                            systemName, stationName, shipyardItem.Name, 
                                            DateTime.UtcNow.ToString("o")));
                                    }

                                    shipyardStringEDDN.Append(String.Format("\"{0}\", ", outfittingItem.Value<String>("name")));

                                    objectCount++;
                                    
                                }
                            } 

                            shipyardStringEDDN.Remove(shipyardStringEDDN.Length-2, 2);
                            shipyardStringEDDN.Append("]}");

                            if(objectCount > 0)
                            { 
                                _Send_Shipyard.Enqueue(shipyardStringEDDN);
                                _SendDelayTimer_Shipyard.Start();
                                m_ID_of_Shipyard_Station = new Tuple<String, DateTime>(systemName +"|" + stationName, DateTime.UtcNow);
                            }

                            if(writeToFile)
                            {
                                writer.Close();
                                writer.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while extracting shipyard data for eddn", ex);
            }
        }

        /// <summary>
        /// send the journal data 
        /// </summary>
        /// <param name="stationData">json object with journal data</param>
        public void SendJournalData(JObject dataObject)
        {
            try
            {
                if(m_SenderIsActivated && m_lDBCon.getIniValue<Boolean>(IBE.EDDN.EDDNView.DB_GROUPNAME, "EDDNPostJournalData", true.ToString(), false))
                {                                                                                             
                    StringBuilder journalStringEDDN = new StringBuilder();
                    journalStringEDDN.Append(String.Format("\"message\": {{"));
                    
                    journalStringEDDN.Append(String.Format("\"timestamp\":\"{0}\", ", DateTime.UtcNow.ToString("o")));
                    journalStringEDDN.Append(String.Format("\"event\":\"{0}\", ",      dataObject.SelectToken("event").ToString()));

                    if(dataObject.SelectToken("StarSystem") == null)
                        journalStringEDDN.Append(String.Format("\"StarSystem\":\"{0}\", ", Program.actualCondition.System));
                    else
                        journalStringEDDN.Append(String.Format("\"StarSystem\":\"{0}\", ", dataObject.SelectToken("StarSystem").ToString()));
                        
                    if(dataObject.SelectToken("StarPos") == null)
                        journalStringEDDN.Append(String.Format("\"StarPos\":[{0},{1},{2}], ", SQL.DBConnector.SQLDecimal(Program.actualCondition.Coordinates.X.Value), SQL.DBConnector.SQLDecimal(Program.actualCondition.Coordinates.Y.Value), SQL.DBConnector.SQLDecimal(Program.actualCondition.Coordinates.Z.Value)));
                    else
                        journalStringEDDN.Append(String.Format("\"StarPos\":{0}, ",    dataObject.SelectToken("StarPos")));

                    System.Text.RegularExpressions.Regex forbiddenPattern   = new System.Text.RegularExpressions.Regex("(CockpitBreach|BoostUsed|FuelLevel|FuelUsed|JumpDist|_Localised$|timestamp|event|StarSystem|StarPos)");
                    List<String> typeList = new List<String>() { "array", "boolean", "integer", "float", "double", "object", "string" };

                    
                    foreach (JToken dataItem in dataObject.SelectTokens("*"))
                    {
                        if(!forbiddenPattern.IsMatch(dataItem.Path))
                        {
                            if(typeList.Contains(dataItem.Type.ToString().ToLower()))
                            {
                                Debug.Print("allowed : " + dataItem.Path + "(" + dataItem.Type.ToString() + ")");

                                switch (dataItem.Type.ToString().ToLower())
                                {
                                    case "string":
                                        journalStringEDDN.Append(String.Format("\"{0}\":\"{1}\", ",    dataItem.Path, dataItem));    
                                        break;

                                    case "float":
                                    case "double":
                                        journalStringEDDN.Append(String.Format("\"{0}\":{1}, ",    dataItem.Path, SQL.DBConnector.SQLDecimal((double)dataItem)));    
                                        break;

                                    case "boolean":
                                        journalStringEDDN.Append(String.Format("\"{0}\":{1}, ",    dataItem.Path, dataItem.ToString().ToLower()));
                                        break;

                                    default:
                                        journalStringEDDN.Append(String.Format("\"{0}\":{1}, ",    dataItem.Path, dataItem));
                                        break;
                                }
                            }
                            else
                                Debug.Print("disallowed : " + dataItem.Path + "(" + dataItem.Type.ToString() + ")");

                            
                        }
                        else
                            Debug.Print("disallowed : " + dataItem.Path);
                    }

                    journalStringEDDN.Remove(journalStringEDDN.Length-2, 2);
                    journalStringEDDN.Append("}");


                    _Send_Journal.Enqueue(journalStringEDDN);
                    _SendDelayTimer_Journal.Start();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while extracting journal data for eddn", ex);
            }
        }


        /// <summary>
        /// internal send routine for registered data:
        /// It's called by the delay-timer "_SendDelayTimer_Commodity"
        /// </summary>
        private void SendMarketData_i()
        {
            try
            {
                Queue activeQueue = null; 
                String UserID;
                EDDNCommodity_v3 Data;
                String TimeStamp;
                String commodity;

                throw new NotImplementedException("function SendMarketData_i ist not tested yet !!");

                do{

                    TimeStamp   = DateTime.UtcNow.ToString("o");
                    UserID      = UserIdentification();
                    Data        = new EDDNCommodity_v3();

                    // test or real ?
                    if((m_lDBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false) == "Test") || (Program.actualCondition.GameversionIsBeta))
                        Data.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/3/test";
                    else
                        Data.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/3";

                    if(_Send_MarketData_API.Count > 0)
                    {
                        // fill the header
                        Data.Header = new EDDNCommodity_v3.Header_Class()
                        {
                            SoftwareName = "ED-IBE (API)",
                            SoftwareVersion = VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3),
                            GatewayTimestamp = TimeStamp,
                            UploaderID = UserID
                        };

                        activeQueue = _Send_MarketData_API;
                    }
                    else
                    { 
                        // fill the header
                        Data.Header = new EDDNCommodity_v3.Header_Class()
                        {
                            SoftwareName = "ED-IBE (OCR)",
                            SoftwareVersion = VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3),
                            GatewayTimestamp = TimeStamp,
                            UploaderID = UserID
                        };

                        activeQueue = _Send_MarketData_OCR;
                    }

                    // prepare the message object
                    Data.Message = new EDDNCommodity_v3.Message_Class()
                    {
                        SystemName = "",
                        StationName = "",
                        Timestamp = TimeStamp,
                        Commodities = new EDDNCommodity_v3.Commodity_Class[activeQueue.Count]
                    };

                    // collect the commodity data
                    for (int i = 0; i <= Data.Message.Commodities.GetUpperBound(0); i++)
                    {
                        CsvRow Row = (CsvRow)activeQueue.Dequeue();

                        commodity = Row.CommodityName;

                        // if it's a user added commodity send it anyhow to see that there's a unknown commodity
                        if (commodity.Equals(Program.COMMODITY_NOT_SET))
                            commodity = Row.CommodityName;

                        Data.Message.Commodities[i] = new EDDNCommodity_v3.Commodity_Class()
                        {
                            Name            = commodity,
                            BuyPrice        = (Int32)Math.Floor(Row.BuyPrice),
                            SellPrice       = (Int32)Math.Floor(Row.SellPrice),
                            Demand          = (Int32)Math.Floor(Row.Demand),
                            DemandBracket   = (Row.DemandLevel == "") ? (int?)null : Int32.Parse(Row.DemandLevel),
                            Stock           = (Int32)Math.Floor(Row.Supply),
                            StockBracket    = (Row.SupplyLevel == "") ? (int?)null : Int32.Parse(Row.SupplyLevel),
                        };

                        if (i == 0)
                        {
                            Data.Message.SystemName     = Row.SystemName;
                            Data.Message.StationName    = Row.StationName;
                        }

                    }

                    using (var client = new WebClient())
                    {
                        try
                        {
                            Debug.Print(JsonConvert.SerializeObject(Data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

                            client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", JsonConvert.SerializeObject(Data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

                            if(activeQueue == _Send_MarketData_API)
                            { 
                                // data over api sent, set cooldown time
                                m_ID_of_Commodity_Station = new Tuple<String, DateTime>(m_ID_of_Commodity_Station.Item1, DateTime.UtcNow);
                            }

                            m_CommoditySendingError  = false;
                            DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Commodity_V3, enTransmittedStates.Sent));
                        }
                        catch (WebException ex)
                        {
                            _logger.Log("Error uploading json (commodity)");
                            _logger.Log(ex.ToString());
                            _logger.Log(ex.Message);
                            _logger.Log(ex.StackTrace);
                            if (ex.InnerException != null)
                                _logger.Log(ex.InnerException.ToString());

                            using (WebResponse response = ex.Response)
                            {
                                using (Stream data = response.GetResponseStream())
                                {
                                    if (data != null)
                                    {
                                        StreamReader sr = new StreamReader(data);
                                        m_CommoditySendingError  = true;
                                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Commodity_V3, enTransmittedStates.Error));
                                        _logger.Log("Error while uploading commodity data to EDDN : " + sr.ReadToEnd());
                                    }
                                }
                            }
                        }
                        finally
                        {
                            client.Dispose();
                        }

                    }

                    // retry, if the ocr-queue has entries and the last queue was then api-queue
	            } while ((activeQueue != _Send_MarketData_OCR) && (_Send_MarketData_OCR.Count > 0));
            }
            catch (Exception ex)
            {
                _logger.Log("Error uploading json (commodity)");
                _logger.Log(ex.ToString());
                _logger.Log(ex.Message);
                _logger.Log(ex.StackTrace);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString());

                CErr.processError(ex, "Error in EDDN-Sending-Thread (commodity)");
            }

        }

        /// <summary>
        /// internal send routine for registered data:
        /// It's called by the delay-timer "_SendDelayTimer_Commodity"
        /// </summary>
        private void SendOutfittingData_i()
        {
            try
            {
                MessageHeader header;
                String schema;
                StringBuilder outfittingMessage = new StringBuilder();

                // fill the header
                header = new MessageHeader()
                {
                    SoftwareName        = "ED-IBE (API)",
                    SoftwareVersion     = VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3),
                    GatewayTimestamp    = DateTime.UtcNow.ToString("o"),
                    UploaderID          = UserIdentification()
                };

                // fill the schema : test or real ?
                if((m_lDBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false) == "Test") || (Program.actualCondition.GameversionIsBeta))
                    schema = "http://schemas.elite-markets.net/eddn/outfitting/2/test";
                else
                    schema = "http://schemas.elite-markets.net/eddn/outfitting/2";

                do
                {
                    // create full message
                    outfittingMessage.Clear();
                    outfittingMessage.Append(String.Format("{{" +
                                                           " \"header\" : {0}," +
                                                           " \"$schemaRef\": \"{1}\","+
                                                           " {2}" +
                                                           "}}", 
                                                           JsonConvert.SerializeObject(header, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                                                           schema,
                                                           _Send_Outfitting.Dequeue().ToString()));


                    using (var client = new WebClient())
                    {
                        try
                        {
                            Debug.Print(outfittingMessage.ToString());

                            client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", outfittingMessage.ToString());

                            m_OutfittingSendingError = false;
                            DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Outfitting_V2, enTransmittedStates.Sent));
                        }
                        catch (WebException ex)
                        {
                            _logger.Log("Error uploading json (outfitting)");
                            _logger.Log(ex.ToString());
                            _logger.Log(ex.Message);
                            _logger.Log(ex.StackTrace);
                            if (ex.InnerException != null)
                                _logger.Log(ex.InnerException.ToString());

                            using (WebResponse response = ex.Response)
                            {
                                using (Stream data = response.GetResponseStream())
                                {
                                    if (data != null)
                                    {
                                        StreamReader sr = new StreamReader(data);
                                        m_OutfittingSendingError = true;
                                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Outfitting_V2, enTransmittedStates.Error));
                                        _logger.Log("Error while uploading outfitting data to EDDN : " + sr.ReadToEnd());
                                    }
                                }
                            }
                        }
                        finally
                        {
                            client.Dispose();
                        }
                    }
                } while (_Send_Outfitting.Count > 0);
            }
            catch (Exception ex)
            {
                _logger.Log("Error uploading Json (outfitting)");
                _logger.Log(ex.ToString());
                _logger.Log(ex.Message);
                _logger.Log(ex.StackTrace);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString());

                CErr.processError(ex, "Error in EDDN-Sending-Thread (outfitting)");
            }

        }

        /// <summary>
        /// internal send routine for registered data:
        /// It's called by the delay-timer "_SendDelayTimer_Commodity"
        /// </summary>
        private void SendCommodityData_i()
        {
            try
            {
                MessageHeader header;
                String schema;
                StringBuilder commodityMessage = new StringBuilder();

                // fill the header
                header = new MessageHeader()
                {
                    SoftwareName        = "ED-IBE (API)",
                    SoftwareVersion     = VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3),
                    GatewayTimestamp    = DateTime.UtcNow.ToString("o"),
                    UploaderID          = UserIdentification()
                };

                // fill the schema : test or real ?
                if((m_lDBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false) == "Test") || (Program.actualCondition.GameversionIsBeta))
                    schema = "http://schemas.elite-markets.net/eddn/commodity/3/test";
                else
                    schema = "http://schemas.elite-markets.net/eddn/commodity/3";

                // create full message
                commodityMessage.Append(String.Format("{{" +
                                                      " \"header\" : {0}," +
                                                      " \"$schemaRef\": \"{1}\","+
                                                      " {2}" +
                                                      "}}", 
                                                      JsonConvert.SerializeObject(header, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                                                      schema,
                                                      _Send_Commodity.Dequeue().ToString()));


                using (var client = new WebClient())
                {
                    try
                    {
                        Debug.Print(commodityMessage.ToString());

                        client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", commodityMessage.ToString());

                        m_CommoditySendingError = false;
                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Commodity_V3, enTransmittedStates.Sent));
                    }
                    catch (WebException ex)
                    {
                        _logger.Log("Error uploading json (commodity)");
                        _logger.Log(ex.ToString());
                        _logger.Log(ex.Message);
                        _logger.Log(ex.StackTrace);
                        if (ex.InnerException != null)
                            _logger.Log(ex.InnerException.ToString());

                        using (WebResponse response = ex.Response)
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                if (data != null)
                                {
                                    StreamReader sr = new StreamReader(data);
                                    m_CommoditySendingError = true;
                                    DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Commodity_V3, enTransmittedStates.Error));
                                    _logger.Log("Error while uploading commodity data to EDDN : " + sr.ReadToEnd());
                                }
                            }
                        }
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Log("Error uploading Json (commodity)");
                _logger.Log(ex.ToString());
                _logger.Log(ex.Message);
                _logger.Log(ex.StackTrace);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString());

                CErr.processError(ex, "Error in EDDN-Sending-Thread (commodity)");
            }

        }

        /// <summary>
        /// internal send routine for registered data:
        /// It's called by the delay-timer "_SendDelayTimer_Commodity"
        /// </summary>
        private void SendShipyardData_i()
        {
            try
            {
                MessageHeader header;
                String schema;
                StringBuilder shipyardMessage = new StringBuilder();

                // fill the header
                header = new MessageHeader()
                {
                    SoftwareName        = "ED-IBE (API)",
                    SoftwareVersion     = VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3),
                    GatewayTimestamp    = DateTime.UtcNow.ToString("o"),
                    UploaderID          = UserIdentification()
                };

                // fill the schema : test or real ?
                if((m_lDBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false) == "Test") || (Program.actualCondition.GameversionIsBeta))
                    schema = "http://schemas.elite-markets.net/eddn/shipyard/2/test";
                else
                    schema = "http://schemas.elite-markets.net/eddn/shipyard/2";

                do
                {
                    // create full message
                    shipyardMessage.Clear();
                    shipyardMessage.Append(String.Format("{{" +
                                                           " \"header\" : {0}," +
                                                           " \"$schemaRef\": \"{1}\","+
                                                           " {2}" +
                                                           "}}", 
                                                           JsonConvert.SerializeObject(header, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                                                           schema,
                                                           _Send_Shipyard.Dequeue().ToString()));


                    using (var client = new WebClient())
                    {
                        try
                        {
                            Debug.Print(shipyardMessage.ToString());

                            client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", shipyardMessage.ToString());

                            m_ShipyardSendingError   = false;
                            DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Shipyard_V1, enTransmittedStates.Sent));
                        }
                        catch (WebException ex)
                        {
                            _logger.Log("Error uploading json (shipyard)");
                            _logger.Log(ex.ToString());
                            _logger.Log(ex.Message);
                            _logger.Log(ex.StackTrace);
                            if (ex.InnerException != null)
                                _logger.Log(ex.InnerException.ToString());

                            using (WebResponse response = ex.Response)
                            {
                                using (Stream data = response.GetResponseStream())
                                {
                                    if (data != null)
                                    {
                                        StreamReader sr = new StreamReader(data);
                                        m_ShipyardSendingError   = true;
                                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Shipyard_V1, enTransmittedStates.Error));
                                        _logger.Log("Error while uploading outfitting data to EDDN : " + sr.ReadToEnd());
                                    }
                                }
                            }
                        }
                        finally
                        {
                            client.Dispose();
                        }
                    }
                } while (_Send_Shipyard.Count > 0);
            }
            catch (Exception ex)
            {
                _logger.Log("Error uploading Json (shipyard)");
                _logger.Log(ex.ToString());
                _logger.Log(ex.Message);
                _logger.Log(ex.StackTrace);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString());

                CErr.processError(ex, "Error in EDDN-Sending-Thread (shipyard)");
            }

        }


        /// <summary>
        /// internal send routine for registered data:
        /// It's called by the delay-timer "_SendDelayTimer_Commodity"
        /// </summary>
        private void SendJournalData_i()
        {
            try
            {
                MessageHeader header;
                String schema;
                StringBuilder journalMessage = new StringBuilder();

                // fill the header
                header = new MessageHeader()
                {
                    SoftwareName        = "ED-IBE (API)",
                    SoftwareVersion     = VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3),
                    GatewayTimestamp    = DateTime.UtcNow.ToString("o"),
                    UploaderID          = UserIdentification()
                };

                // fill the schema : test or real ?
                if((m_lDBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false) == "Test") || (Program.actualCondition.GameversionIsBeta))
                    schema = "http://schemas.elite-markets.net/eddn/journal/1/test";
                else
                    schema = "http://schemas.elite-markets.net/eddn/journal/1";

                do
                {
                    // create full message
                    journalMessage.Clear();
                    journalMessage.Append(String.Format("{{" +
                                                           " \"header\" : {0}," +
                                                           " \"$schemaRef\": \"{1}\","+
                                                           " {2}" +
                                                           "}}", 
                                                           JsonConvert.SerializeObject(header, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                                                           schema,
                                                           _Send_Journal.Dequeue().ToString()));


                    using (var client = new WebClient())
                    {
                        try
                        {
                            Debug.Print(journalMessage.ToString());

                            client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", journalMessage.ToString());

                            m_JournalSendingError   = false;
                            DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Journal_V1, enTransmittedStates.Sent));
                        }
                        catch (WebException ex)
                        {
                            _logger.Log("Error uploading json (journal)");
                            _logger.Log(ex.ToString());
                            _logger.Log(ex.Message);
                            _logger.Log(ex.StackTrace);
                            if (ex.InnerException != null)
                                _logger.Log(ex.InnerException.ToString());

                            using (WebResponse response = ex.Response)
                            {
                                using (Stream data = response.GetResponseStream())
                                {
                                    if (data != null)
                                    {
                                        StreamReader sr = new StreamReader(data);
                                        m_JournalSendingError   = true;
                                        DataTransmittedEvent.Raise(this, new DataTransmittedEventArgs(enTransmittedTypes.Journal_V1, enTransmittedStates.Error));
                                        _logger.Log("Error while uploading journal data to EDDN : " + sr.ReadToEnd());
                                    }
                                }
                            }
                        }
                        finally
                        {
                            client.Dispose();
                        }
                    }
                } while (_Send_Journal.Count > 0);

            }
            catch (Exception ex)
            {
                _logger.Log("Error uploading Json (journal)");
                _logger.Log(ex.ToString());
                _logger.Log(ex.Message);
                _logger.Log(ex.StackTrace);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString());

                CErr.processError(ex, "Error in EDDN-Sending-Thread (journal)");
            }

        }


        /// <summary>
        /// timer routine for sending all registered commoditydata to EDDN
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SendDelayTimerMarket_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                // it's time to start the EDDN transmission
                _Spool2EDDN_Market = new Thread(new ThreadStart(SendMarketData_i));
                _Spool2EDDN_Market.Name = "Spool2EDDN Market";
                _Spool2EDDN_Market.IsBackground = true;

                _Spool2EDDN_Market.Start();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while sending EDDN data (commodity)");
            }
        }

        /// <summary>
        /// timer routine for sending all registered commoditydata to EDDN
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SendDelayTimerCommodity_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                // it's time to start the EDDN transmission
                _Spool2EDDN_Commodity = new Thread(new ThreadStart(SendCommodityData_i));
                _Spool2EDDN_Commodity.Name = "Spool2EDDN Commodity";
                _Spool2EDDN_Commodity.IsBackground = true;

                _Spool2EDDN_Commodity.Start();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while sending EDDN data (commodity)");
            }
        }

        /// <summary>
        /// timer routine for sending all registered data to EDDN
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SendDelayTimerOutfitting_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                // it's time to start the EDDN transmission
                _Spool2EDDN_Outfitting = new Thread(new ThreadStart(SendOutfittingData_i));
                _Spool2EDDN_Outfitting.Name = "Spool2EDDN Outfitting";
                _Spool2EDDN_Outfitting.IsBackground = true;

                _Spool2EDDN_Outfitting.Start();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while sending EDDN data (outfitting)");
            }
        }

        /// <summary>
        /// timer routine for sending all registered data to EDDN
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SendDelayTimerShipyard_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                // it's time to start the EDDN transmission
                _Spool2EDDN_Shipyard = new Thread(new ThreadStart(SendShipyardData_i));
                _Spool2EDDN_Shipyard.Name = "Spool2EDDN Shipyard";
                _Spool2EDDN_Shipyard.IsBackground = true;

                _Spool2EDDN_Shipyard.Start();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while sending EDDN data (shipyard)");
            }
        }

        /// <summary>
        /// timer routine for sending all registered data to EDDN
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SendDelayTimerJournal_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                // it's time to start the EDDN transmission
                _Spool2EDDN_Journal = new Thread(new ThreadStart(SendJournalData_i));
                _Spool2EDDN_Journal.Name = "Spool2EDDN Journal";
                _Spool2EDDN_Journal.IsBackground = true;

                _Spool2EDDN_Journal.Start();

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while sending EDDN data (journal)");
            }
        }

        /// <summary>
        /// checks and gets the EDDN id
        /// </summary>
        public String UserIdentification()
        {
            String retValue = "";
            String userName = "";
            Guid parsedGUID;

            try
            {
                retValue = m_lDBCon.getIniValue<String>(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserID");

                if((!Guid.TryParse(retValue, out parsedGUID)) || (!parsedGUID.ToString().Equals(retValue)))
                {
                    retValue = Guid.NewGuid().ToString();
                    m_lDBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserID", retValue);
                }
                    
                if (m_lDBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Identification", "useUserName") == "useUserName")
                {
                    userName = m_lDBCon.getIniValue<String>(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserName");

                    if (String.IsNullOrWhiteSpace(userName))
                        m_lDBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Identification", "useUserID");
                    else
                        retValue = userName;
                }

                return userName;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking the EDDN id", ex);
            }
        }

        public void SendingReset()
        {
            m_ID_of_Commodity_Station  =  new Tuple<String, DateTime>("", new DateTime());
            m_ID_of_Outfitting_Station =  new Tuple<String, DateTime>("", new DateTime());
            m_ID_of_Shipyard_Station   = new Tuple<String, DateTime>("", new DateTime());
            m_ID_of_Journal_Station   = new Tuple<String, DateTime>("", new DateTime());

            m_CommoditySendingError  = false;
            m_OutfittingSendingError = false;
            m_ShipyardSendingError   = false;
            m_JournalSendingError   = false;

        }


        public void RegisterJournalScanner(FileScanner.EDJournalScanner journalScanner)
        {
            try
            {
                if(m_JournalScanner == null)
                { 
                    m_JournalScanner = journalScanner;
                    m_JournalScanner.JournalEventRecieved += JournalEventRecieved;
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
                    case FileScanner.EDJournalScanner.JournalEvent.FSDJump:
                    case FileScanner.EDJournalScanner.JournalEvent.Docked:
                    case FileScanner.EDJournalScanner.JournalEvent.Scan:
                        SendJournalData((JObject)e.Data);
                        break;
                }

            }
            catch (Exception ex)
            {
                CErr.processError(ex, "Error while processing the JournalEventRecieved-event");
            }
        }

#endregion

        public enum SendingState
        {
            NotSend  = 0,
            Send     = 1,
            Error    = 2
        }

        public SendingState CommodityDataTransmitted 
        { 
            get 
            { 
                SendingState retValue = SendingState.NotSend;
 
                if(m_CommoditySendingError)
                    retValue = SendingState.Error;
                else if ((DateTime.UtcNow - (DateTime)(m_ID_of_Commodity_Station.Item2)).TotalMinutes <= 60)
                    retValue = SendingState.Send;

                return retValue;
            }
        }

        public SendingState OutfittingDataTransmitted
        {
            get 
            { 
                SendingState retValue = SendingState.NotSend;
 
                if(m_OutfittingSendingError)
                    retValue = SendingState.Error;
                else if ((DateTime.UtcNow - (DateTime)(m_ID_of_Outfitting_Station.Item2)).TotalMinutes <= 60)
                    retValue = SendingState.Send;

                return retValue;
            }
        }

        public SendingState ShipyardDataTransmitted
        {
            get 
            { 
                SendingState retValue = SendingState.NotSend;
 
                if(m_ShipyardSendingError)
                    retValue = SendingState.Error;
                else if ((DateTime.UtcNow - (DateTime)(m_ID_of_Shipyard_Station.Item2)).TotalMinutes <= 60)
                    retValue = SendingState.Send;

                return retValue;
            }
        }
    }
}
