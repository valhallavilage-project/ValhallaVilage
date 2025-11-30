using Cysharp.Threading.Tasks;

namespace CrossProject.Ui.Implementations
{
    public struct ConfirmPopupData
    {
        public string Text { get; }
        public ConfirmPopupButtonsType Buttons { get; }

        public ConfirmPopupData(string text, ConfirmPopupButtonsType buttons)
        {
            Text = text;
            Buttons = buttons;
        }
    }
    
    public interface IConfirmPopupOpenHandler
    {
        IReadOnlyAsyncReactiveProperty<ConfirmPopupData> Opened { get; }
        IReadOnlyAsyncReactiveProperty<bool> PopupResult { get; }

        void Open(ConfirmPopupData data);
        void SetPopupResult(bool result);
    }

    public class ConfirmPopupOpenHandler : IConfirmPopupOpenHandler
    {
        private readonly AsyncReactiveProperty<ConfirmPopupData> _opened = new(default);
        private readonly AsyncReactiveProperty<bool> _popupResult = new(default);

        public IReadOnlyAsyncReactiveProperty<ConfirmPopupData> Opened => _opened;
        public IReadOnlyAsyncReactiveProperty<bool> PopupResult => _popupResult;

        public void Open(ConfirmPopupData data)
        {
            _opened.Value = data;
        }

        public void SetPopupResult(bool result)
        {
            _popupResult.Value = result;
        }
    }
}
