using UnityEditor.PackageManager;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Vector3 MoveVelocity => _movement.Velocity;
    public float MoveSpeed => _movement.MoveSpeed;

    [SerializeField] protected CharacterMovementComponent _movement;
    [SerializeField] protected CharacterMeshComponent _mesh;

    private int hMoveAnimHash = Animator.StringToHash("Horizontal");
    private int vMoveAnimHash = Animator.StringToHash("Vertical");

    protected virtual void Awake()
    {
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

    public void MoveByDirection(Vector3 direction)
    {
        _movement.MoveByDirection(direction);

        RefreshAnimParameter();
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

