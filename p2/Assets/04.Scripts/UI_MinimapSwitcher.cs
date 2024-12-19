using UnityEngine;

public class UI_MinimapSwitcher : MonoBehaviour
{
    public enum EMinimapState
    {
        Full,
        Small
    }

    [SerializeField] private GameObject _fullMinimap;
    [SerializeField] private GameObject _smallMinimap;

    private void Start()
    {
        _fullMinimap.SetActive(true);
        _smallMinimap.SetActive(false);
    }

    public void SwitchMinimap()
    {
        _fullMinimap.SetActive(!_fullMinimap.activeSelf);
        _smallMinimap.SetActive(!_smallMinimap.activeSelf);
    }

    public void SetMinimapState(EMinimapState state)
    {
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
