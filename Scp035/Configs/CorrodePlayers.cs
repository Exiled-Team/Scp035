// -----------------------------------------------------------------------
// <copyright file="CorrodePlayers.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Configs
{
    using System.ComponentModel;

    /// <summary>
    /// Configs for the corrosion of players around Scp035 instances.
    /// </summary>
    public class CorrodePlayers
    {
        /// <summary>
        /// Gets or sets a value indicating whether players around a Scp035 host will lose health over time.
        /// </summary>
        [Description("Whether players around a Scp035 host will lose health over time.")]
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the amount of damage that will be dealt to players around a Scp035 host.
        /// </summary>
        [Description("The amount of damage that will be dealt to players around a Scp035 host.")]
        public int Damage { get; set; } = 10;

        /// <summary>
        /// Gets or sets the minimum distance a player must be to a Scp035 instance to take damage.
        /// </summary>
        [Description("The minimum distance a player must be to a Scp035 instance to take damage.")]
        public float Distance { get; set; } = 1.5f;

        /// <summary>
        /// Gets or sets a value indicating whether Scp035 instances will heal while dealing corrosion damage.
        /// </summary>
        [Description("Whether Scp035 instances will heal while dealing corrosion damage.")]
        public bool LifeSteal { get; set; } = true;

        /// <summary>
        /// Gets or sets the amount of seconds between each damage tick.
        /// </summary>
        [Description("The amount of seconds between each damage tick.")]
        public float Interval { get; set; } = 1f;
    }
}