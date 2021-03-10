// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Configs
{
#pragma warning disable SA1135

    using System.ComponentModel;
    using Exiled.API.Interfaces;
    using SubConfigs;

    /// <inheritdoc cref="IConfig"/>
    public sealed class Config : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the plugin will load.
        /// </summary>
        [Description("Whether the plugin should load.")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether debug messages will be present.
        /// </summary>
        [Description("Whether debug messages should show.")]
        public bool Debug { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether Scp035 will leave a trail of blood behind them.
        /// </summary>
        [Description("Whether a Scp035 should leave a trail behind them.")]
        public bool CorrodeTrail { get; private set; } = false;

        /// <summary>
        /// Gets the amount of time between the spawn of each blood spot.
        /// </summary>
        [Description("The amount of time between the creation of a part of the trail.")]
        public int CorrodeTrailInterval { get; private set; } = 5;

        /// <summary>
        /// Gets a value indicating whether Scp035 and Scp subjects can damage each other.
        /// </summary>
        [Description("Whether Scp035 and Scp subjects can damage each other.")]
        public bool ScpFriendlyFire { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether Scp035 and tutorials can damage each other.
        /// </summary>
        [Description("Whether Scp035 and tutorials can damage each other.")]
        public bool TutorialFriendlyFire { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether Scp035 and tutorials will win together.
        /// </summary>
        [Description("Whether Scp035 and tutorials will win together.")]
        public bool WinWithTutorial { get; private set; } = true;

        /// <summary>
        /// Gets all of the configs in relation to corrosion of Scp035 instances.
        /// </summary>
        [Description("All of the configs in relation to corrision of Scp035 instances.")]
        public CorrodeHost CorrodeHost { get; private set; } = new CorrodeHost();

        /// <summary>
        /// Gets all of the configs in relation to corrosion of players near Scp035 instances.
        /// </summary>
        [Description("All of the configs in relation to corrision of players near Scp035 instances.")]
        public CorrodePlayers CorrodePlayers { get; private set; } = new CorrodePlayers();

        /// <summary>
        /// Gets all of the configs in relation to the spawning of Scp035 item instances.
        /// </summary>
        [Description("All of the configs in relation to the spawning of Scp035 item instances.")]
        public ItemSpawning ItemSpawning { get; private set; } = new ItemSpawning();

        /// <summary>
        /// Gets all of the configs in relation to the display of a notification to users looking at a Scp035 instance.
        /// </summary>
        [Description("All of the configs in relation to the display of a notification to users looking at a Scp035 instance.")]
        public RangedNotification RangedNotification { get; private set; } = new RangedNotification();

        /// <summary>
        /// Gets all of the configs in relation to Scp035 instances.
        /// </summary>
        [Description("All of the configs in relation to Scp035 instances.")]
        public Scp035Modifiers Scp035Modifiers { get; private set; } = new Scp035Modifiers();
    }
}