using HarmonyLib;
using Il2Cpp;
using TLD_Twitch_Integration.Game;

namespace TLD_Twitch_Integration.Patches
{
    [HarmonyPatch(typeof(WeatherTransition), nameof(WeatherTransition.Update))]
	internal class WeatherTransitionUpdatePatch
	{
		internal static void Prefix()
		{
			if (!GameService.ShouldStartAurora)
				return;

			ConsoleManager.CONSOLE_force_aurora();
			GameService.ShouldStartAurora = false;
		}

		internal static void Postfix(WeatherTransition __instance)
		{
			if (GameService.WeatherToChange == WeatherStage.Undefined)
				return;

			__instance.ActivateWeatherSet(GameService.WeatherToChange);
			GameService.WeatherToChange = WeatherStage.Undefined;
		}
	}
}
