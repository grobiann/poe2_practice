using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;
    private bool _prevMovedByInput;

    void Update()
    {
        ProcessInputs();
        RotateTowardsMouse();
        MoveToMousePosition();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

        if (moveDirection != Vector3.zero)
        {
            _prevMovedByInput = true;
            _playerCharacter.StopAllBehaviours();
            _playerCharacter.SetMoveDirection(moveDirection);
        }
        else if(_prevMovedByInput)
        {
            _prevMovedByInput = false;
            _playerCharacter.Movement.Movement.Stop();
        }
    }

    void MoveToMousePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, Define.GroundLayer))
            {
                _playerCharacter.SetMoveDestination(hitInfo.point);
            }
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, Define.GroundLayer))
        {
            Vector3 targetDirection = hitInfo.point - transform.position;
            _playerCharacter.RotateTowards(targetDirection);
        }
    }
}
