using System;
using System.Collections.Generic;
using System.Linq;
using PROJECTS.CrossProject.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CrossProject.Editor.OdinEntities
{
        public abstract class AbstractOdinDropdownSelector<T> : LazyOdinValueDrawer<T>, IDefinesGenericMenuItems
    {
        protected abstract string PropertyName { get; }

        private const int MinDropdownWidth = 350;

        private bool _updateCurrentValueRequest = true;
        private string _currentValue;

        private GenericSelector<string> _selector;
        private Func<Rect, OdinSelector<string>> _createSelector;

        private static List<string> _namesArray;
        private List<string> NamesArray => _namesArray ??= GetNamesArray().ToList();

        static AbstractOdinDropdownSelector()
        {
            AssetsWatcher.OnAssetsChanged += () => _namesArray = null;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            UpdateCurrentValue();

            var selections = OdinSelector<string>.DrawSelectorDropdown(label, _currentValue, _createSelector);

            if (selections == null)
                return;

            var items = selections.ToList();
            if (items.Count == 0)
                return;

            SetField(items[0]);
        }

        private void UpdateCurrentValue()
        {
            if (_updateCurrentValueRequest)
            {
                _updateCurrentValueRequest = false;
                _currentValue = GetValue();

                if (!NamesArray.Contains(_currentValue))
                {
                    SetField(null);
                }
            }
        }

        protected override void OnAssetsChanged()
        {
            // When AssetPostprocessor.OnPostprocessAllAssets fires, the object's fields have not yet been updated.
            _updateCurrentValueRequest = true;

            _selector = new GenericSelector<string>(title: null, NamesArray, supportsMultiSelect: false);
            _selector.EnableSingleClickToSelect();
            _createSelector = rect =>
            {
                rect.width = Mathf.Max(rect.width, MinDropdownWidth);
                _selector.ShowInPopup(rect);
                return _selector;
            };
        }

        protected virtual string GetValue()
        {
            return (string)ReflectionUtility.GetInstanceField(ValueEntry.SmartValue, PropertyName);
        }

        protected virtual void SetValue(string value)
        {
            ReflectionUtility.SetInstanceField(ValueEntry.SmartValue, PropertyName, value);
        }

        private void SetField(string selection)
        {
            Property.RecordForUndo();
            _currentValue = selection;
            SetValue(selection);
            Property.MarkSerializationRootDirty();
        }

        protected abstract IEnumerable<string> GetNamesArray();

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddItem(new GUIContent("Set Id.Value to Null"), false, SetNullValue);

            void SetNullValue()
            {
                SetField(null);
                OnAssetsChanged();
            }
        }
    }
}