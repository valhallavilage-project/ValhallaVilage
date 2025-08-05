using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features.QuestsScreen
{
    public class QuestsPopupController : IInitializable
    {
        private readonly UiService _uiService;

        public QuestsPopupController(UiService uiService)
        {
            _uiService = uiService;
        }

        private void OpenPopup()
        {
            Debug.Log("Open Quests");
        }

        public async UniTask Initialize()
        {
            await _uiService.TryOpen(new QuestsButtonModel(OpenPopup));
        }
    }
}
