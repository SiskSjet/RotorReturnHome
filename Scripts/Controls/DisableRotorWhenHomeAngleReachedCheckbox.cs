using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using Sisk.RotorReturnHome.Extensions;
using Sisk.RotorReturnHome.Localization;
using Sisk.RotorReturnHome.LogicComponent;
using System.Collections.Generic;

namespace Sisk.RotorReturnHome.Controls {

    internal static class DisableRotorWhenHomeAngleReachedCheckbox {
        private const string ID = "DisableRotorWhenHomeAngleReached";

        private static IEnumerable<IMyTerminalAction> _actions;
        private static IMyTerminalControlCheckbox _control;
        private static IMyTerminalControlProperty<bool> _property;

        public static IEnumerable<IMyTerminalAction> Actions => _actions ?? (_actions = CreateActions());

        public static IMyTerminalControlCheckbox Control => _control ?? (_control = CreateControl());
        public static IMyTerminalControlProperty<bool> Property => _property ?? (_property = CreateProperty());

        private static IEnumerable<IMyTerminalAction> CreateActions() {
            var actions = new List<IMyTerminalAction> {
                Control.CreateToggleAction<IMyMotorBase>()
            };

            return actions;
        }

        private static IMyTerminalControlCheckbox CreateControl() {
            var control = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCheckbox, IMyMotorBase>(ID);

            control.Title = ModText.BlockPropertyTitle_DisableRotor;
            control.Tooltip = ModText.BlockPropertyTooltip_DisableRotor;
            control.OnText = ModText.BlockPropertyOnText_DisableRotor;
            control.OffText = ModText.BlockPropertyOffText_DisableRotor;
            control.Enabled = Enabled;
            control.SupportsMultipleBlocks = true;
            control.Getter = Getter;
            control.Setter = Setter;

            return control;
        }

        private static IMyTerminalControlProperty<bool> CreateProperty() {
            return Control.CreateProperty<IMyMotorBase>();
        }

        private static bool Enabled(IMyTerminalBlock block) {
            return true;
        }

        private static bool Getter(IMyTerminalBlock block) {
            var logic = block.GameLogic?.GetAs<StatorGameLogic>();

            if (logic != null) {
                return logic.DisableRotorWhenHomePositionReached;
            }

            return false;
        }

        private static void Setter(IMyTerminalBlock block, bool value) {
            var logic = block.GameLogic?.GetAs<StatorGameLogic>();

            if (logic != null) {
                logic.DisableRotorWhenHomePositionReached = value;
            }
        }
    }
}