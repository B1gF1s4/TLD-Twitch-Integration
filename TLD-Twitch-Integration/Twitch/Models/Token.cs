using System.Text.Json.Serialization;

namespace TLD_Twitch_Integration.Twitch.Models
{
	public class Token
	{
		[JsonPropertyName("access_token")]
		public string? Value { get; set; }

		[JsonPropertyName("refresh_token")]
		public string? Refresh { get; set; }

		[JsonPropertyName("expires_in")]
		public int? ExpiresIn { get; set; }

		[JsonPropertyName("token_type")]
		public string? TokenType { get; set; }
	}
}
