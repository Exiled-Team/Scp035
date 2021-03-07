// -----------------------------------------------------------------------
// <copyright file="Scp035Component.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Components
{
#pragma warning disable SA1101
#pragma warning disable SA1135

    using System.Collections.Generic;
    using Configs;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// The main Scp035 player controller.
    /// </summary>
    public class Scp035Component : MonoBehaviour
    {
        private static Config config;
        private static Player player;

        /// <summary>
        /// Gets a value indicating whether Scp035 has been fully set up.
        /// </summary>
        internal bool Initialized { get; private set; }

        /// <summary>
        /// The method to initialize Scp035.
        /// </summary>
        /// <param name="toReplace">The <see cref="Exiled.API.Features.Player"/> to replace.</param>
        internal void AwakeFunc(Player toReplace)
        {
            player = Player.Get(gameObject);
            if (player == null)
            {
                Destroy();
                return;
            }

            config = Scp035.Instance.Config;

            SubscribeEvents();

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

            uint ammo = config.Scp035Modifiers.AmmoAmount;
            player.Ammo[(int)AmmoType.Nato556] = ammo;
            player.Ammo[(int)AmmoType.Nato762] = ammo;
            player.Ammo[(int)AmmoType.Nato9] = ammo;

            player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
            player.CustomInfo = "<color=#FF0000>SCP-035</color>";

            player.SessionVariables.Add("IsScp035", true);

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
            Initialized = true;
        }

        /// <summary>
        /// Handles the destruction of a Scp035 instance.
        /// </summary>
        internal void Destroy()
        {
            player.SessionVariables.Remove("IsScp035");
            if (API.AllScp035.IsEmpty())
            {
                Methods.IsRotating = true;
            }

            Scp096.TurnedPlayers.Remove(player);
            Scp173.TurnedPlayers.Remove(player);

            player.CustomInfo = string.Empty;
            player.ReferenceHub.nicknameSync.ShownPlayerInfo |= PlayerInfoArea.Role;

            player.MaxHealth = player.ReferenceHub.characterClassManager.CurRole.maxHP;

            if (config.ItemSpawning.SpawnAfterDeath)
            {
                Methods.SpawnPickups(config.ItemSpawning.InfectedItemCount);
            }

            UnSubscribeEvents();
            Destroy(this);
        }

        private static void OnContaining(ContainingEventArgs ev)
        {
            if (ev.Player == player && !config.ScpFriendlyFire)
            {
                ev.IsAllowed = false;
            }
        }

        private static void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
        {
            if (ev.Player == player && !config.ScpFriendlyFire)
            {
                ev.IsAllowed = false;
            }
        }

        private static void OnEscaping(EscapingEventArgs ev)
        {
            if (ev.Player == player)
            {
                ev.IsAllowed = false;
            }
        }

        private static void OnInsertingGeneratorTablet(InsertingGeneratorTabletEventArgs ev)
        {
            if (player == ev.Player && !config.ScpFriendlyFire)
            {
                ev.IsAllowed = false;
            }
        }

        private static void OnMedicalItemUsed(UsedMedicalItemEventArgs ev)
        {
            if (ev.Player != player)
            {
                return;
            }

            int maxHp = ev.Player.ReferenceHub.characterClassManager.CurRole.maxHP;
            if (!config.Scp035Modifiers.CanHealBeyondHostHp &&
                ev.Player.Health > maxHp &&
                (ev.Item.IsMedical() || ev.Item == ItemType.SCP207))
            {
                if (ev.Item == ItemType.SCP207)
                {
                    ev.Player.Health = Mathf.Max(maxHp, ev.Player.Health - 30);
                }
                else
                {
                    ev.Player.Health = maxHp;
                }
            }
        }

        private static void OnUsingMedicalItem(UsingMedicalItemEventArgs ev)
        {
            if (ev.Player != player)
            {
                return;
            }

            if (ev.Item.IsMedical() && ((!config.Scp035Modifiers.CanHealBeyondHostHp &&
                                        ev.Player.Health >=
                                        ev.Player.ReferenceHub.characterClassManager.CurRole.maxHP) ||
                                        !config.Scp035Modifiers.CanUseMedicalItems))
            {
                ev.IsAllowed = false;
            }
        }

        private static void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp106.Containing += OnContaining;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            Exiled.Events.Handlers.Player.InsertingGeneratorTablet += OnInsertingGeneratorTablet;
            Exiled.Events.Handlers.Player.MedicalItemUsed += OnMedicalItemUsed;
            Exiled.Events.Handlers.Player.UsingMedicalItem += OnUsingMedicalItem;
        }

        private static void UnSubscribeEvents()
        {
            Exiled.Events.Handlers.Scp106.Containing -= OnContaining;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnPocketDimensionEnter;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            Exiled.Events.Handlers.Player.InsertingGeneratorTablet -= OnInsertingGeneratorTablet;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= OnMedicalItemUsed;
            Exiled.Events.Handlers.Player.UsingMedicalItem -= OnUsingMedicalItem;
        }
    }
}