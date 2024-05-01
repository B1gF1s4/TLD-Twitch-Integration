namespace TLD_Twitch_Integration.Exceptions
{
	public class NotLoggedInException : Exception
	{
		public NotLoggedInException() : base("authorization pending or not connected")
		{ }
	}
}
