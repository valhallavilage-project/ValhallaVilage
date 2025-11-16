using CrossProject.Core;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;

namespace L2Farm
{
    public class TradeScreenModel : ScreenModel
    {
        public GameStateManager GameStateManager { get; set; }
        public ITimeService TimeService { get; set; }
    }
}
