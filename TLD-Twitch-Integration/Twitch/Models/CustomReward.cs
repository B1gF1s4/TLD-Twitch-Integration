using System.Text.Json.Serialization;

namespace TLD_Twitch_Integration.Twitch.Models
{
	public class CustomReward
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("is_enabled")]
		public bool IsEnabled { get; set; }

		[JsonPropertyName("background_color")]
		public string? Color { get; set; }

		[JsonPropertyName("cost")]
		public int? Cost { get; set; }

		[JsonPropertyName("prompt")]
		public string? Prompt { get; set; }

		[JsonPropertyName("is_user_input_required")]
		public bool IsUserInputRequired { get; set; }

		[JsonPropertyName("is_global_cooldown_enabled")]
		public bool IsCooldownEnabled { get; set; }

		[JsonPropertyName("global_cooldown_seconds")]
		public int CooldownInSeconds { get; set; }

	}
}
