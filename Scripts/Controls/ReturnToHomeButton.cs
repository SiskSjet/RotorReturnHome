using Sandbox.ModAPI.Interfaces.Terminal;
using Sandbox.ModAPI;
using System.Collections.Generic;
using Sisk.RotorReturnHome.Extensions;
using Sisk.RotorReturnHome.LogicComponent;
using Sisk.RotorReturnHome.Localization;

namespace Sisk.RotorReturnHome.Controls {

    internal static class ReturnToHomeButton {
        private const string ID = "ReturnToHome";

        private static IEnumerable<IMyTerminalAction> _actions;
        private static IMyTerminalControlButton _control;

        public static IEnumerable<IMyTerminalAction> Actions => _actions ?? (_actions = CreateActions());

        public static IMyTerminalControlButton Control => _control ?? (_control = CreateControl());

        private static void Action(IMyTerminalBlock block) {
            var logic = block.GameLogic?.GetAs<StatorGameLogic>();
            logic?.ReturnToHome();
        }

        private static IEnumerable<IMyTerminalAction> CreateActions() {
            var actions = new List<IMyTerminalAction> {
                Control.CreateButtonAction<IMyMotorBase>()
            };

            return actions;
        }

        private static IMyTerminalControlButton CreateControl() {
            var control = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyMotorBase>(ID);

            control.Title = ModText.BlockPropertyTitle_ReturnToHome;
            control.Tooltip = ModText.BlockPropertyTooltip_ReturnToHome;
            control.Action = Action;
            control.Enabled = Enabled;
            control.SupportsMultipleBlocks = true;

            return control;
        }

        private static bool Enabled(IMyTerminalBlock block) {
            var logic = block.GameLogic?.GetAs<StatorGameLogic>();

            var enabled = false;
            if (logic != null) {
                enabled = logic.CanReturnToHome;
            }

            return enabled;
        }
    }
}