// -----------------------------------------------------------------------
// <copyright file="PlayFootstepSoundPatch.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using HarmonyLib;

    /// <summary>
    /// Spawns the corrosion trail under a Scp035 instance.
    /// </summary>
    [HarmonyPatch(typeof(FootstepSync), nameof(FootstepSync.PlayFootstepSound))]
    internal static class PlayFootstepSoundPatch
    {
        private static int count;

        private static void Postfix(FootstepSync __instance)
        {
            if (!Plugin.Instance.Config.CorrodeTrail)
                return;

            Player player = Player.Get(__instance.gameObject);
            count++;

            foreach (var scp035 in API.AllScp035)
            {
                if (player.Id == scp035?.Id && count >= Plugin.Instance.Config.CorrodeTrailInterval)
                {
                    player.ReferenceHub.characterClassManager.RpcPlaceBlood(player.Position, 1, 2f);
                    count = 0;
                }
            }
        }
    }
}