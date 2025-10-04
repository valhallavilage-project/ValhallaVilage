using System;
using CrossProject.Core.InGameResources;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public class ConsumablesHudElementModel : HudElementModel
    {
        private readonly AsyncReactiveProperty<bool> _healClicked = new(default);
        private readonly AsyncReactiveProperty<bool> _energyClicked = new(default);
        private readonly AsyncReactiveProperty<bool> _timeClicked = new(default);

        public IReadOnlyAsyncReactiveProperty<bool> HealClicked => _healClicked;
        public IReadOnlyAsyncReactiveProperty<bool> EnergyClicked => _energyClicked;
        public IReadOnlyAsyncReactiveProperty<bool> TimeClicked => _timeClicked;
        public ResourceContentPart Resources { get; set; }

        public void ConsumeHealPotion()
        {
            _healClicked.Value = true;
        }

        public void ConsumeEnergyPotion()
        {
            _energyClicked.Value = true;
        }

        public void ConsumeTimePotion()
        {
            _timeClicked.Value = true;
        }
    }
}
