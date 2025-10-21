using CrossProject.Core;
using CrossProject.Core.InGameResources;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    public class DropLootAbilityView : MonoBehaviour
    {
        [SerializeField] private ResourceId _loot;

        private IDieAbility _dieAbility;
        private ResourcesService _resourcesService;

        [Inject]
        private void AddDependencies(IDieAbility dieAbility, ResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
            dieAbility.DeathBegan.Listen(AddLoot, gameObject.GetCancellationTokenOnDestroy());
        }

        private void AddLoot()
        {
            _resourcesService.IncreaseResourceValue(_loot);
        }
    }
}
