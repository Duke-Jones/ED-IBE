using System;
using System.Drawing;
using System.Threading;
using RegulatedNoise.Enums_and_Utility_Classes;

namespace RegulatedNoise
{
	public partial class ProgressView : RNBaseForm
	{
		readonly PerformanceTimer _pTimer;
		private readonly CancellationTokenSource _cancellationTokenSource;
		private bool _canceled;

		public ProgressView(CancellationTokenSource cancellationToken = null)
		{
			InitializeComponent();
			_canceled = false;
			_cancellationTokenSource = cancellationToken;
			_pTimer = new PerformanceTimer();
			_pTimer.startMeasuring();
		}

		public IProgress<Tuple<string, int, int>> NewProgress()
		{
			return new Progress<Tuple<string, int, int>>(report =>
			{
				if (!String.IsNullOrEmpty(report.Item1))
				{
					ProgressInfo(report.Item1);
				}
				ProgressUpdate(report.Item2, report.Item3);
			});
		}

		// <summary>
		// shows the progress view
		// </summary>
		// <param name="Info"></param>
		// <remarks></remarks>
		public void ProgressStart(string info = "")
		{
			Show();
			ProgressUpdate(0);
			ProgressInfo(info);
			TopMost = true;
		}

		// <summary>
		// set the progress to a new value
		// </summary>
		// <param name="Value">progress current value</param>
		// <param name="Total">total value</param>
		// <remarks></remarks>
		public void ProgressUpdate(int current, int total)
		{
			int prozValue;
			if ((total > 0))
			{
				prozValue = (int)Math.Round(current / (double)(total) * 100.0, 0);

				if ((prozValue < 0))
				{
					prozValue = 0;
				}
				else if ((prozValue > 100))
				{
					prozValue = 100;
				}
			}
			else
			{
				prozValue = 0;
			}
			if (_pTimer.currentMeasuring() >= 50)
			{
				_pTimer.startMeasuring();
				ProgressUpdate(prozValue);
			}
		}

		// <summary>
		// set the progress to a new value
		// </summary>
		// <param name="Percent">progress in percent</param>
		// <remarks></remarks>
		private void ProgressUpdate(int percent)
		{
			this.RunInGuiThread(() =>
			{
				ProgressBar1.Value = percent;
				if (percent > 0 && percent < 100)
					ProgressBar1.Value = percent - 1;
				lblProgress.Text = string.Format("{0}%", percent);
				ProgressBar1.Refresh();
			});
		}

		// <summary>
		// sets a new info string
		// </summary>
		// <param name="Info">new information</param>
		// <remarks></remarks>
		public void ProgressInfo(string info)
		{
			this.RunInGuiThread(() =>
			{
				if (string.IsNullOrEmpty(info))
				{
					Height = 125;
				}
				else
				{
					Height = 161;
				}
				lblInfotext.Text = info;
			});
		}

		// <summary>
		// closes the progress view
		// </summary>
		// <remarks></remarks>
		public void ProgressStop()
		{
			this.RunInGuiThread(Close);
		}

		// <summary>
		// show if the user has "cancel" clicked
		// </summary>
		// <value></value>
		// <returns></returns>
		// <remarks></remarks>
		public bool Canceled
		{
			get { return _canceled; }
			private set
			{
				if (value)
				{
					_canceled = true;
					Cancel();
				}
			}
		}

		/// <summary>
		/// event function of cancel button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Canceled = true;
		}

		private void Cancel()
		{
			cmdCancel.Enabled = false;
			lblInfotext.Text = "canceling " + lblInfotext.Text;
			if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
			{
				_cancellationTokenSource.Cancel();
			}
		}
	}
}
