// -----------------------------------------------------------------------
// <copyright file="FlashablePatch.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.ThrowableProjectiles;

    /// <summary>
    /// Allows for Scp035 to flash/be flashed by teammates.
    /// </summary>
    [HarmonyPatch(typeof(FlashbangGrenade), nameof(FlashbangGrenade.PlayExplosionEffects))]
    internal static class FlashablePatch
    {
        private static bool Prefix(FlashbangGrenade __instance)
        {
            double time = __instance._blindingOverDistance.keys[__instance._blindingOverDistance.length - 1].time;
            float num = (float)(time * time);
            Player thrower = Player.Get(__instance.PreviousOwner.Hub);
            foreach (ReferenceHub hub in ReferenceHub.GetAllHubs().Values)
            {
                if (!(hub == null) && (__instance.transform.position - hub.transform.position).sqrMagnitude <= (double)num && !(hub == __instance.PreviousOwner.Hub)
                    && (CheckScp035Application(thrower, Player.Get(hub)) || HitboxIdentity.CheckFriendlyFire(__instance.PreviousOwner.Role, hub.characterClassManager.CurClass)))
                    __instance.ProcessPlayer(hub);
            }

            return false;
        }

        private static bool CheckScp035Application(Player thrower, Player target)
        {
            var config = Plugin.Instance.Config;

            bool scp035Applies = false;
            if (API.IsScp035(thrower))
            {
                scp035Applies = (target.IsScp && config.ScpFriendlyFire) ||
                                 (target.Role == RoleType.Tutorial && config.TutorialFriendlyFire);
            }
            else if (API.IsScp035(target))
            {
                scp035Applies = (thrower.IsScp && config.ScpFriendlyFire) ||
                                 (thrower.Role == RoleType.Tutorial && config.TutorialFriendlyFire);
            }

            return scp035Applies;
        }
    }
}