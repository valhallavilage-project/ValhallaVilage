using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IMainCharacterSharedDataHandler
    {
    }

    public class MainCharacterSharedDataHandler : IMainCharacterSharedDataHandler, IInitializable
    {
        public bool IsInitialized { get; private set; }

        public MainCharacterSharedDataHandler(IMainCharacterSharedDataHolder holder, IHealthHandler healthHandler, IEnergyHandler energyHandler)
        {
            healthHandler.Health.ForEachAsync(v => holder.CurrentHealth.Value = v).Forget();
            healthHandler.MaxHealth.ForEachAsync(v => holder.MaxHealth.Value = v).Forget();
            energyHandler.Energy.ForEachAsync(v => holder.CurrentEnergy.Value = v).Forget();
            energyHandler.MaxEnergy.ForEachAsync(v => holder.MaxEnergy.Value = v).Forget();
        }

        public UniTask Initialize()
        {
            IsInitialized = true;

            return UniTask.CompletedTask;
        }
    }
}
