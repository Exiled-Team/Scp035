// -----------------------------------------------------------------------
// <copyright file="Scp035Parent.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.Commands
{
#pragma warning disable SA1101
#pragma warning disable SA1135

    using System;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;
    using SubCommands;

    /// <summary>
    /// The command which all Scp035 commands are run off of.
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Scp035Parent : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Scp035Parent"/> class.
        /// </summary>
        public Scp035Parent() => LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "035";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc/>
        public override string Description { get; } = "Parent command for Scp035";

        /// <inheritdoc/>
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Kill());
            RegisterCommand(new List());
            RegisterCommand(new Spawn());
            RegisterCommand(new SpawnItems());
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            foreach (var command in AllCommands)
            {
                stringBuilder.AppendLine(command.Aliases.Length > 0
                    ? $"{command.Command} | Aliases: {string.Join(", ", command.Aliases)}"
                    : command.Command);
            }

            response = $"Please enter a valid subcommand! Available:\n{StringBuilderPool.Shared.ToStringReturn(stringBuilder)}";
            return false;
        }
    }
}