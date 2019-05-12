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
		IEventHandlerRoundEnd, IEventHandlerPlayerDie, IEventHandlerPlayerHurt
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
			if (scpPlayer != null &&
				(ev.Attacker.PlayerId == scpPlayer.PlayerId || 
				ev.Player.PlayerId == scpPlayer.PlayerId) && 
				ev.Attacker.PlayerId != ev.Player.PlayerId)
			{
				ev.Player.SetHealth(ev.Player.GetHealth() - (int)ev.Damage);
			}
			if (is035FriendlyFire && ((ev.Attacker.PlayerId == scpPlayer.PlayerId && ev.Player.TeamRole.Team == Smod2.API.Team.SCP) ||
				(ev.Player.PlayerId == scpPlayer.PlayerId && ev.Attacker.TeamRole.Team == Smod2.API.Team.SCP)))
			{
				ev.Damage = 0;
			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (scpPlayer != null && ev.Player.PlayerId == scpPlayer.PlayerId)
			{
				scpPlayer.SetRank("default", " ");
				scpPlayer = null;
				isRotating = true;
			}
		}
	}
}
