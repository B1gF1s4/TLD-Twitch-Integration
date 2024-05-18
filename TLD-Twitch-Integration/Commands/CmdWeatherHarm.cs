using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdWeatherHarm : CommandBase
	{
		public CmdWeatherHarm() : base("tti_weather_harm")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowWeatherHelp)
				throw new RequiresRedeemRefundException(
					"Weather help redeem is currently disabled.");

			if (!Settings.ModSettings.AllowWeatherHarmBlizzard &&
				!Settings.ModSettings.AllowWeatherHarmFog &&
				!Settings.ModSettings.AllowWeatherHarmSnow)
				throw new RequiresRedeemRefundException(
					"All weather types for the weather harm redeem are currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			var defaultWeather = Settings.ModSettings.AllowWeatherHarmBlizzard ? WeatherStage.Blizzard :
				Settings.ModSettings.AllowWeatherHarmFog ? WeatherStage.DenseFog : WeatherStage.HeavySnow;

			var userInput = defaultWeather.ToString().ToLower();
			if (redeem != null && !string.IsNullOrEmpty(redeem.UserInput))
				userInput = redeem.UserInput.ToLower();

			var weatherToSet = userInput.Contains("blizzard") ? WeatherStage.Blizzard :
				userInput.Contains("fog") ? WeatherStage.DenseFog :
				userInput.Contains("snow") ? WeatherStage.HeavySnow : defaultWeather;

			if (weatherToSet == WeatherStage.Blizzard && !Settings.ModSettings.AllowWeatherHarmBlizzard)
				weatherToSet = defaultWeather;

			if (weatherToSet == WeatherStage.DenseFog && !Settings.ModSettings.AllowWeatherHarmFog)
				weatherToSet = defaultWeather;

			if (weatherToSet == WeatherStage.HeavySnow && !Settings.ModSettings.AllowWeatherHarmSnow)
				weatherToSet = defaultWeather;

			GameService.WeatherToChange = weatherToSet;

			string alert;
			if (redeem == null)
				alert = $"weather changing to '{weatherToSet}'";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {weatherToSet}";

			return alert;
		}
	}
}
