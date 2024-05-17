using Il2Cpp;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public abstract class CommandBase
	{
		private readonly string _command;

		public CommandBase(string command)
		{
			_command = command;
		}

		public void AddToConsole()
		{
			uConsole.RegisterCommand(_command, new Action(() =>
			{
				var alert = Execute();
				uConsole.print(alert);
			}));
		}

		public abstract string Execute(Redemption? redeem = null);
	}
}
