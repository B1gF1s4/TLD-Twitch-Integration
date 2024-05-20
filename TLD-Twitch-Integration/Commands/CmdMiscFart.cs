using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdMiscFart : CommandBase
	{
		public CmdMiscFart() : base("tti_misc_fart")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			throw new RequiresRedeemRefundException("redeem not implemented.");
			//if (!Settings.ModSettings.AllowMiscFart)
			//	throw new RequiresRedeemRefundException(
			//		"Fart redeem is currently disabled.");

			//GameManager.GetSuffocatingComponent().ApplySuffocatingVisualEffect();
			//GameService.PlayPlayerSound("PLAY_SUFFOCATIONCOUGH");

			//GameManager.GetToxicFogManager().AddSecondsInCurrentRegion(10.0f);
			//GameManager.GetToxicFogComponent().StartAffliction();

			//string alert;
			//if (redeem == null)
			//	alert = $"farting";
			//else
			//	alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			//return alert;
		}
	}
}
