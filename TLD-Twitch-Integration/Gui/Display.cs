using Il2Cpp;
using Il2CppTLD.Stats;
using TLD_Twitch_Integration.Game;
using UnityEngine;

namespace TLD_Twitch_Integration.Gui
{
	public static class Display
	{
		public enum DisplayMode
		{
			None,
			ChatVsStreamer,
			FuryThenSilence,
			WolfWrangler,
		}

		public static List<DisplayValue> Values { get; set; } = new();

		public static bool DisplayModeHasChanged { get; set; } = true;

		public static bool PositionHasChanged { get; set; } = true;

		public static bool IsInitialized { get; set; } = false;

		public static DisplayMode CurrentDisplayMode { get; set; }

		public static int CurrentDisplayValuesOffset { get; set; }

		private static DisplayValue? _separator;

		private static DisplayValue? _csDistance;
		private static DisplayValue? _csBunnies;
		private static DisplayValue? _csDeer;
		private static DisplayValue? _csWolves;
		private static DisplayValue? _csBears;
		private static DisplayValue? _csMoose;
		private static DisplayValue? _csScore;

		private static DisplayValue? _fBunnies;
		private static DisplayValue? _fDeer;
		private static DisplayValue? _fWolves;
		private static DisplayValue? _fBears;
		private static DisplayValue? _fMoose;
		private static DisplayValue? _fAccuracy;
		private static DisplayValue? _fScore;

		private static DisplayValue? _wwTime;
		private static DisplayValue? _wwWolves;

		public static void Init(Panel_HUD hud)
		{
			if (hud.m_VistaNotification == null)
				return;

			if (hud.m_VistaNotificationTitle == null)
				return;

			_separator = new DisplayValue("Separator");

			_csDistance = new DisplayValue(hud, "CS_Distance", "Distance", nameof(StatID.DistanceTravelled));
			_csBunnies = new DisplayValue(hud, "CS_Bunnies", "Bunnies", nameof(StatID.RabbitsKilled));
			_csDeer = new DisplayValue(hud, "CS_Deer", "Deer", nameof(StatID.StagsKilled));
			_csWolves = new DisplayValue(hud, "CS_Wolves", "Wolves", nameof(StatID.WolvesKilled));
			_csBears = new DisplayValue(hud, "CS_Bears", "Bears", nameof(StatID.BearsKilled));
			_csMoose = new DisplayValue(hud, "CS_Moose", "Moose", nameof(StatID.MooseKilled));
			_csScore = new DisplayValue(hud, "CS_Score", "Score", "FuryScore");

			_fBunnies = new DisplayValue(hud, "F_Bunnies", "Bunnies", nameof(StatID.RabbitsKilled));
			_fDeer = new DisplayValue(hud, "F_Deer", "Deer", nameof(StatID.StagsKilled));
			_fWolves = new DisplayValue(hud, "F_Wolves", "Wolves", nameof(StatID.WolvesKilled));
			_fBears = new DisplayValue(hud, "F_Bears", "Bears", nameof(StatID.BearsKilled));
			_fMoose = new DisplayValue(hud, "F_Moose", "Moose", nameof(StatID.MooseKilled));
			_fAccuracy = new DisplayValue(hud, "F_Acc", "Accuracy", "FuryAcc");
			_fScore = new DisplayValue(hud, "F_Score", "Score", "FuryScore");

			_wwTime = new DisplayValue(hud, "WW_Time", "Time", nameof(StatID.HoursSurvived));
			_wwWolves = new DisplayValue(hud, "WW_Wolves", "Wolves", nameof(StatID.WolvesKilled));

			IsInitialized = true;

			SetCurrentValues();
		}

		public static void CleanupVanillaText(Panel_HUD hud)
		{
			if (hud.m_VistaNotificationTitle != null)
				hud.m_VistaNotificationTitle.text = "";

			if (hud.m_VistaNotificationDescription != null)
				hud.m_VistaNotificationDescription.text = "";
		}

