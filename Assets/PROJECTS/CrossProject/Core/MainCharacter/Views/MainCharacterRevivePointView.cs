using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class MainCharacterRevivePointView : MonoBehaviour
    {
        private IMainCharacterReviveGlobalHandler _mainCharacterReviveGlobalHandler;

        [Inject]
        private void AddDependencies(IMainCharacterReviveGlobalHandler mainCharacterReviveGlobalHandler)
        {
            mainCharacterReviveGlobalHandler.InitRevivePoint(transform);
        }
    }
}
