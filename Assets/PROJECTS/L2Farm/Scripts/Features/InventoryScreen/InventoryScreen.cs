using System.Collections.Generic;
using System.Linq;
using CrossProject.Extensions;
using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.InventoryScreen
{
    public class InventoryScreen : ScreenView<InventoryScreenModel>
    {
        [SerializeField] private List<InventoryItem> _slots = new();
        [SerializeField] private Button _closeBtn;

        protected override void OnBind()
        {
            _closeBtn.SetUniqueCallback(Model.Close);
            var list = Model.gameStatePart.Resources.ToList();

            for (var i = 0; i < Model.gameStatePart.Resources.Count; i++)
            {
                var pair = list[i];
                _slots[i].Setup(Model.resourcesService.GetSprite(pair.Key), pair.Value);
            }
        }
    }
}
