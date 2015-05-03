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

namespace RegulatedNoise
{
    public class EDDN
    {
        private Form1 _caller;
        private Thread _Spool2EDDN;   
        private Queue _SendItems = new Queue(100,10);
        private SingleThreadLogger _logger;

        public EDDN(Form1 caller)
        { 
        
            _caller = caller;

            _logger = new SingleThreadLogger(ThreadLoggerType.EddnSubscriber);
            _logger.Log("Initialising...\n");

            _Spool2EDDN = new Thread(new ThreadStart(EDDNSender));
            _Spool2EDDN.Name = "Spool2EDDN";
            _Spool2EDDN.Start();

            _logger.Log("Initialising...<OK>\n");

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


        private void  EDDNSender()
        {
            do
	        {
                try
                {
                    
                    Thread.Sleep(1000);

                    while (_SendItems.Count > 0)
                    {
                        PostJsonToEddn((CsvRow)_SendItems.Dequeue());    
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log("Error uploading Json", true);
                    _logger.Log(ex.ToString(), true);
                    _logger.Log(ex.Message, true);
                    _logger.Log(ex.StackTrace, true);
                    if (ex.InnerException != null)
                        _logger.Log(ex.InnerException.ToString(), true);

                    cErr.showError(ex, "Error in EDDN-Sending-Thread");
                }
	         
	        } while ((!_caller.IsDisposed) && (!_caller.Disposing));
        }

        public void sendToEdDDN(CsvRow CommodityData)
        {
            _SendItems.Enqueue(CommodityData);
        }

        private void PostJsonToEddn(CsvRow rowToPost)
        {
            string json;

            System.Diagnostics.Debug.Print("eddn send : " + rowToPost.ToString());

            if (Form1.RegulatedNoiseSettings.UseEddnTestSchema)
            {
                json =
                    @"{""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1/test"",""header"": {""uploaderID"": ""$0$"",""softwareName"": ""RegulatedNoise__DJ"",""softwareVersion"": ""v" +
                    Form1.RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture) + "_" + Form1.RegulatedNoiseSettings.VersionDJ.ToString(CultureInfo.InvariantCulture) +
                    @"""},""message"": {""buyPrice"": $2$,""timestamp"": ""$3$"",""stationStock"": $4$,""stationName"": ""$5$"",""systemName"": ""$6$"",""demand"": $7$,""sellPrice"": $8$,""itemName"": ""$9$""}}";
            }
            else
            {
                json =
                    @"{""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1"",""header"": {""uploaderID"": ""$0$"",""softwareName"": ""RegulatedNoise__DJ"",""softwareVersion"": ""v" +
                    Form1.RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture) + "_" + Form1.RegulatedNoiseSettings.VersionDJ.ToString(CultureInfo.InvariantCulture) +
                    @"""},""message"": {""buyPrice"": $2$,""timestamp"": ""$3$"",""stationStock"": $4$,""stationName"": ""$5$"",""systemName"": ""$6$"",""demand"": $7$,""sellPrice"": $8$,""itemName"": ""$9$""}}";
             }

            string commodity = _caller.getCommodityBasename(rowToPost.CommodityName);

            if(!String.IsNullOrEmpty(commodity))
            {
                string commodityJson = json.Replace("$0$", _caller.tbUsername.Text.Replace("$1$", ""))
                    .Replace("$2$", (rowToPost.BuyPrice.ToString(CultureInfo.InvariantCulture)))
                    .Replace("$3$", (rowToPost.SampleDate.ToString("s", CultureInfo.CurrentCulture)))
                    .Replace("$4$", (rowToPost.Supply.ToString(CultureInfo.InvariantCulture)))
                    .Replace("$5$", (rowToPost.StationID.Replace(" [" + rowToPost.SystemName + "]", "")))
                    .Replace("$6$", (rowToPost.SystemName))
                    .Replace("$7$", (rowToPost.Demand.ToString(CultureInfo.InvariantCulture)))
                    .Replace("$8$", (rowToPost.SellPrice.ToString(CultureInfo.InvariantCulture)))
                    .Replace("$9$", (commodity)
                    );

                using (var client = new WebClient())
                {
                    try
                    {
                        client.UploadString("http://eddn-gateway.elite-markets.net:8080/upload/", "POST", commodityJson);
                    }
                    catch (WebException ex)
                    {
                        _logger.Log("Error uploading Json", true);
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
                                    MsgBox.Show(sr.ReadToEnd(), "Error while uploading to EDDN");
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
        }
    }
}
