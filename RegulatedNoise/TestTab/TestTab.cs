using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeProject.Dialog;
using Newtonsoft.Json;
using RegulatedNoise.Core.DataProviders;
using RegulatedNoise.Core.DomainModel;
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
			ApplicationContext.GalacticMarket.OnMarketDataUpdate += MarketDataUpdateEventHandler;
			_commoditiesLogs = new BindingList<MarketDataEventDisplay>();
			lbCommoditiesLog.DataSource = _commoditiesLogs;
		}

		private void MarketDataUpdateEventHandler(object sender, MarketDataEventArgs marketDataEventArgs)
		{
			this.RunInGuiThread(() =>
			{
				_commoditiesLogs.Insert(0, new MarketDataEventDisplay(marketDataEventArgs));
				lbCommoditiesLog.SelectedIndex = 0;
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

		private void btImportfromTd_Click(object sender, EventArgs e)
		{
			btImportfromTd.Enabled = false;
			var tokenSource = new CancellationTokenSource();
			var progress = new ProgressView(tokenSource) { Text = "Trade dangerous import" };
			progress.ProgressStart("retrieving trade dangerous prices...");
			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			try
			{
				ImportFromTradeDangerous(tokenSource.Token, new Progress<Tuple<string, int, int>>(report =>
				{
					if (!String.IsNullOrEmpty(report.Item1))
					{
						progress.ProgressInfo(report.Item1);
					}
					progress.ProgressUpdate(report.Item2, report.Item3);
				}))
					.ContinueWith(task =>
					{
						progress.Dispose();
						if (task.IsCanceled)
						{
							MsgBox.Show("trade dangerous import canceled");
						}
						btImportfromTd.Enabled = true;
					}, scheduler);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("exception raised: " + ex);
			}
		}

		private static async Task ImportFromTradeDangerous(CancellationToken cancellationToken, IProgress<Tuple<string, int, int>> onProgress)
		{
			var tdProvider = new TradeDangerousDataProvider();
			onProgress.Report(new Tuple<string, int, int>("downloading prices...", 1, 2));
			IEnumerable<MarketDataRow> marketDataRows = await tdProvider.RetrievePrices()
				.ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			int rows = marketDataRows.Count();
			int processed = 0;
			onProgress.Report(new Tuple<string, int, int>("importing data...", processed, rows));
			foreach (MarketDataRow marketDataRow in marketDataRows)
			{
				cancellationToken.ThrowIfCancellationRequested();
				var plausibility = ApplicationContext.Milkyway.IsImplausible(marketDataRow, true);
				if (plausibility.Plausible)
				{
					ApplicationContext.GalacticMarket.Update(marketDataRow);
				}
				++processed;
				onProgress.Report(new Tuple<string, int, int>("importing data...", processed, rows));
			}
		}

		private void btFindSystem_Click(object sender, EventArgs e)
		{
			EDSystem system = ApplicationContext.Milkyway.GetSystem(tbFinderRequest.Text);
			if (system == null)
			{
				tbFinderResult.Text = "N/A";
			}
			else
			{
				tbFinderResult.Text = JsonConvert.SerializeObject(system);
			}
		}

		private void btFindStation_Click(object sender, EventArgs e)
		{
			EDStation station = ApplicationContext.Milkyway.GetStation(MarketDataRow.StationIdToSystemName(tbFinderRequest.Text), MarketDataRow.StationIdToStationName(tbFinderRequest.Text));
			if (station == null)
			{
				tbFinderResult.Text = "N/A";
			}
			else
			{
				tbFinderResult.Text = JsonConvert.SerializeObject(station);
			}
		}

		private void btFindMarketData_Click(object sender, EventArgs e)
		{
			MarketDataRow marketData = ApplicationContext.GalacticMarket[tbFinderRequest.Text];
			if (marketData == null)
			{
				tbFinderResult.Text = "N/A";
			}
			else
			{
				tbFinderResult.Text = JsonConvert.SerializeObject(marketData);
			}
		}
	}
}
