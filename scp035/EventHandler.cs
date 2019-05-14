using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace scp035
{
	partial class EventHandler : IEventHandlerWaitingForPlayers, IEventHandlerRoundStart, IEventHandlerPlayerPickupItemLate,
		IEventHandlerRoundEnd, IEventHandlerPlayerDie, IEventHandlerPlayerHurt, IEventHandlerPocketDimensionEnter,
		IEventHandlerCheckRoundEnd, IEventHandlerCheckEscape, IEventHandlerSetRole, IEventHandlerDisconnect
	{
		private Plugin instance;

		private Dictionary<Pickup, float> scpPickups = new Dictionary<Pickup, float>();
		private Player scpPlayer;
		private bool isRoundStarted;
		private bool isRotating;
		private const float dur = 327;
		private System.Random rand = new System.Random();

		// Configs
		private List<int> possibleItems;
		private int scpHealth;
		private float scpInterval;
		private bool is035FriendlyFire;
		private int possessedItemCount;
		private bool spawnNewItems;
		private bool useDamageOverride;

		public EventHandler(Plugin plugin)
		{
			instance = plugin;
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			LoadConfigs();
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			isRoundStarted = true;
			isRotating = true;
			scpPickups.Clear();
			scpPlayer = null;

			Timing.RunCoroutine(RotatePickup());
		}

		public void OnRoundEnd(RoundEndEvent ev)
		{
			isRoundStarted = false;
		}

		public void OnPlayerPickupItemLate(PlayerPickupItemLateEvent ev)
		{
			Inventory.SyncItemInfo? item = ((GameObject)ev.Player.GetGameObject()).GetComponent<Inventory>().items.Last();

			if (item.Value.durability == dur)
			{
				InfectPlayer(ev.Player, ev.Item);
			}
		}

		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			if (scpPlayer != null)
			{
				if (useDamageOverride)
				{
					if ((ev.Attacker.PlayerId == scpPlayer.PlayerId ||
						ev.Player.PlayerId == scpPlayer.PlayerId) &&
						ev.Attacker.PlayerId != ev.Player.PlayerId)
					{
						ev.Player.SetHealth(ev.Player.GetHealth() - (int)ev.Damage);
					}
				}
				if (is035FriendlyFire &&
					((ev.Attacker.PlayerId == scpPlayer.PlayerId &&
					ev.Player.TeamRole.Team == Smod2.API.Team.SCP) ||
					(ev.Player.PlayerId == scpPlayer.PlayerId &&
					ev.Attacker.TeamRole.Team == Smod2.API.Team.SCP)))
				{
					ev.Damage = 0;
				}
			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (scpPlayer != null && ev.Player.PlayerId == scpPlayer.PlayerId)
			{
				KillScp035();
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			// Counter admins changing roles through RA
			if (scpPlayer != null && ev.Player.PlayerId == scpPlayer.PlayerId)
			{
				KillScp035();
			}
		}

		public void OnPocketDimensionEnter(PlayerPocketDimensionEnterEvent ev)
		{
			if (!is035FriendlyFire)
			{
				ev.Damage = 0;
				ev.TargetPosition = ev.LastPosition;
			}
		}

		public void OnCheckRoundEnd(CheckRoundEndEvent ev)
		{
			List< Smod2.API.Team> pList = ev.Server.GetPlayers().Select(x => x.TeamRole.Team).ToList();
			if (scpPlayer != null) pList.Remove(pList.FirstOrDefault(x => x == scpPlayer.TeamRole.Team));
			if (!pList.Contains(Smod2.API.Team.CHAOS_INSURGENCY) &&
				!pList.Contains(Smod2.API.Team.CLASSD) &&
				!pList.Contains(Smod2.API.Team.NINETAILFOX) &&
				!pList.Contains(Smod2.API.Team.SCIENTIST) &&
				((pList.Contains(Smod2.API.Team.SCP) &&
				scpPlayer != null) ||
				!pList.Contains(Smod2.API.Team.SCP) &&
				scpPlayer != null))
			{
				ev.Status = ROUND_END_STATUS.SCP_VICTORY;
			}
		}

		public void OnCheckEscape(PlayerCheckEscapeEvent ev)
		{
			if (scpPlayer != null && ev.Player.PlayerId == scpPlayer.PlayerId) ev.AllowEscape = false;
		}

		public void OnDisconnect(DisconnectEvent ev)
		{
			if (scpPlayer != null)
			{
				Player player = instance.Server.GetPlayers().FirstOrDefault(x => x.PlayerId == scpPlayer.PlayerId);
				{
					if (player == null)
					{
						KillScp035(false);
					}
				}
			}
		}
	}
}
