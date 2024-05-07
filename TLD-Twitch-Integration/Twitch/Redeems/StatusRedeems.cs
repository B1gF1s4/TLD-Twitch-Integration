using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class StatusRedeems
	{
		public static CustomReward Help = new()
		{
			Title = RedeemNames.STATUS_HELP,
			Prompt = "Set specified meter to help setting value. Possible inputs: cold, fatigue, thirst, hunger",
			Cost = 200,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = true
		};

		public static CustomReward Harm = new()
		{
			Title = RedeemNames.STATUS_HARM,
			Prompt = "Set specified meter to harm setting value. Possible inputs: cold, fatigue, thirst, hunger",
			Cost = 1000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = true
		};

		public static CustomReward CabinFever = new()
		{
			Title = RedeemNames.STATUS_CABIN_FEVER,
			Prompt = "Gives the player cabin fever.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Dysentery = new()
		{
			Title = RedeemNames.STATUS_DYSENTERY,
			Prompt = "Gives the player dysentery.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward FoodPoisoning = new()
		{
			Title = RedeemNames.STATUS_FOOD_POISONING,
			Prompt = "Gives the player food poisoning.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Hypothermia = new()
		{
			Title = RedeemNames.STATUS_HYPOTHERMIA,
			Prompt = "Gives the player hypothermia.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Bleeding = new()
		{
			Title = RedeemNames.STATUS_BLEED,
			Prompt = "Gives the player the bleeding affliction on specified location. Possible inputs: handleft, handright, footleft, footright",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward Sprain = new()
		{
			Title = RedeemNames.STATUS_SPRAIN,
			Prompt = "Gives the player a sprain on specified location. Possible inputs: (handleft), (handright), footleft, footright",
			Cost = 1000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward Stink = new()
		{
			Title = RedeemNames.STATUS_STINK,
			Prompt = "Gives the player stink.",
			Cost = 5000,
			Color = RedeemColors.ANIMAL,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
