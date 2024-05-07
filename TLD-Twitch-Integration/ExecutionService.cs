using Il2Cpp;
using MelonLoader;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch;
using TLD_Twitch_Integration.Twitch.Models;
using TLD_Twitch_Integration.Twitch.Redeems;

namespace TLD_Twitch_Integration
{
	public class ExecutionService
	{
		public static bool ExecutionPending { get; set; }
		public static List<Redemption> ExecutionQueue { get; set; } = new();


		private const int _interval = 6;

		private static DateTime _lastUpdated;

		public enum StatusMeter
		{
			Cold,
			Fatigue,
			Thirst,
			Hunger,
		}

		public enum AnimalRedeemType
		{
			None,
			TWolves,
			Bear,
			Moose,
			StalkingWolf,
			BunnyExplosion
		}

		public static async Task OnUpdate()
		{
			if (!Mod.ShouldUpdate(_lastUpdated, _interval))
				return;

			_lastUpdated = DateTime.UtcNow;

			if (!AuthService.IsConnected)
				return;

			if (!Settings.ModSettings.Enabled)
				return;

			if (string.IsNullOrEmpty(GameManager.m_ActiveScene) ||
				GameManager.m_ActiveScene == "MainMenu" ||
				GameManager.m_ActiveScene == "Boot" ||
				GameManager.m_ActiveScene == "Empty")
				return;

			if (GameManager.m_IsPaused)
				return;

			if (!RedemptionService.HasOpenRedeems)
				return;

			await HandleNextRedeem();

		}

		private static async Task HandleNextRedeem()
		{
			var userId = AuthService.User?.Id ??
				throw new NotLoggedInException();

			var redeemToExecute = ExecutionQueue.Any() ?
				ExecutionQueue.FirstOrDefault() :
				RedemptionService.OpenRedeems
					.OrderBy(r => r.RedeemedAt)
					.ToList()
					.FirstOrDefault();

			if (redeemToExecute == null)
				return;

			ExecutionPending = true;

			await TryExecuteRedeem(redeemToExecute, userId);

			ExecutionPending = false;
		}

		private static async Task TryExecuteRedeem(Redemption redeem, string userId)
		{
			GameState.Update();

			var defaultTitle = Settings.Redeems.GetRedeemNameById(redeem.CustomReward?.Id!);
			Melon<Mod>.Logger.Msg($"trying to execute redeem {defaultTitle}");

			var executed = ExecuteRedeem(redeem, defaultTitle);

			if (!executed)
			{
				Melon<Mod>.Logger.Msg($"redeem skipped, trying next");

				ExecutionQueue.Add(redeem);
				RedemptionService.OpenRedeems.RemoveAll(r => r.Id == redeem.Id);

				var nextRedeemToExecute = RedemptionService.OpenRedeems
					.OrderBy(r => r.RedeemedAt)
					.ToList()
					.FirstOrDefault();

				if (nextRedeemToExecute == null)
					return;

				await TryExecuteRedeem(nextRedeemToExecute, userId);
			}
			else
			{
				Melon<Mod>.Logger.Msg($"redeem executed, removing from redemption queue on twitch");

				if (Settings.ModSettings.ShowAlert)
					HUDMessage.AddMessage($"{redeem.UserName} redeemed {redeem.CustomReward?.Title}",
						_interval - 1, true, true);

				try
				{
					await TwitchAdapter.FulfillRedemption(AuthService.ClientId, Settings.Token.Access, userId, redeem);

					ExecutionQueue.RemoveAll(r => r.Id == redeem.Id);
					RedemptionService.OpenRedeems.RemoveAll(r => r.Id == redeem.Id);
				}
				catch (InvalidTokenException)
				{
					await AuthService.RefreshToken();
					return;
				}
				catch (Exception ex)
				{
					Melon<Mod>.Logger.Error(ex);
				}
			}
		}

		private static bool ExecuteRedeem(Redemption redeem, string defaultTitle)
		{
			switch (defaultTitle)
			{
				case RedeemNames.WEATHER_HELP:
					return ExecuteWeatherHelpRedeem(redeem);

				case RedeemNames.WEATHER_HARM:
					return ExecuteWeatherHarmRedeem(redeem);

				case RedeemNames.WEATHER_AURORA:
					if (GameState.IsInBuilding)
						return false;
					GameService.ShouldStartAurora = true;
					break;

				case RedeemNames.ANIMAL_T_WOLVES:
					return ExecuteTWolfRedeem(redeem);

				case RedeemNames.ANIMAL_BIG_GAME:
					return ExecuteBigGameRedeem(redeem);

				case RedeemNames.ANIMAL_STALKING_WOLF:
					return ExecuteStalkingWolfRedeem();

				case RedeemNames.ANIMAL_BUNNY_EXPLOSION:
					return ExecuteBunnyExplosionRedeem();

				case RedeemNames.STATUS_HELP:
					// TODO: evaluate userInput
					GameService.ChangeMeter(StatusMeter.Hunger, true);
					break;

				case RedeemNames.STATUS_HARM:
					// TODO: evaluate userInput
					GameService.ChangeMeter(StatusMeter.Hunger, false);
					break;

				case RedeemNames.SOUND_420:
					GameService.PlayPlayerSound("PLAY_SUFFOCATIONCOUGH");
					break;

				case RedeemNames.DEV_SOUND:
					GameService.PlayPlayerSound(redeem.UserInput!);
					break;

				case RedeemNames.STATUS_CABIN_FEVER:
					GameService.ShouldStartCabinFever = true;
					break;

				case RedeemNames.STATUS_DYSENTERY:
					GameService.ShouldStartDysentery = true;
					break;

				case RedeemNames.STATUS_FOOD_POISONING:
					GameService.ShouldStartFoodPoisoning = true;
					break;

				case RedeemNames.STATUS_HYPOTHERMIA:
					GameService.ShouldStartHypothermia = true;
					break;

				default:
					Melon<Mod>.Logger.Error($"redeem operation not supported - {defaultTitle}");
					break;
			}

			return true;
		}

