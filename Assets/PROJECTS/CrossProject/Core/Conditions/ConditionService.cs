using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrossProject.Core.Conditions.ConditionsImplementations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core.Conditions
{
    public class ConditionService : IPriorityInitializable, IDisposable
    {
        private readonly IObjectResolver _objectResolver;
        private readonly Dictionary<Type, ICondition> _conditionMap = new();

        public bool IsInitialized { get; private set; }

        public ConditionService(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask Initialize()
        {
            FillMap();
            await UniTask.CompletedTask;
            IsInitialized = true;
        }

        public void Dispose()
        {
            _conditionMap.Clear();
        }

        private ICondition GetCondition(IConditionConfig conditionConfig)
        {
            if (conditionConfig is AndConditionConfig)
                return new AndCondition(this);

            if (conditionConfig is OrConditionConfig)
                return new OrCondition(this);

            if (_conditionMap.TryGetValue(conditionConfig.GetType(), out var condition))
                return condition;

            throw new Exception($"Can't find {conditionConfig.GetType().Name} int conditions map!");
        }

        private void FillMap()
        {
            _conditionMap.Clear();

            var conditions = _objectResolver.Resolve<IReadOnlyList<ICondition>>();
            conditions = conditions.Where(x => x is not AndCondition && x is not OrCondition).ToList();
            foreach (var condition in conditions)
            {
                try
                {
                    _conditionMap.TryAdd(condition.ConfigType, condition);
                    //Debug.Log($"[{nameof(ConditionService)}] filled map with : {condition.GetType().Name} : {_conditionMap.Keys.Count}");
                }
                catch (Exception e)
                {
                    Debug.LogException(new NotImplementedException($"No condition found for config {condition.GetType().Name}"));
                    throw;
                }
            }
        }

        public bool Check<TConditionConfig>(TConditionConfig conditionConfig) where TConditionConfig : class, IConditionConfig
        {
            if (conditionConfig == null)
            {
                Debug.LogError("Condition config can't be null. Choose true condition if it is always must be positive");
                
                return false;
            }

            var condition = GetCondition(conditionConfig);
            condition.SetConfig(conditionConfig);
            return condition.Check();
        }
    }
}
