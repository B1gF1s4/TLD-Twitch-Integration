﻿using ModSettings;
using System.Reflection;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Redeems;

namespace TLD_Twitch_Integration
{
	public class CustomRewardIdMapping : JsonFile
	{
		[Name(RedeemNames.WEATHER_BLIZZARD)]
		public string WeatherBlizzard = "";

		[Name(RedeemNames.WEATHER_CLEAR)]
		public string WeatherClear = "";

		[Name(RedeemNames.WEATHER_LIGHT_FOG)]
		public string WeatherLightFog = "";

		[Name(RedeemNames.WEATHER_DENSE_FOG)]
		public string WeatherDenseFog = "";

		[Name(RedeemNames.WEATHER_PARTLY_CLOUDY)]
		public string WeatherPartlyCloudy = "";

		[Name(RedeemNames.WEATHER_CLOUDY)]
		public string WeatherCloudy = "";

		[Name(RedeemNames.WEATHER_LIGHT_SNOW)]
		public string WeatherLightSnow = "";

		[Name(RedeemNames.WEATHER_HEAVY_SNOW)]
		public string WeatherHeavySnow = "";

		[Name(RedeemNames.SOUND_HELLO)]
		public string SoundHello = "";

		[Name(RedeemNames.SOUND_420)]
		public string Sound420 = "";

		[Name(RedeemNames.SOUND_GOOD_NIGHT)]
		public string SoundGoodNight = "";

		[Name(RedeemNames.SOUND_HYDRATE)]
		public string SoundHydrate = "";

		public CustomRewardIdMapping() : base(Path.Combine(Mod.BaseDirectory, "redeems"))
		{ }

		public string GetRedeemNameByFieldName(string fieldName)
		{
			var field = (GetType()?.GetField(fieldName)) ??
				throw new ArgumentException($"cannot find field {fieldName} on {this}");

			var attr = field.GetCustomAttribute(typeof(NameAttribute)) ??
				throw new NotFoundException();

			var value = ((NameAttribute)attr).Name ??
				throw new NotFoundException();

			return value.ToString();
		}

		public string GetRedeemNameById(string id)
		{
			var fields = GetType()?.GetFields(BindingFlags.Instance | BindingFlags.Public);
			if (fields == null || fields.Length <= 0)
				throw new NotFoundException();

			foreach (var field in fields)
			{
				var value = field.GetValue(this);
				if (value == null || string.IsNullOrEmpty(value.ToString()))
					continue;

				if (value.ToString()!.Equals(id))
				{
					var attr = field.GetCustomAttribute(typeof(NameAttribute)) ??
						throw new NotFoundException();

					var redeemName = ((NameAttribute)attr).Name ??
						throw new NotFoundException();

					return redeemName.ToString();
				}
			}

			throw new NotFoundException();
		}

		public string GetIdByRedeemName(string redeemName)
		{
			var fields = GetType()?.GetFields(BindingFlags.Instance | BindingFlags.Public);
			if (fields == null || fields.Length <= 0)
				throw new NotFoundException();

			foreach (var field in fields)
			{
				var attributes = field.GetCustomAttributes(typeof(NameAttribute));
				if (attributes == null || !attributes.Any())
					throw new NotFoundException();

				var attributeValue = ((NameAttribute)attributes.FirstOrDefault()!).Name;

				if (attributeValue.Equals(redeemName))
					return field.GetValue(this)?.ToString()!;
			}

			throw new NotFoundException();
		}
	}
}
