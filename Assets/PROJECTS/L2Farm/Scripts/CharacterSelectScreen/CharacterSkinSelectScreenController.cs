using System.Linq;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.Skins;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace PROJECTS.L2Farm.Scripts.CharacterSkinSelect
{
    public class CharacterSkinSelectScreenController: IAsyncStartable
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly GameStateManager _gameStateManager;
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly SkinService _skinService;

        private CharacterSetConfig _characterSetConfig;
        private CharacterSelectScreen _view;

        public CharacterSkinSelectScreenController(
            AddressablesManager addressablesManager,
            GameStateManager gameStateManager,
            UiService uiService,
            CharactersService charactersService,
            SkinService skinService)
        {
            _addressablesManager = addressablesManager;
            _gameStateManager = gameStateManager;
            _uiService = uiService;
            _charactersService = charactersService;
            _skinService = skinService;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            _characterSetConfig = await _addressablesManager.LoadAssetAsync<CharacterSetConfig>();

            bool hasState = _gameStateManager.State.TryGet<ObtainedCharactersPart>(out var obtainedCharactersPart) && obtainedCharactersPart.ObtainedCharacters.Count > 0;
            if (hasState)
            {
                var currentCharacter = obtainedCharactersPart.CurrentCharacterId;
                _charactersService.Select(currentCharacter);

                _gameStateManager.State.TryGet<ObtainedSkinsPart>(out var obtainedSkinsPart);
                var currentSkin = obtainedSkinsPart.obtainedSkins[currentCharacter].currentSkinId;
                _skinService.Select(currentSkin);
                return;
            }

            _view = await _uiService.TryOpen(GetModel()) as CharacterSelectScreen;
        }

        private CharacterSelectScreenModel GetModel()
        {
            var result = new CharacterSelectScreenModel
            {
                OnCharacterSelected = OnCharacterSelected,
                Close = OnClose
            };

            return result;
        }

        private void OnCharacterSelected(CharacterId characterId)
        {
            _charactersService.Obtain(characterId);
            var defaultSkinId = _characterSetConfig.items.First(x => x.id == characterId).defaultSkinId;
            _skinService.Select(defaultSkinId);
        }

        private void OnClose()
        {
            _uiService.Close(_view);
        }
    }
}