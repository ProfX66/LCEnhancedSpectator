using HarmonyLib;

namespace EnhancedSpectator.Patches
{
    [HarmonyPatch]
    internal class StartOfRoundPatches
    {
        /// <summary>
        /// Reset night vision and hud opacity on end of round
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "AutoSaveShipData")]
        private static void AutoSaveShipData()
        {
#if DEBUG
            LCES.Log.LogWarning("!!!!!!!]> AutoSaveShipData");
#endif
            PlayerControllerBPatches.IsDead = false;
            PlayerControllerBPatches.NightVisionToggle(GameNetworkManager.Instance.localPlayerController, false);
            HUDManager.Instance.PlayerInfo.targetAlpha = 1f;
            HUDManager.Instance.Inventory.targetAlpha = 1f;
            HUDManagerPatches.ClockStatus = false;
        }

        /// <summary>
        /// Reset night vision on player revive
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "ReviveDeadPlayers")]
        private static void PlayerHasRevivedServerRpc()
        {
#if DEBUG
            LCES.Log.LogInfo("!!!!!!!]> AutoSaveShipData");
#endif
            PlayerControllerBPatches.IsDead = false;
            PlayerControllerBPatches.NightVisionToggle(GameNetworkManager.Instance.localPlayerController, false);
        }

        /// <summary>
        /// Reset night vision on end of game
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "EndOfGameClientRpc")]
        private static void RefreshDay()
        {
#if DEBUG
            LCES.Log.LogInfo("!!!!!!!]> AutoSaveShipData");
#endif
            PlayerControllerBPatches.IsDead = false;
            PlayerControllerBPatches.NightVisionToggle(GameNetworkManager.Instance.localPlayerController, false);
        }
    }
}
