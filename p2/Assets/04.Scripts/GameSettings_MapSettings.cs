using UnityEngine;

namespace p2.Settings
{
    public partial class GameSettings
    {
        public class MapAttribute
        {
            public bool ShowMinimap
            {
                get => PlayerPrefsUtil.GetBool("setting_map_show_minimap", true);
                set => PlayerPrefsUtil.SetBool("setting_map_show_minimap", value);
            }

            public bool AutoMap
            {
                get => PlayerPrefsUtil.GetBool("setting_map_auto_map", true);
                set => PlayerPrefsUtil.SetBool("setting_map_auto_map", value);
            }

            public int LandscapeOpacity
            {
                get => PlayerPrefs.GetInt("setting_map_landscape_opacity", 50);
                set => PlayerPrefs.SetInt("setting_map_landscape_opacity", value);
            }

            public int MapOpacity
            {
                get => PlayerPrefs.GetInt("setting_map_map_opacity", 50);
                set => PlayerPrefs.SetInt("setting_map_map_opacity", value);
            }

            public int MinimapFOV
            {
                get => PlayerPrefs.GetInt("setting_map_minimap_fov", 90);
                set => PlayerPrefs.SetInt("setting_map_minimap_fov", value);
            }

            public bool ShowColleagueNames
            {
                get => PlayerPrefsUtil.GetBool("setting_map_show_colleague_names", true);
                set => PlayerPrefsUtil.SetBool("setting_map_show_colleague_names", value);
            }

            public bool ShowInterestPointsNames
            {
                get => PlayerPrefsUtil.GetBool("setting_map_show_interest_points_names", true);
                set => PlayerPrefsUtil.SetBool("setting_map_show_interest_points_names", value);
            }

            public bool HideMapAttribute
            {
                get => PlayerPrefsUtil.GetBool("setting_map_hide_map_attribute", false);
                set => PlayerPrefsUtil.SetBool("setting_map_hide_map_attribute", value);
            }

            public void Reset()
            {
                PlayerPrefs.DeleteKey("setting_map_show_minimap");
                PlayerPrefs.DeleteKey("setting_map_auto_map");
                PlayerPrefs.DeleteKey("setting_map_landscape_opacity");
                PlayerPrefs.DeleteKey("setting_map_map_opacity");
                PlayerPrefs.DeleteKey("setting_map_minimap_fov");
                PlayerPrefs.DeleteKey("setting_map_show_colleague_names");
                PlayerPrefs.DeleteKey("setting_map_show_interest_points_names");
                PlayerPrefs.DeleteKey("setting_map_hide_map_attribute");
            }
        }
    }
}
