using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdStatusAffliction : CommandBase
	{
		private readonly Random _random = new();

		public CmdStatusAffliction() : base("tti_status_affliction")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowAfflictions)
				throw new RequiresRedeemRefundException(
					"Afflictions redeem is currently disabled.");

			if (!Settings.ModSettings.AllowAfflictionCabinFever &&
				!Settings.ModSettings.AllowAfflictionDysentery &&
				!Settings.ModSettings.AllowAfflictionFoodPoisoning &&
				!Settings.ModSettings.AllowAfflictionHypothermia &&
				!Settings.ModSettings.AllowAfflictionParasites)
				throw new RequiresRedeemRefundException(
					"All afflictions are currently disabled.");


			var affliction = GetRandomEnabledAffliction();

			switch (affliction)
			{
				case AfflictionRedeemType.FoodPoisoning:
					if (!Settings.ModSettings.AllowAfflictionFoodPoisoning)
						throw new RequiresRedeemRefundException(
							"Food Poisoning is currently disabled.");

					if (GameManager.GetFoodPoisoningComponent().HasFoodPoisoning())
						throw new RequiresRedeemRefundException(
							"Player already has Food Poisoning.");

					GameService.ShouldStartFoodPoisoning = true;
					break;

				case AfflictionRedeemType.Dysentery:
					if (!Settings.ModSettings.AllowAfflictionDysentery)
						throw new RequiresRedeemRefundException(
							"Dysentery is currently disabled.");

					if (GameManager.GetDysenteryComponent().HasDysentery())
						throw new RequiresRedeemRefundException(
							"Player already has Dysentery.");

					GameService.ShouldStartDysentery = true;
					break;

				case AfflictionRedeemType.CabinFever:
					if (!Settings.ModSettings.AllowAfflictionCabinFever)
						throw new RequiresRedeemRefundException(
							"Cabin Fever is currently disabled.");

					if (GameManager.GetCabinFeverComponent().HasCabinFever())
						throw new RequiresRedeemRefundException(
							"Player already has Cabin Fever.");

					GameService.ShouldStartCabinFever = true;
					break;

				case AfflictionRedeemType.Parasites:
					if (!Settings.ModSettings.AllowAfflictionParasites)
						throw new RequiresRedeemRefundException(
							"Parasites is currently disabled.");

					if (GameManager.GetIntestinalParasitesComponent().HasIntestinalParasites())
						throw new RequiresRedeemRefundException(
							"Player already has Parasites.");

					GameService.ShouldStartParasites = true;
					break;

				case AfflictionRedeemType.Hypothermia:
					if (!Settings.ModSettings.AllowAfflictionHypothermia)
						throw new RequiresRedeemRefundException(
							"Hypothermia is currently disabled.");

					if (GameManager.GetHypothermiaComponent().HasHypothermia())
						throw new RequiresRedeemRefundException(
							"Player already has Hypothermia.");

					GameService.ShouldStartHypothermia = true;
					break;

				default:
					throw new RequiresRedeemRefundException("Afflcition not supported.");
			}

			string alert;
			if (redeem == null)
				alert = $"status harm - '{affliction}'";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' " +
					$"-> {affliction}";

			return alert;
		}

		private AfflictionRedeemType GetRandomEnabledAffliction()
		{
			Array values = Enum.GetValues(typeof(AfflictionRedeemType));
			AfflictionRedeemType affliction = (AfflictionRedeemType)
				(values.GetValue(_random.Next(values.Length)) ??
				throw new Exception("trying to cast null to enum"));

			if (IsAfflictionEnabled(affliction))
				return affliction;
			else
				return GetRandomEnabledAffliction();
		}

		private bool IsAfflictionEnabled(AfflictionRedeemType affliction)
		{
			return affliction switch
			{
				AfflictionRedeemType.Parasites => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.Hypothermia => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.Dysentery => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.FoodPoisoning => Settings.ModSettings.AllowAfflictionParasites,
				AfflictionRedeemType.CabinFever => Settings.ModSettings.AllowAfflictionParasites,
				_ => true,
			};
		}
	}
}
