using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace CrossProject.Core
{
    public interface IMainCharacterSaveHealthHandler : ISaveRestorableParameterHandler
    {
    }

    public class MainCharacterSaveHealthHandler : BaseSaveRestorableParameterHandler<HealthStatePart>, IMainCharacterSaveHealthHandler
    {
        private readonly IRestoreHealthHandler _restoreHandler;
        private readonly IHealthHandler _parameterHandler;
        private readonly IMainCharacterArmorSetsService _mainCharacterArmorSetsService;

        public MainCharacterSaveHealthHandler(IRestoreHealthHandler restoreHandler, IHealthHandler parameterHandler,
            GameStateManager gameStateManager, HealthRestorationConfig restorationConfig, ITimeService timeService,
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
            return _mainCharacterArmorSetsService.GetTotalHealth();
        }

        protected override float GetMaxParameterValue()
        {
            return _mainCharacterArmorSetsService.GetTotalHealth();
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
