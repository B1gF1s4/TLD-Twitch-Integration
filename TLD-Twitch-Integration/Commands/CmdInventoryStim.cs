using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TLD_Twitch_Integration.Commands
{
	public static class CmdInventoryStim
	{
		public static void AddCommandToConsole()
		{
			uConsole.RegisterCommand("tti_inventory_stim", new Action(() =>
			{
				Execute();
			}));
		}

		public static string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowSteppedStim)
				throw new RequiresRedeemRefundException(
					"Stepped on stim redeem is currently disabled.");

			if (GameService.IsMenuOpen())
				return "";

			var prefabName = "GEAR_EmergencyStim";

			var prefab =
				Addressables.LoadAssetAsync<GameObject>(prefabName).WaitForCompletion() ??
					throw new RequiresRedeemRefundException($"Error spawning {prefabName}");

			var stimInstance = UnityEngine.Object.Instantiate(prefab) ??
				throw new RequiresRedeemRefundException($"Error spawning {prefabName}");

			stimInstance.name = prefab.name;

			var stim = stimInstance.GetComponent<EmergencyStimItem>();
			GameManager.GetEmergencyStimComponent().ApplyEmergencyStim(stim);

			string alert;
			if (redeem == null)
				alert = $"stim applied";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
