using Il2Cpp;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Game
{
	public static class GameService
	{
		public static bool IsInBuilding { get; set; }
		public static bool IsAuroraActive { get; set; }
		public static bool IsAuroraFading { get; set; }
		public static bool IsHoldingTorchLike { get; set; }
		public static bool HasTorchLikeInInventory { get; set; }

		public static WeatherStage WeatherToChange { get; set; }
			= WeatherStage.Undefined;
		public static bool ShouldStartAurora { get; set; }

		public static bool ShouldStartCabinFever { get; set; }
		public static bool ShouldStartDysentery { get; set; }
		public static bool ShouldStartFoodPoisoning { get; set; }
		public static bool ShouldStartHypothermia { get; set; }
		public static bool ShouldStartParasites { get; set; }

		public static StatusMeter StatusMeterToChange { get; set; }
		public static bool IsHelpfulStatusMeterChange { get; set; }

		public static bool ShouldAddBleeding { get; set; }
		public static bool ShouldAddSprain { get; set; }
		public static bool SprainIsAnkle { get; set; }
		public static bool ShouldAddFrostbite { get; set; }

		public static bool ShouldAddStink { get; set; }
		public static DateTime StinkStart { get; set; }
		public static float StinkValue { get; set; }

		public static bool IsFartActive { get; set; }
		public static DateTime FartStart { get; set; }

		//public static ClipManager? ClipManager { get; set; }
		//public static Shot? VoiceSource { get; set; }

		public static void Update()
		{
			IsInBuilding = GetIsInBuilding();

			if (GameManager.GetAuroraManager().IsFullyActive())
				IsAuroraActive = true;
			else
				IsAuroraActive = false;

			IsAuroraFading = GetIsAuroraFading();
			IsHoldingTorchLike = GetIsHoldingTorchLike();
			HasTorchLikeInInventory = GetHasTorchLikeInInventory();
		}

		public static void InitAudio()
		{
			//ClipManager = AudioMaster.NewClipManager();

			//var path = Path.Combine(Mod.BaseDirectory, "audio");

			//ClipManager.LoadClipsFromDir(path, ClipManager.LoadType.Stream);

			//VoiceSource = AudioMaster.CreatePlayerShot(AudioMaster.SourceType.Voice);
			//VoiceSource.SetVolume(0.6f);
		}

		public static void ChangeMeter(StatusMeter type, bool isHelp)
		{
			var value = isHelp ?
				Settings.ModSettings.StatusHelpValue :
				Settings.ModSettings.StatusHarmValue;

			switch (type)
			{
				case StatusMeter.Fatigue:
					var fatigue = GameManager.GetFatigueComponent();
					if (fatigue != null)
						fatigue.m_CurrentFatigue = 100f - value;
					break;

				case StatusMeter.Hunger:
					var hunger = GameManager.GetHungerComponent();
					if (hunger != null)
						hunger.m_CurrentReserveCalories = value / 100f * hunger.m_MaxReserveCalories;
					break;

				case StatusMeter.Thirst:
					var thirst = GameManager.GetThirstComponent();
					if (thirst != null)
						thirst.m_CurrentThirst = 100f - value;
					break;

				case StatusMeter.Cold:
					var cold = GameManager.GetFreezingComponent();
					if (cold != null)
						cold.m_CurrentFreezing = 100f - value;
					break;

				default:
					break;
			}
		}

		public static void PlayPlayerSound(string sound)
		{
			var ignoreDelay = (Il2CppVoice.Priority)PlayerVoice.Options.IgnoreNonCriticalDelay;

			if (string.IsNullOrEmpty(sound))
				return;

			GameManager.GetPlayerVoiceComponent()?.Play(sound, ignoreDelay);
		}

		public static bool IsMenuOpen()
		{
			return InterfaceManager.IsOverlayActiveCached();
		}

		public static void StopFart()
		{
			GameManager.GetChemicalPoisoningComponent().m_InHazardZone = false;
			GameManager.GetChemicalPoisoningComponent().m_ActiveZones = 0;
			GameManager.GetChemicalPoisoningComponent().m_ToxicityGainedPerHour = 300;
		}

		private static bool GetIsInBuilding()
		{
			if (GameManager.GetWeatherComponent().IsIndoorScene())
				return true;

			if (GameManager.GetWeatherComponent().IsIndoorEnvironment())
				return true;

			return false;
		}

		private static bool GetIsAuroraFading()
		{
			var alpha = GameManager.GetAuroraManager().GetNormalizedAlpha();

			if (alpha <= 0)
				return false;

			if (alpha >= 1)
				return false;

			return true;
		}

		private static bool GetIsHoldingTorchLike()
		{
			var playerComp = GameManager.GetPlayerManagerComponent();
			if (playerComp == null)
				return false;

			if (playerComp.m_ItemInHands == null)
				return false;

			if (playerComp.m_ItemInHands.m_FlareItem != null ||
				playerComp.m_ItemInHands.m_TorchItem != null ||
				playerComp.m_ItemInHands.m_FlashlightItem != null)
				return true;

			return false;
		}

		private static bool GetHasTorchLikeInInventory()
		{
			var invComp = GameManager.GetInventoryComponent();
			if (invComp == null)
				return false;

			var num = invComp.GetNumFlares(FlareType.Red) +
				invComp.GetNumFlares(FlareType.Blue) +
				invComp.GetNumTorches();

			if (num <= 0)
				return false;

			return true;
		}

	}
}
