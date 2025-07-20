using System.Collections.Generic;
using CrossProject.Core.Characters;
using CrossProject.Editor.OdinEntities;

namespace PROJECTS.L2Farm.Editor
{
    public class CharacterIdDrawer : AbstractOdinDropdownSelector<CharacterId>
    {
        protected override string PropertyName => "value";

        protected override IEnumerable<string> GetNamesArray()
        {
            return new List<string>
            {
                "MC_Female",
                "MC_Male"
            };
        }
    }
}