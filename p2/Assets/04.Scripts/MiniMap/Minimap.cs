using System.Collections.Generic;
using UnityEngine;

namespace p2.mmap
{
    public class MinimapIconAttribute
    {
        public Vector2 uv;
        public bool revealed;
        public Sprite icon;
        public Transform followTarget;
    }

    public class Minimap : MonoBehaviour
    {
        [Header("Fog of War")]
        [SerializeField] private List<Transform> _fogAgents;

        [Header("Drawers")]
        [SerializeField] private MinimapCameraDrawer _minimapCameraDrawer;
        [SerializeField] private FogDrawer _fogDrawer;

        public List<MinimapIconAttribute> icons = new List<MinimapIconAttribute>();
        public Material Material => _drawer.Material;

        private MinimapDrawer _drawer;
        private Map _map;
        private float _tileSize = 1.0f;
        private int _textureSize = 512;
        private float _worldToUv;

        private void Awake()
        {
            // 초기 fogAgent 설정
            PlayerCharacter player = Object.FindFirstObjectByType<PlayerCharacter>();
            AddAgent(player.transform);

            // minimap 초기화
            _map = Object.FindFirstObjectByType<Map>();

            _drawer = new MinimapDrawer(
                _textureSize,
                _minimapCameraDrawer,
                _fogDrawer,
                _minimapCameraDrawer,
                _minimapCameraDrawer);
            _drawer.Init();
            _worldToUv = 1.0f / _map.MapSize / _tileSize;

            // minimap icon 초기화
            MinimapIconable[] iconables = Resources.FindObjectsOfTypeAll<MinimapIconable>();
            foreach (MinimapIconable i in iconables)
            {
                Sprite spr = i.icon;
                icons.Add(new MinimapIconAttribute
                {
                    uv = WorldPositonToUV(i.transform.position),
                    revealed = false,
                    icon = spr,
                    followTarget = i.transform
                });
            }

        }

        private void Update()
        {
            foreach (Transform agent in _fogAgents)
            {
                _drawer.UpdateFogOfWar(agent.transform.position);
            }

            foreach (MinimapIconAttribute icon in icons)
            {
                UpdateIconPosition(icon);
                UpdateIconReveal(icon);
            }
        }

        private void UpdateIconPosition(MinimapIconAttribute icon)
        {
            if (icon.followTarget != null)
            {
                icon.uv = WorldPositonToUV(icon.followTarget.position);
            }
        }

        private void UpdateIconReveal(MinimapIconAttribute icon)
        {
            if (icon.revealed)
                return;

            foreach (Transform agent in _fogAgents)
            {
                Vector2 agentUV = WorldPositonToUV(agent.position);
                Vector2 iconUV = icon.uv;

                float distance = Vector3.Distance(agentUV, iconUV);
                if (distance < _fogDrawer.RevealRadius * _worldToUv)
                {
                    icon.revealed = true;
                }
            }
        }

        public void SetLandscapeOpacity(float opacity)
        {
            _drawer.SetLandscapeOpacity(opacity);
        }

        public void SetMapOpacity(float opacity)
        {
            _drawer.SetMapOpacity(opacity);
        }

        public void AddAgent(Transform agent)
        {
            if (_fogAgents.Contains(agent))
                return;

            _fogAgents.Add(agent);
        }

        public bool RemoveAgent(Transform agnet)
        {
            return _fogAgents.Remove(agnet);
        }

        public Vector2 WorldPositonToUV(Vector3 worldPosition)
        {
            float x = worldPosition.x * _worldToUv;
            float y = worldPosition.z * _worldToUv;
            return new Vector2(x, y);
        }

        public Rect ExtractRect(Vector3 worldPosition, float worldWidth, float worldHeight)
        {
            float mapSize = _map.MapSize;
            float divider = 1 / mapSize / _tileSize;
            float minx = (worldPosition.x - worldWidth * 0.5f) * divider;
            float miny = (worldPosition.z - worldHeight * 0.5f) * divider;
            float width = worldWidth * divider;
            float height = worldHeight * divider;

            return new Rect(minx, miny, width, height);
        }

        public void Draw(RenderTexture texture)
        {
            _drawer.Draw(texture);
        }
    }
}