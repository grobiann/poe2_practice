using UnityEngine;

public class FogOfWar : IFogOfWarDrawer
{
    private Color32[] _fogPixels;
    private float _tileSize = 1.0f;
    private readonly int _textureSize;

    private const float REVEAL_RADIUS = 12.0f;
    private const float DARKNESS_PARAM1 = 6.0f;
    private const float DARKNESS_PARAM2 = 6.0f;

    public FogOfWar(int textureSize)
    {
        _textureSize = textureSize;
        _fogPixels = new Color32[textureSize * textureSize];
        // ClearFogOfWar();
    }

    public void DrawFogOfWar(Texture2D texture)
    {
        
    }

    public void Reveal(Texture2D texture, Vector3 position)
    {
        int centerX = (int)(position.x / _tileSize);
        int centerY = (int)(position.z / _tileSize);
        float radiusSquared = REVEAL_RADIUS * REVEAL_RADIUS;

        for (int y = centerY - (int)REVEAL_RADIUS; y <= centerY + (int)REVEAL_RADIUS; y++)
        {
            for (int x = centerX - (int)REVEAL_RADIUS; x <= centerX + (int)REVEAL_RADIUS; x++)
            {
                int index = y * _textureSize + x;
                if (index >= 0 && index < _fogPixels.Length)
                {
                    // gaussian distribution
                    float dx = x - centerX;
                    float dy = y - centerY;
                    float distanceSquared = dx * dx + dy * dy;

                    if (distanceSquared <= radiusSquared)
                    {
                        byte prevAlpha = _fogPixels[index].a;
                        float floatAlpha = DARKNESS_PARAM1 * Mathf.Exp(-DARKNESS_PARAM2 * distanceSquared / radiusSquared);
                        floatAlpha = Mathf.Min(floatAlpha, 1.0f);
                        byte alpha =  (byte)(255 * floatAlpha);

                        if (alpha > prevAlpha)
                        {
                            _fogPixels[index] = new Color32(0, 0, 0, alpha);
                        }
                    }
                }
            }
        }
        texture.SetPixels32(_fogPixels);
        texture.Apply();
    }

    //public void ClearFogOfWar()
    //{
    //    for (int i = 0; i < _fogPixels.Length; i++)
    //    {
    //        _fogPixels[i] = new Color32(0, 0, 0, 0);
    //    }
    //    _fogOfWarTexture2D.SetPixels32(_fogPixels);
    //    _fogOfWarTexture2D.Apply();
    //}
}