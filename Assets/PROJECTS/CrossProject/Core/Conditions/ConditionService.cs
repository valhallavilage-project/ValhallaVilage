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
    public class ConditionService : IInitializable, IDisposable
    {
        private readonly IObjectResolver _objectResolver;
        private readonly Dictionary<Type, ICondition> _conditionMap = new();

        public ConditionService(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask Initialize()
        {
            FillMap();
            await UniTask.CompletedTask;
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

            var conditionInterface = typeof(ICondition);
            var conditionTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && conditionInterface.IsAssignableFrom(type))
                .ToList();

            conditionTypes.Remove(typeof(AndCondition));
            conditionTypes.Remove(typeof(OrCondition));

            foreach (var type in conditionTypes)
            {
                try
                {
                    var condition = _objectResolver.Resolve(type) as ICondition;
                    _conditionMap.TryAdd(condition.ConfigType, condition);
                }
                catch (Exception e)
                {
                    Debug.LogException(new NotImplementedException($"No condition found for config {type.Name}"));
                    throw;
                }
            }
        }

        public bool Check<TConditionConfig>(TConditionConfig conditionConfig) where TConditionConfig : class, IConditionConfig
        {
            //If there is no condition set -> there is no blockers
            if (conditionConfig == null)
                return true;

            var condition = GetCondition(conditionConfig);
            condition.SetConfig(conditionConfig);
            return condition.Check();
        }
    }
}
