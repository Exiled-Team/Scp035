// -----------------------------------------------------------------------
// <copyright file="FlashablePatch.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Patches
{
#pragma warning disable SA1313

    using CustomPlayerEffects;
    using Exiled.API.Features;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// Allows for Scp035 to flash/be flashed by teammates.
    /// </summary>
    [HarmonyPatch(typeof(Flashed), nameof(Flashed.Flashable))]
    internal static class FlashablePatch
    {
        private static bool Prefix(Flashed __instance, ref bool __result, ReferenceHub throwerPlayerHub, Vector3 sourcePosition, int ignoreMask)
        {
            Player thrower = Player.Get(throwerPlayerHub);
            Player target = Player.Get(__instance.Hub);
            bool scp035Applies = false;
            var config = Scp035.Instance.Config;
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

            __result = __instance.Hub != throwerPlayerHub && (throwerPlayerHub.weaponManager.GetShootPermission(__instance.Hub.characterClassManager.CurRole.team) || scp035Applies) && !Physics.Linecast(sourcePosition, __instance.Hub.PlayerCameraReference.position, ignoreMask);
            return false;
        }
    }
}