using CrossProject.Core.Actions;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using UnityEngine;

namespace L2Farm
{
    public interface ITimerCreator
    {
        TimerBuilder Launch(float seconds, Vector3 position);
    }
    
    public class TimerCreator : ITimerCreator
    {
        private readonly GameStateManager _gameStateManager;
        private readonly ActionService _actionService;
        private readonly ITimersHandler _timersHandler;
        private readonly QuestService _questService;

        public TimerCreator(GameStateManager gameStateManager, ActionService actionService, ITimersHandler timersHandler, 
            QuestService questService)
        {
            _gameStateManager = gameStateManager;
            _actionService = actionService;
            _timersHandler = timersHandler;
            _questService = questService;
        }

        public TimerBuilder Launch(float seconds, Vector3 position)
        {
            var builder = new TimerBuilder(_gameStateManager, _actionService, _timersHandler, _questService);

            return builder.SetupTime(seconds).CorrectPosition(position);
        }
    }
}
