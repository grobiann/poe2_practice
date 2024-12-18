using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MeshRenderer _borderRenderer;
    private Map _map;

    private void Update()
    {
        // Follow player
        _camera.transform.position = 
            GameManager.Instance.CurrentGameMode.MyPlayerCharacter.transform.position + 
            new Vector3(0, 10, 0);
    }

    public void Setup(Map map)
    {
        _map = map;

        _camera.orthographicSize = 5;
        Render();
    }

    public void Render()
    {
        if (_map == null)
        {
            Debug.LogError("Map is not set");
            return;
        }

        Mesh mesh = CreateBorderMesh(NavMesh.CalculateTriangulation());
        MeshFilter meshFilter = _borderRenderer.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private Mesh CreateBorderMesh(NavMeshTriangulation triangulation)
    {
        Dictionary<(Vector3, Vector3), int> edgeCount = new Dictionary<(Vector3, Vector3), int>();

        for (int i = 0; i < triangulation.indices.Length; i += 3)
        {
            Vector3 a = triangulation.vertices[triangulation.indices[i]];
            Vector3 b = triangulation.vertices[triangulation.indices[i + 1]];
            Vector3 c = triangulation.vertices[triangulation.indices[i + 2]];

            AddEdge(edgeCount, a, b);
            AddEdge(edgeCount, b, c);
            AddEdge(edgeCount, c, a);
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        foreach (var edge in edgeCount)
        {
            if (edge.Value == 1)
            {
                int cnt = vertices.Count;
                vertices.Add(edge.Key.Item1);
                vertices.Add(edge.Key.Item2);

                indices.Add(cnt);
                indices.Add(cnt + 1);
            }
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
        return mesh;
    }

    private void AddEdge(Dictionary<(Vector3, Vector3), int> edgeCount, Vector3 a, Vector3 b)
    {
        var edge = (a, b);
        var reverseEdge = (b, a);

        if (edgeCount.ContainsKey(edge))
        {
            edgeCount[edge]++;
        }
        else if (edgeCount.ContainsKey(reverseEdge))
        {
            edgeCount[reverseEdge]++;
        }
        else
        {
            edgeCount[edge] = 1;
        }
    }
}