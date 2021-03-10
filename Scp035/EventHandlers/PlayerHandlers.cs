// -----------------------------------------------------------------------
// <copyright file="PlayerHandlers.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.EventHandlers
{
#pragma warning disable SA1135

    using System.Linq;
    using Components;
    using Configs;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    /// <summary>
    /// All event handlers which use <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public static class PlayerHandlers
    {
        private static readonly Config Config = Scp035.Instance.Config;

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        internal static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Methods.DestroyScp035(ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDestroying(DestroyingEventArgs)"/>
        internal static void OnDestroying(DestroyingEventArgs ev)
        {
            Methods.DestroyScp035(ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDied(DiedEventArgs)"/>
        internal static void OnDied(DiedEventArgs ev)
        {
            Methods.DestroyScp035(ev.Target);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnPickingUpItem(PickingUpItemEventArgs)"/>
        internal static void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (!Methods.ScpPickups.Contains(ev.Pickup))
            {
                return;
            }

            Log.Debug($"{ev.Player.Nickname} attempted to pickup a Scp035 object.", Config.Debug);
            ev.IsAllowed = false;
            if (API.IsScp035(ev.Player))
            {
                Log.Debug($"Pickup failed because {ev.Player.Nickname} is already a Scp035.", Config.Debug);
                return;
            }

            if (Config.Scp035Modifiers.SelfInflict)
            {
                ev.Player.GameObject.AddComponent<Scp035Component>().AwakeFunc(null);
                return;
            }

            Player player = Player.Get(Team.RIP).FirstOrDefault(ply => !ply.IsOverwatchEnabled);
            if (player == null)
            {
                Log.Debug("There were no spectators to spawn Scp035 as, cancelling pickup.", Config.Debug);
                return;
            }

            ev.Pickup.Delete();
            player.GameObject.AddComponent<Scp035Component>().AwakeFunc(ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnHurting(HurtingEventArgs)"/>
        internal static void OnHurting(HurtingEventArgs ev)
        {
            if (Methods.FriendlyFireUsers.Contains(ev.Attacker.UserId))
            {
                Methods.RemoveFf(ev.Attacker);
            }

            if ((!API.IsScp035(ev.Target) && !API.IsScp035(ev.Attacker)) || ev.Attacker == ev.Target)
            {
                return;
            }

            if (!Config.ScpFriendlyFire && ((ev.Target.Team == Team.SCP || ev.Attacker.Team == Team.SCP) || (API.IsScp035(ev.Attacker) && API.IsScp035(ev.Target))))
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
            {
                Methods.GrantFf(ev.Attacker);
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnShooting(ShootingEventArgs)"/>
        internal static void OnShooting(ShootingEventArgs ev)
        {
            if (Methods.FriendlyFireUsers.Contains(ev.Shooter.UserId))
            {
                Methods.RemoveFf(ev.Shooter);
            }

            if (ev.Target == null || !(Player.Get(ev.Target) is Player target))
            {
                return;
            }

            if (!API.IsScp035(target) && !API.IsScp035(ev.Shooter))
            {
                return;
            }

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
    }
}