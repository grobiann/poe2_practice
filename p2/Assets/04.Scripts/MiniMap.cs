using UnityEngine;

public interface IFogOfWarDrawer
{
    void DrawFogOfWar(Texture2D texture);
    void Reveal(Texture2D texture, Vector3 position);
}

public interface IEdgeDrawer
{
    void DrawEdge(Texture2D texture);
}

public interface IWalkableDrawer
{
    void DrawWalkable(Texture2D texture);
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

    private const int TEXTURE_SIZE = 128;
    private const float TILE_SIZE = 1.0f;

    private readonly IFogOfWarDrawer _fogOfWarDrawer;
    private readonly IEdgeDrawer _edgeDrawer;
    private readonly IWalkableDrawer _walkableDrawer;
    private readonly IMapObjectDrawer _mapObjectDrawer;

    private Texture2D _mainTex;
    private Texture2D _fogOfWarTex;
    private Texture2D _walkableTex;

    public Minimap(
        IFogOfWarDrawer fogOfWarDrawer,
        IEdgeDrawer edgeDrawer,
        IWalkableDrawer walkableDrawer,
        IMapObjectDrawer mapObjectDrawer)
    {
        _fogOfWarDrawer = fogOfWarDrawer;
        _edgeDrawer = edgeDrawer;
        _walkableDrawer = walkableDrawer;
        _mapObjectDrawer = mapObjectDrawer;

        _fogOfWarTex = new Texture2D(TEXTURE_SIZE, TEXTURE_SIZE, TextureFormat.RGBA32, false);
        _fogOfWarTex.filterMode = FilterMode.Bilinear;
        _fogOfWarTex.wrapMode = TextureWrapMode.Clamp;
        _fogOfWarTex.Apply();

        _mainTex = new Texture2D(TEXTURE_SIZE, TEXTURE_SIZE, TextureFormat.RGBA32, false);
        _mainTex.filterMode = FilterMode.Bilinear;
        _mainTex.wrapMode = TextureWrapMode.Clamp;
        _mainTex.Apply();

        _walkableTex = new Texture2D(TEXTURE_SIZE, TEXTURE_SIZE, TextureFormat.RGBA32, false);
        _walkableTex.filterMode = FilterMode.Bilinear;
        _walkableTex.wrapMode = TextureWrapMode.Clamp;
        _walkableTex.Apply();

        Material = new Material(Shader.Find("Unlit/FogOfWar"));
        Material.SetTexture("_MainTex", _mainTex);
        Material.SetTexture("_FogTex", _fogOfWarTex);
        Material.SetTexture("_WalkableTex", _walkableTex);
    }

    public void Init()
    {
        _edgeDrawer.DrawEdge(_mainTex);
        _mapObjectDrawer.DrawMapObjects(_mainTex);
        _fogOfWarDrawer.DrawFogOfWar(_fogOfWarTex);
        _walkableDrawer.DrawWalkable(_walkableTex);

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
        float divider = TILE_SIZE * TEXTURE_SIZE;
        float minx = (position.x - worldWidth * 0.5f) / divider;
        float miny = (position.z - worldHeight * 0.5f) / divider;
        float width = worldWidth / divider;
        float height = worldHeight / divider;

        return new Rect(minx, miny, width, height);
    }
}
