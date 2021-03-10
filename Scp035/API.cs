// -----------------------------------------------------------------------
// <copyright file="API.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035
{
#pragma warning disable SA1135

    using System.Collections.Generic;
    using System.Linq;
    using Components;
    using Exiled.API.Features;

    /// <summary>
    /// The main API class to get and manipulate data.
    /// </summary>
    public static class API
    {
        /// <summary>
        /// Gets all active Scp035 instances.
        /// </summary>
        public static IEnumerable<Player> AllScp035 => Player.List.Where(player => player.SessionVariables.ContainsKey("IsScp035"));

        /// <summary>
        /// Determines if a given <see cref="Player"/> is a Scp035.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to check for being a Scp035 instance.</param>
        /// <returns>A value indicating whether the <see cref="Player"/> is a Scp035 instance.</returns>
        public static bool IsScp035(Player player) => player.SessionVariables.ContainsKey("IsScp035");

        /// <summary>
        /// Spawns a user as a Scp035 instance.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to spawn in as a Scp035 instance.</param>
        /// <param name="toReplace">The <see cref="Player"/> to replace.</param>
        public static void Spawn035(Player player, Player toReplace = null) => player.GameObject.AddComponent<Scp035Component>().AwakeFunc(toReplace);

        /// <summary>
        /// Spawns the specified amount of Scp035 item instances.
        /// </summary>
        /// <param name="amount">The amount of items to spawn.</param>
        /// <returns>All <see cref="Pickup"/>s that were spawned as Scp035 item instances.</returns>
        public static IEnumerable<Pickup> SpawnItems(int amount) => Methods.SpawnPickups(amount);
    }
}