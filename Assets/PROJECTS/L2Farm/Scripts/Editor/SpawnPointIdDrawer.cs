using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.SpawnPoints;
using CrossProject.Editor.OdinEntities;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class SpawnPointIdDrawer : AbstractOdinDropdownSelector<SpawnPointId>
    {
        protected override string PropertyName => "value";
        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(SpawnPointSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SpawnPointSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
