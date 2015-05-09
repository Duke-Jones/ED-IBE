using System;
using System.Diagnostics;

namespace RegulatedNoise
{
	internal static class EventBus
	{
		public static event EventHandler<NotificationEventArgs> OnInitializationProgress;

		public static void InitializationStart(string message)
		{
			RaiseInitializationEvent(message + "...", NotificationEventArgs.EventType.InitializationStart);
		}

		public static void InitializationProgress(string message)
		{
			RaiseInitializationEvent(message, NotificationEventArgs.EventType.InitializationProgress);
		}

		public static void InitializationCompleted(string message)
		{
			RaiseInitializationEvent("..." + message + "...<OK>", NotificationEventArgs.EventType.InitializationCompleted);
		}

		public static void Information(string message, string title = null)
		{
			RaiseInitializationEvent(new NotificationEventArgs(message, NotificationEventArgs.EventType.Information) { Title = title });
		}

		private static void RaiseInitializationEvent(string message, NotificationEventArgs.EventType eventType)
		{
			RaiseInitializationEvent(new NotificationEventArgs(message, eventType));
		}

		private static void RaiseInitializationEvent(NotificationEventArgs notificationEventArgs)
		{
			var handler = OnInitializationProgress;
			if (handler != null)
			{
				try
				{
					handler(null, notificationEventArgs);
				}
				catch (Exception ex)
				{
					Trace.TraceError("initialization progress failure: " + ex);
				}
			}
		}
	}
}