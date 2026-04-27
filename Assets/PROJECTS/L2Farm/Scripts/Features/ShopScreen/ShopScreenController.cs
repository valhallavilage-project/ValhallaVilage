using System;
using System.Threading;
using CrossProject.Core.InGameResources;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace L2Farm.Features.Shop
{
    public class ShopScreenController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly IResourcesService _resourcesService;
        private readonly IConfirmPopupOpenHandler _confirmPopupOpenHandler;
        private readonly CancellationTokenSource _disposeCts = new();

        private ShopButtonModel _buttonModel;
        private ShopScreen _screen;

        public bool IsInitialized { get; private set; }

        public ShopScreenController(
            UiService uiService,
            IResourcesService resourcesService,
            IConfirmPopupOpenHandler confirmPopupOpenHandler)
        {
            _uiService = uiService;
            _resourcesService = resourcesService;
            _confirmPopupOpenHandler = confirmPopupOpenHandler;
        }

        public async UniTask Initialize()
        {
            _buttonModel = new ShopButtonModel();
            _buttonModel.Clicked.Listen(OpenScreen, _disposeCts.Token);

            await _uiService.TryOpen(_buttonModel);

            IsInitialized = true;
        }

        private async UniTask OpenScreen()
        {
            _screen = await _uiService.TryOpen(new ShopScreenModel
            {
                Close = Close,
                BuyRequested = OnBuyRequested,
                IconResolver = type => _resourcesService.GetSprite(GetResourceId(type)),
                TitleResolver = GetItemName,
                PriceResolver = GetItemPrice
            }) as ShopScreen;
        }

        private void Close()
        {
            if (_screen == null) return;

            _uiService.Close(_screen);
            _screen = null;
        }

        private void OnBuyRequested(ShopItemType type)
        {
            HandlePurchase(type).Forget();
        }

        private async UniTask HandlePurchase(ShopItemType type)
        {
            _confirmPopupOpenHandler.Open(new ConfirmPopupData(
                $"Вы точно хотите купить \"{GetItemName(type)}\"?",
                ConfirmPopupButtonsType.YesNo));

            var confirmed = await _confirmPopupOpenHandler.PopupResult
                .WithoutCurrent()
                .FirstAsync(_disposeCts.Token);

            if (!confirmed) return;

            //TODO : VM : Google Play billing integration before granting the item
            _resourcesService.ChangeResource(GetResourceId(type), 1);
        }

        private static string GetItemName(ShopItemType type) => type switch
        {
            ShopItemType.HealPotion => "Зелье лечения",
            ShopItemType.EnergyPotion => "Зелье энергии",
            ShopItemType.TimePotion => "Зелье времени",
            ShopItemType.Fertilizer => "Удобрение",
            _ => type.ToString()
        };

        private static string GetItemPrice(ShopItemType type) => type switch
        {
            ShopItemType.Fertilizer => "150 ₽",
            _ => "299 ₽"
        };

        private static ResourceId GetResourceId(ShopItemType type) => type switch
        {
            ShopItemType.HealPotion => (ResourceId)"Resource_HealPotion",
            ShopItemType.EnergyPotion => (ResourceId)"Resource_EnergyPotion",
            ShopItemType.TimePotion => (ResourceId)"Resource_TimePotion",
            ShopItemType.Fertilizer => (ResourceId)"Resource_Fertilizer",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
