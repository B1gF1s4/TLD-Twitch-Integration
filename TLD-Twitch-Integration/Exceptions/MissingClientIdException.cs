namespace TLD_Twitch_Integration.Exceptions
{
	public class MissingClientIdException : Exception
	{
		public MissingClientIdException() : base("missing clientId")
		{ }
	}
}
