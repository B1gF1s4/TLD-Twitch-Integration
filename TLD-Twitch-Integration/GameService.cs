using Il2Cpp;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration
{
	public static class GameService
	{

		public static AnimalRedeemType AnimalToSpawn { get; set; }

		public static bool SpawningAnimal { get; set; }
		public static int SpawnedAnimalCounter { get; set; }

		public const int TWolfPackSize = 5;
		public const int BunnyExplosionSize = 30;

		public static void SpawnTWolves(bool aurora)
		{
			SpawnedAnimalCounter = 0;
			SpawningAnimal = true;
			for (int i = 0; i < TWolfPackSize; i++)
			{
				if (aurora)
					ConsoleManager.CONSOLE_spawn_grey_wolf_aurora();
				else
					ConsoleManager.CONSOLE_spawn_wolf_grey();
			}
		}

		public static void SpawnBear(bool aurora)
		{
			SpawningAnimal = true;
			if (aurora)
				ConsoleManager.CONSOLE_spawn_aurorabear();
			else
				ConsoleManager.CONSOLE_spawn_bear();
		}

		public static void SpawnMoose()
		{
			SpawningAnimal = true;
			ConsoleManager.CONSOLE_spawn_moose();
		}

		public static void SpawnStalkingWolf(bool aurora)
		{
			SpawningAnimal = true;
			if (aurora)
				ConsoleManager.CONSOLE_spawn_aurorawolf();
			else
				ConsoleManager.CONSOLE_spawn_wolf();
		}

		public static void SpawnBunnyExplosion()
		{
			SpawnedAnimalCounter = 0;
			SpawningAnimal = true;
			for (int i = 0; i < BunnyExplosionSize; i++)
			{
				ConsoleManager.CONSOLE_spawn_rabbit();
			}
		}

		public static bool IsInShelter()
		{
			return GameManager.GetSnowShelterManager().PlayerInShelter();
		}

		public static bool IsInBuilding()
		{
			if (GameManager.GetWeatherComponent().IsIndoorScene())
			{
				return true;
			}
			else if (GameManager.GetWeatherComponent().IsIndoorEnvironment())
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsInCar()
		{
			return GameManager.GetPlayerInVehicle().IsInside();
		}

		public static void ChangeWeather(WeatherStage stage)
		{
			GameManager.GetWeatherTransitionComponent()?.ForceUnmanagedWeatherStage(stage, 10f);
		}

		public static void ChangeMeter(MeterType type, bool isHelp)
		{
			var value = isHelp ? 90f : 10f;

			switch (type)
			{
				case MeterType.Fatigue:
					var fatigue = GameManager.GetFatigueComponent();
					if (fatigue != null)
						fatigue.m_CurrentFatigue = 100f - value;
					break;
				case MeterType.Hunger:
					var hunger = GameManager.GetHungerComponent();
					if (hunger != null)
						hunger.m_CurrentReserveCalories = value / 100f * hunger.m_MaxReserveCalories;
					break;
				case MeterType.Thirst:
					var thirst = GameManager.GetThirstComponent();
					if (thirst != null)
						thirst.m_CurrentThirst = 100f - value;
					break;
				case MeterType.Cold:
					var cold = GameManager.GetFreezingComponent();
					if (cold != null)
						cold.m_CurrentFreezing = 100f - value;
					break;
				default:
					break;
			}
		}

		public static void PlayPlayerSound(Sound sound)
		{
			var ignoreDelay = (Il2CppVoice.Priority)PlayerVoice.Options.IgnoreNonCriticalDelay;
			var soundsRessourceName = "";

			switch (sound)
			{
				case Sound.Hello:
					soundsRessourceName = "PLAY_TODDAWN";
					break;
				case Sound.GoodNight:
					soundsRessourceName = "PLAY_FATIGUEYAWN";
					break;
				case Sound.Happy420:
					soundsRessourceName = "PLAY_SUFFOCATIONCOUGH";
					break;
				case Sound.Hydrate:
					soundsRessourceName = "PLAY_VERYTHIRSTY";
					break;
				default:
					break;
			}

			if (string.IsNullOrEmpty(soundsRessourceName))
				return;

			GameManager.GetPlayerVoiceComponent()?.Play(soundsRessourceName, ignoreDelay);
		}

		public static void PlayPlayerSound(string sound)
		{
			var ignoreDelay = (Il2CppVoice.Priority)PlayerVoice.Options.IgnoreNonCriticalDelay;

			if (string.IsNullOrEmpty(sound))
				return;

			GameManager.GetPlayerVoiceComponent()?.Play(sound, ignoreDelay);
		}

		public static void HandleAffliction(string type)
		{
			// TODO: needs fixing!

			switch (type)
			{
				case "CabinFever":
					GameManager.GetCabinFeverComponent()?.CabinFeverStart(true, false);
					break;
				case "Dysentery":
					GameManager.GetDysenteryComponent()?.DysenteryStart(true, false);
					break;
				case "FoodPoisoning":
					GameManager.GetFoodPoisoningComponent()?.FoodPoisoningStart("GAMEPLAY_TaintedFood", true, false);
					break;
				case "Hypothermia":
					GameManager.GetHypothermiaComponent()?.HypothermiaStart("GAMEPLAY_ColdWeather", true, false);
					break;
				case "Parasites":
					GameManager.GetIntestinalParasitesComponent()?.IntestinalParasitesStart(false);
					break;
				default:
					break;
			}
		}



	}
}
