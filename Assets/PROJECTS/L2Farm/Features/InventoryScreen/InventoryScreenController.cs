using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features.InventoryScreen
{
    public class InventoryScreenController : IInitializable
    {
        private readonly UiService _uiService;

        public InventoryScreenController(UiService uiService)
        {
            _uiService = uiService;
        }

        private void OpenScreen()
        {
            Debug.Log("Open Inventory Screen");
        }

        public async UniTask Initialize()
        {
            await _uiService.TryOpen(new InventoryButtonModel(OpenScreen));
        }
    }
}
