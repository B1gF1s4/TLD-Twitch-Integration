﻿using HarmonyLib;
using Il2Cpp;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Patches
{

	[HarmonyPatch(typeof(BaseAiManager), nameof(BaseAiManager.Update))]
	internal class BaseAiManagerUpdatePatch
	{
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

	[HarmonyPatch(typeof(BaseAiManager), nameof(BaseAiManager.Add), new Type[] { typeof(BaseAi) })]
	internal class BaseAiManager_Add
	{
		internal static void Postfix(BaseAi bai)
		{
			if (bai == null)
				return;

			var cam = GameManager.GetCurrentCamera().transform;

			switch (GameService.AnimalToSpawn)
			{
				case AnimalRedeemType.TWolves:
					var animalPosPack = cam.position + cam.forward * Settings.ModSettings.DistanceTWolf;
					animalPosPack.y += 100;
					AiUtils.FindNearestGroundAndNavmeshFor(animalPosPack, out var groundPosPack, out var navmeshPosPack);
					bai.SetPosition(groundPosPack);
					GameService.SpawnedAnimalCounter++;
					if (GameService.SpawnedAnimalCounter >= GameService.SpawningAnimalTargetCount)
					{
						GameService.SpawningAnimal = false;
						GameService.AnimalToSpawn = AnimalRedeemType.None;
						GameService.SpawningAnimalTargetCount = 0;
					}
					break;

				case AnimalRedeemType.Bear:
				case AnimalRedeemType.Moose:
					var animalPos = cam.position + cam.forward * Settings.ModSettings.DistanceBear;
					animalPos.y += 100;
					AiUtils.FindNearestGroundAndNavmeshFor(animalPos, out var groundPos, out var navmeshPos);
					bai.SetPosition(groundPos);
					GameService.SpawningAnimal = false;
					GameService.AnimalToSpawn = AnimalRedeemType.None;
					break;

				case AnimalRedeemType.StalkingWolf:
					var animalPosBehind = cam.position - cam.forward * Settings.ModSettings.DistanceStalkingWolf;
					animalPosBehind.y += 100;
					AiUtils.FindNearestGroundAndNavmeshFor(animalPosBehind, out var groundPosBehind, out var navmeshPosBehind);
					bai.SetPosition(groundPosBehind);
					GameService.SpawningAnimal = false;
					GameService.AnimalToSpawn = AnimalRedeemType.None;
					break;

				case AnimalRedeemType.BunnyExplosion:
					var animalPosExplosion = cam.position + cam.forward * 4;
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
