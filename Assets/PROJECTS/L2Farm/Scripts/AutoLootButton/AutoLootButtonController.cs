using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Scripts.AutoLootButton
{
    public class AutoLootButtonController : IInitializable
    {
        private readonly UiService _uiService;

        private AutoLootButton _view;

        public AutoLootButtonController(UiService uiService)
        {
            _uiService = uiService;
        }

        public async UniTask Initialize()
        {
            var model = new AutoLootButtonModel();
            model.startAutoLoot = AutoLoot;
            _view = await _uiService.TryOpen(model) as AutoLootButton;
        }

        private void AutoLoot()
        {
            
        }
    }
}
