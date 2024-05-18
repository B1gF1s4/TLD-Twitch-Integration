using Il2Cpp;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdWeatherTime : CommandBase
	{
		public CmdWeatherTime() : base("tti_weather_time")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowTime)
				throw new RequiresRedeemRefundException(
					"Day/Night Toggle redeem is currently disabled.");

			GameManager.m_TimeOfDay.SetNormalizedTime(
				GameManager.m_TimeOfDay.GetNormalizedTime() + 0.5f, true);

			string alert;
			if (redeem == null)
				alert = $"day/night toggled";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
