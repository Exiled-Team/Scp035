// -----------------------------------------------------------------------
// <copyright file="Scp035Modifiers.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Configs
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using Scp035.SerializableClasses;

    /// <summary>
    /// Configs in relation to Scp035 instances.
    /// </summary>
    public class Scp035Modifiers
    {
        /// <summary>
        /// Gets the amount of ammo that is given to Scp035.
        /// </summary>
        [Description("The amount of ammo that is given to Scp035.")]
        public uint AmmoAmount { get; private set; } = 250;

        /// <summary>
        /// Gets a value indicating whether a Scp035 instance can heal beyond their current roles max health.
        /// </summary>
        [Description("Whether a Scp035 instance can heal beyond their current roles max health.")]
        public bool CanHealBeyondHostHp { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether a Scp035 instance can use medical items.
        /// </summary>
        [Description("Whether a Scp035 instance can use medical items.")]
        public bool CanUseMedicalItems { get; private set; } = true;

        /// <summary>
        /// Gets the amount of health a Scp035 instance will spawn with.
        /// </summary>
        [Description("The amount of health a Scp035 instance will spawn with.")]
        public int Health { get; private set; } = 300;

        /// <summary>
        /// Gets a value indicating whether the user who picks up an item will become an instance or if someone will be chosen to replace them.
        /// </summary>
        [Description("Whether the user who picks up an item will become an instance or if someone will be chosen to replace them.")]
        public bool SelfInflict { get; private set; } = false;

        /// <summary>
        /// Gets the size of a Scp035 instance.
        /// </summary>
        [Description("The size of a Scp035 instance.")]
        public Vector Scale { get; private set; } = new Vector { X = 1, Y = 1, Z = 1 };

        /// <summary>
        /// Gets the <see cref="Broadcast"/> that will be displayed to an Scp035 instance when they spawn.
        /// </summary>
        [Description("The broadcast that will be displayed to an Scp035 instance when they spawn.")]
        public Broadcast SpawnBroadcast { get; private set; } = new ("<i>You have picked up <color=\"red\">SCP-035.</color> He has infected your body and is now in control of you.</i>");
    }
}