		private static bool ExecuteWeatherHelpRedeem(Redemption redeem)
		{
			if (GameState.IsInBuilding)
				return false;

			var defaultWeatherHelp = Settings.ModSettings.AllowWeatherHelpClear ? WeatherStage.Clear :
				Settings.ModSettings.AllowWeatherHelpFog ? WeatherStage.LightFog :
				Settings.ModSettings.AllowWeatherHelpSnow ? WeatherStage.LightSnow : WeatherStage.PartlyCloudy;

			var userInputWeatherHelp = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultWeatherHelp.ToString().ToLower() : redeem.UserInput.ToLower();

			var weatherHelpToSet = userInputWeatherHelp.Contains("clear") ? WeatherStage.Clear :
				userInputWeatherHelp.Contains("fog") ? WeatherStage.LightFog :
				userInputWeatherHelp.Contains("snow") ? WeatherStage.LightSnow :
				userInputWeatherHelp.Contains("cloudy") ? WeatherStage.PartlyCloudy : defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.Clear && !Settings.ModSettings.AllowWeatherHelpClear)
				weatherHelpToSet = defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.LightFog && !Settings.ModSettings.AllowWeatherHelpFog)
				weatherHelpToSet = defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.LightSnow && !Settings.ModSettings.AllowWeatherHelpSnow)
				weatherHelpToSet = defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.PartlyCloudy && !Settings.ModSettings.AllowWeatherHelpCloudy)
				weatherHelpToSet = defaultWeatherHelp;

			GameService.WeatherToChange = weatherHelpToSet;

			return true;
		}

		private static bool ExecuteWeatherHarmRedeem(Redemption redeem)
		{
			if (GameState.IsInBuilding)
				return false;

			var defaultWeatherHarm = Settings.ModSettings.AllowWeatherHarmBlizzard ? WeatherStage.Blizzard :
				Settings.ModSettings.AllowWeatherHarmFog ? WeatherStage.DenseFog : WeatherStage.HeavySnow;

			var userInputWeatherHarm = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultWeatherHarm.ToString().ToLower() : redeem.UserInput.ToLower();

			var weatherHarmToSet = userInputWeatherHarm.Contains("blizzard") ? WeatherStage.Blizzard :
				userInputWeatherHarm.Contains("fog") ? WeatherStage.DenseFog :
				userInputWeatherHarm.Contains("snow") ? WeatherStage.HeavySnow : defaultWeatherHarm;

			if (weatherHarmToSet == WeatherStage.Blizzard && !Settings.ModSettings.AllowWeatherHarmBlizzard)
				weatherHarmToSet = defaultWeatherHarm;

			if (weatherHarmToSet == WeatherStage.DenseFog && !Settings.ModSettings.AllowWeatherHarmFog)
				weatherHarmToSet = defaultWeatherHarm;

			if (weatherHarmToSet == WeatherStage.HeavySnow && !Settings.ModSettings.AllowWeatherHarmSnow)
				weatherHarmToSet = defaultWeatherHarm;

			GameService.WeatherToChange = weatherHarmToSet;

			return true;
		}

		private static bool ExecuteTWolfRedeem(Redemption redeem)
		{
			if (GameState.IsInBuilding)
				return false;

			if (GameState.IsAuroraFading)
				return false;

			var packSize = int.TryParse(redeem.UserInput, out var number) ?
				(number >= 2 && number <= 5) ? number : 5 : 5;

			GameService.SpawningAnimalTargetCount = packSize;
			GameService.AnimalToSpawn = AnimalRedeemType.TWolves;

			return true;
		}

		private static bool ExecuteBigGameRedeem(Redemption redeem)
		{
			if (GameState.IsInBuilding)
				return false;

			var defaultAnimal = Settings.ModSettings.AllowBigGameBear ?
				AnimalRedeemType.Bear : AnimalRedeemType.Moose;

			var userInputAnimal = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultAnimal.ToString().ToLower() : redeem.UserInput.ToLower();

			var animalToSet = userInputAnimal.Contains("bear") ? AnimalRedeemType.Bear :
				userInputAnimal.Contains("moose") ? AnimalRedeemType.Moose : defaultAnimal;

			if (GameState.IsAuroraFading)
				return false;

			if (animalToSet == AnimalRedeemType.Moose)
			{
				if (GameState.IsAuroraActive)
					return false;

				if (!Settings.ModSettings.AllowBigGameMoose)
					return false;

				GameService.AnimalToSpawn = AnimalRedeemType.Moose;

				return true;
			}
			else
			{
				if (!Settings.ModSettings.AllowBigGameMoose)
					return false;

				GameService.AnimalToSpawn = AnimalRedeemType.Bear;

				return true;
			}
		}

		private static bool ExecuteStalkingWolfRedeem()
		{
			if (GameState.IsInBuilding)
				return false;

			if (GameState.IsAuroraFading)
				return false;

			GameService.AnimalToSpawn = AnimalRedeemType.StalkingWolf;

			return true;
		}

		private static bool ExecuteBunnyExplosionRedeem()
		{
			if (GameState.IsInBuilding)
				return false;

			GameService.SpawningAnimalTargetCount = Settings.ModSettings.BunnyCount;
			GameService.AnimalToSpawn = AnimalRedeemType.BunnyExplosion;

			return true;
		}
	}
}
