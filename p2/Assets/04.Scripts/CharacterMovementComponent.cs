using System;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovementComponent : MonoBehaviour
{
    public float MoveSpeed
    {
        get => _moveSpeed;
        set { _moveSpeed = value; }
    }
    public float CurrentMoveSpeed => Velocity.magnitude;
    public Vector3 Velocity { get; private set; }
    public Vector3 Forward { get; private set; }

    [SerializeField] private float _moveSpeed = 2.0f;

    private bool _isMoving;

    public event Action OnMove = delegate { };
    public event Action OnStop = delegate { };

    private void Awake()
    {
        SetForward(Vector3.forward);

        Vector3 targetPosition = transform.position;
        targetPosition.y = GetGroundHeight(targetPosition);
        transform.position = targetPosition;
    }

    private void FixedUpdate()
    {
        bool shouldMove = CurrentMoveSpeed > float.Epsilon;
        if (shouldMove)
        {
            _isMoving = true;

            Vector3 moveOffset = Time.fixedDeltaTime * Velocity;
            Vector3 targetPosition = transform.position + moveOffset;
            targetPosition.y = GetGroundHeight(targetPosition);
            transform.position = targetPosition;

            OnMove();
        }
        else if (_isMoving && shouldMove == false)
        {
            _isMoving = false;

            OnStop();
        }
    }

    public void MoveByDirection(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            Velocity = Vector3.zero;
            return;
        }

        direction.y = 0;
        direction.Normalize();
        Velocity = direction * MoveSpeed;
        Forward = direction;
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
