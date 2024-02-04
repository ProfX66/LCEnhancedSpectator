using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using EnhancedSpectator.Utils;
using Figgle;
using UnityEngine.InputSystem;
using GameNetcodeStuff;
using UnityEngine;
using EnhancedSpectator.Patches;
using BepInEx.Bootstrap;
using System.Collections.Generic;
using System.Linq;

namespace EnhancedSpectator
{
    public class PluginMetadata
    {
        public const string Author = "PXC";
        public const string Name = "EnhancedSpectator";
        public const string Id = "PXC.EnhancedSpectator";
        public const string Version = "1.0.5";
        public string FullName => string.Format("{0} v{1}", Name, Version);
    }

    [BepInPlugin(PluginMetadata.Id, PluginMetadata.Name, PluginMetadata.Version)]
    [BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
    internal class LCES : BaseUnityPlugin
    {
        #region Plugin Entry

        public static PluginMetadata pluginMetadata = new PluginMetadata();
        public static ManualLogSource Log = new ManualLogSource(PluginMetadata.Id);
        public static LCES Instance;
        public static ConfigInputs Inputs { get; set; }

        /// <summary>
        /// Plugin entry point
        /// </summary>
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            Log = Logger;
            Log.LogInfo($"Initializing plugin: {pluginMetadata.FullName} by {PluginMetadata.Author}");

            Harmony.CreateAndPatchAll(typeof(HUDManagerPatches));
            Log.LogInfo("[Patched] HUDManager");

            Harmony.CreateAndPatchAll(typeof(PlayerControllerBPatches));
            Log.LogInfo("[Patched] PlayerControllerB");

            Harmony.CreateAndPatchAll(typeof(StartOfRoundPatches));
            Log.LogInfo("[Patched] StartOfRound");

            HUDManagerPatches.SpectatedTextSpacer = "\n\n";
            ConfigSettings.Initialize(Config, "Enhances the spectating experience.");

            Inputs = new ConfigInputs();
            Inputs.FlashlightBinding.performed += OnFlashlightPressed;
            Inputs.NightVisionBinding.performed += OnNightVisionPressed;
            ConfigSettings.FlashlightAllowed.SettingChanged += FlashlightAllowed_SettingChanged;
            ConfigSettings.NightVisionAllowed.SettingChanged += NightVisionAllowed_SettingChanged;

#if DEBUG
            Log.LogWarning($"Loaded! (IN DEBUG)\n{FiggleFonts.Doom.Render(pluginMetadata.FullName)}");
#endif
#if !DEBUG
            Log.LogInfo($"Loaded!\n{FiggleFonts.Doom.Render(pluginMetadata.FullName)}");
#endif
        }

        #endregion

        #region Actions

        /// <summary>
        /// Change flashlight state
        /// </summary>
        private void ToggleSpecFlashlight()
        {
            if (GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null)
            {
                PlayerControllerB CurrentPlayer = GameNetworkManager.Instance.localPlayerController;
                if (HUDManager.Instance != null && CurrentPlayer.IsOwner && CurrentPlayer.isPlayerDead && !StartOfRound.Instance.shipIsLeaving && (!CurrentPlayer.IsServer || CurrentPlayer.isHostPlayerObject))
                {
                    Light Flashlight = HUDManager.Instance.playersManager.spectateCamera.GetComponent<Light>();
                    if (Flashlight != null && ConfigSettings.FlashlightAllowed.Value)
                    {
                        Flashlight.enabled = !Flashlight.enabled;
                    }
                    else
                    {
                        Flashlight.enabled = false;
                    }

                    HUDManagerPatches.FlashLightStatus = Flashlight.enabled;
                }
            }
        }

        /// <summary>
        /// Change night vision state
        /// </summary>
        private void ToggleSpecNightVision()
        {
#if DEBUG
            Log.LogInfo("ToggleSpecNightVision");
#endif
            if (GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null)
            {
#if DEBUG
                Log.LogInfo("ToggleSpecNightVision => GameNetworkManager");
#endif
                PlayerControllerB CurrentPlayer = GameNetworkManager.Instance.localPlayerController;
                if (HUDManager.Instance != null && CurrentPlayer.IsOwner && CurrentPlayer.isPlayerDead && !StartOfRound.Instance.shipIsLeaving && (!CurrentPlayer.IsServer || CurrentPlayer.isHostPlayerObject))
                {
#if DEBUG
                    Log.LogInfo(string.Format("ToggleSpecNightVision => GameNetworkManager => HUDManager/CurrentPlayer => NightVisionToggled: {0}", PlayerControllerBPatches.NightVisionToggled));
                    Log.LogInfo(string.Format("ToggleSpecNightVision => GameNetworkManager => HUDManager/CurrentPlayer => NightVisionAllowed: {0}", ConfigSettings.NightVisionAllowed.Value));
                    Log.LogInfo(string.Format("ToggleSpecNightVision => GameNetworkManager => HUDManager/CurrentPlayer => NightvisionStatus: {0}", HUDManagerPatches.NightvisionStatus));
#endif
                    if (ConfigSettings.NightVisionAllowed.Value)
                    {
                        PlayerControllerBPatches.NightVisionToggled = !PlayerControllerBPatches.NightVisionToggled;
                    }
                    else
                    {
                        PlayerControllerBPatches.NightVisionToggled = false;
                    }

                    HUDManagerPatches.NightvisionStatus = PlayerControllerBPatches.NightVisionToggled;
#if DEBUG
                    Log.LogInfo(string.Format("ToggleSpecNightVision => GameNetworkManager => HUDManager/CurrentPlayer => NightvisionStatus: {0}", HUDManagerPatches.NightvisionStatus));
#endif
                }
            }
        }

#endregion

        #region Events

        /// <summary>
        /// Flashlight button pressed event
        /// </summary>
        /// <param name="context"></param>
        private void OnFlashlightPressed(InputAction.CallbackContext context)
        {
#if DEBUG
            Log.LogInfo("OnFlashlightPressed");
#endif
            if (!context.performed) { return; }
            ToggleSpecFlashlight();
        }

        /// <summary>
        /// Night vision button pressed event
        /// </summary>
        /// <param name="context"></param>
        private void OnNightVisionPressed(InputAction.CallbackContext context)
        {
#if DEBUG
            Log.LogInfo("OnNightVisionPressed");
#endif
            if (!context.performed) { return; }
            ToggleSpecNightVision();
        }

        /// <summary>
        /// Disable spectator flashlight if settings changed in game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlashlightAllowed_SettingChanged(object sender, System.EventArgs e)
        {
            ToggleSpecFlashlight();
        }

        /// <summary>
        /// Disable spectator night vision if settings changed in game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NightVisionAllowed_SettingChanged(object sender, System.EventArgs e)
        {
            ToggleSpecNightVision();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Tests if a plugin is available or not
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>bool</returns>
        public static bool PluginExists(string Name, bool ShowWarning = true)
        {
            if (Chainloader.PluginInfos.ContainsKey(Name))
            {
                KeyValuePair<string, PluginInfo> plugin = Chainloader.PluginInfos.FirstOrDefault(n => n.Key == Name);

                if (ShowWarning) Log.LogInfo($"[SoftDependency] Found plugin: {plugin.Value.Metadata.Name} ({plugin.Value.Metadata.GUID}) v{plugin.Value.Metadata.Version} - Initializing methods...");
                return true;
            }

            if (ShowWarning) Log.LogWarning($"[SoftDependency] Unable to find plugin '{Name}' - Skipping its initialization!");
            return false;
        }

        #endregion
    }
}
