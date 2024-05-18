using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdSound420 : CommandBase
	{
		public CmdSound420() : base("tti_sound_420")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowSound420)
				throw new RequiresRedeemRefundException(
					"420 redeem is currently disabled.");

			GameService.PlayPlayerSound("PLAY_SUFFOCATIONCOUGH");

			string alert;
			if (redeem == null)
				alert = $"Happy 420";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> Happy 420, cheers!";

			return alert;
		}
	}
}
