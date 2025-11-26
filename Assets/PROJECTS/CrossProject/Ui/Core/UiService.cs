using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Ui.Core
{
    public class UiService : MonoBehaviour, IInitializable
    {
        [SerializeField] private RectTransform hudRoot;
        [SerializeField] private RectTransform screenRoot;
        [SerializeField] private RectTransform popupRoot;
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private LogInScreen logInScreen;

        private AddressablesManager _addressablesManager;

        //TODO : VM : load prefabs at the moment their models are pushed into queue
        private readonly List<IUiRule> _rules = new();

        public RectTransform HudRoot => hudRoot;
        public RectTransform ScreenRoot => screenRoot;
        public RectTransform PopupRoot => popupRoot;
        public bool IsInitialized { get; private set; }

        [Inject]
        private void Construct(AddressablesManager addressablesManager)
        {
            _addressablesManager = addressablesManager;
        }

        public async UniTask Initialize()
        {
            AddRule(new UiQueueRule<ScreenModel>(ScreenRoot, _addressablesManager));
            AddRule(new UiQueueRule<PopupModel>(PopupRoot, _addressablesManager));
            AddRule(new UiDictionaryRule<HudElementModel>(HudRoot, _addressablesManager));
            IsInitialized = true;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            ApplySafeAreaTo(hudRoot);
        }

        public LogInData LogInData => logInScreen.LogInData;

        public void ApplySafeAreaTo(RectTransform rectTransform, bool left = true, bool top = true, bool right = true, bool bottom = true)
        {
            var safeArea = Screen.safeArea;
            var screenSize = new Vector2(Screen.width, Screen.height);

            var targetMin = safeArea.position / screenSize;
            var targetMax = (safeArea.position + safeArea.size) / screenSize;
            if (left)
                rectTransform.anchorMin = new Vector2(targetMin.x, rectTransform.anchorMin.y);
            if (top)
                rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, targetMax.y);
            if (right)
                rectTransform.anchorMax = new Vector2(targetMax.x, rectTransform.anchorMax.y);
            if (bottom)
                rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, targetMin.y);
        }

        public void AddRule(IUiRule rule)
        {
            _rules.Add(rule);
        }

        public IUiRule GetRule(Type modelType)
        {
            var rule = _rules.FirstOrDefault(x => x.CanApply(modelType));

            if (rule == null)
                throw new Exception($"Can't find UiRule for : {modelType.Name}. Check parent for model and add new rule if needed.");

            return rule;
        }

        public IEnumerable<IUiView> Get<TUiModel>(Func<IUiView, bool> predicate = null) where TUiModel : UiModel
        {
            return GetRule(typeof(TUiModel)).Get<TUiModel>(predicate);
        }

        public IUiView GetFirst<TUiModel>(Func<IUiView, bool> predicate = null) where TUiModel : UiModel
        {
            return GetRule(typeof(TUiModel)).GetFirst<TUiModel>(predicate);
        }

        public async UniTask<IUiView> TryOpen(UiModel model)
        {
            return await GetRule(model.GetType()).Open(model);
        }

        public void Close(IUiView view)
        {
            GetRule(view.ModelType).Close(view);
            _addressablesManager.ReleaseInstance(view.AddressablesInstance);
        }

        public void RevealHudElement<THudElementModel>(Func<IUiView, bool> predicate = null) where THudElementModel : HudElementModel
        {
            //TODO : VM : alpha one
            var rule = GetRule(typeof(THudElementModel));
        }

        public void HideHudElement<THudElementModel>(Func<IUiView, bool> predicate = null) where THudElementModel : HudElementModel
        {
            //TODO : VM : alpha zero
            var rule = GetRule(typeof(THudElementModel));
        }

        public async UniTask<bool> LogIn()
        {
            return await logInScreen.LogIn();
        }

        public async UniTask Load(IReadOnlyList<UniTask> tasks)
        {
            loadingScreen.gameObject.SetActive(true);
            if (tasks.Count > 0)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    var progressValue = i / (float)tasks.Count;
                    await loadingScreen.UpdateProgress(progressValue);
                    await tasks[i];
                }
                await loadingScreen.UpdateProgress(1);
            }
            loadingScreen.gameObject.SetActive(false);
        }
    }
}