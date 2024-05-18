using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdDevSoundCheck : CommandBase
	{
		public CmdDevSoundCheck() : base("tti_dev_sound_check")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowSound420)
				throw new RequiresRedeemRefundException(
					"Sound Check redeem is currently disabled.");

			if (redeem != null && !string.IsNullOrEmpty(redeem.UserInput))
				GameService.PlayPlayerSound(redeem.UserInput);

			string alert;
			if (redeem == null)
				alert = $"Sound Check";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
