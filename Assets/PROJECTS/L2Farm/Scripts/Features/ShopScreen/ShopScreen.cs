using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.Shop
{
    public class ShopScreen : ScreenView<ShopScreenModel>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private ShopItemView[] _items;

        protected override void OnBind()
        {
            base.OnBind();

            _closeButton.SetUniqueCallback(Model.Close);

            foreach (var item in _items)
            {
                var icon = Model.IconResolver?.Invoke(item.Type);
                var title = Model.TitleResolver?.Invoke(item.Type) ?? string.Empty;
                var price = Model.PriceResolver?.Invoke(item.Type) ?? string.Empty;
                item.Bind(icon, title, price, Model.BuyRequested);
            }
        }
    }
}
