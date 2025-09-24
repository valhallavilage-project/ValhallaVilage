using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Scripts.CharacterHudElement
{
    public class CharacterHudElementController : IInitializable
    {
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly GameStateManager _gameStateManager;

        private CharacterHudElement _view;

        public bool IsInitialized { get; private set; }
        
        public CharacterHudElementController(
            UiService uiService,
            CharactersService charactersService,
            GameStateManager gameStateManager)
        {
            _uiService = uiService;
            _charactersService = charactersService;
            _gameStateManager = gameStateManager;
        }

        public async UniTask Initialize()
        {
            _view = await _uiService.TryOpen(new CharacterHudElementModel()) as CharacterHudElement;

            if (_gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part))
            {
                var config = _charactersService.GetConfigFor(part.CurrentCharacterId);
                _view.SetPortrait(config.portrait);
            }
            else
            {
                _charactersService.OnCharacterSelected += OnCharacterSelected;
            }
            
            IsInitialized = true;
        }

        private void OnCharacterSelected(CharacterId characterId)
        {
            var config = _charactersService.GetConfigFor(characterId);
            _view.SetPortrait(config.portrait);
        }
    }
}
