public class EnterMapBehaviour : CharacterBehaviour
{
    private int _mapKey;
    private bool _isEntering;

    public EnterMapBehaviour(int mapKey)
    {
        _mapKey = mapKey;
    }

    protected override bool CalcComplete()
    {
        return false;
    }

    protected override void DoBehaviour()
    {
        if (_isEntering == false)
        {
            _isEntering = true;
            GameManager.Instance.EnterMap(_mapKey);
        }
    }
}