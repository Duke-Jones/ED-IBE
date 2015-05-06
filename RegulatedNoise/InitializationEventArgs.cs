using System;

namespace RegulatedNoise
{
	public class InitializationEventArgs : EventArgs
	{
		public enum EventType
		{
			Info,
			Update,
		}

		public readonly EventType Event;

		public readonly string Message;

		public InitializationEventArgs(string message, EventType @event)
		{
			Message = message;
			Event = @event;
		}
	}
}