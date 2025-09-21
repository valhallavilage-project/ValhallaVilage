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
    public class ActionService : IPriorityInitializable, IDisposable
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

            var actions = _objectResolver.Resolve<IReadOnlyList<IAction>>();
            foreach (var action in actions)
            {
                try
                {
                    _actionMap.TryAdd(action.ConfigType, action);
                    //Debug.Log($"[{nameof(ActionService)}] filled map with : {action.GetType().Name} : {_actionMap.Keys.Count}");
                }
                catch (Exception e)
                {
                    Debug.LogException(new NotImplementedException($"No action found for config {action.GetType().Name}"));
                    throw;
                }
            }
        }

        public async UniTask Execute(IActionConfig config)
        {
            if (!_actionMap.TryGetValue(config.GetType(), out var action))
                throw new Exception($"Can't find {config.GetType().Name} in actions map!");

            action.SetConfig(config);
            await action.Execute();
        }

        public async UniTask Execute(IEnumerable<IActionConfig> configs)
        {
            foreach (var actionConfig in configs)
                await Execute(actionConfig);
        }
    }
}
