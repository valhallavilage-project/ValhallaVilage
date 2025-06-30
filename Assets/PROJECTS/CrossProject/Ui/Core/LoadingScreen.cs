using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CrossProject.Ui.Core
{
    public class LoadingScreen : ScreenView<LoadingScreenModel>
    {
        private const string SpinnerSequenceId = "spinner";
        private const string ProgressSequenceId = "progress";

        [SerializeField] private RectTransform spinner;
        [SerializeField] private RectTransform fill;
        [SerializeField] private TMP_Text flavourTextLabel;

        private string FlavourText
        {
            set
            {
                if (flavourTextLabel == null)
                    return;

                flavourTextLabel.gameObject.SetActive(!string.IsNullOrEmpty(value));
                flavourTextLabel.text = value;
            }
        }

        public override void OnOpen()
        {
            if (spinner == null)
                return;

            var sequence = DOTween.Sequence();
            sequence
                .Append(spinner.DOLocalRotate(new Vector3(0, 0, 360), 1))
                .SetLoops(-1)
                .SetId(SpinnerSequenceId);
        }

        public override void OnClose()
        {
            DOTween.Kill(SpinnerSequenceId);
            DOTween.Kill(ProgressSequenceId);
        }

        public async UniTask UpdateProgress(float progress, string flavourText = null)
        {
            FlavourText = flavourText;

            DOTween.Kill(ProgressSequenceId);
            await DOTween.Sequence()
                .Append(fill.DOScaleX(progress, 0.5f).SetEase(Ease.OutCubic))
                .AppendInterval(0.2f)
                .SetId(ProgressSequenceId)
                .ToUniTask();
        }
    }
}