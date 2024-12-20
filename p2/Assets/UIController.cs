using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private KeyCode key_settings = KeyCode.Escape;

    [SerializeField] private GameObject _settingsPanel;

    private void Update()
    {
        if (Input.GetKeyDown(key_settings))
        {
            _settingsPanel.gameObject.SetActive(!_settingsPanel.gameObject.activeInHierarchy);
        }
    }
}
