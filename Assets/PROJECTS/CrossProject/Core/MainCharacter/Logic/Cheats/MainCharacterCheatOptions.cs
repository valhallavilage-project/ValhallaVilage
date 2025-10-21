using System;
using System.ComponentModel;
using System.Threading;
using CrossProject.Core.Cheats;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterCheatOptions : ICheatOptions, IInitializable, IDisposable
    {
        #if !DISABLE_SRDEBUGGER
        private readonly IHealthHandler _healthHandler;
        private bool _isImmortal;
        #endif

        private CancellationTokenSource _disposableCts = new();

        public bool IsInitialized { get; private set; }

        [Category("MainCharacter")]
        [DisplayName("Is Immortal")]
        public bool IsImmortal
        {
            get => _isImmortal;
        }

        #if !DISABLE_SRDEBUGGER
        public MainCharacterCheatOptions(IHealthHandler healthHandler)
        {
            _healthHandler = healthHandler;
            healthHandler.Health.WithoutCurrent().ForEachAsync(HealthChanged, _disposableCts.Token).Forget();
        }
        #endif

        public async UniTask Initialize()
        {
            #if !DISABLE_SRDEBUGGER
            SRDebug.Instance.AddOptionContainer(this);
            #endif
            IsInitialized = true;
        }

        #if !DISABLE_SRDEBUGGER
        private void HealthChanged(float value)
        {
            if (_isImmortal && value <= _healthHandler.MaxHealth.Value / 2)
            {
                UniTask.NextFrame().ContinueWith(() =>
                {
                    _healthHandler.Restore(_healthHandler.MaxHealth.Value);
                }).Forget();
            }
        }

        [Category("MainCharacter")]
        [DisplayName("Give MC Immortality")]
        public void GiveImmortality()
        {
            _isImmortal = true;
        }

        [Category("MainCharacter")]
        [DisplayName("Remove MC Immortality")]
        public void RemoveImmortality()
        {
            _isImmortal = false;
        }
        #endif

        public void Dispose()
        {
            _disposableCts.Cancel();
            _disposableCts.Dispose();
        }
    }
}
