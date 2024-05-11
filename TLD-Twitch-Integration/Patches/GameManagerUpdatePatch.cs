using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Patches
{
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
	internal class GameManagerUpdatePatch
	{
		internal static void Prefix()
		{
			if (ExecutionPending)
				GameService.Update();
		}
	}

	[HarmonyPatch(typeof(GameManager), nameof(GameManager.Start))]
	internal class GameManagerStartPatch
	{
		internal static void Postfix()
		{
			GameService.LoadingAssets = true;

			if (GameService.PrefabStim == null &&
				!GameService.LoadingAssets)
			{
				GameService.PrefabStim =
					Addressables.LoadAssetAsync<GameObject>("GEAR_EmergencyStim")
						.WaitForCompletion();
			}

			GameService.LoadingAssets = false;
		}
	}
}
