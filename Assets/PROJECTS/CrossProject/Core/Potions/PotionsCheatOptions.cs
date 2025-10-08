using System.ComponentModel;
using CrossProject.Core.Cheats;
using CrossProject.Core.InGameResources;

namespace CrossProject.Core
{
    public class PotionsCheatOptions : ICheatOptions
    {
        private readonly ResourcesService _resourcesService;

        public PotionsCheatOptions(ResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }

        [Category("Potions")]
        [DisplayName("Add Heal potion")]
        public void AddHealPotion()
        {
            AddResource((ResourceId)"Resource_HealPotion");
        }

        [Category("Potions")]
        [DisplayName("Add Energy potion")]
        public void AddEnergyPotion()
        {
            AddResource((ResourceId)"Resource_EnergyPotion");
        }

        [Category("Potions")]
        [DisplayName("Add Time potion")]
        public void AddTimePotion()
        {
            AddResource((ResourceId)"Resource_TimePotion");
        }

        private void AddResource(ResourceId resource)
        {
            _resourcesService.IncreaseResourceValue(resource);
        }
    }
}
