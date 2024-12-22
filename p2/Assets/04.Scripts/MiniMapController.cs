using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

namespace p2.Minimap
{
    public class MiniMapController : MonoBehaviour
    {
        public List<Transform> _fogAgents = new List<Transform>();
        public Minimap Minimap => _minimap;

        [Header("Fog of War")]
        [SerializeField] private float revealRadius = 10.0f;
        [SerializeField] private float edgeSharpness = 3f;

        private FogOfWar _fogOfWar;
        private MinimapCameraDrawer _minimapCameraDrawer;
        private MinimapMovableDrawer _walkableDrawer;
        private Minimap _minimap;

        private void OnValidate()
        {
            UpdateFogOfWarParams();
        }

        private void UpdateFogOfWarParams()
        {
            if (_fogOfWar != null)
            {
                _fogOfWar.RevealRadius = revealRadius;
                _fogOfWar.EdgeSharpness = edgeSharpness;
            }
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

        private void Awake()
        {
            // 초기 fogAgent 설정
            PlayerCharacter player = Object.FindFirstObjectByType<PlayerCharacter>();
            AddAgent(player.transform);

            // minimap 초기화
            int textureSize = 512;
            _minimapCameraDrawer = Object.FindFirstObjectByType<MinimapCameraDrawer>();
            _fogOfWar = new FogOfWar(textureSize);
            _walkableDrawer = new MinimapMovableDrawer();

            UpdateFogOfWarParams();

            _minimap = new Minimap(
                textureSize,
                _minimapCameraDrawer,
                _fogOfWar,
                _minimapCameraDrawer,
                _minimapCameraDrawer);
            _minimap.Init();
        }

        private void Update()
        {
            foreach (Transform agent in _fogAgents)
            {
                _minimap.UpdateFogOfWar(agent.transform.position);
            }
        }
    }
}