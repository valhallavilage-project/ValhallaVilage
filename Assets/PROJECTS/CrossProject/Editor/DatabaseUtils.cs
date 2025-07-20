using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PROJECTS.CrossProject.Editor
{
    public static class DatabaseUtils
    {
        [MenuItem("Assets/Reserialize", false, 40)]
        public static void Reserialize()
        {
            var assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            var paths = GetPaths(assets);
            paths = ExcludeFolderAndCSharpFiles(paths);

            AssetDatabase.ForceReserializeAssets(paths.ToList());
        }

        [MenuItem("Assets/Reserialize", true)]
        public static bool IsReserializeAvailable()
        {
            var assets = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            var paths = GetPaths(assets);
            return paths.Any();
        }

        private static IEnumerable<string> GetPaths(Object[] assets)
        {
            foreach (var asset in assets)
            {
                var path = AssetDatabase.GetAssetPath(asset);

                if (string.IsNullOrEmpty(path))
                    continue;

                yield return path;
            }
        }

        private static IEnumerable<string> ExcludeFolderAndCSharpFiles(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                if (AssetDatabase.IsValidFolder(path))
                    continue;

                if (path.EndsWith(".cs"))
                    continue;

                yield return path;
            }
        }
    }
}
