using System;
using CrossProject.Core;
using CrossProject.Core.Actions;
using CrossProject.Core.Actions.Implementations;
using CrossProject.Core.Camera;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Extensions;
using Cysharp.Threading.Tasks;
using L2Farm.Features.ResourceProduction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace L2Farm.Features.Buildings
{
    public class BuildingTimer : MonoBehaviour
    {
        private ActionService _actionService;
        private QuestService _questService;
        private CameraService _cameraService;
        private GameStateManager _gameStateManager;

        [SerializeField] private Transform vfxRoot;
        [SerializeField] private Transform uiRoot;
        [SerializeField] private TMP_Text timerLabel;
        [SerializeField] private Image progressBar;

        private float _seconds = -1;
        private float _secondsLeft;
        private bool _tick;
        private BuildingId _buildingId;
        private ProductionId _productionId;
        private QuestId _questId;

        [Inject]
        private void Construct(
            ActionService actionService,
            CameraService cameraService,
            QuestService questService,
            GameStateManager gameStateManager)
        {
            _actionService = actionService;
            _cameraService = cameraService;
            questService.OnQuestWin += OnQuestWin;
            _questService = questService;
            _gameStateManager = gameStateManager;
        }

        private void Awake()
        {
            Injector.Instance.Inject(this);
        }

        public void Setup(int seconds, BuildingId buildingId, QuestId questId, float vfxScale)
        {
            _buildingId = buildingId;
            
            Setup(seconds, questId, vfxScale);
        }

        public void Setup(int seconds, ProductionId productionId, QuestId questId, float vfxScale)
        {
            _productionId = productionId;
            
            Setup(seconds, questId, vfxScale);
        }

        private void Setup(int seconds, QuestId questId, float vfxScale)
        {
            _seconds = _secondsLeft = seconds;
            _questId = questId;
            _cameraService.AlignWithCamera(uiRoot);
            vfxRoot.GetChild(0).localScale = Vector3.one * vfxScale;
            Routine().Forget();
        }

        private void OnQuestWin(QuestId id)
        {
            if (id != _questId)
                return;

            _questService.OnQuestWin -= OnQuestWin;
            Destroy(gameObject);
        }

        private async UniTask Routine()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_seconds));
            await _actionService.Execute(new LaunchQuestActionConfig
            {
                questId = _questId
            });
            if (_buildingId != null && !string.IsNullOrEmpty(_buildingId))
                _gameStateManager.State.Get<BuildingPart>().requests.Remove(_buildingId);
            if (_productionId != null && !string.IsNullOrEmpty(_productionId))
                _gameStateManager.State.Get<ProductionPart>().requests.Remove(_productionId);
        }

        private void Update()
        {
            if (_seconds < 0 && _tick)
                return;

            _secondsLeft -= Time.deltaTime;
            if (_secondsLeft > 0)
            {
                progressBar.fillAmount = 1 - _secondsLeft/_seconds;
                timerLabel.text = $"{_secondsLeft.ToTimeFormat()}";
            }
            else
            {
                progressBar.fillAmount = 1;
                timerLabel.text = "DONE";
                progressBar.color = Color.green;
            }
            _tick = true;
        }
    }
}
