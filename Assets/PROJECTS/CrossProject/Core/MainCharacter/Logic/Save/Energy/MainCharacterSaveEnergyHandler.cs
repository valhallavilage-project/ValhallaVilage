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
        private readonly IMainCharacterClothesSetsService _mainCharacterClothesSetsService;

        public MainCharacterSaveEnergyHandler(IRestoreEnergyHandler restoreHandler, IEnergyHandler parameterHandler,
            GameStateManager gameStateManager, EnergyRestorationConfig restorationConfig, ITimeService timeService,
            IMainCharacterClothesSetsService mainCharacterClothesSetsService)
            : base(parameterHandler, gameStateManager, restorationConfig, timeService)
        {
            _restoreHandler = restoreHandler;
            _parameterHandler = parameterHandler;
            _mainCharacterClothesSetsService = mainCharacterClothesSetsService;
        }

        public override async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _mainCharacterClothesSetsService.IsInitialized);

            await base.Initialize();
        }

        protected override float GetInitialParameterValue()
        {
            return _mainCharacterClothesSetsService.GetTotalEnergy();
        }

        protected override float GetMaxParameterValue()
        {
            return _mainCharacterClothesSetsService.GetTotalEnergy();
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
