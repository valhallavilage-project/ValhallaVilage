using CrossProject.Core;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm
{
    public class SoftCurrencyHudElementController : IInitializable
    {
        private readonly UiService _uiService;
        private readonly ISoftCurrencyHolder _softCurrencyHolder;
        private SoftCurrencyHudElement _view;
        public bool IsInitialized { get; private set; }

        public SoftCurrencyHudElementController(UiService uiService, ISoftCurrencyHolder softCurrencyHolder)
        {
            _uiService = uiService;
            _softCurrencyHolder = softCurrencyHolder;
        }

        public async UniTask Initialize()
        {
            var model = new SoftCurrencyHudElementModel
            {
                SoftCurrencyHolder = _softCurrencyHolder,
            };

            _view = await _uiService.TryOpen(model) as SoftCurrencyHudElement;

            IsInitialized = true;
        }
    }
}
