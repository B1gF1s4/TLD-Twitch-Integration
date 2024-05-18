using Il2Cpp;
using MelonLoader;
using TLD_Twitch_Integration.Commands;
using TLD_Twitch_Integration.Exceptions;
using TLD_Twitch_Integration.Game;
using TLD_Twitch_Integration.Twitch;
using TLD_Twitch_Integration.Twitch.Models;
using TLD_Twitch_Integration.Twitch.Redeems;

namespace TLD_Twitch_Integration
{
	public class ExecutionService
	{
		public static bool ExecutionPending { get; set; }
		public static List<Redemption> ExecutionQueue { get; set; } = new();


		private const int _interval = 8;

		private static DateTime _lastUpdated;

		public enum StatusMeter
		{
			None,
			Cold,
			Fatigue,
			Thirst,
			Hunger,
		}

		public enum AnimalRedeemType
		{
			None,
			TWolves,
			Bear,
			Moose,
			StalkingWolf,
			BunnyExplosion
		}

		public enum AfflictionRedeemType
		{
			FoodPoisoning,
			Dysentery,
			CabinFever,
			Parasites,
			Hypothermia
		}

		public static async Task OnUpdate()
		{
			if (!Mod.ShouldUpdate(_lastUpdated, _interval))
				return;

			_lastUpdated = DateTime.UtcNow;

			if (!AuthService.IsConnected)
				return;

			if (!Settings.ModSettings.Enabled)
				return;

			if (string.IsNullOrEmpty(GameManager.m_ActiveScene) ||
				GameManager.m_ActiveScene == "MainMenu" ||
				GameManager.m_ActiveScene == "Boot" ||
				GameManager.m_ActiveScene == "Empty")
				return;

			if (GameManager.m_IsPaused)
				return;

			if (Utils.IsSceneTransition())
				return;

			if (!RedemptionService.HasOpenRedeems)
				return;

			await HandleNextRedeem();

		}

		private static async Task HandleNextRedeem()
		{
			var userId = AuthService.User?.Id ??
				throw new NotLoggedInException();

			var redeemToExecute = ExecutionQueue.Any() ?
				ExecutionQueue.FirstOrDefault() :
				RedemptionService.OpenRedeems
					.OrderBy(r => r.RedeemedAt)
					.ToList()
					.FirstOrDefault();

			if (redeemToExecute == null)
				return;

			ExecutionPending = true;

			await TryExecuteRedeem(redeemToExecute, userId);

			ExecutionPending = false;
		}

		private static async Task TryExecuteRedeem(Redemption redeem, string userId)
		{
			GameService.Update();

			var defaultTitle = Settings.Redeems.GetRedeemNameById(redeem.CustomReward?.Id!);
			Melon<Mod>.Logger.Msg($"trying to execute redeem {defaultTitle}");

			var redeemMessage = "";
			try
			{
				redeemMessage = ExecuteRedeem(redeem, defaultTitle);
			}
			catch (RequiresRedeemRefundException refund)
			{
				await UpdateRedemption(userId, redeem, false);

				Melon<Mod>.Logger.Msg($"'{redeem.CustomReward?.Title}' redeemed by {redeem.UserName} refunded. " +
					$"- {refund.Message}");

				if (Settings.ModSettings.ShowAlert)
					HUDMessage.AddMessage($"{redeem.UserName}'s Redeem refunded. -> {refund.Message}",
						_interval - 1, true, true);

				return;
			}

			if (string.IsNullOrEmpty(redeemMessage))
			{
				Melon<Mod>.Logger.Msg($"redeem skipped, trying next");

				ExecutionQueue.Add(redeem);
				RedemptionService.OpenRedeems.RemoveAll(r => r.Id == redeem.Id);

				var nextRedeemToExecute = RedemptionService.OpenRedeems
					.OrderBy(r => r.RedeemedAt)
					.ToList()
					.FirstOrDefault();

				if (nextRedeemToExecute == null)
					return;

				await TryExecuteRedeem(nextRedeemToExecute, userId);
			}
			else
			{
				Melon<Mod>.Logger.Msg($"redeem executed, removing from redemption queue on twitch");

				if (Settings.ModSettings.ShowAlert)
					HUDMessage.AddMessage(redeemMessage, _interval - 1, true, true);

				await UpdateRedemption(userId, redeem, true);
			}
		}

