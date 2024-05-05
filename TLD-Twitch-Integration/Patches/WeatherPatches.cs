using HarmonyLib;
using Il2Cpp;

namespace TLD_Twitch_Integration.Patches
{
	[HarmonyPatch(typeof(WeatherTransition), nameof(WeatherTransition.Update))]
	internal class WeatherTransitionUpdatePatch
	{
		internal static void Postfix(WeatherTransition __instance)
		{
			if (GameService.WeatherToChange == WeatherStage.Undefined)
				return;

			__instance.ActivateWeatherSet(GameService.WeatherToChange);
			GameService.WeatherToChange = WeatherStage.Undefined;
		}
	}
}
