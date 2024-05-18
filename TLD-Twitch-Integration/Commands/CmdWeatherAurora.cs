using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdWeatherAurora : CommandBase
	{
		public CmdWeatherAurora() : base("tti_weather_aurora")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowWeatherAurora)
				throw new RequiresRedeemRefundException(
					"Aurora redeem is currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			if (GameService.IsAuroraActive)
				throw new RequiresRedeemRefundException(
					"Aurora is already active.");

			if (GameService.IsAuroraFading)
				throw new RequiresRedeemRefundException(
					"Auroa is about to start or just finished.");

			ConsoleManager.CONSOLE_force_aurora();

			string alert;
			if (redeem == null)
				alert = $"Aurora started";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
