using System.Collections.Generic;
using CrossProject.Core.Characters;
using CrossProject.Core.Skins;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sabresaurus.PlayerPrefsUtilities;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.SaveLoad
{
    public class GameStateManager : IInitializable
    {
        private const string GameStatePrefsKey = nameof(GameStatePrefsKey);

        private readonly JsonSerializer _serializer = new ();

        private GameState _gameState;
        private UniTask _saveTask;

        public GameState State
        {
            get
            {
                if (_gameState == null)
                    LoadOrCreateSavedData();

                return _gameState;
            }
        }

        private void CreateEmptyState()
        {
            _gameState = new GameState();
        }

        private void LoadOrCreateSavedData()
        {
            var json = PlayerPrefsUtility.GetEncryptedString(GameStatePrefsKey, null);
            if (json == null)
                CreateEmptyState();
            else
                _gameState = _serializer.Deserialize<GameState>(json);
        }

        private async UniTask SaveTask()
        {
            if (_gameState == null)
            {
                CreateEmptyState();
            }

            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            var json = _serializer.Serialize(_gameState, Formatting.Indented);
            PlayerPrefsUtility.SetEncryptedString(GameStatePrefsKey, json);
            Debug.Log(json);
        }

        public void Save()
        {
            if (_saveTask.Status != UniTaskStatus.Pending)
                _saveTask = SaveTask();
        }

        public void Initialize()
        {
            LoadOrCreateSavedData();
        }
    }
}