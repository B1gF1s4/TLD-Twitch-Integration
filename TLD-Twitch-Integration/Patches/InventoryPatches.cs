﻿using HarmonyLib;
using Il2Cpp;

namespace TLD_Twitch_Integration.Patches
{
	[HarmonyPatch(typeof(Inventory), nameof(Inventory.Update))]
	public class InventoryUpdatePatches
	{
		internal static Random random = new Random();

		internal static void Postfix(Inventory __instance)
		{
			if (GameService.ShouldDropPants)
			{
				var pantsInner = GameManager.GetPlayerManagerComponent().GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Top);
				var pantsOuter = GameManager.GetPlayerManagerComponent().GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Top2);
				var undiesInner = GameManager.GetPlayerManagerComponent().GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Mid);
				var undiesOuter = GameManager.GetPlayerManagerComponent().GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Base);

				var pants = new GearItem?[] { pantsInner, pantsOuter, undiesInner, undiesOuter };
				foreach (var entry in pants)
				{
					if (entry == null)
						continue;

					var droppedItem = entry.Drop(1, true, false, true);
				}

				GameService.ShouldDropPants = false;
			}

			if (GameService.ShouldDropTorch)
			{
				if (GameState.IsHoldingTorchLike)
				{
					var torch = GameManager.GetPlayerManagerComponent().m_ItemInHands;
					var droppedItem = torch?.Drop(1, true, false, true);
				}
				else
				{
					GearItem? itemToDrop = null;

					if (__instance.GetNumTorches() > 0)
						itemToDrop = __instance.GetBestGearItemWithName("GEAR_Torch");

					else if (__instance.GetNumFlares(FlareType.Red) > 0)
						itemToDrop = __instance.GetBestGearItemWithName("GEAR_FlareA");

					else if (__instance.GetNumFlares(FlareType.Blue) > 0)
						itemToDrop = __instance.GetBestGearItemWithName("GEAR_BlueFlare");

					var droppedItem = itemToDrop?.Drop(1, true, false, true);
				}

				GameService.ShouldDropTorch = false;
			}

			if (GameService.ShouldAddBow)
			{
				ConsoleManager.CONSOLE_bow();
				__instance.RemoveGearFromInventory("GEAR_Arrow", 100 - Settings.ModSettings.ArrowCount);

				GameService.ShouldAddBow = false;
			}

			if (GameService.ShouldStepOnStim)
			{
				if (GameService.PrefabStim != null)
				{
					var stimInstance = UnityEngine.Object.Instantiate(GameService.PrefabStim);
					stimInstance.name = GameService.PrefabStim.name;

					var stim = stimInstance.GetComponent<EmergencyStimItem>();
					GameManager.GetEmergencyStimComponent().ApplyEmergencyStim(stim);

					GameService.ShouldStepOnStim = false;
				}
			}

			if (GameService.ShouldDropRandomItem)
			{
				var filter = (GearItem gi) => { return !string.IsNullOrEmpty(gi.name); };
				var list = new Il2CppSystem.Collections.Generic.List<GearItem>();

				__instance.GetGearItems(filter, list);

				var gearItem = list[random.Next(0, list.Count - 1)];
				var droppedItem = gearItem.Drop(1, true, false, true);
				droppedItem.enabled = false;

				GameService.ShouldDropRandomItem = false;
			}
		}
	}
}
