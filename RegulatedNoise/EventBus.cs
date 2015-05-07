using System;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace RegulatedNoise
{
	internal static class EventBus
	{
		public static event EventHandler<NotificationEventArgs> OnNotificationEvent;

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

		public static bool Request(string message, string title = null)
		{
			return !RaiseInitializationEvent(new NotificationEventArgs(message, NotificationEventArgs.EventType.Request) { Title = title }).Cancel;
		}

		public static string FileRequest(string message, string title = null)
		{
			return
				RaiseInitializationEvent(new NotificationEventArgs(message, NotificationEventArgs.EventType.FileRequest)
				{
					Title = title
				}).Response;
		}

		private static NotificationEventArgs RaiseInitializationEvent(string message, NotificationEventArgs.EventType eventType)
		{
			return RaiseInitializationEvent(new NotificationEventArgs(message, eventType));
		}

		private static NotificationEventArgs RaiseInitializationEvent(NotificationEventArgs notificationEventArgs)
		{
			var handler = OnNotificationEvent;
			if (handler != null)
			{
				try
				{
					handler(null, notificationEventArgs);
				}
				catch (Exception ex)
				{
					Trace.TraceError("event notification failure: " + ex);
				}
			}
			return notificationEventArgs;
		}
	}
}