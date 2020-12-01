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
	static class FragGrenadeServersideExplosionPatch
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

	[HarmonyPatch(typeof(FlashGrenade), nameof(FlashGrenade.ServersideExplosion))]
	static class FlashGrenadeServersideExplosionPatch
	{
		public static void Postfix(FlashGrenade __instance)
		{
			Player thrower = Player.Get(__instance.thrower.gameObject);
			Player scp035 = Scp035Data.GetScp035();
			if (thrower != null && scp035 != null && thrower.Id != scp035.Id && thrower.Team == scp035.Team)
			{
				GameObject gameObject = scp035.GameObject;
				Vector3 position = __instance.transform.position;
				ReferenceHub hub = ReferenceHub.GetHub(gameObject);
				CustomPlayerEffects.Flashed effect = hub.playerEffectsController.GetEffect<CustomPlayerEffects.Flashed>();
				CustomPlayerEffects.Deafened effect2 = hub.playerEffectsController.GetEffect<CustomPlayerEffects.Deafened>();
				if (effect != null && __instance.thrower != null && Flashable(thrower.ReferenceHub, scp035.ReferenceHub, position, __instance._ignoredLayers))
				{
					float num = __instance.powerOverDistance.Evaluate(Vector3.Distance(gameObject.transform.position, position) / ((position.y > 900f) ? __instance.distanceMultiplierSurface : __instance.distanceMultiplierFacility)) * __instance.powerOverDot.Evaluate(Vector3.Dot(hub.PlayerCameraReference.forward, (hub.PlayerCameraReference.position - position).normalized));
					byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(num * 10f * __instance.maximumDuration), 1, 255);
					if (b >= effect.Intensity && num > 0f)
					{
						hub.playerEffectsController.ChangeEffectIntensity<CustomPlayerEffects.Flashed>(b);
						if (effect2 != null)
						{
							hub.playerEffectsController.EnableEffect(effect2, num * __instance.maximumDuration, true);
						}
					}
				}
			}
		}

		private static bool Flashable(ReferenceHub throwerPlayerHub, ReferenceHub targetPlayerHub, Vector3 sourcePosition, int ignoreMask)
		{
			return targetPlayerHub != throwerPlayerHub && !Physics.Linecast(sourcePosition, targetPlayerHub.PlayerCameraReference.position, ignoreMask);
		}
	}
}
