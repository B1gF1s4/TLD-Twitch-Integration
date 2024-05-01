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
		private const int _interval = 5;

		private static DateTime _lastUpdated;

		public static async Task OnUpdate()
		{
			if (!Mod.ShouldUpdate(_lastUpdated, _interval))
				return;

			_lastUpdated = DateTime.UtcNow;

			if (!Settings.ModSettings.Enabled)
				return;

			if (string.IsNullOrEmpty(GameManager.m_ActiveScene) ||
				GameManager.m_ActiveScene == "MainMenu")
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

			ExecuteRedeem(redeem);

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

		private static void ExecuteRedeem(Redemption redeem)
		{

			var title = Settings.Redeems.GetRedeemNameById(redeem.CustomReward?.Id!);

			if (string.IsNullOrEmpty(title))
				return;

			if (Settings.ModSettings.ShowAlert)
				HUDMessage.AddMessage($"{redeem.UserName} redeemed {redeem.CustomReward?.Title}",
					_interval - 1, true, true);

			switch (title)
			{
				case RedeemNames.WEATHER_BLIZZARD:
				case RedeemNames.WEATHER_CLEAR:
				case RedeemNames.WEATHER_LIGHT_FOG:
				case RedeemNames.WEATHER_DENSE_FOG:
				case RedeemNames.WEATHER_PARTLY_CLOUDY:
				case RedeemNames.WEATHER_CLOUDY:
				case RedeemNames.WEATHER_LIGHT_SNOW:
				case RedeemNames.WEATHER_HEAVY_SNOW:
					var weatherStage = GetWeatherFromRedeemName(title);
					GameService.ChangeWeather(weatherStage);
					break;
				case RedeemNames.SOUND_HELLO:
				case RedeemNames.SOUND_GOOD_NIGHT:
				case RedeemNames.SOUND_420:
				case RedeemNames.SOUND_HYDRATE:
					var sound = GetSoundFromRedeemName(title);
					GameService.PlayPlayerSound(sound);
					break;
				default:
					break;
			}
		}

		private static WeatherStage GetWeatherFromRedeemName(string title)
		{
			var weather = title.Replace("TTI: ", "").Replace(" ", "");
			var stage = Enum.Parse(typeof(WeatherStage), weather) ?? throw new InvalidCastException();
			return (WeatherStage)stage;
		}

		private static GameService.Sound GetSoundFromRedeemName(string title)
		{
			return title switch
			{
				RedeemNames.SOUND_HELLO => GameService.Sound.Hello,
				RedeemNames.SOUND_GOOD_NIGHT => GameService.Sound.GoodNight,
				RedeemNames.SOUND_420 => GameService.Sound.Happy420,
				RedeemNames.SOUND_HYDRATE => GameService.Sound.Hydrate,
				_ => throw new InvalidOperationException(),
			};
		}
	}
}