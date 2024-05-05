using HarmonyLib;
using Il2Cpp;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Patches
{
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
	internal class GameManagerUpdatePatch
	{
		internal static void Prefix()
		{
			if (ExecutionPending)
				GameState.Update();
		}

		internal static void Postfix()
		{
			if (GameService.SpawningAnimal)
				return;

			switch (GameService.AnimalToSpawn)
			{
				case AnimalRedeemType.TWolves:
					GameService.SpawnTWolves(GameState.IsAuroraActive);
					break;
				case AnimalRedeemType.Bear:
					GameService.SpawnBear(GameState.IsAuroraActive);
					break;
				case AnimalRedeemType.Moose:
					GameService.SpawnMoose();
					break;
				case AnimalRedeemType.StalkingWolf:
					GameService.SpawnStalkingWolf(GameState.IsAuroraActive);
					break;
				case AnimalRedeemType.BunnyExplosion:
					GameService.SpawnBunnyExplosion();
					break;
				default:
					return;
			}
		}
	}
}
