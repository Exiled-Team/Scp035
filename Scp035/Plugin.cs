using System;
using Exiled.API.Features;

namespace Scp035
{
    using System.Collections.Generic;
    using HarmonyLib;

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
        public override Version Version { get; } = new Version(4, 0, 1);

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(4, 2, 0);

        /// <summary>
        /// Gets the reference to this plugin's Event Handler class.
        /// </summary>
        public EventHandlers EventHandlers { get; private set; }

        internal List<Player> StopRagdollsList = new List<Player>();
        private Harmony _harmony;
        private string _harmonyId;

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.OnEndingRound;
            Exiled.Events.Handlers.Player.SpawningRagdoll += EventHandlers.OnSpawningRagdoll;

            _harmonyId = $"com.joker.035-{DateTime.Now.Ticks}";
            _harmony = new Harmony(_harmonyId);
            _harmony.PatchAll();
            Config.Scp035ItemConfig.TryRegister();
            Config.Scp035RoleConfig.TryRegister();
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            _harmony.UnpatchAll(_harmonyId);
            Config.Scp035ItemConfig.TryUnregister();
            Config.Scp035RoleConfig.TryUnregister();

            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.OnEndingRound;
            Exiled.Events.Handlers.Player.SpawningRagdoll -= EventHandlers.OnSpawningRagdoll;
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}