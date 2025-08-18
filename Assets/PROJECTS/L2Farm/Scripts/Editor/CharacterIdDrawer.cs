using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Characters;
using CrossProject.Editor.OdinEntities;
using UnityEditor;

namespace PROJECTS.L2Farm.Editor
{
    public class CharacterIdDrawer : AbstractOdinDropdownSelector<CharacterId>
    {
        protected override string PropertyName => "value";

        protected override IEnumerable<string> GetNamesArray()
        {
            string[] assetUIDs = AssetDatabase.FindAssets($"t:{nameof(CharacterSetConfig)}");
            var ids = assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<CharacterSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id)
                .ToList();
            var result = new List<string> { "MC" };
            result.AddRange(ids);
            return result;
        }
    }
}
