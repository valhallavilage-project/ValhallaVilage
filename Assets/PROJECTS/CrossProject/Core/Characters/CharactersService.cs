using System;
using System.Linq;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.Skins;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.Characters
{
    [DefaultExecutionOrder(-998)]
    public class CharactersService : IInitializable
    {
        private readonly GameStateManager _gameStateManager;
        private readonly AddressablesManager _addressablesManager;
        private readonly SkinService _skinService;

        private CharacterSetConfig _characterSetConfig;

        public event Action<CharacterId> OnCharacterSelected;

        public bool IsInitialized { get; private set; }

        public CharactersService(
            GameStateManager gameStateManager,
            AddressablesManager addressablesManager,
            SkinService skinService)
        {
            _gameStateManager = gameStateManager;
            _addressablesManager = addressablesManager;
            _skinService = skinService;
        }

        public async UniTask Initialize()
        {
            _characterSetConfig = await _addressablesManager.LoadAssetAsync<CharacterSetConfig>();
            IsInitialized = true;
        }

        public void Obtain(CharacterId characterId)
        {
            if (!_gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part))
                part = _gameStateManager.State.Set(new ObtainedCharactersPart());

            if (part.ObtainedCharacters.Contains(characterId))
                return;

            if (part.ObtainedCharacters.Count == 0)
                part.CurrentCharacterId = characterId;
            part.ObtainedCharacters.Add(characterId);
            _gameStateManager.State.Set(part);
            _skinService.Obtain(_skinService.GetDefaultSkinFor(characterId));
            _gameStateManager.Save();
        }

        public bool Select(CharacterId characterId)
        {
            if (!_gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part))
                return false;

            if (!part.ObtainedCharacters.Contains(characterId))
                return false;

            part.CurrentCharacterId = characterId;
            OnCharacterSelected?.Invoke(characterId);
            return true;
        }

        public CharacterConfig GetConfigFor(CharacterId characterId)
        {
            if (characterId == "MC")
            {
                var id = _gameStateManager.State.Get<ObtainedCharactersPart>().ObtainedCharacters.First();
                return _characterSetConfig.items.FirstOrDefault(x => x.id == id);
            }

            return _characterSetConfig.items.FirstOrDefault(x => x.id == characterId);
        }
    }
}