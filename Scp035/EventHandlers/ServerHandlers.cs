// -----------------------------------------------------------------------
// <copyright file="ServerHandlers.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.EventHandlers
{
    using MEC;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public static class ServerHandlers
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted"/>
        internal static void OnRoundStarted()
        {
            Methods.IsRotating = true;
            Methods.ScpPickups.Clear();

            Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.RunSpawning()));
            Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.CorrodePlayers()));
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers"/>
        internal static void OnWaitingForPlayers()
        {
            Methods.KillAllCoroutines();
        }
    }
}