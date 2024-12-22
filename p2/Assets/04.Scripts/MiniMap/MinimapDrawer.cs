using UnityEngine;

namespace p2.mmap
{
    public interface IFogOfWarDrawer
    {
        void DrawFogOfWar(RenderTexture texture);
        void Reveal(RenderTexture texture, Vector3 position);
    }

    public interface IBorderDrawer
    {
        void DrawBorder(Texture2D texture);
    }

    public interface IMovableDrawer
    {
        void DrawMovable(Texture2D texture);
    }

    public interface IMapObjectDrawer
    {
        void DrawMapObjects(Texture2D texture);
    }

    public class MinimapDrawer
    {
        public Material Material { get; private set; }
        public float MapOpacity { get; private set; } = 0.5f;
        public float LandscapeOpacity { get; private set; } = 0.5f;

        private readonly int _textureSize;
        private readonly IFogOfWarDrawer _fogOfWarDrawer;
        private readonly IBorderDrawer _borderDrawer;
        private readonly IMovableDrawer _movableDrawer;
        private readonly IMapObjectDrawer _mapObjectDrawer;

        private Texture2D _mainTex;
        private RenderTexture _fogOfWarTex;
        private Texture2D _movableTex;

        public MinimapDrawer(
            int textureSize,
            IBorderDrawer edgeDrawer,
            IFogOfWarDrawer fogOfWarDrawer,
            IMovableDrawer walkableDrawer,
            IMapObjectDrawer mapObjectDrawer)
        {
            _textureSize = textureSize;
            _fogOfWarDrawer = fogOfWarDrawer;
            _borderDrawer = edgeDrawer;
            _movableDrawer = walkableDrawer;
            _mapObjectDrawer = mapObjectDrawer;

            _fogOfWarTex = new RenderTexture(_textureSize, _textureSize, 0, RenderTextureFormat.ARGB32);
            _fogOfWarTex.filterMode = FilterMode.Bilinear;
            _fogOfWarTex.wrapMode = TextureWrapMode.Clamp;
            _fogOfWarTex.enableRandomWrite = true;
            _fogOfWarTex.Create();
            
            _mainTex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            _mainTex.filterMode = FilterMode.Bilinear;
            _mainTex.wrapMode = TextureWrapMode.Clamp;
            _mainTex.Apply();

            _movableTex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            _movableTex.filterMode = FilterMode.Bilinear;
            _movableTex.wrapMode = TextureWrapMode.Clamp;
            _movableTex.Apply();

            Material = new Material(Shader.Find("Unlit/FogOfWar"));
            Material.SetTexture("_MainTex", _mainTex);
            Material.SetTexture("_FogTex", _fogOfWarTex);
            Material.SetTexture("_MovableTex", _movableTex);
        }

        public void Init()
        {
            _borderDrawer.DrawBorder(_mainTex);
            _mapObjectDrawer.DrawMapObjects(_mainTex);
            _fogOfWarDrawer.DrawFogOfWar(_fogOfWarTex);
            _movableDrawer.DrawMovable(_movableTex);

            SetMapOpacity(MapOpacity);
            SetLandscapeOpacity(LandscapeOpacity);
        }

        public void SetMapOpacity(float opacity)
        {
            MapOpacity = opacity;
            Material.SetFloat("_Opacity", opacity);
        }

        public void SetLandscapeOpacity(float opacity)
        {
            LandscapeOpacity = opacity;
            Material.SetFloat("_LandscapeOpacity", opacity);
        }

        public void Draw(RenderTexture texture)
        {
            Graphics.Blit(_mainTex, texture);
        }

        public void UpdateFogOfWar(Vector3 playerPosition)
        {
            _fogOfWarDrawer.Reveal(_fogOfWarTex, playerPosition);
        }
    }
}