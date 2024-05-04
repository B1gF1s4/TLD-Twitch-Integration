using HarmonyLib;
using Il2Cpp;
using static TLD_Twitch_Integration.GameService;

namespace TLD_Twitch_Integration.Patches
{

	[HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
	internal class GameManager_Update
	{
		internal static void Postfix()
		{
			if (SpawningAnimal)
				return;

			if (IsInBuilding())
				return;

			switch (AnimalToSpawn)
			{
				case AnimalRedeemType.TWolves:
					SpawnTWolves();
					break;
				case AnimalRedeemType.Bear:
					SpawnBear();
					break;
				case AnimalRedeemType.Moose:
					SpawnMoose();
					break;
				case AnimalRedeemType.StalkingWolf:
					SpawnStalkingWolf();
					break;
				case AnimalRedeemType.BunnyExplosion:
					SpawnBunnyExplosion();
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
			var distance = 35.0f;

			switch (AnimalToSpawn)
			{
				case AnimalRedeemType.TWolves:
					var animalPosPack = cam.position + cam.forward * distance;
					bai.SetPosition(animalPosPack);
					SpawnedAnimalCounter++;

					if (SpawnedAnimalCounter >= TWolfPackSize)
					{
						SpawningAnimal = false;
						AnimalToSpawn = AnimalRedeemType.None;
					}
					break;
				case AnimalRedeemType.Bear:
				case AnimalRedeemType.Moose:
					var animalPos = cam.position + cam.forward * distance;
					bai.SetPosition(animalPos);
					SpawningAnimal = false;
					AnimalToSpawn = AnimalRedeemType.None;
					break;
				case AnimalRedeemType.StalkingWolf:
					var animalPosBehind = cam.position - cam.forward * (distance / 2);
					bai.SetPosition(animalPosBehind);
					SpawningAnimal = false;
					AnimalToSpawn = AnimalRedeemType.None;
					break;
				case AnimalRedeemType.BunnyExplosion:
					var animalPosExplosion = cam.position + cam.forward * (distance / 4);
					bai.SetPosition(animalPosExplosion);
					SpawnedAnimalCounter++;
					if (SpawnedAnimalCounter >= BunnyExplosionSize)
					{
						SpawningAnimal = false;
						AnimalToSpawn = AnimalRedeemType.None;
					}
					break;
				default:
					return;
			}


		}
	}

}
