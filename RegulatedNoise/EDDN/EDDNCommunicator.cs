using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Threading;
using ZeroMQ;
using RegulatedNoise.Enums_and_Utility_Classes;
using System.Globalization;
using System.Net;
using CodeProject.Dialog;
using System.Diagnostics;
using Newtonsoft.Json;



namespace RegulatedNoise.EDDN
{
    public class EDDNCommunicator
    {

        private Form1                   _caller;
        private Thread                  _Spool2EDDN;   
        private Queue                   _SendItems = new Queue(100,10);
        private SingleThreadLogger      _logger;
        private System.Timers.Timer     _SendDelayTimer;

        public EDDNCommunicator(Form1 caller)
        { 
        
            _caller                     = caller;
            _SendDelayTimer             = new System.Timers.Timer(2000);
            _SendDelayTimer.AutoReset   = false;
            _SendDelayTimer.Elapsed     += new System.Timers.ElapsedEventHandler(this.SendDelayTimer_Elapsed);

            _logger                     = new SingleThreadLogger(ThreadLoggerType.EddnSubscriber);

        }

        public void Subscribe()
        {
            
            using (var ctx = ZmqContext.Create())
            {
                using (var socket = ctx.CreateSocket(SocketType.SUB))
                {
                    socket.SubscribeAll();

                    socket.Connect("tcp://eddn-relay.elite-markets.net:9500");

                    _caller.SetListening();

                    while (true)
                    {
                        var byteArray = new byte[10240];

                        int i = socket.Receive(byteArray, TimeSpan.FromTicks(50));

                        var decompressedFileStream = new MemoryStream();
                        if(i != -1)
                        using (decompressedFileStream)
                            {
                                Stream stream = new MemoryStream(byteArray);

                                // Don't forget to ignore the first two bytes of the stream (!)
                                stream.ReadByte();
                                stream.ReadByte();
                                using (var decompressionStream = new DeflateStream(stream, CompressionMode.Decompress))
                                {
                                    decompressionStream.CopyTo(decompressedFileStream);
                                }

                                decompressedFileStream.Position = 0;
                                var sr = new StreamReader(decompressedFileStream);
                                var myStr = sr.ReadToEnd();

                                _caller.OutputEddnRawData(myStr);
                                decompressedFileStream.Close();
                            }
                            Thread.Sleep(10);
                    }
                }
            }
// ReSharper disable once FunctionNeverReturns
        }

        /// <summary>
        /// sending routine for registered data:
        /// It's called by the delay-timer "_SendDelayTimer"
        /// </summary>
        private void  EDDNSender()
        {
            try
            {
                String          UserID;
                Schema_v2       Data            = new Schema_v2();
                String          TimeStamp;
                String          commodity; 

                TimeStamp = DateTime.Now.ToString("s", CultureInfo.InvariantCulture) + DateTime.Now.ToString("zzz", CultureInfo.InvariantCulture);

                // get the user id
                if(Form1.RegulatedNoiseSettings.usePilotsName)
                    UserID = System.Net.WebUtility.HtmlEncode(Form1.RegulatedNoiseSettings.PilotsName);
                else
                    UserID = System.Net.WebUtility.HtmlEncode(Form1.RegulatedNoiseSettings.UserName);

                // test or real ?
                if (Form1.RegulatedNoiseSettings.UseEddnTestSchema)
                    Data.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/2/test";
                else
                    Data.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/2";

                // fill the header
                Data.Header = new Schema_v2.Header_Class() {SoftwareName        = "RegulatedNoise__DJ", 
                                                            SoftwareVersion     = Form1.RegulatedNoiseSettings.getVersionString(),
                                                            GatewayTimestamp    = TimeStamp, 
                                                            UploaderID          = UserID};

                // prepare the message object
                Data.Message    = new Schema_v2.Message_Class() {SystemName     = "", 
                                                                 StationName    = "",
                                                                 Timestamp      = TimeStamp, 
                                                                 Commodities    = new Schema_v2.Commodity_Class[_SendItems.Count]};

                // collect the commodity data
                for (int i = 0; i <= Data.Message.Commodities.GetUpperBound(0); i++)
                {
                    CsvRow Row      = (CsvRow)_SendItems.Dequeue();                	

                    commodity = _caller.getCommodityBasename(Row.CommodityName);

                    // if it's a user added commodity send it anyhow to see that there's a unknown commodity
                    if(commodity.Equals(Program.COMMODITY_NOT_SET))
                        commodity = Row.CommodityName;

                    Data.Message.Commodities[i] = new Schema_v2.Commodity_Class()  {Name            = commodity, 
                                                                                    BuyPrice        = (Int32)Math.Floor(Row.BuyPrice), 
                                                                                    SellPrice       = (Int32)Math.Floor(Row.SellPrice), 
                                                                                    Demand          = (Int32)Math.Floor(Row.Demand), 
                                                                                    DemandLevel     = (Row.DemandLevel == "") ? null : Row.DemandLevel,
                                                                                    Supply          = (Int32)Math.Floor(Row.Supply), 
                                                                                    SupplyLevel     = (Row.SupplyLevel == "") ? null : Row.SupplyLevel,
                                                                                    };

                    if(i==0){
                        Data.Message.SystemName     = Row.SystemName;
                        Data.Message.StationName    = Row.StationName;

                    }

                }

                using (var client = new WebClient())
                {
                    try
                    {
                        Debug.Print(JsonConvert.SerializeObject(Data, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore}));

                        client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", JsonConvert.SerializeObject(Data, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore}));
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

            }
            catch (Exception ex)
            {
                _logger.Log("Error uploading Json (v2)", true);
                _logger.Log(ex.ToString(), true); 
                _logger.Log(ex.Message, true);
                _logger.Log(ex.StackTrace, true);
                if (ex.InnerException != null)
                    _logger.Log(ex.InnerException.ToString(), true);

                cErr.showError(ex, "Error in EDDN-Sending-Thread (v2)");
            }
	         
        }

        /// <summary>
        /// register everything for sending with this function.
        /// 2 seconds after the last registration all data will be sent automatically
        /// </summary>
        /// <param name="CommodityData"></param>
        public void sendToEdDDN(CsvRow CommodityData)
        {
            // register next data row
            _SendItems.Enqueue(CommodityData);

            // reset the timer
            _SendDelayTimer.Start();

        }
        
        /// <summary>
        /// timer routine for sending all registered data to EDDN
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void SendDelayTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            try{

                // it's time to start the EDDN transmission
                _Spool2EDDN                 = new Thread(new ThreadStart(EDDNSender));
                _Spool2EDDN.Name            = "Spool2EDDN";
                _Spool2EDDN.IsBackground    = true;

                _Spool2EDDN.Start();
                
            }catch (Exception ex){
                cErr.showError(ex, "Error while sending EDDN data");
            }
        }
    }
}
