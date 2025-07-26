using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace CrossProject.Core.PROJECTS.CrossProject.Core.InGameResources
{
    [Serializable]
    public class ResourceId : EntityId<string>, IEquatable<ResourceId>
    {
        public ResourceId(string value) : base(value) {}

        public bool Equals(ResourceId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is ResourceId id && Equals(id);

        public static explicit operator ResourceId(string value) => new (value);

        public static bool operator ==(ResourceId a, ResourceId b) => a?.Value == b?.Value;

        public static bool operator !=(ResourceId a, ResourceId b) => !(a == b);
    }

    public class ResourceIdConverter : JsonConverter<ResourceId>
    {
        public override void WriteJson(JsonWriter writer, ResourceId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override ResourceId ReadJson(JsonReader reader, Type objectType, ResourceId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (ResourceId)(string)reader.Value;
    }
}
