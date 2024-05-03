using Il2Cpp;

namespace TLD_Twitch_Integration
{
	public static class GameService
	{

		public enum MeterType
		{
			Fatigue,
			Hunger,
			Thirst,
			Cold
		}

		public enum Sound
		{
			Hello,
			GoodNight,
			Happy420,
			Hydrate
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
