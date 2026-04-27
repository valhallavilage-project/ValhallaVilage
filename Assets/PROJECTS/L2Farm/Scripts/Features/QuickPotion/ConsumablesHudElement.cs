using CrossProject.Core.InGameResources;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm
{
    public class ConsumablesHudElement : HudElementView<ConsumablesHudElementModel>
    {
        [SerializeField] private Button _healButton;
        [SerializeField] private TMP_Text _healPotionsAmount;
        [SerializeField] private Button _energyButton;
        [SerializeField] private TMP_Text _energyPotionsAmount;
        [SerializeField] private Button _timeButton;
        [SerializeField] private TMP_Text _timePotionsAmount;
        [SerializeField] private Button _fertilizerButton;
        [SerializeField] private TMP_Text _fertilizerAmount;

        private void Awake()
        {
            _healButton.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.ConsumeHealPotion(), gameObject.GetCancellationTokenOnDestroy()).Forget();
            _energyButton.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.ConsumeEnergyPotion(), gameObject.GetCancellationTokenOnDestroy()).Forget();
            _timeButton.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.ConsumeTimePotion(), gameObject.GetCancellationTokenOnDestroy()).Forget();
            if (_fertilizerButton != null)
                _fertilizerButton.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.ConsumeFertilizer(), gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        protected override void OnBind()
        {
            base.OnBind();

            UpdateResourceTexts();
        }

        public void UpdateResourceTexts()
        {
            UpdateResourceText(_healPotionsAmount, "Resource_HealPotion");
            UpdateResourceText(_energyPotionsAmount, "Resource_EnergyPotion");
            UpdateResourceText(_timePotionsAmount, "Resource_TimePotion");
            UpdateResourceText(_fertilizerAmount, "Resource_Fertilizer");
        }

        public void HealPotionConsumed()
        {
            UpdateResourceText(_healPotionsAmount, "Resource_HealPotion");
        }

        public void ConsumeEnergyPotion()
        {
            UpdateResourceText(_energyPotionsAmount, "Resource_EnergyPotion");
        }

        public void ConsumeTimePotion()
        {
            UpdateResourceText(_timePotionsAmount, "Resource_TimePotion");
        }

        public void FertilizerConsumed()
        {
            UpdateResourceText(_fertilizerAmount, "Resource_Fertilizer");
        }

        private void UpdateResourceText(TMP_Text textRender, string resourceId)
        {
            if (textRender == null) return;
            textRender.text = Model.Resources.Get((ResourceId)resourceId).ToString();
        }
    }
}
