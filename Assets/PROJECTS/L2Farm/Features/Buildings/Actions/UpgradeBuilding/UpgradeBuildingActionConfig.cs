using CrossProject.Core.Actions;

namespace L2Farm.Features.Buildings.Actions
{
    [System.Serializable]
    public class UpgradeBuildingActionConfig : IActionConfig
    {
        public BuildingId buildingId;
    }
}
