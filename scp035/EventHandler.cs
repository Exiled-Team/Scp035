using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

namespace scp035
{
	partial class EventHandler : IEventHandlerWaitingForPlayers, IEventHandlerRoundStart, IEventHandlerPlayerPickupItemLate,
		IEventHandlerRoundEnd, IEventHandlerPlayerDie, IEventHandlerPlayerHurt, IEventHandlerPocketDimensionEnter,
		IEventHandlerCheckRoundEnd, IEventHandlerCheckEscape, IEventHandlerSetRole, IEventHandlerDisconnect,
		IEventHandlerContain106, IEventHandlerGeneratorInsertTablet, IEventHandlerUpdate
	{
		private Plugin instance;
		private Dictionary<Pickup, float> scpPickups = new Dictionary<Pickup, float>();
		private Player scpPlayer;
		DateTime updateTimer;
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
		private bool winWithTutorials;
		private bool changeToZombie;
		private bool isTutorialFriendlyFire;
		private bool isCorroding;
		private float corrodeRange;
		private int corrodeDamage;
		private float corrodeInterval;
		private bool corrodeLifeSteal;

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

			updateTimer = DateTime.Now;

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
				if (!is035FriendlyFire &&
					((ev.Attacker.PlayerId == scpPlayer.PlayerId &&
					ev.Player.TeamRole.Team == Smod2.API.Team.SCP) ||
					(ev.Player.PlayerId == scpPlayer.PlayerId &&
					ev.Attacker.TeamRole.Team == Smod2.API.Team.SCP)))
				{
					ev.Damage = 0;
				}
				if (!isTutorialFriendlyFire &&
					((ev.Attacker.PlayerId == scpPlayer.PlayerId &&
					ev.Player.TeamRole.Team == Smod2.API.Team.TUTORIAL) ||
					(ev.Player.PlayerId == scpPlayer.PlayerId &&
					ev.Attacker.TeamRole.Team == Smod2.API.Team.TUTORIAL)))
				{
					ev.Damage = 0;
				}
				if (useDamageOverride && ev.Damage > 0)
				{
					if ((ev.Attacker.PlayerId == scpPlayer.PlayerId ||
						ev.Player.PlayerId == scpPlayer.PlayerId) &&
						ev.Attacker.PlayerId != ev.Player.PlayerId &&
						ev.DamageType != DamageType.FALLDOWN &&
						ev.DamageType != DamageType.NUKE &&
						ev.DamageType != DamageType.TESLA &&
						ev.DamageType != DamageType.WALL &&
						ev.DamageType != DamageType.DECONT &&
						ev.DamageType != DamageType.FALLDOWN)
					{
						ev.Player.SetHealth(ev.Player.GetHealth() - (int)ev.Damage);
					}
				}
			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (ev.Player.PlayerId == scpPlayer?.PlayerId)
			{
				KillScp035();
			}
		}

		public void OnSetRole(PlayerSetRoleEvent ev)
		{
			// Counter admins changing roles through RA
			if (ev.Player.PlayerId == scpPlayer?.PlayerId)
			{
				KillScp035();
			}
		}

		public void OnPocketDimensionEnter(PlayerPocketDimensionEnterEvent ev)
		{
			if (ev.Player.PlayerId == scpPlayer?.PlayerId && !is035FriendlyFire)
			{
				ev.Damage = 0;
				ev.TargetPosition = ev.LastPosition;
			}
		}

		public void OnCheckRoundEnd(CheckRoundEndEvent ev)
		{
			List< Smod2.API.Team> pList = ev.Server.GetPlayers().Select(x => x.TeamRole.Team).ToList();
			pList.Remove(pList.FirstOrDefault(x => x == scpPlayer?.TeamRole.Team));

			// If everyone but SCPs and 035 or just 035 is alive, end the round
			if ((!pList.Contains(Smod2.API.Team.CHAOS_INSURGENCY) &&
				!pList.Contains(Smod2.API.Team.CLASSD) &&
				!pList.Contains(Smod2.API.Team.NINETAILFOX) &&
				!pList.Contains(Smod2.API.Team.SCIENTIST) &&
				((pList.Contains(Smod2.API.Team.SCP) &&
				scpPlayer != null) ||
				!pList.Contains(Smod2.API.Team.SCP) &&
				scpPlayer != null)) || 
				(winWithTutorials &&
				!pList.Contains(Smod2.API.Team.CHAOS_INSURGENCY) &&
				!pList.Contains(Smod2.API.Team.CLASSD) &&
				!pList.Contains(Smod2.API.Team.NINETAILFOX) &&
				!pList.Contains(Smod2.API.Team.SCIENTIST) &&
				pList.Contains(Smod2.API.Team.TUTORIAL) &&
				scpPlayer != null))
			{
				if (changeToZombie)
				{
					scpPlayer.ChangeRole(Role.SCP_049_2, true, false);
				}
				else
				{
					ev.Status = ROUND_END_STATUS.SCP_VICTORY;
				}
			}
			// If 035 is the only scp alive keep the round going
			else if (scpPlayer != null && !pList.Contains(Smod2.API.Team.SCP))
			{
				ev.Status = ROUND_END_STATUS.ON_GOING;
			}
		}

		public void OnCheckEscape(PlayerCheckEscapeEvent ev)
		{
			if (ev.Player.PlayerId == scpPlayer?.PlayerId) ev.AllowEscape = false;
		}

		public void OnDisconnect(DisconnectEvent ev)
		{
			if (instance.Server.GetPlayers().FirstOrDefault(x => x.PlayerId == scpPlayer?.PlayerId) == null)
			{
				KillScp035(false);
			}
		}

		public void OnContain106(PlayerContain106Event ev)
		{
			if (ev.Player.PlayerId == scpPlayer?.PlayerId && !is035FriendlyFire)
			{
				ev.ActivateContainment = false;
			}
		}

		public void OnGeneratorInsertTablet(PlayerGeneratorInsertTabletEvent ev)
		{
			if (ev.Player.PlayerId == scpPlayer?.PlayerId && !is035FriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnUpdate(UpdateEvent ev)
		{
			if (isRoundStarted && scpPlayer != null && updateTimer != null && updateTimer < DateTime.Now && isCorroding)
			{
				updateTimer = DateTime.Now.AddSeconds(corrodeInterval);
				GameObject scp035 = (GameObject)scpPlayer.GetGameObject();

				IEnumerable<Player> pList = instance.Server.GetPlayers().Where(x => x.PlayerId != scpPlayer.PlayerId);
				if (!is035FriendlyFire) pList = pList.Where(x => x.TeamRole.Team != Smod2.API.Team.SCP);
				if (!isTutorialFriendlyFire) pList = pList.Where(x => x.TeamRole.Team != Smod2.API.Team.TUTORIAL);
				foreach (Player player in pList.Where(x => Vector3.Distance(scp035.transform.position, ((GameObject)x.GetGameObject()).transform.position) <= corrodeRange))
				{
					CorrodePlayer(player);
				}
			}
		}
	}
}
