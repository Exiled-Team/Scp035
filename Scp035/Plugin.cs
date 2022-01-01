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
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "Joker119";
        public override string Name { get; } = "Scp035";
        public override string Prefix { get; } = "Scp035";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 11, 1);

        public Methods Methods { get; private set; }
        public EventHandlers EventHandlers { get; private set; }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Methods = new Methods(this);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventHandlers = null;
            Methods = null;

            base.OnDisabled();
        }
    }
}