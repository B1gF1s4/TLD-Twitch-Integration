using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdInventoryDropItem : CommandBase
	{

		private readonly Random _random = new();
		private int _dropAttempts = 0;

		public CmdInventoryDropItem() : base("tti_inventory_random")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowDropItem)
				throw new RequiresRedeemRefundException(
					"Drop item redeem is currently disabled.");

			if (GameService.IsMenuOpen())
				return "";

			if (GameService.GearItems.Count <= 0)
				throw new RequiresRedeemRefundException(
					"Player has no items in inventory.");

			var droppedItem = TryDropItem();

			GameService.PlayPlayerSound("PLAY_VOBURNREMINDER");

			if (!Settings.ModSettings.AllowItemPickup)
				droppedItem.enabled = false;

			string alert;
			if (redeem == null)
				alert = $"random item dropped";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {droppedItem.DisplayName}";

			return alert;
		}

		private GearItem TryDropItem()
		{
			if (_dropAttempts >= 5)
				throw new RequiresRedeemRefundException(
					"Cant drop item from inventory.");

			_dropAttempts++;

			var gearItem = GameService.GearItems[_random.Next(
				0, GameService.GearItems.Count - 1)];

			if (gearItem.name == "GEAR_WaterSupplyPotable" ||
				gearItem.name == "GEAR_WaterSupplyNotPotable")
				return TryDropItem();

			GearItem droppedItem;

			if (gearItem.m_StackableItem != null)
				droppedItem = gearItem.Drop(
					gearItem.m_StackableItem.m_Units, true, false, true);

			else
				droppedItem = gearItem.Drop(1, true, false, true);

			return droppedItem;
		}
	}
}
