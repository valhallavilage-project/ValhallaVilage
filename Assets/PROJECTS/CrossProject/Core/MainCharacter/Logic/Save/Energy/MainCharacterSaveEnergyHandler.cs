using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace CrossProject.Core
{
    public interface IMainCharacterSaveEnergyHandler : ISaveRestorableParameterHandler
    {
    }

    public class MainCharacterSaveEnergyHandler : BaseSaveRestorableParameterHandler<EnergyStatePart>, IMainCharacterSaveEnergyHandler
    {
        private readonly IRestoreEnergyHandler _restoreHandler;
        private readonly IEnergyHandler _parameterHandler;
        private readonly IMainCharacterArmorSetsService _mainCharacterArmorSetsService;

        public MainCharacterSaveEnergyHandler(IRestoreEnergyHandler restoreHandler, IEnergyHandler parameterHandler,
            GameStateManager gameStateManager, EnergyRestorationConfig restorationConfig, ITimeService timeService,
            IMainCharacterArmorSetsService mainCharacterArmorSetsService)
            : base(parameterHandler, gameStateManager, restorationConfig, timeService)
        {
            _restoreHandler = restoreHandler;
            _parameterHandler = parameterHandler;
            _mainCharacterArmorSetsService = mainCharacterArmorSetsService;
        }

        public override async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _mainCharacterArmorSetsService.IsInitialized);

            await base.Initialize();
        }

        protected override float GetInitialParameterValue()
        {
            return _mainCharacterArmorSetsService.GetTotalEnergy();
        }

        protected override float GetMaxParameterValue()
        {
            return _mainCharacterArmorSetsService.GetTotalEnergy();
        }

        protected override float GetMinParameterValue()
        {
            return 0;
        }

        protected override void SubscribeOnValueChanged()
        {
            _parameterHandler.Energy.WithoutCurrent().ForEachAsync(v => ParameterChanged(v, _restoreHandler.LastRestoreTime), DisposeToken).Forget();
        }
    }
}
