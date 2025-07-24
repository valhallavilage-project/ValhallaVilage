using System.Collections.Generic;
using CrossProject.Core.Characters;
using CrossProject.Core.Skins;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CrossProject.Core.SaveLoad
{
    public static class JsonSerializerSettingsProvider
    {
        public static JsonSerializerSettings DefaultSettings => new ()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ContractResolver = CanWritePropertiesResolver.Instance,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,

            Converters = new List<JsonConverter>
            {
                new VersionConverter(),
                new CharacterIdConverter(),
                new SkinIdConverter(),
            }
        };
    }
}
