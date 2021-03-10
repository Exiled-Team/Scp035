// -----------------------------------------------------------------------
// <copyright file="Scp035.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035
{
#pragma warning disable SA1135

    using System;
    using Configs;
    using EventHandlers;
    using Exiled.API.Features;
    using HarmonyLib;

    /// <summary>
    /// The main class which inherits <see cref="Plugin{TConfig}"/>.
    /// </summary>
    public class Scp035 : Plugin<Config>
    {
        private static readonly Scp035 InstanceValue = new Scp035();
        private static Harmony harmony;

        private Scp035()
        {
        }

        /// <summary>
        /// Gets an instance of <see cref="Scp035"/>.
        /// </summary>
        public static Scp035 Instance { get; } = InstanceValue;

        /// <inheritdoc/>
        public override string Author { get; } = "Build, formerly by Cyanox";

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(2, 8, 0);

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(2, 0, 1);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            SubscribeAll();
            harmony = new Harmony(nameof(Scp035).ToLowerInvariant());
            harmony.PatchAll();
            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            UnSubscribeAll();
            Methods.KillAllCoroutines();
            harmony.UnpatchAll(harmony.Id);
            base.OnDisabled();
        }

        private static void SubscribeAll()
        {
            Exiled.Events.Handlers.Map.ExplodingGrenade += MapHandlers.OnExplodingGrenade;

            Exiled.Events.Handlers.Player.ChangingRole += PlayerHandlers.OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying += PlayerHandlers.OnDestroying;
            Exiled.Events.Handlers.Player.Died += PlayerHandlers.OnDied;
            Exiled.Events.Handlers.Player.Hurting += PlayerHandlers.OnHurting;
            Exiled.Events.Handlers.Player.PickingUpItem += PlayerHandlers.OnPickingUpItem;
            Exiled.Events.Handlers.Player.Shooting += PlayerHandlers.OnShooting;

            Exiled.Events.Handlers.Server.EndingRound += ServerHandlers.OnEndingRound;
            Exiled.Events.Handlers.Server.RoundStarted += ServerHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += ServerHandlers.OnWaitingForPlayers;
        }

        private static void UnSubscribeAll()
        {
            Exiled.Events.Handlers.Map.ExplodingGrenade -= MapHandlers.OnExplodingGrenade;

            Exiled.Events.Handlers.Player.ChangingRole -= PlayerHandlers.OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying -= PlayerHandlers.OnDestroying;
            Exiled.Events.Handlers.Player.Died -= PlayerHandlers.OnDied;
            Exiled.Events.Handlers.Player.Hurting -= PlayerHandlers.OnHurting;
            Exiled.Events.Handlers.Player.PickingUpItem -= PlayerHandlers.OnPickingUpItem;
            Exiled.Events.Handlers.Player.Shooting -= PlayerHandlers.OnShooting;

            Exiled.Events.Handlers.Server.EndingRound -= ServerHandlers.OnEndingRound;
            Exiled.Events.Handlers.Server.RoundStarted -= ServerHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= ServerHandlers.OnWaitingForPlayers;
        }
    }
}