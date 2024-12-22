using UnityEngine;

public class GameMode
{
    public PlayerCharacter MyPlayerCharacter { get; private set; }
    public Map Map { get; private set; }
    
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
    }
}
