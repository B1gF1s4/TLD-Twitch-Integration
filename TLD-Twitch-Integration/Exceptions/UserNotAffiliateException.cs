namespace TLD_Twitch_Integration.Exceptions
{
	public class UserNotAffiliateException : Exception
	{
		public UserNotAffiliateException(string userName) :
			base($"{userName} is not at least affiliate. custom rewards are not enabled.")
		{ }
	}
}
