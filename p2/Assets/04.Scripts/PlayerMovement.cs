using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed => _moveSpeed;
    public float CurrentMoveSpeed => Velocity.magnitude;
    public Vector3 Velocity { get; private set; }
    public Vector3 Forward { get; private set; }

    [SerializeField] private float _moveSpeed = 2.0f;

    private void Awake()
    {
        SetForward(Vector3.forward);
    }

    public void Move(Vector3 offset)
    {
        Vector3 targetPosition = transform.position + offset;
        targetPosition.y = GetGroundHeight(targetPosition);
        transform.position = targetPosition;
    }

    public void SetForward(Vector3 direction)
    {
        Forward = direction;
        transform.forward = direction;
    }

    private float GetGroundHeight(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, Define.GroundLayer))
        {
            return hit.point.y;
        }
        return position.y;
    }
}
