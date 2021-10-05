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
        private PlayerEvents playerEvents;
        private Scp096Events scp096Events;
        private Scp106Events scp106Events;
        private ServerEvents serverEvents;

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
            playerEvents = new PlayerEvents(this);
            PlayerHandlers.ChangingRole += playerEvents.OnChangingRole;
            PlayerHandlers.Destroying += playerEvents.OnDestroying;
            PlayerHandlers.Died += playerEvents.OnDied;
            PlayerHandlers.EnteringPocketDimension += playerEvents.OnEnteringPocketDimension;
            PlayerHandlers.Escaping += playerEvents.OnEscaping;
            PlayerHandlers.Hurting += playerEvents.OnHurting;
            PlayerHandlers.ActivatingGenerator += playerEvents.OnActivatingGenerator;
            PlayerHandlers.ItemUsed += playerEvents.OnItemUsed;
            PlayerHandlers.PickingUpItem += playerEvents.OnPickingUpItem;
            PlayerHandlers.Shot += playerEvents.OnShot;
            PlayerHandlers.UsingItem += playerEvents.OnUsingItem;

            scp096Events = new Scp096Events();
            Scp096Handlers.AddingTarget += scp096Events.OnAddingTarget;

            scp106Events = new Scp106Events(this);
            Scp106Handlers.Containing += scp106Events.OnContaining;

            serverEvents = new ServerEvents(this);
            if (Instance.Config.CheckWinConditions)
                ServerHandlers.EndingRound += serverEvents.OnEndingRound;

            ServerHandlers.RoundStarted += serverEvents.OnRoundStarted;
            ServerHandlers.WaitingForPlayers += serverEvents.OnWaitingForPlayers;
        }

        private void UnSubscribeAll()
        {
            PlayerHandlers.ChangingRole -= playerEvents.OnChangingRole;
            PlayerHandlers.Destroying -= playerEvents.OnDestroying;
            PlayerHandlers.Died -= playerEvents.OnDied;
            PlayerHandlers.EnteringPocketDimension -= playerEvents.OnEnteringPocketDimension;
            PlayerHandlers.Escaping -= playerEvents.OnEscaping;
            PlayerHandlers.Hurting -= playerEvents.OnHurting;
            PlayerHandlers.ActivatingGenerator -= playerEvents.OnActivatingGenerator;
            PlayerHandlers.ItemUsed -= playerEvents.OnItemUsed;
            PlayerHandlers.PickingUpItem -= playerEvents.OnPickingUpItem;
            PlayerHandlers.Shot -= playerEvents.OnShot;
            PlayerHandlers.UsingItem -= playerEvents.OnUsingItem;
            playerEvents = null;

            Scp096Handlers.AddingTarget -= scp096Events.OnAddingTarget;
            scp096Events = null;

            Scp106Handlers.Containing -= scp106Events.OnContaining;
            scp106Events = null;

            ServerHandlers.EndingRound -= serverEvents.OnEndingRound;
            ServerHandlers.RoundStarted -= serverEvents.OnRoundStarted;
            ServerHandlers.WaitingForPlayers -= serverEvents.OnWaitingForPlayers;
            serverEvents = null;
        }
    }
}