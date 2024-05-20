using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class WeatherRedeems
	{
		public static CustomReward Help = new()
		{
			Title = RedeemNames.WEATHER_HELP,
			Prompt = "Changes the ingame weather. Possible inputs: clear, fog, snow, cloudy",
			Cost = 300,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward Harm = new()
		{
			Title = RedeemNames.WEATHER_HARM,
			Prompt = "Changes the ingame weather. Possible inputs: blizzard, fog, snow",
			Cost = 1200,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward Aurora = new()
		{
			Title = RedeemNames.WEATHER_AURORA,
			Prompt = "Immediatly sets the time to midnight and starts an aurora.",
			Cost = 1200,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Time = new()
		{
			Title = RedeemNames.WEATHER_TIME,
			Prompt = "Toggles between day and night (+12h in game time).",
			Cost = 200,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
