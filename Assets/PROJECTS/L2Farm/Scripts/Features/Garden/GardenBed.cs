using System.Threading;
using CrossProject.Core;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    public class GardenBed : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private GardenConfig _gardenConfig;
        [SerializeField] private GardenBedInteractiveObject _interactiveObject;
        [SerializeField] private GameObject _overgrow;
        [SerializeField] private string _notEnoughEnergyText;
        [SerializeField] private string _clearGardenText;

        private GlobalActivateGardenHandler _globalActivateGardenHandler;
        private GardenBedStateType _currentState;
        private GameStateManager _gameStateManager;
        private IConfirmPopupOpenHandler _confirmPopupOpenHandler;
        private CancellationTokenSource _clearGardenBedResultCts;
        private IMainCharacterGlobalFacade _mainCharacterGlobalFacade;
        private IMainCharacterGlobalCleanGardenBedHandler _globalCleanGardenBedHandler;

        [Inject]
        private void AddDependencies(IGlobalActivateGardenHandler globalActivateGardenHandler, GameStateManager gameStateManager,
            IConfirmPopupOpenHandler confirmPopupOpenHandler, IMainCharacterGlobalFacade mainCharacterGlobalFacade,
            IMainCharacterGlobalCleanGardenBedHandler globalCleanGardenBedHandler)
        {
            _globalCleanGardenBedHandler = globalCleanGardenBedHandler;
            _mainCharacterGlobalFacade = mainCharacterGlobalFacade;
            _confirmPopupOpenHandler = confirmPopupOpenHandler;
            _gameStateManager = gameStateManager;
            globalActivateGardenHandler.GardenBedsActivated.Listen(BedsActivated, gameObject.GetCancellationTokenOnDestroy());
        }

        private void Awake()
        {
            _interactiveObject.TryClear.Listen(TryToClearGardenBed, gameObject.GetCancellationTokenOnDestroy());

            var statePart = _gameStateManager.State.Get<GardenStatePart>();

            var state = statePart.GetGardenBedState(_id);

            ChangeState(state);
        }

        private void TryToClearGardenBed()
        {
            if (_mainCharacterGlobalFacade.CurrentEnergy.Value > _gardenConfig.GardenBedClearEnergy)
            {
                _clearGardenBedResultCts = new CancellationTokenSource();

                var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_clearGardenBedResultCts.Token, gameObject.GetCancellationTokenOnDestroy());

                _confirmPopupOpenHandler.PopupResult.WithoutCurrent().ForEachAsync(ClearPopupResultSet, linkedTokenSource.Token);

                _confirmPopupOpenHandler.Open(new ConfirmPopupData(string.Format(_clearGardenText, _gardenConfig.GardenBedClearEnergy), ConfirmPopupButtonsType.YesNo));
            }
            else
            {
                _confirmPopupOpenHandler.Open(new ConfirmPopupData(_notEnoughEnergyText, ConfirmPopupButtonsType.Ok));
            }
        }

        private void ClearPopupResultSet(bool result)
        {
            if (result)
            {
                _globalCleanGardenBedHandler.ClearGardenBed();

                _clearGardenBedResultCts.Cancel();
                _clearGardenBedResultCts.Dispose();

                ChangeState(GardenBedStateType.Clean);
            }
        }

        private void BedsActivated()
        {
            ChangeState(GardenBedStateType.Overgrown);
        }

        private void ChangeState(GardenBedStateType state)
        {
            var statePart = _gameStateManager.State.Get<GardenStatePart>();
            statePart.UpdateGardenBedState(_id, state);
            
            _gameStateManager.Save();

            _currentState = state;
            _interactiveObject.SetState(_currentState);

            _overgrow.SetActive(state is GardenBedStateType.Deactivated or GardenBedStateType.Overgrown);
            _interactiveObject.gameObject.SetActive(state != GardenBedStateType.Deactivated);
        }
    }
}
