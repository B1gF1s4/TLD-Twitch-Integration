using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusAfflictionCure : CommandBase
	{
		public CmdStatusAfflictionCure() : base("tti_status_affliction_cure")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowAfflictionCure)
				throw new RequiresRedeemRefundException(
					"Affliction Cure redeem is currently disabled.");

			ConsoleManager.CONSOLE_afflictions_cure();
			GameService.PlayPlayerSound("PLAY_VOCATCHBREATH");

			string alert;
			if (redeem == null)
				alert = $"all afflictions cured";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' ";

			return alert;
		}
	}
}
