using System.Collections.Generic;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using L2Farm.Features;
using L2Farm.Features.SimpleMonolog;
using L2Farm.Scripts;
using L2Farm.Scripts.Conditions;
using VContainer.Unity;

namespace L2Farm
{
    public interface IResourceConditionService
    {
        List<ConditionResourceData> ProcessConditionResources(QuestId questId, int currentQuestStep);
    }

    public class ResourceConditionService : IResourceConditionService, IInitializable
    {
        private readonly QuestService _questService;
        private readonly ResourcesService _resourcesService;
        private readonly GameStateManager _gameStateManager;

        public bool IsInitialized { get; private set; }

        public async UniTask Initialize()
        {
            IsInitialized = true;
        }

        public ResourceConditionService(QuestService questService, ResourcesService resourcesService,
            GameStateManager gameStateManager)
        {
            _questService = questService;
            _resourcesService = resourcesService;
            _gameStateManager = gameStateManager;
        }

        public List<ConditionResourceData> ProcessConditionResources(QuestId questId, int currentQuestStep)
        {
            var data = new List<ConditionResourceData>();

            var winCondition = _questService.GetConfigFor(questId).steps[currentQuestStep].winCondition;

            switch (winCondition)
            {
                case HasEnoughResourcesConditionConfig hasEnoughResourcesConditionConfig:
                {
                    foreach (var resourceCondition in hasEnoughResourcesConditionConfig.ResourceConditions)
                    {
                        data.Add(new ConditionResourceData
                        {
                            Icon = _resourcesService.GetSprite(resourceCondition.Id),
                            MainCharacterAmount = _gameStateManager.State.Get<ResourceContentPart>().Get(resourceCondition.Id),
                            Count = resourceCondition.NeededAmount,
                            ResourcesType = MonologResourcesType.Demand
                        });
                    }

                    break;
                }
                case GiveResourcesConditionConfig giveResourcesConditionConfig:
                {
                    foreach (var resourceCondition in giveResourcesConditionConfig.ResourceConditions)
                    {
                        data.Add(new ConditionResourceData
                        {
                            Icon = _resourcesService.GetSprite(resourceCondition.Id),
                            MainCharacterAmount = _gameStateManager.State.Get<ResourceContentPart>().Get(resourceCondition.Id),
                            Count = resourceCondition.NeededAmount,
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
