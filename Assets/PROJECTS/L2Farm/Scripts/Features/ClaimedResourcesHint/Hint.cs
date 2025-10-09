using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.ClaimerResourcesHint
{
    public class Hint : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amount;

        public void Setup(Sprite icon, int amount)
        {
            this.icon.sprite = icon;
            this.amount.text = amount.ToString();

            rectTransform.DOAnchorPosY(200, 3);
            this.icon.DOColor(new Color(1, 1, 1, 0), 3);
            Invoke(nameof(Kill), 3);
        }

        private void Kill()
        {
            Destroy(gameObject);
        }
    }
}
