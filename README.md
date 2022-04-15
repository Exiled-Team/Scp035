# SCP-035

Adds a passive SCP to the game. A certain amount of items will randomly be selected to be SCP-035 from a configurable list of item types. If a player picks up the item that SCP-035 is disguised as, the player will be turned into an SCP but are able to use their inventory like a human. This player will be able to kill their own class, bypassing friendly fire rules.  Once SCP-035 dies, he will return to disguising into items until his next victim picks him up.

# Installation

**[EXILED](https://github.com/galaxy119/EXILED) must be installed for this to work.**

Place the "Scp035.dll" file in your Plugins folder.

# Configs
## Main
| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| is_enabled | Boolean | True | Whether the plugin should load. |
| debug | Boolean | False | Whether debug messages should show. |
| corrode_trail | Boolean | False | Whether a Scp035 should leave a trail behind them. |
| corrode_trail_interval | Integer | 5 | The amount of time between the creation of a part of the trail. |
| scp_friendly_fire | Boolean | False | Whether Scp035 and Scp subjects can damage each other. |
| tutorial_friendly_fire | Boolean | True | Whether Scp035 and tutorials can damage each other. |
| win_with_tutorial | Boolean | True | Whether Scp035 and tutorials will win together. |

## Corrode Host
Configs for the corrosion of Scp035 instances.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| is_enabled | Boolean | False | Whether a Scp035 host will lose health over time. |
| damage | Integer | 5 | The amount of damage that will be dealt to a Scp035 host each interval. |
| interval | Float | 6 | The amount of seconds between each damage tick. |

## Corrode Players
Configs for the corrosion of players around Scp035 instances.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| is_enabled | Boolean | False | Whether players around a Scp035 host will lose health over time. |
| distance | Float | 1.5 | The minimum distance a player must be to a Scp035 instance to take damage. |
| damage | Integer | 5 | The amount of damage that will be dealt to players around a Scp035 host. |
| life_steal | Boolean | True | Whether Scp035 instances will heal while dealing corrosion damage. |
| interval | Float | 1 | The amount of seconds between each damage tick. |

## Item Spawning
Configs for the spawning of Scp035 item instances.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| infected_item_count | Integer | 1 | How many Scp035 item instances will spawn per cycle. |
| rotate_interval | Float | 30 | The amount of seconds between each spawn interval. |
| possible_items | ItemType Array | Adrenaline, Coin, Disarmer, Flashlight, Medkit, Painkillers, Radio, GrenadeFlash, GrenadeFrag, MicroHID | All ItemTypes that a Scp035 item instance can spawn as. |
| spawn_after_death | Boolean | false | Whether a Scp035 item instance will spawn when a Scp035 host dies. |

## Ranged Notification
Configs for the display of a notification to users looking at a Scp035 instance.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| is_enabled | Boolean | False | Whether players looking at Scp035 will see a notification. |
| interval | Float | 5 | The time between checking if a player is looking at a Scp035 instance. |
| minimum_range | Float | 10 | The minimum distance a Scp035 instance must be to a player to display a notification. |
| maximum_range | Float | 30 | The maximum distance a Scp035 instance must be to a player to display a notification. |
| use_hints | Boolean | True | Whether hints should be used in place of a broadcast. |
| notification | Broadcast | You are looking at a <color=red>SCP-035</color>! | The message to be displayed to players. |

## Scp035 Modifiers
Configs in relation to Scp035 instances.

| Config        | Type | Default | Description
| :-------------: | :---------: | :---------: | :------ |
| ammo_amount | Unsigned Integer | 250 | The amount of ammo that is given to Scp035. |
| can_heal_beyond_host_hp | Boolean | True | Whether a Scp035 instance can heal beyond their current roles max health. |
| can_use_medial_items | Boolean | True | Whether a Scp035 instance can use medical items. |
| health | Integer | 300 | The amount of health a Scp035 instance will spawn with. |
| self_inflict | Boolean | False | Whether the user who picks up an item will become an instance or if someone will be chosen to replace them. |
| scale | Vector | z: 1 y: 1 x: 1 | The size of a Scp035 instance. |
| spawn_broadcast | Broadcast | <i>You have picked up <color=\"red\">SCP-035.</color> He has infected your body and is now in control of you.</i> | The broadcast that will be displayed to an Scp035 instance when they spawn. |
