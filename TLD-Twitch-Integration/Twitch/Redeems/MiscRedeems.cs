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
			Prompt = "Sets the specified time of day. Possible input: e.g. 08:00, 14:30, 20:52",
			Cost = 500,
			Color = RedeemColors.MISC,
			IsEnabled = false,
			IsUserInputRequired = true,
		};
	}
}
