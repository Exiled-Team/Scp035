// -----------------------------------------------------------------------
// <copyright file="Scp106Events.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.EventHandlers
{
    using Exiled.Events.EventArgs;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Scp106"/>.
    /// </summary>
    public static class Scp106Events
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Scp106.OnContaining(ContainingEventArgs)"/>
        internal static void OnContaining(ContainingEventArgs ev)
        {
            if (API.IsScp035(ev.Player) && !Plugin.Instance.Config.ScpFriendlyFire)
            {
                ev.IsAllowed = false;
            }
        }
    }
}