using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class AnimalRedeems
	{
		public static CustomReward TWolves = new()
		{
			Title = RedeemNames.ANIMAL_T_WOLVES,
			Prompt = "Spawns a pack of 2-5 t-wolves. Input a number between 2 - 5. default: 5",
			Cost = 10000,
			Color = RedeemColors.ANIMAL,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward BigGame = new()
		{
			Title = RedeemNames.ANIMAL_BIG_GAME,
			Prompt = "Spawns big game from input. Possible inputs: bear, moose",
			Cost = 5000,
			Color = RedeemColors.ANIMAL,
			IsEnabled = false,
			IsUserInputRequired = true,
		};

		public static CustomReward StalkingWolf = new()
		{
			Title = RedeemNames.ANIMAL_STALKING_WOLF,
			Prompt = "Spawns a wolf behind the player.",
			Cost = 1000,
			Color = RedeemColors.ANIMAL,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

		public static CustomReward BunnyExplosion = new()
		{
			Title = RedeemNames.ANIMAL_BUNNY_EXPLOSION,
			Prompt = "Spawns all the bunnies.",
			Cost = 1000,
			Color = RedeemColors.ANIMAL,
			IsEnabled = false,
			IsUserInputRequired = false,
		};

	}
}
