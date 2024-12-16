using JetBrains.Annotations;
using UnityEngine;

public class MapDevice : MonoBehaviour, IMouseClickable
{
    public void OnClick()
    {
        // TODO: Enter Other Map
    }
}

public interface IMouseClickable
{
    void OnClick();
}