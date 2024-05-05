using HarmonyLib;
using Il2Cpp;
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
			var distance = 35.0f;

			switch (GameService.AnimalToSpawn)
			{
				case AnimalRedeemType.TWolves:
					var animalPosPack = cam.position + cam.forward * distance;
					bai.SetPosition(animalPosPack);
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
					bai.SetPosition(animalPos);
					GameService.SpawningAnimal = false;
					GameService.AnimalToSpawn = AnimalRedeemType.None;
					break;
				case AnimalRedeemType.StalkingWolf:
					var animalPosBehind = cam.position - cam.forward * (distance / 2);
					bai.SetPosition(animalPosBehind);
					GameService.SpawningAnimal = false;
					GameService.AnimalToSpawn = AnimalRedeemType.None;
					break;
				case AnimalRedeemType.BunnyExplosion:
					var animalPosExplosion = cam.position + cam.forward * (distance / 4);
					bai.SetPosition(animalPosExplosion);
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
