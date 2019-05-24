using Smod2.Attributes;

namespace scp035
{
	[PluginDetails(
	author = "Cyanox",
	name = "SCP-035",
	description = "Adds SCP-035 to the game.",
	id = "cyan.scp035",
	version = "1.5.0",
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
			// This is for the damage override, don't AngryLaserBoi me
			AddEventHandlers(new EventHandler(this), Smod2.Events.Priority.High);

			AddConfig(new Smod2.Config.ConfigSetting("035_possible_items", new[] {
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 30
			}, false, true, "The possible item IDs SCP-035 can disguise as."));
			AddConfig(new Smod2.Config.ConfigSetting("035_health", 300, false, true, "The amount of health SCP-035 should have."));
			AddConfig(new Smod2.Config.ConfigSetting("035_rotate_interval", 120f, false, true, "The time in seconds before the item representing SCP-035 will refresh."));
			AddConfig(new Smod2.Config.ConfigSetting("035_scp_friendly_fire", false, false, true, "If SCP-035 is allowed to damage other SCPs."));
			AddConfig(new Smod2.Config.ConfigSetting("035_infected_item_count", 1, false, true, "The number of items at a time that are possessed by SCP-035."));
			AddConfig(new Smod2.Config.ConfigSetting("035_spawn_new_items", false, false, true, "If the plugin should spawn a new item on top of a randomly selected item rather than possessing already existing items."));
			AddConfig(new Smod2.Config.ConfigSetting("035_use_damage_override", false, false, true, "If the plugin should override AdmintoolBox's damage blockers."));
			AddConfig(new Smod2.Config.ConfigSetting("035_win_with_tutorial", false, false, true, "If SCP-035 should win with tutorials."));
			AddConfig(new Smod2.Config.ConfigSetting("035_change_to_zombie", false, false, true, "If SCP-035 should change to a zombie when the SCP team should win. This may fix round lingering issues due to conflicting end conditions."));
			AddConfig(new Smod2.Config.ConfigSetting("035_tutorial_friendly_fire", false, false, true, "If friendly fire between SCP-035 and tutorials is enabled."));
			AddConfig(new Smod2.Config.ConfigSetting("035_corrode_players", true, false, true, "If SCP-035 should do passive damage to players within a range of him."));
			AddConfig(new Smod2.Config.ConfigSetting("035_corrode_distance", 1.5f, false, true, "The distance in which a player will take corrosion damage from SCP-035."));
			AddConfig(new Smod2.Config.ConfigSetting("035_corrode_damage", 5, false, true, "The amount of damage to do to a player within range of corrosion."));
			AddConfig(new Smod2.Config.ConfigSetting("035_corrode_interval", 1f, false, true, "The interval in seconds for corrosion damage."));
			AddConfig(new Smod2.Config.ConfigSetting("035_corrode_life_steal", true, false, true, "If SCP-035 should steal any health taken from other players by corrosion."));
		}
	}
}
