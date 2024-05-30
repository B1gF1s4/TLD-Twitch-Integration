using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdInventoryDropItem : CommandBase
	{

		private readonly Random _random = new();

		public CmdInventoryDropItem() : base("tti_inventory_random")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowDropItem)
				throw new RequiresRedeemRefundException(
					"Drop item redeem is currently disabled.");

			if (GameService.IsMenuOpen())
				return "";

			// filter out water, since it causes crash when selected to drop
			// TODO: fix drop water supply
			var filter = (GearItem gi) =>
			{
				return !string.IsNullOrEmpty(gi.name) &&
				gi.m_NarrativeCollectibleItem == null &&
				gi.name != "GEAR_WaterSupplyPotable" &&
				gi.name != "GEAR_WaterSupplyNotPotable";
			};

			var list = new Il2CppSystem.Collections.Generic.List<GearItem>();
			GameManager.GetInventoryComponent().GetGearItems(filter, list);

			if (list.Count <= 0)
				throw new RequiresRedeemRefundException(
					"Player has no items in inventory that can be dropped.");

			var gearItem = list[_random.Next(0, list.Count)];

			GearItem droppedItem;
			if (gearItem.m_StackableItem != null)
			{
				droppedItem = gearItem.Drop(
					gearItem.m_StackableItem.m_Units, true, false, true);
			}
			else
			{
				droppedItem = gearItem.Drop(1, true, false, true);
			}

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

	}
}
