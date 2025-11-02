using System;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class MainCharacterAudioView : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioData _walkSound;
        [SerializeField] private AudioData _attackSound;
        [SerializeField] private AudioData _deathSound;
        [SerializeField] private AudioData _gatherSound;
        [SerializeField] private AudioData _axeSound;
        [SerializeField] private AudioData _pickAxeSound;

        [SerializeField] private float _attackSoundOffset;
        
        private IAudioService _audioService;

        [Inject]
        private void AddDependencies(IMainCharacterMovingHandler movingHandler, IAttackAbility attackAbility, 
            IDieAbility dieAbility, IInteractionHandler interactionHandler, IAudioService audioService)
        {
            _audioService = audioService;

            audioService.Init(_audioSource);
            dieAbility.DeathBegan.Listen(DeathBegan, gameObject.GetCancellationTokenOnDestroy());
            attackAbility.AttackBegin.Listen(AttackBegan, gameObject.GetCancellationTokenOnDestroy());
            movingHandler.MoveBegan.Listen(MoveBegan, gameObject.GetCancellationTokenOnDestroy());
            movingHandler.MoveStopped.Listen(MoveStopped, gameObject.GetCancellationTokenOnDestroy());
            
            interactionHandler.InteractionStarted.WithoutCurrent().ForEachAsync(InteractionStarted, gameObject.GetCancellationTokenOnDestroy()).Forget();
            interactionHandler.InteractionFinished.WithoutCurrent().ForEachAsync(InteractionFinished, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask AttackBegan()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_attackSoundOffset));
            
            _audioService.Play(_attackSound);
        }

        private void DeathBegan()
        {
            _audioService.Play(_deathSound);
        }

        private void MoveBegan()
        {
            _audioService.Play(_walkSound);
        }

        private void MoveStopped()
        {
            _audioService.Stop();
        }

        private void InteractionStarted(InteractionType interactionType)
        {
            switch (interactionType)
            {
                case InteractionType.Chop:
                    _audioService.Play(_axeSound);
                    break;
                case InteractionType.Pickaxe:
                    _audioService.Play(_pickAxeSound);
                    break;
                case InteractionType.Gather:
                    _audioService.Play(_gatherSound);
                    break;
            }
        }

        private void InteractionFinished(InteractionType _)
        {
            _audioService.Stop();
        }
    }
}
