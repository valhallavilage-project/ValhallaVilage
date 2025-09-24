using System;
using CrossProject.Core.OdinEntities;
using Newtonsoft.Json;

namespace L2Farm.Features.ResourceProduction
{
    [Serializable]
    public class ProductionId : EntityId<string>, IEquatable<ProductionId>
    {
        public override int GetHashCode() => Value.GetHashCode();

        public ProductionId(string value) : base(value) {}

        public bool Equals(ProductionId other) => Value.Equals(other?.Value);

        public override bool Equals(object obj) => obj is ProductionId id && Equals(id);

        public static implicit operator ProductionId(string value) => new (value);

        public static implicit operator string(ProductionId value) => value.ToString();

        public static bool operator ==(ProductionId a, ProductionId b) => a?.Value == b?.Value;

        public static bool operator !=(ProductionId a, ProductionId b) => !(a == b);
    }

    public class ProductionIdConverter : JsonConverter<ProductionId>
    {
        public override void WriteJson(JsonWriter writer, ProductionId value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
        public override ProductionId ReadJson(JsonReader reader, Type objectType, ProductionId existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (string)reader.Value;
    }
}
