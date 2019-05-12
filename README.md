# SCP-035

Adds a passive SCP to the game. One item at a time will randomly be selected to be SCP-035 from a configurable list of item types. If a player picks up the item that SCP-035 is disguised as, the player will be killed and a random spectator will be assigned their possessed body. Once SCP-035 dies, he will return to disguising into items until his next victim picks him up.

# Installation

**[Smod2](https://github.com/Grover-c13/Smod2) must be installed for this to work.**

Place the "scp035.dll" file in your sm_plugins folder.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| 035_possible_items | Integer List | 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19, 20, 21, 23, 24, 25, 26, 30 | The item IDs SCP-035 can disguise as. |
| 035_health | Integer | 300 | TThe amount of health SCP-035 has. |
| 035_rotate_interval | Float | 120 | The amount of time in seconds before SCP-035 will choose a new item on the map to disguise as. |
