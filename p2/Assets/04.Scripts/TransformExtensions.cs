using UnityEngine;

public static class TransformExtensions
{
    public static Vector2 ToVector2XZ(this Vector3 worldPosition)
    {
        return new Vector2(worldPosition.x, worldPosition.z);
    }

    public static Vector2 UvToPosition(this RectTransform parent, Vector2 uv)
    {
        Vector2 size = parent.sizeDelta;
        Vector2 position = new Vector2(uv.x * size.x, uv.y * size.y);
        return position;
    }
}