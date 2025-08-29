using System.Collections.Generic;
using System.Linq;
using CrossProject.Editor.OdinEntities;
using L2Farm.Features.Tools;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class ToolIdDrawer : AbstractOdinDropdownSelector<ToolId>
    {
        protected override string PropertyName => "value";
        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(ToolSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ToolSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
