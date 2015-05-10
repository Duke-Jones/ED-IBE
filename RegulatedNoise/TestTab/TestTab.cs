using System;
using System.Windows.Forms;

namespace RegulatedNoise.TestTab
{
    public partial class TestTab : UserControl
    {
        public TestTab()
        {
            InitializeComponent();
            tbCustomEddnMessage.Text = @"{""header"": {""softwareVersion"": ""0.6.0.7"", ""gatewayTimestamp"": ""2015-05-09T11:39:24.342335"", ""softwareName"": ""EliteOCR"", ""uploaderID"": ""EO4d1c07c0""}, ""$schemaRef"": ""http://schemas.elite-markets.net/eddn/commodity/1"", ""message"": {""buyPrice"": 0, ""timestamp"": ""2015-05-09T11:30:49+00:00"", ""stationStock"": 0, ""systemName"": ""GANDII"", ""stationName"": ""Lu Hub"", ""demand"": 5384, ""demandLevel"": ""Low"", ""itemName"": ""Tea"", ""sellPrice"": 1463}}";
            // System;Station;Commodity;Sell;Buy;Demand;;Supply;;Date;
            tbFakeOCRResult.Text = @"GANDII;Lu Hub;Tea;10000;11000;32000;;;;2015-05-10T11:39;Source;";
        }

        private void btSendCustomEddnMessage_Click(object sender, EventArgs e)
        {
            Form1.InstanceObject.OutputEddnRawData(tbCustomEddnMessage.Text);
        }

        private void btSendFakeOCRResult_Click(object sender, EventArgs e)
        {
            //force Form1 tbFinalOcrOutput
            //then call Form1 acquisition
            Form1.InstanceObject.FakeAcquisition(tbFakeOCRResult.Text);
        }
    }
}
