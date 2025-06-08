using CrossProject.Ui.Core;
using DG.Tweening;
using UnityEngine;

namespace CrossProject.Ui.Implementations
{
    public class LoadingScreen : ScreenView<LoadingScreenModel>
    {
        //TODO : VM : spinner
        [SerializeField] private RectTransform fill;

        public override void OnOpen()
        {
            //TODO : VM : get tasks
            //TODO : VM : calculate percentage
            //TODO : VM : display
            //TODO : VM : update info
            var sequence = DOTween.Sequence();
            sequence
                .Append(fill.DOScaleX(1, 1.75f).SetEase(Ease.OutCubic))
                .AppendInterval(0.1f)
                .AppendCallback(Model.Close.Invoke);
        }
    }
}