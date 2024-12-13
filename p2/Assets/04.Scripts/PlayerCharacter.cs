using UnityEngine;

public class PlayerCharacterModel : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    }

public class PlayerCharacter : MonoBehaviour
{
    public PlayerCharacterModel Model => _model;

    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerCharacterModel _model;
    [SerializeField] private GameObject _renderer;

    void Update()
    {
        ProcessInputs();
        RotateTowardsMouse();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

        if(moveDirection != Vector3.zero)
        {
            _movement.Move(_movement.MoveSpeed * Time.deltaTime * moveDirection.normalized);
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, Define.GroundLayer))
        {
            Vector3 targetDirection = hitInfo.point - transform.position;
            targetDirection.y = 0; // Keep the direction strictly horizontal
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _movement.MoveSpeed);

                _movement.SetForward(targetDirection);
            }
        }
    }
}
