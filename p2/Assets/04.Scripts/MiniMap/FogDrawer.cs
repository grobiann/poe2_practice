using UnityEngine;
using UnityEngine.Profiling;

namespace p2.mmap
{
    public class FogDrawer : MonoBehaviour, IFogOfWarDrawer
    {
        public float RevealRadius => _revealRadius;

        [SerializeField] private ComputeShader _fogShader;
        [SerializeField] private float _revealRadius = 10.0f;
        [SerializeField] private float _edgeSharpness = 3f;

        private float _tileSize = 1.0f;
        private int _textureSize = 512;

        public void DrawFogOfWar(RenderTexture texture)
        {
            Texture2D white = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            white.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 1.0f));
            white.Apply();

            Graphics.Blit(white, texture);
        }

        public void Reveal(RenderTexture fogTexture, Vector3 position)
        {
            Profiler.BeginSample("FogOfWar.Reveal");

            int mapSize = 128;
            int centerX = (int)(position.x / mapSize * _textureSize / _tileSize);
            int centerY = (int)(position.z / mapSize * _textureSize / _tileSize);
            float gridRevealRadius = _revealRadius / mapSize * _textureSize / _tileSize;
            float radiusSquared = gridRevealRadius * gridRevealRadius;

            int kernel = _fogShader.FindKernel("CSReveal");
            _fogShader.SetTexture(kernel, "fogBuffer", fogTexture);
            _fogShader.SetInt("textureSize", _textureSize);
            _fogShader.SetFloat("gridRevealRadius", gridRevealRadius);
            _fogShader.SetFloat("centerX", centerX);
            _fogShader.SetFloat("centerY", centerY);
            _fogShader.SetFloat("radiusSquared", radiusSquared);
            _fogShader.SetFloat("edgeSharpness", _edgeSharpness);

            int threadGroups = Mathf.CeilToInt(_textureSize / 8.0f);
            _fogShader.Dispatch(kernel, threadGroups, threadGroups, 1);

            Profiler.EndSample();
        }
    }
}