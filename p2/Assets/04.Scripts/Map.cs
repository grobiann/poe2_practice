using Unity.AI.Navigation;
using UnityEngine;

public class Map : MonoBehaviour
{
    public float MapSize = 128;
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
