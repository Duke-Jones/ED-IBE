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
using IBE.Enums_and_Utility_Classes;
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
            Shipyard_V1         =  11,
            Outfitting_V1       =  21    
        }


 #endregion

        private Thread                              _Spool2EDDN;   
        private Queue                               _Send_MarketData_API = new Queue(100,10);
        private Queue                               _Send_MarketData_OCR = new Queue(100,10);
        private Queue                               _Send_Outfitting_OCR = new Queue(100,10);
        private SingleThreadLogger                  _logger;
        private System.Timers.Timer                 _SendDelayTimer;
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
        private List<String>                        m_Relays    = new List<string>() { "tcp://eddn-relay.elite-markets.net:9500", 
                                                                                       "tcp://eddn-relay.ed-td.space:9500"};


        public EDDNCommunicator()
        {

            m_Reciever = new Dictionary<String, EDDNReciever>();

            _SendDelayTimer             = new System.Timers.Timer(2000);
            _SendDelayTimer.AutoReset   = false;
            _SendDelayTimer.Elapsed     += new System.Timers.ElapsedEventHandler(this.SendDelayTimer_Elapsed);

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

            try
            {

                if (Program.DBCon.getIniValue<Boolean>("EDDN", "SpoolEDDNToFile", false.ToString(), false))
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

                ownSchema = Program.DBCon.getIniValue<enSchema>(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false);

                switch (e.InfoType)
                {

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Commodity_v2_Recieved:

                        EDDN.EDDNCommodity_v2 DataObject = (EDDN.EDDNCommodity_v2)e.Data;

                        if(m_DuplicateRelayFilter.DataAccepted(DataObject.Header.UploaderID, DataObject.Message.SystemName + "|" + DataObject.Message.StationName, DataObject.Message.Commodities.Count().ToString(), DateTime.Parse(DataObject.Message.Timestamp)))
                        { 
                            UpdateStatisticDataMsg(enMessageTypes.Commodity_V2);
                            UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));

                            // process only if it's the correct schema
                            dataSchema = ((EDDN.EDDNCommodity_v2)e.Data).isTest() ? enSchema.Test : enSchema.Real;

                            if (ownSchema == dataSchema)
                            {
                                Debug.Print("handle v2 message");



                                // Don't import our own uploads...
                                if (DataObject.Header.UploaderID != UserIdentification())
                                {
                                    DataRows = DataObject.getEDDNCSVImportStrings();
                                    nameAndVersion = String.Format("{0} / {1}", DataObject.Header.SoftwareName, DataObject.Header.SoftwareVersion);
                                    name = String.Format("{0}", DataObject.Header.SoftwareName);
                                    uploaderID = DataObject.Header.UploaderID;
                                }
                                else
                                    Debug.Print("handle v2 rejected (it's our own message)");
                            }
                            else
                                Debug.Print("handle v2 rejected (wrong schema)");
                        }
                        else
                            Debug.Print("handle v2 rejected (double recieved)");

                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Outfitting_v1_Recieved:

                        UpdateStatisticDataMsg(enMessageTypes.Outfitting_V1);
                        //UpdateRawData("recieved shipyard message ignored (coming feature)");
                        Debug.Print("recieved shipyard message ignored");
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));

                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.Shipyard_v1_Recieved:
                        UpdateStatisticDataMsg(enMessageTypes.Shipyard_V1);
                        //UpdateRawData("recieved shipyard message ignored (coming feature)");
                        Debug.Print("recieved shipyard message ignored");
                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.UnknownData:
                        UpdateStatisticDataMsg(enMessageTypes.unknown);
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));
                        UpdateRawData("Recieved a unknown EDDN message:" + Environment.NewLine + e.Message + Environment.NewLine + e.RawData);
                        Debug.Print("handle unkown message");
                        break;

                    case EDDN.EDDNRecievedArgs.enMessageInfo.ParseError:
                        UpdateRawData(String.Format("{0}\r\n(from {2})\r\n{1}", e.Message, e.RawData, e.Adress));
                        Debug.Print("handle error message");
                        UpdateRawData("Error while processing recieved EDDN data:" + Environment.NewLine + e.Message + Environment.NewLine + e.RawData);

                        break;

                }

                if (DataRows != null && DataRows.GetUpperBound(0) >= 0)
                {
                    UpdateStatisticData(DataRows.GetUpperBound(0) + 1, nameAndVersion, e.Adress, uploaderID);

                    foreach (String DataRow in DataRows)
                    {
                        if(m_DuplicateFilter.DataAccepted(DataRow))
                        {
                            bool isTrusty = (Program.Data.BaseData.tbtrustedsenders.Rows.Find(name) != null);

                            // data is plausible ?
                            if (isTrusty || (!Program.PlausibiltyCheck.CheckPricePlausibility(new string[] { DataRow }, SimpleEDDNCheck)))
                            {

                                // import is wanted ?
                                if (Program.DBCon.getIniValue<Boolean>("EDDN", "ImportEDDN", false.ToString(), false))
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

                                if (Program.DBCon.getIniValue<Boolean>("EDDN", "SpoolImplausibleToFile", false.ToString(), false))
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
                        Program.Data.ImportPricesFromCSVStrings(importData.ToArray(), SQL.EliteDBIO.enImportBehaviour.OnlyNewer, SQL.EliteDBIO.enDataSource.fromEDDN);
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
        }

        /// <summary>
        /// register everything for sending with this function.
        /// 2 seconds after the last registration all data will be sent automatically
        /// </summary>
        /// <param name="commodityData"></param>
        public void SendMarketData(CsvRow CommodityData, enInterface usedInterface)
        {
            if(m_SenderIsActivated)
            {
                // register next data row
                if(usedInterface == enInterface.API)
                    _Send_MarketData_API.Enqueue(CommodityData);
                else
                    _Send_MarketData_OCR.Enqueue(CommodityData);

                // reset the timer
                _SendDelayTimer.Start();
            }
        }

        /// <summary>
        /// register everything for sending with this function.
        /// 2 seconds after the last registration all data will be sent automatically
        /// </summary>
        /// <param name="commodityData"></param>
        public void SendMarketData(List<CsvRow> csvRowList, enInterface usedInterface)
        {
            if(m_SenderIsActivated)
            {
                // reset the timer
                _SendDelayTimer.Start();

                // register rows
                foreach (CsvRow csvRowListItem in csvRowList)
                    if(usedInterface == enInterface.API)
                        _Send_MarketData_API.Enqueue(csvRowListItem);
                    else
                        _Send_MarketData_OCR.Enqueue(csvRowListItem);

                // reset the timer
                _SendDelayTimer.Start();
            }
        }

        /// <summary>
        /// register everything for sending with this function.
        /// 2 seconds after the last registration all data will be sent automatically
        /// </summary>
        /// <param name="commodityData"></param>
        public void SendMarketData(String[] csv_Strings, enInterface usedInterface)
        {
            if(m_SenderIsActivated)
            {
                foreach (String csvString in csv_Strings)
                {
                    // reset the timer
                    _SendDelayTimer.Start();

                    // register rows
                    foreach (String csvRowString in csv_Strings)
                        if(usedInterface == enInterface.API)
                            _Send_MarketData_API.Enqueue(new CsvRow(csvString));
                        else
                            _Send_MarketData_OCR.Enqueue(new CsvRow(csvString));

                    // reset the timer
                    _SendDelayTimer.Start();
                }
            }
        }

        

        /// <summary>
        /// to send the outfitting data of this station
        /// </summary>
        /// <param name="commodityData">json object with parsed full companion data</param>
        public void SendOutfittingData(JObject dataObject)
        {
            try
            {
                if(m_SenderIsActivated)
                {
                    String systeName   = dataObject.SelectToken("lastSystem.name").ToString();
                    String stationName = dataObject.SelectToken("lastStarport.name").ToString();

                    StringBuilder outfittingStringEDDN = new StringBuilder();

                    outfittingStringEDDN.Append(String.Format("\"message\": {{"));

                    outfittingStringEDDN.Append(String.Format("\"systemName\":\"{0}\", ",dataObject.SelectToken("lastSystem.name").ToString()));
                    outfittingStringEDDN.Append(String.Format("\"stationName\":\"{0}\", ",dataObject.SelectToken("lastStarport.name").ToString()));

                    outfittingStringEDDN.Append(String.Format("\"timestamp\":\"{0}\", ", DateTime.Now.ToString("s", CultureInfo.InvariantCulture) + DateTime.Now.ToString("zzz", CultureInfo.InvariantCulture)));

                    outfittingStringEDDN.Append(String.Format("\"modules\": ["));


                    foreach (JToken outfittingItem in dataObject.SelectTokens("lastStarport.modules.*"))
                    {
                        var nameParts = outfittingItem.SelectToken("name").ToString().Split(new char[] {'_'}).ToList();

                        var category = outfittingItem.SelectToken("category").ToString();


                        outfittingStringEDDN.Append(String.Format("\"category\":\"{0}\", ", outfittingItem.SelectToken("category").ToString()));
                        outfittingStringEDDN.Append(String.Format("\"name\":\"{0}\", ", outfittingItem.SelectToken("category").ToString()));
                        outfittingStringEDDN.Append(String.Format("\"class\":\"{0}\", ", outfittingItem.SelectToken("category").ToString()));
                        outfittingStringEDDN.Append(String.Format("\"rating\":\"{0}\", ", outfittingItem.SelectToken("category").ToString()));


                        switch (outfittingItem.SelectToken("category").ToString())
                        {
                            case "hardpoint":
                                outfittingStringEDDN.Append(String.Format("\"mount\":\"{0}\", ", outfittingItem.SelectToken("category")));
                                outfittingStringEDDN.Append(String.Format("\"guidance\":\"{0}\", ", outfittingItem.SelectToken("category")));
                                break;

                            case "utility":

                                break;

                            case "standard":
                                outfittingStringEDDN.Append(String.Format("\"ship\":\"{0}\", ", outfittingItem.SelectToken("category")));
                                break;

                            case "internal":

                                break;



                            default:
                                break;
                        }

                    } 
                    outfittingStringEDDN.Append(String.Format("]}"));

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while extracting outfitting data for eddn", ex);
            }
        }


                //"id": 128049390,
                //"category": "weapon",
                //"name": "Hpt_PulseLaser_Turret_Large",
                //"cost": 400400,
                //"sku": null

        /// <summary>
        /// internal send routine for registered data:
        /// It's called by the delay-timer "_SendDelayTimer"
        /// </summary>
        private void SendMarketData_i()
        {
            try
            {
                Queue activeQueue = null; 
                String UserID;
                EDDNCommodity_v2 Data;
                String TimeStamp;
                String commodity;

                do{

                    TimeStamp   = DateTime.Now.ToString("s", CultureInfo.InvariantCulture) + DateTime.Now.ToString("zzz", CultureInfo.InvariantCulture);
                    UserID      = UserIdentification();
                    Data        = new EDDNCommodity_v2();

                    // test or real ?
                    if (Program.DBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Schema", "Real", false) == "Test")
                        Data.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/2/test";
                    else
                        Data.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/2";

                    if(_Send_MarketData_API.Count > 0)
                    {
                        // fill the header
                        Data.Header = new EDDNCommodity_v2.Header_Class()
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
                        Data.Header = new EDDNCommodity_v2.Header_Class()
                        {
                            SoftwareName = "ED-IBE (OCR)",
                            SoftwareVersion = VersionHelper.Parts(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, 3),
                            GatewayTimestamp = TimeStamp,
                            UploaderID = UserID
                        };

                        activeQueue = _Send_MarketData_OCR;
                    }

                    // prepare the message object
                    Data.Message = new EDDNCommodity_v2.Message_Class()
                    {
                        SystemName = "",
                        StationName = "",
                        Timestamp = TimeStamp,
                        Commodities = new EDDNCommodity_v2.Commodity_Class[activeQueue.Count]
                    };

                    // collect the commodity data
                    for (int i = 0; i <= Data.Message.Commodities.GetUpperBound(0); i++)
                    {
                        CsvRow Row = (CsvRow)activeQueue.Dequeue();

                        commodity = Row.CommodityName;

                        // if it's a user added commodity send it anyhow to see that there's a unknown commodity
                        if (commodity.Equals(Program.COMMODITY_NOT_SET))
                            commodity = Row.CommodityName;

                        Data.Message.Commodities[i] = new EDDNCommodity_v2.Commodity_Class()
                        {
                            Name = commodity,
                            BuyPrice = (Int32)Math.Floor(Row.BuyPrice),
                            SellPrice = (Int32)Math.Floor(Row.SellPrice),
                            Demand = (Int32)Math.Floor(Row.Demand),
                            DemandLevel = (Row.DemandLevel == "") ? null : Row.DemandLevel,
                            Supply = (Int32)Math.Floor(Row.Supply),
                            SupplyLevel = (Row.SupplyLevel == "") ? null : Row.SupplyLevel,
                        };

                        if(!String.IsNullOrEmpty(Data.Message.Commodities[i].DemandLevel))
                            Data.Message.Commodities[i].DemandLevel         = char.ToUpper(Data.Message.Commodities[i].DemandLevel[0]) + Data.Message.Commodities[i].DemandLevel.Substring(1);

                        if(!String.IsNullOrEmpty(Data.Message.Commodities[i].SupplyLevel))
                            Data.Message.Commodities[i].SupplyLevel         = char.ToUpper(Data.Message.Commodities[i].SupplyLevel[0]) + Data.Message.Commodities[i].SupplyLevel.Substring(1);

                        if (i == 0)
                        {
                            Data.Message.SystemName = Row.SystemName;
                            Data.Message.StationName = Row.StationName;
                        }

                    }

                    using (var client = new WebClient())
                    {
                        try
                        {
                            Debug.Print(JsonConvert.SerializeObject(Data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

                            client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", JsonConvert.SerializeObject(Data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                        }
                        catch (WebException ex)
                        {
                            _logger.Log("Error uploading Json (v2)", true);
                            _logger.Log(ex.ToString(), true);
                            _logger.Log(ex.Message, true);
                            _logger.Log(ex.StackTrace, true);
                            if (ex.InnerException != null)
                                _logger.Log(ex.InnerException.ToString(), true);

                            using (WebResponse response = ex.Response)
                            {
                                using (Stream data = response.GetResponseStream())
                                {
                                    if (data != null)
                                    {
                                        StreamReader sr = new StreamReader(data);
                                        MsgBox.Show(sr.ReadToEnd(), "Error while uploading to EDDN (v2)");
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
                _logger.Log("Error uploading Json (v2)", true);
                _logger.Log(ex.ToString(), true);
                _logger.Log(ex.Message, true);
                _logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString(), true);

                cErr.processError(ex, "Error in EDDN-Sending-Thread (v2)");
            }

        }

        /// <summary>
        /// timer routine for sending all registered data to EDDN
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void SendDelayTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                // it's time to start the EDDN transmission
                _Spool2EDDN = new Thread(new ThreadStart(SendMarketData_i));
                _Spool2EDDN.Name = "Spool2EDDN";
                _Spool2EDDN.IsBackground = true;

                _Spool2EDDN.Start();

            }
            catch (Exception ex)
            {
                cErr.processError(ex, "Error while sending EDDN data");
            }
        }

        /// <summary>
        /// checks and gets the EDDN id
        /// </summary>
        private String UserIdentification()
        {
            String retValue = "";
            String userName = "";

            try
            {
                retValue = Program.DBCon.getIniValue<String>(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserID");

                if(String.IsNullOrEmpty(retValue))
                {
                    retValue = Guid.NewGuid().ToString();
                    Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserID", retValue);
                }
                    
                if (Program.DBCon.getIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Identification", "useUserName") == "useUserName")
                {
                    userName = Program.DBCon.getIniValue<String>(IBE.EDDN.EDDNView.DB_GROUPNAME, "UserName");

                    if (String.IsNullOrEmpty(userName))
                        Program.DBCon.setIniValue(IBE.EDDN.EDDNView.DB_GROUPNAME, "Identification", "useUserID");
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

#endregion
    }
}
