using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Ui.Core
{
    public interface IUiRule
    {
        bool CanApply(UiModel model);
        bool CanApply(IUiView view);
        bool CanOpen(UiModel model);

        UniTask<IUiView> TryOpen(UiModel model);
        void Close(IUiView view);
        IUiView Hide();
        IUiView Reveal();
        IUiView Get<TUiModel>(Func<IUiView, bool> predicate = null) where TUiModel : UiModel;
    }
}