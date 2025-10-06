using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IMainCharacterGlobalPotionConsumeHandler
    {
        IReadOnlyAsyncReactiveProperty<PotionType> PotionConsumed { get; }
        void ConsumePotion(PotionType potion);
    }

    public class MainCharacterGlobalPotionConsumeHandler : IMainCharacterGlobalPotionConsumeHandler
    {
        private readonly AsyncReactiveProperty<PotionType> _potionConsumed = new(default);

        public IReadOnlyAsyncReactiveProperty<PotionType> PotionConsumed => _potionConsumed;

        public void ConsumePotion(PotionType potion)
        {
            _potionConsumed.Value = potion;
        }
    }
}
