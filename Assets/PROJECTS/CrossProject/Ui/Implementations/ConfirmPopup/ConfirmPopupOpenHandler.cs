using Cysharp.Threading.Tasks;

namespace CrossProject.Ui.Implementations
{
    public interface IConfirmPopupOpenHandler
    {
        IReadOnlyAsyncReactiveProperty<string> Opened { get; }

        void Open(string text);
    }

    public class ConfirmPopupOpenHandler : IConfirmPopupOpenHandler
    {
        private readonly AsyncReactiveProperty<string> _opened = new(default);

        public IReadOnlyAsyncReactiveProperty<string> Opened => _opened;

        public void Open(string text)
        {
            _opened.Value = text;
        }
    }
}
