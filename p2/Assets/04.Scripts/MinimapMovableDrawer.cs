using UnityEngine;
using UnityEngine.AI;

namespace p2.Minimap
{
    public class MinimapMovableDrawer : IMovableDrawer
    {
        public void DrawMovable(Texture2D texture)
        {
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            DrawMovableTexture(texture, triangulation.vertices, triangulation.indices);
        }

        private void DrawMovableTexture(Texture2D texture, Vector3[] vertices, int[] triangles)
        {
            int textureWidth = texture.width;
            int textureHeight = texture.height;

            // 1. Mesh Bounds 계산
            Vector3 center = new Vector3(textureWidth * 0.5f, 0, textureHeight * 0.5f);
            Vector3 size = new Vector3(textureWidth, 0, textureHeight);
            Bounds bounds = new Bounds(center, size);

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
            WriteBoolArrayToTexture2D(texture, walkableArea);
        }

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

        private void WriteBoolArrayToTexture2D(Texture2D texture, bool[,] walkableArea)
        {
            int width = walkableArea.GetLength(0);
            int height = walkableArea.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, walkableArea[x, y] ? Color.white : Color.black);
                }
            }
            texture.Apply();
        }
    }
}