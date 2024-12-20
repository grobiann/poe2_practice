using System;
using System.Collections.Generic;
using UnityEngine;

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
        private List<IItemBuildData> _itemsToInstantiate = new List<IItemBuildData>();

        public void Build()
        {
            if(_parent == null)
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
            _itemsToInstantiate.Add(new TitleItemBuildData(label));
            return this;
        }

        public SettingUiPrefabBuilder CreateBoolOption(ESettingParamID id, string label, bool value)
        {
            _itemsToInstantiate.Add(new BoolItemBuildData(id, label, value));
            return this;
        }

        public SettingUiPrefabBuilder CreateIntOption(ESettingParamID id, string label, int value, int min, int max, int interval)
        {
            _itemsToInstantiate.Add(new IntItemBuildData(id, label, value, min, max, interval));
            return this;
        }

        public SettingUiPrefabBuilder CreateDropDownOption(ESettingParamID id, string label, string[] options, int value)
        {
            _itemsToInstantiate.Add(new DropdownItemBuildData(id, label, options, value));
            return this;
        }

        private SettingItemBase InstantiateItem(IItemBuildData item, Transform parent)
        {
            SettingItemBase result;
            switch (item.ItemBuildType)
            {
                case EItemBuildType.Title:
                    result = Instantiate(_titleOptionPrefab, parent);
                    break;
                case EItemBuildType.Bool:
                    result = Instantiate(_boolOptionPrefab, parent);
                    break;
                case EItemBuildType.Int:
                    result = Instantiate(_intOptionPrefab, parent);
                    break;
                case EItemBuildType.Dropdown:
                    result = Instantiate(_dropDownOptionPrefab, parent);
                    break;
                default:
                    Debug.LogError("Unknown item type: " + item.ItemBuildType);
                    return null;
            }

            result.InitId(item.ParamID);
            return result;
        }

        public enum EItemBuildType
        {
            Title,
            Bool,
            Int,
            Dropdown
        }
        public interface IItemBuildData {
            EItemBuildType ItemBuildType { get; }
            ESettingParamID ParamID { get; }
        }
        private class TitleItemBuildData : IItemBuildData
        {
            public EItemBuildType ItemBuildType => EItemBuildType.Title;
            public ESettingParamID ParamID => ESettingParamID.None;
            public string title;

            public TitleItemBuildData(string title)
            {
                this.title = title;
            }
        }

        private class BoolItemBuildData : IItemBuildData
        {
            public EItemBuildType ItemBuildType => EItemBuildType.Bool;
            public ESettingParamID ParamID => id;
            public ESettingParamID id;
            public string label;
            public bool value;

            public BoolItemBuildData(ESettingParamID id, string label, bool value)
            {
                this.id = id;
                this.label = label;
                this.value = value;
            }
        }

        private class IntItemBuildData : IItemBuildData
        {
            public EItemBuildType ItemBuildType => EItemBuildType.Int;
            public ESettingParamID ParamID => id;
            public ESettingParamID id;
            public string label;
            public int value;
            public int min;
            public int max;
            public int interval;

            public IntItemBuildData(ESettingParamID id, string label, int value, int min, int max, int interval)
            {
                this.id = id;
                this.label = label;
                this.value = value;
                this.min = min;
                this.max = max;
                this.interval = interval;
            }
        }

        private class DropdownItemBuildData : IItemBuildData
        {
            public EItemBuildType ItemBuildType => EItemBuildType.Dropdown;
            public ESettingParamID ParamID => id;
            public ESettingParamID id;
            public string label;
            public string[] options;
            public int value;

            public DropdownItemBuildData(ESettingParamID id, string label, string[] options, int value)
            {
                this.id = id;
                this.label = label;
                this.options = options;
                this.value = value;
            }
        }
    }
}
