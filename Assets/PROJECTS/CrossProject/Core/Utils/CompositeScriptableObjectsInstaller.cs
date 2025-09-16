using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = nameof(CompositeScriptableObjectsInstaller), menuName = "ScriptableObjects/CompositeInstaller")]
    public class CompositeScriptableObjectsInstaller : ScriptableObject
    {
        [SerializeField] private ScriptableObject[] _scriptableObjects;

        public void Install(IContainerBuilder builder)
        {
            foreach (var set in _scriptableObjects)
            {
                builder.RegisterInstance(set);
            }
        }

        #if UNITY_EDITOR
        [Button]
        private void CollectAllScriptableObjectsInFolder()
        {
            var path = AssetDatabase.GetAssetPath(this);
            var folderPath = Path.GetDirectoryName(path);

            var guids = AssetDatabase.FindAssets("t:ScriptableObject", new[]
            {
                folderPath
            });

            var scriptableObjects = new List<ScriptableObject>();

            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);

                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                if (asset == this)
                {
                    continue;
                }
                
                scriptableObjects.Add(asset);
            }

            _scriptableObjects = scriptableObjects.ToArray();
        }
        #endif
    }
}
