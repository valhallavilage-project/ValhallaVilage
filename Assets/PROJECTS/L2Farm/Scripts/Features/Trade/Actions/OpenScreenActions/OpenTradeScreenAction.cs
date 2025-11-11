using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public class OpenTradeScreenAction : Action<OpenTradeScreenActionConfig>
    {
        private readonly IGlobalOpenTradeScreenHandler _openTradeScreenHandler;

        public OpenTradeScreenAction(IGlobalOpenTradeScreenHandler openTradeScreenHandler)
        {
            _openTradeScreenHandler = openTradeScreenHandler;
        }
        
        public override async UniTask Execute()
        {
            _openTradeScreenHandler.OpenScreen();
        }
    }
}
