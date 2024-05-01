namespace TLD_Twitch_Integration.Exceptions
{
	public class DeserializationException : Exception
	{
		public DeserializationException(string typeName, string str) :
			base($"error deserializing to {typeName} - '{str}'")
		{ }
	}
}
