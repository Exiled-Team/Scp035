// -----------------------------------------------------------------------
// <copyright file="ItemSpawning.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Configs.SubConfigs
{
    using System.ComponentModel;

    /// <summary>
    /// Configs for the spawning of Scp035 item instances.
    /// </summary>
    public class ItemSpawning
    {
        /// <summary>
        /// Gets how many Scp035 item instances will spawn per cycle.
        /// </summary>
        [Description("How many Scp035 item instances will spawn per cycle.")]
        public int InfectedItemCount { get; private set; } = 1;

        /// <summary>
        /// Gets the amount of seconds between each spawn interval.
        /// </summary>
        [Description("The amount of seconds between each spawn interval.")]
        public float RotateInterval { get; private set; } = 30f;

        /// <summary>
        /// Gets all <see cref="ItemType"/>s that a Scp035 item instance can spawn as.
        /// </summary>
        [Description("All ItemTypes that a Scp035 item instance can spawn as.")]
        public ItemType[] PossibleItems { get; private set; } =
        {
            ItemType.Adrenaline, ItemType.Coin, ItemType.Disarmer, ItemType.Flashlight, ItemType.Medkit,
            ItemType.Painkillers, ItemType.Radio, ItemType.GrenadeFlash, ItemType.GrenadeFrag, ItemType.MicroHID,
        };

        /// <summary>
        /// Gets a value indicating whether a Scp035 item instance will spawn when a Scp035 host dies.
        /// </summary>
        [Description("Whether a Scp035 item instance will spawn when a Scp035 host dies.")]
        public bool SpawnAfterDeath { get; private set; } = false;
    }
}