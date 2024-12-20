using System;
using System.Collections.Generic;
using UnityEngine;
using static p2.Settings.UI.SettingUiPrefabBuilder;

namespace p2.Settings.UI
{
    public class SettingUiPrefabBuilder : MonoBehaviour
    {
        [SerializeField] private TitleSettingItem _titleOptionPrefab;
        [SerializeField] private BoolSettingItem _boolOptionPrefab;
        [SerializeField] private IntSettingItem _intOptionPrefab;
        [SerializeField] private DropDownSettingItem _dropDownOptionPrefab;

        private Transform _parent;
        private Action<SettingItemBase> _onValueChanged;
        private List<ISettingItemAttribute> _itemsToInstantiate = new List<ISettingItemAttribute>();

        public void Build()
        {
            if (_parent == null)
            {
                Debug.LogWarning("Parent is not set");
                return;
            }

            foreach (var itemBuildData in _itemsToInstantiate)
            {
                SettingItemBase item = InstantiateItem(itemBuildData, _parent);
                item.InitListener(_onValueChanged);
            }
        }

        public SettingUiPrefabBuilder SetParent(Transform parent)
        {
            _parent = parent;
            return this;
        }

        public SettingUiPrefabBuilder SetValueChangedListener(Action<SettingItemBase> listener)
        {
            _onValueChanged = listener;
            return this;
        }

        public SettingUiPrefabBuilder CreateTitleOption(string label)
        {
            _itemsToInstantiate.Add(new TitleItemAttribute(label));
            return this;
        }

        public SettingUiPrefabBuilder CreateBoolOption(ESettingItemID id, string label, string desc, bool value)
        {
            _itemsToInstantiate.Add(new BoolItemAttribute(id, label, desc, value));
            return this;
        }

        public SettingUiPrefabBuilder CreateIntOption(ESettingItemID id, string label, int value, int min, int max, int interval)
        {
            _itemsToInstantiate.Add(new IntItemAttribute(id, label, value, min, max, interval));
            return this;
        }

        public SettingUiPrefabBuilder CreateDropDownOption(ESettingItemID id, string label, string[] options, int value)
        {
            _itemsToInstantiate.Add(new DropdownItemAttribute(id, label, options, value));
            return this;
        }

        private SettingItemBase InstantiateItem(ISettingItemAttribute buildData, Transform parent)
        {
            SettingItemBase result;
            switch (buildData.ItemType)
            {
                case ESettingItemType.Title:
                    result = Instantiate(_titleOptionPrefab, parent);
                    break;
                case ESettingItemType.Bool:
                    result = Instantiate(_boolOptionPrefab, parent);
                    break;
                case ESettingItemType.Int:
                    result = Instantiate(_intOptionPrefab, parent);
                    break;
                case ESettingItemType.Dropdown:
                    result = Instantiate(_dropDownOptionPrefab, parent);
                    break;
                default:
                    Debug.LogError("Unknown item type: " + buildData.ItemType);
                    return null;
            }

            result.InitAttribute(buildData);
            return result;
        }
    }
}
