using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            //TODO : VM : check for AND condition
            //TODO : VM : check for OR condition

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

            //TODO : VM : remove and condition
            //TODO : VM : remove or condition

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
            var condition = GetCondition(conditionConfig);
            condition.SetConfig(conditionConfig);
            return condition.Check();
        }
    }
}
