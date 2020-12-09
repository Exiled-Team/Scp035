using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace scp035
{
	public static class Extensions
	{
		public static void ChangeRole(this Player player, RoleType role, bool spawnTeleport = true)
		{
			if (!spawnTeleport)
			{
				Vector3 pos = player.Position;
				player.SetRole(role);
				Timing.CallDelayed(0.5f, () => player.Position = pos);
			}
			else
			{
				player.SetRole(role);
			}
		}

		public static void RefreshTag(this Player player)
		{
			player.ReferenceHub.serverRoles.HiddenBadge = null;
			player.ReferenceHub.serverRoles.RpcResetFixed();
			player.ReferenceHub.serverRoles.RefreshPermissions(true);
		}

		public static void Damage(this Player player, int amount, DamageTypes.DamageType damageType)
		{
			player.ReferenceHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(amount, "WORLD", damageType, player.ReferenceHub.queryProcessor.PlayerId), player.GameObject);
		}

		public static void PlaceCorrosion(this Player player)
		{
			player.ReferenceHub.characterClassManager.RpcPlaceBlood(player.Position, 1, 2f);
		}
	}
}
