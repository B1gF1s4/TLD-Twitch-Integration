using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Stats;
using TLD_Twitch_Integration.Game;
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

				if (!Settings.ModSettings.ShowStats)
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
			private static UILabel? _titleDistance;
			private static UILabel? _counterDistance;

			private static UILabel? _titleKillScore;
			private static UILabel? _counterKillScore;

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

				if (!Settings.ModSettings.ShowStats)
				{
					__instance.m_DisplayVistaNotification = false;
					__instance.m_VistaNotification.alpha = 0f;
					return;
				}

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

				if (!Settings.ModSettings.ShowStats)
					return;

				if (__instance.m_VistaNotification == null)
					return;

				if (__instance.m_VistaNotificationTitle == null)
					return;

				var parentWidget = __instance.m_VistaNotification;

				parentWidget.alpha = 1f;
				parentWidget.transform.localScale = new Vector3(0.6f * 0.8f, 5.1f * 0.8f, 1f);
				parentWidget.transform.localPosition = new Vector3(
					parentWidget.transform.localPosition.x, 10, 1);

				if (_titleDistance == null)
					_titleDistance = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Distance", -1);

				if (_counterDistance == null)
					_counterDistance = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Distance", -1);

				if (_titleBunny == null)
					_titleBunny = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Bunny", 1);

				if (_counterBunny == null)
					_counterBunny = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Bunny", 1);

				if (_titleDeer == null)
					_titleDeer = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Deer", 2);

				if (_counterDeer == null)
					_counterDeer = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Deer", 2);

				if (_titleWolf == null)
					_titleWolf = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Wolf", 3);

				if (_counterWolf == null)
					_counterWolf = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Wolf", 3);

				if (_titleBear == null)
					_titleBear = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Bear", 4);

				if (_counterBear == null)
					_counterBear = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Bear", 4);

				if (_titleMoose == null)
					_titleMoose = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_Moose", 5);

				if (_counterMoose == null)
					_counterMoose = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_Moose", 5);

				if (_titleKillScore == null)
					_titleKillScore = CreateTitleLabel(__instance,
						"Label_Killcounter_Title_KillScore", 6);

				if (_counterKillScore == null)
					_counterKillScore = CreateCounterLabel(__instance,
						"Label_Killcounter_Counter_KillScore", 6);

				_titleDistance.text = "Distance";
				_titleDistance.enabled = parentWidget.isVisible;
				var distance = StatsManager.GetValue(StatID.DistanceTravelled) / 1000f;
				_counterDistance.text = distance.ToString("0.000") + " km";
				_counterDistance.enabled = parentWidget.isVisible;

				_titleBunny.text = "Bunnies (1 pt)";
				_titleBunny.enabled = parentWidget.isVisible;
				_counterBunny.text =
					StatsManager.GetValue(StatID.RabbitsKilled).ToString();
				_counterBunny.enabled = parentWidget.isVisible;

				_titleDeer.text = "Deer (3 pts)";
				_titleDeer.enabled = parentWidget.isVisible;
				_counterDeer.text =
					StatsManager.GetValue(StatID.StagsKilled).ToString();
				_counterDeer.enabled = parentWidget.isVisible;

				_titleWolf.text = "Wolves (5 pts)";
				_titleWolf.enabled = parentWidget.isVisible;
				_counterWolf.text =
					StatsManager.GetValue(StatID.WolvesKilled).ToString();
				_counterWolf.enabled = parentWidget.isVisible;

				_titleBear.text = "Bears (25 pts)";
				_titleBear.enabled = parentWidget.isVisible;
				_counterBear.text =
					StatsManager.GetValue(StatID.BearsKilled).ToString();
				_counterBear.enabled = parentWidget.isVisible;

				_titleMoose.text = "Moose (100 pts)";
				_titleMoose.enabled = parentWidget.isVisible;
				_counterMoose.text =
					StatsManager.GetValue(StatID.MooseKilled).ToString();
				_counterMoose.enabled = parentWidget.isVisible;

				_titleKillScore.text = $"Kill Score";
				_titleKillScore.enabled = parentWidget.isVisible;
				_counterKillScore.text = AnimalService.GetFuryScore().ToString();
				_counterKillScore.enabled = parentWidget.isVisible;
			}

			private static UILabel CreateTitleLabel(Panel_HUD instance, string name, int index)
			{
				var parentTransform = instance.m_VistaNotification.transform;

				var label = UnityEngine.Object.Instantiate(instance.m_VistaNotificationTitle);
				label.transform.SetParent(parentTransform.parent, false);
				label.transform.localPosition = new Vector3(parentTransform.localPosition.x + 30,
					parentTransform.localPosition.y - index * 36 + 112, parentTransform.localPosition.z);
				label.transform.localScale = new Vector3(0.8f, 0.8f, 1);
				label.name = name;
				label.width = 60;
				label.alignment = NGUIText.Alignment.Center;

				return label;
			}

			private static UILabel CreateCounterLabel(Panel_HUD instance, string name, int index)
			{
				var parentTransform = instance.m_VistaNotification.transform;
				var x = _titleDistance?.transform.localPosition.x ??
					parentTransform.localPosition.x;

				var label = UnityEngine.Object.Instantiate(instance.m_VistaNotificationDescription);
				label.transform.SetParent(parentTransform.parent, false);
				label.transform.localPosition = new Vector3(x,
					parentTransform.localPosition.y - index * 36 + 104,
					parentTransform.localPosition.z);
				label.transform.localScale = new Vector3(0.8f, 0.8f, 1);
				label.name = name;
				label.width = 60;
				label.alignment = NGUIText.Alignment.Center;

				return label;
			}
		}

	}

}
