using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class MiscRedeems
	{
		public static CustomReward Happy420 = new()
		{
			Title = RedeemNames.MISC_420,
			Prompt = "Cheers, my firend!",
			Cost = 420,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Fart = new()
		{
			Title = RedeemNames.MISC_FART,
			Prompt = "No, you!",
			Cost = 1,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
