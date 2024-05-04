using Il2Cpp;
using MelonLoader;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch;
using TLD_Twitch_Integration.Twitch.Models;
using TLD_Twitch_Integration.Twitch.Redeems;
using static TLD_Twitch_Integration.GameService;

namespace TLD_Twitch_Integration
{
	public class ExecutionService
	{
		private const int _interval = 6;

		private static DateTime _lastUpdated;

		public static async Task OnUpdate()
		{
			if (!Mod.ShouldUpdate(_lastUpdated, _interval))
				return;

			_lastUpdated = DateTime.UtcNow;

			if (!Settings.ModSettings.Enabled)
				return;

			if (string.IsNullOrEmpty(GameManager.m_ActiveScene) ||
				GameManager.m_ActiveScene == "MainMenu" ||
				GameManager.m_ActiveScene == "Boot" ||
				GameManager.m_ActiveScene == "Empty")
				return;

			if (GameManager.m_IsPaused)
				return;

			if (AuthService.IsConnected && RedemptionService.HasOpenRedeems)
				await HandleNextRedeem();

		}

		private static async Task HandleNextRedeem()
		{
			var userId = AuthService.User?.Id ?? throw new NotLoggedInException();

			var redeem = RedemptionService.OpenRedeems
				.OrderBy(r => r.RedeemedAt)
				.ToList()
				.FirstOrDefault();

			if (redeem == null)
				return;

			var executed = ExecuteRedeem(redeem);
			if (!executed)
				return;

			if (Settings.ModSettings.ShowAlert)
				HUDMessage.AddMessage($"{redeem.UserName} redeemed {redeem.CustomReward?.Title}",
					_interval - 1, true, true);

			try
			{
				await TwitchAdapter.FulfillRedemption(AuthService.ClientId, Settings.Token.Access, userId, redeem);
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

		private static bool ExecuteRedeem(Redemption redeem)
		{

			var defaultTitle = Settings.Redeems.GetRedeemNameById(redeem.CustomReward?.Id!);

			if (string.IsNullOrEmpty(defaultTitle))
				return false;

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
					var weatherStage = GetWeatherFromRedeemName(defaultTitle);
					ChangeWeather(weatherStage);
					break;
				case RedeemNames.SOUND_HELLO:
				case RedeemNames.SOUND_GOOD_NIGHT:
				case RedeemNames.SOUND_420:
				case RedeemNames.SOUND_HYDRATE:
					var sound = GetSoundFromRedeemName(defaultTitle);
					PlayPlayerSound(sound);
					break;
				case RedeemNames.SOUND:
					PlayPlayerSound(redeem.UserInput!);
					break;
				case RedeemNames.ANIMAL_T_WOLVES:
					if (IsInBuilding())
						return false;
					AnimalToSpawn = AnimalRedeemType.TWolves;
					break;
				case RedeemNames.ANIMAL_BEAR:
					if (IsInBuilding())
						return false;
					AnimalToSpawn = AnimalRedeemType.Bear;
					break;
				case RedeemNames.ANIMAL_MOOSE:
					if (IsInBuilding())
						return false;
					AnimalToSpawn = AnimalRedeemType.Moose;
					break;
				case RedeemNames.ANIMAL_STALKING_WOLF:
					if (IsInBuilding())
						return false;
					AnimalToSpawn = AnimalRedeemType.StalkingWolf;
					break;
				case RedeemNames.ANIMAL_BUNNY_EXPLOSION:
					if (IsInBuilding())
						return false;
					AnimalToSpawn = AnimalRedeemType.BunnyExplosion;
					break;
				case RedeemNames.STATUS_HUNGRY:
					ChangeMeter(MeterType.Hunger, false);
					break;
				case RedeemNames.STATUS_THIRSTY:
					ChangeMeter(MeterType.Thirst, false);
					break;
				case RedeemNames.STATUS_TIRED:
					ChangeMeter(MeterType.Fatigue, false);
					break;
				case RedeemNames.STATUS_FREEZING:
					ChangeMeter(MeterType.Cold, false);
					break;
				case RedeemNames.STATUS_FULL:
					ChangeMeter(MeterType.Hunger, true);
					break;
				case RedeemNames.STATUS_NOT_THIRSTY:
					ChangeMeter(MeterType.Thirst, true);
					break;
				case RedeemNames.STATUS_AWAKE:
					ChangeMeter(MeterType.Fatigue, true);
					break;
				case RedeemNames.STATUS_WARM:
					ChangeMeter(MeterType.Cold, true);
					break;
				default:
					break;
			}

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
