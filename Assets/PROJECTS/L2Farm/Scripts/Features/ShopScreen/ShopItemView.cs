using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private ShopItemType _type;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Button _buyButton;

        public ShopItemType Type => _type;

        public void Bind(Sprite icon, string title, string priceText, Action<ShopItemType> onBuy)
        {
            if (_icon != null && icon != null)
                _icon.sprite = icon;
            if (_title != null)
                _title.text = title;
            if (_price != null)
                _price.text = priceText;

            _buyButton.SetUniqueCallback(() => onBuy?.Invoke(_type));
        }
    }
}
