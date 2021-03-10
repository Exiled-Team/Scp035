// -----------------------------------------------------------------------
// <copyright file="CorrodeHost.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Configs.SubConfigs
{
    using System.ComponentModel;

    /// <summary>
    /// Configs for the corrosion of Scp035 instances.
    /// </summary>
    public class CorrodeHost
    {
        /// <summary>
        /// Gets a value indicating whether a Scp035 host will lose health over time.
        /// </summary>
        [Description("Whether a Scp035 host will lose health over time.")]
        public bool IsEnabled { get; private set; } = false;

        /// <summary>
        /// Gets the amount of damage that will be dealt to a Scp035 host each interval.
        /// </summary>
        [Description("The amount of damage that will be dealt to a Scp035 host each interval.")]
        public int Damage { get; private set; } = 5;

        /// <summary>
        /// Gets the amount of seconds between each damage tick.
        /// </summary>
        [Description("The amount of seconds between each damage tick.")]
        public float Interval { get; private set; } = 6f;
    }
}