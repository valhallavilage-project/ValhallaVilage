using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace CrossProject.Core.SaveLoad
{
    public class JsonSerializer
    {
        private readonly JsonSerializerSettings serializerSettings;

        public JsonSerializer(IJsonSerializerSettingsProvider provider)
        {
            serializerSettings = provider.Settings;
        }

        public T Deserialize<T>(string serializedData)
        {
            return (T)Deserialize(serializedData, typeof(T));
        }

        public object Deserialize(string serializedData, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(serializedData, type, serializerSettings);
            }
            catch (JsonSerializationException ex)
            {
                string message = $"An error has occurred while deserializing data. Error message: {ex.Message}, Type: {type}, Data: {serializedData}";
                Debug.LogError(message);

                throw new SerializationException(message, ex);
            }
            catch (JsonReaderException ex)
            {
                string message = $"An error has occurred while deserializing data. Error message: {ex.Message}, Type: {type}, Data: {serializedData}";
                Debug.LogError(message);

                throw new SerializationException(message, ex);
            }
        }

        public string Serialize(object obj, Formatting format)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, format, serializerSettings);
            }
            catch (JsonSerializationException ex)
            {
                string message = $"An error has occurred while serializing object. Error message: {ex.Message}, Type: {obj.GetType()}, Object: {obj}";
                Debug.LogError(message);

                throw new SerializationException(message, ex);
            }
            catch (JsonWriterException ex)
            {
                string message = $"An error has occurred while serializing object. Error message: {ex.Message}, Type: {obj.GetType()}, Object: {obj}";
                Debug.LogError(message);

                throw new SerializationException(message, ex);
            }
        }

        public string Serialize(object obj) => Serialize(obj, Formatting.None);
    }
}