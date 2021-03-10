// -----------------------------------------------------------------------
// <copyright file="List.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Commands.SubCommands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// A command which lists all active Scp035 instances.
    /// </summary>
    public class List : ICommand
    {
        /// <inheritdoc/>
        public string Command { get; } = "list";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "l" };

        /// <inheritdoc/>
        public string Description { get; } = "Lists all active Scp035 instances.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("035.list"))
            {
                response = "Insufficient permission. Required: 035.list";
                return false;
            }

            response = $"Alive Scp035 Instances: {string.Join(", ", API.AllScp035.Select(player => player.Nickname))}";
            return true;
        }
    }
}