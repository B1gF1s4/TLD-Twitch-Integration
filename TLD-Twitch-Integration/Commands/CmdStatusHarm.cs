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

			if (!Settings.ModSettings.AllowStatusHarmTired &&
				!Settings.ModSettings.AllowStatusHarmFreezing &&
				!Settings.ModSettings.AllowStatusHarmThirsty &&
				!Settings.ModSettings.AllowStatusHarmHungry)
				throw new RequiresRedeemRefundException(
					"All meters for the status harm redeem are currently disabled.");

			var defaultStatus = Settings.ModSettings.AllowStatusHarmFreezing ? StatusMeter.Cold :
						Settings.ModSettings.AllowStatusHarmTired ? StatusMeter.Fatigue :
						Settings.ModSettings.AllowStatusHarmThirsty ? StatusMeter.Thirst : StatusMeter.Hunger;

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
