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
        public static ConfigEntry<float> RaisedClockOffset;

        /// <summary>
        /// Initializes the configuration options
        /// </summary>
        /// <param name="config"></param>
        /// <param name="description"></param>
        public static void Initialize(ConfigFile config, string description)
        {
            string category = "Night Vision";
            NightVisionAllowed = config.Bind<bool>(category, "Allowed", true, "Allow night vision while spectating.");
            NightVisionIntensity = config.Bind<float>(category, "Intensity", 7500f, "This is how bright the night vision makes the environment when enabled.");
            ModifyDarkness = config.Bind<bool>(category, "Modify Darkness", false, "Some mods (Diversity) change the default darkness intensity value. This setting enables the below option.");
            DarknessModifier = config.Bind<float>(category, "Darkness Modifier", 1f, "This option modifies the default darkness intensity value.");
            NightVisionKeybind = config.Bind<string>(category, "Keybind", "<Keyboard>/n", "Input binding to toggle night vision while spectating.");

            category = "Flashlight";
            FlashlightAllowed = config.Bind<bool>(category, "Allowed", true, "Allow flashlight while spectating.");
            FlashlightKeybind = config.Bind<string>(category, "Keybind", "<Keyboard>/f", "Input binding to toggle flashlight while spectating.");

            category = "Clock";
            ClockAllowed = config.Bind<bool>(category, "Allowed", true, "Allow clock while spectating.");
            RaisedClockSupport = config.Bind<bool>(category, "Raised Clock Support", false, "Moves the text showing who you are spectating down a bit to support mods that move the clock position higher (e.g. LCBetterClock).");
            RaisedClockOffset = config.Bind<float>(category, "Offset", -25f, "How much to offset the spectating text by on the Y axis.");

            try
            {
                if (PluginExists("ainavt.lc.lethalconfig")) SetupLethalConfig(description);
            }
            catch { }
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
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(RaisedClockOffset, new FloatSliderOptions { Min = -465f, Max = 30f, RequiresRestart = false }));

        }
    }
}
