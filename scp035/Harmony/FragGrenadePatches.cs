using Exiled.API.Features;
using Grenades;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scp035.API;
using UnityEngine;
using GameCore;

namespace scp035.Harmony
{
	[HarmonyPatch(typeof(FragGrenade), nameof(FragGrenade.ServersideExplosion))]
	static class FragGrenadePatches
	{
		public static void Postfix(FragGrenade __instance)
		{
			Player thrower = Player.Get(__instance.thrower.gameObject);
			Player scp035 = Scp035Data.GetScp035();
			if (thrower != null && scp035 != null && thrower.Id != scp035.Id && thrower.Team == scp035.Team)
			{
				Vector3 position = __instance.transform.position;
				PlayerStats component = scp035.ReferenceHub.GetComponent<PlayerStats>();
				float amount = (float)(__instance.damageOverDistance.Evaluate(Vector3.Distance(position, component.transform.position)) * (component.ccm.IsHuman() ? ConfigFile.ServerConfig.GetFloat("human_grenade_multiplier", 0.7f) : ConfigFile.ServerConfig.GetFloat("scp_grenade_multiplier", 1f)));
				if (amount > __instance.absoluteDamageFalloff)
					component.HurtPlayer(new PlayerStats.HitInfo(amount, (__instance.thrower != null) ? __instance.thrower.hub.LoggedNameFromRefHub() : "(UNKNOWN)", DamageTypes.Grenade, __instance.thrower.hub.queryProcessor.PlayerId), scp035.GameObject, false);
			}
		}
	}
}
