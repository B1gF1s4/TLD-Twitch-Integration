using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public class InventoryRedeems
	{
		public static CustomReward TeamNoPants = new()
		{
			Title = RedeemNames.INVENTORY_NO_PANTS,
			Prompt = "Drops all pants equipped.",
			Cost = 400,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward DropTorch = new()
		{
			Title = RedeemNames.INVENTORY_DROP_TORCH,
			Prompt = "Drops the torch or flare in hand, or best in inventory, if not holding one.",
			Cost = 400,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward DropItem = new()
		{
			Title = RedeemNames.INVENTORY_DROP_ITEM,
			Prompt = "Drops a random item from inventory.",
			Cost = 1500,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward SteppedStim = new()
		{
			Title = RedeemNames.INVENTORY_STEPPED_STIM,
			Prompt = "Applies stim immediately.",
			Cost = 1500,
			Color = RedeemColors.INVENTORY,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Weapon = new()
		{
			Title = RedeemNames.INVENTORY_WEAPON,
			Prompt = "Gives a weapon. Possible inputs: bow, rifle, revolver, flaregun",
			Cost = 200,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward Bandage = new()
		{
			Title = RedeemNames.INVENTORY_BANDAGE,
			Prompt = "Gives 2 bandages.",
			Cost = 200,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
