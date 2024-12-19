using UnityEngine;

public class Minimap
{
    public FogOfWar FogOfWar { get; set; }

    private RenderTexture _mapTexture2D;

    public Minimap(RenderTexture texture)
    {
        _mapTexture2D = texture;
    }

    public void DrawMap(NavMeshBorderDrawer borderDrawer)
    {
        borderDrawer.Draw(_mapTexture2D);
    }

    public void UpdateFogOfWar(Vector3 playerPosition)
    {
        if (FogOfWar == null)
            return;

        FogOfWar.Reveal(playerPosition);
    }
}
