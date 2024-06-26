﻿using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class AnimalRedeems
	{
		public static CustomReward TWolves = new()
		{
			Title = RedeemNames.ANIMAL_T_WOLVES,
			Prompt = "Spawns a pack of t-wolves. Input a number between 2 - 5.",
			Cost = 5000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = true,
			IsCooldownEnabled = true,
			CooldownInSeconds = 600,
		};

		public static CustomReward BigGame = new()
		{
			Title = RedeemNames.ANIMAL_BIG_GAME,
			Prompt = "Spawns big game from input. Possible inputs: bear, moose",
			Cost = 2500,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = true,
			IsCooldownEnabled = true,
			CooldownInSeconds = 600,
		};

		public static CustomReward StalkingWolf = new()
		{
			Title = RedeemNames.ANIMAL_STALKING_WOLF,
			Prompt = "Spawns a wolf behind the player.",
			Cost = 2000,
			Color = RedeemColors.HARM,
			IsEnabled = false,
			IsUserInputRequired = false,
			IsCooldownEnabled = true,
			CooldownInSeconds = 600,
		};

		public static CustomReward BunnyExplosion = new()
		{
			Title = RedeemNames.ANIMAL_BUNNY_EXPLOSION,
			Prompt = "Spawns all ze bunnies.",
			Cost = 600,
			Color = RedeemColors.HELP,
			IsEnabled = false,
			IsUserInputRequired = false,
			IsCooldownEnabled = true,
			CooldownInSeconds = 600,
		};
	}
}
