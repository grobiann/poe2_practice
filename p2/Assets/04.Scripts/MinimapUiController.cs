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

    [SerializeField] private GameObject _fullMinimap;
    [SerializeField] private GameObject _smallMinimap;
    [SerializeField] private KeyCode _minimapSwitchKey = KeyCode.Tab;

    private void Start()
    {
        SetMinimapState(EMinimapState.Small);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_minimapSwitchKey))
        {
            SwitchMinimap();
        }
    }

    public void SwitchMinimap()
    {
        _fullMinimap.SetActive(!_fullMinimap.activeSelf);
        _smallMinimap.SetActive(!_smallMinimap.activeSelf);
    }

    public void SetMinimapState(EMinimapState state)
    {
        CurrentState = state;

        switch (state)
        {
            case EMinimapState.Full:
                _fullMinimap.SetActive(true);
                _smallMinimap.SetActive(false);
                break;
            case EMinimapState.Small:
                _fullMinimap.SetActive(false);
                _smallMinimap.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid minimap state");
                break;
        }
    }
}
