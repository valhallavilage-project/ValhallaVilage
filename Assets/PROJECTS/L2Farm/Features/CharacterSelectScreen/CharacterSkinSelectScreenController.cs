using System.Linq;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.Characters;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.Skins;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace PROJECTS.L2Farm.Scripts.CharacterSkinSelect
{
    public class CharacterSkinSelectScreenController: IInitializable
    {
        private readonly GameStateManager _gameStateManager;
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly SkinService _skinService;
        private readonly QuestService _questService;

        private CharacterSelectScreen _view;

        public CharacterSkinSelectScreenController(
            GameStateManager gameStateManager,
            UiService uiService,
            CharactersService charactersService,
            SkinService skinService,
            QuestService questService)
        {
            _gameStateManager = gameStateManager;
            _uiService = uiService;
            _charactersService = charactersService;
            _skinService = skinService;
            _questService = questService;
        }

        public async UniTask Initialize()
        {
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
            _charactersService.Select(characterId);
            _skinService.Select(_skinService.GetDefaultSkinFor(characterId));
            _questService.TryLaunch(new QuestId("HelloWorld"));
        }

        private void OnClose()
        {
            _uiService.Close(_view);
        }
    }
}