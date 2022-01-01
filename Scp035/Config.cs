using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Scp035
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; }
    }
}