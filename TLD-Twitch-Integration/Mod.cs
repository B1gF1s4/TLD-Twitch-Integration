using MelonLoader;
using MelonLoader.Utils;
using TLD_Twitch_Integration.Commands;

namespace TLD_Twitch_Integration
{
	public class Mod : MelonMod
	{

		public const string BaseDirectory = nameof(TLD_Twitch_Integration);

		public static bool ShouldKill { get; set; }

		public async override void OnInitializeMelon()
		{
			base.OnInitializeMelon();

			Directory.CreateDirectory(Path.Combine(
				MelonEnvironment.ModsDirectory, BaseDirectory));

			CmdAnimalBigGame.AddCommandToConsole();
			CmdAnimalTWolves.AddCommandToConsole();
			CmdAnimalStalkingWolf.AddCommandToConsole();
			CmdAnimalBunnyExplosion.AddCommandToConsole();

			Settings.OnLoad();
			await AuthService.OnLoad();
		}

		public async override void OnUpdate()
		{
			base.OnUpdate();

			if (ShouldKill)
				return;

			await AuthService.OnUpdate();
			await CustomRewardsService.OnUpdate();
			await RedemptionService.OnUpdate();
			await ExecutionService.OnUpdate();
		}

		public static bool ShouldUpdate(DateTime lastUpdated, int interval)
		{
			return !((DateTime.UtcNow - lastUpdated).TotalSeconds < interval);
		}
	}
}
