using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IMainCharacterGlobalArmorSetChangeHandler
    {
        IReadOnlyAsyncReactiveProperty<MainCharacterArmorSetType> ArmorSetChanged { get; }
        void ChangeSet(MainCharacterArmorSetType setType);
    }

    public class MainCharacterGlobalArmorSetChangeHandler : IMainCharacterGlobalArmorSetChangeHandler
    {
        private readonly AsyncReactiveProperty<MainCharacterArmorSetType> _armorSetChanged = new(default);

        public IReadOnlyAsyncReactiveProperty<MainCharacterArmorSetType> ArmorSetChanged => _armorSetChanged;
        
        public void ChangeSet(MainCharacterArmorSetType setType)
        {
            _armorSetChanged.Value = setType;
        }
    }
}
