using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using static EnhancedSpectator.LCES;

namespace EnhancedSpectator.Utils
{
    public class ConfigSettings
    {
        public static ConfigEntry<bool> NightVisionAllowed;
        public static ConfigEntry<float> NightVisionIntensity;
        public static ConfigEntry<bool> ModifyDarkness;
        public static ConfigEntry<float> DarknessModifier;
        public static ConfigEntry<string> NightVisionKeybind;
        public static ConfigEntry<bool> FlashlightAllowed;
        public static ConfigEntry<string> FlashlightKeybind;
        public static ConfigEntry<bool> ClockAllowed;
        public static ConfigEntry<bool> RaisedClockSupport;

        /// <summary>
        /// Initializes the configuration options
        /// </summary>
        /// <param name="config"></param>
        /// <param name="description"></param>
        public static void Initialize(ConfigFile config, string description)
        {
            #region Night Vision

            NightVisionAllowed = config.Bind<bool>("Night Vision", "Allowed", true, "Allow night vision while spectating.");
            NightVisionIntensity = config.Bind<float>("Night Vision", "Intensity", 7500f, "This is how bright the night vision makes the environment when enabled.");
            ModifyDarkness = config.Bind<bool>("Night Vision", "Modify Darkness", false, "Some mods (Diversity) change the default darkness intensity value. This setting enables the below option.");
            DarknessModifier = config.Bind<float>("Night Vision", "Darkness Modifier", 1f, "This option modifies the default darkness intensity value.");
            NightVisionKeybind = config.Bind<string>("Night Vision", "Keybind", "<Keyboard>/n", "Input binding to toggle night vision while spectating.");

            #endregion

            #region Flashlight

            FlashlightAllowed = config.Bind<bool>("Flashlight", "Allowed", true, "Allow flashlight while spectating.");
            FlashlightKeybind = config.Bind<string>("Flashlight", "Keybind", "<Keyboard>/f", "Input binding to toggle flashlight while spectating.");

            #endregion

            #region Clock

            ClockAllowed = config.Bind<bool>("Clock", "Allowed", true, "Allow clock while spectating.");
            RaisedClockSupport = config.Bind<bool>("Clock", "Rasied Clock Support", false, "Moves the text showing who you are spectating down a bit to support mods that move the clock position higher (e.g. LCBetterClock).");

            #endregion

            try
            {
                if (AssemblyExists("LethalConfig")) SetupLethalConfig(description);
            }
            catch { Log.LogWarning("LethalSettings was not found - Skipping its initialization..."); }
        }

        private static void SetupLethalConfig(string description)
        {
            LethalConfigManager.SetModDescription(description);

            //Night Vision
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(NightVisionAllowed, false));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(NightVisionIntensity, new FloatSliderOptions { Min = 100f, Max = 100000f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ModifyDarkness, false));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(DarknessModifier, new FloatSliderOptions { Min = 0f, Max = 1f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(NightVisionKeybind, true));

            //Flashlight
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(FlashlightAllowed, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(FlashlightKeybind, true));

            //Clock
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ClockAllowed, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(RaisedClockSupport, false));

        }
    }
}
