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

        public event Action<CharacterId> OnCharacterObtained;
        public event Action<CharacterId> OnCharacterSelected;

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
            OnCharacterObtained?.Invoke(characterId);
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

        public bool IsObtained(CharacterId characterId)
        {
            return _gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part) && part.ObtainedCharacters.Contains(characterId);
        }

        public CharacterConfig GetConfigFor(CharacterId characterId)
        {
            return _characterSetConfig.items.FirstOrDefault(x => new CharacterId(x.id) == characterId);
        }

        //TODO : VM : remove
        //TODO : VM : trial characters
    }
}