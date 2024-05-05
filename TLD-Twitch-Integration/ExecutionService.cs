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

		public enum AnimalRedeemType
		{
			None,
			TWolves,
			Bear,
			Moose,
			StalkingWolf,
			BunnyExplosion
		}

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
				case RedeemNames.WEATHER_BLIZZARD:
				case RedeemNames.WEATHER_CLEAR:
				case RedeemNames.WEATHER_LIGHT_FOG:
				case RedeemNames.WEATHER_DENSE_FOG:
				case RedeemNames.WEATHER_PARTLY_CLOUDY:
				case RedeemNames.WEATHER_CLOUDY:
				case RedeemNames.WEATHER_LIGHT_SNOW:
				case RedeemNames.WEATHER_HEAVY_SNOW:
					if (!ShouldExecuteWeatherRedeem())
						return false;
					var weatherStage = GetWeatherFromRedeemName(defaultTitle);
					GameService.ChangeWeather(weatherStage);
					break;

				case RedeemNames.SOUND_HELLO:
				case RedeemNames.SOUND_GOOD_NIGHT:
				case RedeemNames.SOUND_420:
				case RedeemNames.SOUND_HYDRATE:
					var sound = GetSoundFromRedeemName(defaultTitle);
					GameService.PlayPlayerSound(sound);
					break;

				case RedeemNames.SOUND:
					GameService.PlayPlayerSound(redeem.UserInput!);
					break;

				case RedeemNames.ANIMAL_T_WOLVES:
					if (!ShouldExecuteAnimalRedeem())
						return false;
					if (GameState.IsAuroraFading)
						return false;
					GameService.AnimalToSpawn = AnimalRedeemType.TWolves;
					break;

				case RedeemNames.ANIMAL_BEAR:
					if (!ShouldExecuteAnimalRedeem())
						return false;
					if (GameState.IsAuroraFading)
						return false;
					GameService.AnimalToSpawn = AnimalRedeemType.Bear;
					break;

				case RedeemNames.ANIMAL_MOOSE:
					if (!ShouldExecuteAnimalRedeem())
						return false;
					if (GameState.IsAuroraActive || GameState.IsAuroraFading)
						return false;
					else
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
					GameService.AnimalToSpawn = AnimalRedeemType.BunnyExplosion;
					break;

				case RedeemNames.STATUS_HUNGRY:
					GameService.ChangeMeter(MeterType.Hunger, false);
					break;

				case RedeemNames.STATUS_THIRSTY:
					GameService.ChangeMeter(MeterType.Thirst, false);
					break;

				case RedeemNames.STATUS_TIRED:
					GameService.ChangeMeter(MeterType.Fatigue, false);
					break;

				case RedeemNames.STATUS_FREEZING:
					GameService.ChangeMeter(MeterType.Cold, false);
					break;

				case RedeemNames.STATUS_FULL:
					GameService.ChangeMeter(MeterType.Hunger, true);
					break;

				case RedeemNames.STATUS_NOT_THIRSTY:
					GameService.ChangeMeter(MeterType.Thirst, true);
					break;

				case RedeemNames.STATUS_AWAKE:
					GameService.ChangeMeter(MeterType.Fatigue, true);
					break;

				case RedeemNames.STATUS_WARM:
					GameService.ChangeMeter(MeterType.Cold, true);
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

		private static WeatherStage GetWeatherFromRedeemName(string title)
		{
			var weather = title.Replace("TTI: ", "").Replace(" ", "");
			var stage = Enum.Parse(typeof(WeatherStage), weather) ?? throw new InvalidCastException();
			return (WeatherStage)stage;
		}

		private static Sound GetSoundFromRedeemName(string title)
		{
			return title switch
			{
				RedeemNames.SOUND_HELLO => Sound.Hello,
				RedeemNames.SOUND_GOOD_NIGHT => Sound.GoodNight,
				RedeemNames.SOUND_420 => Sound.Happy420,
				RedeemNames.SOUND_HYDRATE => Sound.Hydrate,
				_ => throw new InvalidOperationException(),
			};
		}
	}
}
