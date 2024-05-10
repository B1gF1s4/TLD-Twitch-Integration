using Il2Cpp;

namespace TLD_Twitch_Integration
{
	public static class GameState
	{
		public static bool IsInBuilding { get; set; }
		public static bool IsAuroraActive { get; set; }
		public static bool IsAuroraFading { get; set; }
		public static bool IsHoldingTorchLike { get; set; }
		public static bool HasTorchLikeInInventory { get; set; }

		public static void Update()
		{
			IsInBuilding = GetIsInBuilding();

			if (GameManager.GetAuroraManager().IsFullyActive())
				IsAuroraActive = true;
			else
				IsAuroraActive = false;

			IsAuroraFading = GetIsAuroraFading();
			IsHoldingTorchLike = GetIsHoldingTorchLike();
			HasTorchLikeInInventory = GetHasTorchLikeInInventory();
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

		private static bool GetIsHoldingTorchLike()
		{
			var playerComp = GameManager.GetPlayerManagerComponent();
			if (playerComp == null)
				return false;

			if (playerComp.m_ItemInHands == null)
				return false;

			if (playerComp.m_ItemInHands.m_FlareItem != null ||
				playerComp.m_ItemInHands.m_TorchItem != null ||
				playerComp.m_ItemInHands.m_FlashlightItem != null)
				return true;

			return false;
		}

		private static bool GetHasTorchLikeInInventory()
		{
			var invComp = GameManager.GetInventoryComponent();
			if (invComp == null)
				return false;

			var num = invComp.GetNumFlares(FlareType.Red) +
				invComp.GetNumFlares(FlareType.Blue) +
				invComp.GetNumTorches();

			if (num <= 0)
				return false;

			return true;
		}

	}
}
