using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace TLD_Twitch_Integration.Patches
{
	[HarmonyPatch(typeof(Inventory), nameof(Inventory.Update))]
	public class InventoryPatches
	{
		internal static void Postfix()
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

					entry.Drop(1, true, false, true);
				}

				GameService.ShouldDropPants = false;
			}

			if (GameService.ShouldDropTorch)
			{
				if (GameState.IsHoldingTorchLike)
				{
					var torch = GameManager.GetPlayerManagerComponent().m_ItemInHands;

					torch?.Drop(1, true, false, true);
					Melon<Mod>.Logger.Msg($"dropping {torch?.name}");
				}
				else
				{
					var invComp = GameManager.GetInventoryComponent();
					GearItem? itemToDrop = null;

					if (invComp?.GetNumTorches() > 0)
						itemToDrop = invComp?.GetBestGearItemWithName("GEAR_Torch");

					else if (invComp?.GetNumFlares(FlareType.Red) > 0)
						itemToDrop = invComp?.GetBestGearItemWithName("GEAR_FlareA");

					else if (invComp?.GetNumFlares(FlareType.Blue) > 0)
						itemToDrop = invComp?.GetBestGearItemWithName("GEAR_BlueFlare");

					itemToDrop?.Drop(1, true, false, true);
				}

				GameService.ShouldDropTorch = false;
			}
		}
	}
}
