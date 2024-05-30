using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Stats;
using UnityEngine;

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

				if (!__instance.enabled)
					return;

				if (!Settings.ModSettings.ShowKillCounter)
					return;

				if (__instance.m_VistaNotificationTitle != null)
					__instance.m_VistaNotificationTitle.text = "";

				if (__instance.m_VistaNotificationDescription != null)
					__instance.m_VistaNotificationDescription.text = "";
			}
		}

		[HarmonyPatch(typeof(Panel_HUD), "UpdateVistaNotification")]
		private static class AlwaysShowVistaNotificationHud
		{
			private static UILabel? _titleBunny;
			private static UILabel? _counterBunny;

			private static UILabel? _titleDeer;
			private static UILabel? _counterDeer;

			private static UILabel? _titleWolf;
			private static UILabel? _counterWolf;

			private static UILabel? _titleBear;
			private static UILabel? _counterBear;

			private static UILabel? _titleMoose;
			private static UILabel? _counterMoose;

			private static void Prefix(ref Panel_HUD __instance)
			{
				if (__instance == null)
					return;

				if (!__instance.enabled)
					return;

				if (!Settings.ModSettings.ShowKillCounter)
					return;

				if (__instance.m_VistaNotificationTitle != null)
					__instance.m_VistaNotificationTitle.text = "";

				if (__instance.m_VistaNotificationDescription != null)
					__instance.m_VistaNotificationDescription.text = "";

				__instance.m_DisplayVistaNotification = true;
				__instance.m_VistaNotificationElapsedDisplayTimeSeconds = 0.1f;
			}

			private static void Postfix(ref Panel_HUD __instance)
			{
				if (__instance == null)
					return;

				if (!__instance.enabled)
					return;

				if (!Settings.ModSettings.ShowKillCounter)
					return;

				if (__instance.m_VistaNotification == null)
					return;

				if (__instance.m_VistaNotificationTitle == null)
					return;

				__instance.m_VistaNotification.alpha = 1f;
				__instance.m_VistaNotification.transform.localScale = new Vector3(0.5f, 3.3f, 1f);
				__instance.m_VistaNotification.transform.localPosition = new Vector3(
					__instance.m_VistaNotification.transform.localPosition.x, 150, 1);

				if (_titleBunny == null)
					_titleBunny = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Bunny", 0);

				if (_counterBunny == null)
					_counterBunny = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Bunny", 0);

				if (_titleDeer == null)
					_titleDeer = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Deer", 1);

				if (_counterDeer == null)
					_counterDeer = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Deer", 1);

				if (_titleWolf == null)
					_titleWolf = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Wolf", 2);

				if (_counterWolf == null)
					_counterWolf = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Wolf", 2);

				if (_titleBear == null)
					_titleBear = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Bear", 3);

				if (_counterBear == null)
					_counterBear = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Bear", 3);

				if (_titleMoose == null)
					_titleMoose = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Moose", 4);

				if (_counterMoose == null)
					_counterMoose = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Moose", 4);

				_titleBunny.text = "Bunnies";
				_counterBunny.text =
					StatsManager.GetValue(StatID.RabbitsKilled).ToString();

				_titleDeer.text = "Deer";
				_counterDeer.text =
					StatsManager.GetValue(StatID.StagsKilled).ToString();

				_titleWolf.text = "Wolves";
				_counterWolf.text =
					StatsManager.GetValue(StatID.WolvesKilled).ToString();

				_titleBear.text = "Bears";
				_counterBear.text =
					StatsManager.GetValue(StatID.BearsKilled).ToString();

				_titleMoose.text = "Moose";
				_counterMoose.text =
					StatsManager.GetValue(StatID.MooseKilled).ToString();
			}

			private static UILabel CreateTitleLabel(Panel_HUD instance, string name, int index)
			{
				var parentTransform = instance.m_VistaNotification.transform;

				var label = UnityEngine.Object.Instantiate(instance.m_VistaNotificationTitle);
				label.transform.SetParent(parentTransform.parent, false);
				label.transform.localPosition = new Vector3(parentTransform.localPosition.x + 30,
					parentTransform.localPosition.y - index * 45 + 110, parentTransform.localPosition.z);
				label.name = name;
				label.width = 60;

				return label;
			}

			private static UILabel CreateCounterLabel(Panel_HUD instance, string name, int index)
			{
				var parentTransform = instance.m_VistaNotification.transform;

				var label = UnityEngine.Object.Instantiate(instance.m_VistaNotificationDescription);
				label.transform.SetParent(parentTransform.parent, false);
				label.transform.localPosition = new Vector3(parentTransform.localPosition.x + 30,
					parentTransform.localPosition.y - index * 45 + 100, parentTransform.localPosition.z);
				label.name = name;
				label.width = 60;
				label.alignment = NGUIText.Alignment.Left;

				return label;
			}
		}

	}

}
