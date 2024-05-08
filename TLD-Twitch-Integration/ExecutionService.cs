﻿using Il2Cpp;
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
			None,
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

		public enum AfflictionRedeemType
		{
			FoodPoisoning,
			Dysentery,
			CabinFever,
			Parasites,
			Hypothermia
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

			var executed = false;
			try
			{
				executed = ExecuteRedeem(redeem, defaultTitle);
			}
			catch (RequiresRedeemRefundException refund)
			{
				await UpdateRedemption(userId, redeem, false);

				Melon<Mod>.Logger.Msg($"'{redeem.CustomReward?.Title}' redeemed by {redeem.UserName} refunded. " +
					$"- {refund.Message}");
				return;
			}

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

				await UpdateRedemption(userId, redeem, true);
			}
		}

		private static async Task UpdateRedemption(string userId, Redemption redeem, bool fulfill)
		{
			try
			{
				if (fulfill)
					await TwitchAdapter.FulfillRedemption(AuthService.ClientId, Settings.Token.Access, userId, redeem);
				else
					await TwitchAdapter.CancelRedemption(AuthService.ClientId, Settings.Token.Access, userId, redeem);

				ExecutionQueue.RemoveAll(r => r.Id == redeem.Id);
				RedemptionService.OpenRedeems.RemoveAll(r => r.Id == redeem.Id);
			}
			catch (InvalidTokenException)
			{
				await AuthService.RefreshToken();
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
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
					return ExecuteWeatherAurora();

				case RedeemNames.ANIMAL_T_WOLVES:
					return ExecuteTWolfRedeem(redeem);

				case RedeemNames.ANIMAL_BIG_GAME:
					return ExecuteBigGameRedeem(redeem);

				case RedeemNames.ANIMAL_STALKING_WOLF:
					return ExecuteStalkingWolfRedeem();

				case RedeemNames.ANIMAL_BUNNY_EXPLOSION:
					return ExecuteBunnyExplosionRedeem();

				case RedeemNames.STATUS_HELP:
					return ExecuteStatusHelpRedeem(redeem);

				case RedeemNames.STATUS_HARM:
					return ExecuteStatusHarmRedeem(redeem);

				case RedeemNames.STATUS_AFFLICTION:
					return ExecuteStatusAfflictionRedeem();

				case RedeemNames.SOUND_420:
					GameService.PlayPlayerSound("PLAY_SUFFOCATIONCOUGH");
					break;

				case RedeemNames.DEV_SOUND:
					GameService.PlayPlayerSound(redeem.UserInput!);
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

		private static bool ExecuteWeatherAurora()
		{
			if (GameManager.m_ActiveScene == "Dam")
				return false;

			if (GameState.IsInBuilding)
				return false;

			GameService.ShouldStartAurora = true;

			return true;
		}

		private static bool ExecuteStatusHelpRedeem(Redemption redeem)
		{
			var defaultStatusHelp = Settings.ModSettings.AllowStatusHelpWarm ? StatusMeter.Cold :
						Settings.ModSettings.AllowStatusHelpAwake ? StatusMeter.Fatigue :
						Settings.ModSettings.AllowStatusHelpNotThirsty ? StatusMeter.Thirst : StatusMeter.Hunger;

			var userInputStatusHelp = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultStatusHelp.ToString().ToLower() : redeem.UserInput.ToLower();

			var statusHelpToSet = userInputStatusHelp.Contains("cold") ? StatusMeter.Cold :
				userInputStatusHelp.Contains("fatigue") ? StatusMeter.Fatigue :
				userInputStatusHelp.Contains("thirst") ? StatusMeter.Thirst :
				userInputStatusHelp.Contains("hunger") ? StatusMeter.Hunger : defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Cold && !Settings.ModSettings.AllowStatusHelpWarm)
				statusHelpToSet = defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Fatigue && !Settings.ModSettings.AllowStatusHelpAwake)
				statusHelpToSet = defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Thirst && !Settings.ModSettings.AllowStatusHelpNotThirsty)
				statusHelpToSet = defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Hunger && !Settings.ModSettings.AllowStatusHelpFull)
				statusHelpToSet = defaultStatusHelp;

			GameService.StatusMeterToChange = statusHelpToSet;
			GameService.IsHelpfulStatusMeterChange = true;

			return true;
		}

		private static bool ExecuteStatusHarmRedeem(Redemption redeem)
		{
			var defaultStatusHarm = Settings.ModSettings.AllowStatusHarmFreezing ? StatusMeter.Cold :
						Settings.ModSettings.AllowStatusHarmTired ? StatusMeter.Fatigue :
						Settings.ModSettings.AllowStatusHarmThirsty ? StatusMeter.Thirst : StatusMeter.Hunger;

			var userInputStatusHarm = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultStatusHarm.ToString().ToLower() : redeem.UserInput.ToLower();

			var statusHarmToSet = userInputStatusHarm.Contains("cold") ? StatusMeter.Cold :
				userInputStatusHarm.Contains("fatigue") ? StatusMeter.Fatigue :
				userInputStatusHarm.Contains("thirst") ? StatusMeter.Thirst :
				userInputStatusHarm.Contains("hunger") ? StatusMeter.Hunger : defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Cold && !Settings.ModSettings.AllowStatusHarmHungry)
				statusHarmToSet = defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Fatigue && !Settings.ModSettings.AllowStatusHarmTired)
				statusHarmToSet = defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Thirst && !Settings.ModSettings.AllowStatusHarmThirsty)
				statusHarmToSet = defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Hunger && !Settings.ModSettings.AllowStatusHarmHungry)
				statusHarmToSet = defaultStatusHarm;

			GameService.StatusMeterToChange = statusHarmToSet;
			GameService.IsHelpfulStatusMeterChange = false;

			return true;
		}

		private static bool ExecuteStatusAfflictionRedeem()
		{
			if (!Settings.ModSettings.AllowAfflictionCabinFever &&
				!Settings.ModSettings.AllowAfflictionDysentery &&
				!Settings.ModSettings.AllowAfflictionFoodPoisoning &&
				!Settings.ModSettings.AllowAfflictionHypothermia &&
				!Settings.ModSettings.AllowAfflictionParasites)
				throw new RequiresRedeemRefundException("All afflictions are currently disabled.");

			Random random = new();
			var affliction = GetRandomEnabledAffliction(random);

			switch (affliction)
			{
				case AfflictionRedeemType.FoodPoisoning:
					if (GameManager.GetFoodPoisoningComponent().HasFoodPoisoning())
						throw new RequiresRedeemRefundException("Player already has Food Poisoning.");

					GameService.ShouldStartFoodPoisoning = true;
					return true;

				case AfflictionRedeemType.Dysentery:
					if (GameManager.GetDysenteryComponent().HasDysentery())
						throw new RequiresRedeemRefundException("Player already has Dysentery.");

					GameService.ShouldStartDysentery = true;
					return true;

				case AfflictionRedeemType.CabinFever:
					if (GameManager.GetCabinFeverComponent().HasCabinFever())
						throw new RequiresRedeemRefundException("Player already has Cabin Fever.");

					GameService.ShouldStartCabinFever = true;
					return true;

				case AfflictionRedeemType.Parasites:
					if (GameManager.GetIntestinalParasitesComponent().HasIntestinalParasites())
						throw new RequiresRedeemRefundException("Player already has Parasites.");

					GameService.ShouldStartParasites = true;
					return true;

				case AfflictionRedeemType.Hypothermia:
					if (GameManager.GetHypothermiaComponent().HasHypothermia())
						throw new RequiresRedeemRefundException("Player already has Hypothermia.");

					GameService.ShouldStartHypothermia = true;
					return true;

				default:
					return false;
			}
		}

		private static AfflictionRedeemType GetRandomEnabledAffliction(Random random)
		{
			Array values = Enum.GetValues(typeof(AfflictionRedeemType));
			AfflictionRedeemType affliction = (AfflictionRedeemType)
				(values.GetValue(random.Next(values.Length)) ??
				throw new Exception("trying to cast null to enum"));

			if (IsAfflictionEnabled(affliction))
				return affliction;
			else
				return GetRandomEnabledAffliction(random);
		}

		private static bool IsAfflictionEnabled(AfflictionRedeemType affliction)
		{
			return affliction switch
			{
				AfflictionRedeemType.Parasites => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.Hypothermia => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.Dysentery => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.FoodPoisoning => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.CabinFever => Settings.ModSettings.AllowAfflictionParasites,
				_ => true,
			};
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
