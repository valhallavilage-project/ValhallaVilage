using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CrossProject.Ui.Core
{
    public interface IUiRule
    {
        bool CanApply(UiModel model);
        bool CanApply(IUiView view);

        UniTask<IUiView> Open(UiModel model);
        void Close(IUiView view);
        IUiView Hide(IUiView view);
        IUiView Reveal(IUiView view);
        IEnumerable<IUiView> Get<TUiModel>(Func<IUiView, bool> predicate = null) where TUiModel : UiModel;
        IUiView GetFirst<TUiModel>(Func<IUiView, bool> predicate = null) where TUiModel : UiModel;
    }
}