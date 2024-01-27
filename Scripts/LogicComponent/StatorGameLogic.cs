using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using Sisk.RotorReturnHome.Controls;
using Sisk.RotorReturnHome.Extensions;
using System;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sisk.RotorReturnHome.LogicComponent {

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorAdvancedStator), false)]
    internal class MotorAdvancedStatorGameLogic : StatorGameLogic { }

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MotorStator), false)]
    internal class MotorStatorGameLogic : StatorGameLogic { }

    internal class StatorGameLogic : MyGameLogicComponent {
        private Settings _settings;

        public IMyMotorStator Block { get; private set; }

        /// <summary>
        /// Indicates if smart rotor is able to rotate to home position.
        /// </summary>
        public bool CanReturnToHome => Block.Enabled && !Block.RotorLock && Block.IsAttached;

        /// <summary>
        /// Indicates if smart rotor should be disabled when home position is reached.
        /// </summary>
        public bool DisableRotorWhenHomePositionReached {
            get { return _settings.DisableRotorWhenHomePositionReached; }
            set {
                if (value != _settings.DisableRotorWhenHomePositionReached) {
                    _settings.DisableRotorWhenHomePositionReached = value;
                    Save();
                }
            }
        }

        /// <summary>
        /// Indicates the angle of the home position.
        /// </summary>
        public float HomePositionAngle {
            get { return _settings.HomePositionAngle; }
            set {
                if (value != _settings.HomePositionAngle) {
                    _settings.HomePositionAngle = value;
                    Save();
                }
            }
        }

        /// <summary>
        /// Indicates if smart rotor is returning to home position.
        /// </summary>
        private bool IsReturningToHome {
            get { return _settings.IsReturningToHome; }
            set {
                if (value != _settings.IsReturningToHome) {
                    _settings.IsReturningToHome = value;
                    Save();
                }
            }
        }

        public override void Init(MyObjectBuilder_EntityBase objectBuilder) {
            base.Init(objectBuilder);

            Block = Entity as IMyMotorStator;

            try {
                _settings = Block.Load<Settings>(new Guid(Settings.GUID));
                if (_settings != null) {
                    if (_settings.Version < Settings.VERSION) {
                        // todo: merge old and new settings in future versions.
                    }
                } else {
                    _settings = new Settings();
                }
            } catch (Exception exception) {
                _settings = new Settings();
            }

            NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        /// <summary>
        /// Rotate this rotor to it's home position.
        /// </summary>
        public void ReturnToHome() {
            if (!IsReturningToHome && CanReturnToHome) {
                IsReturningToHome = true;
                NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;

                ClampHomePositionAngleToLimits();
                Block.ReturnToHome(HomePositionAngle, 3);
            }
        }

        /// <inheritdoc />
        public override void UpdateBeforeSimulation() {
            if (IsReturningToHome) {
                ClampHomePositionAngleToLimits();

                if (Math.Abs(MathHelper.ToDegrees(Block.Angle) - HomePositionAngle) < 0.01f) {
                    Block.TargetVelocityRPM = 0;
                    if (DisableRotorWhenHomePositionReached) {
                        Block.Enabled = false;
                    }

                    IsReturningToHome = false;
                    NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
                } else {
                    Block.ReturnToHome(HomePositionAngle, 3f);
                }
            }
        }

        public override void UpdateOnceBeforeFrame() {
            base.UpdateOnceBeforeFrame();

            StatorControls.DoOnce();

            if (Block?.CubeGrid?.Physics == null) {
                return;
            }

            if (IsReturningToHome) {
                NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
            }
        }

        private void ClampHomePositionAngleToLimits() {
            if (_settings.HomePositionAngle < Block.LowerLimitDeg) {
                _settings.HomePositionAngle = Block.LowerLimitDeg;
            }

            if (_settings.HomePositionAngle > Block.UpperLimitDeg) {
                _settings.HomePositionAngle = Block.UpperLimitDeg;
            }
        }

        private void Save() {
            try {
                Block.Save(new Guid(Settings.GUID), _settings);
            } catch (Exception exception) { }
        }
    }
}