		private static async Task UpdateRedemption(string userId, Redemption redeem, bool fulfill)
		{
			try
			{
				if (fulfill)
					await TwitchAdapter.FulfillRedemption(AuthService.ClientId, Settings.Token.Access, userId, redeem);
				else
					await TwitchAdapter.CancelRedemption(AuthService.ClientId, Settings.Token.Access, userId, redeem);

				ExecutionQueue.RemoveAll(r => r.Id == redeem.Id);
				RedemptionService.OpenRedeems.RemoveAll(r => r.Id == redeem.Id);
			}
			catch (InvalidTokenException)
			{
				await AuthService.RefreshToken();
			}
			catch (Exception ex)
			{
				Melon<Mod>.Logger.Error(ex);
			}
		}

		private static string ExecuteRedeem(Redemption redeem, string defaultTitle)
		{
			switch (defaultTitle)
			{
				case RedeemNames.WEATHER_HELP:
					return ExecuteWeatherHelpRedeem(redeem);

				case RedeemNames.WEATHER_HARM:
					return ExecuteWeatherHarmRedeem(redeem);

				case RedeemNames.WEATHER_AURORA:
					return ExecuteWeatherAurora(redeem);

				case RedeemNames.ANIMAL_T_WOLVES:
					return CommandDefaults.CmdAnimalTWolves.Execute(redeem);

				case RedeemNames.ANIMAL_BIG_GAME:
					return CommandDefaults.CmdAnimalBigGame.Execute(redeem);

				case RedeemNames.ANIMAL_STALKING_WOLF:
					return CommandDefaults.CmdAnimalStalkingWolf.Execute(redeem);

				case RedeemNames.ANIMAL_BUNNY_EXPLOSION:
					return CommandDefaults.CmdAnimalBunnyExplosion.Execute(redeem);

				case RedeemNames.STATUS_HELP:
					return ExecuteStatusHelpRedeem(redeem);

				case RedeemNames.STATUS_HARM:
					return ExecuteStatusHarmRedeem(redeem);

				case RedeemNames.STATUS_AFFLICTION:
					return ExecuteStatusAfflictionRedeem(redeem);

				case RedeemNames.STATUS_BLEED:
					return ExecuteBleedingRedeem(redeem);

				case RedeemNames.STATUS_SPRAIN:
					return ExecuteSprainRedeem(redeem);

				case RedeemNames.STATUS_FROSTBITE:
					return ExecuteFrostbiteRedeem(redeem);

				case RedeemNames.STATUS_STINK:
					return ExecuteStinkRedeem(redeem);

				case RedeemNames.INVENTORY_NO_PANTS:
					return CommandDefaults.CmdInventoryNoPants.Execute(redeem);

				case RedeemNames.INVENTORY_DROP_TORCH:
					return CommandDefaults.CmdInventoryDropTorch.Execute(redeem);

				case RedeemNames.INVENTORY_WEAPON:
					return CommandDefaults.CmdInventoryWeapon.Execute(redeem);

				case RedeemNames.INVENTORY_STEPPED_STIM:
					return CommandDefaults.CmdInventoryStim.Execute(redeem);

				case RedeemNames.INVENTORY_DROP_ITEM:
					return CommandDefaults.CmdInventoryDropItem.Execute(redeem);

				case RedeemNames.WEATHER_TIME:
					return ExecuteTimeRedeem(redeem);

				case RedeemNames.SOUND_420:
					GameService.PlayPlayerSound("PLAY_SUFFOCATIONCOUGH");
					return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

				case RedeemNames.DEV_SOUND:
					GameService.PlayPlayerSound(redeem.UserInput!);
					return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";

				default:
					Melon<Mod>.Logger.Error($"redeem operation not supported - {defaultTitle}");
					throw new RequiresRedeemRefundException("Redeem not found.");
			}
		}

		private static string ExecuteWeatherHelpRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowWeatherHelp)
				throw new RequiresRedeemRefundException("Weather help redeem is currently disabled.");

