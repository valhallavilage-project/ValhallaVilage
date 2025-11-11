using System;
using CrossProject.Core.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.SimpleMonolog
{
    public class ItemRequirement : MonoBehaviour, IPoolElement
    {
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text count;

        [Header("Colors")]
        [SerializeField] private Color _notEnough = Color.white;
        [SerializeField] private Color _enough = Color.green;
        [SerializeField] private Color _give = Color.yellow;

        public bool IsAvailableToGet { get; private set; }

        public void Setup(ConditionResourceData data)
        {
            icon.sprite = data.Icon;

            switch (data.ResourcesType)
            {
                case MonologResourcesType.Demand:
                    background.color = data.MainCharacterAmount >= data.Count ? _enough : _notEnough;
                    count.text = $"{data.MainCharacterAmount}/{data.Count}";

                    break;
                case MonologResourcesType.Give:
                    background.color = _give;
                    count.text = $"{data.Count}";

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetPool(IPool pool)
        {
        }

        public void OnGet()
        {
            IsAvailableToGet = false;
            gameObject.SetActive(true);
        }

        public void OnReturn()
        {
            IsAvailableToGet = true;
            gameObject.SetActive(false);
        }
    }
}
