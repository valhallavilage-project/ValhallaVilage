using CrossProject.Ui.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.ClaimerResourcesHint
{
    public class ClaimedResourcesHint : HudElementView<ClaimedResourcesHintModel>
    {
        [SerializeField] private Hint hintPrefab;

        public void Spawn(Sprite sprite, int amount)
        {
            var instance = Instantiate(hintPrefab, transform);
            instance.Setup(sprite, amount);
        }
    }
}
