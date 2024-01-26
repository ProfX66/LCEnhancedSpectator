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