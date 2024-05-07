using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Twitch.Redeems
{
	public static class RedeemDefaults
	{

		private static readonly Dictionary<string, CustomReward> _defaults = new()
		{
			{ RedeemNames.WEATHER_HELP, WeatherRedeems.Help },
			{ RedeemNames.WEATHER_HARM, WeatherRedeems.Harm },
			{ RedeemNames.WEATHER_AURORA, WeatherRedeems.Aurora },

			{ RedeemNames.ANIMAL_T_WOLVES, AnimalRedeems.TWolves },
			{ RedeemNames.ANIMAL_BIG_GAME, AnimalRedeems.BigGame },
			{ RedeemNames.ANIMAL_STALKING_WOLF, AnimalRedeems.StalkingWolf },
			{ RedeemNames.ANIMAL_BUNNY_EXPLOSION, AnimalRedeems.BunnyExplosion },

			{ RedeemNames.STATUS_HELP, StatusRedeems.Help },
			{ RedeemNames.STATUS_HARM, StatusRedeems.Harm },
			{ RedeemNames.STATUS_AFFLICTION, StatusRedeems.Afflictions },
			{ RedeemNames.STATUS_BLEED, StatusRedeems.Bleeding },
			{ RedeemNames.STATUS_SPRAIN, StatusRedeems.Sprain },
			{ RedeemNames.STATUS_FROSTBITE, StatusRedeems.Fristbite },
			{ RedeemNames.STATUS_STINK, StatusRedeems.Stink },

			{ RedeemNames.INVENTORY_NO_PANTS, InventoryRedeems.TeamNoPants },
			{ RedeemNames.INVENTORY_DROP_TORCH, InventoryRedeems.DropTorch },
			{ RedeemNames.INVENTORY_DROP_ITEM, InventoryRedeems.DropItem },
			{ RedeemNames.INVENTORY_STEPPED_STIM, InventoryRedeems.SteppedStim },
			{ RedeemNames.INVENTORY_BOW, InventoryRedeems.Bow },

			{ RedeemNames.MISC_TELEPORT, MiscRedeems.Teleport },
			{ RedeemNames.MISC_TIME, MiscRedeems.Time },

			{ RedeemNames.SOUND_420, SoundRedeems.Happy420 },

			{ RedeemNames.DEV_SOUND, DevRedeems.SoundCheck },
		};

		public static CustomReward GetRedeemDefault(string redeemName)
		{
			var redeem = _defaults[redeemName] ??
				throw new NotFoundException();

			return redeem;
		}

	}
}
