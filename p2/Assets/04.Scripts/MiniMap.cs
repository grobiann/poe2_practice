using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Minimap
{
    public Material Material { get; private set; }
    public float MapAlpha { get; private set; } = 0.5f;
    public float LandscapeAlpha { get; private set; } = 0.5f;
    public float FieldOfView { get; private set; } = 90.0f;

    private FogOfWar _fogOfWar;
    private int _textureSize = 128;

    public void Init()
    {
        _fogOfWar = new FogOfWar(_textureSize);
        Material = new Material(Shader.Find("Unlit/FogOfWar"));
        Material.SetTexture("_FogTex", _fogOfWar.GetTexture());

        // Walkable 영역을 미리 계산해서, Fog of War를 표현할때 경계면 별도 처리
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        Texture2D walkableTexture = GenerateWalkableTexture(triangulation.vertices, triangulation.indices, _textureSize, _textureSize);
        Material.SetTexture("_WalkableTex", walkableTexture);

        SetMapAlpha(MapAlpha);
        SetLandscapeAlpha(LandscapeAlpha);
        SetMapFov(FieldOfView);
    }

    public void SetMapAlpha(float alpha)
    {
        MapAlpha = alpha;
        Material.SetFloat("_Alpha", alpha);
    }

    public void SetLandscapeAlpha(float alpha)
    {
        LandscapeAlpha = alpha;
        Material.SetFloat("_LandscapeAlpha", alpha);
    }

    public void SetMapFov(float fov)
    {
        FieldOfView = fov;
        Material.SetFloat("_Fov", fov);
    }

    public void Draw(RenderTexture texture)
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        Texture2D edgeTexture = GenerateEdgeTexture(triangulation.vertices, triangulation.indices, _textureSize, _textureSize);

        OverlayEdgeTextureWithDrawTexture(texture, edgeTexture);
    }

    public void UpdateFogOfWar(Vector3 playerPosition)
    {
        _fogOfWar.Reveal(playerPosition);
    }

    public Rect ExtractTextureRegion(Vector3 position)
    {
        // fov에 따라 전체 Texture중 표현해야 할 UV Rect 반환
        // TODO: Implement ExtractTextureRegion
        return new Rect(0, 0, 1, 1);
    }

    private Texture2D GenerateWalkableTexture(Vector3[] vertices, int[] triangles, int textureWidth, int textureHeight)
    {
        // 1. Mesh Bounds 계산
        Bounds bounds = new Bounds(vertices[0], Vector3.zero);
        foreach (var vertex in vertices) bounds.Encapsulate(vertex);

        // 2. Vertices를 2D로 투영
        Vector2[] projectedVertices = ProjectVerticesTo2D(vertices, textureWidth, textureHeight, bounds);

        // 3. Walkable Area 초기화
        bool[,] walkableArea = new bool[textureWidth, textureHeight];

        // 4. 삼각형을 Walkable Area에 렌더링
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector2 p1 = projectedVertices[triangles[i]];
            Vector2 p2 = projectedVertices[triangles[i + 1]];
            Vector2 p3 = projectedVertices[triangles[i + 2]];
            RasterizeTriangle(p1, p2, p3, walkableArea);
        }

        // 5. Bool 배열을 Texture2D로 변환
        Texture2D texture = ConvertBoolArrayToTexture2D(walkableArea);
        return texture;
    }

    private Texture2D GenerateEdgeTexture(Vector3[] vertices, int[] triangles, int textureWidth, int textureHeight)
    {
        // 1. Mesh의 경계선 찾기
        List<Edge> edges = FindEdges(triangles, vertices);

        // 2. 텍스처 크기 및 초기화
        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        Color[] whitePixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < whitePixels.Length; i++) whitePixels[i] = Color.black;  // 기본적으로 검은색
        texture.SetPixels(whitePixels);

        // 3. Vertices를 2D 공간으로 매핑
        Bounds bounds = new Bounds(vertices[0], Vector3.zero);
        foreach (var vertex in vertices) bounds.Encapsulate(vertex);
        Vector2[] projectedVertices = ProjectVerticesTo2D(vertices, textureWidth, textureHeight, bounds);

        // 4. 경계선 그리기
        foreach (var edge in edges)
        {
            Vector2 start = projectedVertices[edge.vertex1];
            Vector2 end = projectedVertices[edge.vertex2];
            DrawLine(texture, start, end, Color.white); // 흰색으로 선 그리기
        }

        texture.Apply();
        return texture;
    }

    private void OverlayEdgeTextureWithDrawTexture(RenderTexture mainTex, Texture2D edgeTexture)
    {
        // 1. mainTex를 활성화
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = mainTex;

        // 2. Texture2D를 RenderTexture에 그리기
        Rect rect = new Rect(0, 0, mainTex.width, mainTex.height);
        Graphics.DrawTexture(rect, edgeTexture);

        // 3. 작업 완료 후 원래 활성화된 RenderTexture로 복원
        RenderTexture.active = currentRT;
    }

    #region Render Functions
    private Vector2[] ProjectVerticesTo2D(Vector3[] vertices, float width, float height, Bounds bounds)
    {
        Vector2[] projectedVertices = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            float normalizedX = (vertices[i].x - bounds.min.x) / bounds.size.x;
            float normalizedY = (vertices[i].z - bounds.min.z) / bounds.size.z; // Z축 사용
            projectedVertices[i] = new Vector2(normalizedX * width, normalizedY * height);
        }
        return projectedVertices;
    }

    private void RasterizeTriangle(Vector2 p1, Vector2 p2, Vector2 p3, bool[,] walkableArea)
    {
        int width = walkableArea.GetLength(0);
        int height = walkableArea.GetLength(1);

        // 삼각형의 AABB (Axis-Aligned Bounding Box)
        int minX = Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(p1.x, Mathf.Min(p2.x, p3.x))), 0, width - 1);
        int maxX = Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(p1.x, Mathf.Max(p2.x, p3.x))), 0, width - 1);
        int minY = Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(p1.y, Mathf.Min(p2.y, p3.y))), 0, height - 1);
        int maxY = Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(p1.y, Mathf.Max(p2.y, p3.y))), 0, height - 1);

        // 삼각형 내부 체크 (Barycentric 좌표 사용)
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Vector2 point = new Vector2(x + 0.5f, y + 0.5f); // 픽셀 중심
                if (IsPointInTriangle(point, p1, p2, p3))
                {
                    walkableArea[x, y] = true;
                }
            }
        }
    }

    private bool IsPointInTriangle(Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float d1 = Sign(p, p1, p2);
        float d2 = Sign(p, p2, p3);
        float d3 = Sign(p, p3, p1);

        bool hasNegative = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool hasPositive = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNegative && hasPositive);
    }

    private float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    private Texture2D ConvertBoolArrayToTexture2D(bool[,] walkableArea)
    {
        int width = walkableArea.GetLength(0);
        int height = walkableArea.GetLength(1);

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, walkableArea[x, y] ? Color.white : Color.black);
            }
        }
        texture.Apply();
        return texture;
    }

    private void DrawLine(Texture2D texture, Vector2 start, Vector2 end, Color color)
    {
        int x1 = Mathf.FloorToInt(start.x);
        int y1 = Mathf.FloorToInt(start.y);
        int x2 = Mathf.FloorToInt(end.x);
        int y2 = Mathf.FloorToInt(end.y);

        int dx = Mathf.Abs(x2 - x1);
        int dy = Mathf.Abs(y2 - y1);
        int sx = x1 < x2 ? 1 : -1;
        int sy = y1 < y2 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (x1 >= 0 && x1 < texture.width && y1 >= 0 && y1 < texture.height)
                texture.SetPixel(x1, y1, color);

            if (x1 == x2 && y1 == y2) break;
            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
    }

    private List<Edge> FindEdges(int[] triangles, Vector3[] vertices)
    {
        List<Edge> edges = new List<Edge>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            // 삼각형의 3개 점
            int idx0 = triangles[i];
            int idx1 = triangles[i + 1];
            int idx2 = triangles[i + 2];

            // 에지 정의 (각 두 점 간의 연결)
            edges.Add(new Edge(idx0, idx1));
            edges.Add(new Edge(idx1, idx2));
            edges.Add(new Edge(idx2, idx0));
        }

        // 중복된 에지를 제거 (순서가 반대인 경우에도 하나의 에지로 취급)
        edges = edges.Distinct().ToList();
        return edges;
    }
    #endregion

    public struct Edge
    {
        public int vertex1;
        public int vertex2;

        public Edge(int v1, int v2)
        {
            vertex1 = v1 < v2 ? v1 : v2;  // 항상 작은 값을 먼저
            vertex2 = v1 < v2 ? v2 : v1;  // 항상 큰 값을 나중에
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge other)
            {
                return vertex1 == other.vertex1 && vertex2 == other.vertex2;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return vertex1 ^ vertex2;
        }
    }
}
