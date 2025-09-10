using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace CrossProject.Core.Quests
{
    [Serializable]
    public class IndicationTypeId : EntityId<string>, IEquatable<IndicationTypeId>
    {
        public override int GetHashCode() => Value.GetHashCode();

        public IndicationTypeId(string value) : base(value) {}

        public bool Equals(IndicationTypeId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is IndicationTypeId id && Equals(id);

        public static implicit operator IndicationTypeId(string value) => new (value);

        public static implicit operator string(IndicationTypeId value) => value.ToString();

        public static bool operator ==(IndicationTypeId a, IndicationTypeId b) => a?.Value == b?.Value;

        public static bool operator !=(IndicationTypeId a, IndicationTypeId b) => !(a == b);
    }

    public class IndicationTypeIdConverter : JsonConverter<IndicationTypeId>
    {
        public override void WriteJson(JsonWriter writer, IndicationTypeId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override IndicationTypeId ReadJson(JsonReader reader, Type objectType, IndicationTypeId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (string)reader.Value;
    }
}
