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

        private AddressablesManager _addressablesManager;

        //TODO : VM : load prefabs at the moment their models are pushed into queue
        private readonly List<IUiRule> _rules = new();

        public RectTransform HudRoot => hudRoot;
        public RectTransform ScreenRoot => screenRoot;
        public RectTransform PopupRoot => popupRoot;

        [Inject]
        private void Construct(AddressablesManager addressablesManager)
        {
            _addressablesManager = addressablesManager;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            ApplySafeAreaTo(hudRoot);
            ApplySafeAreaTo(popupRoot);
        }

        public void Initialize()
        {
            AddRule(new UiQueue<ScreenModel>(ScreenRoot, _addressablesManager));
            AddRule(new UiQueue<PopupModel>(PopupRoot, _addressablesManager));
            AddRule(new UiDictionary<HudElementModel>(HudRoot, _addressablesManager));
        }

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

        private IUiRule GetRule(Type modelType)
        {
            var rule = _rules.FirstOrDefault(x => x.CanApply(modelType));

            if (rule == null)
                throw new Exception($"Can't find UiRule for : {modelType.Name}. Check parent for model and add new rule if needed.");

            return rule;
        }

        private async UniTask<IUiView> Open(UiModel model, RectTransform root)
        {
            string key = model.GetType().Name[..^5];
            var prefab = await _addressablesManager.LoadAssetAsync<GameObject>(key);
            var instance = Instantiate(prefab, root);
            instance.name = key;
            var uiView = instance.GetComponent<IUiView>();
            uiView.BindModel(model);
            uiView.OnShow();
            return uiView;
        }

        public IUiView Get<TUiModel>(UiModel model, Func<IUiView, bool> predicate = null) where TUiModel : UiModel
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
            //OnHudElementReveal?.Invoke(null);
            //TODO : VM : alpha one
        }

        public void HideHudElement<THudElementModel>(Func<IUiView, bool> predicate = null) where THudElementModel : HudElementModel
        {
            //OnHudElementHide?.Invoke(null);
            //TODO : VM : alpha zero
        }
    }
}