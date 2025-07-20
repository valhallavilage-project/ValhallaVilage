using System;
using UnityEditor;

namespace PROJECTS.CrossProject.Editor
{
    public class AssetsWatcher : AssetPostprocessor
    {
        public static event Action OnAssetsChanged;

        static AssetsWatcher()
        {
            Undo.undoRedoPerformed += () => OnAssetsChanged?.Invoke();
        }

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            OnAssetsChanged?.Invoke();
        }
    }
}