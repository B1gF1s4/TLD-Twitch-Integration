using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdInventoryWeapon : CommandBase
	{
		public const int RifleBulletCapacity = 10;
		public const int RevolverBulletCapacity = 6;
		public const int FlaregunShellCapacity = 1;

		public CmdInventoryWeapon() : base("tti_inventory_weapon")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowWeapon)
				throw new RequiresRedeemRefundException(
					"Weapon redeem is currently disabled.");

			if (GameService.IsMenuOpen())
				return "";

			if (!Settings.ModSettings.AllowWeaponBow &&
				!Settings.ModSettings.AllowWeaponRifle &&
				!Settings.ModSettings.AllowWeaponRevolver &&
				!Settings.ModSettings.AllowWeaponFlaregun)
				throw new RequiresRedeemRefundException(
					"All weapon types for the weapon redeem are currently disabled.");

			var defaultWeapon = Settings.ModSettings.AllowWeaponBow ? "bow" :
				Settings.ModSettings.AllowWeaponRifle ? "rifle" :
				Settings.ModSettings.AllowWeaponRevolver ? "revolver" : "flaregun";

			var weaponToSpawn = defaultWeapon;

			if (redeem != null &&
				!string.IsNullOrEmpty(redeem.UserInput))
			{
				var input = redeem.UserInput.ToLower();
				weaponToSpawn = input.Contains("bow") ? "bow" :
					input.Contains("rifle") ? "rifle" :
					input.Contains("revolver") ? "revolver" :
					input.Contains("flaregun") ? "flaregun" : defaultWeapon;
			}

			var inv = GameManager.GetInventoryComponent() ??
					throw new RequiresRedeemRefundException(
						"Inventory not accessable.");

			GameService.PlayPlayerSound("PLAY_FEATUNLOCKED");

			switch (weaponToSpawn)
			{
				case "bow":
					if (!Settings.ModSettings.AllowWeaponBow)
						throw new RequiresRedeemRefundException(
							"Bow is currently disabled.");
					ConsoleManager.CONSOLE_bow();
					inv.RemoveGearFromInventory("GEAR_Arrow",
						100 - Settings.ModSettings.ArrowCount);
					break;
				case "rifle":
					if (!Settings.ModSettings.AllowWeaponRifle)
						throw new RequiresRedeemRefundException(
							"Rifle is currently disabled.");
					ConsoleManager.CONSOLE_rifle();
					inv.RemoveGearFromInventory("GEAR_RifleAmmoSingle",
						(100 + RifleBulletCapacity) - Settings.ModSettings.RifleBulletCount);
					break;
				case "revolver":
					if (!Settings.ModSettings.AllowWeaponRevolver)
						throw new RequiresRedeemRefundException(
							"Revolver is currently disabled.");
					ConsoleManager.CONSOLE_revolver();
					inv.RemoveGearFromInventory("GEAR_RevolverAmmoSingle",
						(100 + RevolverBulletCapacity) - Settings.ModSettings.RevolverBulletCount);
					break;
				case "flaregun":
					if (!Settings.ModSettings.AllowWeaponFlaregun)
						throw new RequiresRedeemRefundException(
							"Flaregun is currently disabled.");
					ConsoleManager.CONSOLE_flaregun();
					inv.RemoveGearFromInventory("GEAR_FlareGunAmmoSingle",
						(100 + FlaregunShellCapacity) - Settings.ModSettings.ShellCount);
					break;
				default:
					throw new RequiresRedeemRefundException("Error: weapon type not supported");
			}

			string alert;
			if (redeem == null)
				alert = $"{weaponToSpawn} spawned";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {weaponToSpawn}";

			return alert;
		}
	}
}
