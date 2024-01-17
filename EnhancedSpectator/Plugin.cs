using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using EnhancedSpectator.Utils;
using Figgle;
using System.Reflection;
using UnityEngine.InputSystem;
using GameNetcodeStuff;
using UnityEngine;
using EnhancedSpectator.Patches;

namespace EnhancedSpectator
{
    [BepInPlugin(PluginGUID, PluginName, VersionString)]
    [BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.HardDependency)]
    internal class LCES : BaseUnityPlugin
    {
        #region Plugin Information

        private const string PluginGUID = "PXC.EnhancedSpectator";
        private const string PluginName = "EnhancedSpectator";
        private const string VersionString = "1.0.3";
        public static string PluginFullName { get => string.Format("{0} v{1}", PluginName, VersionString); }
        public static string PluginAuthor { get => "PXC"; }

        #endregion

        #region Plugin Declarations

        public static ManualLogSource Log = new ManualLogSource(PluginName);
        public static ConfigInputs Inputs { get; set; }
        public static LCES Instance;

        #endregion

        #region Entry

        /// <summary>
        /// Plugin entry point
        /// </summary>
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGUID);

            Log.LogInfo(string.Format("Initializing plugin: {0} by {1}", PluginFullName, PluginAuthor));
            HUDManagerPatches.SpectatedTextSpacer = "\n\n";
            ConfigSettings.Initialize(Config, "Enhances the spectating experience.");

            Inputs = new ConfigInputs();
            Inputs.FlashlightBinding.performed += OnFlashlightPressed;
            Inputs.NightVisionBinding.performed += OnNightVisionPressed;
            ConfigSettings.FlashlightAllowed.SettingChanged += FlashlightAllowed_SettingChanged;
            ConfigSettings.NightVisionAllowed.SettingChanged += NightVisionAllowed_SettingChanged;

            Log.LogInfo(string.Format("Loaded!\n{0}", FiggleFonts.Doom.Render(PluginFullName)));
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
    }
}
