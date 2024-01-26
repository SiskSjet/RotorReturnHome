using Sandbox.ModAPI;
using System;
using VRage.Utils;
using VRageMath;

namespace Sisk.RotorReturnHome.Extensions {

    internal static class StatorExtension {

        public static void ReturnToHome(this IMyMotorStator stator, float homePosition, float maxVelocity = 5f) {
            var currentAngle = MathHelper.ToDegrees(stator.Angle) % 360;
            var upperLimit = stator.UpperLimitDeg;
            var lowerLimit = stator.LowerLimitDeg;

            var direction = 0;
            if (currentAngle < homePosition) {
                direction = 1;
                if (Math.Abs(currentAngle - (homePosition - 360)) < Math.Abs(currentAngle - homePosition)) {
                    direction = -1;
                    if (lowerLimit != float.MinValue) {
                        direction = -1;
                    }
                }
            } else if (currentAngle > homePosition) {
                direction = -1;
                if (Math.Abs(currentAngle - (homePosition + 360)) < Math.Abs(currentAngle - homePosition)) {
                    direction = 1;
                    if (upperLimit != float.MaxValue) {
                        direction = -1;
                    }
                }
            }

            if (direction == 0) {
                stator.TargetVelocityRPM = 0f;
            } else {
                var distance = Math.Abs(currentAngle - homePosition);

                if (distance > 10f) {
                    stator.TargetVelocityRPM = maxVelocity * direction;
                } else {
                    stator.TargetVelocityRPM = Math.Max(0.1f, maxVelocity * (distance / 10f)) * direction;
                }

                MyLog.Default.WriteLineAndConsole($"currentAngle: {currentAngle} homePosition: {homePosition} lowerLimit: {lowerLimit} upperLimit: {upperLimit} direction: {direction} distance: {distance} TargetVelocityRPM: {stator.TargetVelocityRPM}");
            }
        }
    }
}