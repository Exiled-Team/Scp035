using MEC;
using UnityEngine;

namespace scp035
{
	public static class Extensions
	{
		public static void ChangeRole(this ReferenceHub player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.transform.position;
				Plugin.Info(pos.ToString());
				player.characterClassManager.SetClassID(role);
				Timing.CallDelayed(0.5f, () => player.plyMovementSync.OverridePosition(pos, 0));
			}
			else
			{
				player.characterClassManager.SetClassID(role);
			}
		}

		public static void Damage(this ReferenceHub player, int amount, DamageTypes.DamageType damageType)
		{
			player.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.queryProcessor.PlayerId), player.gameObject);
		}

		public static void SetRank(this ReferenceHub player, string rank, string color = "default")
		{
			player.serverRoles.NetworkMyText = rank;
			player.serverRoles.NetworkMyColor = color;
		}

		public static void RefreshTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = null;
			player.serverRoles.RpcResetFixed();
			player.serverRoles.RefreshPermissions(true);
		}

		public static void HideTag(this ReferenceHub player)
		{
			player.serverRoles.HiddenBadge = player.serverRoles.MyText;
			player.serverRoles.NetworkGlobalBadge = null;
			player.serverRoles.SetText(null);
			player.serverRoles.SetColor(null);
			player.serverRoles.GlobalSet = false;
			player.serverRoles.RefreshHiddenTag();
		}

		public static void Broadcast(this ReferenceHub player, string message, uint time, bool monospaced = false)
		{
			player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, message, time, monospaced);
		}

		public static void PlaceCorrosion(this ReferenceHub player)
		{
			player.characterClassManager.RpcPlaceBlood(player.transform.position, 1, 2f);
		}
	}
}
