using System.Collections.Generic;
using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace L2Farm
{
    public class AdditionalNpcActions : MonoBehaviour
    {
        [SerializeReference] private List<IActionConfig> _actions;
        
        public async UniTask Launch(ActionService actionService)
        {
            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    await actionService.Execute(action);
                }
            }
        }
    }
}
