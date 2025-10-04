using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm
{
    public class ConsumablesHudElement : HudElementView<ConsumablesHudElementModel>
    {
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _energyButton;
        [SerializeField] private Button _timeButton;

        private void Awake()
        {
            _healButton.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.ConsumeHealPotion(), gameObject.GetCancellationTokenOnDestroy()).Forget();
            _energyButton.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.ConsumeEnergyPotion(), gameObject.GetCancellationTokenOnDestroy()).Forget();
            _timeButton.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.ConsumeTimePotion(), gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        public void ConsumeHealPotion()
        {
            Debug.Log("Heal potion consumed");
        }

        public void ConsumeEnergyPotion()
        {
            Debug.Log("Energy potion consumed");
        }

        public void ConsumeTimePotion()
        {
            Debug.Log("Time potion consumed");
        }
    }
}
