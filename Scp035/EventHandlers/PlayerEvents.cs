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
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using NorthwoodLib.Pools;
    using UnityEngine;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public static class PlayerEvents
    {
        private static readonly Config Config = Plugin.Instance.Config;

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        internal static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsEscaped)
                API.Destroy035(ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDestroying(DestroyingEventArgs)"/>
        internal static void OnDestroying(DestroyingEventArgs ev)
        {
            API.Destroy035(ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDied(DiedEventArgs)"/>
        internal static void OnDied(DiedEventArgs ev)
        {
            API.Destroy035(ev.Target);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnEnteringPocketDimension(EnteringPocketDimensionEventArgs)"/>
        internal static void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (API.IsScp035(ev.Player) && !Config.ScpFriendlyFire)
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnEscaping(EscapingEventArgs)"/>
        internal static void OnEscaping(EscapingEventArgs ev)
        {
            if (API.IsScp035(ev.Player))
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnHurting(HurtingEventArgs)"/>
        internal static void OnHurting(HurtingEventArgs ev)
        {
            if (Methods.FriendlyFireUsers.Contains(ev.Attacker.UserId))
                Methods.RemoveFf(ev.Attacker);

            bool targetIs035 = API.IsScp035(ev.Target);
            if (targetIs035 && ev.DamageType == DamageTypes.Pocket && !Config.Scp035Modifiers.PocketDamage)
            {
                ev.IsAllowed = false;
                return;
            }

            if (targetIs035 && ev.DamageType == DamageTypes.Grenade)
                ev.Amount *= 1.428571f;

            bool attackerIs035 = API.IsScp035(ev.Attacker);
            if ((!targetIs035 && !attackerIs035) || ev.Attacker == ev.Target)
                return;

            if (!Config.ScpFriendlyFire && ((ev.Target.Team == Team.SCP || ev.Attacker.Team == Team.SCP) || (attackerIs035 && targetIs035)))
            {
                ev.IsAllowed = false;
                return;
            }

            if (!Config.TutorialFriendlyFire && (ev.Target.Team == Team.TUT || ev.Attacker.Team == Team.TUT))
            {
                ev.IsAllowed = false;
                return;
            }

            if (ev.Attacker.Side == ev.Target.Side)
                Methods.GrantFf(ev.Attacker);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnInsertingGeneratorTablet(InsertingGeneratorTabletEventArgs)"/>
        internal static void OnInsertingGeneratorTablet(InsertingGeneratorTabletEventArgs ev)
        {
            if (API.IsScp035(ev.Player) && !Config.ScpFriendlyFire)
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnMedicalItemUsed(UsedMedicalItemEventArgs)"/>
        internal static void OnMedicalItemUsed(UsedMedicalItemEventArgs ev)
        {
            if (!API.IsScp035(ev.Player))
                return;

            int maxHp = ev.Player.ReferenceHub.characterClassManager.CurRole.maxHP;
            if (!Config.Scp035Modifiers.CanHealBeyondHostHp &&
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

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnPickingUpItem(PickingUpItemEventArgs)"/>
        internal static void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!API.IsScp035Item(ev.Pickup))
                return;

            if (ev.Player.SessionVariables.ContainsKey("IsGhostSpectator"))
                return;

            Log.Debug($"{ev.Player.Nickname} attempted to pickup a Scp035 object.", Config.Debug);
            ev.IsAllowed = false;
            if (API.IsScp035(ev.Player))
            {
                Log.Debug($"Pickup failed because {ev.Player.Nickname} is already a Scp035.", Config.Debug);
                return;
            }

            if (Config.Scp035Modifiers.SelfInflict)
            {
                ev.Pickup.Delete();
                API.Spawn035(ev.Player);
                return;
            }

            List<Player> players = ListPool<Player>.Shared.Rent(Player.List.Where(x => x.IsDead && !x.IsOverwatchEnabled));
            if (players.Count == 0)
                return;

            Player player = players[Random.Range(0, players.Count)];
            ev.Pickup.Delete();
            API.Spawn035(player, ev.Player);
            ListPool<Player>.Shared.Return(players);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnShooting(ShootingEventArgs)"/>
        internal static void OnShooting(ShootingEventArgs ev)
        {
            if (Methods.FriendlyFireUsers.Contains(ev.Shooter.UserId))
            {
                Methods.RemoveFf(ev.Shooter);
            }

            if (ev.Target == null || !(Player.Get(ev.Target) is Player target))
                return;

            if (!API.IsScp035(target) && !API.IsScp035(ev.Shooter))
                return;

            if (!Config.ScpFriendlyFire && ((target.Team == Team.SCP || ev.Shooter.Team == Team.SCP) || (API.IsScp035(ev.Shooter) && API.IsScp035(target))))
            {
                ev.IsAllowed = false;
                return;
            }

            if (!Config.TutorialFriendlyFire && (target.Team == Team.TUT || ev.Shooter.Team == Team.TUT))
            {
                ev.IsAllowed = false;
                return;
            }

            if (target.Side == ev.Shooter.Side)
            {
                Methods.GrantFf(ev.Shooter);
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnShooting(ShootingEventArgs)"/>
        internal static void OnUsingMedicalItem(UsingMedicalItemEventArgs ev)
        {
            if (!API.IsScp035(ev.Player))
                return;

            if (ev.Item.IsMedical() && ((!Config.Scp035Modifiers.CanHealBeyondHostHp &&
                                         ev.Player.Health >=
                                         ev.Player.ReferenceHub.characterClassManager.CurRole.maxHP) ||
                                        !Config.Scp035Modifiers.CanUseMedicalItems))
            {
                ev.IsAllowed = false;
            }
        }
    }
}