// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.EventHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using NorthwoodLib.Pools;
    using UnityEngine;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class PlayerEvents
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public PlayerEvents(Plugin plugin) => this.plugin = plugin;

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Reason != SpawnReason.Escaped)
                API.Destroy035(ev.Player);

            if (ev.NewRole == RoleType.Spectator)
            {
                foreach (Player scp035 in API.AllScp035)
                {
                    Timing.CallDelayed(0.5f, () => ev.Player.SendFakeSyncVar(scp035.ReferenceHub.networkIdentity, typeof(NicknameSync), nameof(NicknameSync.Network_displayName), $"{scp035.Nickname} - (Scp035)"));
                }
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDestroying(DestroyingEventArgs)"/>
        public void OnDestroying(DestroyingEventArgs ev)
        {
            API.Destroy035(ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDied(DiedEventArgs)"/>
        public void OnDied(DiedEventArgs ev)
        {
            API.Destroy035(ev.Target);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnEnteringPocketDimension(EnteringPocketDimensionEventArgs)"/>
        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (API.IsScp035(ev.Player) && !plugin.Config.ScpFriendlyFire)
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnEscaping(EscapingEventArgs)"/>
        public void OnEscaping(EscapingEventArgs ev)
        {
            if (API.IsScp035(ev.Player))
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnHurting(HurtingEventArgs)"/>
        public void OnHurting(HurtingEventArgs ev)
        {
            if (Methods.FriendlyFireUsers.Contains(ev.Attacker.UserId))
                Methods.RemoveFf(ev.Attacker);

            bool targetIs035 = API.IsScp035(ev.Target);
            if (targetIs035 && ev.DamageType.Equals(DamageTypes.Pocket) && !plugin.Config.Scp035Modifiers.PocketDamage)
            {
                ev.IsAllowed = false;
                return;
            }

            if (targetIs035 && ((ItemType)ev.DamageType.Weapon).IsThrowable())
                ev.Amount *= 1.428571f;

            bool attackerIs035 = API.IsScp035(ev.Attacker);
            if ((!targetIs035 && !attackerIs035) || ev.Attacker == ev.Target)
                return;

            if (!plugin.Config.ScpFriendlyFire && ((ev.Target.Team == Team.SCP || ev.Attacker.Team == Team.SCP) || (attackerIs035 && targetIs035)))
            {
                ev.IsAllowed = false;
                return;
            }

            if (!plugin.Config.TutorialFriendlyFire && (ev.Target.Team == Team.TUT || ev.Attacker.Team == Team.TUT))
            {
                ev.IsAllowed = false;
                return;
            }

            if (ev.Attacker.Side == ev.Target.Side)
                Methods.GrantFf(ev.Attacker);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnActivatingGenerator(ActivatingGeneratorEventArgs)"/>
        public void OnActivatingGenerator(ActivatingGeneratorEventArgs ev)
        {
            if (API.IsScp035(ev.Player) && !plugin.Config.ScpFriendlyFire)
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnItemUsed(UsedItemEventArgs)"/>
        public void OnItemUsed(UsedItemEventArgs ev)
        {
            if (!API.IsScp035(ev.Player))
                return;

            int maxHp = ev.Player.ReferenceHub.characterClassManager.CurRole.maxHP;
            if (!plugin.Config.Scp035Modifiers.CanHealBeyondHostHp &&
                ev.Player.Health > maxHp &&
                (ev.Item.Type.IsMedical() || ev.Item.Type == ItemType.SCP207))
            {
                if (ev.Item.Type == ItemType.SCP207)
                {
                    ev.Player.Health = Mathf.Max(maxHp, ev.Player.Health - 30);
                }
                else
                {
                    ev.Player.Health = maxHp;
                }
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnPickingUpItem(PickingUpItemEventArgs)"/>
        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!API.IsScp035Item(ev.Pickup))
                return;

            if (ev.Player.SessionVariables.ContainsKey("IsGhostSpectator"))
                return;

            Log.Debug($"{ev.Player.Nickname} attempted to pickup a Scp035 object.", plugin.Config.Debug);
            ev.IsAllowed = false;
            if (API.IsScp035(ev.Player))
            {
                Log.Debug($"Pickup failed because {ev.Player.Nickname} is already a Scp035.", plugin.Config.Debug);
                ev.Pickup.Locked = false;
                return;
            }

            if (plugin.Config.Scp035Modifiers.SelfInflict)
            {
                API.Spawn035(ev.Player);
                return;
            }

            List<Player> players = ListPool<Player>.Shared.Rent(Player.List.Where(x => x.IsDead && !x.IsOverwatchEnabled));
            if (players.Count == 0)
            {
                ev.Pickup.Locked = false;
                return;
            }

            API.Spawn035(players.GetRandom(), ev.Player);
            ListPool<Player>.Shared.Return(players);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnShot(ShotEventArgs)"/>
        public void OnShot(ShotEventArgs ev)
        {
            if (Methods.FriendlyFireUsers.Contains(ev.Shooter.UserId))
            {
                Methods.RemoveFf(ev.Shooter);
            }

            if (!API.IsScp035(ev.Target) && !API.IsScp035(ev.Shooter))
                return;

            if (!plugin.Config.ScpFriendlyFire && ((ev.Target.Team == Team.SCP || ev.Shooter.Team == Team.SCP) || (API.IsScp035(ev.Shooter) && API.IsScp035(ev.Target))))
            {
                ev.CanHurt = false;
                return;
            }

            if (!plugin.Config.TutorialFriendlyFire && (ev.Target.Team == Team.TUT || ev.Shooter.Team == Team.TUT))
            {
                ev.CanHurt = false;
                return;
            }

            if (ev.Target.Side == ev.Shooter.Side)
            {
                Methods.GrantFf(ev.Shooter);
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnUsingItem(UsingItemEventArgs)"/>
        public void OnUsingItem(UsingItemEventArgs ev)
        {
            if (!API.IsScp035(ev.Player))
                return;

            if (ev.Item.Type.IsMedical() && ((!plugin.Config.Scp035Modifiers.CanHealBeyondHostHp &&
                                         ev.Player.Health >=
                                         ev.Player.ReferenceHub.characterClassManager.CurRole.maxHP) ||
                                        !plugin.Config.Scp035Modifiers.CanUseMedicalItems))
            {
                ev.IsAllowed = false;
            }
        }
    }
}