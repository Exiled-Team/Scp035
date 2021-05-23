// -----------------------------------------------------------------------
// <copyright file="RangedNotification.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Configs
{
    using System.ComponentModel;
    using Exiled.API.Features;

    /// <summary>
    /// Configs for notifying players when looking at a Scp035 instance.
    /// </summary>
    public class RangedNotification
    {
        /// <summary>
        /// Gets or sets a value indicating whether players looking at Scp035 will see a notification.
        /// </summary>
        [Description("Whether players around a Scp035 host will lose health over time.")]
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the time between checking if a player is looking at a Scp035 instance.
        /// </summary>
        [Description("The time between checking if a player is looking at a Scp035 instance.")]
        public float Interval { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the minimum distance a Scp035 instance must be to a player to display a notification.
        /// </summary>
        [Description("The minimum distance a Scp035 instance must be to a player to display a notification.")]
        public float MinimumRange { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the maximum distance a Scp035 instance must be to a player to display a notification.
        /// </summary>
        [Description("The maximum distance a Scp035 instance must be to a player to display a notification.")]
        public float MaximumRange { get; set; } = 30f;

        /// <summary>
        /// Gets or sets a value indicating whether hints should be used in place of a broadcast.
        /// </summary>
        [Description("Whether hints should be used in place of a broadcast.")]
        public bool UseHints { get; set; } = true;

        /// <summary>
        /// Gets or sets the message to be displayed to players.
        /// </summary>
        [Description("The message to be displayed to players.")]
        public Broadcast Notification { get; set; } = new Broadcast("You are looking at a <color=red>SCP-035</color>!", 2);
    }
}