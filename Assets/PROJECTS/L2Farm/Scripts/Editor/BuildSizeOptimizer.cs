using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace L2Farm.Editor
{
    public static class BuildSizeOptimizer
    {
        private static readonly (string path, int maxSize)[] HeavyFolders =
        {
            ("Assets/cemetery halloween set", 512),
            ("Assets/BridgesPack", 1024),
            ("Assets/fbx_and_textures_fantastic_village_pack", 1024),
            ("Assets/Charttyp", 1024),
            ("Assets/FantasyEnvironments", 1024),
            ("Assets/StrategyGameResourceIcons", 512),
        };

        private static readonly (string path, int maxSize)[] IndividualOverrides =
        {
            ("Assets/PROJECTS/L2Farm/Sprites/UiKit/Icon_Selector.png", 1024),
            ("Assets/PROJECTS/L2Farm/Materials/Materials/phongE1_baseColor.png", 1024),
            ("Assets/PROJECTS/L2Farm/Materials/Materials/phongE1_normal.png", 1024),
            ("Assets/PROJECTS/L2Farm/Materials/Materials/phongE1_emissive.png", 512),
            ("Assets/PROJECTS/L2Farm/Textures/Atlas_01.png", 1024),
            ("Assets/PROJECTS/L2Farm/Textures/Atlas_02.png", 1024),
        };

        [MenuItem("Tools/L2Farm/Optimize Build Size (Textures + Android Settings)")]
        public static void OptimizeAll()
        {
            TightenAndroidSettings();
            OptimizeTextures();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[BuildSizeOptimizer] Done. Rebuild the AAB to see new size.");
        }

        [MenuItem("Tools/L2Farm/Tighten Android Player Settings")]
        public static void TightenAndroidSettings()
        {
            var group = NamedBuildTarget.Android;
            // Low: safe for VContainer reflection (High breaks [Inject] and Construct is never called on IL2CPP builds).
            PlayerSettings.SetManagedStrippingLevel(group, ManagedStrippingLevel.Low);
            PlayerSettings.stripEngineCode = true;
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            PlayerSettings.SetScriptingBackend(group, ScriptingImplementation.IL2CPP);
            Debug.Log("[BuildSizeOptimizer] Android player settings tightened (strip Low, IL2CPP, engine code strip).");
        }

        [MenuItem("Tools/L2Farm/Optimize Third-Party Textures")]
        public static void OptimizeTextures()
        {
            var processed = 0;

            foreach (var (folder, maxSize) in HeavyFolders)
            {
                if (!Directory.Exists(folder))
                {
                    Debug.LogWarning($"[BuildSizeOptimizer] Folder missing: {folder}");
                    continue;
                }

                var guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (ApplyTextureCompression(path, maxSize))
                        processed++;
                }
            }

            foreach (var (path, maxSize) in IndividualOverrides)
            {
                if (!File.Exists(path))
                    continue;
                if (ApplyTextureCompression(path, maxSize))
                    processed++;
            }

            Debug.Log($"[BuildSizeOptimizer] Reimported {processed} textures with tightened Android compression.");
        }

        private static bool ApplyTextureCompression(string assetPath, int maxSize)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return false;

            var changed = false;

            if (importer.maxTextureSize > maxSize)
            {
                importer.maxTextureSize = maxSize;
                changed = true;
            }

            if (importer.textureCompression != TextureImporterCompression.CompressedHQ)
            {
                importer.textureCompression = TextureImporterCompression.Compressed;
                changed = true;
            }

            var android = importer.GetPlatformTextureSettings("Android");
            var wantFormat = TextureImporterFormat.ASTC_6x6;
            if (!android.overridden
                || android.format != wantFormat
                || android.maxTextureSize > maxSize
                || android.compressionQuality != (int)TextureCompressionQuality.Normal)
            {
                android.overridden = true;
                android.maxTextureSize = maxSize;
                android.format = wantFormat;
                android.compressionQuality = (int)TextureCompressionQuality.Normal;
                importer.SetPlatformTextureSettings(android);
                changed = true;
            }

            if (changed)
            {
                importer.SaveAndReimport();
                return true;
            }

            return false;
        }
    }
}
