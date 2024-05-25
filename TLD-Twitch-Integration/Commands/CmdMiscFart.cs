﻿using Il2Cpp;
using MelonLoader;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch.Models;

namespace TLD_Twitch_Integration.Commands
{
	public class CmdMiscFart : CommandBase
	{

		public const int Duration = 8;

		public CmdMiscFart() : base("tti_misc_fart")
		{ }

		public override string Execute(Redemption? redeem = null)
		{
			if (!Settings.ModSettings.AllowMiscFart)
				throw new RequiresRedeemRefundException(
					"Fart redeem is currently disabled.");

			GameService.FartStart = DateTime.UtcNow;
			GameService.IsFartActive = true;

			var clip = GameService.ClipManager?.GetClip("fart");

			if (clip == null)
				Melon<Mod>.Logger.Msg("clip not found");

			if (clip != null)
				GameService.VoiceSource?.Play(clip);

			GameManager.GetSuffocatingComponent().ApplySuffocatingVisualEffect();
			//GameService.PlayPlayerSound("PLAY_SUFFOCATIONCOUGH");

			GameManager.GetChemicalPoisoningComponent().m_InHazardZone = true;
			GameManager.GetChemicalPoisoningComponent().m_ActiveZones = 1;

			string alert;
			if (redeem == null)
				alert = $"farting";
			else
				alert = $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

			return alert;
		}
	}
}
