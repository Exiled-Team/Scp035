// -----------------------------------------------------------------------
// <copyright file="MapEvents.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.EventHandlers
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MonoMod.Utils;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Map"/>.
    /// </summary>
    public static class MapEvents
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Map.OnExplodingGrenade(ExplodingGrenadeEventArgs)"/>
        internal static void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            var dict = new Dictionary<Player, float>(ev.TargetToDamages);
            foreach (var kvp in ev.TargetToDamages)
            {
                if (API.IsScp035(kvp.Key))
                {
                    dict[kvp.Key] *= 1.428571f;
                }
            }

            ev.TargetToDamages.Clear();
            ev.TargetToDamages.AddRange(dict);
        }
    }
}