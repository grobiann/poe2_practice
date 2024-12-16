using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera CameraComponent => _cam;

    [SerializeField] private Camera _cam;
    [SerializeField] private Vector3 _offset = new Vector3(0, 9, -5);

    private GameObject _target;

    private void Update()
    {
        if(_target != null)
        {
            transform.position = _target.transform.position + _offset;
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        transform.position = _target.transform.position + _offset;
        transform.LookAt(_target.transform);
    }
}

