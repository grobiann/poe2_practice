using p2.mmap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapFull : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private float _moveSpeedByInputKey = 50.0f;
    [SerializeField] private float _moveSpeedToCenter = 50.0f;

    [Header("Icon")]
    [SerializeField] private Transform _iconParent;
    [SerializeField] private UI_MinimapIcon _iconPrefab;

    public bool IsCenter => _offset == Vector2.zero;
    public bool IsMovingToCenter { get; private set; }

    private Minimap _minimap;
    private Vector2 _offset;
    private List<UI_MinimapIcon> _icons;

    private void Start()
    {
        RenderTexture renderTexture = _rawImage.texture as RenderTexture;

        _minimap = Object.FindFirstObjectByType<Minimap>();
        _minimap.Draw(renderTexture);
        _rawImage.material = _minimap.Material;

        _icons = new List<UI_MinimapIcon>();
        _minimap.icons.ForEach(attribute =>
        {   
            UI_MinimapIcon icon = Instantiate(_iconPrefab, _iconParent);
            icon.SetAttribute(attribute);
            ((RectTransform)icon.transform).anchoredPosition = ((RectTransform)_iconParent).UvToPosition(attribute.uv);
            _icons.Add(icon);
        });
    }


    private void OnEnable()
    {
        if (IsMovingToCenter)
        {
            // 기존에 중앙으로 이동중이었다면 코루틴을 다시 시작한다.
            StartCoroutine(MoveToCenterCoroutine());
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _offset.x -= _moveSpeedByInputKey * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _offset.x += _moveSpeedByInputKey * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _offset.y += _moveSpeedByInputKey * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _offset.y -= _moveSpeedByInputKey * Time.deltaTime;
        }

        // Makes the player's position centered on the screen.
        Vector3 playerPosition = GameManager.Instance.CurrentGameMode.MyPlayerCharacter.transform.position;
        Vector2 playerUV = _minimap.WorldPositonToUV(playerPosition);
        Vector2 centerOffsetUV = new Vector2(0.5f, 0.5f) - playerUV;

        RectTransform rectTransform = (RectTransform)transform;
        Vector2 rectSize = rectTransform.sizeDelta;
        rectTransform.anchoredPosition = centerOffsetUV * rectSize + _offset;

        // Update Icon
        foreach (UI_MinimapIcon icon in _icons)
        {
            icon.Refresh();
        }
    }

    public void MoveToCenter()
    {
        if (IsCenter)
            return;

        if (IsMovingToCenter)
            return;

        StartCoroutine(MoveToCenterCoroutine());
    }

    private IEnumerator MoveToCenterCoroutine()
    {
        IsMovingToCenter = true;

        float moveSpeed = _moveSpeedToCenter;
        Vector2 prevOffset = _offset;
        float duration = prevOffset.magnitude / moveSpeed;
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            _offset = Vector2.Lerp(prevOffset, Vector2.zero, t);
            yield return null;
        }

        _offset = Vector2.zero;
        IsMovingToCenter = false;
    }
}
