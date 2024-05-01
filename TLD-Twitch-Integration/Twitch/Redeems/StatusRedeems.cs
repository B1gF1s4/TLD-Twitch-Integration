using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class StatusRedeems
	{
		public static CustomReward Tired = new()
		{
			Title = RedeemNames.STATUS_TIRED,
			Prompt = "Sets the games fatigue meter to 10%",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false
		};

		public static CustomReward Hungry = new()
		{
			Title = RedeemNames.STATUS_HUNGRY,
			Prompt = "Sets the games hunger meter to 10%",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false
		};

		public static CustomReward Thirsty = new()
		{
			Title = RedeemNames.STATUS_THIRSTY,
			Prompt = "Sets the games thirst meter to 10%",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false
		};

		public static CustomReward Freezing = new()
		{
			Title = RedeemNames.STATUS_FREEZING,
			Prompt = "Sets the games cold meter to 10%",
			Cost = 1,
			Color = RedeemColors.STATUS_HARM,
			IsEnabled = false
		};

		public static CustomReward Awake = new()
		{
			Title = RedeemNames.STATUS_AWAKE,
			Prompt = "Sets the games fatigue meter to 90%",
			Cost = 1,
			Color = RedeemColors.STATUS_HELP,
			IsEnabled = false
		};

		public static CustomReward Full = new()
		{
			Title = RedeemNames.STATUS_FULL,
			Prompt = "Sets the games hunger meter to 90%",
			Cost = 1,
			Color = RedeemColors.STATUS_HELP,
			IsEnabled = false
		};

		public static CustomReward NotThirsty = new()
		{
			Title = RedeemNames.STATUS_NOT_THIRSTY,
			Prompt = "Sets the games thirst meter to 90%",
			Cost = 1,
			Color = RedeemColors.STATUS_HELP,
			IsEnabled = false
		};

		public static CustomReward Warm = new()
		{
			Title = RedeemNames.STATUS_WARM,
			Prompt = "Sets the games warm meter to 90%",
			Cost = 1,
			Color = RedeemColors.STATUS_HELP,
			IsEnabled = false
		};
	}
}
