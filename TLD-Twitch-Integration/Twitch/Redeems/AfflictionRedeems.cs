using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class AfflictionRedeems
	{
		public static CustomReward CabinFever = new()
		{
			Title = RedeemNames.AFFLICTION_CABIN_FEVER,
			Prompt = "Gives the player cabin fever",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Dysentery = new()
		{
			Title = RedeemNames.AFFLICTION_DYSENTERY,
			Prompt = "Gives the player dysentery",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward FoodPoisoning = new()
		{
			Title = RedeemNames.AFFLICTION_FOOD_POISONING,
			Prompt = "Gives the player food poisoning",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Hypothermia = new()
		{
			Title = RedeemNames.AFFLICTION_HYPOTHERMIA,
			Prompt = "Gives the player hypothermia",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Parasites = new()
		{
			Title = RedeemNames.AFFLICTION_PARASITES,
			Prompt = "Gives the player parasites",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
