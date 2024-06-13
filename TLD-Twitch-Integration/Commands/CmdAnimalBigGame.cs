using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdAnimalBigGame : CommandBase
	{

		public CmdAnimalBigGame() : base("tti_animal_big")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowBigGame)
				throw new RequiresRedeemRefundException(
					"Big game redeem is currently disabled.");

			if (!Settings.ModSettings.AllowBigGameBear &&
				!Settings.ModSettings.AllowBigGameMoose)
				throw new RequiresRedeemRefundException(
					"All animal types for the big game redeem are currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			if (GameService.IsAuroraFading)
				return "";

			var prefabName = Settings.ModSettings.AllowBigGameBear ?
				"WILDLIFE_Bear" : "WILDLIFE_Moose";

			if (redeem != null)
			{
				var userInputAnimal = string.IsNullOrEmpty(redeem.UserInput) ?
					prefabName : redeem.UserInput.ToLower();

				prefabName = userInputAnimal.Contains("bear") ? "WILDLIFE_Bear" :
					userInputAnimal.Contains("moose") ? "WILDLIFE_Moose" : prefabName;
			}

			if (prefabName.Contains("Bear") && !Settings.ModSettings.AllowBigGameBear)
				prefabName = "WILDLIFE_Moose";

			if (prefabName.Contains("Moose") && !Settings.ModSettings.AllowBigGameMoose)
				prefabName = "WILDLIFE_Bear";

			var dist = prefabName.Contains("Bear") ?
				Settings.ModSettings.DistanceBear :
				Settings.ModSettings.DistanceMoose;

			if (GameService.IsAuroraActive)
			{
				if (prefabName.Contains("Moose"))
					return "";
				else
					prefabName += "_Aurora";
			}

			var spawned = AnimalService.Spawn(prefabName, dist);
			if (!spawned)
				return "";

			string alert;
			if (redeem == null)
				alert = $"spawning {prefabName}";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {redeem.UserInput}";

			return alert;
		}
	}
}
