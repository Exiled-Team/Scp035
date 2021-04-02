// -----------------------------------------------------------------------
// <copyright file="Kill.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Scp035.Commands.SubCommands
{
    using System;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// A command which kills all active Scp035 instances.
    /// </summary>
    public class Kill : ICommand
    {
        /// <inheritdoc/>
        public string Command { get; } = "kill";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "k" };

        /// <inheritdoc/>
        public string Description { get; } = "Kills all alive Scp035s.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("035.kill"))
            {
                response = "Insufficient permission. Required: 035.kill";
                return false;
            }

            foreach (Player player in API.AllScp035)
            {
                player.Kill();
            }

            response = "Killed all Scp035 users successfully.";
            return false;
        }
    }
}