using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Scripts.CharacterHudElement
{
    public class CharacterHudElement : HudElementView<CharacterHudElementModel>
    {
        private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");

        [SerializeField]
        private Image portrait;

        [SerializeField]
        private Image healthBarFill;

        [SerializeField]
        private Image manaBarFill;

        [SerializeField]
        private Image frame;

        [SerializeField]
        private Sprite premiumFrame;

        public void SetPortrait(Sprite sprite) => portrait.sprite = sprite;

        public void SetHealth(float value01) => healthBarFill.material.SetFloat(FillAmount, value01);

        public void SetMana(float value01) => manaBarFill.material.SetFloat(FillAmount, value01);

        public void SetPremium(bool value)
        {
            if (value)
                frame.sprite = premiumFrame;
        }
    }
}
