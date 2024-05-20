using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class StatusRedeems
	{
		public static CustomReward Help = new()
		{
			Title = RedeemNames.STATUS_HELP,
			Prompt = "Sets specified meter to help value. Possible inputs: cold, fatigue, thirst, hunger",
			Cost = 500,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = true
		};

		public static CustomReward Harm = new()
		{
			Title = RedeemNames.STATUS_HARM,
			Prompt = "Sets specified meter to harm value. Possible inputs: cold, fatigue, thirst, hunger",
			Cost = 500,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = true
		};

		public static CustomReward Afflictions = new()
		{
			Title = RedeemNames.STATUS_AFFLICTION,
			Prompt = "Gives a random affliction. Refunds if affliction is already applied.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward AfflictionCure = new()
		{
			Title = RedeemNames.STATUS_AFFLICTION_CURE,
			Prompt = "Cures all afflictions.",
			Cost = 5000,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Bleeding = new()
		{
			Title = RedeemNames.STATUS_BLEED,
			Prompt = "Gives the player the bleeding affliction.",
			Cost = 1000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Sprain = new()
		{
			Title = RedeemNames.STATUS_SPRAIN,
			Prompt = "Gives the player a sprain.",
			Cost = 700,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Fristbite = new()
		{
			Title = RedeemNames.STATUS_FROSTBITE,
			Prompt = "Gives the player frostbite.",
			Cost = 5000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Stink = new()
		{
			Title = RedeemNames.STATUS_STINK,
			Prompt = "Gives the player stink.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};
	}
}
