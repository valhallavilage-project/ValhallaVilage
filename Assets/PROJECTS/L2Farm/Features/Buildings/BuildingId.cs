using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace L2Farm.Features.Buildings
{
    [Serializable]
    public class BuildingId : EntityId<string>, IEquatable<BuildingId>
    {
        public BuildingId(string value) : base(value) {}

        public bool Equals(BuildingId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is BuildingId id && Equals(id);

        public static implicit operator BuildingId(string value) => new (value);

        public static implicit operator string(BuildingId value) => value.ToString();

        public static bool operator ==(BuildingId a, BuildingId b) => a?.Value == b?.Value;

        public static bool operator !=(BuildingId a, BuildingId b) => !(a == b);
    }

    public class BuildingIdConverter : JsonConverter<BuildingId>
    {
        public override void WriteJson(JsonWriter writer, BuildingId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override BuildingId ReadJson(JsonReader reader, Type objectType, BuildingId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (BuildingId)(string)reader.Value;
    }
}
