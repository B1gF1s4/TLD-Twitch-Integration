using MelonLoader;
using MelonLoader.Utils;

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
