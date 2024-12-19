using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public Material material;
    public RenderTexture renderTexture;

    private void Update()
    {
        // Update fog of war
        GameMode currentMode = GameManager.Instance.CurrentGameMode;
        currentMode.Minimap.UpdateFogOfWar(
            currentMode.MyPlayerCharacter.transform.position);

        // Follow player
        //_camera.transform.position =
        //    GameManager.Instance.CurrentGameMode.MyPlayerCharacter.transform.position +
        //    new Vector3(0, 10, 0);
    }
}
