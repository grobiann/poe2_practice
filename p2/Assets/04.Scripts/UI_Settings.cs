using p2.Minimap;
using Unity.VisualScripting;
using UnityEngine;

namespace p2.Settings.UI
{
    public class UI_Settings : MonoBehaviour
    {
        [SerializeField] private SettingUiPrefabBuilder _builder;
        [SerializeField] private Transform _interfaceParent;

        private void Start()
        {
            CreateInterfaceTab();
        }

        private void CreateInterfaceTab()
        {
            GameSettings.MapAttribute mapSettings = new GameSettings.MapAttribute();

            // 지도
            _builder.SetParent(_interfaceParent);
            _builder.SetValueChangedListener(OnValueChanged);
            _builder.CreateTitleOption("지도");
            _builder.CreateBoolOption(ESettingItemID.Map_ShowMinimap, "미니맵 보기", "화면 우상단에 미니맵을 보여줍니다.", mapSettings.ShowMinimap);
            _builder.CreateBoolOption(ESettingItemID.Map_AutoMap, "오토 맵", "지도가 중앙에서 벗어났을 때 지도 열기/닫기 키를 누르면 지도가 닫히지 않고 중앙으로 조정됩니다.", mapSettings.AutoMap);
            _builder.CreateIntOption(ESettingItemID.Map_LandscapeOpacity, "지형 투명도", mapSettings.LandscapeOpacity, 0, 255, 1);
            _builder.CreateIntOption(ESettingItemID.Map_MapOpacity, "지도 투명도", mapSettings.MapOpacity, 0, 255, 1);
            _builder.CreateIntOption(ESettingItemID.Map_MinimapFOV, "지도 시야 조절", mapSettings.MinimapFOV, 10, 100, 10);
            _builder.CreateBoolOption(ESettingItemID.Map_ShowColleagueNames, "동료 이름 표시", "지도 상에 동료의 이름이 나타납니다.", mapSettings.ShowColleagueNames);
            _builder.CreateBoolOption(ESettingItemID.Map_ShowInterestPointsNames, "관심 지점 이름 표시", "지도 상에 관심 지점의 이름이 나타납니다.", mapSettings.ShowInterestPointsNames);
            _builder.CreateBoolOption(ESettingItemID.Map_HideMapAttribute, "지도 전환 모드 숨기기", "화면에 보이는 지도의 속성 부여를 숨깁니다.", mapSettings.HideMapAttribute);
            _builder.Build();
        }

        private void OnValueChanged(SettingItemBase option)
        {
            GameSettings.MapAttribute mapSettings = new GameSettings.MapAttribute();
            MinimapUiController minimapUiController = Object.FindFirstObjectByType<MinimapUiController>();
            MiniMapController minimapController = Object.FindFirstObjectByType<MiniMapController>();

            switch (option.ItemID)
            {
                case ESettingItemID.Map_ShowMinimap:
                    mapSettings.ShowMinimap = ((BoolSettingItem)option).Value;
                    minimapUiController.Refresh();
                    break;
                case ESettingItemID.Map_AutoMap:
                    mapSettings.AutoMap = ((BoolSettingItem)option).Value;
                    minimapUiController.Refresh();
                    break;
                case ESettingItemID.Map_LandscapeOpacity:
                    mapSettings.LandscapeOpacity = ((IntSettingItem)option).Value;
                    minimapController.Minimap.SetLandscapeOpacity(mapSettings.LandscapeOpacity / 255.0f);
                    break;
                case ESettingItemID.Map_MapOpacity:
                    mapSettings.MapOpacity = ((IntSettingItem)option).Value;
                    minimapController.Minimap.SetMapOpacity(mapSettings.MapOpacity / 255.0f);
                    break;
                case ESettingItemID.Map_MinimapFOV:
                    mapSettings.MinimapFOV = ((IntSettingItem)option).Value;
                    Object.FindFirstObjectByType<UI_MinimapSmall>()?.Refresh();
                    break;
                case ESettingItemID.Map_ShowColleagueNames:
                    mapSettings.ShowColleagueNames = ((BoolSettingItem)option).Value;
                    // Do nothing
                    break;
                case ESettingItemID.Map_ShowInterestPointsNames:
                    mapSettings.ShowInterestPointsNames = ((BoolSettingItem)option).Value;
                    // Do nothing
                    break;
                case ESettingItemID.Map_HideMapAttribute:
                    mapSettings.HideMapAttribute = ((BoolSettingItem)option).Value;
                    // Do nothing
                    break;
                default:
                    Debug.LogError("Unknown option: " + option.ItemID);
                    break;
            }
        }
    }
}
