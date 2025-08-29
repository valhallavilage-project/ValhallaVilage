using System;
using CrossProject.Core;
using CrossProject.Core.Actions;
using CrossProject.Core.Actions.Implementations;
using CrossProject.Core.Camera;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Extensions;
using Cysharp.Threading.Tasks;
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
        private GameStateManager _gameStateManager;

        [SerializeField] private Transform uiRoot;
        [SerializeField] private TMP_Text timerLabel;
        [SerializeField] private Image progressBar;

        private float _seconds = -1;
        private float _secondsLeft;
        private QuestId _questId;

        [Inject]
        private void Construct(
            ActionService actionService,
            CameraService cameraService,
            QuestService questService,
            GameStateManager gameStateManager)
        {
            _actionService = actionService;
            cameraService.AlignWithCamera(uiRoot);
            questService.OnQuestWin += OnQuestWin;
            _questService = questService;
            _gameStateManager = gameStateManager;
        }

        private void Start()
        {
            ManualPrefabInjector.Instance?.Inject(this);
        }

        public void Setup(int seconds, QuestId questId)
        {
            _seconds = _secondsLeft = seconds;
            _questId = questId;
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
            _actionService.Execute(new LaunchQuestActionConfig
            {
                questId = _questId
            });
        }

        private void Update()
        {
            if (_seconds < 0)
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
        }
    }
}
