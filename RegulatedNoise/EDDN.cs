using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RegulatedNoise.Annotations;
using RegulatedNoise.Core.DomainModel;
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Enums_and_Utility_Classes;
using ZeroMQ;

namespace RegulatedNoise
{
    internal class Eddn : IDisposable, INotifyPropertyChanged
    {
        private const string POST_URL = "http://eddn-gateway.elite-markets.net:8080/upload/";
        private const string LISTEN_URL = "tcp://eddn-relay.elite-markets.net:9500";
        private const int DELAY_BETWEEN_POLL = 1000;
        public const string SOURCENAME = "EDDN";
        public event EventHandler<EddnMessageEventArgs> OnMessageReceived;
        private readonly Queue _sendItems;
        private readonly SingleThreadLogger _logger;
        private bool _disposed;

        private readonly EddnPublisherStatisticCollection _eddnPublisherStats;

        private readonly object _listeningStateChange = new object();
        private bool _listening;
        private bool _saveMessagesToFile;
        private readonly dsCommodities _commoditiesLocalisation;
        private readonly RegulatedNoiseSettings _settings;

        public IEnumerable<EddnPublisherVersionStats> PublisherStatistics
        {
            get { return _eddnPublisherStats; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [test mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [test mode]; otherwise, <c>false</c>.
        /// </value>
        public bool TestMode { get; set; }

        public void UnSubscribe()
        {
            lock (_listeningStateChange)
            {
                Listening = false;
            }
        }

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

        public Eddn([NotNull] dsCommodities commoditiesLocalisation,
            [NotNull] RegulatedNoiseSettings regulatedNoiseSettings)
        {
            if (commoditiesLocalisation == null) throw new ArgumentNullException("commoditiesLocalisation");
            if (regulatedNoiseSettings == null) throw new ArgumentNullException("regulatedNoiseSettings");
            _logger = new SingleThreadLogger(ThreadLoggerType.EddnSubscriber);
            _sendItems = new Queue(100, 10);
            _eddnPublisherStats = new EddnPublisherStatisticCollection();
            _logger.Log("Initialising...\n");
            Task.Factory.StartNew(EDDNSender, TaskCreationOptions.LongRunning);
            _logger.Log("Initialising...<OK>\n");
            OnMessageReceived += UpdateStats;
            _commoditiesLocalisation = commoditiesLocalisation;
            _settings = regulatedNoiseSettings;
        }

        private void UpdateStats(object sender, EddnMessageEventArgs e)
        {
            var nameAndVersion = (e.Message.Header.SoftwareName + " / " + e.Message.Header.SoftwareVersion);
            EddnPublisherVersionStats stats;
            if (!_eddnPublisherStats.TryGetValue(nameAndVersion, out stats))
            {
                stats = new EddnPublisherVersionStats(nameAndVersion);
                _eddnPublisherStats.Add(stats);
            }
            ++stats.MessagesReceived;
        }

        public void Subscribe()
        {
            lock (_listeningStateChange)
            {
                if (Listening)
                    return;
                Listening = true;

            }
            Task.Factory.StartNew(ListenToEddn, TaskCreationOptions.LongRunning);
            // ReSharper disable once FunctionNeverReturns
        }

        private void ListenToEddn()
        {
            using (var ctx = ZmqContext.Create())
            {
                using (var socket = ctx.CreateSocket(SocketType.SUB))
                {
                    socket.SubscribeAll();
                    socket.Connect(LISTEN_URL);
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
                            try
                            {
                                var eddnMessage = EddnMessage.ReadJson(message);
                                eddnMessage.Message.Source = SOURCENAME;
                                RaiseMessageReceived(eddnMessage);
                            }
                            catch (Exception ex)
                            {
                                _logger.Log("unable to parse message " + Environment.NewLine + message + Environment.NewLine +
                                            ex);
                                var failedMessage = new EddnMessage
                                {
                                    RawText = message,
                                    Message = {Source = SOURCENAME}
                                };
                                RaiseMessageReceived(failedMessage);
                            }
                            if (SaveMessagesToFile)
                            {
                                SaveToFile(message);
                            }
                        }
                        else
                        {
                            Thread.Sleep(DELAY_BETWEEN_POLL);
                        }
                    }
                }
            }
        }

        private void EDDNSender()
        {
            do
            {
                try
                {
                    Thread.Sleep(1000);

                    while ((_sendItems.Count > 0) && !_disposed)
                    {
                        System.Diagnostics.Debug.Print("Items in Queue : " + _sendItems.Count);
                        PostJsonToEddn((MarketDataRow)_sendItems.Dequeue());
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log("Error uploading Json: " + ex, true);
                    cErr.ShowError(ex, "Error in EDDN-Sending-Thread");
                }

            } while (!_disposed);

            _sendItems.Clear();
        }

        public void SendToEddn(MarketDataRow commodityData)
        {
            System.Diagnostics.Debug.Print("Items in Queue : " + _sendItems.Count);
            _sendItems.Enqueue(commodityData);
            System.Diagnostics.Debug.Print("Items in Queue : " + _sendItems.Count + ", added : " + commodityData);

        }

        private void PostJsonToEddn(MarketDataRow rowToPost)
        {
            Debug.Print("eddn send : " + rowToPost);
            var eddnMessage = new EddnMessage()
            {
                Header = new Header()
                {
                    SoftwareName = "RegulatedNoise__DJ"
                    ,SoftwareVersion = "v" + _settings.Version.ToString(CultureInfo.InvariantCulture) + "_" + _settings.VersionDJ.ToString(CultureInfo.InvariantCulture)
                    ,UploaderId = _settings.UserName
                }
                ,Message = rowToPost
            };
            if (_settings.UseEddnTestSchema)
            {
                eddnMessage.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/1/test";
            }
            else
            {
                eddnMessage.SchemaRef = "http://schemas.elite-markets.net/eddn/commodity/1";
            }

            string commodity = _commoditiesLocalisation.GetCommodityBasename(rowToPost.CommodityName);

            if (!String.IsNullOrEmpty(commodity))
            {
                eddnMessage.Message.CommodityName = commodity;
                var json = eddnMessage.ToJson();
                SendToEddn(json);
            }
        }

        private void SendToEddn(string json)
        {
            if (TestMode)
            {
                Debug.WriteLine("sending to eddn: " + json);
                return;
            }
            using (var client = new WebClient())
            {
                try
                {
                    client.UploadString(POST_URL, "POST", json);
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

        protected virtual void RaiseMessageReceived(EddnMessage message)
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
                File.AppendAllText(RegulatedNoiseSettings.EDDN_OUTPUT_FILEPATH, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _logger.Log("unable to save message to " + RegulatedNoiseSettings.EDDN_OUTPUT_FILEPATH + ": " + ex);
                SaveMessagesToFile = false;
            }
        }

        private class EddnPublisherStatisticCollection : KeyedCollection<string, EddnPublisherVersionStats>
        {
            protected override string GetKeyForItem(EddnPublisherVersionStats item)
            {
                return item.Publisher;
            }

            public bool TryGetValue(string publisher, out EddnPublisherVersionStats stats)
            {
                if (Dictionary != null)
                {
                    return Dictionary.TryGetValue(publisher, out stats);
                }
                else
                {
                    stats = null;
                    return false;
                }
            }
        }
    }
}
