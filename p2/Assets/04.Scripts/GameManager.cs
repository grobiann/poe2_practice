using System.Linq;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameMode CurrentGameMode { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        EnterMap(-1);
    }

    public void EnterMap(int mapKey)
    {
        UnityEngine.Debug.Log("EnterMap: " + mapKey);

        PlayerCharacter player = UnityEngine.Object.FindFirstObjectByType<PlayerCharacter>();

        Map map = new Map();
        map.CreateNavMesh();

        MiniMap miniMap = UnityEngine.Object.FindFirstObjectByType<MiniMap>();
        miniMap.Setup(map);

        CurrentGameMode = new GameMode(map);
        CurrentGameMode.SetPlayerCharacter(player);
    }
}

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
}

public class Map
{
    public Bounds Bounds { get; private set; }
    public Vector3 Position { get; private set; }

    public void CreateNavMesh()
    {
        NavMeshSurface surface = Object.FindFirstObjectByType<NavMeshSurface>();
        surface.BuildNavMesh();

        Bounds = surface.navMeshData.sourceBounds;
        Position = surface.navMeshData.position;
    }
}
