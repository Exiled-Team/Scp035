using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(Scp939PlayerScript), nameof(Scp939PlayerScript.CallCmdShoot))]
	class Scp939AttackPatch
	{
		public static void Postfix(Scp939PlayerScript __instance, GameObject target)
		{
			Player player = Player.Get(target);
			if (player.Role == RoleType.Tutorial && !scp035.instance.Config.ScpFriendlyFire)
			{
				player.ReferenceHub.playerEffectsController.DisableEffect<CustomPlayerEffects.Amnesia>();
			}
		}
	}
}
