using ModSettings;
using System.Reflection;
using TLD_Twitch_Integration.Twitch.Redeems;

namespace TLD_Twitch_Integration
{
	public static class Settings
	{
		internal static readonly Login Token = new();
		internal static readonly ModSettings ModSettings = new();
		internal static readonly CustomRewardIdMapping Redeems = new();

		public static void OnLoad()
		{
			ModSettings.AddToModSettings("Twitch Integration");
		}
	}

	public class Login : JsonFile
	{
		[Name("access")]
		public string Access = "";

		[Name("refresh")]
		public string Refresh = "";

		public Login() : base(Path.Combine(Mod.BaseDirectory, "login"))
		{ }
	}

	public class ModSettings : JsonModSettings
	{
		[Section("General")]
		[Name("Enabled")]
		[Description("Enable or disable all TTI functionality")]
		public bool Enabled = true;

		[Name("Display Redeem Alert")]
		[Description("Enable or disable the redeem alert UI component")]
		public bool ShowAlert = true;

		[Section("Animal Redeems")]
		[Name("Allow Animal Redeems")]
		[Description($"Enable or disable all animal redeems")]
		public bool AllowAnimalRedeems = false;

		[Name("Allow T-Wolves")]
		[Description($"Enable or disable the {RedeemNames.ANIMAL_T_WOLVES} redeem")]
		public bool AllowTWolves = false;

		[Name("Allow Bear")]
		[Description($"Enable or disable the {RedeemNames.ANIMAL_BEAR} redeem")]
		public bool AllowBear = false;

		[Name("Allow Moose")]
		[Description($"Enable or disable the {RedeemNames.ANIMAL_MOOSE} redeem")]
		public bool AllowMoose = false;

		[Name("Allow Stalking Wolf")]
		[Description($"Enable or disable the {RedeemNames.ANIMAL_STALKING_WOLF} redeem")]
		public bool AllowStalkingWolf = false;

		[Name("Allow Bunny Explosion")]
		[Description($"Enable or disable the {RedeemNames.ANIMAL_BUNNY_EXPLOSION} redeem")]
		public bool AllowBunnyExplosion = false;

		[Section("Weather Redeems")]
		[Name("Allow Weather Redeems")]
		[Description($"Enable or disable all weather redeems")]
		public bool AllowWeatherRedeems = false;

		[Name("Allow Blizzard")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_BLIZZARD} redeem")]
		public bool AllowWeatherBlizzard = false;

		[Name("Allow Clear")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_CLEAR} redeem")]
		public bool AllowWeatherClear = false;

		[Name("Allow Light Fog")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_LIGHT_FOG} redeem")]
		public bool AllowWeatherLightFog = false;

		[Name("Allow Dense Fog")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_DENSE_FOG} redeem")]
		public bool AllowWeatherDenseFog = false;

		[Name("Allow Partly Cloudy")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_PARTLY_CLOUDY} redeem")]
		public bool AllowWeatherPartlyCloudy = false;

		[Name("Allow Cloudy")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_CLOUDY} redeem")]
		public bool AllowWeatherCloudy = false;

		[Name("Allow Light Snow")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_LIGHT_SNOW} redeem")]
		public bool AllowWeatherLightSnow = false;

		[Name("Allow Heavy Snow")]
		[Description($"Enable or disable the {RedeemNames.WEATHER_HEAVY_SNOW} redeem")]
		public bool AllowWeatherHeavySnow = false;

		[Section("Sound Redeems")]
		[Name("Allow Sound Redeems")]
		[Description("Enable or disable all sound redeems")]
		public bool AllowSoundRedeems = false;

		[Name("Allow Dev Sound Check")]
		[Description($"Enable or disable the {RedeemNames.SOUND} redeem")]
		public bool AllowDevSoundCheck = false;

		[Name("Allow Hello")]
		[Description($"Enable or disable the {RedeemNames.SOUND_HELLO} redeem")]
		public bool AllowSoundHello = false;

		[Name("Allow Happy 420")]
		[Description($"Enable or disable the {RedeemNames.SOUND_420} redeem")]
		public bool AllowSound420 = false;

		[Name("Allow Good Night")]
		[Description($"Enable or disable the {RedeemNames.SOUND_GOOD_NIGHT} redeem")]
		public bool AllowSoundGoodNight = false;

		[Name("Allow Hydrate")]
		[Description($"Enable or disable the {RedeemNames.SOUND_HYDRATE} redeem")]
		public bool AllowSoundHydrate = false;

		[Section("Harmful Status Redeems")]
		[Name("Allow Harmful Status Redeems")]
		[Description("Enable or disable all harmful status redeems")]
		public bool AllowHarmfulStatus = false;

		[Name("Allow Hungry")]
		[Description($"Enable or disable the {RedeemNames.STATUS_HUNGRY} redeem")]
		public bool AllowStatusHungry = false;

		[Name("Allow Thirsty")]
		[Description($"Enable or disable the {RedeemNames.STATUS_THIRSTY} redeem")]
		public bool AllowStatusThirsty = false;

		[Name("Allow Tired")]
		[Description($"Enable or disable the {RedeemNames.STATUS_TIRED} redeem")]
		public bool AllowStatusTired = false;

