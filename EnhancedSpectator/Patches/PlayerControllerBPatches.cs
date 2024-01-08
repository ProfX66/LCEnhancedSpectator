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
                if (CurrentPlayer.isPlayerDead && !StartOfRound.Instance.shipIsLeaving)
                {
                    NightVisionToggle(__instance, NightVisionToggled);
                }
                else
                {
                    NightVisionToggled = false;
                    NightVisionToggle(__instance, false);
                }
            }
            else
            {
                NightVisionToggled = false;
                NightVisionToggle(__instance, false);
            }
        }

        /// <summary>
        /// Toggles the night vision for the passed instance
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="ToggleAction"></param>
        private static void NightVisionToggle(PlayerControllerB __instance, bool ToggleAction)
        {
            if (!ConfigSettings.NightVisionAllowed.Value) { ToggleAction = false; }
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
                __instance.nightVision.intensity = 366.9317f;
                __instance.nightVision.range = 12f;
                __instance.nightVision.shadowStrength = 1f;
                __instance.nightVision.shadows = (LightShadows)0;
                __instance.nightVision.shape = (LightShape)0;
            }
        }
    }
}
