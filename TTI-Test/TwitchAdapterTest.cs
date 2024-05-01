using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch;

namespace TTI_Test
{
	[TestClass]
	public class TwitchAdapterTest
	{

		private const string ClientId = "";
		private const string ClientSecret = "";
		private const string RefreshToken = "";

		[TestMethod]
		public async Task CanGetDeviceCode()
		{
			var deviceCode = await TwitchAdapter.GetDeviceCode(ClientId);
			Assert.IsNotNull(deviceCode);
		}

		[TestMethod]
		public async Task GetDeviceCodeThrowsMissingClientIdException()
		{
			try
			{
				var deviceCode = await TwitchAdapter.GetDeviceCode("");
				Assert.Fail("exception should have been thrown");
			}
			catch (MissingClientIdException ex)
			{
				Assert.IsTrue(ex.Message.Contains("missing"));
			}
			catch (Exception e)
			{
				Assert.Fail($"unexpected exception of type {e.GetType()} caught: {e.Message}");
			}
		}

		[TestMethod]
		public async Task GetDeviceCodeThrowsInvalidClientIdException()
		{
			try
			{
				var deviceCode = await TwitchAdapter.GetDeviceCode(ClientId[..^5]);
				Assert.Fail("exception should have been thrown");
			}
			catch (InvalidClientIdException ex)
			{
				Assert.IsTrue(ex.Message.Contains("invalid"));
			}
			catch (Exception e)
			{
				Assert.Fail($"unexpected exception of type {e.GetType()} caught: {e.Message}");
			}
		}

		[TestMethod]
		public async Task GetAccessTokenThrowsAuthPendingException()
		{
			var deviceCode = await TwitchAdapter.GetDeviceCode(ClientId);
			Assert.IsNotNull(deviceCode);

			try
			{
				var accessToken = await TwitchAdapter.GetAccessToken(ClientId, deviceCode);
				Assert.Fail("exception should have been thrown");
			}
			catch (AuthorizationPendingException ex)
			{
				Assert.IsTrue(ex.Message.Contains("authorization pending"));
			}
			catch (Exception e)
			{
				Assert.Fail($"unexpected exception of type {e.GetType()} caught: {e.Message}");
			}
		}

		[TestMethod]
		public async Task CanRefreshToken()
		{
			var token = await TwitchAdapter.RefreshToken(ClientId, ClientSecret, RefreshToken);
			Assert.IsNotNull(token);
		}


	}
}