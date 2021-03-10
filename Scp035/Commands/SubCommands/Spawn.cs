// -----------------------------------------------------------------------
// <copyright file="Spawn.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Commands.SubCommands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using RemoteAdmin;

    /// <summary>
    /// A command which spawns an active Scp035 instance.
    /// </summary>
    public class Spawn : ICommand
    {
        /// <inheritdoc/>
        public string Command { get; } = "spawn";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "s" };

        /// <inheritdoc/>
        public string Description { get; } = "Spawns a user as an instance of Scp035.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("035.spawn"))
            {
                response = "Insufficient permission. Required: 035.spawn";
                return false;
            }

            Player player = Player.Get((sender as PlayerCommandSender)?.ReferenceHub);
            if (arguments.Count > 0)
            {
                if (!(Player.Get(arguments.At(0)) is Player ply))
                {
                    response = "Could not find the referenced user.";
                    return false;
                }

                player = ply;
            }

            if (API.IsScp035(player))
            {
                response = $"{player.Nickname} is already a Scp035!";
                return false;
            }

            if (!player.IsAlive || player.IsScp)
            {
                player.Role = RoleType.ClassD;
            }

            API.Spawn035(player);
            response = $"Spawned {player.Nickname} as a Scp035.";
            return true;
        }
    }
}