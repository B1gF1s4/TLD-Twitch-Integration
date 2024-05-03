using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class WeatherRedeems
	{
		public static CustomReward Blizzard = new()
		{
			Title = RedeemNames.WEATHER_BLIZZARD,
			Prompt = "Changes the ingame weather to blizzard",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Clear = new()
		{
			Title = RedeemNames.WEATHER_CLEAR,
			Prompt = "Changes the ingame weather to clear",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward LightFog = new()
		{
			Title = RedeemNames.WEATHER_LIGHT_FOG,
			Prompt = "Changes the ingame weather to light fog",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward DenseFog = new()
		{
			Title = RedeemNames.WEATHER_DENSE_FOG,
			Prompt = "Changes the ingame weather to dense fog",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward PartlyCloudy = new()
		{
			Title = RedeemNames.WEATHER_PARTLY_CLOUDY,
			Prompt = "Changes the ingame weather to partly cloudy",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Cloudy = new()
		{
			Title = RedeemNames.WEATHER_CLOUDY,
			Prompt = "Changes the ingame weather to cloudy",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward LightSnow = new()
		{
			Title = RedeemNames.WEATHER_LIGHT_SNOW,
			Prompt = "Changes the ingame weather to light snow",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward HeavySnow = new()
		{
			Title = RedeemNames.WEATHER_HEAVY_SNOW,
			Prompt = "Changes the ingame weather to heavy snow",
			Cost = 1,
			Color = RedeemColors.WEATHER,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
