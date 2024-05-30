namespace TLD_Twitch_Integration.Exceptions
{
	public class RedeemAlreadyProcessedException : Exception
	{
		public RedeemAlreadyProcessedException(string title) :
			base($"redeem {title} is already completed or rejected")
		{ }
	}
}
