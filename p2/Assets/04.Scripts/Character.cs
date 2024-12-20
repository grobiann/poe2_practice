using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public Vector3 MoveVelocity => _movement.Velocity;
    public float MoveSpeed => _movement.MoveSpeed;

    public CharacterMovementComponent Movement => _movement;
    [SerializeField] protected CharacterMovementComponent _movement;
    [SerializeField] protected CharacterMeshComponent _mesh;

    private int hMoveAnimHash = Animator.StringToHash("Horizontal");
    private int vMoveAnimHash = Animator.StringToHash("Vertical");

    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {
        RefreshAnimParameter();
    }

    public void RotateTowards(Vector3 targetDirection)
    {
        targetDirection.y = 0; // Keep the direction strictly horizontal
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _movement.MoveSpeed);

            _movement.SetForward(targetDirection);
        }
    }


    public void SetMoveDestination(Vector3 destination)
    {
        _movement.MoveToDestination(destination);
    }

    public void SetMoveDirection(Vector3 direction)
    {
        _movement.MoveToDirection(direction);
    }

    private void RefreshAnimParameter()
    {
        if (_mesh)
        {
            _mesh.SetAnimationParameter(hMoveAnimHash, _movement.Velocity.x);
            _mesh.SetAnimationParameter(vMoveAnimHash, _movement.Velocity.z);
        }
    }
}

