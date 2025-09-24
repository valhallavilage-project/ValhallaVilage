using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Scripts.AutoLootButton
{
    public class AutoLootButton : HudElementView<AutoLootButtonModel>
    {
        [SerializeField]
        private Button button;

        protected override void OnBind() => button.onClick.AddListener(() => Model.startAutoLoot?.Invoke());
    }
}
