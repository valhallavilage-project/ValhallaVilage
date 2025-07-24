using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

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

    public class SkinIdConverter : JsonConverter<SkinId>
    {
        public override void WriteJson(JsonWriter writer, SkinId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override SkinId ReadJson(JsonReader reader, Type objectType, SkinId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (SkinId)(string)reader.Value;
    }
}