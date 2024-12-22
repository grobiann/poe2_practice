using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace p2.Minimap
{
    public class FogOfWar : IFogOfWarDrawer
    {
        public ComputeShader fogShader;
        //private RenderTexture fogTexture;

        //private ComputeBuffer fogBuffer;
        //private float[] fogValues;

        public float RevealRadius { get; set; } = 10.0f;
        public float EdgeSharpness { get; set; } = 3f;

        //private Color32[] _fogPixels;
        private float _tileSize = 1.0f;
        private readonly int _textureSize;

        public FogOfWar(int textureSize)
        {
            _textureSize = textureSize;
            //_fogPixels = new Color32[textureSize * textureSize];

            //for (int i = 0; i < _fogPixels.Length; i++)
            //{
            //    _fogPixels[i] = new Color32(0,0,0, 255);
            //}
        }

        //~FogOfWar()
        //{
        //    if (fogTexture != null)
        //    {
        //        fogTexture.Release();
        //    }
        //}

        public void DrawFogOfWar(RenderTexture texture)
        {
            Texture2D white = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            white.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 1.0f));
            white.Apply();

            Graphics.Blit(white, texture);
            //material.SetTexture("_FogBuffer", fogTexture); // RenderTexture 전달

            //texture.SetPixels32(_fogPixels);
            //texture.Apply();
        }

        public void Reveal(RenderTexture fogTexture, Vector3 position)
        {
            Profiler.BeginSample("FogOfWar.Reveal");

            int mapSize = 128;
            int centerX = (int)(position.x / mapSize * _textureSize / _tileSize);
            int centerY = (int)(position.z / mapSize * _textureSize / _tileSize);
            float gridRevealRadius = RevealRadius / mapSize * _textureSize / _tileSize;
            float radiusSquared = gridRevealRadius * gridRevealRadius;

            //if(fogTexture == null | fogTexture.width != _textureSize)
            //{
            //    if (fogTexture != null)
            //    {
            //        fogTexture.Release();
            //    }

            //    fogTexture = new RenderTexture(_textureSize, _textureSize, 0, RenderTextureFormat.ARGB32);
            //    fogTexture.enableRandomWrite = true;
            //    fogTexture.Create();
            //}

            int kernel = fogShader.FindKernel("CSReveal");
            fogShader.SetTexture(kernel, "fogBuffer", fogTexture);
            fogShader.SetInt("textureSize", _textureSize);
            fogShader.SetFloat("gridRevealRadius", gridRevealRadius);
            fogShader.SetFloat("centerX", centerX);
            fogShader.SetFloat("centerY", centerY);
            fogShader.SetFloat("radiusSquared", radiusSquared);
            fogShader.SetFloat("edgeSharpness", EdgeSharpness);

            int threadGroups = Mathf.CeilToInt(_textureSize / 8.0f);
            fogShader.Dispatch(kernel, threadGroups, threadGroups, 1);

            Profiler.EndSample();
        }
    }
}