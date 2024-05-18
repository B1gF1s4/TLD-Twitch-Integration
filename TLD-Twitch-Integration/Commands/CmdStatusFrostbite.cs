using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusFrostbite : CommandBase
	{
		public CmdStatusFrostbite() : base("tti_status_frostbite")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowStatusFrostbite)
				throw new RequiresRedeemRefundException(
					"Frostbite redeem is currently disabled.");

			if (GameManager.GetFrostbiteComponent().GetFrostbiteAfflictionCount() >= 4)
				throw new RequiresRedeemRefundException(
					"Player already has at least 4 frostbites.");

			GameService.ShouldAddFrostbite = true;

			string alert;
			if (redeem == null)
				alert = $"Frostbite applied";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
