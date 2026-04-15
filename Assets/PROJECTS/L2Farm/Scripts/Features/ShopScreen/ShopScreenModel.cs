using System;
using CrossProject.Ui.Core;
using UnityEngine;

namespace L2Farm.Features.Shop
{
    public class ShopScreenModel : ScreenModel
    {
        public Action<ShopItemType> BuyRequested { get; set; }
        public Func<ShopItemType, Sprite> IconResolver { get; set; }
        public Func<ShopItemType, string> TitleResolver { get; set; }
        public Func<ShopItemType, string> PriceResolver { get; set; }
    }
}
