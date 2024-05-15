using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public static class CmdInventoryDropItem
	{

		private static Random random = new();

		public static void AddCommandToConsole()
		{
			uConsole.RegisterCommand("tti_inventory_random", new Action(() =>
			{
				Execute();
			}));
		}

		public static string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowSteppedStim)
				throw new RequiresRedeemRefundException(
					"Drop item redeem is currently disabled.");

			if (GameService.IsMenuOpen())
				return "";

			if (GameService.GearItems.Count <= 0)
				throw new RequiresRedeemRefundException(
					"Player has no items in inventory.");

			var gearItem =
				GameService.GearItems[random.Next(0, GameService.GearItems.Count - 1)];

			var droppedItem = gearItem.Drop(1, true, false, true);

			if (!Settings.ModSettings.AllowItemPickup)
				droppedItem.enabled = false;

			string alert;
			if (redeem == null)
				alert = $"random item dropped";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {gearItem.name}";

			return alert;
		}
	}
}
