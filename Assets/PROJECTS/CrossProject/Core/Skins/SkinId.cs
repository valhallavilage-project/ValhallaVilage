using System;
using CrossProject.Core.OdinEntities;

namespace CrossProject.Core.Skins
{
    [Serializable]
    public class SkinId : EntityId<string>, IEquatable<SkinId>
    {
        public SkinId(string value) : base(value) {}

        public bool Equals(SkinId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is SkinId skinId && Equals(skinId);

        public static explicit operator SkinId(string value) => new (value);

        public static bool operator ==(SkinId a, SkinId b) => a?.Value == b?.Value;

        public static bool operator !=(SkinId a, SkinId b) => !(a == b);
    }
}