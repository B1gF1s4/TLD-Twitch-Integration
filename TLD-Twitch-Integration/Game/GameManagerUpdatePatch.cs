using HarmonyLib;
using Il2Cpp;
using static TLD_Twitch_Integration.ExecutionService;

namespace TLD_Twitch_Integration.Game
{
	[HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
	internal class GameManagerUpdatePatch
	{
		internal static void Prefix()
		{
			if (ExecutionPending)
				GameService.Update();
		}
	}
}
