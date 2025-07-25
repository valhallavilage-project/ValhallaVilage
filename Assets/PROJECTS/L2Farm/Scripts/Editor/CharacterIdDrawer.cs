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
            return assetUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<CharacterSetConfig>)
                .SelectMany(x => x.items)
                .Select(x => x.id);
        }
    }
}
