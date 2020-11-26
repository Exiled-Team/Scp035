using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace scp035
{
	partial class EventHandlers
	{
		public scp035 plugin;
		public EventHandlers(scp035 plugin) => this.plugin = plugin;

		private static List<Pickup> scpPickups = new List<Pickup>();
		private List<int> ffPlayers = new List<int>();
		internal static Player scpPlayer;
		private static bool isHidden;
		private static string tag;
		private static string color;
		private bool isRoundStarted;
		private static bool isRotating;
		private static int maxHP;
		private static System.Random rand = new System.Random();

		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

		public void OnRoundStart()
		{
			isRoundStarted = true;
			isRotating = true;
			scpPickups.Clear();
			ffPlayers.Clear();
			scpPlayer = null;

			coroutines.Add(Timing.CallDelayed(1f, () => Timing.RunCoroutine(RotatePickup())));
			coroutines.Add(Timing.RunCoroutine(CorrodeUpdate()));
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines.ToArray());
			coroutines.Clear();
		}

		public void OnRoundRestart()
		{
			// In case the round is force restarted
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines.ToArray());
			coroutines.Clear();
		}

		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (scpPickups.Contains(ev.Pickup))
			{
				ev.IsAllowed = false;
				InfectPlayer(ev.Player, ev.Pickup);
			}
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ffPlayers.Contains(ev.Attacker.Id))
			{
				RemoveFF(ev.Attacker);
			}

			if (scpPlayer != null)
			{
				if (!scp035.instance.Config.ScpFriendlyFire &&
					((ev.Attacker.Id == scpPlayer?.Id &&
					ev.Target.Team == Team.SCP) ||
					(ev.Target.Id == scpPlayer?.Id &&
					ev.Attacker.Team == Team.SCP)))
				{
					ev.Amount = 0f;
				}

				if (!scp035.instance.Config.TutorialFriendlyFire &&
					ev.Attacker.Id != ev.Target.Id &&
					((ev.Attacker.Id == scpPlayer?.Id &&
					ev.Target.Team == Team.TUT) ||
					(ev.Target.Id == scpPlayer?.Id &&
					ev.Attacker.Team == Team.TUT)))
				{
					ev.Amount = 0f;
				}
			}
		}

		public void OnShoot(ShootingEventArgs ev)
		{
			if (ev.Target == null || scpPlayer == null) return;
			Player target = Player.Get(ev.Target);
			if (target == null) return;

			if ((ev.Shooter.Id == scpPlayer?.Id &&
				target.Team == scpPlayer?.Team)
				|| (target.Id == scpPlayer?.Id &&
				ev.Shooter.Team == scpPlayer?.Team))
			{
				GrantFF(ev.Shooter);
			}

			// If friendly fire is off, to allow for chaos and dclass to hurt eachother
			if ((ev.Shooter.Id == scpPlayer?.Id || target.Id == scpPlayer?.Id) &&
				(((ev.Shooter.Team == Team.CDP && target.Team == Team.CHI)
				|| (ev.Shooter.Team == Team.CHI && target.Team == Team.CDP)) || 
				((ev.Shooter.Team == Team.RSC && target.Team == Team.MTF)
				|| (ev.Shooter.Team == Team.MTF && target.Team == Team.RSC))))
			{
				GrantFF(ev.Shooter);
			}
		}

		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (ev.Target.Id == scpPlayer?.Id)
			{
				KillScp035();
			}
		}

		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.Id && !scp035.instance.Config.ScpFriendlyFire)
			{
				ev.IsAllowed = false;
				ev.Position = scpPlayer.Position;
			}
		}

		public void OnCheckRoundEnd(EndingRoundEventArgs ev)
		{
			List<Team> pList = Player.List.Where(x => x.Id != scpPlayer?.Id).Select(x => x.Team).ToList();

			// If everyone but SCPs and 035 or just 035 is alive, end the round
			if ((!pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && ((pList.Contains(Team.SCP) && scpPlayer != null) || (!pList.Contains(Team.SCP) && scpPlayer != null))) ||
				(scp035.instance.Config.WinWithTutorial && !pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && pList.Contains(Team.TUT) && scpPlayer != null))
			{
				ev.LeadingTeam = Exiled.API.Enums.LeadingTeam.Anomalies;
				ev.IsRoundEnded = true;
			}

			// If 035 is the only scp alive keep the round going
			else if (scpPlayer != null && !pList.Contains(Team.SCP) && (pList.Contains(Team.CDP) || pList.Contains(Team.CHI) || pList.Contains(Team.MTF) || pList.Contains(Team.RSC)))
			{
				ev.IsAllowed = false;
			}
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if ((ev.Player.Id == scpPlayer?.Id) || (scpPlayer != null && ev.Player.Id == scpPlayer.Id && ev.NewRole == RoleType.Spectator))
			{
				KillScp035();
			}
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.Id)
			{
				KillScp035(false);
			}
		}

		public void OnContain106(ContainingEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.Id && !scp035.instance.Config.ScpFriendlyFire)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnInsertTablet(InsertingGeneratorTabletEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.Id && !scp035.instance.Config.ScpFriendlyFire)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.Id)
			{
				ev.IsAllowed = false;
				ev.Player.Position = Map.GetRandomSpawnPoint(RoleType.Scp096);
			}
		}

		public void OnUseMedicalItem(UsingMedicalItemEventArgs ev)
		{
			if ((!scp035.instance.Config.CanUseMedicalItems && ev.Player.Id == scpPlayer?.Id && (ev.Item == ItemType.Adrenaline || ev.Item == ItemType.Painkillers || ev.Item == ItemType.Medkit || ev.Item == ItemType.SCP500)) || (!scp035.instance.Config.CanHealBeyondHostHp && ev.Player.Health >= maxHP))
			{
				ev.IsAllowed = false;
			}
		}

		public void OnUsedMedicalItem(UsedMedicalItemEventArgs ev)
		{
			if (ev.Player.Health > maxHP) ev.Player.Health = maxHP;
		}

		public void OnRACommand(SendingRemoteAdminCommandEventArgs ev)
		{
			string cmd = ev.Name.ToLower();
			if (cmd == "spawn035")
			{
				ev.IsAllowed = false;
				if (scpPlayer == null)
				{
					Player player = null;
					if (ev.Arguments.Count >= 1)
					{
						if (int.TryParse(ev.Arguments[0], out int classid) && classid >= 0 && classid <= 17)
						{
							if (ev.Arguments.Count == 2)
							{
								player = Player.Get(ev.Arguments[1]);
								if (player != null)
								{
									player.SetRole((RoleType)classid);
									EventHandlers.Spawn035(player, null, false);
									ev.Success = true;
									ev.Sender.RemoteAdminMessage($"Spawned '{player.Nickname}' as SCP-035.");
								}
								else
								{
									ev.Success = false;
									ev.Sender.RemoteAdminMessage("Error: Invalid player.");
									return;
								}
							}
							else
							{
								player = Player.List.ElementAt(UnityEngine.Random.Range(0, Player.List.Count()));
								player.SetRole((RoleType)classid);
								EventHandlers.Spawn035(player, null, false);
								ev.Success = true;
								ev.Sender.RemoteAdminMessage($"Spawned '{player.Nickname}' as SCP-035.");
							}
						}
						else
						{
							ev.Success = false;
							ev.Sender.RemoteAdminMessage("Error: Invalid ClassID.");
						}
					}
					else
					{
						ev.Success = false;
						ev.Sender.RemoteAdminMessage("Usage: SPAWN035 (CLASSID) [PLAYER / PLAYERID / STEAMID]");
					}
				}
				else
				{
					ev.Success = false;
					ev.Sender.RemoteAdminMessage("Error: SCP-035 is currently alive.");
				}
			}
		}
	}
}
