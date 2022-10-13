# MNoFlinchNoWobble
Requires [Harmony](https://github.com/pardeike/Harmony/releases)

Requires my [workshop mod](https://steamcommunity.com/sharedfiles/filedetails/?id=2870068059) to disabled explosion wobbles if using the default configs. Not needed if you only want to disable flinching or use your own explosion replacements.

Unturned rockemod plugin that removes the flinching effects from getting hurt and from explosions. 

Should work with modded explosions as of 1.2.0, but you must create your own workshop mod and add the ids to the config.

HP calc offset is used when calculating if a person should flinch or not. The formula is (hp - pending damage + offset > 0) and if true the player will not flinch. This can be used to make people flinch only when say below 50 hp by setting it to 50. It's set to 10 by default to try and prevent weird edge cases where flinching still occurs even though you did not die. The purpose of this is to prevent it from messing with ragdolls.