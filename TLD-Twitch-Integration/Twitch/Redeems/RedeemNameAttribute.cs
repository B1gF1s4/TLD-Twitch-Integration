namespace TLD_Twitch_Integration.Twitch.Redeems
{
	[AttributeUsage(AttributeTargets.Field)]
	public class RedeemNameAttribute : Attribute
	{
		public string RedeemName;

		public RedeemNameAttribute(string name)
		{
			RedeemName = name;
		}
	}
}
