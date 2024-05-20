using Il2Cpp;
using MelonLoader;
using TLD_Twitch_Integration.Commands;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch;
using TLD_Twitch_Integration.Twitch.Models;
using TLD_Twitch_Integration.Twitch.Redeems;

namespace TLD_Twitch_Integration
{
	public class ExecutionService
	{
		public static bool ExecutionPending { get; set; }
		public static List<Redemption> ExecutionQueue { get; set; } = new();


		private const int _interval = 8;

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

			if (Utils.IsSceneTransition())
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
			GameService.Update();

			var defaultTitle = Settings.Redeems.GetRedeemNameById(redeem.CustomReward?.Id!);
			Melon<Mod>.Logger.Msg($"trying to execute redeem {defaultTitle}");

			var redeemMessage = "";
			try
			{
				redeemMessage = ExecuteRedeem(redeem, defaultTitle);
			}
			catch (RequiresRedeemRefundException refund)
			{
				await UpdateRedemption(userId, redeem, false);

				Melon<Mod>.Logger.Msg($"'{redeem.CustomReward?.Title}' redeemed by {redeem.UserName} refunded. " +
					$"- {refund.Message}");

				if (Settings.ModSettings.ShowAlert)
					HUDMessage.AddMessage($"{redeem.UserName}'s Redeem refunded. -> {refund.Message}",
						_interval - 1, true, true);

				return;
			}

			if (string.IsNullOrEmpty(redeemMessage))
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
					HUDMessage.AddMessage(redeemMessage, _interval - 1, true, true);

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

		private static string ExecuteRedeem(Redemption redeem, string defaultTitle)
		{
			switch (defaultTitle)
			{
				case RedeemNames.ANIMAL_T_WOLVES:
					return CommandDefaults.CmdAnimalTWolves.Execute(redeem);

				case RedeemNames.ANIMAL_BIG_GAME:
					return CommandDefaults.CmdAnimalBigGame.Execute(redeem);

				case RedeemNames.ANIMAL_STALKING_WOLF:
					return CommandDefaults.CmdAnimalStalkingWolf.Execute(redeem);

				case RedeemNames.ANIMAL_BUNNY_EXPLOSION:
					return CommandDefaults.CmdAnimalBunnyExplosion.Execute(redeem);

				case RedeemNames.WEATHER_HELP:
					return CommandDefaults.CmdWeatherHelp.Execute(redeem);

				case RedeemNames.WEATHER_HARM:
					return CommandDefaults.CmdWeatherHarm.Execute(redeem);

				case RedeemNames.WEATHER_TIME:
					return CommandDefaults.CmdWeatherTime.Execute(redeem);

				case RedeemNames.WEATHER_AURORA:
					return CommandDefaults.CmdWeatherAurora.Execute(redeem);

				case RedeemNames.STATUS_HELP:
					return CommandDefaults.CmdStatusHelp.Execute(redeem);

				case RedeemNames.STATUS_HARM:
					return CommandDefaults.CmdStatusHarm.Execute(redeem);

				case RedeemNames.STATUS_AFFLICTION:
					return CommandDefaults.CmdStatusAffliction.Execute(redeem);

				case RedeemNames.STATUS_AFFLICTION_CURE:
					return CommandDefaults.CmdStatusAfflictionCure.Execute(redeem);

				case RedeemNames.STATUS_BLEED:
					return CommandDefaults.CmdStatusBleeding.Execute(redeem);

				case RedeemNames.STATUS_SPRAIN:
					return CommandDefaults.CmdStatusSprain.Execute(redeem);

				case RedeemNames.STATUS_FROSTBITE:
					return CommandDefaults.CmdStatusFrostbite.Execute(redeem);

				case RedeemNames.STATUS_STINK:
					return CommandDefaults.CmdStatusStink.Execute(redeem);

				case RedeemNames.INVENTORY_NO_PANTS:
					return CommandDefaults.CmdInventoryNoPants.Execute(redeem);

				case RedeemNames.INVENTORY_DROP_TORCH:
					return CommandDefaults.CmdInventoryDropTorch.Execute(redeem);

				case RedeemNames.INVENTORY_WEAPON:
					return CommandDefaults.CmdInventoryWeapon.Execute(redeem);

				case RedeemNames.INVENTORY_STEPPED_STIM:
					return CommandDefaults.CmdInventoryStim.Execute(redeem);

				case RedeemNames.INVENTORY_DROP_ITEM:
					return CommandDefaults.CmdInventoryDropItem.Execute(redeem);

				case RedeemNames.INVENTORY_BANDAGE:
					return CommandDefaults.CmdInventoryBandage.Execute(redeem);

				case RedeemNames.MISC_420:
					return CommandDefaults.CmdMisc420.Execute(redeem);

				//case RedeemNames.MISC_FART:
				//	return CommandDefaults.CmdMiscFart.Execute(redeem);

				case RedeemNames.DEV_SOUND:
					return CommandDefaults.CmdDevSoundCheck.Execute(redeem);

				default:
					Melon<Mod>.Logger.Error($"redeem operation not supported - {defaultTitle}");
					throw new RequiresRedeemRefundException("Redeem not found.");
			}
		}
	}
}
