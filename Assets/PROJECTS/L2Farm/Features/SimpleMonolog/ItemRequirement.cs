using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.SimpleMonolog
{
    public class ItemRequirement : MonoBehaviour
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private TMP_Text count;

        public void SetVisuals(ResourceRequirementData data)
        {
            icon.sprite = data.icon;
            count.text = $"{data.has}/{data.need}";
            background.color = data.has >= data.need
                ? Color.green
                : Color.white;
        }
    }
}
