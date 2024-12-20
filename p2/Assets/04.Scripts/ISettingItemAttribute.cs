namespace p2.Settings.UI
{
    public interface ISettingItemAttribute
    {
        ESettingItemType ItemType { get; }
        ESettingItemID ItemID { get; }
    }

    public enum ESettingItemType
    {
        Title,
        Bool,
        Int,
        Dropdown
    }

    public class TitleItemAttribute : ISettingItemAttribute
    {
        public ESettingItemType ItemType => ESettingItemType.Title;
        public ESettingItemID ItemID => ESettingItemID.None;
        public string title;

        public TitleItemAttribute(string title)
        {
            this.title = title;
        }
    }

    public class BoolItemAttribute : ISettingItemAttribute
    {
        public ESettingItemType ItemType => ESettingItemType.Bool;
        public ESettingItemID ItemID => id;
        public ESettingItemID id;
        public string label;
        public string desc;
        public bool value;

        public BoolItemAttribute(ESettingItemID id, string label, string desc, bool value)
        {
            this.id = id;
            this.label = label;
            this.desc = desc;
            this.value = value;
        }
    }

    public class IntItemAttribute : ISettingItemAttribute
    {
        public ESettingItemType ItemType => ESettingItemType.Int;
        public ESettingItemID ItemID => id;
        public ESettingItemID id;
        public string label;
        public int value;
        public int min;
        public int max;
        public int interval;

        public IntItemAttribute(ESettingItemID id, string label, int value, int min, int max, int interval)
        {
            this.id = id;
            this.label = label;
            this.value = value;
            this.min = min;
            this.max = max;
            this.interval = interval;
        }
    }

    public class DropdownItemAttribute : ISettingItemAttribute
    {
        public ESettingItemType ItemType => ESettingItemType.Dropdown;
        public ESettingItemID ItemID => id;
        public ESettingItemID id;
        public string label;
        public string[] options;
        public int value;

        public DropdownItemAttribute(ESettingItemID id, string label, string[] options, int value)
        {
            this.id = id;
            this.label = label;
            this.options = options;
            this.value = value;
        }
    }
}
