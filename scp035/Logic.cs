using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using Exiled.API.Features;
using Exiled.API.Enums;

namespace scp035
{
	partial class EventHandlers
	{
		private static void RemovePossessedItems()
		{
			for (int i = 0; i < scpPickups.Count; i++)
			{
				Pickup p = scpPickups[i];
				if (p != null) p.Delete();
			}
			scpPickups.Clear();
		}

		private static Pickup GetRandomItem()
		{
			List<Pickup> pickups = GameObject.FindObjectsOfType<Pickup>().Where(x => !scpPickups.Contains(x)).ToList();
			return pickups[rand.Next(pickups.Count)];
		}

		private static void RefreshItems()
		{
			if (Player.List.Where(x => x.Team == Team.RIP && !x.ReferenceHub.serverRoles.OverwatchEnabled).ToList().Count > 0)
			{
				RemovePossessedItems();
				for (int i = 0; i < scp035.instance.Config.InfectedItemCount; i++)
				{
					Pickup p = GetRandomItem();
					Pickup a = PlayerManager.localPlayer
						.GetComponent<Inventory>().SetPickup((ItemType)scp035.instance.Config.PossibleItems[rand.Next(scp035.instance.Config.PossibleItems.Count)],
						-4.656647E+11f,
						p.transform.position,
						p.transform.rotation,
						0, 0, 0).GetComponent<Pickup>();
					scpPickups.Add(a);
				}
			}
		}

		private static void KillScp035(bool setRank = true)
		{
			Player player = scpPlayer;
			scpPlayer = null;
			if (setRank)
			{
				player.RankName = tag;
				player.RankColor = color;
				if (isHidden) player.ReferenceHub.characterClassManager.CallCmdRequestHideTag();
			}
			if (scp035.instance.Config.CanHealBeyondHostHp)
			{
				player.MaxHealth = maxHP;
			}
			isRotating = true;
			RefreshItems();
		}

		public static void Spawn035(Player p035, Player player = null, bool full = true)
		{
			if (full)
			{
				if (player != null)
				{
					Vector3 pos = player.Position;
					p035.ChangeRole(player.Role);
					Timing.CallDelayed(0.2f, () => p035.Position = pos);

					foreach (Inventory.SyncItemInfo item in player.Inventory.items) p035.Inventory.AddNewItem(item.id);
				}
				maxHP = p035.MaxHealth;
				if (scp035.instance.Config.CanHealBeyondHostHp)
				{
					p035.MaxHealth = scp035.instance.Config.Health;
				}
				p035.Health = scp035.instance.Config.Health;
				p035.SetAmmo(AmmoType.Nato556, 250);
				p035.SetAmmo(AmmoType.Nato762, 250);
				p035.SetAmmo(AmmoType.Nato9, 250);
			}

			if (!string.IsNullOrEmpty(p035.ReferenceHub.serverRoles.HiddenBadge))
			{
				isHidden = true;
				p035.BadgeHidden = false;
			}
			tag = p035.RankName;
			color = p035.RankColor;
			p035.RankName = "SCP-035";
			p035.RankColor = "red";

			p035.Broadcast(10, $"<size=60>You are <color=\"red\"><b>SCP-035</b></color></size>{(full ? "\n<i>You have infected a body and have gained control over it, use it to help the other SCPs!</i>" : string.Empty)}");

			scpPlayer = p035;
		}

		public static void InfectPlayer(Player player, Pickup pItem)
		{
			List<Player> pList = Player.List.Where(x => x.Role == RoleType.Spectator && !x.ReferenceHub.serverRoles.OverwatchEnabled && x.UserId != null && x.UserId != string.Empty).ToList();
			if (pList.Count > 0 && scpPlayer == null)
			{
				pItem.Delete();

				Spawn035(pList[rand.Next(pList.Count)], player);

				isRotating = false;

				player.ClearInventory();
				player.ChangeRole(RoleType.Spectator);
				player.Broadcast(10, "<i>You have picked up <color=\"red\">SCP-035.</color> He has infected your body and is now in control of you.</i>");

				RemovePossessedItems();

				if (scp035.instance.Config.CorrodeHost)
				{
					coroutines.Add(Timing.RunCoroutine(CorrodeHost()));
				}
			}
		}

		private static IEnumerator<float> CorrodeHost()
		{
			while (scpPlayer != null)
			{
				scpPlayer.Health -= scp035.instance.Config.CorrodeHostAmount;
				if (scpPlayer.Health <= 0)
				{
					scpPlayer.ChangeRole(RoleType.Spectator);
					KillScp035();
				}
				yield return Timing.WaitForSeconds(scp035.instance.Config.CorrodeHostInterval);
			}
		}

		private IEnumerator<float> RotatePickup()
		{
			while (isRoundStarted)
			{
				if (isRotating)
				{
					RefreshItems();
				}
				yield return Timing.WaitForSeconds(scp035.instance.Config.RotateInterval);
			}
		}

		private IEnumerator<float> CorrodeUpdate()
		{
			if (scp035.instance.Config.CorrodePlayers)
			{
				while (isRoundStarted)
				{
					if (scpPlayer != null)
					{
						IEnumerable<Player> pList = Player.List.Where(x => x.Id != scpPlayer.Id);
						if (!scp035.instance.Config.ScpFriendlyFire) pList = pList.Where(x => x.Team != Team.SCP);
						if (!scp035.instance.Config.TutorialFriendlyFire) pList = pList.Where(x => x.Team != Team.TUT);
						foreach (Player player in pList)
						{
							if (player != null && Vector3.Distance(scpPlayer.Position, player.Position) <= scp035.instance.Config.CorrodeDistance)
							{
								CorrodePlayer(player);
							}
						}
					}
					yield return Timing.WaitForSeconds(scp035.instance.Config.CorrodeInterval);
				}
			}
		}

		private void CorrodePlayer(Player player)
		{
			if (scp035.instance.Config.CorrodeLifeSteal && scpPlayer != null)
			{
				int currHP = (int)scpPlayer.Health;
				scpPlayer.Health = currHP + scp035.instance.Config.CorrodeDamage > scp035.instance.Config.Health ? scp035.instance.Config.Health : currHP + scp035.instance.Config.CorrodeDamage;
			}
			player.Damage(scp035.instance.Config.CorrodeDamage, DamageTypes.Nuke);
		}

		private void GrantFF(Player player)
		{
			player.IsFriendlyFireEnabled = true;
			ffPlayers.Add(player.Id);
		}

		private void RemoveFF(Player player)
		{
			player.IsFriendlyFireEnabled = false;
			ffPlayers.Remove(player.Id);
		}
	}
}
