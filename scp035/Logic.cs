using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;
using EXILED.Extensions;
using EXILED;

namespace scp035
{
	partial class EventHandlers
	{
		private void RemovePossessedItems()
		{
			for (int i = 0; i < scpPickups.Count; i++)
			{
				Pickup p = scpPickups.ElementAt(i).Key;
				if (p != null) p.Delete();
			}
			scpPickups.Clear();
		}

		private Pickup GetRandomItem()
		{
			List<Pickup> pickups = GameObject.FindObjectsOfType<Pickup>().Where(x => !scpPickups.ContainsKey(x)).ToList();
			return pickups[rand.Next(pickups.Count)];
		}

		private void RefreshItems()
		{
			if (Player.GetHubs().Where(x => Player.GetTeam(x) == Team.RIP && !x.serverRoles.OverwatchEnabled).ToList().Count > 0)
			{
				RemovePossessedItems();
				for (int i = 0; i < Configs.infectedItemCount; i++)
				{
					Pickup p = GetRandomItem();
					Pickup a = PlayerManager.localPlayer
						.GetComponent<Inventory>().SetPickup((ItemType)Configs.possibleItems[rand.Next(Configs.possibleItems.Count)],
						-4.656647E+11f,
						p.transform.position,
						p.transform.rotation,
						0, 0, 0).GetComponent<Pickup>();
					scpPickups.Add(a, a.info.durability);
					a.info.durability = dur;
				}
			}
		}

		private void KillScp035(bool setRank = true)
		{
			if (setRank)
			{
				scpPlayer.SetRank("", "default");
				if (hasTag) scpPlayer.RefreshTag();
				if (isHidden) scpPlayer.HideTag();
			}
			scpPlayer = null;
			isRotating = true;
			RefreshItems();
		}

		private void InfectPlayer(ReferenceHub player, Pickup pItem)
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Spectator && !x.serverRoles.OverwatchEnabled && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (pList.Count > 0 && scpPlayer == null)
			{
				pItem.Delete();

				ReferenceHub p035 = pList[rand.Next(pList.Count)];
				Log.Info(p035.nicknameSync.Network_myNickSync);
				Log.Info(p035.characterClassManager.UserId);
				Log.Info(p035.characterClassManager.CurClass.ToString());
				Vector3 pos = player.transform.position;
				p035.ChangeRole(player.characterClassManager.CurClass);
				Timing.CallDelayed(0.2f, () => p035.plyMovementSync.OverridePosition(pos, 0));

				foreach (Inventory.SyncItemInfo item in player.inventory.items) p035.inventory.AddNewItem(item.id);
				p035.playerStats.health = Configs.health;
				p035.ammoBox.Networkamount = "250:250:250";

				hasTag = !string.IsNullOrEmpty(p035.serverRoles.NetworkMyText);
				isHidden = !string.IsNullOrEmpty(p035.serverRoles.HiddenBadge);
				if (isHidden) p035.RefreshTag();
				p035.SetRank("SCP-035", "red");

				p035.Broadcast("<size=60>You are <color=\"red\"><b>SCP-035</b></color></size>\nYou have infected a body and have gained control over it, use it to help the other SCPs!", 10);
				scpPlayer = p035;
				isRotating = false;

				player.ChangeRole(RoleType.Spectator);
				player.Broadcast("You have picked up <color=\"red\">SCP-035.</color> He has infected your body and is now in control of you.", 10);

				RemovePossessedItems();

				if (Configs.corrodeHost)
				{
					coroutines.Add(Timing.RunCoroutine(CorrodeHost()));
				}
			}
		}

		private IEnumerator<float> CorrodeHost()
		{
			while (scpPlayer != null)
			{
				scpPlayer.playerStats.health -= Configs.corrodeHostAmount;
				if (scpPlayer.playerStats.health <= 0)
				{
					scpPlayer.ChangeRole(RoleType.Spectator);
					KillScp035();
				}
				yield return Timing.WaitForSeconds(Configs.corrodeHostInterval);
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
				yield return Timing.WaitForSeconds(Configs.rotateInterval);
			}
		}

		private IEnumerator<float> CorrodeUpdate()
		{
			if (Configs.corrodePlayers)
			{
				while (isRoundStarted)
				{
					if (scpPlayer != null)
					{
						IEnumerable<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scpPlayer.queryProcessor.PlayerId);
						if (!Configs.scpFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.SCP);
						if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.TUT);
						foreach (ReferenceHub player in pList)
						{
							if (player != null && Vector3.Distance(scpPlayer.transform.position, player.transform.position) <= Configs.corrodeDistance)
							{
								CorrodePlayer(player);
							}
						}
					}
					yield return Timing.WaitForSeconds(Configs.corrodeInterval);
				}
			}
		}

		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}

		private void CorrodePlayer(ReferenceHub player)
		{
			if (Configs.corrodeLifeSteal && scpPlayer != null)
			{
				int currHP = (int)scpPlayer.playerStats.health;
				scpPlayer.playerStats.health = currHP + Configs.corrodeDamage > Configs.health ? Configs.health : currHP + Configs.corrodeDamage;
			}
			player.Damage(Configs.corrodeDamage, DamageTypes.Nuke);
		}

		private void GrantFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = false;
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}
	}
}
