using System;
using System.Runtime.Serialization;

namespace RegulatedNoise.Exceptions
{
	[Serializable]
	public class InitializationException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public InitializationException()
		{
		}

		public InitializationException(string message) : base(message)
		{
		}

		public InitializationException(string message, Exception inner) : base(message, inner)
		{
		}

		protected InitializationException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}