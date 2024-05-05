using Il2Cpp;

namespace TLD_Twitch_Integration
{
	public static class GameState
	{
		public static bool IsInBuilding { get; set; }
		public static bool IsAuroraActive { get; set; }
		public static bool IsAuroraFading { get; set; }

		public static void Update()
		{
			IsInBuilding = GetIsInBuilding();

			if (GameManager.GetAuroraManager().IsFullyActive())
				IsAuroraActive = true;
			else
				IsAuroraActive = false;

			IsAuroraFading = GetIsAuroraFading();
		}

		private static bool GetIsInBuilding()
		{
			if (GameManager.GetWeatherComponent().IsIndoorScene())
				return true;

			if (GameManager.GetWeatherComponent().IsIndoorEnvironment())
				return true;

			return false;
		}

		private static bool GetIsAuroraFading()
		{
			var alpha = GameManager.GetAuroraManager().GetNormalizedAlpha();

			if (alpha <= 0)
				return false;

			if (alpha >= 1)
				return false;

			return true;
		}

	}
}
