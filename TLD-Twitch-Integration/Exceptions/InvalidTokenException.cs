namespace TLD_Twitch_Integration.Exceptions
{
	public class InvalidTokenException : Exception
	{
		public InvalidTokenException() : base("token invalid")
		{ }
	}
}
