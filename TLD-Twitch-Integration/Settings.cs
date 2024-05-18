using ModSettings;
using System.Reflection;
using TLD_Twitch_Integration.Commands;
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
		[Name("Enable")]
		[Description("Enable or disable TTI")]
		public bool Enabled = false;

		[Name("Display Redeem Alert")]
		[Description("Enable or disable TTIs UI components")]
		public bool ShowAlert = true;


		// TTI Animal

		[Section($"Animal: T-Wolves")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_T_WOLVES} redeem")]
		public bool AllowTWolves = true;

		[Name("Spawn Distance")]
		[Description($"How far to spawn T-Wolves in front of you")]
		[Slider(0.5f, 50.0f)]
		public float DistanceTWolf = 15.0f;


		[Section($"Animal: Big Game")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_BIG_GAME} redeem")]
		public bool AllowBigGame = true;

		[Name("Allow Bear")]
		[Description($"Enable or disable 'bear' for the {RedeemNames.ANIMAL_BIG_GAME} redeem")]
		public bool AllowBigGameBear = true;

		[Name("Bear Spawn Distance")]
		[Description($"How far to spawn Bear in front of you")]
		[Slider(0.5f, 50.0f)]
		public float DistanceBear = 30.0f;

		[Name("Allow Moose")]
		[Description($"Enable or disable 'moose' for the {RedeemNames.ANIMAL_BIG_GAME} redeem")]
		public bool AllowBigGameMoose = true;

		[Name("Moose Spawn Distance")]
		[Description($"How far to spawn Moose in front of you")]
		[Slider(0.5f, 50.0f)]
		public float DistanceMoose = 40.0f;


		[Section($"Animal: Stalking Wolf")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_STALKING_WOLF} redeem")]
		public bool AllowStalkingWolf = true;

		[Name("Spawn Distance")]
		[Description($"How far to spawn Stalking Wolf behind you")]
		[Slider(0.5f, 50.0f)]
		public float DistanceStalkingWolf = 18.0f;


		[Section($"Animal: Bunny Explosion")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.ANIMAL_BUNNY_EXPLOSION} redeem")]
		public bool AllowBunnyExplosion = true;

		[Name("Bunny Count")]
		[Description($"How many Bunnies to spawn")]
		[Slider(5, 50)]
		public int BunnyCount = 20;


		// TTI Weather

		[Section("Weather: Help")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelp = true;

		[Name("Allow Clear")]
		[Description($"Enable or disable 'clear' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpClear = true;

		[Name("Allow Light Fog")]
		[Description($"Enable or disable 'light fog' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpFog = true;

		[Name("Allow Light Snow")]
		[Description($"Enable or disable 'light snow' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpSnow = true;

		[Name("Allow Cloudy")]
		[Description($"Enable or disable 'cloudy' for the {RedeemNames.WEATHER_HELP} redeem")]
		public bool AllowWeatherHelpCloudy = true;


		[Section("Weather: Harm")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarm = true;

		[Name("Allow Blizzard")]
		[Description($"Enable or disable 'blizzard' for the {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarmBlizzard = true;

		[Name("Allow Dense Fog")]
		[Description($"Enable or disable 'dense fog' for the {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarmFog = true;

		[Name("Allow Heavy Snow")]
		[Description($"Enable or disable 'heavy snow' for the {RedeemNames.WEATHER_HARM} redeem")]
		public bool AllowWeatherHarmSnow = true;


		[Section("Weather: Aurora")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.WEATHER_AURORA} redeem")]
		public bool AllowWeatherAurora = true;


		[Section("Weather: Day / Night Toggle")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.WEATHER_TIME} redeem")]
		public bool AllowTime = true;


		// TTI Status

		[Section("Status: Help")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelp = true;

		[Name("Allow Warm")]
		[Description($"Enable or disable 'cold' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpWarm = true;

		[Name("Allow Awake")]
		[Description($"Enable or disable 'fatigue' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpAwake = true;

		[Name("Allow Not Thirsy")]
		[Description($"Enable or disable 'thirst' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpNotThirsty = true;

		[Name("Allow Full")]
		[Description($"Enable or disable 'hunger' for the {RedeemNames.STATUS_HELP} redeem")]
		public bool AllowStatusHelpFull = true;

		[Name("Help Value")]
		[Description("Value, helpful status redeems set the meter to.")]
		[Slider(0, 100)]
		public int StatusHelpValue = 90;


		[Section("Status: Harm")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarm = true;

		[Name("Allow Freezing")]
		[Description($"Enable or disable 'cold' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmFreezing = true;

		[Name("Allow Tired")]
		[Description($"Enable or disable 'fatigue' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmTired = true;

		[Name("Allow Thirsy")]
		[Description($"Enable or disable 'thirst' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmThirsty = true;

		[Name("Allow Hungry")]
		[Description($"Enable or disable 'hunger' for the {RedeemNames.STATUS_HARM} redeem")]
		public bool AllowStatusHarmHungry = true;

		[Name("Harm Value")]
		[Description("Value harmful status redeems set the meter to.")]
		[Slider(0, 100)]
		public int StatusHarmValue = 10;


		[Section("Status: Afflictions")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.STATUS_AFFLICTION} redeem")]
		public bool AllowAfflictions = true;

		[Name("Allow Cabin Fever")]
		[Description($"Enable or disable 'cabin fever' as possible output")]
		public bool AllowAfflictionCabinFever = true;

		[Name("Allow Dysentery")]
		[Description($"Enable or disable 'dysentery' as possible output")]
		public bool AllowAfflictionDysentery = true;

		[Name("Allow Food Poisoning")]
		[Description($"Enable or disable 'food poisoning' as possible output")]
		public bool AllowAfflictionFoodPoisoning = true;

		[Name("Allow Hypothermia")]
		[Description($"Enable or disable 'hypothermia' as possible output")]
		public bool AllowAfflictionHypothermia = true;

		[Name("Allow Parasites")]
		[Description($"Enable or disable 'hypothermia' as possible output")]
		public bool AllowAfflictionParasites = true;


		[Section("Status: Bleeding")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.STATUS_BLEED} redeem")]
		public bool AllowStatusBleeding = true;


		[Section("Status: Sprains")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.STATUS_SPRAIN} redeem")]
		public bool AllowStatusSprain = true;

		[Name("Allow Wrists")]
		[Description($"Enable or disable wrists for the {RedeemNames.STATUS_SPRAIN} redeem")]
		public bool AllowStatusSprainWrists = true;


		[Section("Status: Frostbites")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.STATUS_FROSTBITE} redeem")]
		public bool AllowStatusFrostbite = true;


		[Section("Status: Stink")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.STATUS_STINK} redeem")]
		public bool AllowStatusStink = true;

		[Name("Value")]
		[Description($"Amount of stink to get on the {RedeemNames.STATUS_STINK} redeem.")]
		[Slider(1, 100)]
		public int StinkLines = 90;

		[Name("Time")]
		[Description($"Amount of seconds stink lines will be active on the {RedeemNames.STATUS_STINK} redeem.")]
		[Slider(1, 600)]
		public int StinkTime = 300;


		// TTI Inventory

		[Section("Inventory: Team NoPants")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_NO_PANTS} redeem")]
		public bool AllowTeamNoPants = true;

		[Name("Allow Pickup")]
		[Description($"Allow picking up pants.")]
		public bool AllowPantsPickup = false;


		[Section("Inventory: Drop Torch")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_DROP_TORCH} redeem")]
		public bool AllowDropTorch = true;

		[Name("Allow Pickup")]
		[Description($"Allow picking up torch.")]
		public bool AllowTorchPickup = false;


		[Section("Inventory: Drop random Item")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_DROP_ITEM} redeem")]
		public bool AllowDropItem = true;

		[Name("Allow Pickup")]
		[Description($"Allow picking up dropped items.")]
		public bool AllowItemPickup = false;


		[Section("Inventory: Stepped on Stim")]
		[Name("Enable")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_STEPPED_STIM} redeem")]
		public bool AllowSteppedStim = true;


		[Name("Enable Weapon")]
		[Description($"Enable or disable {RedeemNames.INVENTORY_WEAPON} redeem")]
		public bool AllowWeapon = true;

		[Name("Allow Bow")]
		[Description($"Allow 'bow' for the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		public bool AllowWeaponBow = true;

		[Name("Arrow Count")]
		[Description($"Amount of arrows to get on the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		[Slider(1, 30)]
		public int ArrowCount = 15;

		[Name("Allow Rifle")]
		[Description($"Allow 'rifle' for the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		public bool AllowWeaponRifle = true;

		[Name("Rifle Ammo")]
		[Description($"Amount of rifle bullets to get on the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		[Slider(CmdInventoryWeapon.RifleBulletCapacity, 100)]
		public int RifleBulletCount = 40;

		[Name("Allow Revolver")]
		[Description($"Allow 'revolver' for the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		public bool AllowWeaponRevolver = true;

		[Name("Revolver Ammo")]
		[Description($"Amount of revolver bullets to get on the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		[Slider(CmdInventoryWeapon.RevolverBulletCapacity, 100)]
		public int RevolverBulletCount = 40;

		[Name("Allow Flaregun")]
		[Description($"Allow 'flaregun' for the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		public bool AllowWeaponFlaregun = true;

		[Name("Shells")]
		[Description($"Amount of flare shells to get on the {RedeemNames.INVENTORY_WEAPON} redeem.")]
		[Slider(CmdInventoryWeapon.FlaregunShellCapacity, 20)]
		public int ShellCount = 8;

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

			if (field.Name == nameof(AllowTeamNoPants))
				RefreshInventoryTeamNoPantsFields();

			if (field.Name == nameof(AllowDropTorch))
				RefreshInventoryDropTorchFields();

			if (field.Name == nameof(AllowItemPickup))
				RefreshInventoryDropItemFields();

			if (field.Name == nameof(AllowWeapon))
				RefreshInventoryWeaponFields();

			if (field.Name == nameof(AllowWeaponBow))
				RefreshWeaponArrowFields();

			if (field.Name == nameof(AllowWeaponRifle))
				RefreshWeaponRifleAmmoFields();

			if (field.Name == nameof(AllowWeaponRevolver))
				RefreshWeaponRevolverAmmoFields();

			if (field.Name == nameof(AllowWeaponFlaregun))
				RefreshWeaponShellFields();

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
			SetFieldVisible(nameof(AllowWeapon), Enabled);
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
			RefreshInventoryTeamNoPantsFields();
			RefreshInventoryDropTorchFields();
			RefreshInventoryDropItemFields();
			RefreshInventoryWeaponFields();
			RefreshWeaponArrowFields();
			RefreshWeaponRifleAmmoFields();
			RefreshWeaponRevolverAmmoFields();
			RefreshWeaponShellFields();
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

		private void RefreshInventoryTeamNoPantsFields()
		{
			var allow = AllowTeamNoPants && Enabled;
			SetFieldVisible(nameof(AllowPantsPickup), allow);
		}

		private void RefreshInventoryDropTorchFields()
		{
			var allow = AllowDropTorch && Enabled;
			SetFieldVisible(nameof(AllowTorchPickup), allow);
		}

		private void RefreshInventoryDropItemFields()
		{
			var allow = AllowDropItem && Enabled;
			SetFieldVisible(nameof(AllowItemPickup), allow);
		}

		private void RefreshInventoryWeaponFields()
		{
			var allow = AllowWeapon && Enabled;
			SetFieldVisible(nameof(AllowWeaponBow), allow);
			SetFieldVisible(nameof(AllowWeaponRifle), allow);
			SetFieldVisible(nameof(AllowWeaponRevolver), allow);
			SetFieldVisible(nameof(AllowWeaponFlaregun), allow);
			RefreshWeaponArrowFields();
			RefreshWeaponRifleAmmoFields();
			RefreshWeaponRevolverAmmoFields();
			RefreshWeaponShellFields();
		}

		private void RefreshWeaponArrowFields()
		{
			var allow = AllowWeapon && AllowWeaponBow && Enabled;
			SetFieldVisible(nameof(ArrowCount), allow);
		}

		private void RefreshWeaponRifleAmmoFields()
		{
			var allow = AllowWeapon && AllowWeaponRifle && Enabled;
			SetFieldVisible(nameof(RifleBulletCount), allow);
		}

		private void RefreshWeaponRevolverAmmoFields()
		{
			var allow = AllowWeapon && AllowWeaponRevolver && Enabled;
			SetFieldVisible(nameof(RevolverBulletCount), allow);
		}

		private void RefreshWeaponShellFields()
		{
			var allow = AllowWeapon && AllowWeaponFlaregun && Enabled;
			SetFieldVisible(nameof(ShellCount), allow);
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
