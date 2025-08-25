using System.Collections.Generic;
using System.Linq;
using CrossProject.Editor.OdinEntities;
using L2Farm.Features.QuestIndication;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class IndicationTypeIdDrawer : AbstractOdinDropdownSelector<IndicationTypeId>
    {
        protected override string PropertyName => "value";

        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(IndicationTypeSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<IndicationTypeSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
