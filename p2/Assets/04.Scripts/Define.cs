using UnityEngine;

public static class Define
{
    public static LayerMask GroundLayer = 1 << LayerMask.NameToLayer("Ground");
    public static LayerMask MouseClickable = 1 << LayerMask.NameToLayer("MouseClickable");
}
