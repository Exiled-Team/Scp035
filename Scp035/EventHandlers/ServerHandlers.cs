// -----------------------------------------------------------------------
// <copyright file="ServerHandlers.cs" company="Build and Cyanox">
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
    public static class ServerHandlers
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnEndingRound(EndingRoundEventArgs)"/>
        internal static void OnEndingRound(EndingRoundEventArgs ev)
        {
            if (!API.AllScp035.Any())
            {
                return;
            }

            List<Team> teams = (from player in Player.List where !API.IsScp035(player) select player.Team).ToList();

            if (teams.All(team => (team == Team.TUT && Scp035.Instance.Config.WinWithTutorial) || team == Team.SCP || team == Team.RIP))
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
        internal static void OnRoundStarted()
        {
            Methods.IsRotating = true;
            Methods.ScpPickups.Clear();

            Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.RunSpawning()));
            Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.CorrodePlayers()));

            if (Scp035.Instance.Config.RangedNotification.IsEnabled)
            {
                Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.RangedNotification()));
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers"/>
        internal static void OnWaitingForPlayers()
        {
            Methods.KillAllCoroutines();
        }
    }
}