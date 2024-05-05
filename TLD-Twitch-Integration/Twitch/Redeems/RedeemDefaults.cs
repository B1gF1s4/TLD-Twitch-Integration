using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class RedeemDefaults
	{

		private static readonly Dictionary<string, CustomReward> _defaults = new()
		{
			{ RedeemNames.ANIMAL_T_WOLVES, AnimalRedeems.TWolves },
			{ RedeemNames.ANIMAL_BEAR, AnimalRedeems.Bear },
			{ RedeemNames.ANIMAL_MOOSE, AnimalRedeems.Moose },
			{ RedeemNames.ANIMAL_STALKING_WOLF, AnimalRedeems.StalkingWolf },
			{ RedeemNames.ANIMAL_BUNNY_EXPLOSION, AnimalRedeems.BunnyExplosion },

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

			{ RedeemNames.SOUND, SoundRedeems.DevSound },

			{ RedeemNames.STATUS_HUNGRY, StatusRedeems.Hungry },
			{ RedeemNames.STATUS_THIRSTY, StatusRedeems.Thirsty },
			{ RedeemNames.STATUS_TIRED, StatusRedeems.Tired },
			{ RedeemNames.STATUS_FREEZING, StatusRedeems.Freezing },

			{ RedeemNames.STATUS_FULL, StatusRedeems.Full },
			{ RedeemNames.STATUS_NOT_THIRSTY, StatusRedeems.NotThirsty },
			{ RedeemNames.STATUS_AWAKE, StatusRedeems.Awake },
			{ RedeemNames.STATUS_WARM, StatusRedeems.Warm },

			{ RedeemNames.AFFLICTION_CABIN_FEVER, AfflictionRedeems.CabinFever },
			{ RedeemNames.AFFLICTION_DYSENTERY, AfflictionRedeems.Dysentery },
			{ RedeemNames.AFFLICTION_FOOD_POISONING, AfflictionRedeems.FoodPoisoning },
			{ RedeemNames.AFFLICTION_HYPOTHERMIA, AfflictionRedeems.Hypothermia },
			{ RedeemNames.AFFLICTION_PARASITES, AfflictionRedeems.Parasites },
		};

		public static CustomReward GetRedeemDefault(string redeemName)
		{
			var redeem = _defaults[redeemName] ??
				throw new NotFoundException();

			return redeem;
		}

	}
}
