using System;
using System.Linq;
using System.Threading;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.Skins;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Core.Characters
{
    public class CharactersService : IAsyncStartable
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

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            _characterSetConfig = await _addressablesManager.LoadAssetAsync<CharacterSetConfig>();
        }

        public void Obtain(CharacterId characterId)
        {
            if (!_gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part))
                return;

            if (part.obtainedCharacters.Contains(characterId))
                return;

            part.obtainedCharacters.Add(characterId);
            var defaultSkinId = _characterSetConfig.items.First(x => x.id == characterId).defaultSkinId;
            _skinService.Obtain(defaultSkinId);
            OnCharacterObtained?.Invoke(characterId);
        }

        public bool Select(CharacterId characterId)
        {
            if (!_gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part))
                return false;

            if (!part.obtainedCharacters.Contains(characterId))
                return false;

            part.currentCharacterId = characterId;
            OnCharacterSelected?.Invoke(characterId);
            return true;
        }

        public bool IsObtained(CharacterId characterId)
        {
            return _gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part) && part.obtainedCharacters.Contains(characterId);
        }

        //TODO : VM : remove
        //TODO : VM : trial characters
    }
}