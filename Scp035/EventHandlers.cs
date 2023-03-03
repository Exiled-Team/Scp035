namespace Scp035
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using PlayerRoles;

    /// <summary>
    /// Handles general events for this plugin.
    /// </summary>
    public class EventHandlers
    {
        private readonly Plugin _plugin;
        
        internal EventHandlers(Plugin plugin) => this._plugin = plugin;

        internal void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            if (_plugin.StopRagdollsList.Contains(ev.Player))
                ev.IsAllowed = false;
        }

        internal void OnEndingRound(EndingRoundEventArgs ev)
        {
            bool human = false;
            bool scps = false;
            CustomRole role = CustomRole.Get(typeof(Scp035Role));

            if (role == null)
            {
                Log.Debug($"{nameof(OnEndingRound)}: Custom role is null, returning.");
                return;
            }

            foreach (Player player in Player.List)
            {
                if (player == null)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Skipping a null player.");
                    continue;
                }

                if (role.Check(player) || player.Role.Side == Side.Scp)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Found an SCP player.");
                    scps = true;
                }
                else if (player.Role.Side == Side.Mtf || player.Role == RoleTypeId.ClassD)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Found a Human player.");
                    human = true;
                }

                if (scps && human)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Both humans and scps detected.");
                    break;
                }
            }

            Log.Debug($"{nameof(OnEndingRound)}: Should event be blocked: {(human && scps)} -- Should round end: {(human && scps)}");
            if (human && scps)
            {
                ev.IsRoundEnded = false;
            }
        }
    }
}