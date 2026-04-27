using CrossProject.Core;
using CrossProject.Core.Characters;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.Skins;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
            Debug.Log("[SkipFlow] CharacterSkinSelectScreenController.Initialize: waiting for IsLoggedIn");
            await UniTask.WaitUntil(() => _uiService.IsLoggedIn);
            Debug.Log("[SkipFlow] CharacterSkinSelectScreenController: IsLoggedIn=true, checking save state");

            bool hasState = _gameStateManager.State.TryGet<ObtainedCharactersPart>(out var obtainedCharactersPart) && obtainedCharactersPart.ObtainedCharacters.Count > 0;
            Debug.Log($"[SkipFlow] CharacterSkinSelectScreenController: hasState={hasState}");
            if (hasState)
            {
                var currentCharacter = obtainedCharactersPart.CurrentCharacterId;
                _charactersService.Select(currentCharacter);

                _gameStateManager.State.TryGet<ObtainedSkinsPart>(out var obtainedSkinsPart);
                var currentSkin = obtainedSkinsPart.obtainedSkins[currentCharacter].currentSkinId;
                Debug.Log($"[SkipFlow] CharacterSkinSelectScreenController: selecting skin {currentSkin}");
                await _skinService.Select(currentSkin);
                Debug.Log("[SkipFlow] CharacterSkinSelectScreenController: skin selected");

                _gameStateManager.State.TryGet<WornArmorSet>(out var wornArmorSetPart);

                if (wornArmorSetPart != null)
                {
                    _mainCharacterGlobalArmorSetChangeHandler.ChangeSet(wornArmorSetPart.ArmorSet);
                }

                IsInitialized = true;
                Debug.Log("[SkipFlow] CharacterSkinSelectScreenController.Initialize: done (hasState path)");
                return;
            }

            Debug.Log("[SkipFlow] CharacterSkinSelectScreenController: opening CharacterSelectScreen via TryOpen");
            var tcs = new UniTaskCompletionSource();
            var model = GetModel();
            var originalClose = model.Close;
            model.Close = () =>
            {
                originalClose?.Invoke();
                tcs.TrySetResult();
            };

            _view = await _uiService.TryOpen(model) as CharacterSelectScreen;
            Debug.Log($"[SkipFlow] CharacterSkinSelectScreenController: TryOpen returned view={(_view != null ? "ok" : "null")}");
            await tcs.Task;
            IsInitialized = true;
            Debug.Log("[SkipFlow] CharacterSkinSelectScreenController.Initialize: done (select path)");
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