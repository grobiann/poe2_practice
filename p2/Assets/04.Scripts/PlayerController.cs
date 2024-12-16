using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;

    void Update()
    {
        ProcessInputs();
        RotateTowardsMouse();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

        _playerCharacter.MoveByDirection(moveDirection);
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
