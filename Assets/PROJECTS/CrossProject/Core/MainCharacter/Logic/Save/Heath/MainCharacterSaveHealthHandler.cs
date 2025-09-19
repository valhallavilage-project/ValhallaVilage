using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace CrossProject.Core
{
    public class MainCharacterSaveHealthHandler : BaseSaveRestorableParameterHandler<EnergyStatePart>, IMainCharacterSaveEnergyHandler
    {
        private readonly IRestoreHealthHandler _restoreHandler;
        private readonly IHealthHandler _parameterHandler;
        private readonly IMainCharacterClothesSetsService _mainCharacterClothesSetsService;

        public MainCharacterSaveHealthHandler(IRestoreHealthHandler restoreHandler, IHealthHandler parameterHandler,
            GameStateManager gameStateManager, HealthRestorationConfig restorationConfig, ITimeService timeService,
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
            return _mainCharacterClothesSetsService.GetTotalHealth();
        }

        protected override float GetMaxParameterValue()
        {
            return _mainCharacterClothesSetsService.GetTotalHealth();
        }

        protected override float GetMinParameterValue()
        {
            return 0;
        }

        protected override void SubscribeOnValueChanged()
        {
            _parameterHandler.Health.WithoutCurrent().ForEachAsync(v => ParameterChanged(v, _restoreHandler.LastRestoreTime), DisposeToken).Forget();
        }
    }
}
