// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;
    using Scp035.Configs;

    /// <inheritdoc cref="IConfig"/>
    public sealed class Config : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the plugin will load.
        /// </summary>
        [Description("Whether the plugin should load.")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug messages will be present.
        /// </summary>
        [Description("Whether debug messages should show.")]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether Scp035 will leave a trail of blood behind them.
        /// </summary>
        [Description("Whether a Scp035 should leave a trail behind them.")]
        public bool CorrodeTrail { get; set; } = false;

        /// <summary>
        /// Gets or sets the amount of time between the spawn of each blood spot.
        /// </summary>
        [Description("The amount of time between the creation of a part of the trail.")]
        public int CorrodeTrailInterval { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether Scp035 and Scp subjects can damage each other.
        /// </summary>
        [Description("Whether Scp035 and Scp subjects can damage each other.")]
        public bool ScpFriendlyFire { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether Scp035 and tutorials can damage each other.
        /// </summary>
        [Description("Whether Scp035 and tutorials can damage each other.")]
        public bool TutorialFriendlyFire { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether win conditions should be handled by this plugin.
        /// </summary>
        [Description("Whether win conditions should be handled by this plugin.")]
        public bool CheckWinConditions { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether Scp035 and tutorials will win together.
        /// </summary>
        [Description("Whether Scp035 and tutorials will win together.")]
        public bool WinWithTutorial { get; set; } = true;

        /// <summary>
        /// Gets or sets all of the configs in relation to corrosion of Scp035 instances.
        /// </summary>
        [Description("All of the configs in relation to corrision of Scp035 instances.")]
        public CorrodeHost CorrodeHost { get; set; } = new CorrodeHost();

        /// <summary>
        /// Gets or sets all of the configs in relation to corrosion of players near Scp035 instances.
        /// </summary>
        [Description("All of the configs in relation to corrision of players near Scp035 instances.")]
        public CorrodePlayers CorrodePlayers { get; set; } = new CorrodePlayers();

        /// <summary>
        /// Gets or sets all of the configs in relation to the spawning of Scp035 item instances.
        /// </summary>
        [Description("All of the configs in relation to the spawning of Scp035 item instances.")]
        public ItemSpawning ItemSpawning { get; set; } = new ItemSpawning();

        /// <summary>
        /// Gets or sets all of the configs in relation to the display of a notification to users looking at a Scp035 instance.
        /// </summary>
        [Description("All of the configs in relation to the display of a notification to users looking at a Scp035 instance.")]
        public RangedNotification RangedNotification { get; set; } = new RangedNotification();

        /// <summary>
        /// Gets or sets all of the configs in relation to Scp035 instances.
        /// </summary>
        [Description("All of the configs in relation to Scp035 instances.")]
        public Scp035Modifiers Scp035Modifiers { get; set; } = new Scp035Modifiers();
    }
}