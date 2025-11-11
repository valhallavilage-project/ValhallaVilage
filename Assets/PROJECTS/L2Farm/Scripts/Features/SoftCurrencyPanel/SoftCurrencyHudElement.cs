using CrossProject.Core;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TMPro;
using UnityEngine;

namespace L2Farm
{
    public class SoftCurrencyHudElement : HudElementView<SoftCurrencyHudElementModel>
    {
        [SerializeField] private TMP_Text _coinsCount;

       protected override void OnBind()
        {
            base.OnBind();
            
            Model.SoftCurrencyHolder.AmountChanged.WithoutCurrent().ForEachAsync(CurrencyAmountChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();

            _coinsCount.text = Model.SoftCurrencyHolder.Get().ToString();
        }

        private void CurrencyAmountChanged(int newValue)
        {
            _coinsCount.text = newValue.ToString();
        }
    }
}
