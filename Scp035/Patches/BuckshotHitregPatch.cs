// -----------------------------------------------------------------------
// <copyright file="BuckshotHitregPatch.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Patches
{
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.Firearms.Modules;
    using UnityEngine;

    [HarmonyPatch(typeof(BuckshotHitreg), nameof(BuckshotHitreg.ServerPerformShot))]
    internal static class BuckshotHitregPatch
    {
        private static bool Prefix(BuckshotHitreg __instance, Ray shootRay)
        {
            Vector2 offsetVector = (new Vector2(Random.value, Random.value) - (Vector2.one / 2f)).normalized * Random.value * (__instance.Firearm.BaseStats.GetInaccuracy(__instance.Firearm, __instance.Firearm.AdsModule.ServerAds, __instance.Hub.playerMovementSync.PlayerVelocity.magnitude, __instance.Hub.playerMovementSync.Grounded) * 0.4f);
            float num = 0;
            BuckshotHitreg.AlreadyHitNetIds.Clear();
            for (int index = 0; index < __instance._buckshotSettings.PredefinedPellets.Length; ++index)
            {
                if (ShootPellet(__instance, __instance._buckshotSettings.PredefinedPellets[index], shootRay, offsetVector))
                    ++num;

                if (num >= __instance._buckshotSettings.MaxHits)
                    break;
            }

            if (num == 0)
                return false;

            Hitmarker.SendHitmarker(__instance.Conn, (num / __instance._buckshotSettings.MaxHits) + 0.5f);
            return false;
        }

        private static bool ShootPellet(BuckshotHitreg instance, Vector2 pelletSettings, Ray originalRay, Vector2 offsetVector)
        {
            Vector2 vector2 = Vector2.Lerp(pelletSettings, instance.GenerateRandomPelletDirection, instance.BuckshotRandomness) * instance.BuckshotScale;
            Vector3 direction1 = originalRay.direction;
            Vector3 vector3 = Quaternion.AngleAxis(vector2.x + offsetVector.x, instance.Hub.PlayerCameraReference.up) * direction1;
            Vector3 direction2 = Quaternion.AngleAxis(vector2.y + offsetVector.y, instance.Hub.PlayerCameraReference.right) * vector3;
            Ray ray = new Ray(originalRay.origin, direction2);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, instance.Firearm.BaseStats.MaxDistance(), StandardHitregBase.HitregMask))
            {
                if (hitInfo.collider.TryGetComponent(out HitboxIdentity hitboxIdentity))
                {
                    Player target = Player.Get(hitboxIdentity.TargetHub);
                    if (!Plugin.Instance.PlayerEvents.CanHurt(Player.Get(instance.Hub), target))
                        return false;

                    float damage = instance.Firearm.BaseStats.DamageAtDistance(instance.Firearm, hitInfo.distance) / instance._buckshotSettings.MaxHits;
                    target.Hurt(damage, DamageTypes.Shotgun, instance.Hub.LoggedNameFromRefHub(), instance.Hub.playerId);
                    return true;
                }

                if (hitInfo.collider.TryGetComponent(out IDestructible component))
                {
                    instance.RestorePlayerPosition();
                    float damage = instance.Firearm.BaseStats.DamageAtDistance(instance.Firearm, hitInfo.distance) / instance._buckshotSettings.MaxHits;
                    if (instance.CanShoot(component.NetworkId) && component.Damage(damage, instance.Firearm, instance.Firearm.Footprint, hitInfo.point))
                    {
                        instance.ShowHitIndicator(component.NetworkId, damage, originalRay.origin);
                        return true;
                    }
                }

                instance.PlaceBullethole(ray, hitInfo);
            }

            return false;
        }
    }
}