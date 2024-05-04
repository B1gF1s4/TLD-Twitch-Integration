using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class AnimalRedeems
	{
		public static CustomReward TWolves = new()
		{
			Title = RedeemNames.ANIMAL_T_WOLVES,
			Prompt = "Spawns a pack of 5 t-wolves.",
			Cost = 1,
			Color = RedeemColors.ANIMAL_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Bear = new()
		{
			Title = RedeemNames.ANIMAL_BEAR,
			Prompt = "Spawns a bear.",
			Cost = 1,
			Color = RedeemColors.ANIMAL_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward Moose = new()
		{
			Title = RedeemNames.ANIMAL_MOOSE,
			Prompt = "Spawns a moose.",
			Cost = 1,
			Color = RedeemColors.ANIMAL_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward StalkingWolf = new()
		{
			Title = RedeemNames.ANIMAL_STALKING_WOLF,
			Prompt = "Spawns a wolf behind the player.",
			Cost = 1,
			Color = RedeemColors.ANIMAL_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward BunnyExplosion = new()
		{
			Title = RedeemNames.ANIMAL_BUNNY_EXPLOSION,
			Prompt = "Spawns all the bunnies.",
			Cost = 1,
			Color = RedeemColors.ANIMAL_HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

	}
}
