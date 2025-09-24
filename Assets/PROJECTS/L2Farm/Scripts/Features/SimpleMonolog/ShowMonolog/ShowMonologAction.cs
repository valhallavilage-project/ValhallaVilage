using System.Collections.Generic;
using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using L2Farm.Features.ResourceProduction.GiveResources;
using L2Farm.Scripts;
using L2Farm.Scripts.Conditions;
using UnityEngine;

namespace L2Farm.Features.SimpleMonolog
{
    public class ShowMonologAction : Action<ShowMonologActionConfig>
    {
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly QuestService _questService;
        private readonly ResourcesService _resourcesService;
        private readonly GameStateManager _gameStateManager;

        private SimpleMonologPopup _view;

        public ShowMonologAction(
            UiService uiService,
            CharactersService charactersService,
            QuestService questService,
            ResourcesService resourcesService,
            GameStateManager gameStateManager)
        {
            _uiService = uiService;
            _charactersService = charactersService;
            _questService = questService;
            _resourcesService = resourcesService;
            _gameStateManager = gameStateManager;
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

            var data = ProcessConditionResources(currentQuestStep);

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

        private List<MonologResourceData> ProcessConditionResources(int currentQuestStep)
        {
            var data = new List<MonologResourceData>();

            var winCondition = _questService.GetConfigFor(config.questId).steps[currentQuestStep].winCondition;
            
            switch (winCondition)
            {
                case HasEnoughResourcesConditionConfig hasEnoughResourcesConditionConfig:
                {
                    foreach (var resourceCondition in hasEnoughResourcesConditionConfig.ResourceConditions)
                    {
                        data.Add(new MonologResourceData
                        {
                            Icon = _resourcesService.GetSprite(resourceCondition.Id),
                            MainCharacterAmount = _gameStateManager.State.Get<ResourceContentPart>().Has(resourceCondition.Id),
                            Amount = resourceCondition.NeededAmount,
                            ResourcesType = MonologResourcesType.Demand
                        });
                    }

                    break;
                }
                case GiveResourcesConditionConfig giveResourcesConditionConfig:
                {
                    foreach (var resourceCondition in giveResourcesConditionConfig.ResourceConditions)
                    {
                        data.Add(new MonologResourceData
                        {
                            Icon = _resourcesService.GetSprite(resourceCondition.Id),
                            MainCharacterAmount = _gameStateManager.State.Get<ResourceContentPart>().Has(resourceCondition.Id),
                            Amount = resourceCondition.NeededAmount,
                            ResourcesType = MonologResourcesType.Give
                        });
                    }

                    break;
                }
            }

            return data;
        }
    }
}
