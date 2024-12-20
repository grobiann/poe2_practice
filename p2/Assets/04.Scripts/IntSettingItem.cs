using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace p2.Settings.UI
{
    public class IntSettingItem : SettingItemBase
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Slider _slider;

        public int Value { get; private set; }
        private int _interval;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void Initialize(string label, int value, int min, int max, int interval)
        {
            _label.text = label;
            _slider.minValue = min;
            _slider.maxValue = max;
            _slider.wholeNumbers = true;
            SetSliderValueWithoutTrigger(value);

            Value = value;
            _interval = interval;
        }

        protected override void OnAttributeInitialized()
        {
            IntItemAttribute attribute = (IntItemAttribute)_attribute;
            Initialize(attribute.label, attribute.value, attribute.min, attribute.max, attribute.interval);
        }

        private void SetSliderValueWithoutTrigger(float value)
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _slider.value = value;
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            int prevValue = Value;
            int currentValue = Mathf.RoundToInt(value / _interval) * _interval;
            if(prevValue != currentValue)
            {
                Value = currentValue;
                SetSliderValueWithoutTrigger(currentValue);

                OnValueChanged();
            }
        }
    }
}
