using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public interface ICharacterMovement
{
    float MaxMoveSpeed { get; set; }
    Vector3 Velocity { get; }

    void Update(float tick);
    void MoveToDestination(Vector3 destination);
    void MoveToDirection(Vector3 direction);
    void Stop();

    event Action OnStop;
}

public class MoveByNavMeshAgent : ICharacterMovement
{
    public float MaxMoveSpeed
    {
        get => _navMeshAgent.speed;
        set => _navMeshAgent.speed = value;
    }
    public Vector3 Velocity =>
        _moveByNav ? _navMeshAgent.velocity :
         _moveByDirection ? _moveDirection * MaxMoveSpeed :
        Vector3.zero;
    public event Action OnStop = delegate { };

    private NavMeshAgent _navMeshAgent;
    private bool _destinationReached;
    private Vector3 _moveDirection;

    private bool _moveByNav;
    private bool _moveByDirection;

    public MoveByNavMeshAgent(NavMeshAgent agent)
    {
        _navMeshAgent = agent;
    }

    public void Update(float tick)
    {
        if (!_destinationReached && HasReachedDestination())
        {
            _destinationReached = true;
            Stop();
            OnDestinationReached();
        }

        if (_moveDirection != Vector3.zero)
        {
            _navMeshAgent.Move(_moveDirection * _navMeshAgent.speed * tick);
        }
    }

    public void Stop()
    {
        _moveByNav = false;
        _moveByDirection = false;

        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.isStopped = true;
        _moveDirection = Vector3.zero;
    }

    public void MoveToDestination(Vector3 destination)
    {
        Stop();

        _moveByNav = true;
        _destinationReached = false;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(destination);
    }

    public void MoveToDirection(Vector3 direction)
    {
        Stop();
        
        _moveByDirection = true;
        _moveDirection = direction;
    }

    private bool HasReachedDestination()
    {
        if (!_navMeshAgent.pathPending &&
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            return true;
        }

        return false;
    }

    private void OnDestinationReached()
    {
        OnStop.Invoke();
    }
}

//public class MoveByDirection : ICharacterMovement
//{
//    private Vector3 _direction;

//    private void Update()
//    {
//        bool shouldMove = _desiredDirection != Vector3.zero;
//        if (shouldMove)
//        {
//            _isMoving = true;

//            Vector3 moveOffset = Time.fixedDeltaTime * Velocity;
//            Vector3 targetPosition = transform.position + moveOffset;
//            targetPosition.y = GetGroundHeight(targetPosition);
//            transform.position = targetPosition;

//            OnMove();
//        }
//        else if (_isMoving && shouldMove == false)
//        {
//            _isMoving = false;

//            OnStop();
//        }
//    }
//    public void MoveToDirection(Vector3 direction)
//    {
//        _direction = direction;
//    }

//    public void Stop()
//    {
//        _direction = Vector3.zero;
//    }

//    public void SetMoveDirection(Vector3 direction)
//    {
//        if (direction == Vector3.zero)
//        {
//            _desiredDirection = Vector3.zero;
//            _navMeshAgent.isStopped = true;
//            return;
//        }

//        _navMeshAgent.updatePosition = false;
//        direction.y = 0;
//        direction.Normalize();
//        _desiredDirection = direction * MoveSpeed;
//        //Forward = direction;
//    }

//    private float GetGroundHeight(Vector3 position)
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, Define.GroundLayer))
//        {
//            return hit.point.y;
//        }
//        return position.y;
//    }
//}

public class CharacterMovementComponent : MonoBehaviour
{
    public float MoveSpeed
    {
        get => _movement.MaxMoveSpeed;
        set => _movement.MaxMoveSpeed = value;
    }
    public Vector3 Velocity
    {
        get => _movement.Velocity;
    }
    //public float CurrentMoveSpeed => _navMeshAgent.velocity.magnitude;
    //public Vector3 Velocity => _navMeshAgent.velocity;
    //public Vector3 Forward { get; private set; }

    //[SerializeField] private float _moveSpeed = 2.0f;

    [SerializeField] private NavMeshAgent _navMeshAgent;

    public ICharacterMovement Movement => _movement;
    private ICharacterMovement _movement;

    private Vector3 _desiredDirection;
    private bool _moveByAgent;
    private bool _moveByDirection;

    public event Action OnMove = delegate { };
    public event Action OnStop = delegate { };

    private void Awake()
    {
        SetForward(Vector3.forward);

        //Vector3 targetPosition = transform.position;
        //targetPosition.y = GetGroundHeight(targetPosition);
        //transform.position = targetPosition;

        _movement = new MoveByNavMeshAgent(_navMeshAgent);
    }

    private void Update()
    {
        _movement.Update(Time.deltaTime);
    }

    public void MoveToDestination(Vector3 destination)
    {
        _movement.MoveToDestination(destination);
    }

    public void MoveToDirection(Vector3 direction)
    {
        _movement.MoveToDirection(direction);
    }

    public void SetForward(Vector3 direction)
    {
        //Forward = direction;
        transform.forward = direction;
    }
}
