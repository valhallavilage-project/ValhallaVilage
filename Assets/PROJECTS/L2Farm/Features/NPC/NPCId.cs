using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace L2Farm.Features.NPC
{
    [Serializable]
    public class NPCId : EntityId<string>, IEquatable<NPCId>
    {
        public override int GetHashCode() => Value.GetHashCode();

        public NPCId(string value) : base(value) {}

        public bool Equals(NPCId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is NPCId id && Equals(id);

        public static implicit operator NPCId(string value) => new (value);

        public static bool operator ==(NPCId a, NPCId b) => a?.Value == b?.Value;

        public static bool operator !=(NPCId a, NPCId b) => !(a == b);
    }

    public class NPCIdConverter : JsonConverter<NPCId>
    {
        public override void WriteJson(JsonWriter writer, NPCId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override NPCId ReadJson(JsonReader reader, Type objectType, NPCId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (string)reader.Value;
    }
}
