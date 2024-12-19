using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapSmall : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;

    private Minimap _minimap;

    private void Start()
    {
        RenderTexture renderTexture = _rawImage.texture as RenderTexture;
        
        //renderTexture.Clear();
        _minimap = GameManager.Instance.CurrentGameMode.Minimap;
        _minimap.Draw(renderTexture);

        _rawImage.material = _minimap.Material;
    }
}