using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusSprain : CommandBase
	{
		private readonly Random _random = new();

		public CmdStatusSprain() : base("tti_status_sprain")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowStatusSprain)
				throw new RequiresRedeemRefundException(
					"Sprain redeem is currently disabled.");

			var isAnkle = true;
			if (Settings.ModSettings.AllowStatusSprainWrists)
				isAnkle = _random.NextDouble() > 0.5;

			if (isAnkle)
			{
				if (GameManager.GetSprainedAnkleComponent().GetAfflictionsCount() >= 2)
					throw new RequiresRedeemRefundException(
						"Player already has 2 sprained ankles.");
			}
			else
			{
				if (GameManager.GetSprainedWristComponent().GetAfflictionsCount() >= 2)
					throw new RequiresRedeemRefundException(
						"Player already has 2 sprained wrists.");
			}

			GameService.ShouldAddSprain = true;
			GameService.SprainIsAnkle = isAnkle;

			string alert;
			var lastMsgPart = isAnkle ? "Ankle" : "Wrist";
			if (redeem == null)
				alert = $"Sprain applied - {lastMsgPart}";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {lastMsgPart}";

			return alert;
		}
	}
}
