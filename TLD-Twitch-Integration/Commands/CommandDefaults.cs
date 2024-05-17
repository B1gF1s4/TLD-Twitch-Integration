namespace TLD_Twitch_Integration.Commands
{
	public static class CommandDefaults
	{
		public static List<CommandBase> Commands = new();

		public static CmdAnimalBigGame CmdAnimalBigGame { get; set; } = new();
		public static CmdAnimalBunnyExplosion CmdAnimalBunnyExplosion { get; set; } = new();
		public static CmdAnimalStalkingWolf CmdAnimalStalkingWolf { get; set; } = new();
		public static CmdAnimalTWolves CmdAnimalTWolves { get; set; } = new();

		public static CmdInventoryDropItem CmdInventoryDropItem { get; set; } = new();
		public static CmdInventoryStim CmdInventoryStim { get; set; } = new();

		public static void Init()
		{
			Commands.Add(CmdAnimalBigGame);
			Commands.Add(CmdAnimalBunnyExplosion);
			Commands.Add(CmdAnimalStalkingWolf);
			Commands.Add(CmdAnimalTWolves);
			Commands.Add(CmdInventoryDropItem);
			Commands.Add(CmdInventoryStim);

			foreach (var cmd in Commands)
			{
				cmd.AddToConsole();
			}
		}

	}
}
