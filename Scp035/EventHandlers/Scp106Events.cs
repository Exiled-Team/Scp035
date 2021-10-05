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
    public class Scp106Events
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scp106Events"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public Scp106Events(Plugin plugin) => this.plugin = plugin;

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp106.OnContaining(ContainingEventArgs)"/>
        public void OnContaining(ContainingEventArgs ev)
        {
            if (API.IsScp035(ev.Player) && !plugin.Config.ScpFriendlyFire)
                ev.IsAllowed = false;
        }
    }
}