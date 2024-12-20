using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace p2.Settings.UI
{
    public class BoolSettingItem : SettingItemBase
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _checkMark;
        
        private string desc;

        public bool Value { get; private set; }

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void Initialize(string label, bool value)
        {
            _label.text = label;
            Value = value;
            Refresh();
        }

        protected override void OnAttributeInitialized()
        {
            BoolItemAttribute attribute = (BoolItemAttribute)_attribute;
            Initialize(attribute.label, attribute.value);
        }

        private void OnButtonClicked()
        {
            Value = !Value;
            Refresh();

            OnValueChanged();
        }

        private void Refresh()
        {
            _checkMark.gameObject.SetActive(Value);
        }
    }
}
