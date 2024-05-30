using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public class DevRedeems
	{
		public static CustomReward SoundCheck = new()
		{
			Title = RedeemNames.DEV_SOUND,
			Prompt = "Plays the sound ressource from input.",
			Cost = 1,
			Color = RedeemColors.SOUND,
			IsEnabled = false,
			IsUserInputRequired = true,
			IsCooldownEnabled = false,
			CooldownInSeconds = 0,
		};
	}
}
