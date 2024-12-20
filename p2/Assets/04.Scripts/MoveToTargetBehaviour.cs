using UnityEngine;

public class MoveToTargetBehaviour : CharacterBehaviour
{
    private Character _character;
    private GameObject _target;
    private float _stopDistance = 0.5f;

    public MoveToTargetBehaviour(Character character, GameObject target)
    {
        _character = character;
        _target = target;
    }

    protected override bool CalcComplete()
    {
        Vector2 characterPos = _character.transform.position.ToVector2XZ();
        Vector2 targetPos = _target.transform.position.ToVector2XZ();
        return Vector2.Distance(characterPos, targetPos) < _stopDistance;
    }

    protected override void DoBehaviour()
    {
        _character.SetMoveDirection((_target.transform.position - _character.transform.position).normalized);
    }
}
