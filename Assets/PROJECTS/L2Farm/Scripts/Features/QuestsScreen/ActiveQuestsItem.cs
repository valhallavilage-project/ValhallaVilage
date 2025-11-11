using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.Characters;
using CrossProject.Core.Pooling;
using CrossProject.Core.Quests;
using L2Farm.Features.NPC;
using L2Farm.Features.SimpleMonolog;
using L2Farm.Scripts.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features
{
    public class ActiveQuestsItem : MonoBehaviour, IPoolElement
    {
        [SerializeField] private Image _npcImage;
        [SerializeField] private GameObject _itemRequirementsPanel;
        [SerializeField] private GameObject _dialogItem;
        [SerializeField] private GameObject _productionItem;
        [SerializeField] private Sprite _npcStubImage;
        [SerializeField] private NPCSetConfig _npcConfig;
        [SerializeField] private CharacterSetConfig _charactersConfig;

        private readonly List<GameObject> _components = new();
        private readonly List<ItemRequirement> _itemRequirements = new();
        private IPool _questItemPool;
        private ItemRequirementPool _itemRequirementPool;

        public bool IsAvailableToGet { get; private set; }

        private void Awake()
        {
            _components.Add(_itemRequirementsPanel);
            _components.Add(_dialogItem);
            _components.Add(_productionItem);
        }

        public void Setup(ActiveQuestsScreenModel model, QuestConfig config, QuestItemType questType,
            ItemRequirementPool itemRequirementPool)
        {
            _itemRequirementPool = itemRequirementPool;
            
            TurnOffElements();
            InitializeContent(model, config, questType);
        }

        private void TurnOffElements()
        {
            foreach (var component in _components)
            {
                component.gameObject.SetActive(false);
            }

            foreach (var itemRequirement in _itemRequirements)
            {
                _itemRequirementPool.Return(itemRequirement);
            }
        }

        private void InitializeContent(ActiveQuestsScreenModel model, QuestConfig config, QuestItemType questType)
        {
            InitializeNpcImage(config);

            switch (questType)
            {
                case QuestItemType.Empty:
                    break;
                case QuestItemType.Resources:
                    ShowResources(model, config);

                    break;
                case QuestItemType.ProductionCompleted:
                    ShowProduction(model, config);

                    break;
                case QuestItemType.SimpleDialog:
                    ShowDialog(model, config);

                    break;
                case QuestItemType.QuestCompleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(questType), questType, null);
            }
        }

        private void InitializeNpcImage(QuestConfig config)
        {
            if (config.launchActions.FirstOrDefault(a => a is SpawnNPCActionConfig) is SpawnNPCActionConfig npcAction)
            {
                var characterId = _npcConfig.items.First(npc => npc.id == npcAction.npcId).characterId;
                var characterConfig = _charactersConfig.items.First(ch => ch.id == characterId);

                _npcImage.sprite = characterConfig.portrait;
            }
            else
            {
                _npcImage.sprite = _npcStubImage;
            }
        }

        private void ShowResources(ActiveQuestsScreenModel model, QuestConfig config)
        {
            _itemRequirementsPanel.gameObject.SetActive(true);

            var step = model.QuestService.GetCurrentStepFor(config.id);
            var resources = model.ResourceConditionService.ProcessConditionResources(config.id, step);

            foreach (var resourceData in resources)
            {
                var item = _itemRequirementPool.Get();

                item.Setup(resourceData);
                item.transform.SetParent(_itemRequirementsPanel.transform);

                _itemRequirements.Add(item);
            }
        }

        private void ShowProduction(ActiveQuestsScreenModel model, QuestConfig config)
        {
            _productionItem.SetActive(true);
        }

        private void ShowDialog(ActiveQuestsScreenModel model, QuestConfig config)
        {
            _dialogItem.SetActive(true);
        }

        public void SetPool(IPool pool)
        {
            _questItemPool = pool;
        }

        public void OnGet()
        {
            IsAvailableToGet = false;
        }

        public void OnReturn()
        {
            IsAvailableToGet = true;
        }

        public void Clear()
        {
            _questItemPool.Return(this);
        }
    }
}
