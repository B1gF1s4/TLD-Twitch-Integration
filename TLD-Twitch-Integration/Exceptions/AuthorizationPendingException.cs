namespace TLD_Twitch_Integration.Exceptions
{
	public class AuthorizationPendingException : Exception
	{
		public AuthorizationPendingException(string verificationUri) :
			base($"authorization pending. please visit: {verificationUri}")
		{ }
	}
}
