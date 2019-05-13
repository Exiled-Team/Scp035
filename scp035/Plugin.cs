using Smod2.Attributes;

namespace scp035
{
	[PluginDetails(
	author = "Cyanox",
	name = "SCP-035",
	description = "Adds SCP-035 to the game.",
	id = "cyan.scp035",
	version = "1.2.1",
	SmodMajor = 3,
	SmodMinor = 0,
	SmodRevision = 0
	)]
	public class Plugin : Smod2.Plugin
	{
		public override void OnDisable() { }

		public override void OnEnable() { }

		public override void Register()
		{
			// Laser don't yell at me this is to override friendly fire blockers
			AddEventHandlers(new EventHandler(this));

			AddConfig(new Smod2.Config.ConfigSetting("035_possible_items", new[] {
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 30
			}, false, true, "The possible item IDs SCP-035 can disguise as."));
			AddConfig(new Smod2.Config.ConfigSetting("035_health", 300, false, true, "The amount of health SCP-035 should have."));
			AddConfig(new Smod2.Config.ConfigSetting("035_rotate_interval", 20f, false, true, "The time in seconds before the item representing SCP-035 will refresh."));
			AddConfig(new Smod2.Config.ConfigSetting("035_scp_friendly_fire", false, false, true, "If SCP-035 is allowed to damage other SCPs."));
			AddConfig(new Smod2.Config.ConfigSetting("035_infected_item_count", 1, false, true, "The number of items at a time that are possessed by SCP-035."));
			AddConfig(new Smod2.Config.ConfigSetting("035_spawn_new_items", false, false, true, "If the plugin should spawn a new item on top of a randomly selected item rather than possessing already existing items."));
			AddConfig(new Smod2.Config.ConfigSetting("035_use_damage_override", false, false, true, "If the plugin should override AdmintoolBox's damage blockers."));
		}
	}
}
