using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace L2Farm.Features.Tools
{
    [Serializable]
    public class ToolId : EntityId<string>, IEquatable<ToolId>
    {
        public override int GetHashCode() => Value.GetHashCode();

        public ToolId(string value) : base(value) {}

        public bool Equals(ToolId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is ToolId id && Equals(id);

        public static implicit operator ToolId(string value) => new (value);

        public static implicit operator string(ToolId value) => value.ToString();

        public static bool operator ==(ToolId a, ToolId b) => a?.Value == b?.Value;

        public static bool operator !=(ToolId a, ToolId b) => !(a == b);
    }

    public class ToolIdConverter : JsonConverter<ToolId>
    {
        public override void WriteJson(JsonWriter writer, ToolId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override ToolId ReadJson(JsonReader reader, Type objectType, ToolId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (string)reader.Value;
    }
}
