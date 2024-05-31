using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdInventoryNoPants : CommandBase
	{

		public CmdInventoryNoPants() : base("tti_inventory_pants")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowTeamNoPants)
				throw new RequiresRedeemRefundException(
					"Team no pants redeem is currently disabled.");

			if (GameService.IsMenuOpen())
				return "";

			var player = GameManager.GetPlayerManagerComponent();

			var pantsInner = player.GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Top);
			var pantsOuter = player.GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Top2);
			var undiesInner = player.GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Mid);
			var undiesOuter = player.GetClothingInSlot(ClothingRegion.Legs, ClothingLayer.Base);

			var pants = new GearItem?[] { pantsInner, pantsOuter, undiesInner, undiesOuter };

			if (pants.Length <= 0)
				throw new RequiresRedeemRefundException(
					"Player has no pants in inventory");

			GameService.PlayPlayerSound("PLAY_CLOTHESTEARING");

			foreach (var entry in pants)
			{
				if (entry == null)
					continue;

				var droppedItem = entry.Drop(1, true, false, true);

				if (!Settings.ModSettings.AllowPantsPickup)
					droppedItem.enabled = false;
			}

			string alert;
			if (redeem == null)
				alert = $"pants dropped";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
