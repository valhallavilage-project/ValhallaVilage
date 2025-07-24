using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

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

    public class CharacterIdConverter : JsonConverter<CharacterId>
    {
        public override void WriteJson(JsonWriter writer, CharacterId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override CharacterId ReadJson(JsonReader reader, Type objectType, CharacterId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (CharacterId)(string)reader.Value;
    }
}