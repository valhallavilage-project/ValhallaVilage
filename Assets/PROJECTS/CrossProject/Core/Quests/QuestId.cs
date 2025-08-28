using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace CrossProject.Core.Quests
{
    [Serializable]
    public class QuestId : EntityId<string>
    {
        public QuestId(string value) : base(value) {}

        public bool Equals(QuestId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is QuestId id && Equals(id);

        public static implicit operator QuestId(string value) => new (value);

        public static implicit operator string(QuestId value) => value.ToString();

        public static bool operator ==(QuestId a, QuestId b) => a?.Value == b?.Value;

        public static bool operator !=(QuestId a, QuestId b) => !(a == b);
    }

    public class QuestIdConverter : JsonConverter<QuestId>
    {
        public override void WriteJson(JsonWriter writer, QuestId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override QuestId ReadJson(JsonReader reader, Type objectType, QuestId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (QuestId)(string)reader.Value;
    }
}
