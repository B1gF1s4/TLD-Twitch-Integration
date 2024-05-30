using Il2Cpp;
using MelonLoader;
using System.Reflection;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration
{
	public class RedemptionService
	{

		public static List<Redemption> OpenRedeems { get; set; } = new();

		public static bool HasOpenRedeems { get; set; }

		private const int _interval = 5;

		private static DateTime _lastUpdated;
		private static bool _clearingOpenRedeems = false;

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
				GameManager.m_ActiveScene == "Boot" ||
				GameManager.m_ActiveScene == "Empty")
				return;

			if (GameManager.m_ActiveScene == "MainMenu")
			{
				await ClearOpenRedeems();
				return;
			}

			if (GameManager.m_IsPaused)
				return;

			if (!CustomRewardsService.IsInitialized)
				return;

			await CheckForOpenRedeems();
		}

		private static async Task ClearOpenRedeems()
		{
			var userId = AuthService.User?.Id ??
				throw new NotLoggedInException();

			if (_clearingOpenRedeems)
				return;

			HasOpenRedeems = false;
			_clearingOpenRedeems = true;

			var tasks = new List<Task>();
			var hasErrors = false;

			foreach (var redeem in OpenRedeems)
			{
				if (AuthService.Status == AuthService.ConnectionStatus.Refreshing)
					continue;

				var t = TwitchAdapter.CancelRedemption(
						AuthService.ClientId, Settings.Token.Access, userId, redeem);
				tasks.Add(t);
			}

			try
			{
				await Task.WhenAll(tasks);
			}
			catch (InvalidTokenException)
			{
				HasOpenRedeems = true;
				hasErrors = true;
				await AuthService.RefreshToken();
			}
			catch (RedeemAlreadyProcessedException)
			{
				Melon<Mod>.Logger.Msg($"one of the redeems was completed or rejected manually. " +
					$"removing it from TTI processing list.");
			}
			catch (Exception ex)
			{
				HasOpenRedeems = true;
				hasErrors = true;
				Melon<Mod>.Logger.Error(ex);
			}

			if (!hasErrors)
				OpenRedeems.RemoveAll(all => true);

			_clearingOpenRedeems = false;
		}

		private static async Task CheckForOpenRedeems()
		{
			var userId = AuthService.User?.Id ??
				throw new NotLoggedInException();

			var redeemIdFields = Settings.Redeems.GetType()
				.GetFields(BindingFlags.Instance | BindingFlags.Public);

			foreach (var redeemIdField in redeemIdFields)
			{
				var redeemName = Settings.Redeems.GetRedeemNameByFieldName(redeemIdField.Name);
				var redeemId = redeemIdField.GetValue(Settings.Redeems);

				if (redeemId == null || string.IsNullOrEmpty(redeemId.ToString()))
					continue;

				try
				{
					var list = await TwitchAdapter.GetUnfulfilledRedemptions(
						AuthService.ClientId, Settings.Token.Access, userId, redeemId.ToString()!, redeemName);

					if (list.Count > 0)
					{
						OpenRedeems.AddRange(list);
						HasOpenRedeems = true;
					}
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
	}
}
