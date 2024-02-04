# Enhanced Spectator
Enhances the spectating experience for Lethal Company.

## Features
- Adds the clock to the spectator HUD \- Inspired by [BetterSpec](https://thunderstore.io/c/lethal-company/p/ZG/BetterSpec/) by ZG
- Adds the ability to toggle a flashlight while spectating \- Inspired by [SpectateEnemies](https://thunderstore.io/c/lethal-company/p/AllToasters/SpectateEnemies/) by AllToasters
- Adds the ability to toggle night vision while spectating \- Inspired by [ToggleableNightVision](https://thunderstore.io/c/lethal-company/p/kentrosity/ToggleableNightVision/) by Kentrosity
  - Will only be visible when the player being spectated is in the dungeon

## Planned Features
- [ ] Ability to change the zoom level while spectating
- [ ] Ability to change the FOV while spectating
- [ ] Potentially toggle between first and third person while spectating
- [_**x**_] Compatibility with other spectator mods that modify the HUD
- [_**x**_] Enhance and optimize the UI changes

## Compatibility
The following are mods either tested to be compatible or not

### Incompatible
- [BetterSpec](https://thunderstore.io/c/lethal-company/p/ZG/BetterSpec/): This is already handled by EnhancedSpectator
- No others have been found/reported currently

### Fully Compatible (Tested by me or the community)
- [ShipLootPlus](https://thunderstore.io/c/lethal-company/p/PXC/ShipLootPlus/)
- [GeneralImprovements](https://thunderstore.io/c/lethal-company/p/ShaosilGaming/GeneralImprovements/)
- [AdvancedCompany](https://thunderstore.io/c/lethal-company/p/PotatoePet/AdvancedCompany/)
- [LCBetterClock](https://thunderstore.io/c/lethal-company/p/BlueAmulet/LCBetterClock/)
- [EladsHUD](https://thunderstore.io/c/lethal-company/p/EladNLG/EladsHUD//)
- Likely most mods (ill do a more comprehensive test with the most popular mods later)

### Mostly Compatible (Everything is functional just has quirks)
- [SpectateEnemies](https://thunderstore.io/c/lethal-company/p/AllToasters/SpectateEnemies/)
  - The only part that doesn't work is when spectating a mob the night vision doesn't work (this seems to be a vanilla limitation, I am still investigating if I can change it)
- [ToggleableNightVision](https://thunderstore.io/c/lethal-company/p/kentrosity/ToggleableNightVision/)
  - This works fine but when toggling night vision in either mod it will disable it for the other, so any text prompts wont be synced (I'll likely fix  this in the future)

## Configuration
Everything is configurable via the config file or the [LethalConfig](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/) menu (both the main menu and the pause menu).

> **_NOTE:_** With **v1.0.4+** LethalConfig is a soft dependency but to get the best experience it is still required by the Thunderstore package. It allows you to change any setting for EnhancedSpectator at any time, and it will update everything in real time. If do not want LethalConfig in your pack, you can safely remove it and this mod will still work just fine.

<details>
  <summary>Spectator Clock</summary>
  
  ### Enabling or Disabling the spectator clock
  The ```Allowed``` setting lets you enable or disable the clock entirely

  ```cfg
    ## Allow clock while spectating.
    # Setting type: Boolean
    # Default value: true
    Allowed = true
  ```
  
  ### Move the "(Specating: Player)" text down
  The ```Raised Clock Support``` setting lets you move the spectating text down so it doesn't interfere with the clock if you are using mods that move the clocks position higher (e.g. [LCBetterClock](https://thunderstore.io/c/lethal-company/p/BlueAmulet/LCBetterClock/)).

  ```cfg
    ## Moves the text showing who you are spectating down a bit to support mods that move the clock position higher (e.g. LCBetterClock).
    # Setting type: Boolean
    # Default value: false
    Raised Clock Support = false
  ```

  ### How much to move the "(Specating: Player)" text down/up
  This is how much to offset the spectating text element down or up. The text element is actually moved so this retains support for any mod that adds text to it (e.g. [SpectateEnemies](https://thunderstore.io/c/lethal-company/p/AllToasters/SpectateEnemies/))

  ```cfg
    ## How much to offset the spectating text by on the Y axis.
    # Setting type: Single
    # Default value: -25
    # Range: -465 - 30
    Offset = -25
  ```
  
</details>

<details>
  <summary>Spectator Flashlight</summary>
  
  ### Enabling or Disabling the spectator flashlight
  The ```Allowed``` setting lets you enable or disable the flashlight entirely.

  ```cfg
    ## Allow flashlight while spectating.
    # Setting type: Boolean
    # Default value: true
    Allowed = true
  ```
  
  ### Change the spectator flashlight key binding (Requires game restart)
  The ```Keybind``` setting lets you customize which keybind the mod uses for the flashlight.

  ```cfg
    ## Input binding to toggle flashlight while spectating.
    # Setting type: String
    # Default value: <Keyboard>/f
    Keybind = <Keyboard>/f

  ```
</details>

<details>
  <summary>Spectator Night Vision</summary>
  
  ### Enabling or Disabling spectator night vision
  The ```Allowed``` setting lets you enable or disable night vision entirely.

  ```cfg
    ## Allow night vision while spectating.
    # Setting type: Boolean
    # Default value: true
    Allowed = true
  ```
  
  ### Set the intensity of night vision
  The ```Intensity``` setting lets you change how bright the night vision is.

  ```cfg
    ## This is how bright the night vision makes the environment when enabled.
    # Setting type: Single
    # Default value: 7500
    # Range: 100 - 100000 (higher is brighter)
    Intensity = 7500
  ```
  
  ### Enable darkness intensity scaling
  The ```Modify Darkness``` setting enables the ```Darkness Modifier``` setting below. Its used to change the darkness intensity scaling. Use this if you are running mods like Diversity that have darker darkness values.

  ```cfg
    ## Some mods (Diversity) change the default darkness intensity value. This setting enables the below option.
    # Setting type: Boolean
    # Default value: false
    Modify Darkness = false
  ```
  
  ### Modify the darkness intensity scale
  The ```Darkness Modifier``` setting lets you change the default darkness intensity scaling.

  ```cfg
    ## This option modifies the default darkness intensity value.
    # Setting type: Single
    # Default value: 1
    # Range: 0.0 - 1.0
    Darkness Modifier = 1
  ```
  
  ### Change the spectator night vision key binding (Requires game restart)
  The ```Keybind``` setting lets you customize which keybind the mod uses for the night vision.

  ```cfg
    ## Input binding to toggle night vision while spectating.
    # Setting type: String
    # Default value: <Keyboard>/n
    Keybind = <Keyboard>/n
  ```
</details>

## Other Mods
[![ShipLootPlus](https://gcdn.thunderstore.io/live/repository/icons/PXC-ShipLootPlus-1.0.0.png.128x128_q95.png 'ShipLootPlus')](https://thunderstore.io/c/lethal-company/p/PXC/ShipLootPlus/)
[![PrideSuits](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuits-1.0.2.png.128x128_q95.jpg 'PrideSuits')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuits/)
[![PrideSuitsAnimated](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideSuitsAnimated-1.0.1.png.128x128_q95.jpg 'PrideSuitsAnimated')](https://thunderstore.io/c/lethal-company/p/PXC/PrideSuitsAnimated/)
[![PrideCosmetics](https://gcdn.thunderstore.io/live/repository/icons/PXC-PrideCosmetics-1.0.2.png.128x128_q95.png 'PrideCosmetics')](https://thunderstore.io/c/lethal-company/p/PXC/PrideCosmetics/)