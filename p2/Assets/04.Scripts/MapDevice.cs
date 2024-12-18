using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;

public class MapDevice : MonoBehaviour, IMouseClickable
{
    [SerializeField] private int mapKey;
    public void OnClick()
    {
        var player = GameManager.Instance.CurrentGameMode.MyPlayerCharacter;
        player.StopAllBehaviours();
        player.RegisterBehaviour(new MoveToTargetBehaviour(player, gameObject));
        // TODO: 맵 ui 열기
    }
}
