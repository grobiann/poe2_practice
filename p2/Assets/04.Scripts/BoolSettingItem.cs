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

        public bool Value { get; private set; }

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void Initialize(string label, bool value)
        {
            UnityEngine.Debug.Log("Initialize: " + label + " " + value);
        }

        private void OnButtonClicked()
        {
            Value = !Value;
            OnValueChanged();
        }
    }

    public class IntSettingItem : SettingItemBase
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Slider _slider;

        public int Value { get; private set; }

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void Initialize(string label, float value, float min, float max, float interval)
        {
            UnityEngine.Debug.Log("Initialize: " + label + " " + value + " " + min + " " + max);
        }

        private void OnSliderValueChanged(float value)
        {
            Value = (int)value;
            OnValueChanged();
        }
    }

    public class TitleSettingItem : SettingItemBase
    {

    }

    public class DropDownSettingItem : SettingItemBase
    {
    }
}
