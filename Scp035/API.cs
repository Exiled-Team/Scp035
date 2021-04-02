// -----------------------------------------------------------------------
// <copyright file="API.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;
    using Scp035.Configs;
    using UnityEngine;

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
        /// Removes a player from being considered as Scp035.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to be removed from being Scp035.</param>
        public static void Destroy035(Player player)
        {
            if (!IsScp035(player))
            {
                return;
            }

            player.SessionVariables.Remove("IsScp035");
            if (AllScp035.IsEmpty())
            {
                Methods.IsRotating = true;
            }

            Scp096.TurnedPlayers.Remove(player);
            Scp173.TurnedPlayers.Remove(player);

            player.CustomInfo = string.Empty;
            player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;

            player.MaxHealth = player.ReferenceHub.characterClassManager.CurRole.maxHP;

            ItemSpawning itemSpawning = Plugin.Instance.Config.ItemSpawning;
            if (itemSpawning.SpawnAfterDeath)
            {
                Methods.SpawnPickups(itemSpawning.InfectedItemCount);
            }
        }

        /// <summary>
        /// Determines if a given <see cref="Player"/> is a Scp035.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to check for being a Scp035 instance.</param>
        /// <returns>A value indicating whether the <see cref="Player"/> is a Scp035 instance.</returns>
        public static bool IsScp035(Player player) => player.SessionVariables.ContainsKey("IsScp035");

        /// <summary>
        /// Gets a value indicating whether the <see cref="Pickup"/> is considered to be a Scp035 item.
        /// </summary>
        /// <param name="pickup">The item to check.</param>
        /// <returns>If the item is a Scp035 item.</returns>
        public static bool IsScp035Item(Pickup pickup) => Methods.ScpPickups.Contains(pickup);

        /// <summary>
        /// Spawns a user as a Scp035 instance.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to spawn in as a Scp035 instance.</param>
        /// <param name="toReplace">The <see cref="Player"/> to replace.</param>
        public static void Spawn035(Player player, Player toReplace = null)
        {
            if (player == null)
            {
                return;
            }

            Methods.IsRotating = false;

            if (toReplace != null && player.UserId != toReplace.UserId)
            {
                List<Inventory.SyncItemInfo> items = new List<Inventory.SyncItemInfo>(toReplace.Inventory.items);
                toReplace.ClearInventory();

                Vector3 position = toReplace.Position;
                player.Role = toReplace.Role;
                player.ResetInventory(items);

                toReplace.Role = RoleType.Spectator;
                Timing.CallDelayed(0.5f, () => player.Position = position);
            }

            Config config = Plugin.Instance.Config;

            uint ammo = config.Scp035Modifiers.AmmoAmount;
            player.Ammo[(int)AmmoType.Nato556] = ammo;
            player.Ammo[(int)AmmoType.Nato762] = ammo;
            player.Ammo[(int)AmmoType.Nato9] = ammo;

            player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
            player.CustomInfo = "<color=#FF0000>SCP-035</color>";

            Scp096.TurnedPlayers.Add(player);
            Scp173.TurnedPlayers.Add(player);

            player.Health = player.MaxHealth = config.Scp035Modifiers.Health;

            Vector3 scale = config.Scp035Modifiers.Scale.ToVector3();
            if (player.Scale != scale)
            {
                player.Scale = scale;
            }

            if (config.Scp035Modifiers.SpawnBroadcast.Show)
            {
                player.Broadcast(config.Scp035Modifiers.SpawnBroadcast);
            }

            if (config.CorrodeHost.IsEnabled)
            {
                Methods.CoroutineHandles.Add(Timing.RunCoroutine(Methods.CorrodeHost(player)));
            }

            Methods.RemoveScpPickups();
            player.SessionVariables.Add("IsScp035", true);
        }

        /// <summary>
        /// Spawns the specified amount of Scp035 item instances.
        /// </summary>
        /// <param name="amount">The amount of items to spawn.</param>
        /// <returns>All <see cref="Pickup"/>s that were spawned as Scp035 item instances.</returns>
        public static IEnumerable<Pickup> SpawnItems(int amount) => Methods.SpawnPickups(amount);
    }
}