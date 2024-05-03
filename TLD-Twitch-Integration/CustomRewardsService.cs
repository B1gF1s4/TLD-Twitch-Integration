using MelonLoader;
using System.Reflection;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch;
using TLD_Twitch_Integration.Twitch.Models;
using TLD_Twitch_Integration.Twitch.Redeems;

namespace TLD_Twitch_Integration
{
	public class CustomRewardsService
	{
		public static bool IsInitialized { get; private set; }

		public static bool SyncRequired { get; set; }

		private const int _interval = 5;

		private static DateTime _lastUpdated;

		public static async Task OnUpdate()
		{
			if (!Mod.ShouldUpdate(_lastUpdated, _interval))
				return;

			_lastUpdated = DateTime.UtcNow;

			if (AuthService.IsConnected)
			{
				if (!IsInitialized)
				{
					await CreateCustomRewards();
					IsInitialized = true;
				}
				else
				{
					await CheckForDeletedCustomRewards();

					if (SyncRequired)
						await SyncCustomRewardsWithSettings();
				}
			}
		}

		private static async Task CreateCustomRewards()
		{
			var type = Settings.Redeems.GetType();
			var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			var needsTokenRefresh = false;
			var anythingCreated = false;

			foreach (var field in fields)
			{
				var id = field.GetValue(Settings.Redeems)?.ToString();

				if (string.IsNullOrEmpty(id))
				{
					var fieldName = field.Name;
					var redeemName = Settings.Redeems.GetRedeemNameByFieldName(fieldName);
					var defaultCustomReward = RedeemDefaults.GetRedeemDefault(redeemName);
					var userId = AuthService.User?.Id ?? throw new NotLoggedInException();

					try
					{
						var customReward = await TwitchAdapter.CreateCustomReward(
							AuthService.ClientId, Settings.Token.Access, userId, defaultCustomReward);

						field.SetValue(Settings.Redeems, customReward.Id);
						Melon<Mod>.Logger.Msg($"redeem created: {customReward.Title}");
						anythingCreated = true;
					}
					catch (CustomRewardAlreadyExistsException)
					{
						Melon<Mod>.Logger.Msg($"custom reward {redeemName} already exist. " +
							$"please delete it in your twitch dashboard. " +
							$"TTI will try and create it again on next game start.");
					}
					catch (InvalidTokenException)
					{
						needsTokenRefresh = true;
						break;
					}
					catch (Exception ex)
					{
						Melon<Mod>.Logger.Error(ex);
					}
				}
			}

			if (needsTokenRefresh)
			{
				await AuthService.RefreshToken();
				return;
			}

			if (anythingCreated)
			{
				Melon<Mod>.Logger.Msg($"finished creating redeems. saving IDs.");
				Settings.Redeems.Save();
			}

			await SyncCustomRewardsWithSettings();
		}

		private static async Task CheckForDeletedCustomRewards()
		{
			var userId = AuthService.User?.Id ?? throw new NotLoggedInException();
			var rewardsOnTwitch = new List<CustomReward>();

			try
			{
				rewardsOnTwitch = await TwitchAdapter.GetAvailableCustomRewards(
					AuthService.ClientId, Settings.Token.Access, userId);
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

			var type = Settings.Redeems.GetType();
			var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			var needsTokenRefresh = false;
			var anythingCreated = false;

			foreach (var redeemIdField in fields)
			{
				var fieldName = redeemIdField.Name;
				var redeemName = Settings.Redeems.GetRedeemNameByFieldName(fieldName);
				var redeemId = redeemIdField.GetValue(Settings.Redeems);

				if (redeemId == null || string.IsNullOrEmpty(redeemId.ToString()))
					continue;

				var found = rewardsOnTwitch.Any(r => r.Id == redeemId.ToString());

				if (!found)
				{
					Melon<Mod>.Logger.Msg($"missing twitch redeem detected. recreating default. {redeemName}");
					var defaultCustomReward = RedeemDefaults.GetRedeemDefault(redeemName);

					try
					{
						var customReward = await TwitchAdapter.CreateCustomReward(
						AuthService.ClientId, Settings.Token.Access, userId, defaultCustomReward);

						redeemIdField.SetValue(Settings.Redeems, customReward.Id);
						Melon<Mod>.Logger.Msg($"redeem created: {customReward.Title}");
						anythingCreated = true;
					}
					catch (InvalidTokenException)
					{
						needsTokenRefresh = true;
						break;
					}
					catch (Exception ex)
					{
						Melon<Mod>.Logger.Error(ex);
					}
				}
			}

			if (needsTokenRefresh)
			{
				await AuthService.RefreshToken();
				return;
			}

			if (anythingCreated)
			{
				Melon<Mod>.Logger.Msg($"finished recreating redeems. saving IDs.");
				Settings.Redeems.Save();
				await SyncCustomRewardsWithSettings();
			}
		}

		private static async Task SyncCustomRewardsWithSettings()
		{
			Melon<Mod>.Logger.Msg($"syncing twitch redeems with game settings ..");

			try
			{
				// TODO: refactor to use reflection, possibly custom attribute and iterate settings, or redeem defaults

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_BLIZZARD),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherBlizzard,
					RedeemNames.WEATHER_BLIZZARD);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_CLEAR),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherClear,
					RedeemNames.WEATHER_CLEAR);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_LIGHT_FOG),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherLightFog,
					RedeemNames.WEATHER_LIGHT_FOG);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_DENSE_FOG),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherDenseFog,
					RedeemNames.WEATHER_DENSE_FOG);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_PARTLY_CLOUDY),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherPartlyCloudy,
					RedeemNames.WEATHER_PARTLY_CLOUDY);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_CLOUDY),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherCloudy,
					RedeemNames.WEATHER_CLOUDY);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_LIGHT_SNOW),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherLightSnow,
					RedeemNames.WEATHER_LIGHT_SNOW);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_HEAVY_SNOW),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherRedeems &&
					Settings.ModSettings.AllowWeatherHeavySnow,
					RedeemNames.WEATHER_HEAVY_SNOW);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.SOUND),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowSoundRedeems &&
					Settings.ModSettings.AllowDevSoundCheck,
					RedeemNames.SOUND);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.SOUND_HELLO),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowSoundRedeems &&
					Settings.ModSettings.AllowSoundHello,
					RedeemNames.SOUND_HELLO);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.SOUND_420),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowSoundRedeems &&
					Settings.ModSettings.AllowSound420,
					RedeemNames.SOUND_420);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.SOUND_GOOD_NIGHT),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowSoundRedeems &&
					Settings.ModSettings.AllowSoundGoodNight,
					RedeemNames.SOUND_GOOD_NIGHT);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.SOUND_HYDRATE),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowSoundRedeems &&
					Settings.ModSettings.AllowSoundHydrate,
					RedeemNames.SOUND_HYDRATE);
			}
			catch (InvalidTokenException)
			{
				return;
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
			}

			SyncRequired = false;
		}

		private static async Task UpdateCustomReward(string rewardId, bool isEnabled, string rewardName)
		{
			var userId = AuthService.User?.Id ?? throw new NotLoggedInException();

			try
			{
				await TwitchAdapter.UpdateCustomReward(AuthService.ClientId, Settings.Token.Access, userId, rewardId, isEnabled);
				Melon<Mod>.Logger.Msg($"updated enabled status of {rewardName} to {isEnabled}");
			}
			catch (InvalidTokenException)
			{
				await AuthService.RefreshToken();
				throw;
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
			}
		}

	}
}
