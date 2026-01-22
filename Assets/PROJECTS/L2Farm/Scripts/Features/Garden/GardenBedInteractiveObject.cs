using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    [Serializable]
    public struct GardenBedStateData
    {
        [SerializeField] private GardenBedStateType _state;
        [SerializeField] private Sprite _image;
        [SerializeField] private InteractionType _interactionType;

        public GardenBedStateType State => _state;
        public Sprite Image => _image;
        public InteractionType InteractionType => _interactionType;
    }
    
    public class GardenBedInteractiveObject : AbstractInteractiveObject
    {
        [SerializeField] private List<GardenBedStateData> _data;
        
        private readonly AsyncReactiveProperty<Invoker> _tryClear = new(default);
        private GardenBedStateType _currentState;
        
        public IReadOnlyAsyncReactiveProperty<Invoker> TryClear => _tryClear;

        protected override async UniTask AfterInteraction()
        {
            if (_currentState == GardenBedStateType.Overgrown)
            {
                _tryClear.Invoke();
            }
        }

        public void SetState(GardenBedStateType state)
        {
            _currentState = state;

            var data = _data.FirstOrDefault(i => i.State == state);
            if (data.Equals(default(GardenBedStateData)))
            {
                Debug.LogError($"[GardenBedInteractiveObject] Data not found for state: {state}");
                return;
            }

            buttonSprite = data.Image;
            animation = data.InteractionType;
        }
    }
}
