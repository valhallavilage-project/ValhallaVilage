using CrossProject.Core.Actions;
using Sirenix.OdinInspector;

namespace L2Farm.Features.Buildings.Actions
{
    [System.Serializable, HideReferenceObjectPicker]
    public class UpgradeBuildingActionConfig : IActionConfig
    {
        public BuildingId buildingId;
    }
}
