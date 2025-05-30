using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Ui.Core
{
    public abstract class UiRule<TUiModel, TUiView> : IUiRule where TUiModel : UiModel where TUiView : class, IUiView
    {
        protected readonly RectTransform root;
        protected readonly UiService uiService;

        public abstract event Action<IUiView> OnOpen;
        public abstract event Action<IUiView> OnClose;
        public abstract event Action<IUiView> OnHide;
        public abstract event Action<IUiView> OnReveal;

        public UiRule(
            RectTransform root,
            UiService uiService)
        {
            this.root = root;
            this.uiService = uiService;

            var type = typeof(TUiModel);
            if (!type.IsAbstract)
                throw new Exception("Create rules for abstract only models!");
        }

        public bool CanApply(UiModel model) => model is TUiModel;
        public bool CanApply(IUiView view) => view is TUiView;
        public abstract bool CanOpen(UiModel model);

        public abstract UniTask<IUiView> TryOpen(UiModel model);
        public abstract void Close(IUiView view);
        public abstract IUiView Hide();
        public abstract IUiView Reveal();
        public abstract IUiView Get<TUiModel1>(Func<IUiView, bool> predicate = null) where TUiModel1 : UiModel;
    }
}