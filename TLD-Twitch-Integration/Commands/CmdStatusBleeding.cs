using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusBleeding : CommandBase
	{
		public CmdStatusBleeding() : base("tti_status_bleeding")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowStatusBleeding)
				throw new RequiresRedeemRefundException(
					"Bleeding redeem is currently disabled.");

			if (GameManager.GetBloodLossComponent().GetAfflictionsCount() >= 4)
				throw new RequiresRedeemRefundException(
					"Player already has 4 or more Bleedings.");

			GameService.ShouldAddBleeding = true;

			string alert;
			if (redeem == null)
				alert = $"Bleeding applied";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
