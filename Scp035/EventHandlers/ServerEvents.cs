// -----------------------------------------------------------------------
// <copyright file="ServerEvents.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.EventHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public class ServerEvents
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public ServerEvents(Plugin plugin) => this.plugin = plugin;

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnEndingRound(EndingRoundEventArgs)"/>
        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (!API.AllScp035.Any())
                return;

            List<Team> teams = (from player in Player.List where !API.IsScp035(player) select player.Team).ToList();

            if (teams.All(team => (team == Team.TUT && plugin.Config.WinWithTutorial) || team == Team.SCP || team == Team.RIP))
            {
                ev.LeadingTeam = LeadingTeam.Anomalies;
                ev.IsRoundEnded = true;
            }
            else
            {
                ev.IsAllowed = false;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted"/>
        public void OnRoundStarted()
        {
            Methods.IsRotating = true;
            Methods.ScpPickups.Clear();

            Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.RunSpawning()));
            Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.CorrodePlayers()));

            if (plugin.Config.RangedNotification.IsEnabled)
            {
                Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.RangedNotification()));
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers"/>
        public void OnWaitingForPlayers()
        {
            Methods.KillAllCoroutines();
        }
    }
}