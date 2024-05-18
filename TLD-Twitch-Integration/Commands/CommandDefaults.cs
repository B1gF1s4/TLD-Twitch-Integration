namespace TLD_Twitch_Integration.Commands
{
	public static class CommandDefaults
	{
		public static List<CommandBase> Commands = new();

		public static CmdAnimalBigGame CmdAnimalBigGame { get; set; } = new();
		public static CmdAnimalBunnyExplosion CmdAnimalBunnyExplosion { get; set; } = new();
		public static CmdAnimalStalkingWolf CmdAnimalStalkingWolf { get; set; } = new();
		public static CmdAnimalTWolves CmdAnimalTWolves { get; set; } = new();

		public static CmdWeatherHelp CmdWeatherHelp { get; set; } = new();
		public static CmdWeatherHarm CmdWeatherHarm { get; set; } = new();
		public static CmdWeatherTime CmdWeatherTime { get; set; } = new();
		public static CmdWeatherAurora CmdWeatherAurora { get; set; } = new();

		public static CmdStatusHelp CmdStatusHelp { get; set; } = new();
		public static CmdStatusHarm CmdStatusHarm { get; set; } = new();
		public static CmdStatusAffliction CmdStatusAffliction { get; set; } = new();
		public static CmdStatusBleeding CmdStatusBleeding { get; set; } = new();
		public static CmdStatusSprain CmdStatusSprain { get; set; } = new();
		public static CmdStatusFrostbite CmdStatusFrostbite { get; set; } = new();
		public static CmdStatusStink CmdStatusStink { get; set; } = new();

		public static CmdInventoryDropItem CmdInventoryDropItem { get; set; } = new();
		public static CmdInventoryStim CmdInventoryStim { get; set; } = new();
		public static CmdInventoryDropTorch CmdInventoryDropTorch { get; set; } = new();
		public static CmdInventoryNoPants CmdInventoryNoPants { get; set; } = new();
		public static CmdInventoryWeapon CmdInventoryWeapon { get; set; } = new();

		public static CmdSound420 CmdSound420 { get; set; } = new();

		public static CmdDevSoundCheck CmdDevSoundCheck { get; set; } = new();

		public static void Init()
		{
			Commands.Add(CmdAnimalBigGame);
			Commands.Add(CmdAnimalBunnyExplosion);
			Commands.Add(CmdAnimalStalkingWolf);
			Commands.Add(CmdAnimalTWolves);
			Commands.Add(CmdWeatherHelp);
			Commands.Add(CmdWeatherHarm);
			Commands.Add(CmdWeatherTime);
			Commands.Add(CmdWeatherAurora);
			Commands.Add(CmdStatusHelp);
			Commands.Add(CmdStatusHarm);
			Commands.Add(CmdStatusAffliction);
			Commands.Add(CmdStatusBleeding);
			Commands.Add(CmdStatusSprain);
			Commands.Add(CmdStatusFrostbite);
			Commands.Add(CmdStatusStink);
			Commands.Add(CmdInventoryDropItem);
			Commands.Add(CmdInventoryStim);
			Commands.Add(CmdInventoryDropTorch);
			Commands.Add(CmdInventoryNoPants);
			Commands.Add(CmdInventoryWeapon);
			Commands.Add(CmdSound420);
			Commands.Add(CmdDevSoundCheck);

			foreach (var cmd in Commands)
			{
				cmd.AddToConsole();
			}
		}
	}
}
