using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CrossProject.Core.Cheats;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core.SimpleMovement
{
    public class SimpleMovementCheatOptions : ICheatOptions, INotifyPropertyChanged
    {
        private readonly IPlayerVelocityProvider _playerVelocityProvider;

        public event PropertyChangedEventHandler PropertyChanged;

        public SimpleMovementCheatOptions(IPlayerVelocityProvider playerVelocityProvider)
        {
            _playerVelocityProvider = playerVelocityProvider;
            TimerRoutine().Forget();
        }

        [Category("Movement")]
        [DisplayName("PlayerVelocity")]
        public float Velocity => _playerVelocityProvider.Velocity.magnitude;

        [Category("Movement")]
        [DisplayName("Direction X")]
        public float DirectionX => _playerVelocityProvider.Direction.x;

        [Category("Movement")]
        [DisplayName("Direction Z")]
        public float DirectionZ => _playerVelocityProvider.Direction.z;

        private async UniTask TimerRoutine()
        {
            while (true)
            {
                OnPropertyChanged(nameof(Velocity));
                OnPropertyChanged(nameof(DirectionX));
                OnPropertyChanged(nameof(DirectionZ));
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}