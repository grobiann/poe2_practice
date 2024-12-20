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
        int textureSize = 128;

        MinimapCameraDrawer minimapCameraDrawer = Object.FindFirstObjectByType<MinimapCameraDrawer>();

        Minimap = new Minimap(
            new FogOfWar(textureSize),
            minimapCameraDrawer,
            new MinimapWalkableDrawer(),
            minimapCameraDrawer);
        Minimap.Init();
    }
}
