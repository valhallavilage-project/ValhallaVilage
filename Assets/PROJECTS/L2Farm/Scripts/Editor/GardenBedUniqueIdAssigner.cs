using L2Farm;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace L2Farm.Editor
{
    public static class GardenBedUniqueIdAssigner
    {
        private const string ScenePath = "Assets/PROJECTS/L2Farm/Scenes/L2Farm_FirstTown.unity";

        [MenuItem("Tools/L2Farm/Assign Unique GardenBed IDs")]
        public static void AssignUniqueIds()
        {
            var scene = EditorSceneManager.GetActiveScene();
            if (!scene.IsValid() || scene.path != ScenePath)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                else
                    return;
            }

            var beds = Object.FindObjectsOfType<GardenBed>(includeInactive: true);
            if (beds.Length == 0)
            {
                Debug.LogWarning("[GardenBedUniqueIdAssigner] No GardenBed components found in scene.");
                return;
            }

            int changed = 0;
            for (int i = 0; i < beds.Length; i++)
            {
                var so = new SerializedObject(beds[i]);
                var idProp = so.FindProperty("_id");
                var newId = $"GardenBed_{i:D2}";
                if (idProp.stringValue != newId)
                {
                    idProp.stringValue = newId;
                    so.ApplyModifiedProperties();
                    changed++;
                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log($"[GardenBedUniqueIdAssigner] Updated {changed}/{beds.Length} GardenBed _id values in {scene.name}.");
        }
    }
}
