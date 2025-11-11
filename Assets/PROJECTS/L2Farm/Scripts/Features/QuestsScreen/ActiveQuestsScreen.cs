using System.Collections.Generic;
using CrossProject.Core.Conditions;
using CrossProject.Core.Conditions.ConditionsImplementations;
using CrossProject.Ui.Core;
using L2Farm.Scripts;
using L2Farm.Scripts.Conditions;
using L2Farm.Scripts.Conditions.QuestCompleted;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features
{
    public class ActiveQuestsScreen : ScreenView<ActiveQuestsScreenModel>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private QuestItemPool _itemPool;
        [SerializeField] private ItemRequirementPool _itemRequirementPool;
        [SerializeField] private Transform _scrollContent;
        
        private readonly List<ActiveQuestsItem> _questItems = new();

        protected override void OnBind()
        {
            _closeButton.SetUniqueCallback(Model.Close);

            var launchedQuests = Model.GameStatePart.launchedQuests;

            foreach (var questKeyValue in launchedQuests)
            {
                var step = Model.QuestService.GetCurrentStepFor(questKeyValue.Key);
                var winCondition = Model.QuestService.GetConfigFor(questKeyValue.Key).steps[step].winCondition;
                var questType = DetectQuestType(winCondition);
                var questItem = _itemPool.Get();
                
                questItem.transform.SetParent(_scrollContent);
                questItem.Setup(Model, Model.QuestService.GetConfigFor(questKeyValue.Key), questType, _itemRequirementPool);
                
                _questItems.Add(questItem);
            }
        }

        private QuestItemType DetectQuestType(IConditionConfig conditionConfig)
        {
            switch (conditionConfig)
            {
                case TrueConditionConfig:
                    return QuestItemType.SimpleDialog;
                case GiveResourcesConditionConfig or HasEnoughResourcesConditionConfig:
                    return QuestItemType.Resources;
                case ProductionCompletedConditionConfig:
                    return QuestItemType.ProductionCompleted;
                case QuestCompletedConditionConfig:
                    return QuestItemType.QuestCompleted;
                default:
                    Debug.LogError($"Not processed quest type: {conditionConfig.GetType()}");

                    return QuestItemType.Empty;
            }
        }

        public void ClearItems()
        {
            foreach (var questItem in _questItems)
            {
                questItem.Clear();
            }
        }
    }
}
