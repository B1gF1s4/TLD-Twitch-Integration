using Il2Cpp;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration
{
	public static class GameService
	{

		public static AnimalRedeemType AnimalToSpawn { get; set; }
		public static bool SpawningAnimal { get; set; }
		public static int SpawnedAnimalCounter { get; set; }
		public static int SpawningAnimalTargetCount { get; set; }

		public static WeatherStage WeatherToChange { get; set; }
			= WeatherStage.Undefined;

		public static bool ShouldStartCabinFever { get; set; }
		public static bool ShouldStartDysentery { get; set; }
		public static bool ShouldStartFoodPoisoning { get; set; }
		public static bool ShouldStartHypothermia { get; set; }
		public static bool ShouldStartParasites { get; set; }

		public static void SpawnTWolves(bool aurora)
		{
			SpawnedAnimalCounter = 0;
			SpawningAnimal = true;
			for (int i = 0; i < SpawningAnimalTargetCount; i++)
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
			for (int i = 0; i < SpawningAnimalTargetCount; i++)
			{
				ConsoleManager.CONSOLE_spawn_rabbit();
			}
		}

		public static void ChangeMeter(StatusMeter type, bool isHelp)
		{
			var value = isHelp ? 90f : 10f;

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

	}
}
