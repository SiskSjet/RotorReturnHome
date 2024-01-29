using Sandbox.ModAPI.Interfaces.Terminal;
using Sandbox.ModAPI;
using Sisk.RotorReturnHome.LogicComponent;

namespace Sisk.RotorReturnHome.Controls {
    internal static class IsReturningToHomeProperty {
        private const string ID = "IsReturningToHome";
        private static IMyTerminalControlProperty<bool> _property;

        public static IMyTerminalControlProperty<bool> Property => _property ?? (_property = CreateProperty());

        private static IMyTerminalControlProperty<bool> CreateProperty() {
            var property = MyAPIGateway.TerminalControls.CreateProperty<bool, IMyMotorBase>(ID);
            property.Getter = Getter;
            property.Setter = Setter;
            property.SupportsMultipleBlocks = false;
            property.Enabled = Enabled;

            return property;
        }

        private static bool Enabled(IMyTerminalBlock block) {
            return true;
        }

        private static bool Getter(IMyTerminalBlock block) {
            var logic = block.GameLogic?.GetAs<StatorGameLogic>();

            if (logic != null) {
                return logic.IsReturningToHome;
            }

            return false;
        }

        private static void Setter(IMyTerminalBlock block, bool value) { }
    }
}