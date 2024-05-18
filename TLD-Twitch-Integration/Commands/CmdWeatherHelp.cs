using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdWeatherHelp : CommandBase
	{
		public CmdWeatherHelp() : base("tti_weather_help")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowWeatherHelp)
				throw new RequiresRedeemRefundException(
					"Weather help redeem is currently disabled.");

			if (!Settings.ModSettings.AllowWeatherHelpClear &&
				!Settings.ModSettings.AllowWeatherHelpCloudy &&
				!Settings.ModSettings.AllowWeatherHelpFog &&
				!Settings.ModSettings.AllowWeatherHelpSnow)
				throw new RequiresRedeemRefundException(
					"All weather types for the weather help redeem are currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			var defaultWeather = Settings.ModSettings.AllowWeatherHelpClear ? WeatherStage.Clear :
				Settings.ModSettings.AllowWeatherHelpFog ? WeatherStage.LightFog :
				Settings.ModSettings.AllowWeatherHelpSnow ? WeatherStage.LightSnow : WeatherStage.PartlyCloudy;

			var userInput = defaultWeather.ToString().ToLower();
			if (redeem != null && !string.IsNullOrEmpty(redeem.UserInput))
				userInput = redeem.UserInput.ToLower();

			var weatherToSet = userInput.Contains("clear") ? WeatherStage.Clear :
				userInput.Contains("fog") ? WeatherStage.LightFog :
				userInput.Contains("snow") ? WeatherStage.LightSnow :
				userInput.Contains("cloudy") ? WeatherStage.PartlyCloudy : defaultWeather;

			if (weatherToSet == WeatherStage.Clear &&
				!Settings.ModSettings.AllowWeatherHelpClear)
				weatherToSet = defaultWeather;

			if (weatherToSet == WeatherStage.LightFog &&
				!Settings.ModSettings.AllowWeatherHelpFog)
				weatherToSet = defaultWeather;

			if (weatherToSet == WeatherStage.LightSnow &&
				!Settings.ModSettings.AllowWeatherHelpSnow)
				weatherToSet = defaultWeather;

			if (weatherToSet == WeatherStage.PartlyCloudy &&
				!Settings.ModSettings.AllowWeatherHelpCloudy)
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