		public static void UpdatePrefix(Panel_HUD hud)
		{
			if (!IsInitialized)
				return;

			if (hud == null)
				return;

			if (!hud.enabled)
				return;

			CleanupVanillaText(hud);

			if (CurrentDisplayMode == DisplayMode.None)
			{
				hud.m_DisplayVistaNotification = false;
				hud.m_VistaNotification.alpha = 0f;
			}
			else
			{
				hud.m_DisplayVistaNotification = true;
				hud.m_VistaNotificationElapsedDisplayTimeSeconds = 0.1f;
			}
		}

		public static void UpdatePostfix(Panel_HUD hud)
		{
			if (!IsInitialized)
				return;

			if (hud == null)
				return;

			if (!hud.enabled)
				return;

			if (DisplayModeHasChanged)
				SetCurrentValues();

			if (CurrentDisplayMode == DisplayMode.None)
				return;

			if (hud.m_VistaNotification == null)
				return;

			if (hud.m_VistaNotificationTitle == null)
				return;

			hud.m_VistaNotification.alpha = 1f;

			UpdatePositionAndScale(hud);

			for (int i = 0; i < Values.Count; i++)
			{
				var displayValue = Values[i];

				if (PositionHasChanged && !displayValue.IsSeparator)
					displayValue.UpdatePosition(hud, i);

				displayValue.Update(hud);
			}
		}

		public static string GetCurrentValue(string valueID)
		{
			switch (valueID)
			{
				case nameof(StatID.DistanceTravelled):
					var distance = StatsManager.GetValue(StatID.DistanceTravelled) / 1000f;
					return distance.ToString("0.000") + " km";
				case nameof(StatID.RabbitsKilled):
					return StatsManager.GetValue(StatID.RabbitsKilled).ToString();
				case nameof(StatID.StagsKilled):
					return StatsManager.GetValue(StatID.StagsKilled).ToString();
				case nameof(StatID.WolvesKilled):
					return StatsManager.GetValue(StatID.WolvesKilled).ToString();
				case nameof(StatID.BearsKilled):
					return StatsManager.GetValue(StatID.BearsKilled).ToString();
				case nameof(StatID.MooseKilled):
					return StatsManager.GetValue(StatID.MooseKilled).ToString();
				case "FuryScore":
					return AnimalService.GetFuryScore(false).ToString();
				case "FuryAcc":
					return $"{AnimalService.GetAccuracyValue() * 100} %";
				case nameof(StatID.HoursSurvived):
					var hours = StatsManager.GetValue(StatID.HoursSurvived);
					var timespan = TimeSpan.FromHours(hours);
					return $"{timespan:%d}d {timespan:%h}h";
				default:
					return "ValueID not supported";
			}
		}

		private static void UpdatePositionAndScale(Panel_HUD hud)
		{
			var widget = hud!.m_VistaNotification;

			widget.transform.localScale = new Vector3(0.5f, 0.51f * Values.Count, 1f);
			widget.transform.localPosition = new Vector3(widget.transform.localPosition.x, 10, 1);
		}

		private static void SetCurrentValues()
		{
			CurrentDisplayMode = Settings.ModSettings.DisplayMode;
			DisplayModeHasChanged = false;

			foreach (var value in Values)
			{
				value.Disable();
			}

			Values.Clear();

			switch (CurrentDisplayMode)
			{
				case DisplayMode.ChatVsStreamer:
					Values.Add(_csDistance!);
					Values.Add(_separator!);
					Values.Add(_csBunnies!);
					Values.Add(_csDeer!);
					Values.Add(_csWolves!);
					Values.Add(_csBears!);
					Values.Add(_csMoose!);
					Values.Add(_csScore!);
					CurrentDisplayValuesOffset = 140;
					return;

				case DisplayMode.FuryThenSilence:
					Values.Add(_fBunnies!);
					Values.Add(_fDeer!);
					Values.Add(_fWolves!);
					Values.Add(_fBears!);
					Values.Add(_fMoose!);
					Values.Add(_separator!);
					Values.Add(_fAccuracy!);
					Values.Add(_fScore!);
					CurrentDisplayValuesOffset = 140;
					return;

				case DisplayMode.WolfWrangler:
					Values.Add(_wwTime!);
					Values.Add(_wwWolves!);
					CurrentDisplayValuesOffset = 20;
					return;

				case DisplayMode.None:
				default:
					return;
			}
		}
	}
}
