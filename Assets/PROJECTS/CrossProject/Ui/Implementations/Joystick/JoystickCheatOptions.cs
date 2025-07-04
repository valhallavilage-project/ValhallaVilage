using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CrossProject.Core.Cheats;
using CrossProject.Core.SimpleMovement;
using Cysharp.Threading.Tasks;

namespace CrossProject.Ui.Implementations
{
    public class JoystickCheatOptions : ICheatOptions, INotifyPropertyChanged
    {
        private readonly IJoystickValueProvider _joystickValueProvider;

        public event PropertyChangedEventHandler PropertyChanged;

        public JoystickCheatOptions(IJoystickValueProvider joystickValueProvider)
        {
            _joystickValueProvider = joystickValueProvider;
            TimerRoutine().Forget();
        }

        [Category("Joystick")]
        [DisplayName("X Value")]
        public float XValue
        {
            get => _joystickValueProvider.NormalizedVector3OnPlain.x;
        }

        [Category("Joystick")]
        [DisplayName("Y Value")]
        public float YValue
        {
            get => _joystickValueProvider.NormalizedVector3OnPlain.z;
        }

        private async UniTask TimerRoutine()
        {
            while (true)
            {
                OnPropertyChanged(nameof(XValue));
                OnPropertyChanged(nameof(YValue));
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}