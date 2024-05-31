using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdInventoryDropTorch : CommandBase
	{

		public CmdInventoryDropTorch() : base("tti_inventory_torch")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowDropTorch)
				throw new RequiresRedeemRefundException(
					"Drop torch redeem is currently disabled.");

			if (GameService.IsMenuOpen())
				return "";

			if (!GameService.HasTorchLikeInInventory)
				throw new RequiresRedeemRefundException(
					"Player has no torch in inventory.");

			GameService.PlayPlayerSound("PLAY_FEARAFFLICTION");

			if (GameService.IsHoldingTorchLike)
			{
				var torch = GameManager.GetPlayerManagerComponent().m_ItemInHands;
				var droppedItem = torch?.Drop(1, true, false, true);

				if (droppedItem != null &&
					!Settings.ModSettings.AllowTorchPickup)
					droppedItem.enabled = false;
			}
			else
			{
				GearItem? itemToDrop = null;
				var inv = GameManager.GetInventoryComponent() ??
					throw new RequiresRedeemRefundException("Inventory not accessable");

				if (inv.GetNumTorches() > 0)
					itemToDrop = inv.GetBestGearItemWithName("GEAR_Torch");

				else if (inv.GetNumFlares(FlareType.Red) > 0)
					itemToDrop = inv.GetBestGearItemWithName("GEAR_FlareA");

				else if (inv.GetNumFlares(FlareType.Blue) > 0)
					itemToDrop = inv.GetBestGearItemWithName("GEAR_BlueFlare");

				var droppedItem = itemToDrop?.Drop(1, true, false, true);

				if (droppedItem != null &&
					!Settings.ModSettings.AllowTorchPickup)
					droppedItem.enabled = false;
			}

			string alert;
			if (redeem == null)
				alert = $"torch dropped";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
