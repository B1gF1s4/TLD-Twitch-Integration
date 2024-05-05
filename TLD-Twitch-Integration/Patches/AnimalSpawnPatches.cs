using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Patches
{

	[HarmonyPatch(typeof(BaseAiManager), nameof(BaseAiManager.Add), new Type[] { typeof(BaseAi) })]
	internal class BaseAiManager_Add
	{
		internal static void Postfix(BaseAi bai)
		{
			if (bai == null)
				return;

			var cam = GameManager.GetCurrentCamera().transform;
			var distance = Settings.ModSettings.AnimalSpawnDistance;

			switch (GameService.AnimalToSpawn)
			{
				case AnimalRedeemType.TWolves:
					var animalPosPack = cam.position + cam.forward * distance;
					animalPosPack.y += 100;
					AiUtils.FindNearestGroundAndNavmeshFor(animalPosPack, out var groundPosPack, out var navmeshPosPack);
					bai.SetPosition(groundPosPack);
					GameService.SpawnedAnimalCounter++;
					if (GameService.SpawnedAnimalCounter >= GameService.TWolfPackSize)
					{
						GameService.SpawningAnimal = false;
						GameService.AnimalToSpawn = AnimalRedeemType.None;
					}
					break;

				case AnimalRedeemType.Bear:
				case AnimalRedeemType.Moose:
					var animalPos = cam.position + cam.forward * distance;
					animalPos.y += 100;
					AiUtils.FindNearestGroundAndNavmeshFor(animalPos, out var groundPos, out var navmeshPos);
					bai.SetPosition(groundPos);
					GameService.SpawningAnimal = false;
					GameService.AnimalToSpawn = AnimalRedeemType.None;
					break;

				case AnimalRedeemType.StalkingWolf:
					var animalPosBehind = cam.position - cam.forward * (distance / 2);
					animalPosBehind.y += 100;
					AiUtils.FindNearestGroundAndNavmeshFor(animalPosBehind, out var groundPosBehind, out var navmeshPosBehind);
					bai.SetPosition(groundPosBehind);
					GameService.SpawningAnimal = false;
					GameService.AnimalToSpawn = AnimalRedeemType.None;
					break;

				case AnimalRedeemType.BunnyExplosion:
					var animalPosExplosion = cam.position + cam.forward * (distance / 4);
					animalPosExplosion.y += 100;
					AiUtils.FindNearestGroundAndNavmeshFor(animalPosExplosion, out var groundPosExplosion, out var navmeshPosExplosion);
					bai.SetPosition(groundPosExplosion);
					GameService.SpawnedAnimalCounter++;
					if (GameService.SpawnedAnimalCounter >= GameService.BunnyExplosionSize)
					{
						GameService.SpawningAnimal = false;
						GameService.AnimalToSpawn = AnimalRedeemType.None;
					}
					break;

				default:
					return;
			}
		}
	}

}
