// -----------------------------------------------------------------------
// <copyright file="Methods.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035
{
#pragma warning disable SA1135

    using System.Collections.Generic;
    using System.Linq;
    using Configs;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using MEC;
    using Mirror;
    using UnityEngine;
    using Random = System.Random;

    /// <summary>
    /// Contains all methods and various tracking.
    /// </summary>
    public static class Methods
    {
        private static readonly Config Config = Scp035.Instance.Config;

        private static readonly Random Random = new Random();

        /// <summary>
        /// Gets all active coroutines.
        /// </summary>
        internal static List<CoroutineHandle> CoroutineHandles { get; } = new List<CoroutineHandle>();

        /// <summary>
        /// Gets all user ids that currently have friendly fire enabled.
        /// </summary>
        internal static List<string> FriendlyFireUsers { get; } = new List<string>();

        /// <summary>
        /// Gets all Scp035 item instances.
        /// </summary>
        internal static List<Pickup> ScpPickups { get; } = new List<Pickup>();

        /// <summary>
        /// Gets or sets a value indicating whether a Scp035 item instance can spawn.
        /// </summary>
        internal static bool IsRotating { get; set; }

        /// <summary>
        /// Kills all active coroutines.
        /// </summary>
        internal static void KillAllCoroutines()
        {
            Timing.KillCoroutines(CoroutineHandles.ToArray());
            CoroutineHandles.Clear();
        }

        /// <summary>
        /// Grants tracked friendly fire to a user.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to be given friendly fire.</param>
        internal static void GrantFf(Player player)
        {
            player.IsFriendlyFireEnabled = true;
            FriendlyFireUsers.Add(player.UserId);
        }

        /// <summary>
        /// Revokes tracked friendly fire from a user.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to be removed from friendly fire.</param>
        internal static void RemoveFf(Player player)
        {
            player.IsFriendlyFireEnabled = false;
            FriendlyFireUsers.Remove(player.UserId);
        }

        /// <summary>
        /// Removes the <see cref="Components.Scp035Component"/>, if initialized, from a player.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to be removed from being Scp035.</param>
        internal static void DestroyScp035(Player player)
        {
            if (!API.IsScp035(player))
            {
                return;
            }

            if (player.GameObject.TryGetComponent(out Components.Scp035Component scp035Component) &&
                scp035Component.Initialized)
            {
                scp035Component.Destroy();
            }
        }

        /// <summary>
        /// Deletes all active Scp035 item instances.
        /// </summary>
        internal static void RemoveScpPickups()
        {
            foreach (var pickup in ScpPickups)
            {
                if (pickup.InUse)
                {
                    pickup.Locked = true;
                }

                if (pickup != null)
                {
                    pickup.Delete();
                }
            }

            ScpPickups.Clear();
        }

        /// <summary>
        /// Spawns Scp035 item instances.
        /// </summary>
        /// <param name="amount">The number of Scp035 items to spawn.</param>
        /// <returns>All spawned <see cref="Pickup"/> objects.</returns>
        internal static List<Pickup> SpawnPickups(int amount)
        {
            Log.Debug($"Running {nameof(SpawnPickups)}.", Config.Debug);
            RemoveScpPickups();

            List<Pickup> pickups = Pickup.Instances.Where(pickup => !ScpPickups.Contains(pickup)).ToList();
            if (Warhead.IsDetonated)
            {
                pickups.RemoveAll(pickup => Map.FindParentRoom(pickup.gameObject).Type != RoomType.Surface);
            }

            List<Pickup> returnPickups = new List<Pickup>();
            for (int i = 0; i < amount; i++)
            {
                if (pickups.Count == 0)
                {
                    return returnPickups;
                }

                Pickup mimicAs = pickups[Random.Next(pickups.Count)];
                Transform transform = mimicAs.transform;
                Pickup scpPickup = Config.ItemSpawning
                    .PossibleItems[Random.Next(Config.ItemSpawning.PossibleItems.Length)]
                    .Spawn(0, transform.position, transform.rotation);

                Log.Debug($"Spawned Scp035 item with ItemType of {scpPickup.itemId} at {scpPickup.transform.position}");
                ScpPickups.Add(scpPickup);
                returnPickups.Add(scpPickup);

                pickups.Remove(mimicAs);
            }

            return returnPickups;
        }

        /// <summary>
        /// A coroutine which loops the spawning of Scp035 item instances.
        /// </summary>
        /// <returns>A delay in seconds based on <see cref="Configs.SubConfigs.ItemSpawning.RotateInterval"/>.</returns>
        internal static IEnumerator<float> RunSpawning()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(Config.ItemSpawning.RotateInterval);
                Log.Debug($"Running {nameof(RunSpawning)} loop.", Config.Debug);
                if (IsRotating)
                {
                    SpawnPickups(Config.ItemSpawning.InfectedItemCount);
                }
            }
        }

        /// <summary>
        /// A coroutine which deals damage to the host of Scp035 over time.
        /// </summary>
        /// <param name="player">The Scp035 instance to corrode.</param>
        /// <returns>A delay in seconds based on <see cref="Configs.SubConfigs.CorrodeHost.Interval"/>.</returns>
        internal static IEnumerator<float> CorrodeHost(Player player)
        {
            while (player != null && API.IsScp035(player))
            {
                yield return Timing.WaitForSeconds(Config.CorrodeHost.Interval);
                Log.Debug($"Running {nameof(CorrodeHost)} loop.", Config.Debug);
                player.Hurt(Config.CorrodeHost.Damage);
            }
        }

        /// <summary>
        /// A coroutine which deals damage to players near a Scp035 instance.
        /// </summary>
        /// <returns>A delay in seconds based on <see cref="Configs.SubConfigs.CorrodePlayers.Interval"/>.</returns>
        internal static IEnumerator<float> CorrodePlayers()
        {
            if (!Config.CorrodePlayers.IsEnabled)
            {
                yield break;
            }

            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(Config.CorrodePlayers.Interval);
                Log.Debug($"Running {nameof(CorrodePlayers)} loop.", Config.Debug);
                List<Player> scp035List = API.AllScp035.ToList();
                if (scp035List.Count == 0)
                {
                    continue;
                }

                foreach (var player in GetValidPlayers())
                {
                    foreach (var scp035 in scp035List)
                    {
                        if (Vector3.Distance(scp035.Position, player.Position) <= Config.CorrodePlayers.Distance)
                        {
                            CorrodePlayer(player);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Runs the ranged notification loop check.
        /// </summary>
        /// <returns>A delay in seconds based on <see cref="Configs.SubConfigs.RangedNotification.Interval"/>.</returns>
        internal static IEnumerator<float> RangedNotification()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(Config.RangedNotification.Interval);
                Broadcast broadcast = Config.RangedNotification.Notification;
                if (!API.AllScp035.Any() || !broadcast.Show)
                {
                    continue;
                }

                foreach (Player player in Player.List)
                {
                    Vector3 forward = player.CameraTransform.forward;
                    if (Physics.Raycast(player.CameraTransform.position + forward, forward, out var hit, Config.RangedNotification.MaximumRange, player.ReferenceHub.weaponManager.raycastMask))
                    {
                        if (hit.distance < Config.RangedNotification.MinimumRange)
                        {
                            continue;
                        }

                        if (hit.collider.GetComponentInParent<HitboxIdentity>() != null)
                        {
                            var parent = hit.collider.GetComponentInParent<NetworkIdentity>().gameObject;
                            var hitCcm = parent.GetComponent<CharacterClassManager>();

                            if (Player.Get(hitCcm._hub) is Player target && API.IsScp035(target))
                            {
                                if (Config.RangedNotification.UseHints)
                                {
                                    player.ShowHint(broadcast.Content, broadcast.Duration);
                                }
                                else
                                {
                                    player.Broadcast(broadcast);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void CorrodePlayer(Player player)
        {
            player.Hurt(Config.CorrodePlayers.Damage, DamageTypes.Pocket);
            List<Player> scp035List = API.AllScp035.ToList();
            if (!Config.CorrodePlayers.LifeSteal || scp035List.Count == 0)
            {
                return;
            }

            foreach (var scp035 in scp035List)
            {
                HealPlayer(scp035, Config.CorrodePlayers.Damage);
            }
        }

        private static void HealPlayer(Player player, int amount)
        {
            if (player.Health + amount > player.MaxHealth)
            {
                player.Health = player.MaxHealth;
            }
            else
            {
                player.Health += amount;
            }
        }

        private static IEnumerable<Player> GetValidPlayers()
        {
            foreach (var player in Player.List)
            {
                if ((!Config.ScpFriendlyFire && player.IsScp) || (!Config.TutorialFriendlyFire && player.Role == RoleType.Tutorial) || API.IsScp035(player))
                {
                    continue;
                }

                if (player.IsAlive)
                {
                    yield return player;
                }
            }
        }
    }
}