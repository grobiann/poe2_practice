using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MinimapCameraDrawer : MonoBehaviour, IEdgeDrawer, IMapObjectDrawer
{
    [SerializeField] private MeshRenderer _borderRenderer;
    [SerializeField] private Camera _borderCamera;

    public void DrawEdge(Texture2D texture)
    {
        int mapSize = 128;

        // NavMesh 경계선 그리기
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        Mesh mesh = CreateBorderMesh(triangulation);
        MeshFilter meshFilter = _borderRenderer.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // 카메라 설정
        Vector3 center = new Vector3(mapSize * 0.5f, 0, mapSize * 0.5f);
        _borderCamera.transform.position = center + new Vector3(0, 100, 0);
        _borderCamera.orthographicSize = mapSize * 0.5f;
        _borderCamera.allowHDR = false;
        _borderCamera.clearFlags = CameraClearFlags.SolidColor;
        _borderCamera.backgroundColor = new Color(0, 0, 0, 0);
        _borderCamera.Render();

        // RenderTexture에서 Texture2D로 복사
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = _borderCamera.targetTexture;
        texture.ReadPixels(new Rect(0, 0, mapSize, mapSize), 0, 0);
        texture.Apply();
        RenderTexture.active = currentRT;
    }

    public void DrawMapObjects(Texture2D texture)
    {
        // TODO: Implement
        return;
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

        void AddEdge(Dictionary<(Vector3, Vector3), int> edgeCount, Vector3 a, Vector3 b)
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













    //private Texture2D GenerateEdgeTexture(Vector3[] vertices, int[] triangles, int textureWidth, int textureHeight)
    //{
    //    // 1. Mesh의 경계선 찾기
    //    List<Edge> edges = FindEdges(triangles, vertices);

    //    // 2. 텍스처 크기 및 초기화
    //    Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
    //    Color[] whitePixels = new Color[textureWidth * textureHeight];
    //    for (int i = 0; i < whitePixels.Length; i++) whitePixels[i] = Color.black;  // 기본적으로 검은색
    //    texture.SetPixels(whitePixels);

    //    // 3. Vertices를 2D 공간으로 매핑
    //    Bounds bounds = new Bounds(vertices[0], Vector3.zero);
    //    foreach (var vertex in vertices) bounds.Encapsulate(vertex);
    //    Vector2[] projectedVertices = MinimapUtil.ProjectVerticesTo2D(vertices, textureWidth, textureHeight, bounds);

    //    // 4. 경계선 그리기
    //    foreach (var edge in edges)
    //    {
    //        Vector2 start = projectedVertices[edge.vertex1];
    //        Vector2 end = projectedVertices[edge.vertex2];
    //        DrawLine(texture, start, end, Color.white); // 흰색으로 선 그리기
    //    }

    //    texture.Apply();
    //    return texture;
    //}

    //private void DrawLine(Texture2D texture, Vector2 start, Vector2 end, Color color)
    //{
    //    int x1 = Mathf.FloorToInt(start.x);
    //    int y1 = Mathf.FloorToInt(start.y);
    //    int x2 = Mathf.FloorToInt(end.x);
    //    int y2 = Mathf.FloorToInt(end.y);

    //    int dx = Mathf.Abs(x2 - x1);
    //    int dy = Mathf.Abs(y2 - y1);
    //    int sx = x1 < x2 ? 1 : -1;
    //    int sy = y1 < y2 ? 1 : -1;
    //    int err = dx - dy;

    //    while (true)
    //    {
    //        if (x1 >= 0 && x1 < texture.width && y1 >= 0 && y1 < texture.height)
    //            texture.SetPixel(x1, y1, color);

    //        if (x1 == x2 && y1 == y2) break;
    //        int e2 = err * 2;
    //        if (e2 > -dy)
    //        {
    //            err -= dy;
    //            x1 += sx;
    //        }
    //        if (e2 < dx)
    //        {
    //            err += dx;
    //            y1 += sy;
    //        }
    //    }
    //}

    //private List<Edge> FindEdges(int[] triangles, Vector3[] vertices)
    //{
    //    List<Edge> edges = new List<Edge>();

    //    for (int i = 0; i < triangles.Length; i += 3)
    //    {
    //        // 삼각형의 3개 점
    //        int idx0 = triangles[i];
    //        int idx1 = triangles[i + 1];
    //        int idx2 = triangles[i + 2];

    //        // 에지 정의 (각 두 점 간의 연결)
    //        edges.Add(new Edge(idx0, idx1));
    //        edges.Add(new Edge(idx1, idx2));
    //        edges.Add(new Edge(idx2, idx0));
    //    }

    //    // 중복된 에지를 제거 (순서가 반대인 경우에도 하나의 에지로 취급)
    //    edges = edges.Distinct().ToList();
    //    return edges;
    //}

    //public struct Edge
    //{
    //    public int vertex1;
    //    public int vertex2;

    //    public Edge(int v1, int v2)
    //    {
    //        vertex1 = v1 < v2 ? v1 : v2;  // 항상 작은 값을 먼저
    //        vertex2 = v1 < v2 ? v2 : v1;  // 항상 큰 값을 나중에
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj is Edge other)
    //        {
    //            return vertex1 == other.vertex1 && vertex2 == other.vertex2;
    //        }
    //        return false;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return vertex1 ^ vertex2;
    //    }
    //}


    //private void OverlayEdgeTextureWithDrawTexture(RenderTexture mainTex, Texture2D edgeTexture)
    //{
    //    // 1. mainTex를 활성화
    //    RenderTexture currentRT = RenderTexture.active;
    //    RenderTexture.active = mainTex;

    //    // 2. Texture2D를 RenderTexture에 그리기
    //    Rect rect = new Rect(0, 0, mainTex.width, mainTex.height);
    //    Graphics.DrawTexture(rect, edgeTexture);

    //    // 3. 작업 완료 후 원래 활성화된 RenderTexture로 복원
    //    RenderTexture.active = currentRT;
    //}
}
