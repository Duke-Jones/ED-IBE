using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Threading;
using ZeroMQ;
using RegulatedNoise.Enums_and_Utility_Classes;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using CodeProject.Dialog;

namespace RegulatedNoise
{
	public class EDDN : IDisposable
	{
		private Form1 _caller;
		private Thread _Spool2EDDN;
		private readonly Queue _sendItems;
		private readonly SingleThreadLogger _logger;
		private bool _disposed;
		private bool _listening;

		public EDDN(Form1 caller)
		{

			_caller = caller;
			_logger = new SingleThreadLogger(ThreadLoggerType.EddnSubscriber);
			_sendItems = new Queue(100, 10);
			_logger.Log("Initialising...\n");
			Task.Factory.StartNew(EDDNSender, TaskCreationOptions.LongRunning);
			_logger.Log("Initialising...<OK>\n");
		}

		public void UnSubscribe()
		{
			_listening = false;
		}

		public void Subscribe()
		{
			_listening = true;
			Task.Factory.StartNew(() =>
			{
				using (var ctx = ZmqContext.Create())
				{
					using (var socket = ctx.CreateSocket(SocketType.SUB))
					{
						socket.SubscribeAll();
						socket.Connect("tcp://eddn-relay.elite-markets.net:9500");
						_caller.SetListening();
						while (!_disposed && _listening)
						{
							var byteArray = new byte[10240];
							int i = socket.Receive(byteArray, TimeSpan.FromTicks(50));
							if (i != -1)
							{
								Stream stream = new MemoryStream(byteArray);
								// Don't forget to ignore the first two bytes of the stream (!)
								stream.ReadByte();
								stream.ReadByte();
								string output;
								using (var decompressionStream = new DeflateStream(stream, CompressionMode.Decompress))
								{
									using (var sr = new StreamReader(decompressionStream))
									{
										output = sr.ReadToEnd();
									}
								}
								_caller.OutputEddnRawData(output);
							}
							Thread.Sleep(10);
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
					Thread.Sleep(1000);
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

			System.Diagnostics.Debug.Print("eddn send : " + rowToPost);

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

			string commodity = _caller.getCommodityBasename(rowToPost.CommodityName);

			if (!String.IsNullOrEmpty(commodity))
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
						_logger.Log("Error uploading Json: " + ex, true);
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
				}
			}
		}

		public void Dispose()
		{
			_disposed = true;
		}
	}
}