		[Name("Allow Freezing")]
		[Description($"Enable or disable the {RedeemNames.STATUS_FREEZING} redeem")]
		public bool AllowStatusFreezing = false;

		[Section("Helpful Status Redeems")]
		[Name("Allow Helpful Status Redeems")]
		[Description("Enable or disable all helpful status redeems")]
		public bool AllowHelpfulStatus = false;

		[Name("Allow Full")]
		[Description($"Enable or disable the {RedeemNames.STATUS_FULL} redeem")]
		public bool AllowStatusFull = false;

		[Name("Allow Not Thirsty")]
		[Description($"Enable or disable the {RedeemNames.STATUS_NOT_THIRSTY} redeem")]
		public bool AllowStatusNotThirsty = false;

		[Name("Allow Awake")]
		[Description($"Enable or disable the {RedeemNames.STATUS_AWAKE} redeem")]
		public bool AllowStatusAwake = false;

		[Name("Allow Warm")]
		[Description($"Enable or disable the {RedeemNames.STATUS_WARM} redeem")]
		public bool AllowStatusWarm = false;

		public ModSettings() : base(Path.Combine(Mod.BaseDirectory, "user-settings"))
		{
			RefreshAllFields();
		}

		protected override void OnConfirm()
		{
			base.OnConfirm();
			CustomRewardsService.SyncRequired = true;
		}

		protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
		{
			if (field.Name == nameof(Enabled))
				RefreshAllFields();

			if (field.Name == nameof(AllowAnimalRedeems))
				RefreshAnimalFields();

			if (field.Name == nameof(AllowWeatherRedeems))
				RefreshWeatherFields();

			if (field.Name == nameof(AllowSoundRedeems))
				RefreshSoundFields();

			if (field.Name == nameof(AllowHelpfulStatus))
				RefreshHelpfulStatusFields();

			if (field.Name == nameof(AllowHarmfulStatus))
				RefreshHarmfulStatusFields();
		}

		public void RefreshAllFields()
		{
			SetFieldVisible(nameof(ShowAlert), Enabled);
			SetFieldVisible(nameof(AllowAnimalRedeems), Enabled);
			SetFieldVisible(nameof(AllowWeatherRedeems), Enabled);
			SetFieldVisible(nameof(AllowSoundRedeems), Enabled);
			SetFieldVisible(nameof(AllowHelpfulStatus), Enabled);
			SetFieldVisible(nameof(AllowHarmfulStatus), Enabled);
			RefreshAnimalFields();
			RefreshWeatherFields();
			RefreshSoundFields();
			RefreshHelpfulStatusFields();
			RefreshHarmfulStatusFields();
		}

		private void RefreshAnimalFields()
		{
			var allow = AllowAnimalRedeems && Enabled;
			SetFieldVisible(nameof(AllowTWolves), allow);
			SetFieldVisible(nameof(AllowBear), allow);
			SetFieldVisible(nameof(AllowMoose), allow);
			SetFieldVisible(nameof(AllowStalkingWolf), allow);
			SetFieldVisible(nameof(AllowBunnyExplosion), allow);
		}

		private void RefreshWeatherFields()
		{
			var allow = AllowWeatherRedeems && Enabled;
			SetFieldVisible(nameof(AllowWeatherBlizzard), allow);
			SetFieldVisible(nameof(AllowWeatherClear), allow);
			SetFieldVisible(nameof(AllowWeatherLightFog), allow);
			SetFieldVisible(nameof(AllowWeatherDenseFog), allow);
			SetFieldVisible(nameof(AllowWeatherPartlyCloudy), allow);
			SetFieldVisible(nameof(AllowWeatherCloudy), allow);
			SetFieldVisible(nameof(AllowWeatherLightSnow), allow);
			SetFieldVisible(nameof(AllowWeatherHeavySnow), allow);
		}

		private void RefreshSoundFields()
		{
			var allow = AllowSoundRedeems && Enabled;
			SetFieldVisible(nameof(AllowDevSoundCheck), allow);
			SetFieldVisible(nameof(AllowSoundHello), allow);
			SetFieldVisible(nameof(AllowSound420), allow);
			SetFieldVisible(nameof(AllowSoundGoodNight), allow);
			SetFieldVisible(nameof(AllowSoundHydrate), allow);
		}

		private void RefreshHelpfulStatusFields()
		{
			var allow = AllowHelpfulStatus && Enabled;
			SetFieldVisible(nameof(AllowStatusFull), allow);
			SetFieldVisible(nameof(AllowStatusNotThirsty), allow);
			SetFieldVisible(nameof(AllowStatusAwake), allow);
			SetFieldVisible(nameof(AllowStatusWarm), allow);
		}

		private void RefreshHarmfulStatusFields()
		{
			var allow = AllowHarmfulStatus && Enabled;
			SetFieldVisible(nameof(AllowStatusHungry), allow);
			SetFieldVisible(nameof(AllowStatusThirsty), allow);
			SetFieldVisible(nameof(AllowStatusTired), allow);
			SetFieldVisible(nameof(AllowStatusFreezing), allow);
		}
	}

}
