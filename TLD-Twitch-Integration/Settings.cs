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
		[Name("Enabled")]
		[Description("Enable or disable all TTI functionality")]
		public bool Enabled = true;

		[Name("Display Redeem Alert")]
		[Description("Enable or disable the redeem alert UI component")]
		public bool ShowAlert = true;

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

			if (field.Name == nameof(AllowWeatherRedeems))
				RefreshWeatherFields();

			if (field.Name == nameof(AllowSoundRedeems))
				RefreshSoundFields();
		}

		public void RefreshAllFields()
		{
			SetFieldVisible(nameof(ShowAlert), Enabled);
			SetFieldVisible(nameof(AllowWeatherRedeems), Enabled);
			SetFieldVisible(nameof(AllowSoundRedeems), Enabled);
			RefreshWeatherFields();
			RefreshSoundFields();
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
			SetFieldVisible(nameof(AllowSoundHello), allow);
			SetFieldVisible(nameof(AllowSound420), allow);
			SetFieldVisible(nameof(AllowSoundGoodNight), allow);
			SetFieldVisible(nameof(AllowSoundHydrate), allow);
		}
	}

}
