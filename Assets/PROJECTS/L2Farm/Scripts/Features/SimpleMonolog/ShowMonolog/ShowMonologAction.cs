using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using CrossProject.Core.Quests;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace L2Farm.Features.SimpleMonolog
{
    public class ShowMonologAction : Action<ShowMonologActionConfig>
    {
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly QuestService _questService;
        private readonly IResourceConditionService _resourceConditionService;

        private SimpleMonologPopup _view;

        public ShowMonologAction(
            UiService uiService,
            CharactersService charactersService,
            QuestService questService,
            IResourceConditionService resourceConditionService)
        {
            _uiService = uiService;
            _charactersService = charactersService;
            _questService = questService;
            _resourceConditionService = resourceConditionService;
        }

        public override async UniTask Execute()
        {
            var characterConfig = _charactersService.GetConfigFor(config.speaker);

            if (characterConfig == null)
            {
                Debug.LogError($"[{nameof(ShowMonologAction)}] : no character for : {config.speaker}");

                return;
            }

            var currentQuestStep = _questService.GetCurrentStepFor(config.questId);

            var data = _resourceConditionService.ProcessConditionResources(config.questId, currentQuestStep);

            var canProceed = _questService.CanProceed(config.questId);
            
            var model = new SimpleMonologPopupModel
            {
                portrait = characterConfig.portrait,
                personName = characterConfig.personName,
                message = config.message,
                resourcesData = data,
                close = () => _uiService.Close(_view),
                next = () =>
                {
                    _uiService.Close(_view);
                    if (canProceed)
                        _questService.TryProceedStepsOf(config.questId).Forget();
                }
            };
            _view = await _uiService.TryOpen(model) as SimpleMonologPopup;
        }
    }
}
