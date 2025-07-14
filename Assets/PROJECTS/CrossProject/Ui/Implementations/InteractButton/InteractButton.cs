using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CrossProject.Ui.Implementations.InteractButton
{
    public class InteractButton : HudElementView<InteractButtonModel>
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        protected override void OnBind()
        {
            rectTransform.anchorMin = Model.anchorMin;
            rectTransform.anchorMax = Model.anchorMax;
            rectTransform.sizeDelta = Model.sizeDelta;
            rectTransform.anchoredPosition = Vector2.zero;

            icon.gameObject.SetActive(Model.InteractionIcon != null);
            if (Model.InteractionIcon != null)
                icon.sprite = Model.InteractionIcon;

            button.onClick.RemoveAllListeners();
            if (Model.Interaction != null)
                button.onClick.AddListener(() => Model.Interaction?.Invoke());
        }
    }
}