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
            _builder.CreateBoolOption(ESettingParamID.Map_ShowMinimap, "미니맵 보기", mapSettings.ShowMinimap);
            _builder.CreateBoolOption(ESettingParamID.Map_AutoMap, "오토 맵", mapSettings.AutoMap);
            _builder.CreateIntOption(ESettingParamID.Map_LandscapeOpacity, "지형 투명도", mapSettings.LandscapeOpacity, 0, 255, 1);
            _builder.CreateIntOption(ESettingParamID.Map_MapOpacity, "지도 투명도", mapSettings.MapOpacity, 0, 255, 1);
            _builder.CreateIntOption(ESettingParamID.Map_MinimapFOV, "지도 시야 조절", mapSettings.MinimapFOV, 1, 1000, 1);
            _builder.CreateBoolOption(ESettingParamID.Map_ShowColleagueNames, "동료 이름 표시", mapSettings.ShowColleagueNames);
            _builder.CreateBoolOption(ESettingParamID.Map_ShowInterestPointsNames, "관심 지점 이름 표시", mapSettings.ShowInterestPointsNames);
            _builder.CreateBoolOption(ESettingParamID.Map_HideMapAttribute, "지도 전환 모드 숨기기", mapSettings.HideMapAttribute);
            _builder.Build();
        }

        private void OnValueChanged(SettingItemBase option)
        {
            GameSettings.MapAttribute mapSettings = new GameSettings.MapAttribute();

            switch (option.ParamID)
            {
                case ESettingParamID.Map_ShowMinimap:
                    mapSettings.ShowMinimap = ((BoolSettingItem)option).Value;
                    break;
                case ESettingParamID.Map_AutoMap:
                    mapSettings.AutoMap = ((BoolSettingItem)option).Value;
                    break;
                case ESettingParamID.Map_LandscapeOpacity:
                    mapSettings.LandscapeOpacity = ((IntSettingItem)option).Value;
                    break;
                case ESettingParamID.Map_MapOpacity:
                    mapSettings.MapOpacity = ((IntSettingItem)option).Value;
                    break;
                case ESettingParamID.Map_MinimapFOV:
                    mapSettings.MinimapFOV = ((IntSettingItem)option).Value;
                    break;
                case ESettingParamID.Map_ShowColleagueNames:
                    mapSettings.ShowColleagueNames = ((BoolSettingItem)option).Value;
                    break;
                case ESettingParamID.Map_ShowInterestPointsNames:
                    mapSettings.ShowInterestPointsNames = ((BoolSettingItem)option).Value;
                    break;
                case ESettingParamID.Map_HideMapAttribute:
                    mapSettings.HideMapAttribute = ((BoolSettingItem)option).Value;
                    break;
                default:
                    Debug.LogError("Unknown option: " + option.ParamID);
                    break;
            }

            // TODO: 설정 변경에 대한 기능 처리
        }
    }
}
