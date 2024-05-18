using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusHelp : CommandBase
	{
		public CmdStatusHelp() : base("tti_status_help")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowStatusHelp)
				throw new RequiresRedeemRefundException(
					"Status help redeem is currently disabled.");

			if (!Settings.ModSettings.AllowStatusHelpAwake &&
				!Settings.ModSettings.AllowStatusHelpFull &&
				!Settings.ModSettings.AllowStatusHelpNotThirsty &&
				!Settings.ModSettings.AllowStatusHelpWarm)
				throw new RequiresRedeemRefundException(
					"All meters for the status help redeem are currently disabled.");

			var defaultStatus = Settings.ModSettings.AllowStatusHelpWarm ? StatusMeter.Cold :
						Settings.ModSettings.AllowStatusHelpAwake ? StatusMeter.Fatigue :
						Settings.ModSettings.AllowStatusHelpNotThirsty ? StatusMeter.Thirst : StatusMeter.Hunger;

			var userInput = defaultStatus.ToString().ToLower();
			if (redeem != null && !string.IsNullOrEmpty(redeem.UserInput))
				userInput = redeem.UserInput.ToLower();

			var statusToSet = userInput.Contains("cold") ? StatusMeter.Cold :
				userInput.Contains("fatigue") ? StatusMeter.Fatigue :
				userInput.Contains("thirst") ? StatusMeter.Thirst :
				userInput.Contains("hunger") ? StatusMeter.Hunger : defaultStatus;

			if (statusToSet == StatusMeter.Cold &&
				!Settings.ModSettings.AllowStatusHelpWarm)
				statusToSet = defaultStatus;

			if (statusToSet == StatusMeter.Fatigue &&
				!Settings.ModSettings.AllowStatusHelpAwake)
				statusToSet = defaultStatus;

			if (statusToSet == StatusMeter.Thirst &&
				!Settings.ModSettings.AllowStatusHelpNotThirsty)
				statusToSet = defaultStatus;

			if (statusToSet == StatusMeter.Hunger &&
				!Settings.ModSettings.AllowStatusHelpFull)
				statusToSet = defaultStatus;

			GameService.StatusMeterToChange = statusToSet;
			GameService.IsHelpfulStatusMeterChange = true;

			string alert;
			if (redeem == null)
				alert = $"status help - '{statusToSet}'";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {statusToSet}";

			return alert;
		}
	}
}
