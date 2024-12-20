using p2.Settings;
using UnityEngine;

public class MinimapUiController : MonoBehaviour
{
    public enum EMinimapState
    {
        Full,
        Small
    }

    public EMinimapState CurrentState { get; private set; }


    [SerializeField] private UI_MinimapFull _fullMinimap;
    [SerializeField] private UI_MinimapSmall _smallMinimap;
    [SerializeField] private KeyCode _minimapSwitchKey = KeyCode.Tab;

    public bool EnableSmallMinimap { get; private set; }
    public bool AutoMap { get; private set; }

    private void Start()
    {
        SetMinimapState(EMinimapState.Small);
        Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_minimapSwitchKey))
        {
            HandleSwitchState();
        }
    }

    private void HandleSwitchState()
    {
        if (CurrentState == EMinimapState.Small)
        {
            SetMinimapState(EMinimapState.Full);
            return;
        }

        if (AutoMap && _fullMinimap.IsCenter == false && _fullMinimap.IsMovingToCenter == false)
        {
            _fullMinimap.MoveToCenter();
            return;
        }

        SetMinimapState(EMinimapState.Small);
    }

    public void Refresh()
    {
        var settings = new GameSettings.MapAttribute();

        EnableSmallMinimap = settings.ShowMinimap;
        AutoMap = settings.AutoMap;

        if (CurrentState == EMinimapState.Small)
        {
            _smallMinimap.gameObject.SetActive(EnableSmallMinimap);
        }
    }

    public void SetMinimapState(EMinimapState state)
    {
        CurrentState = state;

        switch (state)
        {
            case EMinimapState.Full:
                _fullMinimap.gameObject.SetActive(true);
                _smallMinimap.gameObject.SetActive(false);
                break;
            case EMinimapState.Small:
                _fullMinimap.gameObject.SetActive(false);
                _smallMinimap.gameObject.SetActive(EnableSmallMinimap ? true : false);
                break;
            default:
                Debug.LogError("Invalid minimap state");
                break;
        }
    }
}
