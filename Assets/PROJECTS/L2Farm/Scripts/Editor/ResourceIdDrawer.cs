using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.PROJECTS.CrossProject.Core.InGameResources;
using CrossProject.Editor.OdinEntities;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class ResourceIdDrawer : AbstractOdinDropdownSelector<ResourceId>
    {
        protected override string PropertyName => "value";
        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(ResourceSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ResourceSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
