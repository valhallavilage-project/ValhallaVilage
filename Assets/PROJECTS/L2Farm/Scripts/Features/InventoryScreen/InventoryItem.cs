using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.InventoryScreen
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text count;
        [SerializeField] private GameObject blocker;

        public void Setup(Sprite icon, int count, bool blocker = false)
        {
            this.blocker.SetActive(blocker);
            this.icon.gameObject.SetActive(icon != null);
            this.count.gameObject.SetActive(icon != null);
            this.icon.sprite = icon;
            this.count.text = count.ToString();
        }
    }
}
