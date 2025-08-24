using System.Collections.Generic;
using CrossProject.Core.Characters;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.Skins;
using CrossProject.Core.SpawnPoints;
using L2Farm.Features.Buildings;
using L2Farm.Features.NPC;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace L2Farm.Scripts
{
    public class L2FarmJsonSerializerSettingsProvider : IJsonSerializerSettingsProvider
    {
        public JsonSerializerSettings Settings => new()
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
                new ResourceIdConverter(),
                new SceneIdConverter(),
                new SpawnPointIdConverter(),
                new QuestIdConverter(),
                new BuildingIdConverter(),
                new NPCIdConverter(),
            }
        };
    }
}
