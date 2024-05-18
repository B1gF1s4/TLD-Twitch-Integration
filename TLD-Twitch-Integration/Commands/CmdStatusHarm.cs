using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusHarm : CommandBase
	{
		public CmdStatusHarm() : base("tti_status_harm")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowStatusHarm)
				throw new RequiresRedeemRefundException(
					"Status harm redeem is currently disabled.");

			if (!Settings.ModSettings.AllowStatusHelpAwake &&
				!Settings.ModSettings.AllowStatusHelpFull &&
				!Settings.ModSettings.AllowStatusHelpNotThirsty &&
				!Settings.ModSettings.AllowStatusHelpWarm)
				throw new RequiresRedeemRefundException(
					"All meters for the status harm redeem are currently disabled.");

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
				!Settings.ModSettings.AllowStatusHarmFreezing)
				statusToSet = defaultStatus;

			if (statusToSet == StatusMeter.Fatigue &&
				!Settings.ModSettings.AllowStatusHarmTired)
				statusToSet = defaultStatus;

			if (statusToSet == StatusMeter.Thirst &&
				!Settings.ModSettings.AllowStatusHarmThirsty)
				statusToSet = defaultStatus;

			if (statusToSet == StatusMeter.Hunger &&
				!Settings.ModSettings.AllowStatusHarmHungry)
				statusToSet = defaultStatus;

			GameService.StatusMeterToChange = statusToSet;
			GameService.IsHelpfulStatusMeterChange = false;

			string alert;
			if (redeem == null)
				alert = $"status harm - '{statusToSet}'";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {statusToSet}";

			return alert;
		}
	}
}
