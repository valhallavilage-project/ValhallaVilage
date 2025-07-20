using System;
using CrossProject.Core.OdinEntities;

namespace CrossProject.Core.Characters
{
    [Serializable]
    public class CharacterId : EntityId<string>, IEquatable<CharacterId>
    {
        public CharacterId(string value) : base(value) {}

        public bool Equals(CharacterId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is CharacterId characterId && Equals(characterId);

        public static explicit operator CharacterId(string value) => new (value);

        public static bool operator ==(CharacterId a, CharacterId b) => a?.Value == b?.Value;

        public static bool operator !=(CharacterId a, CharacterId b) => !(a == b);
    }
}