using L2Farm;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Editor
{
    public static class ConsumablesHudFertilizerButton
    {
        private const string HudPrefabPath = "Assets/PROJECTS/L2Farm/Prefabs/Ui/Buttons/ConsumablesHudElement.prefab";
        private const string BaseButtonPrefabPath = "Assets/PROJECTS/L2Farm/Prefabs/Ui/Buttons/BaseQuickPotionButton.prefab";
        private const string BucketSpritePath = "Assets/StrategyGameResourceIcons/Icons/bucket.png";

        [MenuItem("Tools/L2Farm/Add Fertilizer Button To Consumables HUD")]
        public static void AddFertilizerButton()
        {
            var hudRoot = PrefabUtility.LoadPrefabContents(HudPrefabPath);
            if (hudRoot == null)
            {
                Debug.LogError($"Failed to load prefab at {HudPrefabPath}");
                return;
            }

            try
            {
                var hudComp = hudRoot.GetComponent<ConsumablesHudElement>();
                if (hudComp == null)
                {
                    Debug.LogError("ConsumablesHudElement component not found on prefab root.");
                    return;
                }

                var hudSo = new SerializedObject(hudComp);
                var fertBtnProp = hudSo.FindProperty("_fertilizerButton");
                if (fertBtnProp == null)
                {
                    Debug.LogError("_fertilizerButton field not found. Recompile scripts first.");
                    return;
                }

                Transform existing = null;
                foreach (Transform child in hudRoot.transform)
                {
                    if (child.name == "FertilizerQuickPotionButton")
                    {
                        existing = child;
                        break;
                    }
                }

                GameObject fertBtnGo;
                if (existing != null)
                {
                    fertBtnGo = existing.gameObject;
                }
                else
                {
                    var baseBtnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(BaseButtonPrefabPath);
                    if (baseBtnPrefab == null)
                    {
                        Debug.LogError($"Base button prefab not found at {BaseButtonPrefabPath}");
                        return;
                    }

                    fertBtnGo = (GameObject)PrefabUtility.InstantiatePrefab(baseBtnPrefab, hudRoot.transform);
                    fertBtnGo.name = "FertilizerQuickPotionButton";
                }

                var rt = fertBtnGo.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(100, 100);
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;

                var iconTr = fertBtnGo.transform.Find("Icon");
                if (iconTr != null)
                {
                    var iconImg = iconTr.GetComponent<Image>();
                    var bucketSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BucketSpritePath);
                    if (iconImg != null && bucketSprite != null)
                    {
                        var iconSo = new SerializedObject(iconImg);
                        iconSo.FindProperty("m_Sprite").objectReferenceValue = bucketSprite;
                        iconSo.ApplyModifiedProperties();
                    }
                }

                var btnComp = fertBtnGo.GetComponent<Button>();
                var amountTr = fertBtnGo.transform.Find("Amount");
                var amountText = amountTr != null ? amountTr.GetComponent<TMP_Text>() : null;

                fertBtnProp.objectReferenceValue = btnComp;
                hudSo.FindProperty("_fertilizerAmount").objectReferenceValue = amountText;
                hudSo.ApplyModifiedProperties();

                var hudRt = hudRoot.GetComponent<RectTransform>();
                hudRt.sizeDelta = new Vector2(580, 100);

                PrefabUtility.SaveAsPrefabAsset(hudRoot, HudPrefabPath);
                Debug.Log($"Fertilizer button added to {HudPrefabPath}. HUD width set to 580.");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(hudRoot);
            }
        }
    }
}
