using System.Text.Json.Serialization;

namespace TLD_Twitch_Integration.Twitch.Models
{
	public class User
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("login")]
		public string? Login { get; set; }

		[JsonPropertyName("display_name")]
		public string? DisplayName { get; set; }

		[JsonPropertyName("broadcaster_type")]
		public string? BroadcasterType { get; set; }
	}
}
