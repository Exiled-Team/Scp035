// -----------------------------------------------------------------------
// <copyright file="SpawnItems.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Commands.SubCommands
{
    using System;
    using System.Text;
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using NorthwoodLib.Pools;

    /// <summary>
    /// A command which spawns a Scp035 item instance.
    /// </summary>
    public class SpawnItems : ICommand
    {
        private const string RequiredPermission = "035.spawnitem";

        /// <inheritdoc/>
        public string Command { get; } = "spawnitems";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "i", "item", "items" };

        /// <inheritdoc/>
        public string Description { get; } = "Spawns the specified amount of Scp035 instanced items.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = $"Insufficient permission. Required: {RequiredPermission}";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Syntax: 035 item <Amount>";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int amount))
            {
                response = $"Could not parse \"{arguments.At(0)}\" as a number.";
                return false;
            }

            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent().AppendLine("Spawned Items:");
            foreach (Pickup item in API.SpawnItems(amount))
            {
                stringBuilder.AppendLine($"ItemType: {item.itemId} - Position: {item.transform.position}");
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder);
            return true;
        }
    }
}