using System;
using System.ComponentModel;
using System.Windows.Forms;
using RegulatedNoise.DomainModel;
using RegulatedNoise.EDDB_Data;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise.TestTab
{
    public partial class TestTab : UserControl
    {
        private class MarketDataEventDisplay
        {
            private readonly string _display;

            public MarketDataEventDisplay(MarketDataEventArgs marketDataEventArgs)
            {
                _display = String.Format("[{0}] {1}", DisplayStatus(marketDataEventArgs), marketDataEventArgs.Actual);
            }

            private string DisplayStatus(MarketDataEventArgs marketDataEventArgs)
            {
                if (marketDataEventArgs.IsAdded)
                {
                    return "A";
                }
                else if (marketDataEventArgs.IsRemoved)
                {
                    return "D";
                }
                else if (marketDataEventArgs.IsReplaced)
                {
                    return "S";
                }
                else
                {
                    return "?";
                }
            }

            public override string ToString()
            {
                return _display;
            }
        }

        private readonly BindingList<MarketDataEventDisplay> _commoditiesLogs;

        public event EventHandler<EddnMessageEventArgs> OnFakeEddnMessage; 
        public TestTab()
        {
            InitializeComponent();
            tbCustomEddnMessage.Text = @"{""header"": {""softwareVersion"": ""0.6.0.7"", ""gatewayTimestamp"": ""2015-05-09T11:39:24.342335"", ""softwareName"": ""EliteOCR"", ""uploaderID"": ""EO4d1c07c0""}, ""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1"", ""message"": {""buyPrice"": 0, ""timestamp"": ""2015-05-09T11:30:49+00:00"", ""stationStock"": 0, ""systemName"": ""GANDII"", ""stationName"": ""Lu Hub"", ""demand"": 5384, ""demandLevel"": ""Low"", ""itemName"": ""Tea"", ""sellPrice"": 1463}}";
            // System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;
            tbFakeOCRResult.Text = @"GANDII;Lu Hub;Tea;10000;11000;32000;;;;2015-05-10T11:39;Source;";
            ApplicationContext.GlobalMarket.OnMarketDataUpdate += MarketDataUpdateEventHandler;
            _commoditiesLogs = new BindingList<MarketDataEventDisplay>();
            lbCommoditiesLog.DataSource = _commoditiesLogs;
        }

        private void MarketDataUpdateEventHandler(object sender, MarketDataEventArgs marketDataEventArgs)
        {
            this.RunInGuiThread(() =>
            {
                _commoditiesLogs.Add(new MarketDataEventDisplay(marketDataEventArgs));
            });
        }

        private void btSendCustomEddnMessage_Click(object sender, EventArgs e)
        {
            EddnMessage eddnMessage = null;
            try
            {
                eddnMessage = EddnMessage.ReadJson(tbCustomEddnMessage.Text);
            }
            catch
            {
                eddnMessage = new EddnMessage();
            }
            eddnMessage.RawText = tbCustomEddnMessage.Text;
            RaiseFakeEddnMessage(new EddnMessageEventArgs(eddnMessage));
        }

        private void btSendFakeOCRResult_Click(object sender, EventArgs e)
        {
            //force Form1 tbFinalOcrOutput
            //then call Form1 acquisition
            Form1.InstanceObject.FakeAcquisition(tbFakeOCRResult.Text);
        }

        protected virtual void RaiseFakeEddnMessage(EddnMessageEventArgs e)
        {
            var handler = OnFakeEddnMessage;
            if (handler != null) handler(this, e);
        }
    }
}
