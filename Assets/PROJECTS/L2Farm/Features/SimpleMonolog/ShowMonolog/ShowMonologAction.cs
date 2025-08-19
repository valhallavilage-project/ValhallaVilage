using System.Collections.Generic;
using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using UnityEngine;

namespace L2Farm.Features.SimpleMonolog
{
    public class ShowMonologAction : Action<ShowMonologActionConfig>
    {
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly QuestService _questService;
        private readonly ActionService _actionService;
        private readonly ResourcesService _resourcesService;
        private readonly GameStateManager _gameStateManager;

        private SimpleMonologPopup _view;

        public ShowMonologAction(
            UiService uiService,
            CharactersService charactersService,
            QuestService questService,
            ActionService actionService,
            ResourcesService resourcesService,
            GameStateManager gameStateManager)
        {
            _uiService = uiService;
            _charactersService = charactersService;
            _questService = questService;
            _actionService = actionService;
            _resourcesService = resourcesService;
            _gameStateManager = gameStateManager;
        }

        public override async void Execute()
        {
            var characterConfig = _charactersService.GetConfigFor(config.speaker);
            var data = new List<ResourceRequirementData>();
            foreach (var resourceCondition in config.resources.resourceConditions)
            {
                data.Add(new ResourceRequirementData
                {
                    icon = _resourcesService.GetSprite(resourceCondition.resourceId),
                    has = _gameStateManager.State.Get<ResourceContentPart>().Has(resourceCondition.resourceId),
                    need = resourceCondition.neededQuantity
                });
            }
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
                    _questService.TryProceed(config.questId);
                }
            };
            _view = await _uiService.TryOpen(model) as SimpleMonologPopup;
        }
    }
}
