using Sandbox.ModAPI.Interfaces.Terminal;
using Sandbox.ModAPI;
using System.Collections.Generic;

namespace Sisk.RotorReturnHome.Controls {

    internal static class StatorControls {
        public static bool IsDone { get; private set; }

        public static void DoOnce() {
            if (IsDone) {
                return;
            }

            IsDone = true;

            CreateControls();
            CreateActions();
            CreateProperties();
        }

        private static void CreateActions() {
            var actions = new List<IMyTerminalAction>();
            actions.AddRange(ReturnToHomeButton.Actions);
            actions.AddRange(HomePositionAngleSlider.Actions);
            actions.AddRange(DisableRotorWhenHomeAngleReachedCheckbox.Actions);

            foreach (var action in actions) {
                MyAPIGateway.TerminalControls.AddAction<IMyMotorBase>(action);
            }
        }

        private static void CreateControls() {
            MyAPIGateway.TerminalControls.AddControl<IMyMotorBase>(ReturnToHomeButton.Control);
            MyAPIGateway.TerminalControls.AddControl<IMyMotorBase>(HomePositionAngleSlider.Control);
            MyAPIGateway.TerminalControls.AddControl<IMyMotorBase>(DisableRotorWhenHomeAngleReachedCheckbox.Control);
        }

        private static void CreateProperties() {
            MyAPIGateway.TerminalControls.AddControl<IMyMotorBase>(HomePositionAngleSlider.Property);
            MyAPIGateway.TerminalControls.AddControl<IMyMotorBase>(DisableRotorWhenHomeAngleReachedCheckbox.Property);
            MyAPIGateway.TerminalControls.AddControl<IMyMotorBase>(IsReturningToHomeProperty.Property);
        }
    }
}