			if (!Settings.ModSettings.AllowWeatherHelpClear &&
				!Settings.ModSettings.AllowWeatherHelpCloudy &&
				!Settings.ModSettings.AllowWeatherHelpFog &&
				!Settings.ModSettings.AllowWeatherHelpSnow)
				throw new RequiresRedeemRefundException("All weather types for the weather help redeem are currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			var defaultWeatherHelp = Settings.ModSettings.AllowWeatherHelpClear ? WeatherStage.Clear :
				Settings.ModSettings.AllowWeatherHelpFog ? WeatherStage.LightFog :
				Settings.ModSettings.AllowWeatherHelpSnow ? WeatherStage.LightSnow : WeatherStage.PartlyCloudy;

			var userInputWeatherHelp = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultWeatherHelp.ToString().ToLower() : redeem.UserInput.ToLower();

			var weatherHelpToSet = userInputWeatherHelp.Contains("clear") ? WeatherStage.Clear :
				userInputWeatherHelp.Contains("fog") ? WeatherStage.LightFog :
				userInputWeatherHelp.Contains("snow") ? WeatherStage.LightSnow :
				userInputWeatherHelp.Contains("cloudy") ? WeatherStage.PartlyCloudy : defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.Clear && !Settings.ModSettings.AllowWeatherHelpClear)
				weatherHelpToSet = defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.LightFog && !Settings.ModSettings.AllowWeatherHelpFog)
				weatherHelpToSet = defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.LightSnow && !Settings.ModSettings.AllowWeatherHelpSnow)
				weatherHelpToSet = defaultWeatherHelp;

			if (weatherHelpToSet == WeatherStage.PartlyCloudy && !Settings.ModSettings.AllowWeatherHelpCloudy)
				weatherHelpToSet = defaultWeatherHelp;

			GameService.WeatherToChange = weatherHelpToSet;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> {weatherHelpToSet}";
		}

		private static string ExecuteWeatherHarmRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowWeatherHarm)
				throw new RequiresRedeemRefundException("Weather harm redeem is currently disabled.");

			if (!Settings.ModSettings.AllowWeatherHarmBlizzard &&
				!Settings.ModSettings.AllowWeatherHarmFog &&
				!Settings.ModSettings.AllowWeatherHarmSnow)
				throw new RequiresRedeemRefundException("All weather types for the weather harm redeem are currently disabled.");

			if (GameService.IsInBuilding)
				return "";

			var defaultWeatherHarm = Settings.ModSettings.AllowWeatherHarmBlizzard ? WeatherStage.Blizzard :
				Settings.ModSettings.AllowWeatherHarmFog ? WeatherStage.DenseFog : WeatherStage.HeavySnow;

			var userInputWeatherHarm = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultWeatherHarm.ToString().ToLower() : redeem.UserInput.ToLower();

			var weatherHarmToSet = userInputWeatherHarm.Contains("blizzard") ? WeatherStage.Blizzard :
				userInputWeatherHarm.Contains("fog") ? WeatherStage.DenseFog :
				userInputWeatherHarm.Contains("snow") ? WeatherStage.HeavySnow : defaultWeatherHarm;

			if (weatherHarmToSet == WeatherStage.Blizzard && !Settings.ModSettings.AllowWeatherHarmBlizzard)
				weatherHarmToSet = defaultWeatherHarm;

			if (weatherHarmToSet == WeatherStage.DenseFog && !Settings.ModSettings.AllowWeatherHarmFog)
				weatherHarmToSet = defaultWeatherHarm;

			if (weatherHarmToSet == WeatherStage.HeavySnow && !Settings.ModSettings.AllowWeatherHarmSnow)
				weatherHarmToSet = defaultWeatherHarm;

			GameService.WeatherToChange = weatherHarmToSet;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> {weatherHarmToSet}";
		}

		private static string ExecuteWeatherAurora(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowWeatherAurora)
				throw new RequiresRedeemRefundException("Aurora redeem is currently disabled.");

			if (GameManager.m_ActiveScene == "Dam")
				return "";

			if (GameService.IsInBuilding)
				return "";

			GameService.ShouldStartAurora = true;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";
		}

		private static string ExecuteStatusHelpRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowStatusHelp)
				throw new RequiresRedeemRefundException("Status help redeem is currently disabled.");

			if (!Settings.ModSettings.AllowStatusHelpAwake &&
				!Settings.ModSettings.AllowStatusHelpFull &&
				!Settings.ModSettings.AllowStatusHelpNotThirsty &&
				!Settings.ModSettings.AllowStatusHelpWarm)
				throw new RequiresRedeemRefundException("All meters for the status help redeem are currently disabled.");

			var defaultStatusHelp = Settings.ModSettings.AllowStatusHelpWarm ? StatusMeter.Cold :
						Settings.ModSettings.AllowStatusHelpAwake ? StatusMeter.Fatigue :
						Settings.ModSettings.AllowStatusHelpNotThirsty ? StatusMeter.Thirst : StatusMeter.Hunger;

			var userInputStatusHelp = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultStatusHelp.ToString().ToLower() : redeem.UserInput.ToLower();

			var statusHelpToSet = userInputStatusHelp.Contains("cold") ? StatusMeter.Cold :
				userInputStatusHelp.Contains("fatigue") ? StatusMeter.Fatigue :
				userInputStatusHelp.Contains("thirst") ? StatusMeter.Thirst :
				userInputStatusHelp.Contains("hunger") ? StatusMeter.Hunger : defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Cold && !Settings.ModSettings.AllowStatusHelpWarm)
				statusHelpToSet = defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Fatigue && !Settings.ModSettings.AllowStatusHelpAwake)
				statusHelpToSet = defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Thirst && !Settings.ModSettings.AllowStatusHelpNotThirsty)
				statusHelpToSet = defaultStatusHelp;

			if (statusHelpToSet == StatusMeter.Hunger && !Settings.ModSettings.AllowStatusHelpFull)
				statusHelpToSet = defaultStatusHelp;

			GameService.StatusMeterToChange = statusHelpToSet;
			GameService.IsHelpfulStatusMeterChange = true;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> set {statusHelpToSet} to {Settings.ModSettings.StatusHelpValue}";
		}

		private static string ExecuteStatusHarmRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowStatusHarm)
				throw new RequiresRedeemRefundException("Status harm redeem is currently disabled.");

			if (!Settings.ModSettings.AllowStatusHarmFreezing &&
				!Settings.ModSettings.AllowStatusHarmHungry &&
				!Settings.ModSettings.AllowStatusHarmThirsty &&
				!Settings.ModSettings.AllowStatusHarmTired)
				throw new RequiresRedeemRefundException("All meters for the status harm redeem are currently disabled.");

			var defaultStatusHarm = Settings.ModSettings.AllowStatusHarmFreezing ? StatusMeter.Cold :
						Settings.ModSettings.AllowStatusHarmTired ? StatusMeter.Fatigue :
						Settings.ModSettings.AllowStatusHarmThirsty ? StatusMeter.Thirst : StatusMeter.Hunger;

			var userInputStatusHarm = string.IsNullOrEmpty(redeem.UserInput) ?
				defaultStatusHarm.ToString().ToLower() : redeem.UserInput.ToLower();

			var statusHarmToSet = userInputStatusHarm.Contains("cold") ? StatusMeter.Cold :
				userInputStatusHarm.Contains("fatigue") ? StatusMeter.Fatigue :
				userInputStatusHarm.Contains("thirst") ? StatusMeter.Thirst :
				userInputStatusHarm.Contains("hunger") ? StatusMeter.Hunger : defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Cold && !Settings.ModSettings.AllowStatusHarmHungry)
				statusHarmToSet = defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Fatigue && !Settings.ModSettings.AllowStatusHarmTired)
				statusHarmToSet = defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Thirst && !Settings.ModSettings.AllowStatusHarmThirsty)
				statusHarmToSet = defaultStatusHarm;

			if (statusHarmToSet == StatusMeter.Hunger && !Settings.ModSettings.AllowStatusHarmHungry)
				statusHarmToSet = defaultStatusHarm;

			GameService.StatusMeterToChange = statusHarmToSet;
			GameService.IsHelpfulStatusMeterChange = false;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> set {statusHarmToSet} to {Settings.ModSettings.StatusHarmValue}";
		}

		private static string ExecuteStatusAfflictionRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowAfflictions)
				throw new RequiresRedeemRefundException("Afflictions redeem is currently disabled.");

			if (!Settings.ModSettings.AllowAfflictionCabinFever &&
				!Settings.ModSettings.AllowAfflictionDysentery &&
				!Settings.ModSettings.AllowAfflictionFoodPoisoning &&
				!Settings.ModSettings.AllowAfflictionHypothermia &&
				!Settings.ModSettings.AllowAfflictionParasites)
				throw new RequiresRedeemRefundException("All afflictions are currently disabled.");

			Random random = new();
			var affliction = GetRandomEnabledAffliction(random);

			switch (affliction)
			{
				case AfflictionRedeemType.FoodPoisoning:
					if (GameManager.GetFoodPoisoningComponent().HasFoodPoisoning())
						throw new RequiresRedeemRefundException("Player already has Food Poisoning.");

					GameService.ShouldStartFoodPoisoning = true;
					return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> Food Poisoning";

				case AfflictionRedeemType.Dysentery:
					if (GameManager.GetDysenteryComponent().HasDysentery())
						throw new RequiresRedeemRefundException("Player already has Dysentery.");

					GameService.ShouldStartDysentery = true;
					return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> Food Dysentery";

				case AfflictionRedeemType.CabinFever:
					if (GameManager.GetCabinFeverComponent().HasCabinFever())
						throw new RequiresRedeemRefundException("Player already has Cabin Fever.");

					GameService.ShouldStartCabinFever = true;
					return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> Food Cabin Fever";

				case AfflictionRedeemType.Parasites:
					if (GameManager.GetIntestinalParasitesComponent().HasIntestinalParasites())
						throw new RequiresRedeemRefundException("Player already has Parasites.");

					GameService.ShouldStartParasites = true;
					return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> Parasites";

				case AfflictionRedeemType.Hypothermia:
					if (GameManager.GetHypothermiaComponent().HasHypothermia())
						throw new RequiresRedeemRefundException("Player already has Hypothermia.");

					GameService.ShouldStartHypothermia = true;
					return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> Hypothermia";

				default:
					return "";
			}
		}

		private static AfflictionRedeemType GetRandomEnabledAffliction(Random random)
		{
			Array values = Enum.GetValues(typeof(AfflictionRedeemType));
			AfflictionRedeemType affliction = (AfflictionRedeemType)
				(values.GetValue(random.Next(values.Length)) ??
				throw new Exception("trying to cast null to enum"));

			if (IsAfflictionEnabled(affliction))
				return affliction;
			else
				return GetRandomEnabledAffliction(random);
		}

		private static bool IsAfflictionEnabled(AfflictionRedeemType affliction)
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

		private static string ExecuteBleedingRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowStatusBleeding)
				throw new RequiresRedeemRefundException("Bleeding redeem is currently disabled.");

			if (GameManager.GetBloodLossComponent().GetAfflictionsCount() >= 4)
				throw new RequiresRedeemRefundException("Player already has 4 or more Bleedings.");

			GameService.ShouldAddBleeding = true;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";
		}

		private static string ExecuteSprainRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowStatusSprain)
				throw new RequiresRedeemRefundException("Sprains redeem is currently disabled.");

			Random random = new();
			var isAnkle = true;
			if (Settings.ModSettings.AllowStatusSprainWrists)
				isAnkle = random.NextDouble() > 0.5;

			if (isAnkle)
			{
				if (GameManager.GetSprainedAnkleComponent().GetAfflictionsCount() >= 2)
					throw new RequiresRedeemRefundException("Player already has 2 sprained ankles.");
			}
			else
			{
				if (GameManager.GetSprainedWristComponent().GetAfflictionsCount() >= 2)
					throw new RequiresRedeemRefundException("Player already has 2 sprained wrists.");
			}

			GameService.ShouldAddSprain = true;
			GameService.SprainIsAnkle = isAnkle;

			var lastMsgPart = isAnkle ? "Ankle" : "Wrist";
			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> {lastMsgPart}";
		}

		private static string ExecuteFrostbiteRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowStatusFrostbite)
				throw new RequiresRedeemRefundException("Frostbite redeem is currently disabled.");

			if (GameManager.GetFrostbiteComponent().GetFrostbiteAfflictionCount() >= 4)
				throw new RequiresRedeemRefundException("Player already has at least 4 frostbites.");

			GameService.ShouldAddFrostbite = true;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";
		}

		private static string ExecuteStinkRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowStatusStink)
				throw new RequiresRedeemRefundException("Stink redeem is currently disabled.");

			GameService.ShouldAddStink = true;
			GameService.StinkValue = Settings.ModSettings.StinkLines;
			GameService.StinkStart = DateTime.UtcNow;

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}' -> not active for {Settings.ModSettings.StinkTime}s";
		}

		private static string ExecuteTimeRedeem(Redemption redeem)
		{
			if (!Settings.ModSettings.AllowTime)
				throw new RequiresRedeemRefundException("Time redeem is currently disabled.");

			GameManager.m_TimeOfDay.SetNormalizedTime(GameManager.m_TimeOfDay.GetNormalizedTime() + 0.5f, true);

			return $"{redeem.UserName} redeemed '{redeem.CustomReward?.Title}'";
		}
	}
}
