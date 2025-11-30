using System.ComponentModel;
using CrossProject.Core.Actions;
using CrossProject.Core.Cheats;
using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public class GardenCheatOptions : ICheatOptions
    {
        private readonly ActionService _actionService;

        public GardenCheatOptions(ActionService actionService)
        {
            _actionService = actionService;
        }

        [Category("Garden")]
        [DisplayName("Activate garden beds")]
        public void ActivateGardenBeds()
        {
            _actionService.Execute(new ActivateGardenActionConfig()).Forget();
        }
    }
}
