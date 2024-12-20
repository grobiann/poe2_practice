using UnityEngine;

public static class Define
{
    public static LayerMask GroundLayer = 1 << LayerMask.NameToLayer("Ground");
    public static LayerMask MouseClickable = 1 << LayerMask.NameToLayer("MouseClickable");
}

public enum ESettingItemID
{
    None = 0,

    Map_ShowMinimap,
    Map_AutoMap,
    Map_LandscapeOpacity,
    Map_MapOpacity,
    Map_MinimapFOV,
    Map_ShowColleagueNames,
    Map_ShowInterestPointsNames,
    Map_HideMapAttribute,
}