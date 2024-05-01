using System.Text.Json.Serialization;

namespace TLD_Twitch_Integration.Twitch.Models
{
	public class DeviceCode
	{
		[JsonPropertyName("device_code")]
		public string? Code { get; set; }

		[JsonPropertyName("verification_uri")]
		public string? VerificationUri { get; set; }

		[JsonPropertyName("user_code")]
		public string? UserCode { get; set; }

		[JsonPropertyName("expires_in")]
		public int? ExpiresIn { get; set; }
	}
}
