using System;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TMPro;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    public class GardenBed : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private string _id;
        [SerializeField] private GardenConfig _gardenConfig;
        [Tooltip("If set, this state is used on first run (when no save data exists). Useful for test beds to skip activation.")]
        [SerializeField] private GardenBedStateType _initialStateIfNoSave = GardenBedStateType.Deactivated;
        [Tooltip("If true, always reset to _initialStateIfNoSave on every Play start (ignores saved state). For testing.")]
        [SerializeField] private bool _forceInitialStateEveryStart = false;

        [Header("Interaction")]
        [SerializeField] private GardenBedInteractiveObject _interactiveObject;
        [SerializeField] private string _notEnoughEnergyText = "Недостаточно энергии";
        [SerializeField] private string _clearGardenText = "Очистить грядку за {0} энергии?";

        [Header("Farming Settings")]
        [SerializeField] private float _growthTimeHours = 6f;
        [SerializeField] private float _decayTimeHours = 15f;
        [Tooltip("If > 0, overrides _growthTimeHours (used for testing)")]
        [SerializeField] private float _growthTimeSecondsOverride = 0f;
        [Tooltip("If > 0, overrides _decayTimeHours (used for testing)")]
        [SerializeField] private float _decayTimeSecondsOverride = 0f;
        [SerializeField] private string _seedsResourceId = "Resource_Seeds";
        [SerializeField] private int _seedsRequiredCount = 50;
        [SerializeField] private string _harvestResourceId = "Resource_TomatoSeeds";
        [SerializeField] private int _harvestCount = 20;
        [SerializeField] private float _plantingDurationSeconds = 3f;
        [Tooltip("If true, decayed plot becomes Overgrown (requires cleaning again). If false, becomes Empty.")]
        [SerializeField] private bool _decayBecomesOvergrown = true;

        [Header("Visuals")]
        [SerializeField] private GameObject _overgrow;
        [SerializeField] private GameObject _emptyModel;
        [SerializeField] private GameObject _growingModel;
        [SerializeField] private GameObject _readyModel;
        [SerializeField] private TextMeshPro _statusText;

        [Header("Localization")]
        [SerializeField] private string _notEnoughSeedsText = "Недостаточно семян";
        [SerializeField] private string _readyText = "Готово";
        [SerializeField] private string _plantingText = "Посадка...";

        private GardenBedStateType _currentState;
        private GameStateManager _gameStateManager;
        private GardenStatePart _gardenStatePart;
        private GardenBedState _detailedState;
        
        private IConfirmPopupOpenHandler _confirmPopupOpenHandler;
        private CancellationTokenSource _clearGardenBedResultCts;
        private IMainCharacterGlobalFacade _mainCharacterGlobalFacade;
        private IMainCharacterGlobalCleanGardenBedHandler _globalCleanGardenBedHandler;
        private IResourcesService _resourcesService;

        private bool _isInteracting;

        [Inject]
        private void AddDependencies(
            IGlobalActivateGardenHandler globalActivateGardenHandler, 
            GameStateManager gameStateManager,
            IConfirmPopupOpenHandler confirmPopupOpenHandler, 
            IMainCharacterGlobalFacade mainCharacterGlobalFacade,
            IMainCharacterGlobalCleanGardenBedHandler globalCleanGardenBedHandler,
            IResourcesService resourcesService)
        {
            _globalCleanGardenBedHandler = globalCleanGardenBedHandler;
            _mainCharacterGlobalFacade = mainCharacterGlobalFacade;
            _confirmPopupOpenHandler = confirmPopupOpenHandler;
            _gameStateManager = gameStateManager;
            _resourcesService = resourcesService;

            globalActivateGardenHandler.GardenBedsActivated.Listen(BedsActivated, gameObject.GetCancellationTokenOnDestroy());
        }

        private void Awake()
        {
            _interactiveObject.TryClear.Listen(TryToClearGardenBed, gameObject.GetCancellationTokenOnDestroy());
            _interactiveObject.OnInteracted.Listen(OnInteracted, gameObject.GetCancellationTokenOnDestroy());
        }

        private void Start()
        {
            AutoLinkModels();

            _gardenStatePart = _gameStateManager.State.Get<GardenStatePart>();

            bool hadSavedState = _gardenStatePart.DetailedStates.ContainsKey(_id);

            _detailedState = _gardenStatePart.GetDetailedState(_id);

            bool shouldForceInitial = _forceInitialStateEveryStart
                || (!hadSavedState && _initialStateIfNoSave != GardenBedStateType.Deactivated);

            if (shouldForceInitial)
            {
                _detailedState.State = _initialStateIfNoSave;
                _detailedState.StartTimeTicks = 0;
                _detailedState.FinishTimeTicks = 0;
                _gardenStatePart.UpdateGardenBedState(_id, _initialStateIfNoSave);
                Debug.Log($"[GardenBed:{_id}] Force initial state to {_initialStateIfNoSave}");
            }

            _currentState = _detailedState.State;

            RefreshVisuals();
        }

        private void AutoLinkModels()
        {
            if (_emptyModel == null)   _emptyModel   = transform.Find("Garden_Empty")?.gameObject;
            if (_growingModel == null) _growingModel = transform.Find("Garden_Growing")?.gameObject;
            if (_readyModel == null)   _readyModel   = transform.Find("Garden_Ready")?.gameObject;
            if (_overgrow == null)     _overgrow     = transform.Find("Leafs_pile_big")?.gameObject;
            if (_statusText == null)
            {
                var statusGo = transform.Find("StatusText");
                if (statusGo != null) _statusText = statusGo.GetComponent<TextMeshPro>();
            }

            Debug.Log($"[GardenBed:{_id}] AutoLinkModels: overgrow={(_overgrow!=null)} empty={(_emptyModel!=null)} growing={(_growingModel!=null)} ready={(_readyModel!=null)} status={(_statusText!=null)}");
        }

        private void OnInteracted()
        {
            if (_isInteracting) return;

            switch (_currentState)
            {
                case GardenBedStateType.Clean:
                case GardenBedStateType.Empty:
                    TryPlant();
                    break;
                case GardenBedStateType.Ready:
                    Harvest();
                    break;
            }
        }

        #region Clearing Logic (Energy)

        private void TryToClearGardenBed()
        {
            Debug.Log($"[GardenBed:{_id}] TryToClearGardenBed state={_currentState} facade={(_mainCharacterGlobalFacade != null)} popup={(_confirmPopupOpenHandler != null)} config={(_gardenConfig != null)}");

            if (_currentState != GardenBedStateType.Overgrown)
            {
                Debug.LogWarning($"[GardenBed:{_id}] Cannot clear - state is not Overgrown ({_currentState})");
                return;
            }

            if (_mainCharacterGlobalFacade == null || _confirmPopupOpenHandler == null || _gardenConfig == null)
            {
                Debug.LogError($"[GardenBed:{_id}] Null dependency: facade={_mainCharacterGlobalFacade}, popup={_confirmPopupOpenHandler}, config={_gardenConfig}");
                return;
            }

            var currentEnergy = _mainCharacterGlobalFacade.CurrentEnergy.Value;
            var required = _gardenConfig.GardenBedClearEnergy;
            Debug.Log($"[GardenBed:{_id}] Energy={currentEnergy} required={required}");

            if (currentEnergy >= required)
            {
                _clearGardenBedResultCts = new CancellationTokenSource();
                var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_clearGardenBedResultCts.Token, gameObject.GetCancellationTokenOnDestroy());

                _confirmPopupOpenHandler.PopupResult.WithoutCurrent().ForEachAsync(ClearPopupResultSet, linkedTokenSource.Token).Forget();

                _confirmPopupOpenHandler.Open(new ConfirmPopupData(string.Format(_clearGardenText, _gardenConfig.GardenBedClearEnergy), ConfirmPopupButtonsType.YesNo));
                Debug.Log($"[GardenBed:{_id}] Confirm popup opened");
            }
            else
            {
                _confirmPopupOpenHandler.Open(new ConfirmPopupData(_notEnoughEnergyText, ConfirmPopupButtonsType.Ok));
                Debug.Log($"[GardenBed:{_id}] Not enough energy popup opened");
            }
        }

        private void ClearPopupResultSet(bool result)
        {
            if (result)
            {
                _globalCleanGardenBedHandler.ClearGardenBed();

                _clearGardenBedResultCts?.Cancel();
                _clearGardenBedResultCts?.Dispose();
                _clearGardenBedResultCts = null;

                ChangeState(GardenBedStateType.Clean);
            }
        }

        private void BedsActivated()
        {
            ChangeState(GardenBedStateType.Overgrown);
        }

        #endregion

        #region Farming Logic (Seeds & Growth)

        private void TryPlant()
        {
            var seedsId = new ResourceId(_seedsResourceId);
            var currentSeeds = _gameStateManager.State.Get<ResourceContentPart>().Get(seedsId);
            Debug.Log($"[GardenBed:{_id}] TryPlant currentSeeds={currentSeeds} required={_seedsRequiredCount}");

            if (currentSeeds >= _seedsRequiredCount)
            {
                Debug.Log($"[GardenBed:{_id}] Enough seeds, starting planting...");
                StartPlanting().Forget();
            }
            else
            {
                Debug.Log($"[GardenBed:{_id}] Not enough seeds");
                ShowTemporaryStatus(_notEnoughSeedsText).Forget();
            }
        }

        private async UniTaskVoid StartPlanting()
        {
            _isInteracting = true;
            if (_statusText != null)
            {
                _statusText.text = _plantingText;
                _statusText.gameObject.SetActive(true);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_plantingDurationSeconds), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

            Debug.Log($"[GardenBed:{_id}] Consuming {_seedsRequiredCount} {_seedsResourceId}");
            _resourcesService.ChangeResource(new ResourceId(_seedsResourceId), -_seedsRequiredCount);

            _detailedState.StartTimeTicks = DateTime.UtcNow.Ticks;
            _detailedState.FinishTimeTicks = DateTime.UtcNow.Add(GetGrowthDuration()).Ticks;
            _detailedState.ResourceId = _harvestResourceId;
            _detailedState.Amount = _harvestCount;

            _isInteracting = false;
            ChangeState(GardenBedStateType.Growing);
        }

        private TimeSpan GetGrowthDuration()
        {
            return _growthTimeSecondsOverride > 0
                ? TimeSpan.FromSeconds(_growthTimeSecondsOverride)
                : TimeSpan.FromHours(_growthTimeHours);
        }

        private TimeSpan GetDecayDuration()
        {
            return _decayTimeSecondsOverride > 0
                ? TimeSpan.FromSeconds(_decayTimeSecondsOverride)
                : TimeSpan.FromHours(_decayTimeHours);
        }

        private void Harvest()
        {
            _resourcesService.ChangeResource(new ResourceId(_detailedState.ResourceId), _detailedState.Amount);
            ResetPlot();
        }

        private void ResetPlot()
        {
            _detailedState.StartTimeTicks = 0;
            _detailedState.FinishTimeTicks = 0;
            ChangeState(GardenBedStateType.Empty);
        }

        private void DecayPlot()
        {
            _detailedState.StartTimeTicks = 0;
            _detailedState.FinishTimeTicks = 0;
            ChangeState(_decayBecomesOvergrown ? GardenBedStateType.Overgrown : GardenBedStateType.Empty);
        }

        private void Update()
        {
            if (_isInteracting) return;

            if (_currentState == GardenBedStateType.Growing)
            {
                var now = DateTime.UtcNow;
                var finish = new DateTime(_detailedState.FinishTimeTicks);

                if (now >= finish)
                {
                    _detailedState.StartTimeTicks = now.Ticks; // Decay start
                    _detailedState.FinishTimeTicks = now.Add(GetDecayDuration()).Ticks;
                    ChangeState(GardenBedStateType.Ready);
                }
                else
                {
                    UpdateTimerText(finish - now);
                }
            }
            else if (_currentState == GardenBedStateType.Ready)
            {
                var now = DateTime.UtcNow;
                var decayFinish = new DateTime(_detailedState.FinishTimeTicks);

                if (now >= decayFinish)
                {
                    DecayPlot();
                }
                else
                {
                    if (_statusText != null)
                    {
                        _statusText.text = _readyText;
                        _statusText.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void UpdateTimerText(TimeSpan timeLeft)
        {
            if (_statusText != null)
            {
                _statusText.text = $"{(int)timeLeft.TotalHours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
                _statusText.gameObject.SetActive(true);
            }
        }

        private async UniTaskVoid ShowTemporaryStatus(string text)
        {
            if (_statusText == null) return;

            _statusText.text = text;
            _statusText.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            
            if (_currentState is GardenBedStateType.Empty or GardenBedStateType.Clean or GardenBedStateType.Overgrown or GardenBedStateType.Deactivated)
            {
                _statusText.gameObject.SetActive(false);
            }
            else
            {
                RefreshVisuals();
            }
        }

        #endregion

        private void ChangeState(GardenBedStateType state)
        {
            _currentState = state;
            _detailedState.State = state;
            
            // Sync with GardenStatePart (which has both Dictionary and DetailedStates)
            _gardenStatePart.UpdateGardenBedState(_id, state);
            
            _gameStateManager.Save();
            RefreshVisuals();
        }

        private void RefreshVisuals()
        {
            if (_overgrow != null) _overgrow.SetActive(_currentState is GardenBedStateType.Deactivated or GardenBedStateType.Overgrown);
            if (_emptyModel != null) _emptyModel.SetActive(_currentState is GardenBedStateType.Empty or GardenBedStateType.Clean);
            if (_growingModel != null) _growingModel.SetActive(_currentState == GardenBedStateType.Growing);
            if (_readyModel != null) _readyModel.SetActive(_currentState == GardenBedStateType.Ready);

            _interactiveObject.SetState(_currentState);

            if (_statusText != null && _currentState != GardenBedStateType.Growing && _currentState != GardenBedStateType.Ready && !_isInteracting)
            {
                _statusText.gameObject.SetActive(false);
            }
        }
    }
}
