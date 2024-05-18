using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusStink : CommandBase
	{
		public CmdStatusStink() : base("tti_status_stink")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowStatusStink)
				throw new RequiresRedeemRefundException(
					"Stink redeem is currently disabled.");

			GameService.ShouldAddStink = true;
			GameService.StinkValue = Settings.ModSettings.StinkLines;
			GameService.StinkStart = DateTime.UtcNow;

			string alert;
			if (redeem == null)
				alert = $"Stink applied";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> now active for {Settings.ModSettings.StinkTime}s";

			return alert;
		}
	}
}
