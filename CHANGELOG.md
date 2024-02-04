# v1.0.5
- Added compatibility for [MelonLoader](https://thunderstore.io/c/lethal-company/p/BepInEx/BepInEx_MLLoader) so EnhancedSpectator loads correctly if MelonLoader is present
- Aligned the code closer to my coding standards used in ShipLootPlus to be more consistent
- Changed how the flashlight enable/disable text is set, so any other mod with a spectator flash light doesn't cause text prompt sync issues
- Changed how the raised clock feature works, the spectating text object is now moved in a way that wont impact any mod that modifies the spectating text content
- Added a config option to tweak the spectating text Y position offset (can move it up or down with raised clock support if needed)
- Added compatibility section to readme
- Fixed a rare issue where night vision could persist after being revived in orbit when the round ended
- Fixed some minor typos

# v1.0.4
- Fixed an issue that stopped the player hud from fading when interacting with the terminal
- Changed LethalConfig to a soft dependency so it can safely be removed (if desired) and not break EnhancedSpectator
  - The Thunderstore package will still have it as a dependency as using it is the intended way to configure EnhancedSpectator
- Compiled with the latest versions of LethalConfig and InputUtils

# v1.0.3
- Fixed compatibility with other mods that enable night vision
- Added compatibility for mods like Diversity which modify the default darkness intensity

# v1.0.2
- v47 support

# v1.0.1
- Removed Nuget dependency weaving to be compliant with Thunderstore policies.

# v1.0.0
- Release