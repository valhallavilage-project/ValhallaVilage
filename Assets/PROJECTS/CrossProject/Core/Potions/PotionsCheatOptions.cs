using System.ComponentModel;
using CrossProject.Core.Cheats;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    public class PotionsCheatOptions : ICheatOptions
    {
        private readonly GameStateManager _gameState;

        public PotionsCheatOptions(GameStateManager gameState)
        {
            _gameState = gameState;
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
            var part = _gameState.State.Get<ResourceContentPart>();

            if (part.Resources.ContainsKey(resource))
            {
                part.Resources[resource]++;
            }
            else
            {
                part.Resources[resource] = 1;
            }
        }
    }
}
