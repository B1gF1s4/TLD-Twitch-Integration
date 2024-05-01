using System.Net.Http.Headers;
using System.Text.Json;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch
{
	public static class TwitchAdapter
	{

		private const string AuthScope = "channel:manage:redemptions";
		private const string GrantType = "urn:ietf:params:oauth:grant-type:device_code";

		private static HttpClient _client = null!;

		public static async Task<DeviceCode> GetDeviceCode(string clientId)
		{
			var httpClient = GetHttpClient();

			var request = new HttpRequestMessage(HttpMethod.Post, "https://id.twitch.tv/oauth2/device")
			{
				Content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("client_id", clientId),
					new KeyValuePair<string, string>("scopes", AuthScope)
				})
			};

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseContent.Contains("missing"))
						throw new MissingClientIdException();

					if (responseContent.Contains("invalid"))
						throw new InvalidClientIdException(clientId);
				}

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var deviceCode = JsonSerializer.Deserialize<DeviceCode>(responseContent) ??
				throw new DeserializationException(nameof(DeviceCode), responseContent);

			return deviceCode;
		}

		public static async Task<Token> GetAccessToken(string clientId, DeviceCode deviceCode)
		{
			var httpClient = GetHttpClient();

			var request = new HttpRequestMessage(HttpMethod.Post, "https://id.twitch.tv/oauth2/token")
			{
				Content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("client_id", clientId),
					new KeyValuePair<string, string>("scope", AuthScope),
					new KeyValuePair<string, string>("device_code", deviceCode.Code!),
					new KeyValuePair<string, string>("grant_type", GrantType),
				})
			};

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseContent.Contains("pending"))
						throw new AuthorizationPendingException(deviceCode.VerificationUri!);
				}

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var token = JsonSerializer.Deserialize<Token>(responseContent) ??
				throw new DeserializationException(nameof(Token), responseContent);

			return token;
		}

		public static async Task<Token> RefreshToken(string clientId, string clientSecret, string refreshToken)
		{
			var httpClient = GetHttpClient();

			var request = new HttpRequestMessage(HttpMethod.Post, "https://id.twitch.tv/oauth2/token")
			{
				Content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("client_id", clientId),
					new KeyValuePair<string, string>("client_secret", clientSecret),
					new KeyValuePair<string, string>("refresh_token", refreshToken),
					new KeyValuePair<string, string>("grant_type", "refresh_token"),
				})
			};

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new InvalidTokenException();

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var token = JsonSerializer.Deserialize<Token>(responseContent) ??
				throw new DeserializationException(nameof(Token), responseContent);

			return token;
		}

		public static async Task<User> GetUserInfo(string clientId, string accessToken)
		{
			var httpClient = GetHttpClient();

			var request = new HttpRequestMessage(HttpMethod.Get, "https://api.twitch.tv/helix/users");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			request.Headers.Add("Client-Id", clientId);

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new InvalidTokenException();

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var user = JsonSerializer.Deserialize<ListResponse<User>>(responseContent) ??
				throw new DeserializationException(nameof(ListResponse<User>), responseContent);

			if (user.Data == null || user.Data.Count <= 0)
				throw new NotFoundException();

			if (user.Data.Count > 1)
				throw new NotUniqueException();

			return user.Data[0];
		}

		public static async Task<CustomReward> CreateCustomReward(string clientId, string accessToken,
			string broadcasterId, CustomReward reward)
		{
			var httpClient = GetHttpClient();

			var baseUrl = "https://api.twitch.tv/helix/channel_points/custom_rewards";
			var url = $"{baseUrl}?broadcaster_id={broadcasterId}";

			var request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("title", reward.Title!),
					new KeyValuePair<string, string>("cost", reward.Cost.ToString()!),
					new KeyValuePair<string, string>("prompt", reward.Prompt!),
					new KeyValuePair<string, string>("is_enabled", reward.IsEnabled.ToString()),
					new KeyValuePair<string, string>("background_color", reward.Color!),
				})
			};

			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			request.Headers.Add("Client-Id", clientId);

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new InvalidTokenException();

				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseContent.Contains("CREATE_CUSTOM_REWARD_DUPLICATE_REWARD"))
						throw new CustomRewardAlreadyExistsException(reward.Title!);
				}

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var customReward = JsonSerializer.Deserialize<ListResponse<CustomReward>>(responseContent) ??
				throw new DeserializationException(nameof(ListResponse<CustomReward>), responseContent);

			if (customReward.Data == null || customReward.Data.Count <= 0)
				throw new NotFoundException();

			if (customReward.Data.Count > 1)
				throw new NotUniqueException();

			return customReward.Data[0];
		}

		public static async Task<List<CustomReward>> GetAvailableCustomRewards(string clientId, string accessToken,
			string broadcasterId)
		{
			var httpClient = GetHttpClient();

			var baseUrl = "https://api.twitch.tv/helix/channel_points/custom_rewards";
			var url = $"{baseUrl}?broadcaster_id={broadcasterId}";

			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			request.Headers.Add("Client-Id", clientId);

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new InvalidTokenException();

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var customRewards = JsonSerializer.Deserialize<ListResponse<CustomReward>>(responseContent) ??
				throw new DeserializationException(nameof(ListResponse<CustomReward>), responseContent);

			if (customRewards.Data == null || customRewards.Data.Count <= 0)
				throw new NotFoundException();

			return customRewards.Data.ToList();
		}


		public static async Task<CustomReward> UpdateCustomReward(string clientId, string accessToken,
			string broadcasterId, string rewardId, bool isEnabled)
		{
			var httpClient = GetHttpClient();

			var baseUrl = "https://api.twitch.tv/helix/channel_points/custom_rewards";
			var url = $"{baseUrl}?broadcaster_id={broadcasterId}&id={rewardId}";

			var request = new HttpRequestMessage(HttpMethod.Patch, url)
			{
				Content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("is_enabled", isEnabled.ToString()),
				})
			};
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			request.Headers.Add("Client-Id", clientId);

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new InvalidTokenException();

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var customRewards = JsonSerializer.Deserialize<ListResponse<CustomReward>>(responseContent) ??
				throw new DeserializationException(nameof(ListResponse<CustomReward>), responseContent);

			if (customRewards.Data == null || customRewards.Data.Count <= 0)
				throw new NotFoundException();

			if (customRewards.Data.Count > 1)
				throw new NotUniqueException();

			return customRewards.Data[0];
		}

		public static async Task<List<Redemption>> GetUnfulfilledRedemptions(string clientId, string accessToken,
			string broadcasterId, string customRewardId, string customRewardName)
		{
			var httpClient = GetHttpClient();

			var baseUrl = "https://api.twitch.tv/helix/channel_points/custom_rewards/redemptions";
			var url = $"{baseUrl}?broadcaster_id={broadcasterId}&reward_id={customRewardId}&status=UNFULFILLED";

			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			request.Headers.Add("Client-Id", clientId);

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					if (responseContent.Contains("manage"))
						throw new CustomRewardNotManageableException(customRewardName);
				}

				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}

			var redemptions = JsonSerializer.Deserialize<ListResponse<Redemption>>(responseContent) ??
				throw new DeserializationException(nameof(ListResponse<Redemption>), responseContent);

			return redemptions.Data?.ToList()!;
		}

		public static async Task FulfillRedemption(string clientId, string accessToken, string broadcasterId, Redemption redemption)
		{
			var httpClient = GetHttpClient();

			var baseUrl = "https://api.twitch.tv/helix/channel_points/custom_rewards/redemptions";
			var url = $"{baseUrl}?id={redemption.Id}&broadcaster_id={broadcasterId}&reward_id={redemption.CustomReward?.Id}";

			var request = new HttpRequestMessage(HttpMethod.Patch, url)
			{
				Content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("status", "FULFILLED")
				})
			};

			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			request.Headers.Add("Client-Id", clientId);

			var response = await httpClient.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new UnhandledErrorResponseException(response.StatusCode, responseContent);
			}
		}

		private static HttpClient GetHttpClient()
		{
			if (_client != null)
				return _client;

			_client = new HttpClient();
			return _client;
		}
	}
}
