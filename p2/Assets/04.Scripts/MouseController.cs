using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, Define.MouseClickable))
        {
            if (hit.collider.TryGetComponent(out IMouseClickable clickable))
            {
                // TODO: Highlight
                // clickable.Highlight();

                if (Input.GetMouseButtonDown(0))
                {
                    clickable.OnClick();
                }
            }
        }
    }
}
