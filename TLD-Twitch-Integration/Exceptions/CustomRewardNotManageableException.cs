namespace TLD_Twitch_Integration.Exceptions
{
	public class CustomRewardNotManageableException : Exception
	{
		public CustomRewardNotManageableException(string rewardName) : base($"this custom reward is not manageable by the twitch api. " +
			$"this mostlikely means it has been created on the dashboard and not by the mod itself and now causes naming conflicts. " +
			$"please go to your dashboard and delete the reward manually. name: {rewardName}")
		{ }
	}
}
