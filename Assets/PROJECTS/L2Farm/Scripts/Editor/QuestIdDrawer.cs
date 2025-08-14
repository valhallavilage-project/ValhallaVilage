using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Quests;
using CrossProject.Editor.OdinEntities;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class QuestIdDrawer : AbstractOdinDropdownSelector<QuestId>
    {
        protected override string PropertyName => "value";
        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(QuestSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<QuestSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
