using HarmonyLib;
using Il2Cpp;
using TLD_Twitch_Integration.Game;

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
				GameService.PlayPlayerSound("PLAY_CLOTHESTEARING");

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
					droppedItem.enabled = false;
				}

				GameService.ShouldDropPants = false;
			}

			if (GameService.ShouldDropTorch)
			{
				GameService.PlayPlayerSound("PLAY_FEARAFFLICTION");

				if (GameService.IsHoldingTorchLike)
				{
					var torch = GameManager.GetPlayerManagerComponent().m_ItemInHands;
					var droppedItem = torch?.Drop(1, true, false, true);

					if (droppedItem != null)
						droppedItem.enabled = false;
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

					if (droppedItem != null)
						droppedItem.enabled = false;
				}

				GameService.ShouldDropTorch = false;
			}

			if (GameService.ShouldAddBow)
			{
				ConsoleManager.CONSOLE_bow();
				__instance.RemoveGearFromInventory("GEAR_Arrow", 100 - Settings.ModSettings.ArrowCount);

				GameService.PlayPlayerSound("PLAY_FEATUNLOCKED");

				GameService.ShouldAddBow = false;
			}
		}
	}
}
