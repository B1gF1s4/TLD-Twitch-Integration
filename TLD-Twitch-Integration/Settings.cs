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
		// General

		[Section("General")]

		[Name("Enabled")]
		[Description("Enable or disable TTI")]
		public bool Enabled = false;

		[Name("Display Redeem Alert")]
		[Description("Enable or disable TTIs UI components")]
		public bool ShowAlert = true;

		// TTI Animal

		[Section("Animals")]

		[Name("Enable T-Wolves")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_T_WOLVES} redeem")]
		public bool AllowTWolves = true;

		[Name("      T-Wolf Spawn Distance")]
		[Description($"How far to spawn T-Wolves in front of you")]
		[Slider(0.5f, 50.0f)]
		public float DistanceTWolf = 15.0f;

		[Name("Enable Big Game")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_BIG_GAME} redeem")]
		public bool AllowBigGame = true;

		[Name("      Allow Bear")]
		[Description($"Enable or disable 'bear' for the {RedeemNames.ANIMAL_BIG_GAME} redeem")]
		public bool AllowBigGameBear = true;

		[Name("      Bear Spawn Distance")]
		[Description($"How far to spawn Bear in front of you")]
		[Slider(0.5f, 100.0f)]
		public float DistanceBear = 30.0f;

		[Name("      Allow Moose")]
		[Description($"Enable or disable 'moose' for the {RedeemNames.ANIMAL_BIG_GAME} redeem")]
		public bool AllowBigGameMoose = true;

		[Name("      Moose Spawn Distance")]
		[Description($"How far to spawn Moose in front of you")]
		[Slider(0.5f, 100.0f)]
		public float DistanceMoose = 40.0f;

		[Name("Enable Stalking Wolf")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_STALKING_WOLF} redeem")]
		public bool AllowStalkingWolf = true;

		[Name("      Stalking Wolf Distance")]
		[Description($"How far to spawn Stalking Wolf behind you")]
		[Slider(0.5f, 100.0f)]
		public float DistanceStalkingWolf = 18.0f;

		[Name("Enable Bunny Explosion")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_BUNNY_EXPLOSION} redeem")]
		public bool AllowBunnyExplosion = true;

		[Name("      Bunny Count")]
		[Description($"How many Bunnies to spawn")]
		[Slider(5, 100)]
		public int BunnyCount = 20;

		// TTI Weather

		[Section("Weather")]

		[Name("Enable Weather Help")]
		[Description($"Enable or disable {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelp = true;

		[Name("      Allow Weather Clear")]
		[Description($"Enable or disable 'clear' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpClear = true;

		[Name("      Allow Weather Light Fog")]
		[Description($"Enable or disable 'light fog' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpFog = true;

		[Name("      Allow Weather Light Snow")]
		[Description($"Enable or disable 'light snow' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpSnow = true;

		[Name("      Allow Weather Cloudy")]
		[Description($"Enable or disable 'cloudy' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpCloudy = true;

		[Name("Enable Weather Harm")]
		[Description($"Enable or disable {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarm = true;

		[Name("      Allow Weather Blizzard")]
		[Description($"Enable or disable 'blizzard' for the {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarmBlizzard = true;

		[Name("     Allow Weather Dense Fog")]
		[Description($"Enable or disable 'dense fog' for the {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarmFog = true;

		[Name("     Allow Weather Heavy Snow")]
		[Description($"Enable or disable 'heavy snow' for the {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarmSnow = true;

		[Name("Enable Aurora")]
		[Description($"Enable or disable {RedeemNames.WEATHER_AURORA} redeem")]
		public bool AllowWeatherAurora = true;

		[Name("Enable Day/Night Toggle")]
		[Description($"Enable or disable {RedeemNames.MISC_TIME} redeem")]
		public bool AllowTime = true;

		// TTI Status

		[Section("Status")]

		[Name("Enable Status Help")]
		[Description($"Enable or disable {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelp = true;

		[Name("      Allow Status Warm")]
		[Description($"Enable or disable 'cold' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpWarm = true;

		[Name("      Allow Status Awake")]
		[Description($"Enable or disable 'fatigue' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpAwake = true;

		[Name("      Allow Status Not Thirsy")]
		[Description($"Enable or disable 'thirst' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpNotThirsty = true;

		[Name("      Allow Status Full")]
		[Description($"Enable or disable 'hunger' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpFull = true;

		[Name("      Status Help Value")]
		[Description("Value helpful status redeems set the meter to.")]
		[Slider(0f, 100f)]
		public float StatusHelpValue = 90.0f;

		[Name("Enable Status Harm")]
		[Description($"Enable or disable {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarm = true;

		[Name("      Allow Status Freezing")]
		[Description($"Enable or disable 'cold' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmFreezing = true;

		[Name("      Allow Status Tired")]
		[Description($"Enable or disable 'fatigue' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmTired = true;

		[Name("      Allow Status Thirsy")]
		[Description($"Enable or disable 'thirst' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmThirsty = true;

		[Name("      Allow Status Hungry")]
		[Description($"Enable or disable 'hunger' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmHungry = true;

		[Name("      Status Harm Value")]
		[Description("Value harmful status redeems set the meter to.")]
		[Slider(0f, 100f)]
		public float StatusHarmValue = 10.0f;

		[Name("Enable Afflictions")]
		[Description($"Enable or disable {RedeemNames.STATUS_AFFLICTION} redeem")]
		public bool AllowAfflictions = true;

		[Name("      Allow Affliction Cabin Fever")]
		[Description($"Enable or disable 'cabin fever' as possible output")]
		public bool AllowAfflictionCabinFever = true;

		[Name("      Allow Affliction Dysentery")]
		[Description($"Enable or disable 'dysentery' as possible output")]
		public bool AllowAfflictionDysentery = true;

		[Name("      Allow Affliction Food Poisoning")]
		[Description($"Enable or disable 'food poisoning' as possible output")]
		public bool AllowAfflictionFoodPoisoning = true;

		[Name("      Allow Affliction Hypothermia")]
		[Description($"Enable or disable 'hypothermia' as possible output")]
		public bool AllowAfflictionHypothermia = true;

		[Name("      Allow Affliction Parasites")]
		[Description($"Enable or disable 'hypothermia' as possible output")]
		public bool AllowAfflictionParasites = true;

		[Name("Enable Bleeding")]
		[Description($"Enable or disable {RedeemNames.STATUS_BLEED} redeem")]
		public bool AllowStatusBleeding = true;

		[Name("Enable Sprains")]
		[Description($"Enable or disable {RedeemNames.STATUS_SPRAIN} redeem")]
		public bool AllowStatusSprain = true;

		[Name("      Allow Sprained Wrists")]
		[Description($"Enable or disable wrists for the {RedeemNames.STATUS_SPRAIN} redeem")]
		public bool AllowStatusSprainWrists = true;

		[Name("Enable Frostbites")]
		[Description($"Enable or disable {RedeemNames.STATUS_FROSTBITE} redeem")]
		public bool AllowStatusFrostbite = true;

		[Name("Enable Stink")]
		[Description($"Enable or disable {RedeemNames.STATUS_STINK} redeem")]
		public bool AllowStatusStink = true;

		[Name("      Stink Value")]
		[Description($"Amount of stink to get on the {RedeemNames.STATUS_STINK} redeem.")]
		[Slider(1, 100)]
		public float StinkLines = 90;

		[Name("      Stink Time")]
		[Description($"Amount of seconds stink lines will be active on the {RedeemNames.STATUS_STINK} redeem.")]
		[Slider(1, 600)]
		public int StinkTime = 300;

		// TTI Inventory

		[Section("Inventory")]

		[Name("Enable Team NoPants")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_NO_PANTS} redeem")]
		public bool AllowTeamNoPants = true;

		[Name("Enable Drop Torch")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_DROP_TORCH} redeem")]
		public bool AllowDropTorch = true;

		[Name("Enable Drop Item")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_DROP_ITEM} redeem")]
		public bool AllowDropItem = true;

		[Name("Enable Stepped on Stim")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_STEPPED_STIM} redeem")]
		public bool AllowSteppedStim = true;

		[Name("Enable Bow")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_BOW} redeem")]
		public bool AllowBow = true;

		[Name("      Arrow Count")]
		[Description($"Amount of arrows to get on the {RedeemNames.INVENTORY_BOW} redeem.")]
		[Slider(1, 30)]
		public int ArrowCount = 15;

		// TTI Sounds

		[Section("Sounds")]

		[Name("Enable Happy 420")]
		[Description($"Enable or disable {RedeemNames.SOUND_420} redeem")]
		public bool AllowSound420 = false;

		[Section("Dev")]

		[Name("Enable Sound Check")]
		[Description($"Enable or disable {RedeemNames.DEV_SOUND} redeem")]
		public bool AllowDevSoundCheck = false;

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

			if (field.Name == nameof(AllowWeatherHelp))
				RefreshWeatherHelpFields();

			if (field.Name == nameof(AllowWeatherHarm))
				RefreshWeatherHarmFields();

			if (field.Name == nameof(AllowStatusHelp))
				RefreshStatusHelpFields();

			if (field.Name == nameof(AllowStatusHarm))
				RefreshStatusHarmFields();

			if (field.Name == nameof(AllowAfflictions))
				RefreshAfflictionsFields();

			if (field.Name == nameof(AllowStatusSprain))
				RefreshStatusSprainFields();

			if (field.Name == nameof(AllowStatusStink))
				RefreshStatusStinkFields();

			if (field.Name == nameof(AllowBow))
				RefreshInventoryBowFields();

			if (field.Name == nameof(AllowBigGame))
				RefreshBigGameFields();

			if (field.Name == nameof(AllowBigGameBear))
				RefreshBigGameBearFields();

			if (field.Name == nameof(AllowBigGameMoose))
				RefreshBigGameMooseFields();

			if (field.Name == nameof(AllowTWolves))
				RefreshTWolfFields();

			if (field.Name == nameof(AllowStalkingWolf))
				RefreshStalkingWolfFields();

			if (field.Name == nameof(AllowBunnyExplosion))
				RefreshBunnyExplosionFields();
		}

		public void RefreshAllFields()
		{
			SetFieldVisible(nameof(ShowAlert), Enabled);
			SetFieldVisible(nameof(AllowWeatherHelp), Enabled);
			SetFieldVisible(nameof(AllowWeatherHarm), Enabled);
			SetFieldVisible(nameof(AllowWeatherAurora), Enabled);
			SetFieldVisible(nameof(AllowStatusHelp), Enabled);
			SetFieldVisible(nameof(AllowStatusHarm), Enabled);
			SetFieldVisible(nameof(AllowAfflictions), Enabled);
			SetFieldVisible(nameof(AllowStatusBleeding), Enabled);
			SetFieldVisible(nameof(AllowStatusSprain), Enabled);
			SetFieldVisible(nameof(AllowStatusFrostbite), Enabled);
			SetFieldVisible(nameof(AllowStatusStink), Enabled);
			SetFieldVisible(nameof(AllowTeamNoPants), Enabled);
			SetFieldVisible(nameof(AllowDropTorch), Enabled);
			SetFieldVisible(nameof(AllowDropItem), Enabled);
			SetFieldVisible(nameof(AllowSteppedStim), Enabled);
			SetFieldVisible(nameof(AllowBow), Enabled);
			SetFieldVisible(nameof(AllowTWolves), Enabled);
			SetFieldVisible(nameof(AllowBigGame), Enabled);
			SetFieldVisible(nameof(AllowStalkingWolf), Enabled);
			SetFieldVisible(nameof(AllowBunnyExplosion), Enabled);
			SetFieldVisible(nameof(AllowTime), Enabled);
			SetFieldVisible(nameof(AllowSound420), Enabled);
			SetFieldVisible(nameof(AllowDevSoundCheck), Enabled);
			RefreshWeatherHelpFields();
			RefreshWeatherHarmFields();
			RefreshStatusHelpFields();
			RefreshStatusHarmFields();
			RefreshAfflictionsFields();
			RefreshStatusSprainFields();
			RefreshStatusStinkFields();
			RefreshInventoryBowFields();
			RefreshBigGameFields();
			RefreshTWolfFields();
			RefreshStalkingWolfFields();
			RefreshBunnyExplosionFields();
		}

		private void RefreshWeatherHelpFields()
		{
			var allow = AllowWeatherHelp && Enabled;
			SetFieldVisible(nameof(AllowWeatherHelpClear), allow);
			SetFieldVisible(nameof(AllowWeatherHelpCloudy), allow);
			SetFieldVisible(nameof(AllowWeatherHelpFog), allow);
			SetFieldVisible(nameof(AllowWeatherHelpSnow), allow);
		}

		private void RefreshWeatherHarmFields()
		{
			var allow = AllowWeatherHarm && Enabled;
			SetFieldVisible(nameof(AllowWeatherHarmBlizzard), allow);
			SetFieldVisible(nameof(AllowWeatherHarmFog), allow);
			SetFieldVisible(nameof(AllowWeatherHarmSnow), allow);
		}

		private void RefreshStatusHelpFields()
		{
			var allow = AllowStatusHelp && Enabled;
			SetFieldVisible(nameof(AllowStatusHelpWarm), allow);
			SetFieldVisible(nameof(AllowStatusHelpAwake), allow);
			SetFieldVisible(nameof(AllowStatusHelpNotThirsty), allow);
			SetFieldVisible(nameof(AllowStatusHelpFull), allow);
			SetFieldVisible(nameof(StatusHelpValue), allow);
		}

		private void RefreshStatusHarmFields()
		{
			var allow = AllowStatusHarm && Enabled;
			SetFieldVisible(nameof(AllowStatusHarmFreezing), allow);
			SetFieldVisible(nameof(AllowStatusHarmTired), allow);
			SetFieldVisible(nameof(AllowStatusHarmThirsty), allow);
			SetFieldVisible(nameof(AllowStatusHarmHungry), allow);
			SetFieldVisible(nameof(StatusHarmValue), allow);
		}

		private void RefreshAfflictionsFields()
		{
			var allow = AllowAfflictions && Enabled;
			SetFieldVisible(nameof(AllowAfflictionCabinFever), allow);
			SetFieldVisible(nameof(AllowAfflictionDysentery), allow);
			SetFieldVisible(nameof(AllowAfflictionFoodPoisoning), allow);
			SetFieldVisible(nameof(AllowAfflictionHypothermia), allow);
			SetFieldVisible(nameof(AllowAfflictionParasites), allow);
		}

		private void RefreshStatusSprainFields()
		{
			var allow = AllowStatusSprain && Enabled;
			SetFieldVisible(nameof(AllowStatusSprainWrists), allow);
		}

		private void RefreshStatusStinkFields()
		{
			var allow = AllowStatusStink && Enabled;
			SetFieldVisible(nameof(StinkLines), allow);
			SetFieldVisible(nameof(StinkTime), allow);
		}

		private void RefreshInventoryBowFields()
		{
			var allow = AllowBow && Enabled;
			SetFieldVisible(nameof(ArrowCount), allow);
		}

		private void RefreshBigGameFields()
		{
			var allow = AllowBigGame && Enabled;
			SetFieldVisible(nameof(AllowBigGameBear), allow);
			SetFieldVisible(nameof(AllowBigGameMoose), allow);
			RefreshBigGameBearFields();
			RefreshBigGameMooseFields();
		}

		private void RefreshBigGameBearFields()
		{
			var allow = AllowBigGame && AllowBigGameBear && Enabled;
			SetFieldVisible(nameof(DistanceBear), allow);
		}

		private void RefreshBigGameMooseFields()
		{
			var allow = AllowBigGame && AllowBigGameMoose && Enabled;
			SetFieldVisible(nameof(DistanceMoose), allow);
		}

		private void RefreshTWolfFields()
		{
			var allow = AllowTWolves && Enabled;
			SetFieldVisible(nameof(DistanceTWolf), allow);
		}

		private void RefreshStalkingWolfFields()
		{
			var allow = AllowStalkingWolf && Enabled;
			SetFieldVisible(nameof(DistanceStalkingWolf), allow);
		}

		private void RefreshBunnyExplosionFields()
		{
			var allow = AllowBunnyExplosion && Enabled;
			SetFieldVisible(nameof(BunnyCount), allow);
		}
	}
}
