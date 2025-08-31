using CrossProject.Core;
using CrossProject.Core.Quests;
using UnityEngine;
using VContainer;

namespace L2Farm.Features.NPC
{
    public class NpcQuestMarker : MonoBehaviour
    {
        private QuestService _questService;

        [SerializeField] private GameObject exclamationMark;
        [SerializeField] private GameObject activeQuestionMark;
        [SerializeField] private GameObject disabledQuestionMark;

        private QuestId _targetQuestId;

        private void Start()
        {
            Injector.Instance?.Inject(this);
        }

        [Inject]
        private void Construct(QuestService questService)
        {
            _questService = questService;
            _questService.OnQuestProceed += OnQuestProceed;
        }

        private void OnDestroy()
        {
            _questService.OnQuestProceed -= OnQuestProceed;
        }

        private void OnQuestProceed(QuestId questId, int stepIndex)
        {
            if (string.IsNullOrEmpty(questId))
            {
                activeQuestionMark.SetActive(false);
                disabledQuestionMark.SetActive(false);
                exclamationMark.SetActive(false);
                return;
            }

            if (questId != _targetQuestId)
                return;

            if (stepIndex == 0)
            {
                exclamationMark.SetActive(true);
            }
            else
            {
                exclamationMark.SetActive(false);
                var condition = _questService.CanProceed(questId);
                activeQuestionMark.SetActive(condition);
                disabledQuestionMark.SetActive(!condition);
            }
        }

        public void Setup(QuestId questId, QuestService questService)
        {
            _targetQuestId = questId;
            _questService = questService;
            _questService.OnQuestProceed -= OnQuestProceed;
            _questService.OnQuestProceed += OnQuestProceed;
            OnQuestProceed(questId, questService.GetCurrentStepFor(questId));
        }
    }
}
