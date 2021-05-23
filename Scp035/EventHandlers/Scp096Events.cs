// -----------------------------------------------------------------------
// <copyright file="Scp096Events.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.EventHandlers
{
    using Exiled.Events.EventArgs;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Scp096"/>.
    /// </summary>
    public static class Scp096Events
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Scp096.OnAddingTarget(AddingTargetEventArgs)"/>
        internal static void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (API.IsScp035(ev.Target))
                ev.IsAllowed = false;
        }
    }
}