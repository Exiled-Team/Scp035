using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using MEC;

namespace scp035
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		private Dictionary<Pickup, float> scpPickups = new Dictionary<Pickup, float>();
		internal static ReferenceHub scpPlayer;
		DateTime updateTimer;
		private bool isRoundStarted;
		private bool isRotating;
		private const float dur = 327;
		private Random rand = new Random();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			isRoundStarted = true;
			isRotating = true;
			scpPickups.Clear();
			scpPlayer = null;

			updateTimer = DateTime.Now;

			Timing.RunCoroutine(RotatePickup());
		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;
		}

		public void OnPickupItem(ref PickupItemEvent ev)
		{
			Inventory.SyncItemInfo? item = ev.Player.gameObject.GetComponent<Inventory>().items.Last();

			if (item.Value.durability == dur)
			{
				InfectPlayer(ev.Player, ev.Item);
			}
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			// to do
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId())
			{
				KillScp035();
			}
		}

		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId() && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			// to do
		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId()) ev.Allow = false;
		}

		public void OnSetClass(SetClassEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId())
			{
				KillScp035();
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId())
			{
				KillScp035(false);
			}
		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId() && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnInsertTablet(ref GeneratorInsertTabletEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId() && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId())
			{
				ev.Allow = false;
				// Teleport player to 096 room via assembly
				//ev.Player.Teleport(instance.Server.Map.GetRandomSpawnPoint(Role.SCP_096));
			}
		}

		// Make a coroutine for corrode damage
	}
}
