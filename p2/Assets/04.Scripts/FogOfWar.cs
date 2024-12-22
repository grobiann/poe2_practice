using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace p2.Minimap
{
    public class FogOfWar : IFogOfWarDrawer
    {
        public float RevealRadius { get; set; } = 10.0f;
        public float EdgeSharpness { get; set; } = 3f;

        private Color32[] _fogPixels;
        private float _tileSize = 1.0f;
        private readonly int _textureSize;

        public FogOfWar(int textureSize)
        {
            _textureSize = textureSize;
            _fogPixels = new Color32[textureSize * textureSize];

            for (int i = 0; i < _fogPixels.Length; i++)
            {
                _fogPixels[i] = new Color32(0,0,0, 255);
            }
        }

        public void DrawFogOfWar(Texture2D texture)
        {
            texture.SetPixels32(_fogPixels);
            texture.Apply();
        }

        public void Reveal(Texture2D texture, Vector3 position)
        {
            Profiler.BeginSample("FogOfWar.Reveal");

            int mapSize = 128;
            int centerX = (int)(position.x / mapSize * _textureSize / _tileSize);
            int centerY = (int)(position.z / mapSize * _textureSize / _tileSize);
            float gridRevealRadius = RevealRadius / mapSize * _textureSize / _tileSize;
            float radiusSquared = gridRevealRadius * gridRevealRadius;

            for (int y = centerY - (int)gridRevealRadius; y <= centerY + (int)gridRevealRadius; y++)
            {
                for (int x = centerX - (int)gridRevealRadius; x <= centerX + (int)gridRevealRadius; x++)
                {
                    int index = y * _textureSize + x;
                    if (index < 0 || index >= _fogPixels.Length)
                        continue;

                    float dx = x - centerX;
                    float dy = y - centerY;
                    float distanceSquared = dx * dx + dy * dy;
                    if (distanceSquared > radiusSquared)
                        continue;

                    // (1,0)과 (-1,0)을 지나도록 하는 alpha 계산식
                    // clamp01(cof*(1-abs(x))
                    // https://www.wolframalpha.com/input?i=min%28max%282*%281-abs%28x%29%29%2C0%29%2C1%29
                    float xValue = Mathf.Sqrt(distanceSquared / radiusSquared);
                    float alpha = Mathf.Clamp01(EdgeSharpness * (1 - xValue));
                    byte opacity = (byte)(255 * (1.0f - alpha));
                    byte prevOpacity = _fogPixels[index].a;

                    if (opacity < prevOpacity)
                    {
                        byte inverseOpacity = (byte)(255 - opacity);
                        _fogPixels[index] = new Color32(inverseOpacity, inverseOpacity, inverseOpacity, opacity);
                    }
                }
            }

            texture.SetPixels32(_fogPixels);
            texture.Apply();

            Profiler.EndSample();
        }
    }
}