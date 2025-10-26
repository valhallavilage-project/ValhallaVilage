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
        private readonly IMainCharacterGlobalArmorSetChangeHandler _mainCharacterGlobalArmorSetChangeHandler;

        private CharacterSelectScreen _view;
        private CharacterId _selectedCharacterId;

        public bool IsInitialized { get; private set; }

        public CharacterSkinSelectScreenController(
            GameStateManager gameStateManager,
            UiService uiService,
            CharactersService charactersService,
            SkinService skinService,
            QuestService questService,
            IMainCharacterGlobalArmorSetChangeHandler mainCharacterGlobalArmorSetChangeHandler)
        {
            _gameStateManager = gameStateManager;
            _uiService = uiService;
            _charactersService = charactersService;
            _skinService = skinService;
            _questService = questService;
            _mainCharacterGlobalArmorSetChangeHandler = mainCharacterGlobalArmorSetChangeHandler;
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
                await _skinService.Select(currentSkin);
                
                _gameStateManager.State.TryGet<WornArmorSet>(out var wornArmorSetPart);

                if (wornArmorSetPart != null)
                {
                    _mainCharacterGlobalArmorSetChangeHandler.ChangeSet(wornArmorSetPart.ArmorSet);
                }
                
                IsInitialized = true;
                return;
            }

            _view = await _uiService.TryOpen(GetModel()) as CharacterSelectScreen;
            IsInitialized = true;
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
            _selectedCharacterId = characterId;
        }

        private void OnClose()
        {
            _charactersService.Obtain(_selectedCharacterId);
            _charactersService.Select(_selectedCharacterId);
            _skinService.Select(_skinService.GetDefaultSkinFor(_selectedCharacterId));
            _uiService.Close(_view);
            _questService.TryLaunch(new QuestId("HelloWorld")).Forget();
        }
    }
}