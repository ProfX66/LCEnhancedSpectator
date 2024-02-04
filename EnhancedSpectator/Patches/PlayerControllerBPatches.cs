using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using EnhancedSpectator.Utils;

namespace EnhancedSpectator.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB), "Update")]
    internal class PlayerControllerBPatches
    {
        internal static bool NightVisionToggled = false;
        internal static bool WasToggledOnce = false;
        public static bool IsDead = false;

        /// <summary>
        /// PlayerControllerB patches to enable the use of night vision while spectating
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPrefix]
        private static void Prefix(PlayerControllerB __instance)
        {
            if (GameNetworkManager.Instance.localPlayerController != null && GameNetworkManager.Instance.localPlayerController != null)
            {
                PlayerControllerB CurrentPlayer = GameNetworkManager.Instance.localPlayerController;
                if (WasToggledOnce && StartOfRound.Instance.shipIsLeaving)
                {
                    LCES.Log.LogInfo("Ship is leaving - Resetting toggle states...");
                    WasToggledOnce = false;
                    IsDead = false;
                    NightVisionToggle(__instance, false);
                }

                if (CurrentPlayer.isPlayerDead && !StartOfRound.Instance.shipIsLeaving)
                {
#if DEBUG
                    LCES.Log.LogInfo("PlayerControllerBPatches.Prefix => Player Dead");
#endif
                    NightVisionToggle(__instance, NightVisionToggled);
                    if (!WasToggledOnce) { WasToggledOnce = true; IsDead = true; }
                }
                else
                {
                    if (WasToggledOnce)
                    {
#if DEBUG
                        LCES.Log.LogInfo("PlayerControllerBPatches.Prefix => Player Alive");
#endif
                        NightVisionToggled = false;
                        NightVisionToggle(__instance, false);
                    }
                }
            }
            else
            {
                if (WasToggledOnce)
                {
#if DEBUG
                    LCES.Log.LogInfo("PlayerControllerBPatches.Prefix => NOT GameNetworkManager");
#endif
                    NightVisionToggled = false;
                    NightVisionToggle(__instance, false);
                }
            }
        }

        /// <summary>
        /// Toggles the night vision for the passed instance
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="ToggleAction"></param>
        public static void NightVisionToggle(PlayerControllerB __instance, bool ToggleAction)
        {
#if DEBUG
            LCES.Log.LogInfo(string.Format("[{0}/{1}] PlayerControllerBPatches.NightVisionToggle => ToggleAction: {2}", __instance.isPlayerDead, IsDead, ToggleAction.ToString()));
            LCES.Log.LogWarning($"Dead? {__instance.isPlayerDead}\nIsServer? {__instance.IsServer}\nisPlayerControlled? {__instance.isPlayerControlled}\nIsLocalPlayer: {__instance.IsLocalPlayer}\nNull? {__instance == null}");
#endif
            if (!ConfigSettings.NightVisionAllowed.Value) { ToggleAction = false; }
            if (!IsDead) { ToggleAction = false; }
#if DEBUG
            LCES.Log.LogInfo(string.Format("[{0}/{1}] PlayerControllerBPatches.NightVisionToggle => Allowed => ToggleAction: {2}", __instance.isPlayerDead, IsDead, ToggleAction.ToString()));
#endif
            if (ToggleAction)
            {
                __instance.nightVision.intensity = ConfigSettings.NightVisionIntensity.Value;
                __instance.nightVision.range = 100000f;
                __instance.nightVision.shadowStrength = 0f;
                __instance.nightVision.shadows = (LightShadows)0;
                __instance.nightVision.shape = (LightShape)2;
            }
            else
            {
                float DarknessModifier = ConfigSettings.ModifyDarkness.Value ? Mathf.Clamp(1f - ConfigSettings.DarknessModifier.Value, 0f, 1f) : 1f;
#if DEBUG
                LCES.Log.LogInfo(string.Format("DarknessModifier: {0}", DarknessModifier.ToString()));
#endif
                __instance.nightVision.intensity = 366.9317f * DarknessModifier;
                __instance.nightVision.range = 12f;
                __instance.nightVision.shadowStrength = 1f;
                __instance.nightVision.shadows = (LightShadows)0;
                __instance.nightVision.shape = (LightShape)0;
            }
        }
    }
}
