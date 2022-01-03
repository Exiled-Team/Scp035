namespace Scp035
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;

    /// <summary>
    /// Handles general events for this plugin.
    /// </summary>
    public class EventHandlers
    {
        private readonly Plugin _plugin;
        
        internal EventHandlers(Plugin plugin) => this._plugin = plugin;

        internal void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            if (_plugin.StopRagdollsList.Contains(ev.Owner))
                ev.IsAllowed = false;
        }

        internal void OnEndingRound(EndingRoundEventArgs ev)
        {
            bool human = false;
            bool scps = false;

            foreach (Player player in Player.List)
            {
                if (!CustomRole.Get(typeof(Scp035Role)).Check(player) || player.Side == Side.Scp)
                    scps = true;
                else if (player.Side == Side.Mtf || player.Role == RoleType.ClassD)
                    human = true;

                if (scps && human)
                    break;
            }

            ev.IsAllowed = !(human && scps);
            ev.IsRoundEnded = !(human && scps);
        }
    }
}