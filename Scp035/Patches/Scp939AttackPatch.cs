// -----------------------------------------------------------------------
// <copyright file="Scp939AttackPatch.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Patches
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;
    using PlayableScps;
    using UnityEngine;

    /// <summary>
    /// Removes the <see cref="EffectType.Amnesia"/> effect from a Scp035 when bitten by a Scp939.
    /// </summary>
    [HarmonyPatch(typeof(Scp939), nameof(Scp939.ServerAttack))]
    internal static class Scp939AttackPatch
    {
        private static void Postfix(GameObject target)
        {
            Player player = Player.Get(target);
            if (API.IsScp035(player) && !Plugin.Instance.Config.ScpFriendlyFire)
            {
                player.DisableEffect(EffectType.Amnesia);
            }
        }
    }
}