using UnityEngine;

public class GameMode
{
    public PlayerCharacter MyPlayerCharacter { get; private set; }
    public Map Map { get; private set; }
    public Minimap Minimap { get; private set; }

    public GameMode(Map map)
    {
        Map = map;
    }

    public void SetPlayerCharacter(PlayerCharacter playerCharacter)
    {
        MyPlayerCharacter = playerCharacter;
    }

    public void OnStart()
    {
        PrepareMiniMap();
    }

    private void PrepareMiniMap()
    {
        MiniMapController controller = Object.FindFirstObjectByType<MiniMapController>();
        int textureSize = 128;

        RenderTexture mainTex = controller.renderTexture;
        mainTex.width = textureSize;
        mainTex.height = textureSize;
        Minimap = new Minimap(mainTex);

        NavMeshBorderDrawer borderDrawer = Object.FindFirstObjectByType<NavMeshBorderDrawer>();
        Minimap.DrawMap(borderDrawer);

        FogOfWar fogOfWar = new FogOfWar(textureSize, controller.material);
        Minimap.FogOfWar = fogOfWar;
    }
}
