using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.SimpleMonolog
{
    public class MonologConditionItem : MonoBehaviour
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private TMP_Text count;

        public void SetVisuals(Sprite icon, int has, int need)
        {
            this.icon.sprite = icon;
            count.text = $"{has}/{need}";
            background.color = has >= need
                ? Color.green
                : Color.white;
        }
    }
}
