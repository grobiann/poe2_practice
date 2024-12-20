using p2.Settings.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace p2.Settings
{
    public abstract class SettingItemBase : MonoBehaviour
    {
        public enum EBackgroundColor
        {
            None,
            Gray,
        }

        public bool Interactable => _interactable;
        public ESettingItemID ItemID => _attribute.ItemID;
        public ESettingItemType ItemType => _attribute.ItemType;
        protected ISettingItemAttribute _attribute;

        [SerializeField] private Image _background;
        [SerializeField] private bool _interactable = true;

        private event Action<SettingItemBase> onValueChanged = delegate { };

        public void InitAttribute(ISettingItemAttribute attribute)
        {
            _attribute = attribute;
            OnAttributeInitialized();
        }

        public void InitListener(Action<SettingItemBase> listener)
        {
            onValueChanged = listener;
        }

        public void SetInteractable(bool interactable)
        {
            _interactable = interactable;

            UpdateRender();
        }

        public void SetBackgroundColor(EBackgroundColor color)
        {
            _background.enabled = color != EBackgroundColor.None;
            switch (color)
            {
                case EBackgroundColor.None:
                    // Do nothing
                    break;
                case EBackgroundColor.Gray:
                    _background.color = Color.gray;
                    break;
                default:
                    Debug.LogError("Unknown color: " + color);
                    break;
            }
        }

        protected virtual void UpdateRender() { }
        protected void OnValueChanged()
        {
            onValueChanged(this);
        }
        protected virtual void OnAttributeInitialized() { }
    }
}
