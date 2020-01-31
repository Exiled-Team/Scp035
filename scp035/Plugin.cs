using EXILED;
using Harmony;

namespace scp035
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;

		private bool enabled;

		public override void OnEnable()
		{
			HarmonyInstance.Create("scp035").PatchAll();

			enabled = Config.GetBool("035_enabled", true);

			if (!enabled) return;

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent += EventHandlers.OnPlayerDie;
			Events.PlayerHurtEvent += EventHandlers.OnPlayerHurt;
			Events.PocketDimEnterEvent += EventHandlers.OnPocketDimensionEnter;
			Events.CheckRoundEndEvent += EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent += EventHandlers.OnCheckEscape;
			Events.SetClassEvent += EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent += EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent += EventHandlers.OnContain106;
			Events.GeneratorInsertedEvent += EventHandlers.OnInsertTablet;
			Events.PocketDimDeathEvent += EventHandlers.OnPocketDimensionDie;
			Events.ShootEvent += EventHandlers.OnShoot;
		}

		public override void OnDisable()
		{
			// Unregister events
			Events.WaitingForPlayersEvent -= EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.PickupItemEvent -= EventHandlers.OnPickupItem;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.PlayerDeathEvent -= EventHandlers.OnPlayerDie;
			Events.PlayerHurtEvent -= EventHandlers.OnPlayerHurt;
			Events.PocketDimEnterEvent -= EventHandlers.OnPocketDimensionEnter;
			Events.CheckRoundEndEvent -= EventHandlers.OnCheckRoundEnd;
			Events.CheckEscapeEvent -= EventHandlers.OnCheckEscape;
			Events.SetClassEvent -= EventHandlers.OnSetClass;
			Events.PlayerLeaveEvent -= EventHandlers.OnPlayerLeave;
			Events.Scp106ContainEvent -= EventHandlers.OnContain106;
			Events.GeneratorInsertedEvent -= EventHandlers.OnInsertTablet;
			Events.PocketDimDeathEvent -= EventHandlers.OnPocketDimensionDie;
			Events.ShootEvent -= EventHandlers.OnShoot;

			EventHandlers = null;
		}

		public override void OnReload() { }

		public override string getName { get; } = "SCP-035";
	}
}
