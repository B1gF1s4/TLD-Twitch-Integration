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

		public enum WeatherHelp
		{
			Clear,
			Fog,
			Snow,
			Cloudy
		}

		public enum WeatherHarm
		{
			Blizzard,
			Fog,
			Snow
		}

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
					if (!ShouldExecuteWeatherRedeem())
						return false;
					// TODO: evaluate userInput
					GameService.WeatherToChange = WeatherStage.Clear;
					break;

				case RedeemNames.WEATHER_HARM:
					if (!ShouldExecuteWeatherRedeem())
						return false;
					// TODO: evaluate userInput
					GameService.WeatherToChange = WeatherStage.Blizzard;
					break;

				case RedeemNames.WEATHER_AURORA:
					if (!ShouldExecuteWeatherRedeem())
						return false;
					// TODO: implement forcing aurora
					break;

				case RedeemNames.ANIMAL_T_WOLVES:
					if (!ShouldExecuteAnimalRedeem())
						return false;
					if (GameState.IsAuroraFading)
						return false;
					var packSize = int.TryParse(redeem.UserInput, out var number) ?
						(number >= 2 && number <= 5) ? number : 5 : 5;
					GameService.SpawningAnimalTargetCount = packSize;
					GameService.AnimalToSpawn = AnimalRedeemType.TWolves;
					break;

				case RedeemNames.ANIMAL_BIG_GAME:
					if (!ShouldExecuteAnimalRedeem())
						return false;
					var defaultAnimal = Settings.ModSettings.AllowBigGameBear ?
						"bear" : "moose";
					var userInputAnimal = string.IsNullOrEmpty(redeem.UserInput) ?
						defaultAnimal : redeem.UserInput;
					var animalToSet = userInputAnimal.Contains("bear") ? "bear" :
						userInputAnimal.Contains("moose") ? "moose" : defaultAnimal;
					if (animalToSet == "moose" && GameState.IsAuroraFading)
						return false;
					if (animalToSet == "bear")
						GameService.AnimalToSpawn = AnimalRedeemType.Bear;
					if (animalToSet == "moose")
						GameService.AnimalToSpawn = AnimalRedeemType.Moose;
					break;

				case RedeemNames.ANIMAL_STALKING_WOLF:
					if (!ShouldExecuteAnimalRedeem())
						return false;
					if (GameState.IsAuroraFading)
						return false;
					GameService.AnimalToSpawn = AnimalRedeemType.StalkingWolf;
					break;

				case RedeemNames.ANIMAL_BUNNY_EXPLOSION:
					if (!ShouldExecuteAnimalRedeem())
						return false;
					GameService.SpawningAnimalTargetCount = Settings.ModSettings.BunnyCount;
					GameService.AnimalToSpawn = AnimalRedeemType.BunnyExplosion;
					break;

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

		private static bool ShouldExecuteWeatherRedeem()
		{
			if (GameState.IsInBuilding)
				return false;

			return true;
		}

		private static bool ShouldExecuteAnimalRedeem()
		{
			if (GameState.IsInBuilding)
				return false;

			return true;
		}
	}
}
