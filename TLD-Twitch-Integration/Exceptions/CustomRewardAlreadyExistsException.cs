namespace TLD_Twitch_Integration.Exceptions
{
	public class CustomRewardAlreadyExistsException : Exception
	{
		public CustomRewardAlreadyExistsException(string name) :
			base($"the custom reward {name} already exists")
		{ }
	}
}
