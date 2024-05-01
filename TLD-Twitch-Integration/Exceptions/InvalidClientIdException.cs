namespace TLD_Twitch_Integration.Exceptions
{
	public class InvalidClientIdException : Exception
	{
		public InvalidClientIdException(string id) : base($"clientId {id} is invalid")
		{ }
	}
}
