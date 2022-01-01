using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Scp035
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not to display debug messages in the server console.")]
        public bool Debug { get; set; }
    }
}