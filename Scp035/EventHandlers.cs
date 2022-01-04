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
            CustomRole role = CustomRole.Get(typeof(Scp035Role));

            if (role == null)
            {
                Log.Debug($"{nameof(OnEndingRound)}: Custom role is null, returning.", _plugin.Config.Debug);
                return;
            }

            foreach (Player player in Player.List)
            {
                if (player == null)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Skipping a null player.", _plugin.Config.Debug);
                    continue;
                }

                if (role.Check(player) || player.Side == Side.Scp)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Found an SCP player.", _plugin.Config.Debug);
                    scps = true;
                }
                else if (player.Side == Side.Mtf || player.Role == RoleType.ClassD)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Found a Human player.", _plugin.Config.Debug);
                    human = true;
                }

                if (scps && human)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Both humans and scps detected.", _plugin.Config.Debug);
                    break;
                }
            }

            Log.Debug($"{nameof(OnEndingRound)}: Should event be blocked: {(human && scps)} -- Should round end: {(human && scps)}", _plugin.Config.Debug);
            if (human && scps)
            {
                ev.IsAllowed = false;
                ev.IsRoundEnded = false;
            }
        }
    }
}