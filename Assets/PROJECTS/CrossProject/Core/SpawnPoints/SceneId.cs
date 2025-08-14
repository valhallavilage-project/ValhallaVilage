using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace CrossProject.Core.SpawnPoints
{
    [Serializable]
    public class SceneId : EntityId<string>
    {
        public SceneId(string value) : base(value) {}

        public bool Equals(SceneId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is SceneId id && Equals(id);

        public static explicit operator SceneId(string value) => new (value);

        public static bool operator ==(SceneId a, SceneId b) => a?.Value == b?.Value;

        public static bool operator !=(SceneId a, SceneId b) => !(a == b);
    }

    public class SceneIdConverter : JsonConverter<SceneId>
    {
        public override void WriteJson(JsonWriter writer, SceneId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override SceneId ReadJson(JsonReader reader, Type objectType, SceneId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (SceneId)(string)reader.Value;
    }
}
