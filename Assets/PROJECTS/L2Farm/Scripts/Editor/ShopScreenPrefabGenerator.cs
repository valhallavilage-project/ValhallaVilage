using System.IO;
using System.Linq;
using L2Farm.Features.Shop;
using TMPro;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Editor
{
    public static class ShopScreenPrefabGenerator
    {
        private const string PrefabDir = "Assets/PROJECTS/L2Farm/Prefabs/Ui/Screens/Shop";
        private const string PrefabPath = PrefabDir + "/ShopScreen.prefab";

        [MenuItem("Tools/L2Farm/Generate ShopScreen Prefab")]
        public static void Generate()
        {
            if (!Directory.Exists(PrefabDir))
                Directory.CreateDirectory(PrefabDir);

            var root = new GameObject("ShopScreen", typeof(RectTransform), typeof(CanvasGroup));
            var rootRt = root.GetComponent<RectTransform>();
            Stretch(rootRt);

            var bg = CreateChild(root.transform, "Background", out var bgRt);
            Stretch(bgRt);
            var bgImg = bg.AddComponent<Image>();
            bgImg.color = new Color(0f, 0f, 0f, 0.6f);
            bg.AddComponent<Button>();

            var panel = CreateChild(root.transform, "Panel", out var panelRt);
            panelRt.anchorMin = new Vector2(0.5f, 0.5f);
            panelRt.anchorMax = new Vector2(0.5f, 0.5f);
            panelRt.pivot = new Vector2(0.5f, 0.5f);
            panelRt.anchoredPosition = Vector2.zero;
            panelRt.sizeDelta = new Vector2(960, 720);
            var panelImg = panel.AddComponent<Image>();
            panelImg.color = new Color(0.16f, 0.11f, 0.07f, 0.97f);

            var title = CreateChild(panel.transform, "Title", out var titleRt);
            titleRt.anchorMin = new Vector2(0f, 1f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -30f);
            titleRt.sizeDelta = new Vector2(0f, 80f);
            var titleText = title.AddComponent<TextMeshProUGUI>();
            titleText.text = "МАГАЗИН";
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.fontSize = 56;
            titleText.color = Color.white;

            var items = CreateChild(panel.transform, "Items", out var itemsRt);
            itemsRt.anchorMin = new Vector2(0.5f, 0.5f);
            itemsRt.anchorMax = new Vector2(0.5f, 0.5f);
            itemsRt.pivot = new Vector2(0.5f, 0.5f);
            itemsRt.anchoredPosition = new Vector2(0f, -20f);
            itemsRt.sizeDelta = new Vector2(900f, 520f);
            var layout = items.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 24f;
            layout.padding = new RectOffset(20, 20, 20, 20);
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlHeight = false;
            layout.childControlWidth = false;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;

            var shop = root.AddComponent<ShopScreen>();

            var heal = CreateItem(items.transform, "HealItem", ShopItemType.HealPotion, "Зелье лечения");
            var energy = CreateItem(items.transform, "EnergyItem", ShopItemType.EnergyPotion, "Зелье энергии");
            var time = CreateItem(items.transform, "TimeItem", ShopItemType.TimePotion, "Зелье времени");

            var closeGo = CreateChild(panel.transform, "CloseButton", out var closeRt);
            closeRt.anchorMin = new Vector2(1f, 1f);
            closeRt.anchorMax = new Vector2(1f, 1f);
            closeRt.pivot = new Vector2(1f, 1f);
            closeRt.anchoredPosition = new Vector2(-20f, -20f);
            closeRt.sizeDelta = new Vector2(80f, 80f);
            var closeImg = closeGo.AddComponent<Image>();
            closeImg.color = new Color(0.75f, 0.2f, 0.2f);
            var closeBtn = closeGo.AddComponent<Button>();
            var closeLabel = CreateChild(closeGo.transform, "X", out var closeLabelRt);
            Stretch(closeLabelRt);
            var closeLabelText = closeLabel.AddComponent<TextMeshProUGUI>();
            closeLabelText.text = "X";
            closeLabelText.alignment = TextAlignmentOptions.Center;
            closeLabelText.fontSize = 48;
            closeLabelText.color = Color.white;

            var shopSo = new SerializedObject(shop);
            shopSo.FindProperty("_closeButton").objectReferenceValue = closeBtn;
            var itemsProp = shopSo.FindProperty("_items");
            itemsProp.arraySize = 3;
            itemsProp.GetArrayElementAtIndex(0).objectReferenceValue = heal;
            itemsProp.GetArrayElementAtIndex(1).objectReferenceValue = energy;
            itemsProp.GetArrayElementAtIndex(2).objectReferenceValue = time;
            shopSo.ApplyModifiedProperties();

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            Object.DestroyImmediate(root);

            RegisterAddressable(PrefabPath, "ShopScreen", "Ui.Implementations");

            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
            Debug.Log($"ShopScreen prefab created at {PrefabPath} and registered as Addressable \"ShopScreen\".");
        }

        private static void RegisterAddressable(string assetPath, string address, string groupName)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("Addressable settings are missing — cannot register entry.");
                return;
            }

            var group = settings.groups.FirstOrDefault(g => g != null && g.Name == groupName)
                        ?? settings.DefaultGroup;

            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = address;

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true);
            AssetDatabase.SaveAssets();
        }

        private static ShopItemView CreateItem(Transform parent, string objectName, ShopItemType type, string titleText)
        {
            var go = new GameObject(objectName, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(260f, 460f);
            var bg = go.GetComponent<Image>();
            bg.color = new Color(0.22f, 0.16f, 0.1f, 1f);

            var layout = go.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 12f;
            layout.padding = new RectOffset(12, 12, 16, 16);
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            var iconGo = CreateChild(go.transform, "Icon", out _);
            var icon = iconGo.AddComponent<Image>();
            icon.color = Color.white;
            icon.preserveAspect = true;
            var iconLe = iconGo.AddComponent<LayoutElement>();
            iconLe.preferredHeight = 200f;
            iconLe.preferredWidth = 200f;

            var titleGo = CreateChild(go.transform, "Title", out _);
            var titleTm = titleGo.AddComponent<TextMeshProUGUI>();
            titleTm.text = titleText;
            titleTm.alignment = TextAlignmentOptions.Center;
            titleTm.fontSize = 28;
            titleTm.color = Color.white;
            titleTm.enableWordWrapping = true;
            var titleLe = titleGo.AddComponent<LayoutElement>();
            titleLe.preferredHeight = 60f;

            var priceGo = CreateChild(go.transform, "Price", out _);
            var priceTm = priceGo.AddComponent<TextMeshProUGUI>();
            priceTm.text = "299 ₽";
            priceTm.alignment = TextAlignmentOptions.Center;
            priceTm.fontSize = 34;
            priceTm.color = new Color(1f, 0.85f, 0.3f);
            var priceLe = priceGo.AddComponent<LayoutElement>();
            priceLe.preferredHeight = 50f;

            var buyGo = CreateChild(go.transform, "BuyButton", out _);
            var buyImg = buyGo.AddComponent<Image>();
            buyImg.color = new Color(0.2f, 0.65f, 0.25f);
            var buyBtn = buyGo.AddComponent<Button>();
            var buyLe = buyGo.AddComponent<LayoutElement>();
            buyLe.preferredHeight = 64f;
            var labelGo = CreateChild(buyGo.transform, "Label", out var labelRt);
            Stretch(labelRt);
            var labelTm = labelGo.AddComponent<TextMeshProUGUI>();
            labelTm.text = "КУПИТЬ";
            labelTm.alignment = TextAlignmentOptions.Center;
            labelTm.fontSize = 30;
            labelTm.color = Color.white;

            var item = go.AddComponent<ShopItemView>();
            var so = new SerializedObject(item);
            so.FindProperty("_type").enumValueIndex = (int)type;
            so.FindProperty("_icon").objectReferenceValue = icon;
            so.FindProperty("_title").objectReferenceValue = titleTm;
            so.FindProperty("_price").objectReferenceValue = priceTm;
            so.FindProperty("_buyButton").objectReferenceValue = buyBtn;
            so.ApplyModifiedProperties();

            return item;
        }

        private static GameObject CreateChild(Transform parent, string objectName, out RectTransform rt)
        {
            var go = new GameObject(objectName, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            rt = go.GetComponent<RectTransform>();
            return go;
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}
