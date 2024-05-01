using System.Text.Json.Serialization;

namespace TLD_Twitch_Integration.Twitch.Models
{
	public class ListResponse<T>
	{
		[JsonPropertyName("data")]
		public List<T>? Data { get; set; }

	}
}
