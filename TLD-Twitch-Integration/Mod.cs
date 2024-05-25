using MelonLoader;
using MelonLoader.Utils;
using TLD_Twitch_Integration.Commands;
using TLD_Twitch_Integration.Game;

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

			CommandDefaults.Init();

			Settings.OnLoad();
			await AuthService.OnLoad();
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if (sceneName.Contains("Menu"))
			{
				GameService.InitAudio();
			}
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
