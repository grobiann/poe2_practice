using p2.mmap;
using p2.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapSmall : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private float _fieldOfView = 50.0f;

    [Header("Icon")]
    [SerializeField] private Transform _iconParent;
    [SerializeField] private UI_MinimapIcon _iconPrefab;
    [SerializeField] private FloatPadding _padding;

    private Minimap _minimap;
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
            UI_MinimapIcon iconObject = Instantiate(_iconPrefab, _iconParent);
            iconObject.SetAttribute(attribute);
            _icons.Add(iconObject);
        });
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Update()
    {
        Vector3 playerPosition = GameManager.Instance.CurrentGameMode.MyPlayerCharacter.transform.position;
        Rect uvRect = _minimap.ExtractRect(playerPosition, _fieldOfView, _fieldOfView);
        _rawImage.uvRect = uvRect;

        // Update Icon
        RectTransform parent = _iconParent as RectTransform;
        Vector2 playerUV = _minimap.WorldPositonToUV(playerPosition);
        Rect paddedBoundary = new Rect(
            uvRect.xMin + _padding.left * uvRect.width,
            uvRect.yMin + _padding.bottom * uvRect.height,
            uvRect.width * (1.0f - _padding.horizontal),
            uvRect.height * (1.0f - _padding.vertical));

        foreach (UI_MinimapIcon icon in _icons)
        {
            icon.Refresh();

            Vector2 intersectionUV = GetBoundaryIntersection(playerUV, icon.Attribute.uv, paddedBoundary);
            Vector2 uv = new Vector2(
                (intersectionUV.x - uvRect.xMin) / uvRect.width,
                (intersectionUV.y - uvRect.yMin) / uvRect.height);
            ((RectTransform)icon.transform).anchoredPosition = parent.UvToPosition(uv);

        }
    }

    public void Refresh()
    {
        GameSettings.MapAttribute setting = new GameSettings.MapAttribute();
        _fieldOfView = setting.MinimapFOV;
    }

    private Vector2 GetBoundaryIntersection(Vector2 iconUV, Rect boundary)
    {
        //Vector2 intersection = iconUV;
        Vector2 center = boundary.center;
        Vector2 diagonalVector = boundary.max - center;
        float diagonalSin = diagonalVector.y / diagonalVector.magnitude;

        Vector2 iconVector = iconUV - center;
        float iconSin = iconVector.y / iconVector.magnitude;

        bool hitHorizontal = Mathf.Abs(iconSin) >= diagonalSin;
        if (hitHorizontal)
        {
            Vector2 intersection = new Vector2(
                center.x + iconVector.x * (boundary.height * 0.5f) / Mathf.Abs(iconVector.y),
                iconVector.y > 0 ? boundary.yMax : boundary.yMin);
        }
        else
        {
            Vector2 intersection = new Vector2(
                center.x + iconVector.x * (boundary.height * 0.5f) / Mathf.Abs(iconVector.y),
                iconVector.y > 0 ? boundary.yMax : boundary.yMin);
        }


        if (intersection.x < boundary.xMin)
        {
            float t = (boundary.xMin - intersection.x) / (playerUV.x - intersection.x);
            intersection = new Vector2(boundary.xMin, intersection.y + t * (playerUV.y - intersection.y));
        }
        else if (intersection.x > boundary.xMax)
        {
            float t = (intersection.x - boundary.xMax) / (playerUV.x - intersection.x);
            intersection = new Vector2(boundary.xMax, intersection.y + t * (playerUV.y - intersection.y));
        }

        if (intersection.y < boundary.yMin)
        {
            float t = (boundary.yMin - intersection.y) / (playerUV.y - intersection.y);
            intersection = new Vector2(intersection.x + t * (playerUV.x - intersection.x), boundary.yMin);
        }
        else if (intersection.y > boundary.yMax)
        {
            float t = (intersection.y - boundary.yMax) / (playerUV.y - intersection.y);
            intersection = new Vector2(intersection.x - t * (playerUV.x - intersection.x), boundary.yMax);
        }

        return intersection;
    }
}
