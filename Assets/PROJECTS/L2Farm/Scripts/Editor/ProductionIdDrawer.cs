using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Quests;
using CrossProject.Editor.OdinEntities;
using L2Farm.Features.ResourceProduction;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class ProductionIdDrawer : AbstractOdinDropdownSelector<ProductionId>
    {
        protected override string PropertyName => "value";

        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(ProductionSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<IndicationTypeSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
