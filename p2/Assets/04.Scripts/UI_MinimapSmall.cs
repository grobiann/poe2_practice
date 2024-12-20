﻿using p2.Settings;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapSmall : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;

    public float FieldOfView { get; private set; } = 50.0f;
    
    private Minimap _minimap;

    private void Start()
    {
        RenderTexture renderTexture = _rawImage.texture as RenderTexture;
        
        _minimap = GameManager.Instance.CurrentGameMode.Minimap;
        _minimap.Draw(renderTexture);
        _rawImage.material = _minimap.Material;
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Update()
    {
        Vector3 playerPosition = GameManager.Instance.CurrentGameMode.MyPlayerCharacter.transform.position;
        _rawImage.uvRect = _minimap.ExtractRect(playerPosition, FieldOfView, FieldOfView);
    }

    public void Refresh()
    {
        GameSettings.MapAttribute setting = new GameSettings.MapAttribute();
        FieldOfView = setting.MinimapFOV;
    }
}