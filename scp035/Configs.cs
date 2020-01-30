using System.Collections.Generic;

namespace scp035
{
	internal static class Configs
	{
		internal static List<int> possibleItems;

		internal static int health;
		internal static int infectedItemCount;
		internal static int corrodeDamage;

		internal static bool scpFriendlyFire;
		internal static bool tutorialFriendlyFire;
		internal static bool useDamageOverride;
		internal static bool winWithTutorial;
		internal static bool changeToZombie;
		internal static bool corrodePlayers;
		internal static bool corrodeLifeSteal;
		internal static bool enabled;

		internal static float corrodeDistance;
		internal static float rotateInterval;
		internal static float corrodeInterval;

		internal static void ReloadConfig()
		{
			Configs.health = Plugin.Config.GetInt("035_health", 300);
			Configs.rotateInterval = Plugin.Config.GetFloat("035_rotate_interval", 120f);
			Configs.scpFriendlyFire = Plugin.Config.GetBool("035_scp_friendly_fire", false);
			Configs.infectedItemCount = Plugin.Config.GetInt("035_infected_item_count", 1);
			Configs.useDamageOverride = Plugin.Config.GetBool("035_use_damage_override", false);
			Configs.winWithTutorial = Plugin.Config.GetBool("035_win_with_tutorial", false);
			Configs.changeToZombie = Plugin.Config.GetBool("035_change_to_zombie", false);
			Configs.tutorialFriendlyFire = Plugin.Config.GetBool("035_tutorial_friendly_fire", false);
			Configs.corrodePlayers = Plugin.Config.GetBool("035_corrode_players", true);
			Configs.corrodeDistance = Plugin.Config.GetFloat("035_corrode_distance", 1.5f);
			Configs.corrodeDamage = Plugin.Config.GetInt("035_corrode_damage", 5);
			Configs.corrodeInterval = Plugin.Config.GetFloat("035_corrode_interval", 1f);
			Configs.corrodeLifeSteal = Plugin.Config.GetBool("035_corrode_life_steal", true);
			Configs.enabled = Plugin.Config.GetBool("035_enabled", true);
			Configs.possibleItems = Plugin.Config.GetIntList("035_possible_items");
			if (Configs.possibleItems == null)
			{
				Configs.possibleItems = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 30 };
			}
		}
	}
}
