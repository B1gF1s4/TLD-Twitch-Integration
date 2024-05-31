using Il2Cpp;
using Il2CppTLD.Stats;
using TLD_Twitch_Integration.Exceptions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TLD_Twitch_Integration.Game
{
	public static class AnimalService
	{
		private static List<GameObject> _spawnedAnimals = new();
		private static DateTime _lastCleanup;
		private const int _intervalCleanup = 10;

		public static bool Spawn(string prefabName, float distance)
		{
			var cam = GameManager.GetCurrentCamera();
			var errorMsg = $"Error spawning {prefabName}";
			var layerMask = Utils.m_PhysicalCollisionLayerMask | 67108864;
			var maxAcceptedHeightDifference = 5f;

			GameObject prefab =
				Addressables.LoadAssetAsync<GameObject>(prefabName).WaitForCompletion() ??
					throw new RequiresRedeemRefundException(errorMsg);

			var animalPos = cam.transform.position
				+ cam.transform.forward * distance;
			animalPos.y += 1000;

			if (!Physics.Raycast(
				animalPos,
				-Vector3.up,
				out RaycastHit raycastHit,
				float.PositiveInfinity,
				layerMask))
				return false;

			var directionToAnimal = (raycastHit.point - cam.transform.position).normalized;
			if (!Physics.Raycast(raycastHit.point, directionToAnimal,
				float.PositiveInfinity, layerMask))
				return false;

			if (Mathf.Abs(raycastHit.point.y - cam.transform.position.y) >
				maxAcceptedHeightDifference)
				return false;

			var rotation = Quaternion.LookRotation(
				-GameManager.GetMainCamera().transform.forward);

			GameObject animalObj = UnityEngine.Object.Instantiate(prefab,
				raycastHit.point, rotation) ??
					throw new RequiresRedeemRefundException(errorMsg);

			animalObj.name = prefabName;

			BaseAi animal = animalObj.GetComponent<BaseAi>() ??
				throw new RequiresRedeemRefundException(errorMsg);

			animal.m_SpawnPos = raycastHit.point;

			_spawnedAnimals.Add(animalObj);

			return true;
		}

		public static void ResetSpawnedAnimals()
		{
			_spawnedAnimals.Clear();
		}

		public static void CleanupSpawnedAnimals()
		{
			if (_spawnedAnimals.Count <= 0)
				return;

			var delta = (DateTime.UtcNow - _lastCleanup).TotalSeconds;
			if (delta < _intervalCleanup)
				return;

			_lastCleanup = DateTime.UtcNow;

			var playerPos = GameManager.GetCurrentCamera().transform.position;
			var aisInRange = AiUtils.GetAisWithinRange(playerPos,
				Settings.ModSettings.AnimalCleanupDistance);
			var animalObjsToRemove = new List<GameObject>();

			foreach (var animalObj in _spawnedAnimals)
			{
				BaseAi animal = animalObj.GetComponent<BaseAi>();

				if (animal == null)
				{
					animalObjsToRemove.Add(animalObj);
					continue;
				}

				if (!animal.isActiveAndEnabled)
				{
					animal.Despawn();
					animalObjsToRemove.Add(animalObj);
					continue;
				}

				if (!aisInRange.Contains(animal))
				{
					animal.Despawn();
					animalObjsToRemove.Add(animalObj);
					continue;
				}
			}

			foreach (var objToRemove in animalObjsToRemove)
			{
				_spawnedAnimals.Remove(objToRemove);
			}
		}

		public static int GetFuryScore()
		{
			var bunnies = (int)StatsManager.GetValue(StatID.RabbitsKilled);
			var deer = (int)StatsManager.GetValue(StatID.StagsKilled);
			var wolves = (int)StatsManager.GetValue(StatID.WolvesKilled);
			var bears = (int)StatsManager.GetValue(StatID.BearsKilled);
			var moose = (int)StatsManager.GetValue(StatID.MooseKilled);

			return bunnies + deer * 3 + wolves * 5 + bears * 25 + moose * 100;
		}
	}
}
