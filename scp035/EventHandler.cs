using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using ServerMod2.API;
using Smod2.EventSystem.Events;

namespace scp035
{
	partial class EventHandler : IEventHandlerWaitingForPlayers, IEventHandlerRoundStart, IEventHandlerPlayerPickupItemLate,
		IEventHandlerRoundEnd, IEventHandlerPlayerDie, IEventHandlerPlayerHurt
	{
		private Plugin instance;

		private Pickup scpPickup;
		private Player scpPlayer;
		private bool isRoundStarted;
		private bool isRotating;
		private const float dur = 327;
		private System.Random rand = new System.Random();

		// Configs
		private List<int> possibleItems;
		private int scpHealth;
		private float scpInterval;

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
			scpPickup = null;
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
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (scpPlayer != null && ev.Player.PlayerId == scpPlayer.PlayerId)
			{
				scpPlayer.SetRank("default", " ");
				scpPickup = GetNext035();
				scpPlayer = null;
				isRotating = true;
			}
		}
	}
}
