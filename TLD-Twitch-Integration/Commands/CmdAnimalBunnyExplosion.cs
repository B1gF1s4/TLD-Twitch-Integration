using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdAnimalBunnyExplosion : CommandBase
	{
		public CmdAnimalBunnyExplosion() : base("tti_animal_bunny")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowBunnyExplosion)
				throw new RequiresRedeemRefundException(
					"Bunny explosion redeem is currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			var prefabName = "WILDLIFE_Rabbit";
			var dist = 7f;

			var nothingSpawned = true;
			for (int i = 0; i < Settings.ModSettings.BunnyCount; i++)
			{
				var spawned = AnimalService.Spawn(prefabName, dist);
				if (spawned)
					nothingSpawned = false;
			}

			if (nothingSpawned)
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
