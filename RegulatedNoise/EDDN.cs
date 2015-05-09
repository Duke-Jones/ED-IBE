using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using ZeroMQ;
using RegulatedNoise.Enums_and_Utility_Classes;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RegulatedNoise.Annotations;

namespace RegulatedNoise
{
    public class EDDN : IDisposable, INotifyPropertyChanged
    {
        public const string EDDN_OUTPUT_FILEPATH = "EddnOutput.txt";
        private const string EDDN_POST_URL = "http://eddn-gateway.elite-markets.net:8080/upload/";
        private const string EDDN_LISTEN_URL = "tcp://eddn-relay.elite-markets.net:9500";
        private const int DELAY_BETWEEN_LISTEN = 1000;
        public event EventHandler<EddnMessageEventArgs> OnMessageReceived;
        private readonly Queue _sendItems;
        private readonly SingleThreadLogger _logger;
        private bool _disposed;

        public bool Listening
        {
            get { return _listening; }
            private set
            {
                if (value == _listening) return;
                _listening = value;
                RaisePropertyChanged();
            }
        }

        public bool SaveMessagesToFile
        {
            get { return _saveMessagesToFile; }
            set
            {
                if (value == _saveMessagesToFile) return;
                _saveMessagesToFile = value;
                RaisePropertyChanged();
            }
        }

        private readonly object _listeningStateChange = new object();
        private bool _listening;
        private bool _saveMessagesToFile;

        public EDDN()
        {
            _logger = new SingleThreadLogger(ThreadLoggerType.EddnSubscriber);
            _sendItems = new Queue(100, 10);
            _logger.Log("Initialising...\n");
            Task.Factory.StartNew(EDDNSender, TaskCreationOptions.LongRunning);
            _logger.Log("Initialising...<OK>\n");
        }

        public void UnSubscribe()
        {
            lock (_listeningStateChange)
            {
                Listening = false;
            }
        }

        public void Subscribe()
        {
            lock (_listeningStateChange)
            {
                if (Listening)
                    return;
                Listening = true;

            }
            Task.Factory.StartNew(() =>
            {
                using (var ctx = ZmqContext.Create())
                {
                    using (var socket = ctx.CreateSocket(SocketType.SUB))
                    {
                        socket.SubscribeAll();
                        socket.Connect(EDDN_LISTEN_URL);
                        while (!_disposed && Listening)
                        {
                            var byteArray = new byte[10240];
                            int i = socket.Receive(byteArray, TimeSpan.FromTicks(50));
                            if (i != -1)
                            {
                                Stream stream = new MemoryStream(byteArray);
                                // Don't forget to ignore the first two bytes of the stream (!)
                                stream.ReadByte();
                                stream.ReadByte();
                                string message;
                                using (var decompressionStream = new DeflateStream(stream, CompressionMode.Decompress))
                                {
                                    using (var sr = new StreamReader(decompressionStream))
                                    {
                                        message = sr.ReadToEnd();
                                    }
                                }
                                RaiseMessageReceived(message);
                                if (SaveMessagesToFile)
                                {
                                    SaveToFile(message);
                                }
                            }
                            Thread.Sleep(DELAY_BETWEEN_LISTEN);
                        }
                    }
                }
            }, TaskCreationOptions.LongRunning);
            // ReSharper disable once FunctionNeverReturns
        }


        private void EDDNSender()
        {
            do
            {
                try
                {
                    Thread.Sleep(10000);
                    while (_sendItems.Count > 0)
                    {
                        PostJsonToEddn((CsvRow)_sendItems.Dequeue());
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log("Error uploading Json: " + ex, true);
                    cErr.ShowError(ex, "Error in EDDN-Sending-Thread");
                }

            } while (!_disposed);
        }

        public void SendToEdDdn(CsvRow commodityData)
        {
            _sendItems.Enqueue(commodityData);
        }

        private void PostJsonToEddn(CsvRow rowToPost)
        {
            string json;

            Debug.Print("eddn send : " + rowToPost);

            if (ApplicationContext.RegulatedNoiseSettings.UseEddnTestSchema)
            {
                json =
                     @"{""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1/test"",""header"": {""uploaderID"": ""$0$"",""softwareName"": ""RegulatedNoise__DJ"",""softwareVersion"": ""v" +
                     ApplicationContext.RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture) + "_" + ApplicationContext.RegulatedNoiseSettings.VersionDJ.ToString(CultureInfo.InvariantCulture) +
                     @"""},""message"": {""buyPrice"": $2$,""timestamp"": ""$3$"",""stationStock"": $4$,""stationName"": ""$5$"",""systemName"": ""$6$"",""demand"": $7$,""sellPrice"": $8$,""itemName"": ""$9$""}}";
            }
            else
            {
                json =
                     @"{""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1"",""header"": {""uploaderID"": ""$0$"",""softwareName"": ""RegulatedNoise__DJ"",""softwareVersion"": ""v" +
                     ApplicationContext.RegulatedNoiseSettings.Version.ToString(CultureInfo.InvariantCulture) + "_" + ApplicationContext.RegulatedNoiseSettings.VersionDJ.ToString(CultureInfo.InvariantCulture) +
                     @"""},""message"": {""buyPrice"": $2$,""timestamp"": ""$3$"",""stationStock"": $4$,""stationName"": ""$5$"",""systemName"": ""$6$"",""demand"": $7$,""sellPrice"": $8$,""itemName"": ""$9$""}}";
            }

            string commodity = ApplicationContext.CommoditiesLocalisation.GetCommodityBasename(rowToPost.CommodityName);

            if (!String.IsNullOrEmpty(commodity))
            {
                string commodityJson = json.Replace("$0$", rowToPost.Username.Replace("$1$", ""))
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
                        client.UploadString(EDDN_POST_URL, "POST", commodityJson);
                    }
                    catch (WebException ex)
                    {
                        _logger.Log("Error uploading Json: " + ex, true);
                        using (WebResponse response = ex.Response)
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                if (data != null)
                                {
                                    StreamReader sr = new StreamReader(data);
                                    EventBus.Alert(sr.ReadToEnd(), "Error while uploading to EDDN");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                try
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
                catch (Exception ex)
                {
                    _logger.Log(propertyName + " notification failed " + ex);
                }
        }

        protected virtual void RaiseMessageReceived(string message)
        {
            var handler = OnMessageReceived;
            if (handler != null)
                try
                {
                    handler(this, new EddnMessageEventArgs(message));
                }
                catch (Exception exception)
                {
                    _logger.Log("EDDN message notification failure: " + exception, true);
                }
            ;
        }

        private void SaveToFile(string message)
        {
            try
            {
                File.AppendAllText(EDDN_OUTPUT_FILEPATH, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _logger.Log("unable to save message to " + EDDN_OUTPUT_FILEPATH + ": " + ex);
                SaveMessagesToFile = false;
            }
        }
    }

    public class EddnMessageEventArgs : EventArgs
    {
        public readonly string Message;

        public EddnMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
