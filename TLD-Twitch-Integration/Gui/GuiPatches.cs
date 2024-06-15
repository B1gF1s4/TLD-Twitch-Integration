using HarmonyLib;
using Il2Cpp;

namespace TLD_Twitch_Integration.Gui
{

	internal static class GuiPatches
	{
		[HarmonyPatch(typeof(Panel_HUD), "Enable")]
		private static class FixFlicker
		{
			private static void Prefix(ref Panel_HUD __instance)
			{
				if (__instance == null)
					return;

				if (!__instance.isActiveAndEnabled)
					return;

				Display.CleanupVanillaText(__instance);
			}
		}

		[HarmonyPatch(typeof(Panel_HUD), "UpdateVistaNotification")]
		private static class AlwaysShowVistaNotificationHud
		{

			private static void Prefix(ref Panel_HUD __instance)
			{
				Display.UpdatePrefix(__instance);
			}

			private static void Postfix(ref Panel_HUD __instance)
			{
				if (!Display.IsInitialized)
				{
					if (__instance == null)
						return;

					if (!__instance.enabled)
						return;

					Display.Init(__instance);
				}

				Display.UpdatePostfix(__instance);
			}
		}

	}

}
