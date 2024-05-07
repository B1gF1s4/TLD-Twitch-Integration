using ModSettings;
using System.Reflection;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Redeems;

namespace TLD_Twitch_Integration
{
	public class CustomRewardIdMapping : JsonFile
	{
		[Name(RedeemNames.WEATHER_HELP)]
		public string WeatherHelp = "";

		[Name(RedeemNames.WEATHER_HARM)]
		public string WeatherHarm = "";

		[Name(RedeemNames.WEATHER_AURORA)]
		public string WeatherAurora = "";

		[Name(RedeemNames.ANIMAL_T_WOLVES)]
		public string AnimalTWolves = "";

		[Name(RedeemNames.ANIMAL_BIG_GAME)]
		public string AnimalBigGame = "";

		[Name(RedeemNames.ANIMAL_STALKING_WOLF)]
		public string AnimalStalkingWolf = "";

		[Name(RedeemNames.ANIMAL_BUNNY_EXPLOSION)]
		public string AnimalBunnyExplosion = "";

		[Name(RedeemNames.STATUS_HELP)]
		public string StatusHelp = "";

		[Name(RedeemNames.STATUS_HARM)]
		public string StatusHarm = "";

		[Name(RedeemNames.STATUS_AFFLICTION)]
		public string StatusAffliction = "";

		[Name(RedeemNames.STATUS_BLEED)]
		public string StatusBleeding = "";

		[Name(RedeemNames.STATUS_SPRAIN)]
		public string StatusSprain = "";

		[Name(RedeemNames.STATUS_FROSTBITE)]
		public string StatusFrostbite = "";

		[Name(RedeemNames.STATUS_STINK)]
		public string StatusStink = "";

		[Name(RedeemNames.INVENTORY_NO_PANTS)]
		public string InventoryNoPants = "";

		[Name(RedeemNames.INVENTORY_DROP_TORCH)]
		public string InventoryDropTorch = "";

		[Name(RedeemNames.INVENTORY_DROP_ITEM)]
		public string InventoryDropItem = "";

		[Name(RedeemNames.INVENTORY_STEPPED_STIM)]
		public string InventorySteppedStim = "";

		[Name(RedeemNames.INVENTORY_BOW)]
		public string InventoryBow = "";

		[Name(RedeemNames.MISC_TELEPORT)]
		public string MiscTeleport = "";

		[Name(RedeemNames.MISC_TIME)]
		public string MiscTime = "";

		[Name(RedeemNames.SOUND_420)]
		public string Sound420 = "";

		[Name(RedeemNames.DEV_SOUND)]
		public string DevSoundCheck = "";

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
