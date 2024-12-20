using UnityEngine;

public static class TransformExtensions
{
    public static Vector2 ToVector2XZ(this Vector3 worldPosition)
    {
        return new Vector2(worldPosition.x, worldPosition.z);
    }
}