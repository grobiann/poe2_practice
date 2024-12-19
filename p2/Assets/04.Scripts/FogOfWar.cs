using UnityEngine;

public class FogOfWar
{
    private Material _fogMaterial;
    private Texture2D _fogOfWarTexture2D;
    private Color32[] _fogPixels;
    private int _textureSize;
    private float _tileSize = 1.0f;

    private const float REVEAL_RADIUS = 12.0f;
    private const float DARKNESS_PARAM1 = 6.0f;
    private const float DARKNESS_PARAM2 = 6.0f;

    public FogOfWar(int textureSize, Material material)
    {
        _textureSize = textureSize;
        _fogOfWarTexture2D = new Texture2D(_textureSize, _textureSize, TextureFormat.RGBA32, false);
        _fogOfWarTexture2D.filterMode = FilterMode.Bilinear;
        _fogOfWarTexture2D.wrapMode = TextureWrapMode.Clamp;
        _fogOfWarTexture2D.Apply();

        _fogMaterial = material;
        _fogMaterial.SetTexture("_FogTex", _fogOfWarTexture2D);
        _fogPixels = new Color32[_textureSize * _textureSize];

        ClearFogOfWar();
    }

    public void Reveal(Vector3 position)
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
        _fogOfWarTexture2D.SetPixels32(_fogPixels);
        _fogOfWarTexture2D.Apply();
    }

    public void ClearFogOfWar()
    {
        for (int i = 0; i < _fogPixels.Length; i++)
        {
            _fogPixels[i] = new Color32(0, 0, 0, 0);
        }
        _fogOfWarTexture2D.SetPixels32(_fogPixels);
        _fogOfWarTexture2D.Apply();
    }

    public Texture2D GetTexture()
    {
        return _fogOfWarTexture2D;
    }
}