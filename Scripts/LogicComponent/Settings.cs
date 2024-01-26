using ProtoBuf;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Sisk.RotorReturnHome.LogicComponent {

    [ProtoContract]
    internal class Settings {
        public const string GUID = "D8681F13-9F7B-4583-9B3C-062B1BCDD287";
        public const int VERSION = 1;
        private const bool DISABLE_ROTOR_WHEN_HOME_POSITION_REACHED = true;
        private const float HOME_POSITION_ANGLE = 0f;
        private const bool IS_RETURNING_TO_HOME = false;

        [ProtoMember(4)]
        [XmlElement(Order = 4)]
        [DefaultValue(DISABLE_ROTOR_WHEN_HOME_POSITION_REACHED)]
        public bool DisableRotorWhenHomePositionReached { get; set; } = DISABLE_ROTOR_WHEN_HOME_POSITION_REACHED;

        [ProtoMember(2)]
        [XmlElement(Order = 2)]
        [DefaultValue(HOME_POSITION_ANGLE)]
        public float HomePositionAngle { get; set; } = HOME_POSITION_ANGLE;

        [ProtoMember(3)]
        [XmlElement(Order = 3)]
        [DefaultValue(IS_RETURNING_TO_HOME)]
        public bool IsReturningToHome { get; set; } = IS_RETURNING_TO_HOME;

        [ProtoMember(1)]
        [XmlElement(Order = 1)]
        public int Version { get; set; } = VERSION;
    }
}