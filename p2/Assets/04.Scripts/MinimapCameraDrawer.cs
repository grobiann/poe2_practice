using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace p2.Minimap
{
    public class MinimapCameraDrawer : MonoBehaviour, IEdgeDrawer, IMapObjectDrawer, IMovableDrawer
    {
        [SerializeField] private MeshRenderer _borderRenderer;
        [SerializeField] private MeshRenderer _movableRenderer;
        [SerializeField] private Camera _borderCamera;
        [SerializeField] private Camera _movableCamera;

        private int _mapSize = 128;

        public void DrawEdge(Texture2D texture)
        {
            _borderRenderer.gameObject.SetActive(true);
            _borderCamera.gameObject.SetActive(true);

            // NavMesh 경계선 그리기
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            Mesh mesh = CreateBorderMesh(triangulation);
            MeshFilter meshFilter = _borderRenderer.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            // 카메라 설정
            Vector3 center = new Vector3(_mapSize * 0.5f, 0, _mapSize * 0.5f);
            _borderCamera.transform.position = center + new Vector3(0, 100, 0);
            _borderCamera.orthographicSize = _mapSize * 0.5f;
            _borderCamera.allowHDR = false;
            _borderCamera.clearFlags = CameraClearFlags.SolidColor;
            _borderCamera.backgroundColor = new Color(0, 0, 0, 0);
            _borderCamera.Render();

            // RenderTexture에서 Texture2D로 복사
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = _borderCamera.targetTexture;
            texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture.Apply();
            RenderTexture.active = currentRT;

            _borderRenderer.gameObject.SetActive(false);
            _borderCamera.gameObject.SetActive(false);
        }

        public void DrawMapObjects(Texture2D texture)
        {
            // TODO: Implement
            return;
        }

        public void DrawMovable(Texture2D texture)
        {
            _movableCamera.gameObject.SetActive(true);
            _movableRenderer.gameObject.SetActive(true);

            // NavMesh 경계선 그리기
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            Mesh mesh = CreateMovableMesh(triangulation);
            MeshFilter meshFilter = _movableRenderer.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            // 카메라 설정
            Vector3 center = new Vector3(_mapSize * 0.5f, 0, _mapSize * 0.5f);
            _movableCamera.transform.position = center + new Vector3(0, 100, 0);
            _movableCamera.orthographicSize = _mapSize * 0.5f;
            _movableCamera.allowHDR = false;
            _movableCamera.clearFlags = CameraClearFlags.SolidColor;
            _movableCamera.backgroundColor = new Color(0, 0, 0, 0);
            _movableCamera.Render();

            // RenderTexture에서 Texture2D로 복사
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = _movableCamera.targetTexture;
            texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture.Apply();
            RenderTexture.active = currentRT;

            _movableCamera.gameObject.SetActive(false);
            _movableRenderer.gameObject.SetActive(false);
        }

        private Mesh CreateMovableMesh(NavMeshTriangulation triangulation)
        {
            Mesh mesh = new Mesh();
            mesh.SetVertices(triangulation.vertices);
            mesh.SetIndices(triangulation.indices, MeshTopology.Triangles, 0);
            return mesh;
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
    }
}