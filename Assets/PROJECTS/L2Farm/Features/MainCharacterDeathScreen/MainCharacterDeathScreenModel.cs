using CrossProject.Core;
using CrossProject.Ui.Core;

namespace L2Farm.Features
{
    public class MainCharacterDeathScreenModel : ScreenModel
    {
        public IMainCharacterReviveGlobalHandler MainCharacterReviveHandler { get; set; }
    }
}
