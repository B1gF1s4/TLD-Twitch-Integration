using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class RedeemDefaults
	{

		private static readonly Dictionary<string, CustomReward> _defaults = new()
		{
			{ RedeemNames.WEATHER_BLIZZARD, WeatherRedeems.Blizzard },
			{ RedeemNames.WEATHER_CLEAR, WeatherRedeems.Clear },
			{ RedeemNames.WEATHER_LIGHT_FOG, WeatherRedeems.LightFog },
			{ RedeemNames.WEATHER_DENSE_FOG, WeatherRedeems.DenseFog },
			{ RedeemNames.WEATHER_PARTLY_CLOUDY, WeatherRedeems.PartlyCloudy },
			{ RedeemNames.WEATHER_CLOUDY, WeatherRedeems.Cloudy },
			{ RedeemNames.WEATHER_LIGHT_SNOW, WeatherRedeems.LightSnow },
			{ RedeemNames.WEATHER_HEAVY_SNOW, WeatherRedeems.HeavySnow },

			{ RedeemNames.SOUND_HELLO, SoundRedeems.Hello },
			{ RedeemNames.SOUND_420, SoundRedeems.Happy420 },
			{ RedeemNames.SOUND_GOOD_NIGHT, SoundRedeems.GoodNight },
			{ RedeemNames.SOUND_HYDRATE, SoundRedeems.Hydrate },
		};

		public static CustomReward GetRedeemDefault(string redeemName)
		{
			var redeem = _defaults[redeemName] ??
				throw new NotFoundException();

			return redeem;
		}

	}
}
