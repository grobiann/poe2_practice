using p2.Settings;
using System.Linq;
using System.Runtime.CompilerServices;
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

        CurrentGameMode = new GameMode(map);
        CurrentGameMode.SetPlayerCharacter(player);
        CurrentGameMode.OnStart();
    }
}
