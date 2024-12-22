using System.Runtime.CompilerServices;
using UnityEngine;

namespace p2.Minimap
{
    public interface IFogOfWarDrawer
    {
        void DrawFogOfWar(Texture2D texture);
        void Reveal(Texture2D texture, Vector3 position);
    }

    public interface IEdgeDrawer
    {
        void DrawEdge(Texture2D texture);
    }

    public interface IMovableDrawer
    {
        void DrawMovable(Texture2D texture);
    }

    public interface IMapObjectDrawer
    {
        void DrawMapObjects(Texture2D texture);
    }

    public class Minimap
    {
        public Material Material { get; private set; }
        public float MapOpacity { get; private set; } = 0.5f;
        public float LandscapeOpacity { get; private set; } = 0.5f;

        private const float TILE_SIZE = 1.0f;

        private readonly IFogOfWarDrawer _fogOfWarDrawer;
        private readonly IEdgeDrawer _edgeDrawer;
        private readonly IMovableDrawer _movableDrawer;
        private readonly IMapObjectDrawer _mapObjectDrawer;

        private Texture2D _mainTex;
        private Texture2D _fogOfWarTex;
        private Texture2D _movableTex;
        private readonly int _textureSize;

        public Minimap(
            int textureSize,
            IEdgeDrawer edgeDrawer,
            IFogOfWarDrawer fogOfWarDrawer,
            IMovableDrawer walkableDrawer,
            IMapObjectDrawer mapObjectDrawer)
        {
            _textureSize = textureSize;

            _fogOfWarDrawer = fogOfWarDrawer;
            _edgeDrawer = edgeDrawer;
            _movableDrawer = walkableDrawer;
            _mapObjectDrawer = mapObjectDrawer;

            _fogOfWarTex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            _fogOfWarTex.filterMode = FilterMode.Bilinear;
            _fogOfWarTex.wrapMode = TextureWrapMode.Clamp;
            _fogOfWarTex.Apply();

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
            _edgeDrawer.DrawEdge(_mainTex);
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

        public Rect ExtractRect(Vector3 position, float worldWidth, float worldHeight)
        {
            float mapSize = 128;
            float divider = 1 / mapSize / TILE_SIZE;
            float minx = (position.x - worldWidth * 0.5f) * divider;
            float miny = (position.z - worldHeight * 0.5f) * divider;
            float width = worldWidth * divider;
            float height = worldHeight * divider;

            return new Rect(minx, miny, width, height);
        }
    }
}