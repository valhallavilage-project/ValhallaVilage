using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.SpawnPoints;
using CrossProject.Editor.OdinEntities;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class SceneIdDrawer : AbstractOdinDropdownSelector<SceneId>
    {
        protected override string PropertyName => "value";
        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(SceneSetConfig)}");
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneSetConfig>)
                .SelectMany(x => x.items);
        }
    }
}
