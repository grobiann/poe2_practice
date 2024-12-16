using UnityEngine;

public class CharacterMeshComponent : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _model;

    public void SetAnimationParameter(int id, float value)
    {
        _anim.SetFloat(id, value, 0.1f, Time.deltaTime);
    }
}
