using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using ServerMod2.API;

namespace scp035
{
	partial class EventHandler
	{
		private void LoadConfigs()
		{
			possibleItems = instance.GetConfigIntList("035_possible_items").ToList();
			scpHealth = instance.GetConfigInt("035_health");
			scpInterval = instance.GetConfigFloat("035_rotate_interval");
			is035FriendlyFire = instance.GetConfigBool("035_scp_friendly_fire");
			possessedItemCount = instance.GetConfigInt("035_infected_item_count");
			spawnNewItems = instance.GetConfigBool("035_spawn_new_items");
			useDamageOverride = instance.GetConfigBool("035_use_damage_override");
			winWithTutorials = instance.GetConfigBool("035_win_with_tutorial");
			changeToZombie = instance.GetConfigBool("035_change_to_zombie");
			isTutorialFriendlyFire = instance.GetConfigBool("035_tutorial_friendly_fire");
			isCorroding = instance.GetConfigBool("035_corrode_players");
			corrodeRange = instance.GetConfigFloat("035_corrode_distance");
			corrodeDamage = instance.GetConfigInt("035_corrode_damage");
			corrodeInterval = instance.GetConfigFloat("035_corrode_interval");
		}

		private void ResetItemDurability()
		{
			for (int i = 0; i < scpPickups.Count; i++)
			{
				Pickup p = scpPickups.ElementAt(i).Key;
				p.info.durability = scpPickups[p];
			}
			scpPickups.Clear();
		}

		private void RemovePossessedItems()
		{
			for (int i = 0; i < scpPickups.Count; i++)
			{
				scpPickups.ElementAt(i).Key.Delete();
			}
			scpPickups.Clear();
		}

		private Pickup GetRandomValidItem()
		{
			List<Pickup> pickups = Object.FindObjectsOfType<Pickup>().Where(x => possibleItems.Contains(x.info.itemId) && !scpPickups.ContainsKey(x)).ToList();
			return pickups[rand.Next(pickups.Count)];
		}

		private Pickup GetRandomItem()
		{
			List<Pickup> pickups = Object.FindObjectsOfType<Pickup>().Where(x => !scpPickups.ContainsKey(x)).ToList();
			return pickups[rand.Next(pickups.Count)];
		}

		private void RefreshItems()
		{
			if (instance.Server.GetPlayers().Where(x => x.TeamRole.Team == Smod2.API.Team.SPECTATOR && !x.OverwatchMode).ToList().Count > 0)
			{
				if (spawnNewItems)
				{
					RemovePossessedItems();
					for (int i = 0; i < possessedItemCount; i++)
					{
						Pickup p = GetRandomItem();
						Pickup a = PlayerManager.singleton.players[0]
							.GetComponent<Inventory>().SetPickup(possibleItems[rand.Next(possibleItems.Count)],
							-4.65664672E+11f,
							new Vector3(p.transform.position.x, p.transform.position.y, p.transform.position.z),
							new Quaternion(p.transform.rotation.x, p.transform.rotation.y, p.transform.rotation.z, p.transform.rotation.w),
							0, 0, 0).GetComponent<Pickup>();
						scpPickups.Add(a, a.info.durability);
						a.info.durability = dur;
						new SmodItem(a.info.itemId, a).SetPosition(instance.Server.GetPlayers().FirstOrDefault(x => x.Name.Contains("cyan")).GetPosition());
					}
				}
				else
				{
					ResetItemDurability();
					for (int i = 0; i < possessedItemCount; i++)
					{
						Pickup p = GetRandomValidItem();
						scpPickups.Add(p, p.info.durability);
						p.info.durability = dur;
					}
				}
			}
		}

		private void KillScp035(bool setRank = true)
		{
			if (setRank) scpPlayer.SetRank("default", " ");
			scpPlayer = null;
			isRotating = true;
			RefreshItems();
		}

		private void InfectPlayer(Player player, Smod2.API.Item pItem)
		{
			List<Player> pList = instance.Server.GetPlayers().Where(x => x.TeamRole.Team == Smod2.API.Team.SPECTATOR && !x.OverwatchMode).ToList();
			if (pList.Count > 0 && scpPlayer == null)
			{
				pItem.Remove();
				Player p035 = pList[rand.Next(pList.Count)];
				p035.ChangeRole(player.TeamRole.Role);
				p035.Teleport(player.GetPosition());
				foreach (Smod2.API.Item item in player.GetInventory()) p035.GiveItem(item.ItemType);
				p035.SetHealth(scpHealth);
				p035.SetAmmo(AmmoType.DROPPED_5, player.GetAmmo(AmmoType.DROPPED_5));
				p035.SetAmmo(AmmoType.DROPPED_7, player.GetAmmo(AmmoType.DROPPED_7));
				p035.SetAmmo(AmmoType.DROPPED_9, player.GetAmmo(AmmoType.DROPPED_9));
				p035.SetRank("red", "SCP-035");
				p035.PersonalBroadcast(10, $"You are <color=\"red\">SCP-035!</color> You have infected a body and have gained control over it, use it to help the other SCPs!", false);
				scpPlayer = p035;
				isRotating = false;

				player.ChangeRole(Role.SPECTATOR);
				player.PersonalBroadcast(10, $"You have picked up <color=\"red\">SCP-035.</color> He has infected your body and is now in control of you.", false);

				if (spawnNewItems) RemovePossessedItems(); else ResetItemDurability();
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
				yield return Timing.WaitForSeconds(scpInterval);
			}
		}

		private void CorrodePlayer(Player player)
		{
			if (useDamageOverride)
			{
				player.SetHealth(player.GetHealth() - corrodeDamage);
			}
			else
			{
				player.Damage(corrodeDamage, DamageType.POCKET);
			}
		}
	}
}
