using System.Text.Json.Serialization;

namespace TLD_Twitch_Integration.Twitch.Models
{
	public class Redemption
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("status")]
		public string? Status { get; set; }

		[JsonPropertyName("reward")]
		public CustomReward? CustomReward { get; set; }

		[JsonPropertyName("redeemed_at")]
		public DateTime? RedeemedAt { get; set; }

		[JsonPropertyName("user_name")]
		public string? UserName { get; set; }
	}
}
