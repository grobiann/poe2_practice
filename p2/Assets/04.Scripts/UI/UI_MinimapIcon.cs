using p2.mmap;
using UnityEngine;
using UnityEngine.UI;

public class UI_MinimapIcon : MonoBehaviour
{
    [SerializeField] private Image _image;

    public MinimapIconAttribute Attribute { get; private set; }

    public void SetAttribute(MinimapIconAttribute attribute)
    {
        Attribute = attribute;

        _image.sprite = attribute.icon;
    }

    public void SetOpacity(float opacity)
    {
        Color color = _image.color;
        color.a = opacity;
        _image.color = color;
    }

    public void Refresh()
    {
        gameObject.SetActive(Attribute.revealed);
    }
}
