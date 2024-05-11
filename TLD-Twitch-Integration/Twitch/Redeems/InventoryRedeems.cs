using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public class InventoryRedeems
	{
		public static CustomReward TeamNoPants = new()
		{
			Title = RedeemNames.INVENTORY_NO_PANTS,
			Prompt = "Drops all pants equipped.",
			Cost = 1,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward DropTorch = new()
		{
			Title = RedeemNames.INVENTORY_DROP_TORCH,
			Prompt = "Makes the player drop the torch or flare in hand, or best in inventory, if the player is not holding one.",
			Cost = 1,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward DropItem = new()
		{
			Title = RedeemNames.INVENTORY_DROP_ITEM,
			Prompt = "Drops a random item from inventory. It cannot be picked back up.",
			Cost = 1,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward SteppedStim = new()
		{
			Title = RedeemNames.INVENTORY_STEPPED_STIM,
			Prompt = "Starts the stim effect, without consuming one.",
			Cost = 1,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Bow = new()
		{
			Title = RedeemNames.INVENTORY_BOW,
			Prompt = "Gives the player the bow and some arrows.",
			Cost = 1,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
