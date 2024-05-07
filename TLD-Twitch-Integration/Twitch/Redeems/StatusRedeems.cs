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

		public static CustomReward Afflictions = new()
		{
			Title = RedeemNames.STATUS_AFFLICTION,
			Prompt = "Gives a random affliction. Possible outcomes: foodpoisoning, dysentery, cabinfever, parasites, hypothermia",
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

		public static CustomReward Fristbite = new()
		{
			Title = RedeemNames.STATUS_FROSTBITE,
			Prompt = "Gives the player frostbite at a random body part.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
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
