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
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
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

			Timing.RunCoroutine(DelayAction(1f, () => Timing.RunCoroutine(RotatePickup())));
			Timing.RunCoroutine(CorrodeUpdate());
		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;
		}

		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Item.info.durability == dur)
			{
				ev.Allow = false;
				InfectPlayer(ev.Player, ev.Item);
			}
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			// Remove friendly fire
			if (ffPlayers.Contains(ev.Attacker.GetPlayerId()))
			{
				GrantFF(ev.Attacker);
			}


			if (scpPlayer != null)
			{
				if (!Configs.scpFriendlyFire &&
					((ev.Attacker.GetPlayerId() == scpPlayer?.GetPlayerId() &&
					ev.Player.GetTeam() == Team.SCP) ||
					(ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId() &&
					ev.Attacker.GetTeam() == Team.SCP)))
				{
					ev.Info = new PlayerStats.HitInfo(0f, ev.Attacker.nicknameSync.name, ev.Info.GetDamageType(), ev.Attacker.queryProcessor.PlayerId);
				}

				if (!Configs.tutorialFriendlyFire &&
					ev.Attacker.GetPlayerId() != ev.Player.GetPlayerId() &&
					((ev.Attacker.GetPlayerId() == scpPlayer?.GetPlayerId() &&
					ev.Player.GetTeam() == Team.TUT) ||
					(ev.Player.GetPlayerId() == scpPlayer?.GetPlayerId() &&
					ev.Attacker.GetTeam() == Team.TUT)))
				{
					ev.Info = new PlayerStats.HitInfo(0f, ev.Attacker.nicknameSync.name, ev.Info.GetDamageType(), ev.Attacker.queryProcessor.PlayerId);
				}
			}
		}

		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.Target == null) return;
			ReferenceHub target = Plugin.GetPlayer(ev.Target);

			if ((ev.Shooter.GetPlayerId() == scpPlayer?.GetPlayerId() && target.GetTeam() == scpPlayer?.GetTeam()) || (target.GetPlayerId() == scpPlayer?.GetPlayerId() && ev.Shooter.GetTeam() == scpPlayer?.GetTeam()))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.GetPlayerId());
			}
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
			List<Team> pList = Plugin.GetHubs().Select(x => x.GetTeam()).ToList();
			pList.Remove(pList.FirstOrDefault(x => x == scpPlayer?.GetTeam()));

			// If everyone but SCPs and 035 or just 035 is alive, end the round
			if ((!pList.Contains(Team.CHI) &&
				!pList.Contains(Team.CDP) &&
				!pList.Contains(Team.MTF) &&
				!pList.Contains(Team.RSC) &&
				((pList.Contains(Team.SCP) &&
				scpPlayer != null) ||
				!pList.Contains(Team.SCP) &&
				scpPlayer != null)) ||
				(Configs.winWithTutorial &&
				!pList.Contains(Team.CHI) &&
				!pList.Contains(Team.CDP) &&
				!pList.Contains(Team.MTF) &&
				!pList.Contains(Team.RSC) &&
				pList.Contains(Team.TUT) &&
				scpPlayer != null))
			{
				if (Configs.changeToZombie)
				{
					scpPlayer.ChangeRole(RoleType.Scp0492, false);
				}
				else
				{
					ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
					ev.ForceEnd = true;
				}
			}
			// If 035 is the only scp alive keep the round going
			else if (scpPlayer != null && !pList.Contains(Team.SCP))
			{
				ev.ForceEnd = false;
			}
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
	}
}
