using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdInventoryBandage : CommandBase
	{
		public CmdInventoryBandage() : base("tti_inventory_bandage")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			throw new RequiresRedeemRefundException("Redeem not implemented yet");

			//if (!Settings.ModSettings.AllowBandage)
			//	throw new RequiresRedeemRefundException(
			//		"Bandage redeem is currently disabled.");

			//if (GameService.IsMenuOpen())
			//	return "";


			//var gearItem = GearItem.LoadGearItemPrefab("GEAR_HeavyBandage") ??
			//	throw new RequiresRedeemRefundException("Error loading prefab.");

			//gearItem.SetNormalizedHP(1.0f);
			//gearItem.m_StackableItem.m_Units = 2;

			//GameManager.GetInventoryComponent().AddGear(gearItem, true);

			//string alert;
			//if (redeem == null)
			//	alert = $"2 bandages added";
			//else
			//	alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			//return alert;
		}
	}
}
