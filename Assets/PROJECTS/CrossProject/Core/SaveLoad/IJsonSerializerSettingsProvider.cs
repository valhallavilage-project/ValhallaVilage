using Newtonsoft.Json;

namespace CrossProject.Core.SaveLoad
{
    public interface IJsonSerializerSettingsProvider
    {
        JsonSerializerSettings Settings { get; }
    }
}
