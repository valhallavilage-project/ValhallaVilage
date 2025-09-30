using CrossProject.Core.Interactions;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    public class ConfirmPopupInteractiveObject : AbstractInteractiveObject
    {
        [SerializeField] private string _text;

        private IConfirmPopupOpenHandler _confirmPopupOpenHandler;

        [Inject]
        private void AddDependencies(IConfirmPopupOpenHandler confirmPopupOpenHandler)
        {
            _confirmPopupOpenHandler = confirmPopupOpenHandler;
        }
        
        protected override async UniTask AfterInteraction()
        {
            _confirmPopupOpenHandler.Open(_text);

            UniTask.WaitForEndOfFrame(this).ContinueWith(Select).Forget();
        }
    }
}
