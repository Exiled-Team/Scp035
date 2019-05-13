# SCP-035

Adds a passive SCP to the game. A certain amount of items will randomly be selected to be SCP-035 from a configurable list of item types. If a player picks up the item that SCP-035 is disguised as, the player will be killed and a random spectator will be assigned their possessed body. Once SCP-035 dies, he will return to disguising into items until his next victim picks him up.

# Installation

**[Smod2](https://github.com/Grover-c13/Smod2) must be installed for this to work.**

Place the "scp035.dll" file in your sm_plugins folder.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| 035_possible_items | Integer List | 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 30 | The item IDs SCP-035 can disguise as. |
| 035_health | Integer | 300 | The amount of health SCP-035 has. |
| 035_rotate_interval | Float | 120 | The amount of time in seconds before SCP-035 will choose a new item on the map to disguise as. |
| 035_scp_friendly_fire | Boolean | False | If SCP-035 is allowed to damage other SCPs. |
| 035_infected_item_count | Integer | 1 | The number of items every refresh that are possessed by SCP-035. |
| 035_spawn_new_items | Boolean | False | If the plugin should spawn a new item from the possible items list on top of a randomly selected item rather than possessing already existing possible items. |
| 035_use_damage_override | Boolean | False | If the plugin should override AdmintoolBox's damage blockers. |
