using Sandbox.ModAPI.Interfaces.Terminal;
using Sandbox.ModAPI;
using Sisk.RotorReturnHome.Localization;
using System.Collections.Generic;
using System.Text;
using Sisk.RotorReturnHome.Extensions;
using Sisk.RotorReturnHome.LogicComponent;

namespace Sisk.RotorReturnHome.Controls {

    internal static class HomePositionAngleSlider {
        private const string ID = "HomePositionAngle";

        private static IEnumerable<IMyTerminalAction> _actions;
        private static IMyTerminalControlSlider _control;
        private static IMyTerminalControlProperty<float> _property;

        public static IEnumerable<IMyTerminalAction> Actions => _actions ?? (_actions = CreateActions());

        public static IMyTerminalControlSlider Control => _control ?? (_control = CreateControl());

        public static IMyTerminalControlProperty<float> Property => _property ?? (_property = CreateProperty());

        private static IEnumerable<IMyTerminalAction> CreateActions() {
            var actions = new List<IMyTerminalAction> {
                Control.CreateResetAction<IMyMotorBase>(DefaultValue),
                Control.CreateIncreaseAction<IMyMotorBase>(1f, MinGetter, MaxGetter),
                Control.CreateDecreaseAction<IMyMotorBase>(1f, MinGetter, MaxGetter)
            };

            return actions;
        }

        private static IMyTerminalControlSlider CreateControl() {
            var control = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, IMyMotorBase>(ID);
            control.Title = ModText.BlockPropertyTitle_HomePositionAngle;
            control.Tooltip = ModText.BlockPropertyTooltip_HomePositionAngle;
            control.Writer = Writer;
            control.Getter = Getter;
            control.Setter = Setter;
            control.SetLimits(MinGetter, MaxGetter);
            control.SupportsMultipleBlocks = true;
            control.Enabled = Enabled;

            return control;
        }

        private static IMyTerminalControlProperty<float> CreateProperty() {
            return Control.CreateProperty<IMyMotorBase>();
        }

        private static float DefaultValue(IMyTerminalBlock block) {
            return MinGetter(block);
        }

        private static bool Enabled(IMyTerminalBlock block) {
            return true;
        }

        private static float Getter(IMyTerminalBlock block) {
            var logic = block.GameLogic?.GetAs<StatorGameLogic>();

            if (logic != null) {
                return logic.HomePositionAngle;
            }

            return 0;
        }

        private static float MaxGetter(IMyTerminalBlock block) {
            var stator = block as IMyMotorStator;

            var upperLimit = stator.UpperLimitDeg;

            return upperLimit == float.MaxValue ? 360 : upperLimit;
        }

        private static float MinGetter(IMyTerminalBlock block) {
            var stator = block as IMyMotorAdvancedStator;

            var lowerLimit = stator.LowerLimitDeg;
            var upperLimit = stator.UpperLimitDeg;
            if (upperLimit <= 90f && lowerLimit >= -90f) {
                return lowerLimit;
            }

            return lowerLimit == float.MinValue ? 0 : lowerLimit + 360;
        }

        private static void Setter(IMyTerminalBlock block, float value) {
            var logic = block.GameLogic?.GetAs<StatorGameLogic>();

            if (logic != null) {
                logic.HomePositionAngle = value;
            }
        }

        private static void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Append($"{Getter(block):F2}°");
        }
    }
}