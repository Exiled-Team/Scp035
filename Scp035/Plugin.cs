// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;
    using Scp035.EventHandlers;
    using MapHandlers = Exiled.Events.Handlers.Map;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using Scp096Handlers = Exiled.Events.Handlers.Scp096;
    using Scp106Handlers = Exiled.Events.Handlers.Scp106;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// The main class which inherits <see cref="Plugin{TConfig}"/>.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;

        /// <summary>
        /// Gets an instance of <see cref="Plugin"/>.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc/>
        public override string Author { get; } = "Build";

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(3, 0, 0);

        public PlayerEvents PlayerEvents { get; private set; }

        public Scp096Events Scp096Events { get; private set; }

        public Scp106Events Scp106Events { get; private set; }

        public ServerEvents ServerEvents { get; private set; }

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;
            SubscribeAll();
            harmony = new Harmony($"build.scp035.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();
            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            UnSubscribeAll();
            Methods.KillAllCoroutines();
            harmony.UnpatchAll();
            Instance = null;
            base.OnDisabled();
        }

        private void SubscribeAll()
        {
            PlayerEvents = new PlayerEvents(this);
            PlayerHandlers.ChangingRole += PlayerEvents.OnChangingRole;
            PlayerHandlers.Destroying += PlayerEvents.OnDestroying;
            PlayerHandlers.Died += PlayerEvents.OnDied;
            PlayerHandlers.EnteringPocketDimension += PlayerEvents.OnEnteringPocketDimension;
            PlayerHandlers.Escaping += PlayerEvents.OnEscaping;
            PlayerHandlers.Hurting += PlayerEvents.OnHurting;
            PlayerHandlers.ActivatingGenerator += PlayerEvents.OnActivatingGenerator;
            PlayerHandlers.ItemUsed += PlayerEvents.OnItemUsed;
            PlayerHandlers.PickingUpItem += PlayerEvents.OnPickingUpItem;
            PlayerHandlers.Shot += PlayerEvents.OnShot;
            PlayerHandlers.UsingItem += PlayerEvents.OnUsingItem;

            Scp096Events = new Scp096Events();
            Scp096Handlers.AddingTarget += Scp096Events.OnAddingTarget;

            Scp106Events = new Scp106Events(this);
            Scp106Handlers.Containing += Scp106Events.OnContaining;

            ServerEvents = new ServerEvents(this);
            if (Instance.Config.CheckWinConditions)
                ServerHandlers.EndingRound += ServerEvents.OnEndingRound;

            ServerHandlers.RoundStarted += ServerEvents.OnRoundStarted;
            ServerHandlers.WaitingForPlayers += ServerEvents.OnWaitingForPlayers;
        }

        private void UnSubscribeAll()
        {
            PlayerHandlers.ChangingRole -= PlayerEvents.OnChangingRole;
            PlayerHandlers.Destroying -= PlayerEvents.OnDestroying;
            PlayerHandlers.Died -= PlayerEvents.OnDied;
            PlayerHandlers.EnteringPocketDimension -= PlayerEvents.OnEnteringPocketDimension;
            PlayerHandlers.Escaping -= PlayerEvents.OnEscaping;
            PlayerHandlers.Hurting -= PlayerEvents.OnHurting;
            PlayerHandlers.ActivatingGenerator -= PlayerEvents.OnActivatingGenerator;
            PlayerHandlers.ItemUsed -= PlayerEvents.OnItemUsed;
            PlayerHandlers.PickingUpItem -= PlayerEvents.OnPickingUpItem;
            PlayerHandlers.Shot -= PlayerEvents.OnShot;
            PlayerHandlers.UsingItem -= PlayerEvents.OnUsingItem;
            PlayerEvents = null;

            Scp096Handlers.AddingTarget -= Scp096Events.OnAddingTarget;
            Scp096Events = null;

            Scp106Handlers.Containing -= Scp106Events.OnContaining;
            Scp106Events = null;

            ServerHandlers.EndingRound -= ServerEvents.OnEndingRound;
            ServerHandlers.RoundStarted -= ServerEvents.OnRoundStarted;
            ServerHandlers.WaitingForPlayers -= ServerEvents.OnWaitingForPlayers;
            ServerEvents = null;
        }
    }
}