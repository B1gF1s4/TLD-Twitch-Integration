using HarmonyLib;
using Il2Cpp;

namespace TLD_Twitch_Integration.Patches
{
	[HarmonyPatch(typeof(CabinFever), nameof(CabinFever.Update))]
	internal class CabinFeverUpdatePatch
	{
		internal static void Postfix()
		{
			if (!GameService.ShouldStartCabinFever)
				return;

			GameService.ShouldStartCabinFever = false;
			GameManager.GetCabinFeverComponent().CabinFeverStart(true);
		}
	}

	[HarmonyPatch(typeof(Dysentery), nameof(Dysentery.Update))]
	internal class DysenteryUpdatePatch
	{
		internal static void Postfix()
		{
			if (!GameService.ShouldStartDysentery)
				return;

			GameService.ShouldStartDysentery = false;
			GameManager.GetDysenteryComponent().DysenteryStart(true);
		}
	}

	[HarmonyPatch(typeof(FoodPoisoning), nameof(FoodPoisoning.Update))]
	internal class FoodPoisoningUpdatePatch
	{
		internal static void Postfix()
		{
			if (!GameService.ShouldStartFoodPoisoning)
				return;

			GameService.ShouldStartFoodPoisoning = false;
			GameManager.GetFoodPoisoningComponent().FoodPoisoningStart("GAMEPLAY_TaintedFood", true);
		}
	}

	[HarmonyPatch(typeof(Hypothermia), nameof(Hypothermia.Update))]
	internal class HypothermiaUpdatePatch
	{
		internal static void Postfix()
		{
			if (!GameService.ShouldStartHypothermia)
				return;

			GameService.ShouldStartHypothermia = false;
			GameManager.GetHypothermiaComponent().HypothermiaStart("GAMEPLAY_ColdWeather", true);
		}
	}

	[HarmonyPatch(typeof(IntestinalParasites), nameof(IntestinalParasites.Update))]
	internal class IntestinalParasitesUpdatePatch
	{
		internal static void Postfix()
		{
			if (!GameService.ShouldStartParasites)
				return;

			GameService.ShouldStartParasites = false;
			GameManager.GetIntestinalParasitesComponent().IntestinalParasitesStart();
		}
	}
}
