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

		public static CmdInventoryDropItem CmdInventoryDropItem { get; set; } = new();
		public static CmdInventoryStim CmdInventoryStim { get; set; } = new();
		public static CmdInventoryDropTorch CmdInventoryDropTorch { get; set; } = new();
		public static CmdInventoryNoPants CmdInventoryNoPants { get; set; } = new();
		public static CmdInventoryWeapon CmdInventoryWeapon { get; set; } = new();

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
			Commands.Add(CmdInventoryDropItem);
			Commands.Add(CmdInventoryStim);
			Commands.Add(CmdInventoryDropTorch);
			Commands.Add(CmdInventoryNoPants);
			Commands.Add(CmdInventoryWeapon);

			foreach (var cmd in Commands)
			{
				cmd.AddToConsole();
			}
		}
	}
}
