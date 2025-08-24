using System.Collections.Generic;
using System.Linq;
using CrossProject.Editor.OdinEntities;
using L2Farm.Features.NPC;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class NPCIdDdrawer : AbstractOdinDropdownSelector<NPCId>
    {
        protected override string PropertyName => "value";

        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(NPCSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<NPCSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id)
                .ToList();
        }
    }
}
