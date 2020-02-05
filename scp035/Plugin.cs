using EXILED;
using Harmony;

namespace scp035
{
	public class Plugin : EXILED.Plugin
	{
		private EventHandlers EventHandlers;

		public static HarmonyInstance harmonyInstance { private set; get; }
		public static int harmonyCounter;

		private bool enabled;

		public override void OnEnable()
		{
			enabled = Config.GetBool("035_enabled", true);

			if (!enabled) return;

			harmonyCounter++;
			harmonyInstance = HarmonyInstance.Create($"cyanox.scp035{harmonyCounter}");
			harmonyInstance.PatchAll();

			EventHandlers = new EventHandlers(this);

			// Register events
			Events.WaitingForPlayersEvent += EventHandlers.OnWaitingForPlayers;
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.PickupItemEvent += EventHandlers.OnPickupItem;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.RoundRestartEvent += EventHandlers.OnRoundRestart;
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
