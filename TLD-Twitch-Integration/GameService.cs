using Il2Cpp;
using UnityEngine;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration
{
	public static class GameService
	{
		public static bool IsInBuilding { get; set; }
		public static bool IsAuroraActive { get; set; }
		public static bool IsAuroraFading { get; set; }
		public static bool IsHoldingTorchLike { get; set; }
		public static bool HasTorchLikeInInventory { get; set; }
		public static Il2CppSystem.Collections.Generic.List<GearItem> GearItems
		{ get; set; } = new();

		public static bool PanelFirstAidEnabled { get; set; }
		public static bool PanelClothingEnabled { get; set; }
		public static bool PanelInventoryEnabled { get; set; }
		public static bool PanelCraftingEnabled { get; set; }
		public static bool PanelCookingEnabled { get; set; }
		public static bool PanelLogEnabled { get; set; }
		public static bool PanelMapEnabled { get; set; }

		public static AnimalRedeemType AnimalToSpawn { get; set; }
		public static bool SpawningAnimal { get; set; }
		public static int SpawnedAnimalCounter { get; set; }
		public static int SpawningAnimalTargetCount { get; set; }

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

		public static bool ShouldDropPants { get; set; }
		public static bool ShouldDropTorch { get; set; }
		public static bool ShouldAddBow { get; set; }
		public static GearItem? RandomItemToDrop { get; set; }
		public static string? LastItemDropped { get; set; }

		public static bool ShouldStepOnStim { get; set; }
		public static GameObject? PrefabStim { get; set; }
		public static bool LoadingAssets { get; set; }

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
			GearItems = GetItemsInInventory();
		}

		public static void SpawnTWolves()
		{
			PlayPlayerSound("PLAY_ANXIETYAFFLICTION");

			SpawnedAnimalCounter = 0;
			SpawningAnimal = true;
			for (int i = 0; i < SpawningAnimalTargetCount; i++)
			{
				if (IsAuroraActive)
					ConsoleManager.CONSOLE_spawn_grey_wolf_aurora();
				else
					ConsoleManager.CONSOLE_spawn_wolf_grey();
			}
		}

		public static void SpawnBear()
		{
			SpawningAnimal = true;
			if (IsAuroraActive)
				ConsoleManager.CONSOLE_spawn_aurorabear();
			else
				ConsoleManager.CONSOLE_spawn_bear();
		}

		public static void SpawnMoose()
		{
			SpawningAnimal = true;
			ConsoleManager.CONSOLE_spawn_moose();
		}

		public static void SpawnStalkingWolf()
		{
			SpawningAnimal = true;
			if (IsAuroraActive)
				ConsoleManager.CONSOLE_spawn_aurorawolf();
			else
				ConsoleManager.CONSOLE_spawn_wolf();
		}

		public static void SpawnBunnyExplosion()
		{
			SpawnedAnimalCounter = 0;
			SpawningAnimal = true;
			for (int i = 0; i < SpawningAnimalTargetCount; i++)
			{
				ConsoleManager.CONSOLE_spawn_rabbit();
			}
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

		private static Il2CppSystem.Collections.Generic.List<GearItem> GetItemsInInventory()
		{
			var filter = (GearItem gi) => { return !string.IsNullOrEmpty(gi.name); };
			var list = new Il2CppSystem.Collections.Generic.List<GearItem>();

			GameManager.GetInventoryComponent().GetGearItems(filter, list);

			return list;
		}
	}
}
