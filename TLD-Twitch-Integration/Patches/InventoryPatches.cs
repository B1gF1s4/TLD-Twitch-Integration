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
