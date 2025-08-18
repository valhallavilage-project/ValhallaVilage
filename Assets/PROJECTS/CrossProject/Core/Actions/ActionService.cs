using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core.Actions
{
    public class ActionService : IInitializable, IDisposable
    {
        private readonly IObjectResolver _objectResolver;
        private readonly Dictionary<Type, IAction> _actionMap = new();

        public bool IsInitialized { get; private set; }

        public ActionService(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask Initialize()
        {
            FillMap();
            IsInitialized = true;
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _actionMap.Clear();
        }

        private void FillMap()
        {
            _actionMap.Clear();

            var actionInterface = typeof(IAction);
            var actionTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && actionInterface.IsAssignableFrom(type))
                .ToList();

            foreach (var type in actionTypes)
            {
                try
                {
                    var action = _objectResolver.Resolve(type) as IAction;
                    _actionMap.TryAdd(action.ConfigType, action);
                }
                catch (Exception e)
                {
                    Debug.LogException(new NotImplementedException($"No action found for config {type.Name}"));
                    throw;
                }
            }
        }

        public void Execute(IActionConfig config)
        {
            if (!_actionMap.TryGetValue(config.GetType(), out var action))
                throw new Exception($"Can't find {config.GetType().Name} in actions map!");

            action.SetConfig(config);
            action.Execute();
        }

        public void Execute(IEnumerable<IActionConfig> configs)
        {
            foreach (var actionConfig in configs)
                Execute(actionConfig);
        }
    }
}
