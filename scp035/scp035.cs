using Exiled.API.Features;

namespace scp035
{
	public class scp035 : Plugin<Config>
	{
		// todo
		// fix tags when 035 is assigned to a player with a tag

		internal static scp035 instance;

		private HarmonyLib.Harmony hInstance;

		private EventHandlers ev;

		public override void OnEnabled()
		{
			base.OnEnabled();

			instance = this;

			hInstance = new HarmonyLib.Harmony($"cyanox.scp035");
			hInstance.PatchAll();

			ev = new EventHandlers(this);

			Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
			Exiled.Events.Handlers.Player.PickingUpItem += ev.OnPickupItem;
			Exiled.Events.Handlers.Server.RoundEnded += ev.OnRoundEnd;
			Exiled.Events.Handlers.Player.Died += ev.OnPlayerDie;
			Exiled.Events.Handlers.Player.Hurting += ev.OnPlayerHurt;
			Exiled.Events.Handlers.Player.EnteringPocketDimension += ev.OnPocketDimensionEnter;
			Exiled.Events.Handlers.Server.EndingRound += ev.OnCheckRoundEnd;
			Exiled.Events.Handlers.Player.Escaping += ev.OnCheckEscape;
			Exiled.Events.Handlers.Player.ChangingRole += ev.OnSetClass;
			Exiled.Events.Handlers.Player.Left += ev.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing += ev.OnContain106;
			Exiled.Events.Handlers.Player.InsertingGeneratorTablet += ev.OnInsertTablet;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension += ev.OnPocketDimensionDie;
			Exiled.Events.Handlers.Player.Shooting += ev.OnShoot;
			Exiled.Events.Handlers.Player.UsingMedicalItem += ev.OnUseMedicalItem;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
			Exiled.Events.Handlers.Player.PickingUpItem -= ev.OnPickupItem;
			Exiled.Events.Handlers.Server.RoundEnded -= ev.OnRoundEnd;
			Exiled.Events.Handlers.Player.Died -= ev.OnPlayerDie;
			Exiled.Events.Handlers.Player.Hurting -= ev.OnPlayerHurt;
			Exiled.Events.Handlers.Player.EnteringPocketDimension -= ev.OnPocketDimensionEnter;
			Exiled.Events.Handlers.Server.EndingRound -= ev.OnCheckRoundEnd;
			Exiled.Events.Handlers.Player.Escaping -= ev.OnCheckEscape;
			Exiled.Events.Handlers.Player.ChangingRole -= ev.OnSetClass;
			Exiled.Events.Handlers.Player.Left -= ev.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing -= ev.OnContain106;
			Exiled.Events.Handlers.Player.InsertingGeneratorTablet -= ev.OnInsertTablet;
			Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= ev.OnPocketDimensionDie;
			Exiled.Events.Handlers.Player.Shooting -= ev.OnShoot;
			Exiled.Events.Handlers.Player.UsingMedicalItem -= ev.OnUseMedicalItem;

			hInstance.UnpatchAll();

			ev = null;
		}

		public override string Name => "scp035";
	}
}
