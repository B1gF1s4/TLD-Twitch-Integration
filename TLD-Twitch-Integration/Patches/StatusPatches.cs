using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Patches
{
	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.Update))]
	internal class PlayerManagerUpdatePatch
	{
		internal static void Postfix()
		{
			if (GameService.StatusMeterToChange != StatusMeter.None)
			{
				GameService.ChangeMeter(GameService.StatusMeterToChange,
				GameService.IsHelpfulStatusMeterChange);

				GameService.StatusMeterToChange = StatusMeter.None;
			}
		}
	}

	[HarmonyPatch(typeof(Inventory), nameof(Inventory.GetExtraScentIntensity))]
	internal class InventoryGetExtraScentIntensityPatch
	{
		internal static void Postfix(ref float __result)
		{
			if (GameService.ShouldAddStink)
			{
				__result = GameService.StinkValue;

				if ((DateTime.UtcNow - GameService.StinkStart).TotalSeconds >=
					Settings.ModSettings.StinkTime)
				{
					Melon<Mod>.Logger.Msg("time is up, stink is gone");
					GameService.ShouldAddStink = false;
				}
			}
		}
	}

	[HarmonyPatch(typeof(CabinFever), nameof(CabinFever.Update))]
	internal class CabinFeverUpdatePatch
	{
		internal static void Postfix(CabinFever __instance)
		{
			if (!GameService.ShouldStartCabinFever)
				return;

			__instance.CabinFeverStart(true);

			GameService.ShouldStartCabinFever = false;
		}
	}

	[HarmonyPatch(typeof(Dysentery), nameof(Dysentery.Update))]
	internal class DysenteryUpdatePatch
	{
		internal static void Postfix(Dysentery __instance)
		{
			if (!GameService.ShouldStartDysentery)
				return;

			__instance.DysenteryStart(true);

			GameService.ShouldStartDysentery = false;
		}
	}

	[HarmonyPatch(typeof(FoodPoisoning), nameof(FoodPoisoning.Update))]
	internal class FoodPoisoningUpdatePatch
	{
		internal static void Postfix(FoodPoisoning __instance)
		{
			if (!GameService.ShouldStartFoodPoisoning)
				return;

			__instance.FoodPoisoningStart("GAMEPLAY_TaintedFood", true);

			GameService.ShouldStartFoodPoisoning = false;
		}
	}

	[HarmonyPatch(typeof(Hypothermia), nameof(Hypothermia.Update))]
	internal class HypothermiaUpdatePatch
	{
		internal static void Postfix(Hypothermia __instance)
		{
			if (!GameService.ShouldStartHypothermia)
				return;

			__instance.HypothermiaStart("GAMEPLAY_ColdWeather", true);

			GameService.ShouldStartHypothermia = false;
		}
	}

	[HarmonyPatch(typeof(IntestinalParasites), nameof(IntestinalParasites.Update))]
	internal class IntestinalParasitesUpdatePatch
	{
		internal static void Postfix(IntestinalParasites __instance)
		{
			if (!GameService.ShouldStartParasites)
				return;

			__instance.IntestinalParasitesStart();

			GameService.ShouldStartParasites = false;
		}
	}

	[HarmonyPatch(typeof(BloodLoss), nameof(BloodLoss.Update))]
	internal class BloodLossUpdatePatch
	{
		internal static Random random = new();

		internal static void Postfix(BloodLoss __instance)
		{
			if (!GameService.ShouldAddBleeding)
				return;

			Array values = Enum.GetValues(typeof(AfflictionBodyArea));
			AfflictionBodyArea bodyArea = (AfflictionBodyArea)
				(values.GetValue(random.Next(values.Length)) ??
					throw new Exception("trying to cast null to enum"));

			var countBefore = __instance.GetAfflictionsCount();

			__instance.BloodLossStartOverrideArea(bodyArea, "Shrapnel", true,
				AfflictionOptions.PlayFX | AfflictionOptions.DoAutoSave);

			var countAfter = __instance.GetAfflictionsCount();

			if (countBefore < countAfter)
				GameService.ShouldAddBleeding = false;
		}
	}

	[HarmonyPatch(typeof(SprainedWrist), nameof(SprainedWrist.Update))]
	internal class SprainedWristUpdatePatch
	{
		internal static void Postfix(SprainedWrist __instance)
		{
			if (!GameService.ShouldAddSprain)
				return;

			if (GameService.SprainIsAnkle)
				return;

			__instance.SprainedWristStart("GAMEPLAY_Fall",
				AfflictionOptions.PlayFX | AfflictionOptions.DoAutoSave |
				AfflictionOptions.DisplayIcon | AfflictionOptions.Stumble);

			GameService.ShouldAddSprain = false;
		}
	}

	[HarmonyPatch(typeof(SprainedAnkle), nameof(SprainedAnkle.Update))]
	internal class SprainedAnkleUpdatePatch
	{
		internal static void Postfix(SprainedAnkle __instance)
		{
			if (!GameService.ShouldAddSprain)
				return;

			if (!GameService.SprainIsAnkle)
				return;

			__instance.SprainedAnkleStart("GAMEPLAY_Fall",
				AfflictionOptions.PlayFX | AfflictionOptions.DoAutoSave |
				AfflictionOptions.DisplayIcon | AfflictionOptions.Stumble);

			GameService.ShouldAddSprain = false;
		}
	}

	[HarmonyPatch(typeof(Frostbite), nameof(Frostbite.Update))]
	internal class FrostbiteUpdatePatch
	{
		internal static Random random = new();

		internal static void Postfix(Frostbite __instance)
		{
			if (!GameService.ShouldAddFrostbite)
				return;

			Array values = Enum.GetValues(typeof(AfflictionBodyArea));
			AfflictionBodyArea bodyArea = (AfflictionBodyArea)
				(values.GetValue(random.Next(values.Length)) ??
					throw new Exception("trying to cast null to enum"));

			var countBefore = __instance.GetFrostbiteAfflictionCount();

			__instance.FrostbiteStart((int)bodyArea, true, false);

			var countAfter = __instance.GetFrostbiteAfflictionCount();

			if (countBefore < countAfter)
				GameService.ShouldAddFrostbite = false;
		}
	}
}
