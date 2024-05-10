using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public class MiscRedeems
	{
		public static CustomReward Teleport = new()
		{
			Title = RedeemNames.MISC_TELEPORT,
			Prompt = "Teleports the player to specified location. Possible inputs: [n/a]",
			Cost = 10000,
			Color = RedeemColors.MISC,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward Time = new()
		{
			Title = RedeemNames.MISC_TIME,
			Prompt = "Toggles between day and night (+12h in game time)",
			Cost = 500,
			Color = RedeemColors.MISC,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
