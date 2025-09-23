using CrossProject.Core;
using CrossProject.Core.Conditions;
using CrossProject.Core.SaveLoad;
using L2Farm.Features.ResourceProduction;
using UnityEngine;

namespace L2Farm.Scripts.Conditions
{
    public class ProductionCompletedCondition : Condition<ProductionCompletedConditionConfig>
    {
        private readonly GameStateManager _gameStateManager;
        private readonly ITimeService _timeService;

        public ProductionCompletedCondition(ConditionService conditionService, GameStateManager gameStateManager,
            ITimeService timeService) : base(conditionService)
        {
            _gameStateManager = gameStateManager;
            _timeService = timeService;
        }

        public override bool Check()
        {
            if (!_gameStateManager.State.Get<ProductionPart>().requests.ContainsKey(config.Production))
            {
                Debug.LogError($"Production {config.Production} hasn't found in the game state");
                
                return true;
            }
            
            var productionFinishTime = _gameStateManager.State.Get<ProductionPart>().requests[config.Production];

            return _timeService.Now > productionFinishTime;
        }
    }
}
