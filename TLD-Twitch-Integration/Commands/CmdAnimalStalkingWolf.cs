﻿using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
    public class CmdAnimalStalkingWolf : CommandBase
	{
		public CmdAnimalStalkingWolf() : base("tti_animal_wolf")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowStalkingWolf)
				throw new RequiresRedeemRefundException(
					"Stalking wolf redeem is currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			if (GameService.IsAuroraFading)
				return "";

			var prefabName = "WILDLIFE_Wolf";
			if (GameService.IsAuroraActive)
				prefabName += "_Aurora";

			var dist = Settings.ModSettings.DistanceStalkingWolf;

			var spawned = AnimalService.Spawn(prefabName, -dist);
			if (!spawned)
				return "";

			string alert;
			if (redeem == null)
				alert = $"spawning {prefabName}";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
