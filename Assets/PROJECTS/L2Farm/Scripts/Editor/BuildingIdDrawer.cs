using System.Collections.Generic;
using System.Linq;
using CrossProject.Editor.OdinEntities;
using L2Farm.Features.Buildings;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class BuildingIdDrawer : AbstractOdinDropdownSelector<BuildingId>
    {
        protected override string PropertyName => "value";
        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(BuildingSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<BuildingSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
