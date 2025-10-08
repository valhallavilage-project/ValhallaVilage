using CrossProject.Core.Actions;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using L2Farm.Features.Buildings;
using L2Farm.Features.ResourceProduction;
using UnityEngine;

namespace L2Farm
{
    public class TimerBuilder
    {
        private readonly GameStateManager _gameStateManager;
        private readonly ActionService _actionService;
        private readonly ITimersHandler _timersHandler;
        private readonly QuestService _questService;
        private readonly TimerSetupData _timerSetupData = new();

        public TimerBuilder(GameStateManager gameStateManager, ActionService actionService, ITimersHandler timersHandler, QuestService questService)
        {
            _gameStateManager = gameStateManager;
            _actionService = actionService;
            _timersHandler = timersHandler;
            _questService = questService;
        }

        public TimerBuilder SetupTime(float seconds)
        {
            _timerSetupData.Seconds = seconds;

            return this;
        }

        public TimerBuilder BindBuilding(BuildingId buildingId)
        {
            _timerSetupData.Callbacks.Add(new BuildingTimerCallback(_gameStateManager, buildingId));

            return this;
        }

        public TimerBuilder BindQuest(QuestId questId)
        {
            _timerSetupData.Callbacks.Add(new QuestTimerCallback(questId, _actionService, _questService));

            return this;
        }

        public TimerBuilder BindProduction(ProductionId productionId)
        {
            _timerSetupData.Callbacks.Add(new ProductionTimerCallback(_gameStateManager, productionId));

            return this;
        }

        public TimerBuilder CorrectVfxScale(float vfxScale)
        {
            _timerSetupData.VfxScale = vfxScale;

            return this;
        }

        public TimerBuilder CorrectPosition(Vector3 position)
        {
            _timerSetupData.WorldPosition = position;

            return this;
        }

        public string Start()
        {
            return _timersHandler.Launch(_timerSetupData);
        }
    }
}