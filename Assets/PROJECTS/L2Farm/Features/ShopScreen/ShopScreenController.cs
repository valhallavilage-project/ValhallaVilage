using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features.ShopScreen
{
    public class ShopScreenController : IInitializable
    {
        private readonly UiService _uiService;

        public ShopScreenController(UiService uiService)
        {
            _uiService = uiService;
        }

        private void OpenScreen()
        {
            Debug.Log("Open Shop");
        }

        public async UniTask Initialize()
        {
            await _uiService.TryOpen(new ShopButtonModel(OpenScreen));
        }
    }
}
