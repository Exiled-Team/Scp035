using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Scp035
{
    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc />
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug messages will be shown.
        /// </summary>
        [Description("Whether or not to display debug messages in the server console.")]
        public bool Debug { get; set; }

        /// <summary>
        /// Item configs for 035.
        /// </summary>
        [Description("Configs for the item players interact with to become SCP-035.")]
        public Scp035Item Scp035ItemConfig { get; set; } = new();

        /// <summary>
        /// Role configs for 035.
        /// </summary>
        [Description("Configs for the SCP-035 role players turn into.")]
        public Scp035Role Scp035RoleConfig { get; set; } = new();
    }
}