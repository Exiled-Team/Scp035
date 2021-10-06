// -----------------------------------------------------------------------
// <copyright file="ItemSpawning.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Configs
{
    using System.ComponentModel;

    /// <summary>
    /// Configs for the spawning of Scp035 item instances.
    /// </summary>
    public class ItemSpawning
    {
        /// <summary>
        /// Gets or sets how many Scp035 item instances will spawn per cycle.
        /// </summary>
        [Description("How many Scp035 item instances will spawn per cycle.")]
        public int InfectedItemCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the amount of seconds between each spawn interval.
        /// </summary>
        [Description("The amount of seconds between each spawn interval.")]
        public float RotateInterval { get; set; } = 30f;

        /// <summary>
        /// Gets or sets all <see cref="ItemType"/>s that a Scp035 item instance can spawn as.
        /// </summary>
        [Description("All ItemTypes that a Scp035 item instance can spawn as.")]
        public ItemType[] PossibleItems { get; set; } =
        {
            ItemType.Painkillers,
            ItemType.GrenadeHE,
            ItemType.MicroHID,
            ItemType.KeycardO5,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardContainmentEngineer,
            ItemType.SCP500,
            ItemType.SCP268,
            ItemType.SCP018,
        };

        /// <summary>
        /// Gets or sets a value indicating whether a Scp035 item instance will spawn when a Scp035 host dies.
        /// </summary>
        [Description("Whether a Scp035 item instance will spawn when a Scp035 host dies.")]
        public bool SpawnAfterDeath { get; set; } = false;
    }
}