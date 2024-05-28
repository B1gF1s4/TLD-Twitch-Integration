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

			HasOpenRedeems = false;

			var tasks = new List<Task>();
			var hasErrors = false;

			foreach (var redeem in OpenRedeems)
			{
				if (AuthService.Status == AuthService.ConnectionStatus.Refreshing)
					continue;

				try
				{
					var t = TwitchAdapter.FulfillRedemption(
						AuthService.ClientId, Settings.Token.Access, userId, redeem);
					tasks.Add(t);
				}
				catch (InvalidTokenException)
				{
					HasOpenRedeems = true;
					hasErrors = true;
					var t = AuthService.RefreshToken();
					tasks.Add(t);
				}
				catch (Exception ex)
				{
					HasOpenRedeems = true;
					hasErrors = true;
					Melon<Mod>.Logger.Error(ex);
				}
			}

			await Task.WhenAll(tasks);

			if (!hasErrors)
				OpenRedeems.RemoveAll(all => true);
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
