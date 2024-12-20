using TMPro;
using UnityEngine;

namespace p2.Settings.UI
{
    public class TitleSettingItem : SettingItemBase
    {
        [SerializeField] private TextMeshProUGUI _label;

        public void Initialize(string label)
        {
            _label.text = label;
        }

        protected override void OnAttributeInitialized()
        {
            TitleItemAttribute attribute = (TitleItemAttribute)_attribute;
            Initialize(attribute.title);
        }
    }
}
