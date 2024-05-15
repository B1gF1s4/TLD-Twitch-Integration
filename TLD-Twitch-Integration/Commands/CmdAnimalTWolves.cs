using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public static class CmdAnimalTWolves
	{
		public static void AddCommandToConsole()
		{
			uConsole.RegisterCommand("tti_animal_twolf", new Action(() =>
			{
				Execute();
			}));
		}

		public static string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowTWolves)
				throw new RequiresRedeemRefundException(
					"TWolf redeem is currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			if (GameService.IsAuroraFading)
				return "";

			int packSize = 5;
			if (redeem != null)
			{
				packSize = int.TryParse(redeem.UserInput, out var number) ?
					(number >= 2 && number <= 5) ? number : 5 : 5;
			}

			var prefabName = "WILDLIFE_Wolf_grey";
			if (GameService.IsAuroraActive)
				prefabName += "_aurora";

			var dist = Settings.ModSettings.DistanceTWolf;

			var nothingSpawned = true;
			for (int i = 0; i < packSize; i++)
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
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> Pack of {packSize}";

			return alert;
		}
	}
}
