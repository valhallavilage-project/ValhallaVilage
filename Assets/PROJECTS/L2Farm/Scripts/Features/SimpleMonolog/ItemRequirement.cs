using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.SimpleMonolog
{
    public class ItemRequirement : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text count;

        [Header("Colors")]
        [SerializeField] private Color _notEnough = Color.white;
        [SerializeField] private Color _enough = Color.green;
        [SerializeField] private Color _give = Color.yellow;

        public void SetVisuals(MonologResourceData data)
        {
            icon.sprite = data.Icon;

            switch (data.ResourcesType)
            {
                case MonologResourcesType.Demand:
                    background.color = data.MainCharacterAmount >= data.Amount ? _enough : _notEnough;
                    count.text = $"{data.MainCharacterAmount}/{data.Amount}";

                    break;
                case MonologResourcesType.Give:
                    background.color = _give;
                    count.text = $"{data.Amount}";

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
