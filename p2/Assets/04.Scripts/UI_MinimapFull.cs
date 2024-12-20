using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapFull : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;

    private Minimap _minimap;

    private void Start()
    {
        RenderTexture renderTexture = _rawImage.texture as RenderTexture;

        _minimap = GameManager.Instance.CurrentGameMode.Minimap;
        _minimap.Draw(renderTexture);
        _rawImage.material = _minimap.Material;
    }

    private void Update()
    {
        // Makes the player's position centered on the screen.
        Vector3 playerPosition = GameManager.Instance.CurrentGameMode.MyPlayerCharacter.transform.position;
        Rect uvRect = _minimap.ExtractRect(playerPosition, 0, 0);
        Vector2 centerOffsetUV = new Vector2(0.5f, 0.5f) - uvRect.center;
        
        RectTransform rectTransform = (RectTransform)transform;
        Vector2 rectSize = rectTransform.sizeDelta;
        rectTransform.anchoredPosition = centerOffsetUV * rectSize;
    }
}
