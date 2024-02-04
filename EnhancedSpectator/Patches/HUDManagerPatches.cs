using System.Text.RegularExpressions;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using EnhancedSpectator.Utils;
using System.Text;

namespace EnhancedSpectator.Patches
{
    [HarmonyPatch(typeof(HUDManager), "Update")]
    internal class HUDManagerPatches
    {
        private static string FlashLightAction { get; set; }
        private static string NightvisionAction { get; set; }
        private static string SpectatedPlayerText { get; set; }
        public static string SpectatedTextSpacer { get; set; }
        public static bool FlashLightStatus { get; set; }
        public static bool NightvisionStatus { get; set; }
        public static bool FoundSpectating { get; set; }
        
        public static bool? ClockStatus { get; set; }
        public static bool? ModifiedSpectateText { get; set; }
        public static GameObject Spectating { get; set; }
        public static Vector3 SpectatingPos { get; set; }

        /// <summary>
        /// HUD patches to modify the spectator HUD items
        /// </summary>
        /// <param name="__instance"></param>
        private static void Postfix(HUDManager __instance)
        {
            if (SpectatedTextSpacer == null) { SpectatedTextSpacer = ""; }
            if (GameNetworkManager.Instance.localPlayerController == null) return;

            if (Spectating == null) {  }

            Spectating = Spectating ?? (Spectating = GameObject.Find("/Systems/UI/Canvas/DeathScreen/SpectateUI/Spectating"));
            if (!FoundSpectating && Spectating)
            {
                FoundSpectating = true;
                SpectatingPos = Spectating.transform.localPosition;
            }

            if (ConfigSettings.RaisedClockSupport.Value)
            {
                if (!Spectating)
                {
                    string TempText = Regex.Replace(__instance.spectatingPlayerText.text, @"\n", "", RegexOptions.IgnoreCase);
                    if (string.IsNullOrEmpty(SpectatedPlayerText)) { SpectatedPlayerText = TempText; }
                    else
                    {
                        if (!Regex.IsMatch(TempText, string.Format("^{0}$", Regex.Escape(SpectatedPlayerText)), RegexOptions.IgnoreCase))
                        {
                            SpectatedPlayerText = TempText;
                            __instance.spectatingPlayerText.text = string.Concat(SpectatedTextSpacer, TempText);
                        }
                        else
                        {
                            if (!Regex.IsMatch(__instance.spectatingPlayerText.text, @"\n", RegexOptions.IgnoreCase))
                            {
                                __instance.spectatingPlayerText.text = string.Concat(SpectatedTextSpacer, TempText);
                            }
                        }
                    }
                }
                else
                {
                    Spectating.transform.localPosition = new Vector3(SpectatingPos.x, SpectatingPos.y + ConfigSettings.RaisedClockOffset.Value, SpectatingPos.z);
                }
            }
            else
            {
                if (!Spectating)
                {
                    __instance.spectatingPlayerText.text = Regex.Replace(__instance.spectatingPlayerText.text, @"\n", "", RegexOptions.IgnoreCase);
                }
                else
                {
                    Spectating.transform.localPosition = SpectatingPos;
                }
            }

            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;

            if (player.isPlayerDead)
            {
                Light Flashlight = __instance.playersManager.spectateCamera.GetComponent<Light>();
                if (StartOfRound.Instance.shipIsLeaving)
                {
                    if (Flashlight != null) { Flashlight.enabled = false; }
                    return;
                }

                string FlashlightBinding = LCES.Inputs.FlashlightBinding.GetBindingDisplayString();
                string NightVisionBinding = LCES.Inputs.NightVisionBinding.GetBindingDisplayString();

                FlashLightStatus = Flashlight.enabled;
                FlashLightAction = Flashlight.enabled ? "Disable" : "Enable";
                NightvisionAction = NightvisionStatus ? "Disable" : "Enable";

                StringBuilder sb = new StringBuilder();
                sb.Append("\n\n\n\n\n");
                if (ConfigSettings.FlashlightAllowed.Value) { sb.AppendLine(string.Format("{0} Flashlight : [{1}]", FlashLightAction, FlashlightBinding)); }
                if (ConfigSettings.NightVisionAllowed.Value) { sb.AppendLine(string.Format("{0} Night Vision : [{1}]", NightvisionAction, NightVisionBinding)); }
                __instance.holdButtonToEndGameEarlyText.text += sb.ToString();
            }
        }

        /// <summary>
        /// Show the clock on the spectator HUD
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPrefix]
        static void ClockOnSpectate(HUDManager __instance)
        {
            if (__instance != null && (HUDManager.Instance?.Clock) != null && (GameNetworkManager.Instance?.localPlayerController) != null)
            {
                PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
                if (player.isPlayerDead)
                {
                    if (ConfigSettings.ClockAllowed.Value)
                    {
                        if (HUDManager.Instance.Clock != null)
                        {
                            HUDManager.Instance.HideHUD(false);
                            HUDManager.Instance.SetClockVisible(true);
                            HUDManager.Instance.Clock.targetAlpha = 1f;
                            HUDManager.Instance.Inventory.targetAlpha = 0f;
                            HUDManager.Instance.PlayerInfo.targetAlpha = 0f;
                            ClockStatus = true;
                        }
                    }
                    else
                    {
                        HUDManager.Instance.HideHUD(true);
                        HUDManager.Instance.SetClockVisible(false);
                        ClockStatus = false;
                    }
                }
            }
        }
    }
}
