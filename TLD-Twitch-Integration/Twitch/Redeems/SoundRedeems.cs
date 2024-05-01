using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class SoundRedeems
	{
		public static CustomReward Hello = new()
		{
			Title = RedeemNames.SOUND_HELLO,
			Prompt = "Plays the TODDAWN player sound",
			Cost = 1,
			Color = RedeemColors.SOUND,
			IsEnabled = false
		};

		public static CustomReward GoodNight = new()
		{
			Title = RedeemNames.SOUND_GOOD_NIGHT,
			Prompt = "Plays the FATIGUEYAWN player sound",
			Cost = 1,
			Color = RedeemColors.SOUND,
			IsEnabled = false
		};

		public static CustomReward Happy420 = new()
		{
			Title = RedeemNames.SOUND_420,
			Prompt = "Plays the SUFFOCATIONCOUGH player sound",
			Cost = 1,
			Color = RedeemColors.SOUND,
			IsEnabled = false
		};

		public static CustomReward Hydrate = new()
		{
			Title = RedeemNames.SOUND_HYDRATE,
			Prompt = "Plays the VERYTHIRSTY player sound",
			Cost = 1,
			Color = RedeemColors.SOUND,
			IsEnabled = false
		};
	}
}
