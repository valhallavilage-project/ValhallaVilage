using UnityEngine;
using UnityEngine.UI;

namespace CrossProject.Ui.Core
{
    public class HudButton<THudButtonModel> : HudElementView<THudButtonModel> where THudButtonModel : HudButtonModel
    {
        [SerializeField]
        protected Button button;

        protected override void OnBind()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => Model.OnClick?.Invoke());
        }
    }
}
