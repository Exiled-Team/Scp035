namespace Scp035
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API;
    using Exiled.CustomItems.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Loader;
    using MEC;
    using UnityEngine;
    using YamlDotNet.Serialization;
    
    /// <inheritdoc />
    [CustomItem(ItemType.Coin)]
    public class Scp035Item : CustomItem
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 51;

        /// <inheritdoc />
        public override string Name { get; set; } = "SCP-035";

        /// <inheritdoc />
        public override string Description { get; set; } =
            "An item that will quickly kill you and turn you into an SCP.";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0.75f;

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint
                {
                    Chance = 35,
                    Location = SpawnLocation.InsideLocker,
                }
            }
        };

        /// <inheritdoc />
        [YamlIgnore] 
        public override ItemType Type { get; set; } = ItemType.Coin;

        /// <summary>
        /// Gets or sets a list of <see cref="ItemSpawn"/> values used to determine what <see cref="ItemType"/> the item will have when it spawns.
        /// </summary>
        protected List<ItemSpawn> Types { get; set; } = new List<ItemSpawn>
        {
            new ItemSpawn(ItemType.Coin, 10),
            new ItemSpawn(ItemType.Medkit, 20),
            new ItemSpawn(ItemType.Adrenaline, 30),
            new ItemSpawn(ItemType.SCP018, 60),
            new ItemSpawn(ItemType.GrenadeHE, 100),
        };

        /// <summary>
        /// Gets or sets how long the item takes to transform the player into SCP-035.
        /// </summary>
        [Description("How long (in seconds) it takes a player to transform into SCP-035 when they pickup the item. If they drop the item before this timer ends, they will not transform.")]
        protected float TransformationDelay { get; set; } = 5f;

        /// <summary>
        /// A list of players already transformed into SCP-035.
        /// </summary>
        public static readonly List<Player> ChangedPlayers = new List<Player>();

        /// <inheritdoc />
        public override Pickup Spawn(Vector3 position)
        {
            Pickup pickup = Item.Create(RandomType()).Spawn(position);
            pickup.Weight = Weight;
            TrackedSerials.Add(pickup.Serial);
            return pickup;
        }

        private ItemType RandomType()
        {
            if (Types.Count == 1)
                return Types[0].Type;

            foreach ((ItemType type, int chance) in Types)
            {
                if (Loader.Random.Next(100) <= chance)
                    return type;
            }

            return Type;
        }

        private IEnumerator<float> ChangeTo035(Player player)
        {
            RoleType playerRole = player.Role;
            yield return Timing.WaitForSeconds(TransformationDelay);

            if (player.Role != playerRole)
            {
                Log.Debug($"{nameof(ChangeTo035)}: {player.Nickname} picked up 035 as {playerRole} but they are now {player.Role}, cancelling change.", Plugin.Instance.Config.Debug);
                yield break;
            }
            
            ChangedPlayers.Add(player);
            foreach (Item item in player.Items.ToList())
                if (Check(item))
                    player.RemoveItem(item);
            CustomRole.Get(typeof(Scp035Role)).AddRole(player);
        }

        /// <inheritdoc />
        protected override void OnPickingUp(PickingUpItemEventArgs ev)
        {
            Timing.RunCoroutine(ChangeTo035(ev.Player), $"035change-{ev.Player.UserId}");
        }

        /// <inheritdoc />
        protected override void OnDropping(DroppingItemEventArgs ev)
        {
            if (ChangedPlayers.Contains(ev.Player))
            {
                ev.IsAllowed = false;
                return;
            }

            Log.Debug($"{ev.Player.Nickname} dropped 035 before their change, cancelling.", Plugin.Instance.Config.Debug);
            Timing.KillCoroutines($"035change-{ev.Player.UserId}");
        }
    }
}