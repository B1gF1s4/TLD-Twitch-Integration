using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class SoundRedeems
	{
		public static CustomReward Happy420 = new()
		{
			Title = RedeemNames.SOUND_420,
			Prompt = "Plays the SUFFOCATIONCOUGH player sound.",
			Cost = 420,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
