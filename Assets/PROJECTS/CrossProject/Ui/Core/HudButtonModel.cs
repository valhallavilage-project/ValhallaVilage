using Cysharp.Threading.Tasks;

namespace CrossProject.Ui.Core
{
    public class HudButtonModel : HudElementModel
    {
        private readonly AsyncReactiveProperty<Invoker> _clicked = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> Clicked => _clicked;

        public void Click()
        {
            _clicked.Invoke(); 
        }
    }
}
