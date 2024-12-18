using NUnit.Framework.Constraints;
using UnityEngine;

public class Portal : MonoBehaviour, IMouseClickable
{
    [SerializeField] private int mapKey;

    public void OnClick()
    {
        var player = GameManager.Instance.CurrentGameMode.MyPlayerCharacter;
        player.StopAllBehaviours();
        player.RegisterBehaviour(new MoveToTargetBehaviour(player, gameObject));
        player.RegisterBehaviour(new EnterMapBehaviour(mapKey));
    }
}
