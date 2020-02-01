using MEC;
using UnityEngine;

namespace scp035
{
	public enum AmmoType
	{
		NONE = -1,
		DROPPED_5 = 0,
		DROPPED_7 = 1,
		DROPPED_9 = 2
	}

	public static class Extensions
	{
		public static void Broadcast(this ReferenceHub player, string message, uint time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, monospaced);
		}

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.SetColor(color);
			player.serverRoles.SetText(rank);
		}

		public static Vector3 GetPosition(this ReferenceHub player)
		{
			return player.transform.position;
		}

		public static void SetPosition(this ReferenceHub player, Vector3 pos, float rot = 0, bool forceGround = false)
		{
			player.plyMovementSync.OverridePosition(pos, rot, forceGround);
		}

		public static void SetHealth(this ReferenceHub player, int health)
		{
			player.playerStats.health = health;
		}

		public static int GetHealth(this ReferenceHub player)
		{
			return (int)player.playerStats.health;
		}

		public static int GetPlayerId(this ReferenceHub player)
		{
			return player.queryProcessor.PlayerId;
		}

		public static void SetOverwatch(this ReferenceHub player, bool value)
		{
			player.serverRoles.TargetSetOverwatch(player.scp079PlayerScript.connectionToClient, value);
		}

		public static bool GetOverwatch(this ReferenceHub player)
		{
			// to do
			return false;
		}

		public static Inventory.SyncListItemInfo GetInventory(this ReferenceHub player)
		{
			return player.inventory.items;
		}

		public static void GiveItem(this ReferenceHub player, ItemType item)
		{
			player.inventory.AddNewItem(item);
		}

		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Plugin.Info("trying");
				Vector3 pos = player.GetPosition();
				Plugin.Info(pos.ToString());
				player.characterClassManager.SetClassID(role);
				Timing.RunCoroutine(EventHandlers.DelayAction(0.5f, () => player.SetPosition(pos)));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}

		public static void SetAmmo(this ReferenceHub player, AmmoType type, int amount)
		{
			player.ammoBox.SetOneAmount((int)type, amount.ToString());
		}

		public static int GetAmmo(this ReferenceHub player, AmmoType type)
		{
			return player.ammoBox.GetAmmo((int)type);
		}

		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.GetPlayerId()), player.gameObject);
		}

		public static Team GetTeam(this ReferenceHub player)
		{
			return Plugin.GetTeam(player.characterClassManager.CurClass);
		}
	}
}
