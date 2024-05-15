using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TLD_Twitch_Integration.Game
{
	public static class AnimalService
	{
		public static bool Spawn(string prefabName, float distance)
		{
			var cam = GameManager.GetCurrentCamera();
			var errorMsg = $"Error spawning {prefabName}";
			var layerMask = Utils.m_PhysicalCollisionLayerMask | 67108864;
			var maxAcceptedHeightDifference = 8f;

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
			return true;
		}
	}
}
