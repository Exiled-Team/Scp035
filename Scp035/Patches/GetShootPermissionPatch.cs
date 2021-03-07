// -----------------------------------------------------------------------
// <copyright file="GetShootPermissionPatch.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Patches
{
#pragma warning disable SA1313

    using Exiled.API.Features;
    using HarmonyLib;

    [HarmonyPatch(typeof(WeaponManager), nameof(WeaponManager.GetShootPermission), typeof(CharacterClassManager), typeof(bool))]
    internal static class GetShootPermissionPatch
    {
        private static bool Prefix(WeaponManager __instance, ref bool __result, CharacterClassManager c)
        {
            Player shooter = Player.Get(c._hub);
            Player target = Player.Get(__instance._hub);
            if (!API.IsScp035(shooter) && !API.IsScp035(target))
            {
                return true;
            }

            var config = Scp035.Instance.Config;
            if (API.IsScp035(shooter))
            {
                __result = (target.IsScp && config.ScpFriendlyFire) ||
                           (target.Role == RoleType.Tutorial && config.TutorialFriendlyFire);

                return false;
            }

            __result = (shooter.IsScp && config.ScpFriendlyFire) ||
                       (shooter.Role == RoleType.Tutorial && config.TutorialFriendlyFire);
            return false;
        }
    }
}