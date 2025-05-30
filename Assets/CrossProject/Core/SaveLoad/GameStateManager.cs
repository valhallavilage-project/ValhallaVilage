using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sabresaurus.PlayerPrefsUtilities;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Converters;

namespace CrossProject.Core.SaveLoad
{
    public class GameStateManager : MonoSingleton<GameStateManager>
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
                return false;

            _gameState = _serializer.Deserialize<GameState>(json);
            return true;
        }

        public GameState Get()
        {
            if (_gameState == null)
                if (!TryLoadSavedData())
                    CreateEmptyState();

            return _gameState;
        }

        private async UniTask SaveTask()
        {
            if (_gameState == null)
                return;

            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            PlayerPrefsUtility.SetEncryptedString(GameStatePrefsKey, _serializer.Serialize(_gameState));
        }

        public void Save()
        {
            if (_saveTask.Status != UniTaskStatus.Pending)
                _saveTask = SaveTask();
        }
    }
}