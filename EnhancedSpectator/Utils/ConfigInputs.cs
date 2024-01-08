using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;

namespace EnhancedSpectator.Utils
{
    internal class ConfigInputs : LcInputActions
    {
        public InputAction FlashlightBinding => Asset["FlashlightBinding"];
        public InputAction NightVisionBinding => Asset["NightVisionBinding"];

        /// <summary>
        /// Creates the input bindings with configuration values
        /// </summary>
        /// <param name="builder"></param>
        public override void CreateInputActions(in InputActionMapBuilder builder)
        {
            base.CreateInputActions(builder);

            builder.NewActionBinding()
                .WithActionId("FlashlightBinding")
                .WithActionType(InputActionType.Button)
                .WithKbmPath(ConfigSettings.FlashlightKeybind.Value)
                .WithBindingName("Flashlight")
                .Finish();

            builder.NewActionBinding()
                .WithActionId("NightVisionBinding")
                .WithActionType(InputActionType.Button)
                .WithKbmPath(ConfigSettings.NightVisionKeybind.Value)
                .WithBindingName("NightVision")
                .Finish();
        }
    }
}
