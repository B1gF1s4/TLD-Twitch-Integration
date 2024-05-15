using MelonLoader;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration
{
	public static class AuthService
	{
		public enum ConnectionStatus
		{
			NotConnected,
			AuthorizationPending,
			Refreshing,
			Connected
		}

		public static string ClientId { get; set; } = "kgpiz5p809sncyzuh770wikis8t9no";
		public static string ClientSecret { get; set; } = "40v7bdedi0vss1mr9r2y3k3y0fqoz9";

		public static ConnectionStatus Status { get; private set; }

		public static User? User { get; private set; }

		public static bool IsConnected
		{
			get
			{
				return Status == ConnectionStatus.Connected;
			}
		}


		private const int _interval = 10;

		private static DateTime _lastUpdated;
		private static DeviceCode? _deviceCode;

		public static async Task OnLoad()
		{
			Status = ConnectionStatus.NotConnected;

			if (!string.IsNullOrEmpty(Settings.Token.Access))
			{
				Status = ConnectionStatus.Connected;
				await SetUser();
				Melon<Mod>.Logger.Msg("logged in");
				return;
			}

			await Authenticate();
		}

		public static async Task OnUpdate()
		{
			if (!Mod.ShouldUpdate(_lastUpdated, _interval))
				return;

			_lastUpdated = DateTime.UtcNow;

			if (Status == ConnectionStatus.AuthorizationPending)
				await SetToken();

			if (Status == ConnectionStatus.NotConnected)
			{
				Melon<Mod>.Logger.Msg("not connected");
				await Authenticate();
			}
		}

		public static async Task RefreshToken()
		{
			Status = ConnectionStatus.Refreshing;

			if (string.IsNullOrEmpty(Settings.Token.Refresh))
			{
				await Authenticate();
				return;
			}

			try
			{
				var token = await TwitchAdapter.RefreshToken(ClientId, ClientSecret,
					Settings.Token.Refresh);
				Settings.Token.Access = token.Value!;
				Settings.Token.Refresh = token.Refresh!;
				Settings.Token.Save();

				await SetUser();
				Status = ConnectionStatus.Connected;
			}
			catch (InvalidTokenException exInvalidToken)
			{
				Melon<Mod>.Logger.Msg(exInvalidToken.Message);
				Status = ConnectionStatus.NotConnected;
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
				Status = ConnectionStatus.NotConnected;
			}
		}

		private static async Task Authenticate()
		{
			try
			{
				_deviceCode = await TwitchAdapter.GetDeviceCode(ClientId);
				var ex = new AuthorizationPendingException(_deviceCode.VerificationUri!);
				Melon<Mod>.Logger.Msg(ex.Message);
				Status = ConnectionStatus.AuthorizationPending;
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
				Status = ConnectionStatus.NotConnected;
			}
		}

		private static async Task SetToken()
		{
			if (_deviceCode == null)
				return;

			try
			{
				var token = await TwitchAdapter.GetAccessToken(ClientId, _deviceCode);
				Settings.Token.Access = token.Value!;
				Settings.Token.Refresh = token.Refresh!;
				Settings.Token.Save();

				await SetUser();
				Status = ConnectionStatus.Connected;

				Melon<Mod>.Logger.Msg("logged in");
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Msg(ex.Message);
			}
		}

		private static async Task SetUser()
		{
			try
			{
				var user = await TwitchAdapter.GetUserInfo(ClientId, Settings.Token.Access);

				if (string.IsNullOrEmpty(user.BroadcasterType))
					throw new UserNotAffiliateException(user.Login!);

				User = user;
			}
			catch (InvalidTokenException)
			{
				await RefreshToken();
				await SetUser();
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
				Mod.ShouldKill = true;
			}
		}
	}
}
