using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace CrossProject.Core.SpawnPoints
{
    [Serializable]
    public class SpawnPointId : EntityId<string>
    {
        public override int GetHashCode() => Value.GetHashCode();

        public SpawnPointId(string value) : base(value) {}

        public bool Equals(SpawnPointId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is SpawnPointId id && Equals(id);

        public static implicit operator SpawnPointId(string value) => new (value);

        public static bool operator ==(SpawnPointId a, SpawnPointId b) => a?.Value == b?.Value;

        public static bool operator !=(SpawnPointId a, SpawnPointId b) => !(a == b);
    }

    public class SpawnPointIdConverter : JsonConverter<SpawnPointId>
    {
        public override void WriteJson(JsonWriter writer, SpawnPointId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override SpawnPointId ReadJson(JsonReader reader, Type objectType, SpawnPointId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (SpawnPointId)(string)reader.Value;
    }
}
