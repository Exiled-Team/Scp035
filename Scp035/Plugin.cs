using System;
using Exiled.API.Features;
using MapEvents = Exiled.Events.Handlers.Map;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using Scp079Events = Exiled.Events.Handlers.Scp079;
using Scp096Events = Exiled.Events.Handlers.Scp096;
using Scp106Events = Exiled.Events.Handlers.Scp106;
using Scp914Events = Exiled.Events.Handlers.Scp914;
using ServerEvents = Exiled.Events.Handlers.Server;
using WarheadEvents = Exiled.Events.Handlers.Warhead;

namespace Scp035
{
    using System.Collections.Generic;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        /// <summary>
        /// Static reference to the main instance of this class.
        /// </summary>
        public static Plugin Instance;

        /// <inheritdoc />
        public override string Author { get; } = "Joker119";

        /// <inheritdoc />
        public override string Name { get; } = "Scp035";

        /// <inheritdoc />
        public override string Prefix { get; } = "Scp035";

        /// <inheritdoc />
        public override Version Version { get; } = new Version(4, 0, 0);

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(4, 2, 0);

        /// <summary>
        /// Gets the reference to this plugin's Event Handler class.
        /// </summary>
        public EventHandlers EventHandlers { get; private set; }

        internal List<Player> StopRagdollsList = new List<Player>();

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Player.SpawningRagdoll += EventHandlers.OnSpawningRagdoll;

            Config.Scp035ItemConfig.TryRegister();
            Config.Scp035RoleConfig.TryRegister();
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Config.Scp035ItemConfig.TryUnregister();
            Config.Scp035RoleConfig.TryUnregister();
            
            Exiled.Events.Handlers.Player.SpawningRagdoll -= EventHandlers.OnSpawningRagdoll;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}