using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sabresaurus.PlayerPrefsUtilities;
using UnityEngine;

namespace CrossProject.Core.SaveLoad
{
    public class GameStateManager
    {
        private const string GameStatePrefsKey = nameof(GameStatePrefsKey);

        private readonly JsonSerializer _serializer = new (new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ContractResolver = CanWritePropertiesResolver.Instance,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,

            Converters = new List<JsonConverter>
            {
                 new VersionConverter(),
            }
        });

        private GameState _gameState;
        private UniTask _saveTask;

        private void CreateEmptyState()
        {
            _gameState = new GameState();
        }

        private bool TryLoadSavedData()
        {
            var json = PlayerPrefsUtility.GetEncryptedString(GameStatePrefsKey, null);
            if (json == null)
            {
                CreateEmptyState();
                return false;
            }

            _gameState = _serializer.Deserialize<GameState>(json);
            return true;
        }

        private async UniTask SaveTask()
        {
            if (_gameState == null)
            {
                CreateEmptyState();
            }

            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            var json = _serializer.Serialize(_gameState);
            PlayerPrefsUtility.SetEncryptedString(GameStatePrefsKey, json);
            Debug.Log(json);
        }

        public GameState Get()
        {
            if (_gameState == null && !TryLoadSavedData())
                CreateEmptyState();

            return _gameState;
        }

        public void Save()
        {
            if (_saveTask.Status != UniTaskStatus.Pending)
                _saveTask = SaveTask();
        }
    }
}