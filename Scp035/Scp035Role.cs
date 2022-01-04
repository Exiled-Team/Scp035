namespace Scp035
{
    using System.Collections.Generic;
    using System.Linq;
    using Assets._Scripts.Dissonance;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using PlayerStatsSystem;
    using UnityEngine;
    using YamlDotNet.Serialization;
    using RoleType = RoleType;

    /// <summary>
    /// The <see cref="CustomRole"/> handler for SCP-035.
    /// </summary>
    public class Scp035Role : CustomRole
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 51;

        /// <summary>
        /// Gets or sets the role that is visible to players on the server aside from the player playing this role.
        /// </summary>
        public RoleType VisibleRole { get; set; } = RoleType.Scp049;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 500;

        /// <inheritdoc />
        public override string Name { get; set; } = "SCP-035";

        /// <inheritdoc />
        public override string Description { get; set; } =
            "An SCP who slowly corrodes over time, but is able to use items normally.";

        /// <inheritdoc />
        protected override bool KeepInventoryOnSpawn { get; set; } = true;
        
        /// <summary>
        /// Gets or sets a multiplier used to modify the player's movement speed (running and walking).
        /// </summary>
        protected float MovementMultiplier { get; set; } = 0.75f;

        /// <summary>
        /// Gets a list of item names that the player is unable to pickup while playing this role.
        /// </summary>
        protected List<string> BlacklistedItems { get; set; } = new List<string>
        {
            "SR-119",
            "GL-119",
            "SCP-2818",
            ItemType.MicroHID.ToString(),
        };

        /// <summary>
        /// Gets or sets how much damage per tick (1second) the player will take.
        /// </summary>
        protected float DamagePerTick { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the custom scale factor for players when they are this role.
        /// </summary>
        protected Vector3 Scale { get; set; } = new Vector3(1.25f, 0.75f, 1f);

        // The following properties are only defined so that we can add the YamlIgnore attribute to them so they cannot be changed via configs.
        /// <inheritdoc />
        [YamlIgnore]
        public override RoleType Role { get; set; } = RoleType.Tutorial;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>();

        /// <inheritdoc />
        [YamlIgnore]
        protected override SpawnProperties SpawnProperties { get; set; } = null;

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            player.CustomInfo = $"<color=red>{player.Nickname}\nSCP-035</color>";
            player.InfoArea &= ~PlayerInfoArea.Nickname;
            player.InfoArea &= ~PlayerInfoArea.Role;
            player.InfoArea &= ~PlayerInfoArea.PowerStatus;
            player.InfoArea &= ~PlayerInfoArea.UnitName;
            player.UnitName = "Scp035";

            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeAppearance(VisibleRole);
                player.ChangeWalkingSpeed(MovementMultiplier);
                player.ChangeRunningSpeed(MovementMultiplier);
                player.IsGodModeEnabled = false;
            });

            player.Scale = Scale;
            DissonanceUserSetup dissonance = player.GameObject.GetComponent<DissonanceUserSetup>();
            dissonance.EnableListening(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
            dissonance.EnableSpeaking(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
            dissonance.SCPChat = true;
            
            foreach (Item item in player.Items.ToList())
                if (CustomItem.TryGet(item, out CustomItem customItem))
                {
                    customItem.Spawn(player.Position, item);
                    player.RemoveItem(item);
                }
            
            player.DropItems();

            Timing.RunCoroutine(Appearance(player), $"{player.UserId}-appearance");
            Timing.RunCoroutine(Corrosion(player), $"{player.UserId}-corrosion");

            base.RoleAdded(player);
        }

        /// <inheritdoc />
        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines($"{player.UserId}-appearance");
            Timing.KillCoroutines($"{player.UserId}-corrosion");
            player.InfoArea |= PlayerInfoArea.PowerStatus;
            player.InfoArea |= PlayerInfoArea.UnitName;
            player.InfoArea |= PlayerInfoArea.Nickname;
            player.InfoArea |= PlayerInfoArea.Role;
            player.Scale = Vector3.one;
            player.CustomInfo = string.Empty;
            Scp035Item.ChangedPlayers.Remove(player);
            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeWalkingSpeed(1f);
                player.ChangeRunningSpeed(1f);
            });

            base.RoleRemoved(player);
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnSubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            base.UnSubscribeEvents();
        }
        
        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Target))
                Plugin.Instance.StopRagdollsList.Add(ev.Target);
        }
        
        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && Check(ev.Attacker) && ev.Target.Side == Side.Scp)
                ev.IsAllowed = Server.FriendlyFire || ev.Attacker.IsFriendlyFireEnabled;
        }
        
        private void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Check(ev.Player) && CheckItem(ev.Pickup))
                ev.IsAllowed = false;
        }

        private bool CheckItem(Pickup pickup)
        {
            return CustomItem.TryGet(pickup, out CustomItem customItem) && BlacklistedItems.Contains(customItem.Name) ||
                   BlacklistedItems.Contains(pickup.Type.ToString());
        }

        private IEnumerator<float> Appearance(Player player)
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(20f);
                player.ChangeAppearance(VisibleRole);
                player.CustomInfo = $"<color=red>{player.Nickname}\nSCP-035</color>";
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Nickname;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.PowerStatus;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.UnitName;
            }
        }

        private IEnumerator<float> Corrosion(Player player)
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(1f);
                player.Hurt(new UniversalDamageHandler(DamagePerTick, DeathTranslations.Poisoned));
            }
        }
    }
}