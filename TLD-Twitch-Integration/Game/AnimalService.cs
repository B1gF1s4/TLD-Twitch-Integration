using Il2Cpp;
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

		public static List<BaseAi> GetAliveBaseAiInRange()
		{
			var playerPos = GameManager.GetCurrentCamera().transform.position;
			var aisInRange = AiUtils.GetAisWithinRange(playerPos,
				Settings.ModSettings.AnimalCleanupDistance);

			var list = new List<BaseAi>();

			foreach (var ai in aisInRange)
			{
				if (ai.GetNormalizedCondition() > 0.01f)
					list.Add(ai);
			}

			return list;
		}

		public static void CleanupSpawnedAnimals()
		{
			if (_spawnedAnimals.Count <= 0)
				return;

			var delta = (DateTime.UtcNow - _lastCleanup).TotalSeconds;
			if (delta < _intervalCleanup)
				return;

			_lastCleanup = DateTime.UtcNow;

			var aisInRange = GetAliveBaseAiInRange();
			var animalObjsToRemove = new List<GameObject>();

			foreach (var animalObj in _spawnedAnimals)
			{
				BaseAi animal = animalObj.GetComponent<BaseAi>() ??
					throw new RequiresRedeemRefundException("error getting base ai component");

				if (!animal.isActiveAndEnabled)
				{
					animal.Despawn();
					animalObjsToRemove.Add(animalObj);
				}

				if (!aisInRange.Contains(animal))
				{
					animal.Despawn();
					animalObjsToRemove.Add(animalObj);
				}
			}

			foreach (var objToRemove in animalObjsToRemove)
			{
				_spawnedAnimals.Remove(objToRemove);
			}
		}
	}
}
