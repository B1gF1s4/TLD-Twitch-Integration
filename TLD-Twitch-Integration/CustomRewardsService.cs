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

		public static bool SyncRequired { get; set; } = true;

		private const int _interval = 3;

		private const int _intervalCheckForDeleted = 30;

		private static DateTime _lastUpdated;

		private static DateTime _lastCheckedForDeleted;

		private static bool _isSyncing = false;

		public static async Task OnUpdate()
		{
			if (!Mod.ShouldUpdate(_lastUpdated, _interval))
				return;

			_lastUpdated = DateTime.UtcNow;

			if (!AuthService.IsConnected)
				return;

			if (_isSyncing)
				return;

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

		private static async Task CreateCustomRewards()
		{
			_isSyncing = true;

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

			_isSyncing = false;
		}

		private static async Task CheckForDeletedCustomRewards()
		{
			var lastCheckedDelta = (DateTime.UtcNow - _lastCheckedForDeleted).TotalSeconds;
			if (lastCheckedDelta < _intervalCheckForDeleted)
			{
				Melon<Mod>.Logger.Msg($"skipping check for deleted redeems. {lastCheckedDelta}s since last check");
				return;
			}

			Melon<Mod>.Logger.Msg("checking for deleted redeems to recreate..");

			_lastCheckedForDeleted = DateTime.UtcNow;

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
				SyncRequired = true;
			}
		}

		private static async Task SyncCustomRewardsWithSettings()
		{
			Melon<Mod>.Logger.Msg($"syncing twitch redeems with game settings ..");

			_isSyncing = true;

			try
			{
				// TODO: refactor to use reflection, possibly custom attribute and iterate settings, or redeem defaults
				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.ANIMAL_T_WOLVES),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowTWolves,
					RedeemNames.ANIMAL_T_WOLVES);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.ANIMAL_BIG_GAME),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowBigGame,
					RedeemNames.ANIMAL_BIG_GAME);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.ANIMAL_STALKING_WOLF),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowStalkingWolf,
					RedeemNames.ANIMAL_STALKING_WOLF);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.ANIMAL_BUNNY_EXPLOSION),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowBunnyExplosion,
					RedeemNames.ANIMAL_BUNNY_EXPLOSION);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_HELP),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherHelp,
					RedeemNames.WEATHER_HELP);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_HARM),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherHarm,
					RedeemNames.WEATHER_HARM);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_AURORA),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeatherAurora,
					RedeemNames.WEATHER_AURORA);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.WEATHER_TIME),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowTime,
					RedeemNames.WEATHER_TIME);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_HELP),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowStatusHelp,
					RedeemNames.STATUS_HELP);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_HARM),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowStatusHarm,
					RedeemNames.STATUS_HARM);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_AFFLICTION),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowAfflictions,
					RedeemNames.STATUS_AFFLICTION);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_AFFLICTION_CURE),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowAfflictionCure &&
					Settings.ModSettings.AllowAfflictions,
					RedeemNames.STATUS_AFFLICTION_CURE);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_BLEED),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowStatusBleeding,
					RedeemNames.STATUS_BLEED);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_SPRAIN),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowStatusSprain,
					RedeemNames.STATUS_SPRAIN);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_FROSTBITE),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowStatusFrostbite,
					RedeemNames.STATUS_FROSTBITE);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.STATUS_STINK),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowStatusStink,
					RedeemNames.STATUS_STINK);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.INVENTORY_NO_PANTS),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowTeamNoPants,
					RedeemNames.INVENTORY_NO_PANTS);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.INVENTORY_DROP_TORCH),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowDropTorch,
					RedeemNames.INVENTORY_DROP_TORCH);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.INVENTORY_DROP_ITEM),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowDropItem,
					RedeemNames.INVENTORY_DROP_ITEM);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.INVENTORY_STEPPED_STIM),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowSteppedStim,
					RedeemNames.INVENTORY_STEPPED_STIM);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.INVENTORY_WEAPON),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowWeapon,
					RedeemNames.INVENTORY_WEAPON);

				//await UpdateCustomReward(
				//	Settings.Redeems.GetIdByRedeemName(RedeemNames.INVENTORY_BANDAGE),
				//	Settings.ModSettings.Enabled &&
				//	Settings.ModSettings.AllowBandage,
				//	RedeemNames.INVENTORY_BANDAGE);

				await UpdateCustomReward(
					Settings.Redeems.GetIdByRedeemName(RedeemNames.MISC_420),
					Settings.ModSettings.Enabled &&
					Settings.ModSettings.AllowMisc420,
					RedeemNames.MISC_420);

				//await UpdateCustomReward(
				//	Settings.Redeems.GetIdByRedeemName(RedeemNames.MISC_FART),
				//	Settings.ModSettings.Enabled &&
				//	Settings.ModSettings.AllowMiscFart,
				//	RedeemNames.MISC_FART);

				//await UpdateCustomReward(
				//	Settings.Redeems.GetIdByRedeemName(RedeemNames.DEV_SOUND),
				//	Settings.ModSettings.Enabled &&
				//	Settings.ModSettings.AllowDevSoundCheck,
				//	RedeemNames.DEV_SOUND);
			}
			catch (InvalidTokenException)
			{
				return;
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
			}
			finally
			{
				SyncRequired = false;
				_isSyncing = false;
			}
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
