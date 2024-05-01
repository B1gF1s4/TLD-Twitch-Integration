using System.Net;

namespace TLD_Twitch_Integration.Exceptions
{
	public class UnhandledErrorResponseException : Exception
	{
		public UnhandledErrorResponseException(HttpStatusCode status, string msg) :
			base($"status: {status} - {msg}")
		{ }
	}
}
