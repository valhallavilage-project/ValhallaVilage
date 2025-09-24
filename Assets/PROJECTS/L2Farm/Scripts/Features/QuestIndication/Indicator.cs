using System;
using UnityEngine;

namespace L2Farm.Features.QuestIndication
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField] private Transform handle;
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private SpriteRenderer pointer;
        [SerializeField] private SpriteRenderer icon;

        public void SetRadius(float radius)
        {
            handle.position = Vector3.forward * radius;
        }

        public void SetUp(Sprite sprite, Color color)
        {
            icon.sprite = sprite;
            body.color = pointer.color = color;
        }
    }
}
