using System.Collections.Generic;
using CrossProject.Editor.OdinEntities;
using CrossProject.Core.Skins;

namespace PROJECTS.CrossProject.Editor
{
    public class SkinIdDrawer : AbstractOdinDropdownSelector<SkinId>
    {
        protected override string PropertyName => "value";

        protected override IEnumerable<string> GetNamesArray()
        {
            return new List<string>
            {
                "MC_Female_Default",
                "MC_Male_Default",
            };
        }
    }
}